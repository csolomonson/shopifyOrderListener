using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CrystalDecisions.ReportAppServer.ReportDefModel;

namespace CrystalDecisions.ReportAppServer.Controllers;

[ComImport]
[CompilerGenerated]
[Guid("1A20D6ED-DB48-45D7-A59F-E8B98B644EE0")]
[TypeIdentifier]
public interface ISCRReportSectionController
{
	void _VtblGap1_2();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	void SetProperty([In][MarshalAs(UnmanagedType.Interface)] Section Section, [In] CrReportSectionPropertyEnum Property, [In][MarshalAs(UnmanagedType.Struct)] object PropertyValue);
}
