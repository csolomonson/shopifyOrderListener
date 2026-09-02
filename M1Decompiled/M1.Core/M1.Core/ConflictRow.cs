using System.Collections.Generic;

namespace M1.Core;

public class ConflictRow
{
	public string RowDescription = string.Empty;

	public List<ConflictItem> FieldConflicts = new List<ConflictItem>();
}
