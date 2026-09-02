using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPSalesOrderComponentInformationDto
{
	public decimal omoAdditionalQuantity { get; set; }

	public string omoCreatedBy { get; set; }

	public DateTime? omoCreatedDate { get; set; }

	public decimal omoDeliveryQuantity { get; set; }

	public string omoDescription { get; set; }

	public Guid omoUniqueID { get; set; }

	public bool omoClosed { get; set; }

	public bool omoShippedComplete { get; set; }

	public decimal omoParentQuantity { get; set; }

	public string omoPartBinID { get; set; }

	public string omoPartID { get; set; }

	public string omoPartRevisionID { get; set; }

	public string omoPartWarehouseLocationID { get; set; }

	public decimal omoQuantityAllocated { get; set; }

	public decimal omoQuantityPerParent { get; set; }

	public decimal omoQuantityShipped { get; set; }

	public byte[] omoRowVersion { get; set; }

	public short omoSalesOrderDeliveryID { get; set; }

	public string omoSalesOrderID { get; set; }

	public short omoSalesOrderLineID { get; set; }

	public short omoSalesOrderComponentID { get; set; }

	public string omoUnitOfMeasure { get; set; }

	public decimal omoWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
