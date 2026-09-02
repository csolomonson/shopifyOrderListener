using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace M1.Core.Report;

[Guid("A4C46780-499F-101B-BB78-00AA00384CBB")]
[ComVisible(true)]
public interface IM1CrystalParameterCollection
{
	int Count { get; }

	[IndexerName("_Default")]
	[DispId(0)]
	CrystalParameter this[string name] { get; }

	int IndexOf(CrystalParameter value);

	CrystalParameter GetItem(int index);

	[DispId(-4)]
	IEnumerator<CrystalParameter> GetEnumerator();
}
