using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CrystalDecisions.ReportAppServer.ReportDefModel;

[ComImport]
[CompilerGenerated]
[Guid("29F3A003-414A-11D3-9D7E-00902741EE80")]
[TypeIdentifier]
public interface ISCRSection
{
	void _VtblGap1_10();

	[DispId(4)]
	int Height
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[param: In]
		set;
	}

	[DispId(5)]
	SectionFormat Format
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	void _VtblGap2_2();

	[DispId(9)]
	int Width
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(9)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(9)]
		[param: In]
		set;
	}
}
