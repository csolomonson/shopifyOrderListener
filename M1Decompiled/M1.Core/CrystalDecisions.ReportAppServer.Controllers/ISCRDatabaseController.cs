using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CrystalDecisions.ReportAppServer.DataDefModel;

namespace CrystalDecisions.ReportAppServer.Controllers;

[ComImport]
[CompilerGenerated]
[Guid("58486784-55AA-4139-BCA1-F4710B199E31")]
[TypeIdentifier]
public interface ISCRDatabaseController
{
	void _VtblGap1_2();

	[DispId(2)]
	Database Database
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	void _VtblGap2_22();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(42)]
	[return: MarshalAs(UnmanagedType.Interface)]
	ConnectionInfos GetConnectionInfos([Optional][In][MarshalAs(UnmanagedType.Interface)] PropertyBag PromptProperties);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(43)]
	void SetConnectionInfos([In][MarshalAs(UnmanagedType.Interface)] ConnectionInfos ConnectionInfos);

	void _VtblGap3_9();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(53)]
	void ReplaceConnection([In][MarshalAs(UnmanagedType.Struct)] object oldConnection, [In][MarshalAs(UnmanagedType.Struct)] object newConnection, [In][MarshalAs(UnmanagedType.Struct)] object parameterFields, [Optional][In][MarshalAs(UnmanagedType.Struct)] object crDBOptionUseDefault);
}
