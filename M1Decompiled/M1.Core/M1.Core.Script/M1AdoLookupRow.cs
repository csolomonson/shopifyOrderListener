using System.Data;
using System.Runtime.InteropServices;

namespace M1.Core.Script;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class M1AdoLookupRow
{
	public DataRow Row;

	private M1AdoLookupRowField currentField = new M1AdoLookupRowField();

	public bool EOF => Row == null;

	public object Fields(string fieldName)
	{
		if (Row != null && Row.RowState == DataRowState.Deleted)
		{
			currentField.Value = Row[fieldName, DataRowVersion.Original];
		}
		else if (Row != null && Row.RowState != DataRowState.Detached)
		{
			currentField.Value = Row[fieldName];
		}
		else if (Row != null && Row.HasVersion(DataRowVersion.Proposed))
		{
			currentField.Value = Row[fieldName, DataRowVersion.Proposed];
		}
		else
		{
			currentField.Value = string.Empty;
		}
		return currentField;
	}
}
