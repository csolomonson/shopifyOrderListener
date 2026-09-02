using System.Collections.Generic;

namespace M1.Core;

public class ByPassUI
{
	public List<string> Output { get; set; } = new List<string>();

	public List<string> ExceptionOutput { get; set; } = new List<string>();

	public bool Silent { get; set; }

	public string DefaultWarehouse { get; set; } = "W1";

	public string DefaultBin { get; set; } = "Bin1";
}
