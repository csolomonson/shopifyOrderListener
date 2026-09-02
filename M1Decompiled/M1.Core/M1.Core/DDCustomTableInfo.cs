using System.Collections.Generic;

namespace M1.Core;

public class DDCustomTableInfo
{
	public string TempTable = string.Empty;

	public string CustomFieldsSelectStatement = string.Empty;

	public string StandardFieldsSelectStatement = string.Empty;

	public string StandardFieldsSelectStatementWithAppExtension = string.Empty;

	public string AppExtensionField = string.Empty;

	public List<string> ReloadStatements = new List<string>();

	public string LoadTableExpression = string.Empty;

	public string Table = string.Empty;

	public bool QueryHasRun;
}
