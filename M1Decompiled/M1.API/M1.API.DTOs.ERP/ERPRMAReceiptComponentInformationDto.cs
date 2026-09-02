using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPRMAReceiptComponentInformationDto
{
	public decimal rroAdditionalQuantity { get; set; }

	public string rroCreatedBy { get; set; }

	public DateTime? rroCreatedDate { get; set; }

	public string rroDescription { get; set; }

	public Guid rroUniqueID { get; set; }

	public decimal rroExtendedCost { get; set; }

	public decimal rroExtendedCostForeign { get; set; }

	public decimal rroInspParentQuantity { get; set; }

	public bool rroClosed { get; set; }

	public bool rroInspectionComplete { get; set; }

	public bool rroPosted { get; set; }

	public bool rroReceivedComplete { get; set; }

	public bool rroReversed { get; set; }

	public decimal rroParentQuantity { get; set; }

	public string rroPartBinID { get; set; }

	public string rroPartID { get; set; }

	public string rroPartRevisionID { get; set; }

	public string rroPartWarehouseLocationID { get; set; }

	public decimal rroQuantityPerParent { get; set; }

	public decimal rroQuantityReceived { get; set; }

	public decimal rroQuantityToInspect { get; set; }

	public int rroReverseRmaReceiptCompID { get; set; }

	public string rroReverseRmaReceiptID { get; set; }

	public short rroReverseRmaReceiptLineID { get; set; }

	public int rroRmaClaimComponentID { get; set; }

	public string rroRmaClaimID { get; set; }

	public short rroRmaClaimLineID { get; set; }

	public string rroRmaReceiptID { get; set; }

	public short rroRmaReceiptLineID { get; set; }

	public byte[] rroRowVersion { get; set; }

	public int rroRmaReceiptComponentID { get; set; }

	public decimal rroUnitCost { get; set; }

	public decimal rroUnitCostForeign { get; set; }

	public string rroUnitOfMeasure { get; set; }

	public decimal rroWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
