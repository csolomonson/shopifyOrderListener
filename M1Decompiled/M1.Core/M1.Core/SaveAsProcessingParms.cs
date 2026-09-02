using System.Data.SqlClient;

namespace M1.Core;

public class SaveAsProcessingParms
{
	public SqlTransaction SqlTransaction;

	public M1DataDictionary DataDictionary;

	public M1Database Database;

	public object[] OldKeyValues;

	public object[] NewKeyValues;

	public string[] KeyFieldNames;

	public string Table;

	public string TableDescription;

	public string ParentTable;

	public string ParentKeyFields;

	public bool ParentIdExists;

	public SaveAsProcessingParms(M1DataDictionary dataDictionary, M1Database database, string table, object[] oldKeyValues, object[] newKeyValues, string[] keyFieldNames, string parentTable, string parentKeyFields, string tableDescription)
	{
		DataDictionary = dataDictionary;
		Database = database;
		SqlTransaction = null;
		Table = table;
		TableDescription = tableDescription;
		OldKeyValues = oldKeyValues;
		NewKeyValues = newKeyValues;
		KeyFieldNames = keyFieldNames;
		ParentKeyFields = parentKeyFields;
		ParentTable = parentTable;
	}
}
