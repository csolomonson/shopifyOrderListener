using System.Runtime.InteropServices;

namespace M1.Ax.Erp;

[ComVisible(true)]
public class YearAndPeriod
{
	public short Year;

	public byte Period;

	public string Message = string.Empty;

	public bool Success;
}
