using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Extensions;

namespace M1.Core;

public class M1DataDictionary : M1Database
{
	public M1UserCollection Users;

	public Language Language;

	public M1ProductCode ProductCode;

	public AppExtensionCollection AppExtensions;

	public ModuleCollection Modules;

	private string _Region;

	public string Version { get; private set; }

	public bool Hosted { get; private set; }

	public new string Region
	{
		get
		{
			if (_Region == null)
			{
				return currentContext.GetWindowsRegion();
			}
			return _Region;
		}
		private set
		{
			_Region = value;
		}
	}

	public new M1User User => null;

	public new event EventHandler<LoggingOutEventArgs> LoggingOut;

	public event EventHandler<DataDictionaryChangedEventArgs> DataDictionaryChanged;

	public M1DataDictionary(IServiceProvider parentProvider, ServerManager m1ServerManager)
		: base(parentProvider, m1ServerManager)
	{
		serviceContainer.AddService(typeof(M1DataDictionary), this);
		Users = new M1UserCollection(this, currentContext);
		Language = new Language(this, currentContext);
		ProductCode = new M1ProductCode(this, currentContext);
		AppExtensions = new AppExtensionCollection(this);
		Modules = new ModuleCollection();
	}

	public new void OnLoggingOut(LoggingOutEventArgs e)
	{
		this.LoggingOut?.Invoke(this, e);
	}

	public void OnDataDictionaryChanged(DataDictionaryChangedEventArgs e)
	{
		this.DataDictionaryChanged?.Invoke(this, e);
	}

	public void ClearSecurityCache()
	{
		ClearSecurityCache(string.Empty);
	}

	public void ClearSecurityCache(string userID)
	{
		foreach (M1User user in Users)
		{
			if (userID.Length != 0 && !user.ID.Equals(userID, StringComparison.CurrentCultureIgnoreCase))
			{
				continue;
			}
			foreach (M1Database database in user.Databases)
			{
				database.Security.ClearCache();
			}
		}
	}

	public bool LogoutDD()
	{
		if (Users.LogoutAndRemove(null))
		{
			LoggingOutEventArgs e = new LoggingOutEventArgs();
			OnLoggingOut(e);
			if (e.Cancel)
			{
				return false;
			}
		}
		return true;
	}

	public void DeleteSecurityForDatabase(string database)
	{
		database = database.Trim().ToUpper();
		if (database.Length != 0)
		{
			SqlCommand sqlCommand = NewSqlCommand("DELETE FROM DDSecurityTables WHERE dtDataset = @Dataset");
			sqlCommand.Parameters.Add(new SqlParameter("@Dataset", SqlDbType.NVarChar)).Value = database;
			ExecuteCommand(sqlCommand);
			sqlCommand = NewSqlCommand("DELETE FROM DDSecurityReports WHERE drDataset = @Dataset");
			sqlCommand.Parameters.Add(new SqlParameter("@Dataset", SqlDbType.NVarChar)).Value = database;
			ExecuteCommand(sqlCommand);
			sqlCommand = NewSqlCommand("DELETE FROM DDSecurityGroups WHERE dzDataset = @Dataset");
			sqlCommand.Parameters.Add(new SqlParameter("@Dataset", SqlDbType.NVarChar)).Value = database;
			ExecuteCommand(sqlCommand);
		}
	}

