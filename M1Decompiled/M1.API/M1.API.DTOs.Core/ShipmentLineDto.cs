using System.Collections.Generic;

namespace M1.API.DTOs.Core;

public class ShipmentLineDto
{
	public string ShipmentID { get; set; }

	public short ShipmentLineID { get; set; }

	public string SalesOrderID { get; set; }

	public short SalesOrderLineID { get; set; }

	public short SalesOrderDeliveryID { get; set; }

	public string PartID { get; set; }

	public string PartRevisionID { get; set; }

	public string OrgPartID { get; set; }

	public string OrgPartShortDescription { get; set; }

	public string UnitOfMeasure { get; set; }

	public decimal QuantityShipped { get; set; }

	public decimal Weight { get; set; }

	public string Description { get; set; }

	public decimal UnitPrice { get; set; }

	public IList<ShipmentPackageDetailsDto> ShipmentPackageDetails { get; set; }
}
