using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace M1.Core.Script;

[Guid("0490E147-F2D2-4909-A4B8-3533D2F264D0")]
[ComVisible(true)]
public interface IM1TestComCollection
{
	int Count { get; }

	[IndexerName("_Default")]
	[DispId(0)]
	M1AdoFieldProxy this[string name]
	{
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
	}

	int Add(object value);

	void Clear();

	bool Contains(object value);

	int IndexOf(object value);

	void Insert(int index, object value);

	void Remove(object value);

	void RemoveAt(int index);

	[DispId(-4)]
	IEnumerator GetEnumerator();
}
