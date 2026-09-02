using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CrystalDecisions.ReportAppServer.ReportDefModel;

[ComImport]
[CompilerGenerated]
[Guid("CBCB8D47-F606-4CA2-A26E-BB582637D62A")]
[TypeIdentifier]
public interface ISCRSubreportLink
{
	void _VtblGap1_8();

	[DispId(3)]
	string LinkedParameterName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}
}
