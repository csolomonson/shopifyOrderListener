using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CrystalDecisions.ReportAppServer.ReportDefModel;

[ComImport]
[CompilerGenerated]
[Guid("46927657-EC14-4A77-9122-A88D976FF40E")]
[TypeIdentifier]
public interface ISCRBorder
{
	void _VtblGap1_12();

	[DispId(5)]
	bool HasDropShadow
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[param: In]
		set;
	}
}
