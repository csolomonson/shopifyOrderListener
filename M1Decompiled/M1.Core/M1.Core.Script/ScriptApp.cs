using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ADODB;
using M1.Core.Database;
using M1.Core.Report;
using M1.Extensions;
using M1.Script.Interfaces;

namespace M1.Core.Script;

[ComVisible(true)]
public class ScriptApp : IServiceProvider, IApp, IDisposable
{
	public M1User User;

	private M1Database _databaseRef;

	public M1DataDictionary dataDictionaryRef;

	protected AppContext contextRef;

	private Connection _Connection;

	private Connection _DDConnection;

	private bool _UpdatingCost;

	private bool _UpdatingUnitCost;

	private bool _WebGearServerMode;

	private AppAx _Ax;

	private AppScript _Script;

	private AppSecurity _Security;

	private AppSecurityInfo _SecurityInfo;

	private short transactionCount;

	private short _TransactionLevel;

	private M1AdoRecordsetProxy _PropsProxy;

	private M1AdoConnectionProxy _CheckedConnection = new M1AdoConnectionProxy();

	private bool _InRelFieldChangeEvent;

	private AppExport _Export;

	private AppPrinter _Printer;

	private AppIO _IO;

	private AppConvert _ComConvert;

	public M1Database databaseRef
	{
		get
		{
			return _databaseRef;
		}
		set
		{
			_databaseRef = value;
			_CheckedConnection.Database = databaseRef;
		}
	}

	public object Connection
	{
		get
		{
			checkConnections();
			return _Connection;
		}
	}

	public object Connection2
	{
		get
		{
			checkConnections();
			return _Connection;
		}
	}

	public object DDConnection
	{
		get
		{
			checkConnections();
			return _DDConnection;
		}
	}

	public bool UpdatingCost
	{
		get
		{
			return _UpdatingCost;
		}
		set
		{
			_UpdatingCost = value;
		}
	}

	public bool InUserChangeEvent
	{
		get
		{
			return databaseRef.InUserChangeEvent;
		}
		set
		{
			databaseRef.InUserChangeEvent = value;
		}
	}

	public bool UpdatingUnitCost
	{
		get
		{
			return _UpdatingUnitCost;
		}
		set
		{
			_UpdatingUnitCost = value;
		}
	}

	public string DataDictionaryDB => dataDictionaryRef.ID;

	public string Database => databaseRef.ID;

	public string DatasetID => Database.Substring(3, 2);

	public string ServerLocation => contextRef.Server.Location;

	public string ClientLocation => contextRef.Client.Location;

	public bool IsHosted => contextRef.IsHosted;

	public string FileShareLocation => contextRef.Metadata.FileShareLocation;

	public string UserID => User.ID;

	public bool WebGearServerMode => _WebGearServerMode;

	public bool AutoShutDownMode => User.AutoShutdownMode;

	public IUserSettings UserSettings => User.Settings;

	public IScript Script
	{
		get
		{
			if (_Script == null)
			{
				_Script = new AppScript(databaseRef);
			}
			return _Script;
		}
	}

	public ISecurity Security
	{
		get
		{
			if (_Security == null)
			{
				_Security = new AppSecurity(databaseRef);
			}
			return _Security;
		}
	}

	public ISecurityInfo SecurityInfo
	{
		get
		{
			if (_SecurityInfo == null)
			{
				_SecurityInfo = new AppSecurityInfo(databaseRef);
			}
			return _SecurityInfo;
		}
	}

	public short TransactionLevel => _TransactionLevel;

	public string DatasetDescription => databaseRef.Description;

	public string DDRegion => dataDictionaryRef.Region;

	public string DSRegion => databaseRef.Region;

	public string CompanyNumberText => databaseRef.Region switch
	{
		"US" => "Fed ID", 
		"CAN" => "FBN", 
		"UK" => "VAT ID", 
		"NL" => "VAT ID", 
		"NZ" => "GST No", 
		_ => "ABN", 
	};

	public DateTime LastActivity
	{
		get
		{
			return User.LastActivityTime;
		}
		set
		{
			if (User != null)
			{
				User.LastActivityTime = value;
			}
		}
	}

	public string HomeCurrencyID
	{
		get
		{
			return databaseRef.HomeCurrencyID;
		}
		set
		{
			databaseRef.HomeCurrencyID = value;
		}
	}

	public string HomeCurrencySymbol => databaseRef.HomeCurrencySymbol;

