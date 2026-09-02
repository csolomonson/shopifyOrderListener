using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CrystalDecisions.ReportAppServer.DataDefModel;

[ComImport]
[CompilerGenerated]
[Guid("C64E46EC-405D-11D3-9169-00902741EE7C")]
[DefaultMember("Item")]
[TypeIdentifier]
public interface ISCRValues : IEnumerable
{
	void _VtblGap1_7();

	[DispId(1010)]
	int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1010)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1011)]
	void Add([In][MarshalAs(UnmanagedType.Struct)] object val);
}
