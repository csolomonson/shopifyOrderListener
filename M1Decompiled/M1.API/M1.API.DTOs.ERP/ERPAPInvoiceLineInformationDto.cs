using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAPInvoiceLineInformationDto
{
	public string aplApInvoiceID { get; set; }

	public string aplAssetID { get; set; }

	public string aplAssetTypeID { get; set; }

	public decimal aplConversionFactor { get; set; }

	public string aplCreatedBy { get; set; }

	public DateTime? aplCreatedDate { get; set; }

	public string aplDmrClaimID { get; set; }

	public short aplDmrClaimLineID { get; set; }

	public string aplDmrShipmentID { get; set; }

	public short aplDmrShipmentLineID { get; set; }

	public Guid aplUniqueID { get; set; }

	public decimal aplExtendedCostBase { get; set; }

	public decimal aplExtendedCostForeign { get; set; }

	public byte aplForm1099Box { get; set; }

	public bool aplInvoicedComplete { get; set; }

	public bool aplPostedToGl { get; set; }

	public bool aplRetention { get; set; }

	public string aplItemType { get; set; }

	public int aplJobAssemblyID { get; set; }

	public string aplJobID { get; set; }

	public int aplJobMaterialID { get; set; }

	public int aplJobOperationID { get; set; }

	public byte aplJobType { get; set; }

	public short aplLandedCostChargeID { get; set; }

	public string aplLandedCostID { get; set; }

	public string aplNonTaxReasonID { get; set; }

	public string aplOrgPartID { get; set; }

	public string aplOrgPartShortDescription { get; set; }

	public string aplPartDescription { get; set; }

	public string aplPartID { get; set; }

	public string aplPartLongDescriptionRtf { get; set; }

	public string aplPartLongDescriptionText { get; set; }

	public string aplPartRevisionID { get; set; }

	public string aplProjectAreaID { get; set; }

	public string aplProjectID { get; set; }

	public string aplPurchaseOrderID { get; set; }

	public short aplPurchaseOrderLineID { get; set; }

	public decimal aplPurchaseQuantity { get; set; }

	public decimal aplPurchaseUnitCostBase { get; set; }

	public decimal aplPurchaseUnitCostForeign { get; set; }

	public string aplPurchaseUnitOfMeasure { get; set; }

	public string aplReceiptID { get; set; }

	public short aplReceiptLineID { get; set; }

	public decimal aplReceivedQuantity { get; set; }

	public string aplReceivedUnitOfMeasure { get; set; }

	public decimal aplRetentionAmountBase { get; set; }

	public decimal aplRetentionAmountForeign { get; set; }

	public decimal aplRetentionPercent { get; set; }

	public DateTime? aplRetentionReleaseDate { get; set; }

	public string aplRmaClaimID { get; set; }

	public short aplRmaClaimLineID { get; set; }

	public byte[] aplRowVersion { get; set; }

	public decimal aplSecondTaxAmountBase { get; set; }

	public decimal aplSecondTaxAmountForeign { get; set; }

	public string aplSecondTaxCodeID { get; set; }

	public short aplApInvoiceLineID { get; set; }

	public decimal aplSetupChargeBase { get; set; }

	public decimal aplSetupChargeForeign { get; set; }

	public decimal aplTaxAmountBase { get; set; }

	public decimal aplTaxAmountForeign { get; set; }

	public string aplTaxCodeID { get; set; }

	public decimal aplTotalExtendedCostBase { get; set; }

	public decimal aplTotalExtendedCostForeign { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
