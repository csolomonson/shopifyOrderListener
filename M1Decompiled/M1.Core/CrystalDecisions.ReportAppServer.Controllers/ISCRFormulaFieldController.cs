using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CrystalDecisions.ReportAppServer.DataDefModel;

namespace CrystalDecisions.ReportAppServer.Controllers;

[ComImport]
[CompilerGenerated]
[Guid("88DF5674-092A-4D40-8A48-193B3D4790D7")]
[TypeIdentifier]
public interface ISCRFormulaFieldController
{
	void _VtblGap1_1();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2)]
	void Remove([In][MarshalAs(UnmanagedType.Struct)] object FormulaField);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	void Modify([In][MarshalAs(UnmanagedType.Struct)] object OldFormulaField, [In][MarshalAs(UnmanagedType.Interface)] FormulaField NewFormulaField);
}
