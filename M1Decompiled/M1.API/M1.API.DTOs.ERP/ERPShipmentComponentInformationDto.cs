using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPShipmentComponentInformationDto
{
	public decimal smoAdditionalQuantity { get; set; }

	public string smoCreatedBy { get; set; }

	public DateTime? smoCreatedDate { get; set; }

	public string smoDescription { get; set; }

	public Guid smoUniqueID { get; set; }

	public bool smoClosed { get; set; }

	public bool smoPostedToGl { get; set; }

	public bool smoReversed { get; set; }

	public bool smoShippedComplete { get; set; }

	public string smoJobID { get; set; }

	public decimal smoJobParentQuantity { get; set; }

	public decimal smoJobQuantityShipped { get; set; }

	public decimal smoParentQuantity { get; set; }

	public string smoPartBinID { get; set; }

	public string smoPartID { get; set; }

	public string smoPartRevisionID { get; set; }

	public string smoPartWarehouseLocationID { get; set; }

	public decimal smoQuantityPerParent { get; set; }

	public decimal smoQuantityShipped { get; set; }

	public short smoReverseShipmentComponentID { get; set; }

	public string smoReverseShipmentID { get; set; }

	public short smoReverseShipmentLineID { get; set; }

	public byte[] smoRowVersion { get; set; }

	public short smoSalesOrderComponentID { get; set; }

	public short smoSalesOrderDeliveryID { get; set; }

	public string smoSalesOrderID { get; set; }

	public short smoSalesOrderLineID { get; set; }

	public short smoShipmentComponentID { get; set; }

	public string smoShipmentID { get; set; }

	public short smoShipmentLineID { get; set; }

	public string smoSourceTableName { get; set; }

	public Guid smoSourceTableUniqueID { get; set; }

	public string smoUnitOfMeasure { get; set; }

	public decimal smoWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
