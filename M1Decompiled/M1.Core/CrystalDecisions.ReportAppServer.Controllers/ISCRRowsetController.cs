using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CrystalDecisions.ReportAppServer.DataDefModel;

namespace CrystalDecisions.ReportAppServer.Controllers;

[ComImport]
[CompilerGenerated]
[Guid("503575AD-433C-11D3-8C41-00A0C9E71919")]
[TypeIdentifier]
public interface ISCRRowsetController
{
	void _VtblGap1_8();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(18)]
	void Refresh();

	void _VtblGap2_23();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(42)]
	[return: MarshalAs(UnmanagedType.Interface)]
	RowsetCursor CreateCursor([In][MarshalAs(UnmanagedType.Interface)] ISCRGroupPath GroupPath, [Optional][In][MarshalAs(UnmanagedType.Interface)] RowsetMetaData MetaData, [In] int Reserved = 0);
}
