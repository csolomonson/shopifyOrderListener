using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CrystalDecisions.ReportAppServer.DataDefModel;

[ComImport]
[CompilerGenerated]
[Guid("86ACA0BE-3E06-11D3-9169-00902741EE7C")]
[TypeIdentifier]
public interface ISCRFormulaField : ISCRField
{
	void _VtblGap1_26();

	[DispId(19)]
	int UseCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(19)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(19)]
		[param: In]
		set;
	}

	void _VtblGap2_6();

	[DispId(100)]
	string Text
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(100)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(100)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(101)]
	CrFormulaSyntaxEnum Syntax
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(101)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(101)]
		[param: In]
		set;
	}
}
