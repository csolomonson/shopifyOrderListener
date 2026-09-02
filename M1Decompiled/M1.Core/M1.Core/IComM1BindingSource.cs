using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core.Script;

namespace M1.Core;

[ComVisible(true)]
public interface IComM1BindingSource
{
	int Count { get; }

	M1AdoRecordsetProxy Recordset { get; set; }

	TableDefinition PrimaryTable { get; }

	bool AllowEdit { get; }

	DataRow CurrentAsDataRow { get; }

	SqlTransaction Transaction { get; }

	bool Modified { get; set; }

	bool ReadOnly { get; }

	string DataSourceTable { get; set; }

	object ParametersLength { get; }

	void SetLastKeyValue(object value);

	M1AdoRecordsetProxy AddNewAsRs();

	FieldDefinition Fields(string name);

	M1AdoRecordsetProxy GetRecordset(object parentRow = null);

	int SetRowCount(object rowCount);

	void ResequenceKeys();

	double GetTotal(string field);

	void AllocateValueToAllRows(string field, object value, int decimals);

	bool IsAnyRowFieldNotEmpty(string field);

	void RemoveWhere(string vbExpr);

	void OnDataChanged(object value);

	void GenerateActionMessage(string messageID, object parameters, object parametersEx);

	void ShowInfoMsg(string msg);

	void SetKeyToNextAvailable();

	bool DoesKeyExist(object[] keys);

	void NavigateTo(string queryFilter);

	void NavigateToByArray(object[] aKeys);

	void RemoveCurrent();

	object GenerateNextID();

	void MarkAsChanged(bool changed = true);

	bool LoadDefinition(string gridID);

	void ClearCache();

	void OnTableChanged(string tableName);

	object Parameters(object index);
}
