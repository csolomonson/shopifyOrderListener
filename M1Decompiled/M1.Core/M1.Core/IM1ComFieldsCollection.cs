using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace M1.Core;

[Guid("0490E147-F2D2-4909-A4B8-3533D2F264D0")]
[ComVisible(true)]
public interface IM1ComFieldsCollection
{
	[IndexerName("_Default")]
	[DispId(0)]
	FieldDefinition this[string name] { get; }

	void Clear();

	bool Contains(FieldDefinition value);

	int IndexOf(FieldDefinition value);

	void Insert(int index, FieldDefinition value);

	bool Remove(string value);

	void RemoveAt(int index);

	[DispId(-4)]
	IEnumerator<FieldDefinition> GetEnumerator();
}
