using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CrystalDecisions.ReportAppServer.DataDefModel;

[ComImport]
[CompilerGenerated]
[Guid("BE3B38F8-6819-4DBC-BADB-F223BE1485B9")]
[TypeIdentifier]
public interface ISCRAlert
{
	void _VtblGap1_6();

	[DispId(2)]
	bool Enable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[param: In]
		set;
	}

	void _VtblGap2_2();

	[DispId(4)]
	string Message
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}
}