	public string LanguageTable
	{
		get
		{
			if (databaseRef.LanguageTable.Length == 0)
			{
				return dataDictionaryRef.Language.LanguageTable;
			}
			return databaseRef.LanguageTable;
		}
	}

	public SqlTransaction SqlTransaction
	{
		get
		{
			return _CheckedConnection.SqlTransaction;
		}
		set
		{
			_CheckedConnection.SqlTransaction = value;
		}
	}

	public object CheckedConnection
	{
		get
		{
			if (SqlTransaction == null)
			{
				return Connection;
			}
			return _CheckedConnection;
		}
	}

	public bool IsWordInstalled => contextRef.IsInstalled.Word;

	public bool IsOpenOfficeInstalled => contextRef.IsInstalled.OpenOffice;

	public bool InRelFieldChangeEvent
	{
		get
		{
			return _InRelFieldChangeEvent;
		}
		set
		{
			_InRelFieldChangeEvent = value;
		}
	}

	public string ShortDateFormat => CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

	public string SystemCurrencySymbol
	{
		get
		{
			int currencyPositivePattern = CultureInfo.CurrentCulture.NumberFormat.CurrencyPositivePattern;
			if (currencyPositivePattern == 1 || currencyPositivePattern == 3)
			{
				return " ";
			}
			return CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
		}
	}

	public int CurrencyPosition => CultureInfo.CurrentCulture.NumberFormat.CurrencyPositivePattern;

	public string DecimalSeparator => CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

	public string ThousandSeparator => CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;

	public bool DBTrustedConnection => contextRef.DBServerManager.ConnectionInfo.TrustedConnection;

	public string DBNetworkLibrary => contextRef.DBServerManager.ConnectionInfo.NetworkLibrary;

	public string DBServer => contextRef.DBServerManager.ConnectionInfo.Server;

	public string DBUserID => contextRef.DBServerManager.ConnectionInfo.SqlUserID;

	public bool DDTrustedConnection => contextRef.DDServerManager.ConnectionInfo.TrustedConnection;

	public string DDServer => contextRef.DDServerManager.ConnectionInfo.Server;

	public string DDUserID => contextRef.DDServerManager.ConnectionInfo.SqlUserID;

	public IExport Export
	{
		get
		{
			if (_Export == null)
			{
				_Export = new AppExport(databaseRef);
			}
			return _Export;
		}
	}

	public IPrinter Printer
	{
		get
		{
			if (_Printer == null)
			{
				_Printer = new AppPrinter();
			}
			return _Printer;
		}
	}

	public IIO IO
	{
		get
		{
			if (_IO == null)
			{
				_IO = new AppIO();
			}
			return _IO;
		}
	}

	public IConvert Convert
	{
		get
		{
			if (_ComConvert == null)
			{
				_ComConvert = new AppConvert();
			}
			return _ComConvert;
		}
	}

	public bool RunningFromMobile { get; set; }

