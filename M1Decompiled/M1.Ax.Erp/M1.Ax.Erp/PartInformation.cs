namespace M1.Ax.Erp;

public class PartInformation
{
	public string Part { get; set; }

	public string PartRevision { get; set; }

	public string PartWarehouse { get; set; }

	public string PartBin { get; set; }

	public bool IsSerialLotPart { get; set; }

	public bool HasNegativeQOH { get; set; }

	public bool IsBinInactive { get; set; }
}
