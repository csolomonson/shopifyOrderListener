using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CrystalDecisions.ReportAppServer.DataDefModel;

[ComImport]
[CompilerGenerated]
[Guid("83B3E945-3ABB-4C4A-B126-2C02395C5075")]
[TypeIdentifier]
public interface ISCRParameterFieldDiscreteValue : ISCRParameterFieldValue
{
	void _VtblGap1_18();

	[DispId(200)]
	object Value
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(200)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(200)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Struct)]
		set;
	}
}
