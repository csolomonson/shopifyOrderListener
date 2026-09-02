using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CrystalDecisions.ReportAppServer.Controllers;
using CrystalDecisions.ReportAppServer.DataDefModel;

namespace CrystalDecisions.ReportAppServer.ClientDoc;

[ComImport]
[CompilerGenerated]
[Guid("369D6214-71F0-11D3-9DA5-00902741EE80")]
[TypeIdentifier]
public interface ISCDReportClientDocument : ISCDClientDocument
{
	void _VtblGap1_37();

	[DispId(101)]
	DataDefController DataDefController
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(101)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(102)]
	DatabaseController DatabaseController
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(102)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(103)]
	RowsetController RowsetController
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(103)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap2_1();

	[DispId(105)]
	ReportDefController2 ReportDefController
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(105)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap3_2();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(112)]
	void VerifyDatabase();

	[DispId(113)]
	SummaryInfo SummaryInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(113)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(113)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	void _VtblGap4_20();

	[DispId(133)]
	SubreportController SubreportController
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(133)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(134)]
	SearchController SearchController
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(134)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
