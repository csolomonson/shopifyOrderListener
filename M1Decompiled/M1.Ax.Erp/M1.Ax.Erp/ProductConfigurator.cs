using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using ADODB;
using M1.Core;
using M1.Core.Script;
using M1.Extensions;
using M1.Script.Interfaces;

namespace M1.Ax.Erp;

[ComVisible(true)]
public class ProductConfigurator : ScriptingBase, IProductConfigurator
{
	[Guid("A4C46780-499F-101B-BB78-00AA00383CBB")]
	[ComVisible(true)]
	public interface IPCParametersComCollection
	{
		[IndexerName("_Default")]
		[DispId(0)]
		PCParameter this[string name] { get; }

		void Add(PCParameter value);

		void Clear();

		bool Contains(object value);

		int IndexOf(PCParameter value);

		void Insert(int index, PCParameter value);

		bool Remove(PCParameter value);

		void RemoveAt(int index);

		[DispId(-4)]
		IEnumerator<PCParameter> GetEnumerator();
	}

	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(IPCParametersComCollection))]
	public class PCParameterList : KeyedCollection<string, PCParameter>, IPCParametersComCollection
	{
		public PCParameterList()
			: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
		{
		}

		protected override string GetKeyForItem(PCParameter item)
		{
			return item.Key;
		}

		public bool Contains(object value)
		{
			if (value is PCParameter)
			{
				return base.Contains((PCParameter)value);
			}
			return base.Contains(value.ToString());
		}

		PCParameter IPCParametersComCollection.get__Default(string name)
		{
			return base[name];
		}
	}

	[ComVisible(true)]
	public class PCParameter
	{
		public string Key = string.Empty;

		public object Value = string.Empty;

		public PCParameter(string key, object value)
		{
			Key = key;
			Value = value;
		}
	}

	[ComVisible(true)]
	public interface IPCProxy
	{
		string PCParentPartID { get; set; }

		string PCParentPartRevisionID { get; set; }

		string PCTopLevelPartID { get; set; }

		string PCTopLevelPartRevisionID { get; set; }

		string PCTable { get; set; }

		Guid PCUniqueID { get; set; }

		M1Database Database { get; set; }

		object GetPartConfigurationValue(string partID, string partRevisionID, string parameterName);
	}

	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(IPCProxy))]
	public class PCProxy : IPCProxy
	{
		private string _PCParentPartID = string.Empty;

		private string _PCParentPartRevisionID = string.Empty;

		private string _PCTopLevelPartID = string.Empty;

		private string _PCTopLevelPartRevisionID = string.Empty;

		private string _PCTable = string.Empty;

		public string PCParentPartID
		{
			get
			{
				return _PCParentPartID;
			}
			set
			{
				_PCParentPartID = value;
			}
		}

		public string PCParentPartRevisionID
		{
			get
			{
				return _PCParentPartRevisionID;
			}
			set
			{
				_PCParentPartRevisionID = value;
			}
		}

		public string PCTopLevelPartID
		{
			get
			{
				return _PCTopLevelPartID;
			}
			set
			{
				_PCTopLevelPartID = value;
			}
		}

		public string PCTopLevelPartRevisionID
		{
			get
			{
				return _PCTopLevelPartRevisionID;
			}
			set
			{
				_PCTopLevelPartRevisionID = value;
			}
		}

		public string PCTable
		{
			get
			{
				return _PCTable;
			}
			set
			{
				_PCTable = value;
			}
		}

		public Guid PCUniqueID { get; set; }

		public M1Database Database { get; set; }

		public void SetPartConfigurationParameters(string cParentPartID, string cParentPartRevisionID, string cTopLevelPartID, string cTopLevelPartRevisionID, string cTable, Guid uniqueID, M1Database database)
		{
			PCParentPartID = cParentPartID;
			PCParentPartRevisionID = cParentPartRevisionID;
			PCTopLevelPartID = cTopLevelPartID;
			PCTopLevelPartRevisionID = cTopLevelPartRevisionID;
			PCTable = cTable;
			PCUniqueID = uniqueID;
			Database = database;
		}

		public object GetPartConfigurationValue(string partID, string partRevisionID, string parameterName)
		{
			if (!string.IsNullOrWhiteSpace(partID))
			{
				return GetPartConfigurationValueEx(Database, partID, partRevisionID, PCParentPartID, PCParentPartRevisionID, PCTopLevelPartID, PCTopLevelPartRevisionID, PCTable, PCUniqueID, parameterName);
			}
			return string.Empty;
		}
	}

	public PCParameterList PCParameters = new PCParameterList();

	private M1Database _Database;

	private PCProxy _Proxy = new PCProxy();

	private M1AdoRecordsetProxy proxy = new M1AdoRecordsetProxy();

	private DataTable rulesTable;

	public ProductConfigurator(IServiceProvider provider)
		: base(provider)
	{
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public override void Dispose()
	{
		rulesTable = null;
		_Database = null;
		if (PCParameters != null)
		{
			PCParameters.Clear();
			PCParameters = null;
		}
		base.Dispose();
	}

	public static string GenerateFormIDForPart(string partID, string revisionID)
	{
		return "PART-" + partID.Trim().ToUpper() + "-REV-" + revisionID.Trim().ToUpper();
	}

	public static string ConvertToPropertyString(object value)
	{
		if (value == null)
		{
			return string.Empty;
		}
		if (value.GetType() == typeof(string))
		{
			string text = (string)value;
			if (text.IndexOf('"') == -1)
			{
				return "\"" + text + "\"";
			}
			if (text.IndexOf('\'') == -1)
			{
				return "'" + text + "'";
			}
			return "[" + text + "]";
		}
		if (value.GetType() == typeof(DateTime))
		{
			return "#" + value.ToString() + "#";
		}
		return value.ToString();
	}

	public static object ReadPropertyString(string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			if (value.StartsWith("'") || value.StartsWith("\"") || value.StartsWith("["))
			{
				value = value.Substring(1);
				if (value.EndsWith("'") || value.EndsWith("\"") || value.EndsWith("]"))
				{
					value = value.Substring(0, value.Length - 1);
				}
				return value;
			}
			if (value.Equals("True", StringComparison.CurrentCultureIgnoreCase))
			{
				return true;
			}
			if (value.Equals("False", StringComparison.CurrentCultureIgnoreCase))
			{
				return false;
			}
			if (value.StartsWith("#"))
			{
				value = value.Substring(1);
				if (value.EndsWith("#"))
				{
					value = value.Substring(0, value.Length - 1);
				}
				if (DateTime.TryParse(value, out var result))
				{
					return result;
				}
				return value;
			}
			if (decimal.TryParse(value, out var result2))
			{
				return result2;
			}
			return value;
		}
		return string.Empty;
	}

	public static object GetPartConfigurationValueEx(M1Database database, string cPartID, string cPartRevisionID, string cParentPartID, string cParentPartRevisionID, string cTopLevelPartID, string cTopLevelPartRevisionID, string cTable, Guid uniqueID, string cParameterName)
	{
		if (!string.IsNullOrWhiteSpace(cPartID))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select xaiValue From FormInputValues Where xaiSourceUniqueID = @SourceUniqueID And xaiSourceTable = @SourceTable And xaiFormID = @FormID And xaiTopLevelFormID = @TopLevelFormID And xaiControlName = @ControlName");
			sqlCommand.Parameters.Add(new SqlParameter("@FormID", SqlDbType.NVarChar)).Value = GenerateFormIDForPart(cPartID, cPartRevisionID);
			if (!string.IsNullOrWhiteSpace(cTopLevelPartID))
			{
				sqlCommand.Parameters.Add(new SqlParameter("@TopLevelFormID", SqlDbType.NVarChar)).Value = GenerateFormIDForPart(cTopLevelPartID, cTopLevelPartRevisionID);
			}
			else
			{
				sqlCommand.Parameters.Add(new SqlParameter("@TopLevelFormID", SqlDbType.NVarChar)).Value = string.Empty;
			}
			sqlCommand.Parameters.Add(new SqlParameter("@SourceUniqueID", SqlDbType.UniqueIdentifier)).Value = uniqueID;
			sqlCommand.Parameters.Add(new SqlParameter("@SourceTable", SqlDbType.NVarChar)).Value = cTable;
			sqlCommand.Parameters.Add(new SqlParameter("@ControlName", SqlDbType.NVarChar)).Value = cParameterName;
			string text = (string)database.ExecuteScalar(sqlCommand);
			if (text != null)
			{
				return ReadPropertyString(text);
			}
		}
		return string.Empty;
	}

	protected void SetParameterValues(object parameters, string customerID)
	{
		PCParameters.Clear();
		if (parameters != null && parameters is object[])
		{
			object[] array = (object[])parameters;
			int i = array.GetLowerBound(0);
			for (int upperBound = array.GetUpperBound(0); i < upperBound; i += 2)
			{
				string text = array[i].ToString().Trim();
				if (text.Length != 0)
				{
					PCParameters.Add(new PCParameter(text, array[i + 1]));
				}
			}
		}
		if (PCParameters.Contains("CustomerID"))
		{
			PCParameters["CustomerID"].Value = customerID;
		}
		else
		{
			PCParameters.Add(new PCParameter("CustomerID", customerID));
		}
		initScript();
	}

	private void initScript()
	{
		LoadEnvironment();
		AddObject("PCProxy", _Proxy);
		AddCode("Function GetPartConfigurationValue(cPartID, cPartRevisionID, cParameterName)\r\nGetPartConfigurationValue = PCProxy.GetPartConfigurationValue(cPartID, cPartRevisionID, cParameterName)\r\nEnd Function\r\n");
		AddObject("Parameters", PCParameters);
		AddCode("M1TempVar = 0\r\nSub SetTempVar(val)\r\nM1TempVar=val\r\nEnd Sub\r\n");
		AddObject("Forms", _Provider.GetService(typeof(IForms)));
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Length = 0;
		stringBuilder.Append("Dim Formula\r");
		foreach (PCParameter pCParameter in PCParameters)
		{
			stringBuilder.Append("Dim " + pCParameter.Key + "\r");
		}
		AddCode(stringBuilder.ToString());
		foreach (PCParameter pCParameter2 in PCParameters)
		{
			if (pCParameter2.Key.Length != 0)
			{
				Run("SetTempVar", new object[1] { pCParameter2.Value });
				base.ExecuteStatement(pCParameter2.Key + "=M1TempVar");
			}
		}
	}

	protected void PreProcessRecord(object data)
	{
		initScript();
		if (data is Recordset)
		{
			AddObject("Fields", ((Recordset)data).Fields);
		}
		else if (data is DataRow)
		{
			AddObject("Fields", new M1AdoRecordsetProxy(new DataRow[1] { (DataRow)data }).FieldsCollection);
		}
	}

	protected void PostProcessRecord()
	{
		ResetEnvironment();
	}

	private bool isFieldInData(object data, string field)
	{
		if (data is Recordset)
		{
			foreach (Field field2 in ((Recordset)data).Fields)
			{
				if (field2.Name.Equals(field, StringComparison.CurrentCultureIgnoreCase))
				{
					return true;
				}
			}
		}
		else
		{
			if (data is M1AdoRecordsetProxy)
			{
				return ((M1AdoRecordsetProxy)data).GetDataTable().Columns.Contains(field);
			}
			if (data is DataTable)
			{
				return ((DataTable)data).Columns.Contains(field);
			}
			if (data is DataRow)
			{
				return ((DataRow)data).Table.Columns.Contains(field);
			}
		}
		return false;
	}

	protected void ProcessRuleForField(object data, string field, string code)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(code))
			{
				return;
			}
			field = field.Trim();
			base.ExecuteStatement("Formula = Null");
			base.ExecuteStatement(code);
			if (field.Length == 0)
			{
				return;
			}
			object obj = base.Eval("Formula");
			if (obj == null || obj == DBNull.Value)
			{
				return;
			}
			if (data is Recordset)
			{
				Recordset recordset = (Recordset)data;
				if (field.Equals("IMHUNITSALEPRICE", StringComparison.CurrentCultureIgnoreCase))
				{
					if (isFieldInData(data, field))
					{
						recordset.Fields[field].Value = obj;
					}
					return;
				}
				if (recordset.Fields[field].Type == DataTypeEnum.adChar || recordset.Fields[field].Type == DataTypeEnum.adWChar || recordset.Fields[field].Type == DataTypeEnum.adVarChar || recordset.Fields[field].Type == DataTypeEnum.adVarWChar)
				{
					string text = Convert.ToString(obj);
					if (text.Length > recordset.Fields[field].DefinedSize)
					{
						obj = text.Substring(0, recordset.Fields[field].DefinedSize);
					}
				}
				recordset.Fields[field].Value = obj;
			}
			else
			{
				if (!(data is DataRow))
				{
					return;
				}
				DataRow dataRow = (DataRow)data;
				if (field.Equals("IMHUNITSALEPRICE", StringComparison.CurrentCultureIgnoreCase))
				{
					if (isFieldInData(data, field))
					{
						dataRow[field] = obj;
					}
					return;
				}
				if (dataRow.Table.Columns[field].DataType == typeof(string))
				{
					string text2 = Convert.ToString(obj);
					if (text2.Length > dataRow.Table.Columns[field].MaxLength && dataRow.Table.Columns[field].MaxLength > 0)
					{
						obj = text2.Substring(0, dataRow.Table.Columns[field].MaxLength);
					}
				}
				dataRow[field] = obj;
			}
		}
		catch (Exception ex)
		{
			throw new M1Exception("The following error occurred while processing the rule for field " + field + ":\r" + ex.Message);
		}
	}

	private string getFilterForTable(object data, string table)
	{
		if (data is DataRow)
		{
			DataRow dataRow = (DataRow)data;
			if (table.Equals("PARTREVISIONS", StringComparison.CurrentCultureIgnoreCase))
			{
				return "pcrMethodID = " + M1Util.ConvertToLinq(dataRow.Field<string>("imrPartID")) + " and pcrMethodRevisionID = " + M1Util.ConvertToLinq(dataRow.Field<string>("imrPartRevisionID")) + " And pcrMethodAssemblyID = 0 And pcrMethodType = 1";
			}
			if (table.Equals("PARTASSEMBLIES", StringComparison.CurrentCultureIgnoreCase))
			{
				return "pcrMethodID = " + M1Util.ConvertToLinq(dataRow.Field<string>("imaMethodID")) + " and pcrMethodRevisionID = " + M1Util.ConvertToLinq(dataRow.Field<string>("imaMethodRevisionID")) + " And pcrMethodAssemblyID = " + M1Util.ConvertToLinq(Convert.ToInt32(dataRow["imaMethodAssemblyID"])) + " And pcrMethodType = 2";
			}
			if (table.Equals("PARTMATERIALS", StringComparison.CurrentCultureIgnoreCase))
			{
				return "pcrMethodID = " + M1Util.ConvertToLinq(dataRow.Field<string>("immMethodID")) + " and pcrMethodRevisionID = " + M1Util.ConvertToLinq(dataRow.Field<string>("immMethodRevisionID")) + " And pcrMethodAssemblyID = " + M1Util.ConvertToLinq(Convert.ToInt32(dataRow["immMethodAssemblyID"])) + " And pcrMethodMaterialID = " + M1Util.ConvertToLinq(Convert.ToInt32(dataRow["immMethodMaterialID"])) + " And pcrMethodType = 3";
			}
			if (table.Equals("PARTOPERATIONS", StringComparison.CurrentCultureIgnoreCase))
			{
				return "pcrMethodID = " + M1Util.ConvertToLinq(dataRow.Field<string>("imoMethodID")) + " and pcrMethodRevisionID = " + M1Util.ConvertToLinq(dataRow.Field<string>("imoMethodRevisionID")) + " And pcrMethodAssemblyID = " + M1Util.ConvertToLinq(Convert.ToInt32(dataRow["imoMethodAssemblyID"])) + " And pcrMethodOperationID = " + M1Util.ConvertToLinq(Convert.ToInt32(dataRow["imoMethodOperationID"])) + " And pcrMethodType = 4";
			}
			throw new M1Exception("Invalid table " + table + " specified in GetFilterForTable.");
		}
		if (data is Recordset)
		{
			Recordset recordset = (Recordset)data;
			if (table.Equals("PARTREVISIONS", StringComparison.CurrentCultureIgnoreCase))
			{
				return "pcrMethodID = " + M1Util.ConvertToLinq(recordset.Fields["imrPartID"].Value) + " and pcrMethodRevisionID = " + M1Util.ConvertToLinq(recordset.Fields["imrPartRevisionID"].Value) + " And pcrMethodAssemblyID = 0 And pcrMethodType = 1";
			}
			if (table.Equals("PARTASSEMBLIES", StringComparison.CurrentCultureIgnoreCase))
			{
				return "pcrMethodID = " + M1Util.ConvertToLinq(recordset.Fields["imaMethodID"].Value) + " and pcrMethodRevisionID = " + M1Util.ConvertToLinq(recordset.Fields["imaMethodRevisionID"].Value) + " And pcrMethodAssemblyID = " + M1Util.ConvertToLinq(recordset.Fields["imaMethodAssemblyID"].Value) + " And pcrMethodType = 2";
			}
			if (table.Equals("PARTMATERIALS", StringComparison.CurrentCultureIgnoreCase))
			{
				return "pcrMethodID = " + M1Util.ConvertToLinq(recordset.Fields["immMethodID"].Value) + " and pcrMethodRevisionID = " + M1Util.ConvertToLinq(recordset.Fields["immMethodRevisionID"].Value) + " And pcrMethodAssemblyID = " + M1Util.ConvertToLinq(recordset.Fields["immMethodAssemblyID"].Value) + " And pcrMethodMaterialID = " + M1Util.ConvertToLinq(recordset.Fields["immMethodMaterialID"].Value) + " And pcrMethodType = 3";
			}
			if (table.Equals("PARTOPERATIONS", StringComparison.CurrentCultureIgnoreCase))
			{
				return "pcrMethodID = " + M1Util.ConvertToLinq(recordset.Fields["imoMethodID"].Value) + " and pcrMethodRevisionID = " + M1Util.ConvertToLinq(recordset.Fields["imoMethodRevisionID"].Value) + " And pcrMethodAssemblyID = " + M1Util.ConvertToLinq(recordset.Fields["imoMethodAssemblyID"].Value) + " And pcrMethodOperationID = " + M1Util.ConvertToLinq(recordset.Fields["imoMethodOperationID"].Value) + " And pcrMethodType = 4";
			}
			throw new M1Exception("Invalid table " + table + " specified in GetFilterForTable.");
		}
		throw new M1Exception("Invalid type " + data.GetType().Name + " specified in GetFilterForTable.");
	}

	public string TestCode(string code, DataTable fieldsTable)
	{
		try
		{
			if (code != null && code.Length != 0)
			{
				initScript();
				if (fieldsTable != null)
				{
					proxy.LoadDataTable(fieldsTable);
					AddObject("Fields", proxy.FieldsCollection);
				}
				base.ExecuteStatement(code);
			}
			return string.Empty;
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
	}

	public void InitializeRulesForProcessing(string partID, string revisionID, object parameters, string customerID)
	{
		SqlCommand sqlCommand = _Database.NewSqlCommand("Select * From PartRules Where pcrMethodID = @MethodID And pcrMethodRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@MethodID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		rulesTable = _Database.GetDataTable(sqlCommand);
		SetParameterValues(parameters, customerID);
	}

	public bool DoesFieldHaveRule(object data, string table, string field)
	{
		if (!string.IsNullOrWhiteSpace(table) && !string.IsNullOrWhiteSpace(field))
		{
			string filterExpression = getFilterForTable(data, table) + " And pcrField = " + M1Util.ConvertToLinq(field);
			DataRow[] array = rulesTable.Select(filterExpression);
			if (array.Length != 0)
			{
				return !string.IsNullOrWhiteSpace(array[0].Field<string>("pcrCode"));
			}
			return false;
		}
		return false;
	}

	public void ProcessRulesForTable(object data, string table)
	{
		string filterForTable = getFilterForTable(data, table);
		DataRow[] array = rulesTable.Select(filterForTable);
		if (array.Length != 0)
		{
			PreProcessRecord(data);
			DataRow[] array2 = array;
			foreach (DataRow row in array2)
			{
				ProcessRuleForField(data, row.Field<string>("pcrField").Trim(), row.Field<string>("pcrCode"));
			}
			PostProcessRecord();
		}
	}

	public decimal? ProcessUnitSalePrice(string partID, string revisionID, object[] parameters, string customerID, string table, Guid uniqueID)
	{
		SqlCommand sqlCommand = _Database.NewSqlCommand("select PartRevisions.*,imrStandardMaterialCost As imhUnitSalePrice from PartRevisions Where imrPartID = @PartID And imrPartRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		DataTable dataTable = _Database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			DataRow dataRow = dataTable.Rows[0];
			new Part().RefreshUnitSalePriceInRow(_Database, dataRow);
			try
			{
				InitializeRulesForProcessing(partID, revisionID, parameters, customerID);
				if (DoesFieldHaveRule(dataRow, "PartRevisions", "imhUnitSalePrice"))
				{
					ProcessRulesForTable(dataRow, "PartRevisions");
					if (dataRow.Field<decimal>("imhUnitSalePrice") != 0m)
					{
						return dataRow.Field<decimal>("imhUnitSalePrice");
					}
				}
			}
			finally
			{
				Cleanup();
			}
		}
		return null;
	}

	public Dictionary<string, object> LoadPartConfigurationValues(M1Database database, string cPartID, string cPartRevisionID, string cParentPartID, string cParentPartRevisionID, string cTopLevelPartID, string cTopLevelPartRevisionID, string cTable, Guid uniqueID)
	{
		Dictionary<string, object> dictionary = null;
		if (!string.IsNullOrWhiteSpace(cPartID))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select xaiControlName,xaiValue From FormInputValues Where xaiSourceUniqueID = @SourceUniqueID And xaiSourceTable = @SourceTable And xaiFormID = @FormID And xaiParentFormID = @ParentFormID And xaiTopLevelFormID = @TopLevelFormID");
			sqlCommand.Parameters.Add(new SqlParameter("@FormID", SqlDbType.NVarChar)).Value = GenerateFormIDForPart(cPartID, cPartRevisionID);
			if (!string.IsNullOrWhiteSpace(cParentPartID))
			{
				sqlCommand.Parameters.Add(new SqlParameter("@ParentFormID", SqlDbType.NVarChar)).Value = GenerateFormIDForPart(cParentPartID, cParentPartRevisionID);
			}
			else
			{
				sqlCommand.Parameters.Add(new SqlParameter("@ParentFormID", SqlDbType.NVarChar)).Value = string.Empty;
			}
			if (!string.IsNullOrWhiteSpace(cTopLevelPartID))
			{
				sqlCommand.Parameters.Add(new SqlParameter("@TopLevelFormID", SqlDbType.NVarChar)).Value = GenerateFormIDForPart(cTopLevelPartID, cTopLevelPartRevisionID);
			}
			else
			{
				sqlCommand.Parameters.Add(new SqlParameter("@TopLevelFormID", SqlDbType.NVarChar)).Value = string.Empty;
			}
			sqlCommand.Parameters.Add(new SqlParameter("@SourceUniqueID", SqlDbType.UniqueIdentifier)).Value = uniqueID;
			sqlCommand.Parameters.Add(new SqlParameter("@SourceTable", SqlDbType.NVarChar)).Value = cTable;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				dictionary = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
				foreach (DataRow row in dataTable.Rows)
				{
					string value = row.Field<string>("xaiValue");
					if (!string.IsNullOrWhiteSpace(value))
					{
						dictionary.Add(row.Field<string>("xaiControlName"), ReadPropertyString(value));
					}
				}
			}
		}
		return dictionary;
	}

	public void Cleanup()
	{
		Dispose();
	}
}