	public ArrayList GetAdditionalFieldsForTable(string TableName)
	{
		DataTable dataTable = new DataTable("DDTables");
		ArrayList arrayList = new ArrayList();
		SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
		using SqlCommand sqlCommand = NewSqlCommand("SELECT dtaddfld1, dtaddfld2, dtaddfld3, dtuaddfld1, dtuaddfld2, dtuaddfld3 FROM DDTables WHERE dtTable = @TableName");
		sqlCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)).Value = TableName.Trim().ToUpper();
		using (sqlDataAdapter = new SqlDataAdapter(sqlCommand))
		{
			sqlDataAdapter.Fill(dataTable);
			if (dataTable.Rows.Count != 0)
			{
				string[] array = dataTable.Rows[0][0].ToString().Trim().Split(',');
				foreach (string text in array)
				{
					arrayList.Add(new string[2]
					{
						text,
						dataTable.Rows[0][0].ToString().Trim()
					});
				}
				return arrayList;
			}
			return null;
		}
	}

	public void LoginDD(string ddName, bool languageOnly)
	{
		if (ddName.Length == 0 || !currentContext.DDServerManager.DoesDatabaseExist(null, null, ddName))
		{
			throw new M1LoginDataDictionaryDoesNotExistException("Data dictionary " + ddName + " does not exist.");
		}
		string text = string.Empty;
		string ddLanguageField = string.Empty;
		string text2 = string.Empty;
		_ = string.Empty;
		bool flag = false;
		Version = string.Empty;
		Hosted = false;
		base.ID = ddName;
		DataTable dataTable = GetDataTable("select * from DDInfo");
		if (dataTable.Rows.Count != 0)
		{
			DataRow row = dataTable.Rows[0];
			foreach (DataColumn column in dataTable.Columns)
			{
				switch (column.ColumnName.ToUpper())
				{
				case "DDREGION":
					text = row.Field<string>(column).Trim();
					break;
				case "DDLANGUAGE":
					ddLanguageField = row.Field<string>(column).Trim();
					break;
				case "DDVERSION":
				case "DDVERS":
					Version = row.Field<string>(column).Trim();
					break;
				case "DDPRODUCTCODE":
					text2 = row.Field<string>(column).Trim();
					break;
				case "DDCUSTOMPRODUCTCODES":
					if (row.Field<string>(column) != null)
					{
						row.Field<string>(column).Trim();
					}
					break;
				case "DDHOSTED":
					Hosted = row.Field<bool>(column);
					flag = true;
					break;
				}
			}
		}
		if (text.Length == 0)
		{
			text = currentContext.GetWindowsRegion();
		}
		Region = text.Trim().ToUpper();
		if (currentContext.IsHosted != Hosted && flag)
		{
			ExecuteCommand("Update DDInfo Set DDHosted = " + currentContext.IsHosted.ToSql());
		}
		Language.CheckForLanguage(ddLanguageField);
		if (languageOnly)
		{
			return;
		}
		if (Version.CompareTo(currentContext.Version) != 0)
		{
			object[] args = new string[3] { ddName, Version, currentContext.Version };
			throw new M1LoginInvalidVersionException(string.Format("The data dictionary {0} is at version {1}, which is different than this installation of M1 ({2}).", args));
		}
		if (text2.Length == 0)
		{
			throw new M1LoginProductCodeInvalidException("Data dictionary " + ddName + " does not have a valid product code. Please enter your product code now.");
		}
		if (ProductCode.LastLoadedProductCode != text2)
		{
			ProductCode.LoadProductCode(text2);
			if (ProductCode.IsProductCodeExpired())
			{
				throw new M1LoginProductCodeExpiredException("This copy of M1 has expired. Please contact ECi Solutions for an updated product ID for this software.");
			}
			ProductCode.LoadCustomProductIDFromIni();
		}
		Modules.Refresh(this);
		AppExtensions.Refresh();
		foreach (AppExtension appExtension in AppExtensions)
		{
			if (appExtension.DDAssemblyVersion.Length != 0 && !appExtension.LastUpdatedDDVersion.Equals(appExtension.DDAssemblyVersion))
			{
				object[] args = new string[3] { appExtension.Caption, appExtension.DDAssemblyVersion, appExtension.LastUpdatedDDVersion };
				throw new M1LoginInvalidVersionException(string.Format("The app extension {0} is at version {1}, which is different than the last loaded image of the extension ({2}).", args));
			}
		}
	}

	public void SetRegion(string newRegion)
	{
		new DmoDD(currentContext).SetRegionOnDD(base.ID, newRegion);
		Region = string.Empty;
		LoginDD(base.ID, languageOnly: true);
	}

	public static string GetDisplayName(string nameUpper, string nameLower)
	{
		if (nameUpper.Trim().ToUpper() != nameLower.Trim().ToUpper())
		{
			return nameUpper.Trim().ToUpper();
		}
		return nameLower.Trim();
	}

	public static string GetTableList(string select)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string text5 = "";
		int num = 0;
		for (num = select.ToUpper().IndexOf(" FROM "); num >= 0; num = select.ToUpper().IndexOf(" FROM "))
		{
			text4 = select.Substring(num + 6).ToUpper();
			text5 = select.Substring(0, num);
			select = text4;
			string[] array = new string[4] { " WHERE ", " ORDER ", " GROUP ", " HAVING " };
			foreach (string value in array)
			{
				num = text4.IndexOf(value);
				if (num > 0)
				{
					text4 = text4.Substring(0, num - 1);
				}
			}
			array = text4.Split(new string[1] { " JOIN " }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				text3 = array[i].Trim();
				if (text3.Length == 0)
				{
					continue;
				}
				num = text3.LastIndexOf(".");
				if (num > 0)
				{
					text3 = text3.Substring(num + 1);
				}
				for (num = 0; num < text3.Length; num++)
				{
					if (text3[num] == ' ' || text3[num] == ',')
					{
						text3 = text3.Substring(0, num);
						break;
					}
				}
				if (text5.IndexOf("(SELECT ") > 0)
				{
					text2 = text2 + "," + text3;
				}
				else if (("," + text + ",").IndexOf("," + text3 + ",") == -1)
				{
					text = text + "," + text3;
				}
			}
		}
		text += text2;
		if (text.StartsWith(","))
		{
			text = text.Substring(1);
		}
		return text;
	}

	public static string RemoveDuplicateFields(string fields)
	{
		List<string> list = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string item in M1Util.ParseFieldList(fields, ','))
		{
			if (!list.Contains(item.Trim(), StringComparer.CurrentCultureIgnoreCase))
			{
				list.Add(item.Trim());
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append(item);
			}
		}
		return stringBuilder.ToString();
	}

	public MatchingFieldsInfo FindMatchingFields(string sSourceTables, string sDestTable, string[] sourceFields, string[] destFields)
	{
		MatchingFieldsInfo matchingFieldsInfo = FindMatchingFields(sSourceTables, sDestTable);
		for (int i = 0; i < sourceFields.Length; i++)
		{
			matchingFieldsInfo.Fields.Add(sourceFields[i], destFields[i]);
		}
		return matchingFieldsInfo;
	}

	public MatchingFieldsInfo FindMatchingFields(string sSourceTables, string sDestTable)
	{
		int num = 0;
		int num2 = 0;
		string text = string.Empty;
		string text2 = string.Empty;
		sDestTable = sDestTable.Trim();
		MatchingFieldsInfo matchingFieldsInfo = new MatchingFieldsInfo(sSourceTables, sDestTable);
		string[] array = sSourceTables.Split(',');
		foreach (string value in array)
		{
			if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(sDestTable))
			{
				using (SqlCommand sqlCommand = NewSqlCommand("select dtPrefixUser from DDTables where dtTable = @TableName"))
				{
					sqlCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)).Value = value;
					text = (string)ExecuteScalar(sqlCommand);
					sqlCommand.Parameters["@TableName"].Value = sDestTable;
					text2 = (string)ExecuteScalar(sqlCommand);
				}
				if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
				{
					continue;
				}
				num = text.Trim().Length;
				num2 = text2.Trim().Length;
				DataTable dataTable;
				DataTable dataTable2;
				using (SqlCommand sqlCommand2 = NewSqlCommand("select dfField,dfdbtype,dfLength from DDFields where dfTable = @TableName and LEFT(dfField,@FieldLength) = @FieldPrefix"))
				{
					sqlCommand2.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)).Value = value;
					sqlCommand2.Parameters.Add(new SqlParameter("@FieldLength", SqlDbType.Int)).Value = num;
					sqlCommand2.Parameters.Add(new SqlParameter("@FieldPrefix", SqlDbType.NVarChar)).Value = text;
					dataTable = GetDataTable(sqlCommand2);
					sqlCommand2.Parameters["@TableName"].Value = sDestTable;
					sqlCommand2.Parameters["@FieldLength"].Value = num2;
					sqlCommand2.Parameters["@FieldPrefix"].Value = text2;
					dataTable2 = GetDataTable(sqlCommand2);
				}
				if (dataTable.Rows.Count <= 0 || dataTable2.Rows.Count <= 0)
				{
					continue;
				}
				foreach (DataRow row3 in dataTable.Rows)
				{
					foreach (DataRow row4 in dataTable2.Rows)
					{
						string text3 = text2 + row3.Field<string>("dfField").Trim().Substring(num);
						if (string.Equals(row4.Field<string>("dfField").Trim(), text3, StringComparison.CurrentCultureIgnoreCase) && string.Equals(row3.Field<string>("dfdbtype").Trim(), row4.Field<string>("dfdbtype").Trim(), StringComparison.CurrentCultureIgnoreCase) && row3.Field<byte>("dfLength").Equals(row4.Field<byte>("dfLength")))
						{
							matchingFieldsInfo.Fields.Add(row3.Field<string>("dfField").Trim(), text3);
							break;
						}
					}
				}
				continue;
			}
			throw new Exception("Both the source and destination table must be specified.");
		}
		return matchingFieldsInfo;
	}
}
