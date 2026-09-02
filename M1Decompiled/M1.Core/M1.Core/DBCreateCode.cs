using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using M1.Extensions;

namespace M1.Core;

public class DBCreateCode
{
	public delegate void CreateDelegate(string process);

	public string CreateDatabase(M1User m1User, M1DataDictionary m1DataDictionary, AppContext context, Dictionary<string, object> dsProps, int databaseSize, CreateDelegate func)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = string.Empty;
		string empty = string.Empty;
		_ = string.Empty;
		_ = string.Empty;
		string text2 = string.Empty;
		bool flag = false;
		string text3 = context.Server.IniSettings.Get("DataLocation", "C:\\M1Data\\").AddBackslash();
		if (databaseSize <= 0)
		{
			databaseSize = 25;
		}
		string text4 = databaseSize + "MB";
		if (dsProps != null)
		{
			if (dsProps.ContainsKey("dbID"))
			{
				text = (string)dsProps["dbID"];
			}
			if (dsProps.ContainsKey("copySecurityFrom"))
			{
				text2 = (string)dsProps["copySecurityFrom"];
			}
			if (text2 == null)
			{
				text2 = string.Empty;
			}
		}
		text = "M1_" + text.ToUpper();
		ServerManager dBServerManager = context.DBServerManager;
		if (dBServerManager.GetDataTable(null, m1User, "master", 0, "select name from sys.databases where name = '" + text + "'").Rows.Count > 0)
		{
			flag = true;
		}
		if (flag)
		{
			func("Removing Existing Database");
			dBServerManager.ExecuteCommand(null, m1User, "master", "DROP DATABASE " + text);
		}
		func("Creating Database");
		dBServerManager.ExecuteCommand(null, m1User, "master", "CREATE DATABASE " + text + " ON (NAME = " + text + ", FILENAME = '" + text3 + text + ".mdf', SIZE = " + text4 + ")");
		func("Making " + text + " the active database");
		using (SqlConnection sqlConnection = dBServerManager.GetConnection(m1User, text, openImmediately: true))
		{
			dBServerManager.ExecuteCommand(sqlConnection, m1User, text, "USE " + text);
			dBServerManager.ExecuteCommand(sqlConnection, m1User, text, "SET ANSI_PADDING ON");
			dBServerManager.ExecuteCommand(sqlConnection, m1User, text, "ALTER DATABASE " + text + " Set RECOVERY BULK_LOGGED");
			DataTable dataTable = m1DataDictionary.GetDataTable("select dtTable," + m1DataDictionary.Language.GetdtCaptionField(null) + " from DDTables " + m1DataDictionary.Language.GetdtCaptionJoin(null) + " order by dtTable");
			if (dataTable.Rows.Count > 0)
			{
				Dmo dmo = new Dmo(context, dBServerManager);
				foreach (DataRow row in dataTable.Rows)
				{
					empty = row.Field<string>("dtTable");
					func(empty + " - " + row.Field<string>("dtCaption"));
					try
					{
						dmo.CreateTable(sqlConnection, m1User, m1DataDictionary, text, empty);
					}
					catch (Exception ex)
					{
						stringBuilder.AppendLine(ex.Message);
					}
				}
			}
			SqlDataAdapter adapter;
			DataTable dataTable2 = dBServerManager.GetDataTable(sqlConnection, m1User, text, 0, "Select * From DatasetProperties", fillSchema: true, out adapter);
			if (dataTable2.Rows.Count == 0)
			{
				DataRow dataRow = dataTable2.NewRow();
				dataRow.BeginEdit();
				dataRow.BlankRow();
				func("Setting Default Dataset Properties");
				foreach (KeyValuePair<string, object> dsProp in dsProps)
				{
					if (dataTable2.Columns.Contains(dsProp.Key))
					{
						dataRow[dsProp.Key] = dsProp.Value;
					}
				}
				dataRow.SetField("xadVersion", m1DataDictionary.Version);
				dataRow.SetField("xadExtensionVersions", m1DataDictionary.AppExtensions.GetVersionString());
				dataRow.SetField("xadColor", 8421504);
				dataRow.SetField("xadExtendedSearchOptions", value: true);
				dataRow.EndEdit();
				dataTable2.Rows.Add(dataRow);
				dBServerManager.UpdateData(sqlConnection, m1User, text, new DataRow[1] { dataRow }, adapter);
			}
			if (!m1DataDictionary.ProductCode.IsModulePurchased("MW", m1DataDictionary))
			{
				insertNewWarehouseAndBin(dBServerManager, sqlConnection, m1User, text);
			}
		}
		M1Database m1Database = new M1Database(m1User, dBServerManager);
		LoginCredentials loginCredentials = new LoginCredentials(m1User.ID, "notused");
		m1Database.Login(text, m1User, loginCredentials, readOnlyLogin: false);
		try
		{
			DBCreateDefaultParms dBCreateDefaultParms = new DBCreateDefaultParms(dBServerManager, m1User, m1DataDictionary, text, m1Database);
			foreach (AppExtension appExtension in m1DataDictionary.AppExtensions)
			{
				Assembly dDAssembly = appExtension.GetDDAssembly();
				if (!(dDAssembly != null))
				{
					continue;
				}
				Type[] types = dDAssembly.GetTypes();
				foreach (Type type in types)
				{
					object[] customAttributes = type.GetCustomAttributes(typeof(DBCreateDefaultAttribute), inherit: false);
					if (customAttributes != null && customAttributes.Length != 0)
					{
						func(((DBCreateDefaultAttribute)customAttributes[0]).Description);
						Activator.CreateInstance(type, dBCreateDefaultParms);
					}
				}
			}
		}
		finally
		{
			m1Database.Logout();
		}
		if (text2 != string.Empty)
		{
			func("Copying security from " + text2);
			m1DataDictionary.Users.CopySecurityToDatabase(text2, text);
		}
		func(string.Empty);
		return stringBuilder.ToString();
	}

	private void insertNewWarehouseAndBin(ServerManager serverManager, SqlConnection connection, M1User m1User, string dbName)
	{
		using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO Warehouses (imwWarehouseID,imwName,imwDefaultWarehouse,imwDefaultBinCount,imwCreatedBy,imwCreatedDate) VALUES (@WarehouseID,@Name,@DefaultWarehouse,@DefaultBinCount,@CreatedBy,@CreatedDate)".ToString()))
		{
			sqlCommand.Parameters.Add(new SqlParameter("WarehouseID", "MAIN"));
			sqlCommand.Parameters.Add(new SqlParameter("Name", "Main Warehouse"));
			sqlCommand.Parameters.Add(new SqlParameter("DefaultWarehouse", true));
			sqlCommand.Parameters.Add(new SqlParameter("DefaultBinCount", 1));
			sqlCommand.Parameters.Add(new SqlParameter("CreatedBy", "CONVERSION"));
			sqlCommand.Parameters.Add(new SqlParameter("CreatedDate", DateTime.Now));
			serverManager.ExecuteCommand(connection, m1User, dbName, sqlCommand, null);
		}
		using SqlCommand sqlCommand2 = new SqlCommand("INSERT INTO WarehouseBins (inbWarehouseID,inbWarehouseBinID,inbDescription,inbDefaultBin,inbCreatedBy,inbCreatedDate) VALUES (@WarehouseID,@WarehouseBinID,@Description,@DefaultBin,@CreatedBy,@CreatedDate)".ToString());
		sqlCommand2.Parameters.Add(new SqlParameter("WarehouseID", "MAIN"));
		sqlCommand2.Parameters.Add(new SqlParameter("WarehouseBinID", "DEFAULT"));
		sqlCommand2.Parameters.Add(new SqlParameter("Description", "Default Bin"));
		sqlCommand2.Parameters.Add(new SqlParameter("DefaultBin", true));
		sqlCommand2.Parameters.Add(new SqlParameter("CreatedBy", "CONVERSION"));
		sqlCommand2.Parameters.Add(new SqlParameter("CreatedDate", DateTime.Now));
		serverManager.ExecuteCommand(connection, m1User, dbName, sqlCommand2, null);
	}
}
