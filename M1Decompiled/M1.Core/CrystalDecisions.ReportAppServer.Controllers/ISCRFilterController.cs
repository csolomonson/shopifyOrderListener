using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CrystalDecisions.ReportAppServer.Controllers;

[ComImport]
[CompilerGenerated]
[Guid("3AC68B80-BA24-41D3-877E-AC4874E57935")]
[TypeIdentifier]
public interface ISCRFilterController
{
	void _VtblGap1_8();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(9)]
	void SetFormulaText([In][MarshalAs(UnmanagedType.BStr)] string newText);
}
