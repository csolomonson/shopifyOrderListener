using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace M1RepBitConv;

[ComImport]
[CompilerGenerated]
[Guid("AC71D181-83BA-4250-92E4-9AC325D0F56F")]
[TypeIdentifier]
public interface _App2
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809345)]
	void SetLogon([In][MarshalAs(UnmanagedType.BStr)] string cDBServer, [In][MarshalAs(UnmanagedType.BStr)] string cDBUserID, [In][MarshalAs(UnmanagedType.BStr)] string cDBPassword, [In] bool bTrustedConnection);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809346)]
	void Shutdown();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809347)]
	void AddBitField([In][MarshalAs(UnmanagedType.BStr)] string field);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809348)]
	void AddSource([In][MarshalAs(UnmanagedType.BStr)] string source);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809349)]
	void AddDest([In][MarshalAs(UnmanagedType.BStr)] string dest);

	void _VtblGap1_1();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809351)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string UpdateReport([In][MarshalAs(UnmanagedType.BStr)] string cCurrentFile, [In][MarshalAs(UnmanagedType.BStr)] string cBackupFolder);
}
