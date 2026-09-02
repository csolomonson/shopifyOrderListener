using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPurchaseOrderLineInformationDto
{
	public string pmlAssetID { get; set; }

	public string pmlAssetTypeID { get; set; }

	public decimal pmlConversionFactor { get; set; }

	public string pmlCreatedBy { get; set; }

	public DateTime? pmlCreatedDate { get; set; }

	public string pmlDmrClaimID { get; set; }

	public short pmlDmrClaimLineID { get; set; }

	public string pmlDocuments { get; set; }

	public DateTime? pmlDueDate { get; set; }

	public Guid pmlUniqueID { get; set; }

	public decimal pmlExpenseSplitPercentTotal { get; set; }

	public decimal pmlExtendedCostBase { get; set; }

	public decimal pmlExtendedCostForeign { get; set; }

	public byte pmlForm1099Box { get; set; }

	public decimal pmlInventoryQuantity { get; set; }

	public decimal pmlInventoryQuantityReceived { get; set; }

	public string pmlInventoryUnitOfMeasure { get; set; }

	public bool pmlClosed { get; set; }

	public bool pmlCreateJobSeq { get; set; }

	public bool pmlIntraCompanyPosted { get; set; }

	public bool pmlInTransit { get; set; }

	public bool pmlInTransitJournalsCreated { get; set; }

	public bool pmlInvoicedComplete { get; set; }

	public bool pmlKitPart { get; set; }

	public bool pmlPlanned { get; set; }

	public bool pmlPriceOverride { get; set; }

	public bool pmlReceivedComplete { get; set; }

	public bool pmlRequiresInspection { get; set; }

	public bool pmlSupplierRequirement { get; set; }

	public bool pmlTaxable { get; set; }

	public string pmlItemType { get; set; }

	public int pmlJobAssemblyID { get; set; }

	public string pmlJobID { get; set; }

	public int pmlJobMaterialID { get; set; }

	public decimal pmlJobOpenQuantity { get; set; }

	public int pmlJobOperationID { get; set; }

	public byte pmlJobType { get; set; }

	public string pmlLandedCostID { get; set; }

	public short pmlLeadTime { get; set; }

	public string pmlNonTaxReasonID { get; set; }

	public string pmlOrgPartID { get; set; }

	public string pmlOrgPartShortDescription { get; set; }

	public string pmlPartBinID { get; set; }

	public string pmlPartID { get; set; }

	public string pmlPartLongDescriptionRtf { get; set; }

	public string pmlPartLongDescriptionText { get; set; }

	public string pmlPartRevisionID { get; set; }

	public string pmlPartShortDescription { get; set; }

	public string pmlPartWarehouseLocationID { get; set; }

	public string pmlProcessID { get; set; }

	public string pmlProjectAreaID { get; set; }

	public string pmlProjectID { get; set; }

	public string pmlPurchaseOrderID { get; set; }

	public decimal pmlPurchaseQuantity { get; set; }

	public decimal pmlPurchaseQuantityReceived { get; set; }

	public byte pmlPurchaseType { get; set; }

	public decimal pmlPurchaseUnitCostBase { get; set; }

	public decimal pmlPurchaseUnitCostForeign { get; set; }

	public string pmlPurchaseUnitOfMeasure { get; set; }

	public decimal pmlQuantityOnOrder { get; set; }

	public string pmlRfqID { get; set; }

	public short pmlRfqLineID { get; set; }

	public string pmlRmaClaimID { get; set; }

	public short pmlRmaClaimLineID { get; set; }

	public byte[] pmlRowVersion { get; set; }

	public short pmlSalesOrderDeliveryID { get; set; }

	public string pmlSalesOrderID { get; set; }

	public short pmlSalesOrderLineID { get; set; }

	public decimal pmlSecondTaxAmountBase { get; set; }

	public decimal pmlSecondTaxAmountForeign { get; set; }

	public string pmlSecondTaxCodeID { get; set; }

	public short pmlPurchaseOrderLineID { get; set; }

	public decimal pmlSetupChargeBase { get; set; }

	public decimal pmlSetupChargeForeign { get; set; }

	public string pmlSourcePurchaseOrderID { get; set; }

	public short pmlSourcePurchaseOrderLineID { get; set; }

	public string pmlSourceTableName { get; set; }

	public Guid pmlSourceTableUniqueID { get; set; }

	public decimal pmlTaxAmountBase { get; set; }

	public decimal pmlTaxAmountForeign { get; set; }

	public string pmlTaxCodeID { get; set; }

	public decimal pmlTotalComponentCosts { get; set; }

	public decimal pmlTotalExtendedCostBase { get; set; }

	public decimal pmlTotalExtendedCostForeign { get; set; }

	public string pmlTrackingNumber { get; set; }

	public string pmlWorkCenterID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