	public ScriptApp(IServiceProvider provider)
	{
		contextRef = provider.GetService(typeof(AppContext)) as AppContext;
		dataDictionaryRef = provider.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		User = provider.GetService(typeof(M1User)) as M1User;
		databaseRef = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public object Ax(string id)
	{
		if (_Ax == null)
		{
			_Ax = new AppAx(this);
		}
		return _Ax[id];
	}

	private void checkConnections()
	{
		if (_Connection == null || _DDConnection == null)
		{
			ReopenComConnection();
		}
	}

	public void CloseComConnection()
	{
		_ = _Connection;
		if (_Connection != null && _Connection.State != 0)
		{
			try
			{
				_Connection.DefaultDatabase = "master";
			}
			catch
			{
			}
			finally
			{
				_Connection.Close();
			}
		}
		_Connection = null;
		if (_DDConnection != null && _DDConnection.State != 0)
		{
			try
			{
				_DDConnection.DefaultDatabase = "master";
			}
			catch
			{
			}
			finally
			{
				_DDConnection.Close();
			}
		}
		_DDConnection = null;
	}

	public void ReopenComConnection()
	{
		if (_Connection == null)
		{
			_Connection = new ConnectionClass();
		}
		if (_Connection != null && _Connection.State != 0)
		{
			_Connection.Close();
		}
		if (!AppContext.InQuickExit)
		{
			contextRef.DBServerManager.GetComConnection(databaseRef.ID, User.ID, _Connection);
		}
		if (_DDConnection == null)
		{
			_DDConnection = new ConnectionClass();
		}
		if (_DDConnection != null && _DDConnection.State != 0)
		{
			_DDConnection.Close();
		}
		if (!AppContext.InQuickExit)
		{
			contextRef.DDServerManager.GetComConnection(dataDictionaryRef.ID, User.ID, _DDConnection);
		}
	}

	public void BeginTransaction()
	{
		if (_TransactionLevel == 0)
		{
			checkConnections();
			object RecordsAffected = 0;
			_Connection.Execute("Begin Transaction", out RecordsAffected);
			_TransactionLevel++;
			transactionCount++;
		}
		else
		{
			transactionCount++;
		}
	}

	public void CommitTransaction()
	{
		if (transactionCount == 1)
		{
			object RecordsAffected = 0;
			_Connection.Execute("Commit Transaction", out RecordsAffected);
			_TransactionLevel--;
			transactionCount--;
		}
		else
		{
			transactionCount--;
		}
	}

	public void RollbackTransaction()
	{
		if (transactionCount == 1)
		{
			object RecordsAffected = 0;
			_Connection.Execute("Rollback Transaction", out RecordsAffected);
			_TransactionLevel--;
			transactionCount--;
		}
		else
		{
			transactionCount--;
		}
	}

	public void TestComConnection()
	{
		if (Connection != null && _Connection.State != 0)
		{
			object RecordsAffected = 0;
			_Connection.Execute("select getdate()", out RecordsAffected);
		}
	}

	public bool IsNull(object value)
	{
		if (value != null)
		{
			return value == DBNull.Value;
		}
		return true;
	}

	public DateTime CvDate(object uDate, object defaultDate = null)
	{
		if (uDate is M1DataTableFieldComProxy)
		{
			uDate = ((M1DataTableFieldComProxy)uDate).Value;
		}
		if (uDate == null || uDate == DBNull.Value)
		{
			if (defaultDate == null || defaultDate == DBNull.Value)
			{
				return new DateTime(2099, 12, 31);
			}
			return System.Convert.ToDateTime(defaultDate);
		}
		return System.Convert.ToDateTime(uDate);
	}

	public void BlankRecord(object AdoDbRecordset)
	{
		if (AdoDbRecordset is M1AdoRecordsetProxy)
		{
			((M1AdoRecordsetProxy)AdoDbRecordset).CurrentDataRow.BlankRow();
			return;
		}
		_Recordset recordset = (_Recordset)AdoDbRecordset;
		for (int i = 0; i < recordset.Fields.Count; i++)
		{
			switch (recordset.Fields[i].Type)
			{
			case DataTypeEnum.adChar:
			case DataTypeEnum.adWChar:
			case DataTypeEnum.adVarChar:
			case DataTypeEnum.adVarWChar:
				recordset.Fields[i].Value = string.Empty;
				break;
			case DataTypeEnum.adSmallInt:
			case DataTypeEnum.adInteger:
			case DataTypeEnum.adSingle:
			case DataTypeEnum.adDouble:
			case DataTypeEnum.adCurrency:
			case DataTypeEnum.adDecimal:
			case DataTypeEnum.adBigInt:
			case DataTypeEnum.adNumeric:
				recordset.Fields[i].Value = 0;
				break;
			case DataTypeEnum.adDate:
			case DataTypeEnum.adDBDate:
			case DataTypeEnum.adDBTime:
			case DataTypeEnum.adDBTimeStamp:
			case DataTypeEnum.adLongVarChar:
			case DataTypeEnum.adLongVarWChar:
				recordset.Fields[i].Value = DBNull.Value;
				break;
			case DataTypeEnum.adBoolean:
				recordset.Fields[i].Value = false;
				break;
			case DataTypeEnum.adGUID:
				recordset.Fields[i].Value = Guid.NewGuid().ToString("B");
				break;
			}
		}
	}

	public double Max(object value1, object value2)
	{
		double num = System.Convert.ToDouble(value1);
		double num2 = System.Convert.ToDouble(value2);
		if (!(num > num2))
		{
			return num2;
		}
		return num;
	}

	public double Min(object value1, object value2)
	{
		double num = System.Convert.ToDouble(value1);
		double num2 = System.Convert.ToDouble(value2);
		if (!(num < num2))
		{
			return num2;
		}
		return num;
	}

	public string PadLeft(string value, int length, string character)
	{
		if (character.Length == 0)
		{
			return value.PadLeft(length);
		}
		return value.PadLeft(length, character[0]);
	}

	public string AddBackslash(string s)
	{
		return s.AddBackslash();
	}

	public string GetWindowsRegion()
	{
		return contextRef.GetWindowsRegion();
	}

	public bool M1Empty(object value)
	{
		return M1Util.IsNullOrEmpty(value);
	}

	public DateTime BegOfYear(DateTime dValue)
	{
		return new DateTime(dValue.Year, 1, 1);
	}

	public DateTime EOM(DateTime Value, short Offset)
	{
		Value = Value.AddMonths(1 + Offset);
		return Value.AddDays(-Value.Day);
	}

	public void FreeUnusedNextIDForTable(string table, string value, string datasets = "?")
	{
		if (datasets == "?")
		{
			databaseRef.NextIDs.FreeUnusedNextIDForTable(table, value);
		}
		else
		{
			databaseRef.NextIDs.FreeUnusedNextIDForTable(table, value, datasets);
		}
	}

	public string GetNextIDDatasetsForTable(string table)
	{
		return databaseRef.NextIDs.GetNextIDInfo(table).Databases;
	}

	public short GetNextIDAutoIncrementForTable(string table)
	{
		return (short)databaseRef.NextIDs.GetNextIDInfo(table).AutoIncrement;
	}

	public short GetIncrementAmountForTable(string table)
	{
		short num = databaseRef.NextIDs.GetNextIDInfo(table).IncrementAmount;
		if (num <= 1)
		{
			SqlCommand sqlCommand = dataDictionaryRef.NewSqlCommand("select IncrementAmount = Case When dtIncrementAmountUser = 0 Then dtIncrementAmount Else dtIncrementAmountUser End from DDTables where dtTable = @table");
			sqlCommand.Parameters.Add(new SqlParameter("@table", SqlDbType.NVarChar)).Value = table;
			object obj = dataDictionaryRef.ExecuteScalar(sqlCommand);
			if (obj != null)
			{
				num = System.Convert.ToInt16(obj);
				if (num <= 1)
				{
					num = 1;
				}
			}
		}
		return num;
	}

	public void ResetNextIDCacheForTable(string table)
	{
		if (databaseRef.NextIDs.Contains(table))
		{
			databaseRef.NextIDs.Remove(table);
		}
	}

	public object GetNextIDForTable(string table, object parentValues = null)
	{
		return databaseRef.NextIDs.GetNextIDForTable(table, (object[])parentValues);
	}

	public string GetDefaultReportForFolder(string folderName)
	{
		return databaseRef.GetDefaultReportForFolder(folderName);
	}

	public void SetDefaultReportForFolder(string folderName, string reportName)
	{
		databaseRef.SetDefaultReportForFolder(folderName, reportName);
	}

	public string GetDefaultPrinterForReport(string report)
	{
		return databaseRef.GetDefaultPrinterForReport(report);
	}

	public string GetDefaultPrinterForReportAndFolder(string folderName, string report)
	{
		return databaseRef.GetDefaultPrinterForReportAndFolder(folderName, report);
	}

	public object Props(string module, bool hideMessage = false)
	{
		if (_PropsProxy == null)
		{
			_PropsProxy = new M1AdoRecordsetProxy();
		}
		DataRow dataRow = databaseRef.Props(module);
		_PropsProxy.LoadDataTable(dataRow.Table);
		return _PropsProxy;
	}

	public void PropsRefreshData()
	{
		databaseRef.PropsRefresh();
	}

	public object IIf(bool test, object trueResult, object falseResult)
	{
		if (test)
		{
			return trueResult;
		}
		return falseResult;
	}

	public bool CheckHomeCurrency(string currencyID)
	{
		return databaseRef.CheckHomeCurrency(currencyID);
	}

	public double RoundAusCents(double nValue)
	{
		return (double)M1Math.RoundAusCents((decimal)nValue);
	}

	public double Round(double nValue, short decimals)
	{
		return (double)M1Math.Round((decimal)nValue, decimals);
	}

	public object CreateObject(string classId)
	{
		object obj = M1Util.COMCreateObject(classId);
		if (obj == null)
		{
			return DBNull.Value;
		}
		return obj;
	}

	public object CreateRecordset()
	{
		if (SqlTransaction == null)
		{
			return new RecordsetClass();
		}
		return new M1AdoRecordsetProxy();
	}

	public void DropLogTriggersForTable(string table)
	{
		using SqlConnection sqlConnection = contextRef.DBServerManager.GetConnection(User, databaseRef.ID, openImmediately: true);
		new Dmo(contextRef, contextRef.DBServerManager).DropLogTriggersForTable(sqlConnection, User, databaseRef.ID, table);
	}

	public void VerifyLogTriggersForTable(string table)
	{
		using SqlConnection sqlConnection = contextRef.DBServerManager.GetConnection(User, databaseRef.ID, openImmediately: true);
		new Dmo(contextRef, contextRef.DBServerManager).VerifyLogTriggersForTable(sqlConnection, User, dataDictionaryRef, databaseRef.ID, table);
	}

	public string GetCode(string codeName)
	{
		SqlCommand sqlCommand = dataDictionaryRef.NewSqlCommand("Select dkCode From DDScripts Inner Join DDCode On dyUniqueID = dkSourceUniqueID And dkSourceTable = 'DDScripts' Where dyName = @CodeName");
		sqlCommand.Parameters.Add(new SqlParameter("@CodeName", SqlDbType.NVarChar)).Value = codeName;
		string text = (string)dataDictionaryRef.ExecuteScalar(sqlCommand);
		if (text == null)
		{
			return string.Empty;
		}
		return "Class " + codeName + "Class\r\n" + text + "\r\nEnd Class\r\nDim " + codeName + "\r\nSet " + codeName + " = New " + codeName + "Class\r\n";
	}

	public string ReadSetting(string section, string settingName, string defaultValue)
	{
		return contextRef.Server.IniSettings.Get(settingName, defaultValue);
	}

	public string ChangeID(string table, object oldKeyValues, object newKeyValues, short changeIDType)
	{
		return new ChangeIDProcessing().ChangeID(databaseRef, table, (object[])oldKeyValues, (object[])newKeyValues, changeIDType);
	}

	public string GenerateTempFileName(string extension = "")
	{
		return M1Util.GenerateTempFileName(extension);
	}

	public void CreateM1PFile(string file, string url)
	{
		M1Util.CreateM1PFile(file, url);
	}

	public void CreateURLFile(string file, string url)
	{
		M1Util.CreateURLFile(file, url);
	}

	public void SaveTextToFile(string text, string fileName)
	{
		M1Util.SaveTextToFile(text, fileName);
	}

	public string LoadTextFromFile(string sFile)
	{
		if (!string.IsNullOrEmpty(sFile))
		{
			try
			{
				return File.ReadAllText(sFile, Encoding.Default);
			}
			catch (Exception)
			{
				throw new Exception("Given file could not be read.");
			}
		}
		return "";
	}

	public void SaveUserSettings()
	{
		User.Settings.SaveSettings(dataDictionaryRef, User.ID);
	}

	public object DateAddByContractType(object dStartDate, int nLength, string cLengthType)
	{
		if (dStartDate != null && dStartDate != DBNull.Value && nLength > 0)
		{
			DateTime dateTime = System.Convert.ToDateTime(dStartDate);
			cLengthType = cLengthType.Trim().ToUpper();
			if (cLengthType.Length != 0)
			{
				switch (cLengthType)
				{
				case "D":
					return dateTime.AddDays(nLength);
				case "W":
					return dateTime.AddDays(nLength * 7);
				case "M":
					return dateTime.AddMonths(nLength);
				case "Y":
					return dateTime.AddYears(nLength);
				}
			}
		}
		return null;
	}

	public bool CopyMatchingFields(object oSourceData, object oDestData, string cSourcePrefixes = "")
	{
		if (cSourcePrefixes == null)
		{
			cSourcePrefixes = string.Empty;
		}
		if (oSourceData is FieldCollection && oDestData is M1AdoRecordsetProxy)
		{
			string[] array = cSourcePrefixes.Split(',');
			FieldCollection obj = (FieldCollection)oSourceData;
			M1AdoRecordsetProxy m1AdoRecordsetProxy = (M1AdoRecordsetProxy)oDestData;
			foreach (FieldDefinition item in obj)
			{
				foreach (DataColumn column in m1AdoRecordsetProxy.dataView.Table.Columns)
				{
					if (!copyMatchCheck(item.FieldName, column.ColumnName))
					{
						continue;
					}
					if (!item.IsMatchingType(column) || SystemGeneratedFields.IsGenerated(item.FieldName))
					{
						break;
					}
					bool flag = false;
					if (cSourcePrefixes.Length == 0 || array.Length == 0)
					{
						flag = true;
					}
					else
					{
						string[] array2 = array;
						foreach (string value in array2)
						{
							if (item.FieldName.StartsWith(value, StringComparison.CurrentCultureIgnoreCase))
							{
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						m1AdoRecordsetProxy.Fields(column.ColumnName).Value = item.Value;
					}
					break;
				}
			}
			return true;
		}
		if (oSourceData is M1AdoRecordsetProxy && oDestData is FieldCollection)
		{
			string[] array3 = cSourcePrefixes.Split(',');
			M1AdoRecordsetProxy m1AdoRecordsetProxy2 = (M1AdoRecordsetProxy)oSourceData;
			FieldCollection fieldCollection = (FieldCollection)oDestData;
			foreach (DataColumn column2 in m1AdoRecordsetProxy2.dataView.Table.Columns)
			{
				foreach (FieldDefinition item2 in fieldCollection)
				{
					if (!copyMatchCheck(column2.ColumnName, item2.FieldName))
					{
						continue;
					}
					if (!item2.IsMatchingType(column2) || SystemGeneratedFields.IsGenerated(column2.ColumnName))
					{
						break;
					}
					bool flag2 = false;
					if (cSourcePrefixes.Length == 0 || array3.Length == 0)
					{
						flag2 = true;
					}
					else
					{
						string[] array2 = array3;
						foreach (string value2 in array2)
						{
							if (column2.ColumnName.StartsWith(value2, StringComparison.CurrentCultureIgnoreCase))
							{
								flag2 = true;
								break;
							}
						}
					}
					if (flag2)
					{
						item2.Value = m1AdoRecordsetProxy2.Fields(column2.ColumnName).Value;
					}
					break;
				}
			}
			return true;
		}
		if (oSourceData is Recordset && oDestData is M1AdoRecordsetProxy)
		{
			string[] array4 = cSourcePrefixes.Split(',');
			Fields fields = ((Recordset)oSourceData).Fields;
			M1AdoRecordsetProxy m1AdoRecordsetProxy3 = (M1AdoRecordsetProxy)oDestData;
			foreach (Field item3 in fields)
			{
				foreach (DataColumn column3 in m1AdoRecordsetProxy3.dataView.Table.Columns)
				{
					if (!copyMatchCheck(item3.Name, column3.ColumnName))
					{
						continue;
					}
					if (SystemGeneratedFields.IsGenerated(item3.Name))
					{
						break;
					}
					bool flag3 = false;
					if (cSourcePrefixes.Length == 0 || array4.Length == 0)
					{
						flag3 = true;
					}
					else
					{
						string[] array2 = array4;
						foreach (string value3 in array2)
						{
							if (item3.Name.StartsWith(value3, StringComparison.CurrentCultureIgnoreCase))
							{
								flag3 = true;
								break;
							}
						}
					}
					if (flag3)
					{
						m1AdoRecordsetProxy3.Fields(column3.ColumnName).Value = item3.Value;
					}
					break;
				}
			}
			return true;
		}
		if (oSourceData is M1AdoRecordsetProxy && oDestData is Recordset)
		{
			string[] array5 = cSourcePrefixes.Split(',');
			M1AdoRecordsetProxy m1AdoRecordsetProxy4 = (M1AdoRecordsetProxy)oSourceData;
			Fields fields2 = ((Recordset)oDestData).Fields;
			foreach (DataColumn column4 in m1AdoRecordsetProxy4.dataView.Table.Columns)
			{
				foreach (Field item4 in fields2)
				{
					if (!copyMatchCheck(column4.ColumnName, item4.Name))
					{
						continue;
					}
					if (SystemGeneratedFields.IsGenerated(column4.ColumnName))
					{
						break;
					}
					bool flag4 = false;
					if (cSourcePrefixes.Length == 0 || array5.Length == 0)
					{
						flag4 = true;
					}
					else
					{
						string[] array2 = array5;
						foreach (string value4 in array2)
						{
							if (column4.ColumnName.StartsWith(value4, StringComparison.CurrentCultureIgnoreCase))
							{
								flag4 = true;
								break;
							}
						}
					}
					if (flag4)
					{
						fields2[item4.Name].Value = m1AdoRecordsetProxy4.Fields(column4.ColumnName).Value;
					}
					break;
				}
			}
			return true;
		}
		Fields fields3 = null;
		Fields fields4 = null;
		if (oSourceData is Fields)
		{
			fields3 = (Fields)oSourceData;
		}
		else if (oSourceData is Recordset)
		{
			fields3 = ((Recordset)oSourceData).Fields;
		}
		if (oDestData is Fields)
		{
			fields4 = (Fields)oDestData;
		}
		else if (oDestData is Recordset)
		{
			fields4 = ((Recordset)oDestData).Fields;
		}
		if (fields3 != null && fields4 != null)
		{
			string[] array6 = cSourcePrefixes.Split(',');
			foreach (Field item5 in fields3)
			{
				foreach (Field item6 in fields4)
				{
					if (!copyMatchCheck(item5.Name, item6.Name))
					{
						continue;
					}
					if (item5.Type != item6.Type || item5.DefinedSize != item6.DefinedSize || SystemGeneratedFields.IsGenerated(item5.Name))
					{
						break;
					}
					bool flag5 = false;
					if (cSourcePrefixes.Length == 0 || array6.Length == 0)
					{
						flag5 = true;
					}
					else
					{
						string[] array2 = array6;
						foreach (string value5 in array2)
						{
							if (item5.Name.StartsWith(value5, StringComparison.CurrentCultureIgnoreCase))
							{
								flag5 = true;
								break;
							}
						}
					}
					if (flag5)
					{
						item6.Value = item5.Value;
					}
					break;
				}
			}
			return true;
		}
		throw new M1Exception("Unknown type for oSourceData or oDestData parameters in CopyMatchingFields.");
	}

	private bool copyMatchCheck(string field1, string field2)
	{
		if (field1.Length <= 3 || field2.Length <= 3 || !field1.Substring(3).Equals(field2.Substring(3), StringComparison.CurrentCultureIgnoreCase))
		{
			if (field1.Length > 4 && field2.Length > 4)
			{
				return field1.Substring(4).Equals(field2.Substring(5), StringComparison.CurrentCultureIgnoreCase);
			}
			return false;
		}
		return true;
	}

	public string M1GetComputerName()
	{
		return Environment.MachineName;
	}

	public int Occurs(string sSource, string sSearch)
	{
		try
		{
			return sSource.Count((char c) => c == System.Convert.ToChar(sSearch));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public bool IsEmpty(object oValue)
	{
		bool result = false;
		try
		{
			if (oValue == null || oValue == DBNull.Value)
			{
				return true;
			}
			switch (oValue.GetType().ToString())
			{
			case "System.String":
				result = System.Convert.ToString(oValue) == "";
				break;
			case "System.Double":
			case "System.Single":
			case "System.Int16":
			case "System.Int32":
			case "System.Int64":
			case "System.Boolean":
			case "System.Decimal":
			case "System.Byte":
				result = System.Convert.ToInt32(oValue) == 0;
				break;
			case "System.DateTime":
				result = System.Convert.ToInt32(oValue) <= 0;
				break;
			case "System.DBNull":
				result = true;
				break;
			}
			return result;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public string GetModuleText(string module)
	{
		return dataDictionaryRef.Modules.GetModuleText(module);
	}

	public string PrepareQuery(string queryString)
	{
		return databaseRef.PrepareQuery(queryString);
	}

	public void MakeSelectStatements(string fields, string fromClause, string whereClause, string groupClause, string orderClause, string databases, bool loadNow, bool fromGrid, ref string selectNormal, ref string selectLoadOption, ref string extraFields)
	{
		databaseRef.MakeSelectStatements(fields, fromClause, whereClause, groupClause, orderClause, databases, loadNow, fromGrid, ref selectNormal, ref selectLoadOption, ref extraFields);
	}

	public object IncrementValue(object value, short incrementAmount, string format, string fieldType)
	{
		return databaseRef.NextIDs.IncrementValue(value, incrementAmount, format, FieldDefinition.charToFieldType(fieldType));
	}

	public string VBFormat(object uExpr, string cFormat = "")
	{
		if (string.IsNullOrEmpty(cFormat))
		{
			return System.Convert.ToString(uExpr);
		}
		return string.Format(cFormat, uExpr);
	}

	public string TrimNull(string value)
	{
		if (value == null)
		{
			return string.Empty;
		}
		return value.Trim();
	}

	public string GetLanguageText(string languageID, string defaultText = "", object parms = null, string type = "")
	{
		if (parms is string)
		{
			parms = null;
		}
		if (type == null)
		{
			type = string.Empty;
		}
		if (defaultText == null)
		{
			defaultText = string.Empty;
		}
		return dataDictionaryRef.Language.GetLanguageText(databaseRef, languageID, defaultText, (object[])parms, type);
	}

	public string GetLocalString(string text)
	{
		return dataDictionaryRef.Language.GetLocalString(text);
	}

	public object GetDatasets(bool includeVersion = true)
	{
		string[,] array = new string[2, contextRef.InstalledDatabases.Count];
		for (int i = 0; i < contextRef.InstalledDatabases.Count; i++)
		{
			array[0, i] = contextRef.InstalledDatabases[i].Name;
			array[1, i] = contextRef.InstalledDatabases[i].Name.Substring(4) + " - " + contextRef.InstalledDatabases[i].Description + (includeVersion ? (" - " + contextRef.InstalledDatabases[i].Version) : string.Empty);
		}
		return array;
	}

	public string CreateGuid()
	{
		return Guid.NewGuid().ToString("B");
	}

	public object CreateM1Report()
	{
		return null;
	}

	public object OpenReport(string report, string defaults = "")
	{
		if (defaults == null)
		{
			defaults = string.Empty;
		}
		ReportProxy reportProxy = contextRef.Reports.OpenReport(databaseRef, report, defaults);
		if (reportProxy == null)
		{
			return DBNull.Value;
		}
		return reportProxy;
	}

	public string ProcessChangeID(string table, object oldKeyValues, object newKeyValues)
	{
		SqlTransaction sqlTransaction = databaseRef.BeginTransaction();
		try
		{
			string text = new ChangeIDProcessing().ProcessChangeID(databaseRef, table, (object[])oldKeyValues, (object[])newKeyValues, 1, sqlTransaction, null);
			if (text.Length == 0)
			{
				databaseRef.RollbackTransaction(sqlTransaction);
			}
			else
			{
				databaseRef.CommitTransaction(sqlTransaction);
			}
			return text;
		}
		catch (Exception ex)
		{
			databaseRef.RollbackTransaction(sqlTransaction);
			throw new M1Exception(ex.Message);
		}
	}

	public string RunTransferProcess(string processType, IServiceProvider provider, object[] promptValues)
	{
		using (ProcessParameters processParameters = ProcessParameters.CreateTransferProcess(processType, provider, null))
		{
			StartProcessEventArgs e = processParameters.Run(M1Util.TranslateToList(promptValues), NoUICheckForValidationErrors);
			if (e.Cancel)
			{
				if (e.Messages != null && e.Messages.Count != 0)
				{
					StringBuilder builder = new StringBuilder();
					e.Messages.ForEach(delegate(string m)
					{
						builder.AppendLine(m);
					});
					return builder.ToString();
				}
				return "Transfer process " + processType + " was cancelled with no information returned.";
			}
		}
		return string.Empty;
	}

	private bool NoUICheckForValidationErrors(IServiceProvider provider, ErrorItemsList errors, CancelEventArgs arg)
	{
		if (errors != null && errors.Count != 0)
		{
			int num = 0;
			foreach (ValidationInfo error in errors)
			{
				num += error.ErrorCount;
			}
			if (num != 0)
			{
				arg.Cancel = true;
				return false;
			}
		}
		return true;
	}

	public void VBDoEvents()
	{
		Application.DoEvents();
	}

	public virtual object GetService(Type serviceType)
	{
		return databaseRef.GetService(serviceType);
	}

	public void DebugLog(string message, bool overrideAlwaysPrint = false)
	{
		try
		{
			bool flag = false;
			if (overrideAlwaysPrint)
			{
				flag = true;
			}
			if (flag)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(message);
				File.AppendAllText(contextRef.Server.Location + "DebugLog.txt", DateTime.Now.ToString("dd-MMM-yyyy h:mm:ss tt") + " - " + stringBuilder.ToString());
			}
		}
		catch (Exception)
		{
		}
	}

	public void Dispose()
	{
		if (_Ax != null)
		{
			_Ax.Dispose();
			_Ax = null;
		}
		_ = _Connection;
		if (_Connection != null && _Connection.State != 0)
		{
			_Connection.Close();
			_Connection = null;
		}
		if (_DDConnection != null && _DDConnection.State != 0)
		{
			_DDConnection.Close();
			_DDConnection = null;
		}
		User = null;
		databaseRef = null;
		contextRef = null;
		dataDictionaryRef = null;
	}
}
