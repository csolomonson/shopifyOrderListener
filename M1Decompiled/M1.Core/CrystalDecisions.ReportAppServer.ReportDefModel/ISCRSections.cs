using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CrystalDecisions.ReportAppServer.ReportDefModel;

[ComImport]
[CompilerGenerated]
[Guid("F321EB96-4122-11D3-9D7C-00902741EE80")]
[TypeIdentifier]
public interface ISCRSections : IEnumerable
{
	void _VtblGap1_5();

	[DispId(0)]
	Section this[[In][MarshalAs(UnmanagedType.Struct)] object Index]
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}
}
