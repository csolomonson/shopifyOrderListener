using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CrystalDecisions.ReportAppServer.DataDefModel;

[ComImport]
[CompilerGenerated]
[Guid("FD51A72A-3F89-11D3-A682-000000000000")]
[TypeIdentifier]
public interface ISCRTables : IEnumerable
{
	void _VtblGap1_5();

	[DispId(0)]
	ISCRTable this[[In] int Index]
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(1010)]
	int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1010)]
		get;
	}
}
