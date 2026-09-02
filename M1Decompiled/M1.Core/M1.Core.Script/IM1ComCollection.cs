using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace M1.Core.Script;

[Guid("A4C46780-499F-101B-BB78-00AA00383CBB")]
[ComVisible(true)]
public interface IM1ComCollection
{
	[IndexerName("_Default")]
	[DispId(0)]
	object this[string name] { get; }

	int Add(object value);

	void Clear();

	bool Contains(object value);

	int IndexOf(object value);

	void Insert(int index, object value);

	void Remove(object value);

	void RemoveAt(int index);

	void LoadCollection(object controlCollection);

	[DispId(-4)]
	IEnumerator GetEnumerator();
}
[Guid("A4C46780-499F-101B-BB78-00AA00383CBB")]
[ComVisible(true)]
public interface IM1ComCollection<T>
{
	[IndexerName("_Default")]
	[DispId(0)]
	T this[string name] { get; }

	void Add(T value);

	void Clear();

	bool Contains(object value);

	int IndexOf(T value);

	void Insert(int index, T value);

	bool Remove(T value);

	void RemoveAt(int index);

	[DispId(-4)]
	IEnumerator<T> GetEnumerator();
}
