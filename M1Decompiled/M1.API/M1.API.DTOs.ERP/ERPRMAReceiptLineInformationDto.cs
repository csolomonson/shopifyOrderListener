using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPRMAReceiptLineInformationDto
{
	public decimal rrlConversionFactor { get; set; }

	public string rrlCreatedBy { get; set; }

	public DateTime? rrlCreatedDate { get; set; }

	public string rrlDescription { get; set; }

	public Guid rrlUniqueID { get; set; }

	public decimal rrlExtendedCost { get; set; }

	public decimal rrlExtendedCostForeign { get; set; }

	public string rrlHeatLot { get; set; }

	public decimal rrlInventoryQuantityReceived { get; set; }

	public string rrlInventoryUnitOfMeasure { get; set; }

	public bool rrlClosed { get; set; }

	public bool rrlInInspection { get; set; }

	public bool rrlInspectionComplete { get; set; }

	public bool rrlInvoicedComplete { get; set; }

	public bool rrlKitPart { get; set; }

	public bool rrlPosted { get; set; }

	public bool rrlReceivedComplete { get; set; }

	public bool rrlRequiresInspection { get; set; }

	public bool rrlReversed { get; set; }

	public string rrlOrgPartID { get; set; }

	public string rrlOrgPartShortDescription { get; set; }

	public string rrlPartBinID { get; set; }

	public string rrlPartID { get; set; }

	public string rrlPartLongDescriptionRtf { get; set; }

	public string rrlPartLongDescriptionText { get; set; }

	public string rrlPartRevisionID { get; set; }

	public string rrlPartWarehouseLocationID { get; set; }

	public string rrlProjectAreaID { get; set; }

	public string rrlProjectID { get; set; }

	public decimal rrlQuantityToInspect { get; set; }

	public string rrlReference { get; set; }

	public string rrlReverseRmaReceiptID { get; set; }

	public short rrlReverseRmaReceiptLineID { get; set; }

	public string rrlRmaClaimID { get; set; }

	public short rrlRmaClaimLineID { get; set; }

	public decimal rrlRmaClaimQuantity { get; set; }

	public decimal rrlRmaOpenQuantity { get; set; }

	public string rrlRmaReceiptID { get; set; }

	public byte[] rrlRowVersion { get; set; }

	public decimal rrlSalesQuantityReceived { get; set; }

	public string rrlSalesUnitOfMeasure { get; set; }

	public short rrlRmaReceiptLineID { get; set; }

	public decimal rrlTotalComponentCosts { get; set; }

	public decimal rrlUnitCost { get; set; }

	public decimal rrlUnitCostForeign { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
