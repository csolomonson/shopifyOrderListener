using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace M1.Core.Script;

[ComVisible(true)]
public interface IAx
{
	[IndexerName("_Default")]
	[DispId(0)]
	object this[string name] { get; }
}
