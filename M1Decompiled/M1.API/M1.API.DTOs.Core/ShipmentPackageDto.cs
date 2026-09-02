namespace M1.API.DTOs.Core;

public class ShipmentPackageDto
{
	public string ShipmentID { get; set; }

	public int ShipmentPackageID { get; set; }

	public string UPSPackageTypes { get; set; }

	public string FedExPackageTypes { get; set; }

	public string PackageWeightUOM { get; set; }

	public string UserDefinedLabel { get; set; }
}
