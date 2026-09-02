using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.Core;
using M1.Extensions;

namespace M1.API.Repositories;

public abstract class APIBaseRepository : IAPIBaseRepository, IDisposable
{
	private static readonly Regex filterPattern = new Regex("^[a-zA-Z0-9_!@#$%^&*\\(\\)\\-+=\\{\\}\\[\\]\\|\\\\:;\"'<>,./]+\\[(eq|gt|lt|ne)\\][a-zA-Z0-9_!@#$%^&*\\(\\)\\-+=\\{\\}\\[\\]\\|\\\\:;\"'<>,./]+$", RegexOptions.Compiled);

	private static readonly string[] forbiddenSqlCommands = new string[6] { "DELETE", "TRUNCATE", "DROP", "INSERT", "UPDATE", "SELECT" };

	public readonly string TAXRATE_FOR_TAXCODEID = "SELECT TOP 1  xabTaxRate FROM TaxCodeLines WHERE (xabTaxCodeID  = @p1 AND (CONVERT(DATETIME, ISNULL(XABEFFECTIVEDATE, '1900-01-01'), 102)  < CONVERT(DATETIME, @p2, 102))) ORDER BY xabEffectiveDate DESC";

	public M1Database M1database { get; set; }

	public M1Database M1DD { get; set; }

	public List<string> OrderOrGroupByList { get; set; }

	public List<string> selectList { get; set; }

	public Dictionary<string, dynamic> filterList { get; set; }

	public string ApiID { get; set; }

	public int MaxPageSize { get; set; } = 1000;

	private static SqlCommand getSelectSqlCommand(string m1TableName, Dictionary<string, dynamic> filterPairs, List<string> returnList, List<string> orderbyList, int? pageSize = null, int? pageNumber = null)
	{
		SqlCommand command = null;
		StringBuilder stringBuilder = new StringBuilder("*");
		StringBuilder stringBuilder2 = new StringBuilder();
		StringBuilder stringBuilder3 = new StringBuilder("ORDER BY ");
		string text = string.Empty;
		if (returnList != null)
		{
			stringBuilder.Length = 0;
			stringBuilder.Append(string.Join(",", returnList.Select((string r, int i) => r)));
		}
		if (orderbyList != null)
		{
			stringBuilder3.Append(string.Join(",", orderbyList.Select((string r, int i) => r)));
		}
		else
		{
			stringBuilder3.Length = 0;
			stringBuilder3.Clear();
		}
		if (pageNumber.HasValue && pageSize.HasValue)
		{
			text = AddPaginationClause(pageSize.Value, pageNumber.Value);
		}
		if (filterPairs != null && filterPairs.Count > 0)
		{
			stringBuilder2 = getWhereFilterString(filterPairs, out var commandParameters);
			command = new SqlCommand($"SELECT {stringBuilder} FROM {m1TableName} WHERE {stringBuilder2} {stringBuilder3}{text}".Trim());
			commandParameters.ForEach(delegate(SqlParameter p)
			{
				command.Parameters.Add(p);
			});
		}
		else
		{
			command = new SqlCommand($"SELECT {stringBuilder} FROM {m1TableName} {stringBuilder3}{text}".Trim());
		}
		return command;
	}

	private static string AddPaginationClause(int pageSize, int pageNumber)
	{
		if (pageSize <= 0 || pageNumber < 0)
		{
			return string.Empty;
		}
		int num = pageNumber * pageSize;
		return $" OFFSET {num} ROWS FETCH NEXT {pageSize} ROWS ONLY ";
	}

	private static SqlCommand getAggregateSqlCommand(string m1TableName, Dictionary<string, dynamic> filterPairs, string columnWithFunctionName, List<string> groupByList)
	{
		SqlCommand command = null;
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder("GROUP BY ");
		if (groupByList != null)
		{
			stringBuilder2.Append(string.Join(",", groupByList.Select((string r, int i) => r)));
		}
		if (filterPairs != null)
		{
			stringBuilder = getWhereFilterString(filterPairs, out var commandParameters);
			command = new SqlCommand($"SELECT {columnWithFunctionName} FROM {m1TableName} WHERE {stringBuilder} {stringBuilder2}".Trim());
			commandParameters.ForEach(delegate(SqlParameter p)
			{
				command.Parameters.Add(p);
			});
		}
		else
		{
			command = new SqlCommand($"SELECT {columnWithFunctionName} FROM {m1TableName} {stringBuilder2}".Trim());
		}
		return command;
	}

