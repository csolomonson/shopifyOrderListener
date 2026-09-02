namespace M1.API.DTOs.Core;

public class ShipmentPackageDetailsDto
{
	public string ShipmentID { get; set; }

	public short ShipmentLineID { get; set; }

	public int ShipmentPackageID { get; set; }

	public decimal Quantity { get; set; }

	public decimal Weight { get; set; }

	public string CountryOfManufacture { get; set; }
}
