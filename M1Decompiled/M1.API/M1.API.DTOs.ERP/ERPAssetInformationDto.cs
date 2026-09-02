using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAssetInformationDto
{
	public string fapApInvoiceID { get; set; }

	public short fapApInvoiceLineID { get; set; }

	public string fapAssetTypeID { get; set; }

	public DateTime? fapBookDepreciationEndDate { get; set; }

	public decimal fapBookDepreciationRate { get; set; }

	public decimal fapBookEffectiveLife { get; set; }

	public decimal fapBookStartValue { get; set; }

	public string fapAssetID { get; set; }

	public string fapCreatedBy { get; set; }

	public DateTime? fapCreatedDate { get; set; }

	public decimal fapDeemedValue { get; set; }

	public decimal fapDepreciationLimit { get; set; }

	public DateTime? fapDepreciationStartDate { get; set; }

	public string fapDescription { get; set; }

	public DateTime? fapDisposalDate { get; set; }

	public decimal fapDisposalValue { get; set; }

	public Guid fapUniqueID { get; set; }

	public int fapEstimatedProductionUnits { get; set; }

	public string fapFinanceOrganizationID { get; set; }

	public DateTime? fapInServiceDate { get; set; }

	public bool fapLowCostAsset { get; set; }

	public bool fapLowValueAssetInPool { get; set; }

	public string fapItemType { get; set; }

	public DateTime? fapLeaseExpiryDate { get; set; }

	public short fapLeaseMonths { get; set; }

	public string fapLocation { get; set; }

	public string fapLongDescriptionRtf { get; set; }

	public string fapLongDescriptionText { get; set; }

	public decimal fapPaymentAmount { get; set; }

	public string fapPlantID { get; set; }

	public DateTime? fapPurchaseDate { get; set; }

	public string fapPurchaseOrderID { get; set; }

	public short fapPurchaseOrderLineID { get; set; }

	public string fapPurchaseType { get; set; }

	public decimal fapPurchaseValue { get; set; }

	public int fapQuantity { get; set; }

	public DateTime? fapReceiptDate { get; set; }

	public string fapReceiptID { get; set; }

	public short fapReceiptLineID { get; set; }

	public decimal fapResidualAmount { get; set; }

	public byte[] fapRowVersion { get; set; }

	public string fapSerialNumber { get; set; }

	public short fapStartYearInPool { get; set; }

	public string fapStatus { get; set; }

	public string fapSupplierOrganizationID { get; set; }

	public decimal fapTaxableUsePercentage { get; set; }

	public DateTime? fapTaxDepreciationEndDate { get; set; }

	public decimal fapTaxDepreciationRate { get; set; }

	public decimal fapTaxEffectiveLife { get; set; }

	public decimal fapTaxStartValue { get; set; }

	public string fapWorkCenterID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