	private static StringBuilder getWhereFilterString(Dictionary<string, dynamic> filterPairs, out List<SqlParameter> commandParameters)
	{
		commandParameters = new List<SqlParameter>();
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		string arg = "=";
		foreach (KeyValuePair<string, object> filterPair in filterPairs)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" AND ");
			}
			stringBuilder2.Length = 0;
			dynamic val = (((dynamic)filterPair.Value == null) ? string.Empty : ((dynamic)filterPair.Value).ToString());
			if (filterPair.Key.IndexOf('|') > 0)
			{
				int num = filterPair.Key.IndexOf('|');
				stringBuilder2.Append(filterPair.Key.Substring(num + 1));
				if (stringBuilder2.ToString().Trim().StartsWith("C") || stringBuilder2.ToString().Trim().StartsWith("c"))
				{
					if (stringBuilder2.Length > 1 || (!string.IsNullOrWhiteSpace(stringBuilder2.ToString()) && !stringBuilder2.ToString().Equals("c", StringComparison.CurrentCultureIgnoreCase)))
					{
						arg = stringBuilder2.ToString().Substring(1);
					}
					stringBuilder.Append($"{filterPair.Key.Substring(0, num)} {arg} @P{commandParameters.Count}");
					commandParameters.Add(new SqlParameter($"@P{commandParameters.Count}", val));
				}
				else
				{
					if (stringBuilder2.Length > 1 || (!string.IsNullOrWhiteSpace(stringBuilder2.ToString()) && !stringBuilder2.ToString().Equals("c", StringComparison.CurrentCultureIgnoreCase)))
					{
						arg = stringBuilder2.ToString().Trim();
					}
					stringBuilder.Append($"{filterPair.Key.Substring(0, num)} {arg} @P{commandParameters.Count}");
					commandParameters.Add(new SqlParameter($"@P{commandParameters.Count}", val));
				}
			}
			else
			{
				stringBuilder.Append($"{filterPair.Key} = @P{commandParameters.Count}");
				commandParameters.Add(new SqlParameter($"@P{commandParameters.Count}", val));
			}
		}
		return stringBuilder;
	}

	private static string replaceEscape(string str)
	{
		str = str.Replace("'", "''");
		return str;
	}

	public void InitializeParameterLists()
	{
		OrderOrGroupByList = new List<string>();
		selectList = new List<string>();
		filterList = new Dictionary<string, object>();
	}

	public object GetAsObject(string m1TableName, Dictionary<string, dynamic> filterPairs, List<string> returnList, List<string> orderbyList, SqlTransaction sqlTransaction)
	{
		return M1database.ExecuteScalar(getSelectSqlCommand(m1TableName, filterPairs, returnList, orderbyList), sqlTransaction);
	}

	public DataTable GetAsDataTable(string m1TableName, Dictionary<string, dynamic> filterPairs, List<string> returnList, List<string> orderbyList, SqlTransaction sqlTransaction, int? pageSize = null, int? pageNumber = null)
	{
		return M1database.GetDataTable(getSelectSqlCommand(m1TableName, filterPairs, returnList, orderbyList, pageSize, pageNumber), sqlTransaction);
	}

	public DataTable GetAsDataTable(string sqlString, Dictionary<string, dynamic> filterPairs, SqlTransaction sqlTransaction)
	{
		SqlCommand sqlCommand = null;
		sqlCommand = new SqlCommand(sqlString);
		if (filterPairs != null)
		{
			foreach (KeyValuePair<string, object> filterPair in filterPairs)
			{
				sqlCommand.Parameters.AddWithValue(filterPair.Key, (dynamic)filterPair.Value);
			}
		}
		DataTable dataTable = M1database.GetDataTable(sqlCommand, sqlTransaction);
		sqlCommand.Dispose();
		return dataTable;
	}

	public object GetAggregateResult(string m1TableName, Dictionary<string, dynamic> filterPairs, string columnWithFunctionName, List<string> groupByList, SqlTransaction sqlTransaction)
	{
		return M1database.ExecuteScalar(getAggregateSqlCommand(m1TableName, filterPairs, columnWithFunctionName, groupByList), sqlTransaction);
	}

	public void AddCustomFieldsToSelectList(string tableName)
	{
		using SqlCommand sqlCommand = new SqlCommand("\r\n                SELECT TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME\r\n                FROM INFORMATION_SCHEMA.COLUMNS\r\n                WHERE COLUMN_NAME LIKE 'u%' AND TABLE_NAME = @TableName");
		sqlCommand.Parameters.AddWithValue("@TableName", tableName);
		using DataTable dataTable = M1database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count <= 0)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			selectList.Add(row.Field<string>("COLUMN_NAME"));
		}
	}

	public bool ValidateFilterClause(string filterClause)
	{
		if (string.IsNullOrEmpty(filterClause))
		{
			return false;
		}
		if (!filterPattern.IsMatch(filterClause))
		{
			return false;
		}
		string[] array = forbiddenSqlCommands;
		foreach (string value in array)
		{
			if (filterClause.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return false;
			}
		}
		return true;
	}

	public bool ValidateFilterClause(string[] filterClauses)
	{
		foreach (string filterClause in filterClauses)
		{
			if (!ValidateFilterClause(filterClause))
			{
				return false;
			}
		}
		return true;
	}

	public bool ValidateOrderByClause(string orderByClause)
	{
		if (string.IsNullOrEmpty(orderByClause))
		{
			return false;
		}
		Regex regex = new Regex("^[a-zA-Z0-9_]+(\\[(ASC|DESC)?\\])?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		string[] array = orderByClause.Split(',');
		foreach (string text in array)
		{
			if (!regex.IsMatch(text.Trim()))
			{
				return false;
			}
		}
		array = forbiddenSqlCommands;
		foreach (string value in array)
		{
			if (orderByClause.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return false;
			}
		}
		return true;
	}

	public bool CheckCustomTableForUniqueIDField(string tableName, string prefix)
	{
		using SqlCommand sqlCommand = new SqlCommand("\r\n            SELECT 1\r\n            FROM INFORMATION_SCHEMA.COLUMNS\r\n            WHERE TABLE_NAME = @TableName\r\n            AND COLUMN_NAME = '" + prefix + "UniqueID'\r\n            AND DATA_TYPE = 'uniqueidentifier'");
		sqlCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)
		{
			Value = tableName
		});
		return M1database.ExecuteScalar(sqlCommand) != null;
	}

	public List<string> GetAllColumnsForCustomTable(string tableName, string prefix)
	{
		List<string> list = new List<string>();
		using SqlCommand sqlCommand = new SqlCommand("\r\n            SELECT COLUMN_NAME\r\n            FROM INFORMATION_SCHEMA.COLUMNS\r\n            WHERE TABLE_NAME = @TableName");
		sqlCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)
		{
			Value = tableName
		});
		foreach (DataRow row in M1database.GetDataTable(sqlCommand).Rows)
		{
			list.Add(row.Field<string>("COLUMN_NAME"));
		}
		return list;
	}

	public void ParseAndAddFilter(string[] filter, Dictionary<string, object> filterList, string[] fields)
	{
		if (filter != null && filter.Length == 0)
		{
			return;
		}
		HashSet<string> hashSet = new HashSet<string>(fields, StringComparer.OrdinalIgnoreCase);
		foreach (string obj in filter)
		{
			int num = obj.IndexOf('[');
			int num2 = obj.IndexOf(']');
			if (num == -1 || num2 == -1 || num2 <= num)
			{
				throw new ArgumentException("Invalid filter format");
			}
			string text = obj.Substring(0, num);
			string text2 = obj.Substring(num + 1, num2 - num - 1);
			string text3 = obj.Substring(num2 + 1);
			bool result;
			if (IsNumeric(text3.ToString()))
			{
				_ = 1;
			}
			else
				bool.TryParse(text3, out result);
			string text4 = "|";
			if (hashSet.Contains(text))
			{
				switch (text2)
				{
				case "eq":
					filterList.Add(text + text4 + "=", GetTypedValue(text3));
					break;
				case "gt":
					filterList.Add(text + text4 + ">", GetTypedValue(text3));
					break;
				case "lt":
					filterList.Add(text + text4 + "<", GetTypedValue(text3));
					break;
				case "ne":
					filterList.Add(text + text4 + "!=", GetTypedValue(text3));
					break;
				default:
					throw new ArgumentException("Invalid filter operator");
				}
			}
		}
	}

	public void ParseAndAddOrderByFields(string orderBy, List<string> orderByList, string[] fields)
	{
		if (string.IsNullOrWhiteSpace(orderBy))
		{
			return;
		}
		string[] array = orderBy.Split(',');
		if (array.Length == 0)
		{
			return;
		}
		HashSet<string> hashSet = new HashSet<string>(fields, StringComparer.OrdinalIgnoreCase);
		string[] array2 = array;
		foreach (string obj in array2)
		{
			int num = obj.IndexOf('[');
			int num2 = obj.IndexOf(']');
			if (num == -1 || num2 == -1 || num2 <= num)
			{
				throw new ArgumentException("Invalid filter format");
			}
			string text = obj.Substring(0, num);
			string text2 = obj.Substring(num + 1, num2 - num - 1);
			if (hashSet.Contains(text))
			{
				orderByList.Add(text + " " + text2);
			}
		}
	}

	private object GetTypedValue(string value)
	{
		return value;
	}

	public bool IsNumeric(string expression)
	{
		if (expression == null)
		{
			return false;
		}
		double result;
		return double.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out result);
	}

	public string GetString(object obj)
	{
		if (obj != null)
		{
			return obj.ToString().Trim();
		}
		return string.Empty;
	}

	public Task<bool> DoesCurrencyCodeExists(string currCode)
	{
		return DoesCurrencyCodeExists(currCode, null);
	}

	public Task<bool> DoesCurrencyCodeExists(string currCode, SqlTransaction sqlTransaction)
	{
		InitializeParameterLists();
		filterList.Add("mcpCurrencyRateID|C", currCode);
		selectList.Add("mcpCurrencyRateID");
		return Task.FromResult(GetAsObject("CurrencyRates", filterList, selectList, null, sqlTransaction) != null);
	}

	public Task<bool> DoesPaymentTermExists(string paymentTermCode)
	{
		return DoesPaymentTermExists(paymentTermCode, null);
	}

	public Task<bool> DoesPaymentTermExists(string paymentTermCode, SqlTransaction sqlTransaction)
	{
		InitializeParameterLists();
		filterList.Add("xatPaymentTermID|C", paymentTermCode);
		selectList.Add("xatPaymentTermID");
		return Task.FromResult(GetAsObject("PaymentTerms", filterList, selectList, null, sqlTransaction) != null);
	}

	public Task<bool> DoesTaxCodeExists(string taxCode)
	{
		return DoesTaxCodeExists(taxCode, null);
	}

	public Task<bool> DoesTaxCodeExists(string taxCode, SqlTransaction sqlTransaction)
	{
		InitializeParameterLists();
		filterList.Add("xaxTaxCodeID|C", taxCode);
		filterList.Add("xaxInactive", 0);
		selectList.Add("xaxTaxCodeID");
		return Task.FromResult(GetAsObject("TaxCodes", filterList, selectList, null, sqlTransaction) != null);
	}

	public Task<bool> IsMultiCurrencyEnabled()
	{
		InitializeParameterLists();
		filterList.Add("xadEnableMultiCurrency", 0);
		selectList.Add("xadEnableMultiCurrency");
		return Task.FromResult(GetAsObject("DatasetProperties", filterList, selectList, null, null) != null);
	}

	public Task<decimal> GetExchangeRate(string currencyRateId, DateTime orderDate)
	{
		return GetExchangeRate(currencyRateId, orderDate, null);
	}

	public Task<decimal> GetExchangeRate(string currencyRateId, DateTime orderDate, SqlTransaction sqlTransaction)
	{
		decimal result = 1m;
		if (!string.IsNullOrEmpty(currencyRateId))
		{
			result = M1database.GetExchangeRate(currencyRateId, orderDate, sqlTransaction);
		}
		return Task.FromResult(result);
	}

	public Task<decimal> GetTaxRate(string taxCodeId, DateTime effectiveDate)
	{
		return GetTaxRate(taxCodeId, effectiveDate, null);
	}

	public Task<decimal> GetTaxRate(string taxCodeId, DateTime effectiveDate, SqlTransaction sqlTransaction)
	{
		decimal result = default(decimal);
		InitializeParameterLists();
		filterList.Add("@p1", taxCodeId);
		filterList.Add("@p2", effectiveDate);
		foreach (DataRow row in GetAsDataTable(TAXRATE_FOR_TAXCODEID, filterList, sqlTransaction).Rows)
		{
			result = row.Field<decimal>("xabTaxRate");
		}
		return Task.FromResult(result);
	}

	public Task<string> GetPaymentTermName(string paymentTermID)
	{
		return GetPaymentTermName(paymentTermID, null);
	}

	public Task<string> GetPaymentTermName(string paymentTermID, SqlTransaction sqlTransaction)
	{
		string result = string.Empty;
		if (!string.IsNullOrEmpty(paymentTermID))
		{
			InitializeParameterLists();
			filterList.Add("xatPaymentTermID|C", paymentTermID);
			selectList.Add("xatDescription");
			object asObject = GetAsObject("PaymentTerms", filterList, selectList, null, sqlTransaction);
			if (asObject != null)
			{
				result = (string)asObject;
			}
		}
		return Task.FromResult(result);
	}

	public Task<string> GetShippingMethodName(string shippingMethodID)
	{
		return GetShippingMethodName(shippingMethodID, null);
	}

	public Task<string> GetShippingMethodName(string shippingMethodID, SqlTransaction sqlTransaction)
	{
		string result = string.Empty;
		if (!string.IsNullOrEmpty(shippingMethodID))
		{
			InitializeParameterLists();
			filterList.Add("xasShippingMethodID|C", shippingMethodID);
			selectList.Add("xasDescription");
			object asObject = GetAsObject("ShippingMethods", filterList, selectList, null, sqlTransaction);
			if (asObject != null)
			{
				result = (string)asObject;
			}
		}
		return Task.FromResult(result);
	}

	public Task<string> WhereUsed(string table, object[] aKeys, object[] aKeyFields, bool onlyIncludeForeignRelations = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string[] array = null;
		object[] array2 = null;
		M1DataDictionary m1DataDictionary = M1DD as M1DataDictionary;
		M1Database m1database = M1database;
		string text = string.Empty;
		int num = 0;
		string empty = string.Empty;
		string empty2 = string.Empty;
		string text2 = string.Empty;
		string empty3 = string.Empty;
		if (table.Length > 0)
		{
			array = (from x in aKeyFields
				where x != null
				select x.ToString()).ToArray();
			object[] array3 = (from x in aKeys
				where x != null
				select x.ToString()).ToArray();
			array2 = array3;
			empty3 = string.Join(",", array);
			if (!table.Equals("DOCUMENTLINKS", StringComparison.CurrentCultureIgnoreCase))
			{
				for (int num2 = 0; num2 < array.Length; num2++)
				{
					if (array[num2].Length <= 0)
					{
						continue;
					}
					using SqlCommand sqlCommand = new SqlCommand("SELECT " + m1DataDictionary.Language.GetdfCaptionField(M1database) + " FROM DDFields " + m1DataDictionary.Language.GetdfCaptionJoin(M1database) + " WHERE dfField = @field");
					sqlCommand.Parameters.Add(new SqlParameter("@field", SqlDbType.NVarChar)).Value = array[num2];
					DataTable dataTable = m1DataDictionary.GetDataTable(sqlCommand);
					if (dataTable.Rows.Count > 0 && array2.GetUpperBound(0) >= num2 && array2[num2] != null)
					{
						text = text + ", " + dataTable.Rows[0]["dfCaption"]?.ToString() + " " + array2[num2].ToString().Trim();
					}
				}
				string empty4 = string.Empty;
				empty4 = ((!onlyIncludeForeignRelations) ? ("SELECT drcTable, drcField, drDFilter, dtGridID, " + m1DataDictionary.Language.GetdtCaptionField(m1database) + " FROM DDRelations INNER JOIN DDTables ON drCTable = dtTable " + m1DataDictionary.Language.GetdtCaptionJoin(m1database) + " WHERE drPTable = @table AND drPField = @keys ORDER BY dtCaption") : ("SELECT drcTable, drcField, drDFilter, dtGridID, " + m1DataDictionary.Language.GetdtCaptionField(m1database) + " FROM DDRelations INNER JOIN DDTables ON drCTable = dtTable " + m1DataDictionary.Language.GetdtCaptionJoin(m1database) + " WHERE drPTable = @table AND drForeign <> 0 AND drPField = @keys ORDER BY dtCaption"));
				using SqlCommand sqlCommand2 = new SqlCommand(empty4);
				sqlCommand2.Parameters.Add(new SqlParameter("@table", SqlDbType.NVarChar)).Value = table;
				sqlCommand2.Parameters.Add(new SqlParameter("@keys", SqlDbType.NVarChar)).Value = empty3;
				DataTable dataTable2 = m1DataDictionary.GetDataTable(sqlCommand2);
				if (dataTable2.Rows.Count > 0)
				{
					foreach (DataRow row2 in dataTable2.Rows)
					{
						string text3 = row2["drcTable"].ToString().Trim();
						empty = row2["dtCaption"].ToString().Trim();
						empty2 = string.Empty;
						string[] array4 = row2["drCField"].ToString().Split(',');
						for (int num3 = 0; num3 < array2.Length; num3++)
						{
							empty2 = empty2 + " AND " + array4[num3] + " = " + array2[num3].ToSql();
						}
						if (empty2.Length > 0)
						{
							empty2 = empty2.Substring(5);
						}
						using SqlCommand sqlCommand3 = new SqlCommand("SELECT COUNT(*) AS Rec_Count FROM " + text3 + " WHERE " + empty2);
						DataTable dataTable3 = M1database.GetDataTable(sqlCommand3);
						if (dataTable3.Rows.Count <= 0)
						{
							continue;
						}
						num = 0;
						if (dataTable3.Rows[0]["Rec_Count"] == DBNull.Value)
						{
							continue;
						}
						num = Convert.ToInt32(dataTable3.Rows[0]["Rec_Count"]);
						if (num == 0)
						{
							continue;
						}
						text2 = text2 + num.ToSql() + " " + empty + "\r\n";
						_ = string.Empty;
						if (row2["drcField"].ToString().Trim().Length > 0)
						{
							using SqlCommand sqlCommand4 = new SqlCommand("SELECT " + m1DataDictionary.Language.GetdfCaptionField(m1database) + " FROM DDFields " + m1DataDictionary.Language.GetdfCaptionJoin(m1database) + " WHERE dfField = @field");
							sqlCommand4.Parameters.Add(new SqlParameter("@field", SqlDbType.NVarChar)).Value = array4[0];
							DataTable dataTable4 = m1DataDictionary.GetDataTable(sqlCommand4);
							if (dataTable4.Rows.Count > 0)
							{
								_ = " - " + dataTable4.Rows[0]["dfCaption"];
							}
						}
						stringBuilder.AppendLine(num + " " + empty);
					}
				}
			}
		}
		string empty5 = string.Empty;
		empty5 = ((!onlyIncludeForeignRelations) ? $"With RelationsCTE As ( SELECT drPTable, drCTable, drCField, drFilter FROM DDRelations WHERE drPTable = {table.ToSql()} AND drSaveAs <> 0 Union All SELECT childRelations.drPTable, childRelations.drCTable, childRelations.drCField, childRelations.drFilter FROM RelationsCTE parentRelations Inner Join DDRelations childRelations on parentRelations.drCTable = childRelations.drPTable WHERE childRelations.drSaveAs <> 0 ) Select Distinct * from RelationsCTE;" : $"With RelationsCTE As ( SELECT drPTable, drCTable, drCField, drFilter FROM DDRelations WHERE drPTable = {table.ToSql()} AND drSaveAs <> 0  AND drPersist <> 0 Union All SELECT childRelations.drPTable, childRelations.drCTable, childRelations.drCField, childRelations.drFilter FROM RelationsCTE parentRelations Inner Join DDRelations childRelations on parentRelations.drCTable = childRelations.drPTable WHERE childRelations.drSaveAs <> 0 AND childRelations.drPersist <> 0 ) Select Distinct * from RelationsCTE;");
		foreach (DataRow row3 in m1DataDictionary.GetDataTable(empty5).Rows)
		{
			string empty6 = string.Empty;
			empty6 = ((!onlyIncludeForeignRelations) ? ("SELECT drCTable, drcField, drPField, drDFilter, dtGridID, " + m1DataDictionary.Language.GetdtCaptionField(m1database) + " FROM DDRelations INNER JOIN DDTables ON drCTable = dtTable " + m1DataDictionary.Language.GetdtCaptionJoin(m1database) + " WHERE drPTable = @table AND drPField like '%UniqueID' ORDER BY dtCaption") : ("SELECT drCTable, drcField, drPField, drDFilter, dtGridID, " + m1DataDictionary.Language.GetdtCaptionField(m1database) + " FROM DDRelations INNER JOIN DDTables ON drCTable = dtTable " + m1DataDictionary.Language.GetdtCaptionJoin(m1database) + " WHERE drPTable = @table AND drForeign <> 0 AND drPField like '%UniqueID' ORDER BY dtCaption"));
			using SqlCommand sqlCommand5 = new SqlCommand(empty6);
			sqlCommand5.Parameters.Add(new SqlParameter("@table", SqlDbType.NVarChar)).Value = row3.Field<string>("drCTable");
			DataTable dataTable5 = m1DataDictionary.GetDataTable(sqlCommand5);
			if (dataTable5.Rows.Count <= 0)
			{
				continue;
			}
			foreach (DataRow row4 in dataTable5.Rows)
			{
				string text4 = row4["drcTable"].ToString().Trim();
				empty = row4["dtCaption"].ToString().Trim();
				empty2 = string.Empty;
				string[] array5 = row4["drCField"].ToString().Split(',');
				string[] array6 = row4["drPField"].ToString().Split(',');
				string[] array7 = row3.Field<string>("drCField").Split(',');
				string text5 = string.Empty;
				for (int num4 = 0; num4 < array2.Length; num4++)
				{
					text5 = text5 + " And " + array7[num4] + " = " + array2[num4].ToSql();
				}
				if (text5.Length > 0)
				{
					text5 = text5.Substring(5);
				}
				SqlCommand sqlCommand6 = new SqlCommand("Select " + row4["drpField"].ToString() + " From " + row3.Field<string>("drCTable") + " Where " + text5);
				DataTable dataTable6 = m1database.GetDataTable(sqlCommand6);
				object[] array8 = new object[dataTable6.Rows.Count];
				if (dataTable6.Rows.Count <= 0)
				{
					continue;
				}
				for (int num5 = 0; num5 < dataTable6.Rows.Count; num5++)
				{
					array8[num5] = dataTable6.Rows[num5][array6[0]];
				}
				for (int num6 = 0; num6 < array8.Length; num6++)
				{
					empty2 = empty2 + " Or " + array5[0] + " = " + array8[num6].ToSql();
				}
				if (empty2.Length > 0)
				{
					empty2 = empty2.Substring(4);
				}
				using SqlCommand sqlCommand7 = new SqlCommand("SELECT COUNT(*) AS Rec_Count FROM " + text4 + " WHERE " + empty2);
				DataTable dataTable7 = m1database.GetDataTable(sqlCommand7);
				if (dataTable7.Rows.Count <= 0)
				{
					continue;
				}
				num = 0;
				if (dataTable7.Rows[0]["Rec_Count"] == DBNull.Value)
				{
					continue;
				}
				num = Convert.ToInt32(dataTable7.Rows[0]["Rec_Count"]);
				if (num == 0)
				{
					continue;
				}
				text2 = text2 + num.ToSql() + " " + empty + "\r\n";
				_ = string.Empty;
				if (row4["drcField"].ToString().Trim().Length > 0)
				{
					using SqlCommand sqlCommand8 = new SqlCommand("SELECT " + m1DataDictionary.Language.GetdfCaptionField(m1database) + " FROM DDFields " + m1DataDictionary.Language.GetdfCaptionJoin(m1database) + " WHERE dfField = @field");
					sqlCommand8.Parameters.Add(new SqlParameter("@field", SqlDbType.NVarChar)).Value = array5[0];
					DataTable dataTable8 = m1DataDictionary.GetDataTable(sqlCommand8);
					if (dataTable8.Rows.Count > 0)
					{
						_ = " - " + dataTable8.Rows[0]["dfCaption"];
					}
				}
				stringBuilder.AppendLine(num + " " + empty);
			}
		}
		string empty7 = string.Empty;
		empty7 = ((!onlyIncludeForeignRelations) ? ("SELECT drCTable, drcField, drPField, drDFilter, dtGridID, " + m1DataDictionary.Language.GetdtCaptionField(m1database) + " FROM DDRelations INNER JOIN DDTables ON drCTable = dtTable " + m1DataDictionary.Language.GetdtCaptionJoin(m1database) + " WHERE drPTable = @table AND drPField like '%UniqueID' ORDER BY dtCaption") : ("SELECT drCTable, drcField, drPField, drDFilter, dtGridID, " + m1DataDictionary.Language.GetdtCaptionField(m1database) + " FROM DDRelations INNER JOIN DDTables ON drCTable = dtTable " + m1DataDictionary.Language.GetdtCaptionJoin(m1database) + " WHERE drPTable = @table AND drForeign <> 0 AND drPField like '%UniqueID' ORDER BY dtCaption"));
		using (SqlCommand sqlCommand9 = new SqlCommand(empty7))
		{
			sqlCommand9.Parameters.Add(new SqlParameter("@table", SqlDbType.NVarChar)).Value = table;
			DataTable dataTable9 = m1DataDictionary.GetDataTable(sqlCommand9);
			if (dataTable9.Rows.Count > 0)
			{
				foreach (DataRow row5 in dataTable9.Rows)
				{
					string text6 = row5["drcTable"].ToString().Trim();
					empty = row5["dtCaption"].ToString().Trim();
					empty2 = string.Empty;
					string[] array9 = row5["drCField"].ToString().Split(',');
					string[] array10 = row5["drPField"].ToString().Split(',');
					string text7 = string.Empty;
					for (int num7 = 0; num7 < array.Length; num7++)
					{
						text7 = text7 + " AND " + array[num7] + " = " + array2[num7].ToSql();
					}
					if (text7.Length > 0)
					{
						text7 = text7.Substring(5);
					}
					SqlCommand sqlCommand10 = new SqlCommand("Select " + row5["drpField"].ToString() + " From " + table + " Where " + text7);
					DataTable dataTable10 = m1database.GetDataTable(sqlCommand10);
					object[] array11 = new object[array10.Length];
					if (dataTable10.Rows.Count <= 0)
					{
						continue;
					}
					for (int num8 = 0; num8 < array11.Length; num8++)
					{
						array11[num8] = dataTable10.Rows[0][array10[num8]];
					}
					for (int num9 = 0; num9 < array11.Length; num9++)
					{
						empty2 = empty2 + " AND " + array9[num9] + " = " + array11[num9].ToSql();
					}
					if (empty2.Length > 0)
					{
						empty2 = empty2.Substring(5);
					}
					using SqlCommand sqlCommand11 = new SqlCommand("SELECT COUNT(*) AS Rec_Count FROM " + text6 + " WHERE " + empty2);
					DataTable dataTable11 = m1database.GetDataTable(sqlCommand11);
					if (dataTable11.Rows.Count <= 0)
					{
						continue;
					}
					num = 0;
					if (dataTable11.Rows[0]["Rec_Count"] == DBNull.Value)
					{
						continue;
					}
					num = Convert.ToInt32(dataTable11.Rows[0]["Rec_Count"]);
					if (num == 0)
					{
						continue;
					}
					text2 = text2 + num.ToSql() + " " + empty + "\r\n";
					_ = string.Empty;
					if (row5["drcField"].ToString().Trim().Length > 0)
					{
						using SqlCommand sqlCommand12 = new SqlCommand("SELECT " + m1DataDictionary.Language.GetdfCaptionField(m1database) + " FROM DDFields " + m1DataDictionary.Language.GetdfCaptionJoin(m1database) + " WHERE dfField = @field");
						sqlCommand12.Parameters.Add(new SqlParameter("@field", SqlDbType.NVarChar)).Value = array9[0];
						DataTable dataTable12 = m1DataDictionary.GetDataTable(sqlCommand12);
						if (dataTable12.Rows.Count > 0)
						{
							_ = " - " + dataTable12.Rows[0]["dfCaption"];
						}
					}
					stringBuilder.AppendLine(num + " " + empty);
				}
			}
		}
		return Task.FromResult(stringBuilder.ToString());
	}

	public string GetCustomTablePrefix(string tableName)
	{
		return M1DD.ExecuteScalar("SELECT TOP 1 ISNULL(dtPrefix, '') FROM DDTables WHERE dtTable = '" + tableName + "' and dtCustom = 1")?.ToString();
	}

	public bool DoesCustomTableExist(string tableName)
	{
		return M1DD.ExecuteScalar("SELECT TOP 1 1 FROM DDTables WHERE dtTable = '" + tableName + "' and dtCustom = 1") != null;
	}

	public Task<APIValidationInfoDto> DeleteRowFromTable(string tableName, string tablePrefix, Guid recordId)
	{
		APIValidationInfoDto result = new APIValidationInfoDto();
		SqlTransaction sqlTransaction = null;
		List<string> list = new List<string>();
		try
		{
			sqlTransaction = M1database.BeginTransaction();
			using (SqlCommand sqlCommand = M1database.NewSqlCommand("DELETE FROM " + tableName + " WHERE " + tablePrefix + "UniqueID = @UniqueID"))
			{
				sqlCommand.Parameters.AddWithValue("@UniqueID", recordId);
				M1database.ExecuteCommand(sqlCommand, sqlTransaction);
			}
			M1database.CommitTransaction(sqlTransaction);
		}
		catch (Exception ex)
		{
			M1database.RollbackTransaction(sqlTransaction);
			list.Add($"Error occurred [{ex.Message}] while processing the delete action of {tableName} [{recordId}]");
			result = new APIValidationInfoDto(list, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(result);
	}

	public Task<bool> DoesRecordExistInTableUsingKeys(string table, object[] aKeyFields, object[] aKeyValues)
	{
		if (aKeyValues.Length != aKeyFields.Length)
		{
			return Task.FromResult(result: false);
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < aKeyValues.Length; i++)
		{
			stringBuilder.Append($" AND {aKeyFields[i]} = {aKeyValues[i].ToSql()}");
		}
		string queryString = "SELECT COUNT(*) FROM " + table + " WHERE " + stringBuilder.ToString().Substring(5);
		object obj = M1database.ExecuteScalar(queryString);
		return Task.FromResult(obj != null && Convert.ToInt32(obj) > 0);
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}
}
