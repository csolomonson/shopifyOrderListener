using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CrystalDecisions.ReportAppServer.ReportDefModel;

namespace CrystalDecisions.ReportAppServer.Controllers;

[ComImport]
[CompilerGenerated]
[Guid("8B654221-CF28-4A26-9902-2226D56C8D60")]
[TypeIdentifier]
public interface ISCRReportDefController2 : ISCRReportDefController
{
	void _VtblGap1_4();

	[DispId(4)]
	ReportDefinition ReportDefinition
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	void _VtblGap2_14();

	[DispId(51)]
	ReportSectionController ReportSectionController
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(51)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(52)]
	ReportObjectController ReportObjectController
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(52)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
