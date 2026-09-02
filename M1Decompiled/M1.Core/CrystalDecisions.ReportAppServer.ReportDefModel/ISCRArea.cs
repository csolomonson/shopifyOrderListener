using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CrystalDecisions.ReportAppServer.ReportDefModel;

[ComImport]
[CompilerGenerated]
[Guid("F321EB77-4122-11D3-9D7C-00902741EE80")]
[TypeIdentifier]
public interface ISCRArea
{
	void _VtblGap1_4();

	[DispId(1)]
	Sections Sections
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}
}
