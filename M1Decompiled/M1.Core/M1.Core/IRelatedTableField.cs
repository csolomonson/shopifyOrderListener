using System.Runtime.InteropServices;

namespace M1.Core;

[ComVisible(true)]
public interface IRelatedTableField
{
	object Value { get; }

	bool RowExists { get; }

	RelatedTableField Fields(string name);
}
