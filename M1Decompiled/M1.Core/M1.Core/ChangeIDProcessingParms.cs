using System.Data.SqlClient;
using System.Text;

namespace M1.Core;

public class ChangeIDProcessingParms
{
	public bool NewIDExists;

	public bool UsersChoiceOfCascadingChangeOnDefaultBin;

	public bool ParentIDMustExist = true;

	public bool LastKeyCanBeEmpty;

	public string Table;

	public object[] OldKeyValues;

	public object[] NewKeyValues;

	public short ChangeIDType;

	public SqlTransaction SqlTransaction;

	public M1DataDictionary DataDictionary;

	public M1Database Database;

	public StringBuilder DeleteStatements = new StringBuilder();

	public StringBuilder UpdateStatements = new StringBuilder();

	public StringBuilder ProcessChangeIdMessage = new StringBuilder();

	public ChangeIDProcessingParms(string table, object[] oldKeyValues, object[] newKeyValues, short changeIDType, M1DataDictionary dataDictionary, M1Database database)
	{
		Table = table;
		OldKeyValues = oldKeyValues;
		NewKeyValues = newKeyValues;
		ChangeIDType = changeIDType;
		DataDictionary = dataDictionary;
		Database = database;
		NewIDExists = false;
		SqlTransaction = null;
	}
}
