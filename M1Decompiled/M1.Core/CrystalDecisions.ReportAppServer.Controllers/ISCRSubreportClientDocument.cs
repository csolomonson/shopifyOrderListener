using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CrystalDecisions.ReportAppServer.Controllers;

[ComImport]
[CompilerGenerated]
[Guid("53AB8A82-8CB8-4E6E-A57B-F25C270E535A")]
[TypeIdentifier]
public interface ISCRSubreportClientDocument
{
	void _VtblGap1_4();

	[DispId(20)]
	DataDefController DataDefController
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(20)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap2_2();

	[DispId(23)]
	ReportDefController2 ReportDefController
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(23)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
