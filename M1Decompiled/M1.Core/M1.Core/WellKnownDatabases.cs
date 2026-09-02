using System.Runtime.InteropServices;

namespace M1.Core;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct WellKnownDatabases
{
	public const string DemoM1 = "ab5637f2-6f45-4831-baf7-f699b3841433";

	public const string BlankM1 = "cfdc0db6-0d3f-42c7-9683-54d26239bfaa";
}
