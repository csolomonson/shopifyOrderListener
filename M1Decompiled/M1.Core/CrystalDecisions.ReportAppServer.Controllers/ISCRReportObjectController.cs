using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CrystalDecisions.ReportAppServer.ReportDefModel;

namespace CrystalDecisions.ReportAppServer.Controllers;

[ComImport]
[CompilerGenerated]
[Guid("B2865C7F-6544-4995-864B-6ECA50731013")]
[TypeIdentifier]
public interface ISCRReportObjectController
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1)]
	void Add([In][MarshalAs(UnmanagedType.Interface)] ISCRReportObject ReportObject, [In][MarshalAs(UnmanagedType.Interface)] Section Section, [In] int nIndex = -1);

	void _VtblGap1_1();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	void Modify([In][MarshalAs(UnmanagedType.Interface)] ISCRReportObject OldObject, [In][MarshalAs(UnmanagedType.Interface)] ISCRReportObject NewObject);

	void _VtblGap2_1();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5)]
	[return: MarshalAs(UnmanagedType.Interface)]
	ReportObjects GetReportObjectsByKind([In] CrReportObjectKindEnum nObjectKind);
}
