using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPRMAClaimComponentInformationDto
{
	public decimal raoAdditionalQuantity { get; set; }

	public string raoCreatedBy { get; set; }

	public DateTime? raoCreatedDate { get; set; }

	public string raoDescription { get; set; }

	public Guid raoUniqueID { get; set; }

	public bool raoReceivedComplete { get; set; }

	public decimal raoParentQuantity { get; set; }

	public string raoPartBinID { get; set; }

	public string raoPartID { get; set; }

	public string raoPartRevisionID { get; set; }

	public string raoPartWarehouseLocationID { get; set; }

	public decimal raoQuantity { get; set; }

	public decimal raoQuantityPerParent { get; set; }

	public decimal raoQuantityReceived { get; set; }

	public string raoRmaClaimID { get; set; }

	public short raoRmaClaimLineID { get; set; }

	public byte[] raoRowVersion { get; set; }

	public int raoRmaClaimComponentID { get; set; }

	public short raoShipmentComponentID { get; set; }

	public string raoShipmentID { get; set; }

	public short raoShipmentLineID { get; set; }

	public string raoUnitOfMeasure { get; set; }

	public decimal raoWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
