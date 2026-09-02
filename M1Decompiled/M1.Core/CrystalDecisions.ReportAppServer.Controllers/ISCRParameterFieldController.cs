using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CrystalDecisions.ReportAppServer.DataDefModel;

namespace CrystalDecisions.ReportAppServer.Controllers;

[ComImport]
[CompilerGenerated]
[Guid("3AE3F2DC-D813-47F7-8184-C365EEE5CD45")]
[TypeIdentifier]
public interface ISCRParameterFieldController
{
	void _VtblGap1_5();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(51)]
	void SetCurrentValues([In][MarshalAs(UnmanagedType.BStr)] string ReportName, [In][MarshalAs(UnmanagedType.BStr)] string ParameterFieldName, [In][MarshalAs(UnmanagedType.Interface)] Values Values);

	void _VtblGap2_1();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(53)]
	void SetCurrentValue([In][MarshalAs(UnmanagedType.BStr)] string ReportName, [In][MarshalAs(UnmanagedType.BStr)] string fieldName, [In][MarshalAs(UnmanagedType.Struct)] object Value);
}
