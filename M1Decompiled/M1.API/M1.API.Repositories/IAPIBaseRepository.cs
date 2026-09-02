using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.Core;

namespace M1.API.Repositories;

public interface IAPIBaseRepository : IDisposable
{
	M1Database M1database { get; set; }

	M1Database M1DD { get; set; }

	List<string> OrderOrGroupByList { get; set; }

	List<string> selectList { get; set; }

	Dictionary<string, dynamic> filterList { get; set; }

	int MaxPageSize { get; set; }

	DataTable GetAsDataTable(string sqlString, Dictionary<string, dynamic> filterPairs, SqlTransaction sqlTransaction);

	DataTable GetAsDataTable(string m1TableName, Dictionary<string, dynamic> filterPairs, List<string> returnList, List<string> orderbyList, SqlTransaction sqlTransaction, int? pageSize = null, int? pageNumber = null);

	object GetAsObject(string m1TableName, Dictionary<string, dynamic> filterPairs, List<string> returnList, List<string> orderbyList, SqlTransaction sqlTransaction);

	object GetAggregateResult(string m1TableName, Dictionary<string, dynamic> filterPairs, string columnWithFunction, List<string> groupByList, SqlTransaction sqlTransaction);

	bool IsNumeric(string expression);

	string GetString(object obj);

	Task<bool> DoesCurrencyCodeExists(string currCode);

	Task<bool> DoesCurrencyCodeExists(string currCode, SqlTransaction sqlTransaction);

	Task<bool> DoesPaymentTermExists(string paymentTermCode);

	Task<bool> DoesPaymentTermExists(string paymentTermCode, SqlTransaction sqlTransaction);

	Task<bool> DoesTaxCodeExists(string taxCode);

	Task<bool> DoesTaxCodeExists(string taxCode, SqlTransaction sqlTransaction);

	Task<bool> IsMultiCurrencyEnabled();

	Task<decimal> GetTaxRate(string taxCodeId, DateTime effectiveDate);

	Task<decimal> GetTaxRate(string taxCodeId, DateTime effectiveDate, SqlTransaction sqlTransaction);

	Task<decimal> GetExchangeRate(string currencyRateId, DateTime orderDate);

	Task<decimal> GetExchangeRate(string currencyRateId, DateTime orderDate, SqlTransaction sqlTransaction);

	Task<string> GetPaymentTermName(string paymentTermID);

	Task<string> GetPaymentTermName(string paymentTermID, SqlTransaction sqlTransaction);

	Task<string> GetShippingMethodName(string shippingMethodID);

	Task<string> GetShippingMethodName(string shippingMethodID, SqlTransaction sqlTransaction);

	Task<string> WhereUsed(string table, object[] aKeys, object[] aKeyFields, bool onlyIncludeForeignRelations = false);

	/// <summary>
	/// Deletes a row from the specified table using the records Unique ID.
	/// </summary>
	/// <param name="tableName">M1 Table Name</param>
	/// <param name="tablePrefix">3 Letter M1 Table Prefix</param>
	/// <param name="recordId">The M1 record Unique ID</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> DeleteRowFromTable(string tableName, string tablePrefix, Guid recordId);

	bool ValidateFilterClause(string[] filterClauses);

	bool ValidateFilterClause(string filterClause);

	bool ValidateOrderByClause(string orderByClause);

	string GetCustomTablePrefix(string tableName);

	bool DoesCustomTableExist(string tableName);

	bool CheckCustomTableForUniqueIDField(string tableName, string prefix);

	List<string> GetAllColumnsForCustomTable(string tableName, string prefix);

	Task<bool> DoesRecordExistInTableUsingKeys(string table, object[] aKeyFields, object[] aKeyValues);

	void ParseAndAddOrderByFields(string orderBy, List<string> orderByList, string[] fields);

	void ParseAndAddFilter(string[] filter, Dictionary<string, object> filterList, string[] fields = null);
}
