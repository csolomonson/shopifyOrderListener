using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CrystalDecisions.ReportAppServer.DataDefModel;
using CrystalDecisions.ReportAppServer.ReportDefModel;

namespace CrystalDecisions.ReportAppServer.Controllers;

[ComImport]
[CompilerGenerated]
[Guid("2359E4AA-1CE2-11D5-9E58-00902741EE80")]
[TypeIdentifier]
public interface ISCRSubreportController
{
	void _VtblGap1_9();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(21)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Database GetSubreportDatabase([In][MarshalAs(UnmanagedType.BStr)] string SubreportName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(22)]
	void SetTableLocation([In][MarshalAs(UnmanagedType.BStr)] string SubreportName, [In][MarshalAs(UnmanagedType.Interface)] ISCRTable CurTable, [In][MarshalAs(UnmanagedType.Interface)] ISCRTable NewTable);

	void _VtblGap2_1();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(24)]
	void SetDataSource([In][MarshalAs(UnmanagedType.BStr)] string SubreportName, [In][MarshalAs(UnmanagedType.IDispatch)] object DataSource, [In][MarshalAs(UnmanagedType.BStr)] string OldTableAlias = "", [In][MarshalAs(UnmanagedType.BStr)] string NewTableName = "");

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(25)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Strings GetSubreportNames();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(26)]
	[return: MarshalAs(UnmanagedType.Interface)]
	SubreportClientDocument GetSubreport([In][MarshalAs(UnmanagedType.BStr)] string Name);

	void _VtblGap3_2();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(29)]
	[return: MarshalAs(UnmanagedType.Interface)]
	SubreportLinks GetSubreportLinks([In][MarshalAs(UnmanagedType.BStr)] string SubreportName);
}
