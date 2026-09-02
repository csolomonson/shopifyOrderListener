using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace M1.Core.Report;

[ComVisible(true)]
public class CrystalFieldOption
{
	public string Operator = "=";

	public string Filter = string.Empty;

	public string[] DefaultValueExpressions;

	public List<object[]> Values = new List<object[]>();
}
