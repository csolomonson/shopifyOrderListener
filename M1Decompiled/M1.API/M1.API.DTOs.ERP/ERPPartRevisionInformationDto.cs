using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartRevisionInformationDto
{
	public decimal imrAverageDutyCost { get; set; }

	public decimal imrAverageFreightCost { get; set; }

	public decimal imrAverageLaborCost { get; set; }

	public decimal imrAverageMaterialCost { get; set; }

	public decimal imrAverageMiscCost { get; set; }

	public decimal imrAverageOverheadCost { get; set; }

	public decimal imrAverageSubcontractCost { get; set; }

	public decimal imrBarLength { get; set; }

	public DateTime? imrBlanketPeriodBegin { get; set; }

	public DateTime? imrBlanketPeriodEnd { get; set; }

	public string imrPartRevisionID { get; set; }

	public string imrCommodityCode { get; set; }

	public string imrCommodityDescription { get; set; }

	public decimal imrConversionFactor { get; set; }

	public string imrCountryOfManufacture { get; set; }

	public string imrCreatedBy { get; set; }

	public DateTime? imrCreatedDate { get; set; }

	public string imrDocuments { get; set; }

	public DateTime? imrEffectiveEndDate { get; set; }

	public DateTime? imrEffectiveStartDate { get; set; }

	public Guid imrUniqueID { get; set; }

	public decimal imrExpenseSplitPercentTotal { get; set; }

	public decimal imrFdxHandlingCost { get; set; }

	public int imrFdxPackageHeight { get; set; }

	public int imrFdxPackageLength { get; set; }

	public int imrFdxPackageWidth { get; set; }

	public string imrFdxPackaging { get; set; }

	public decimal imrFdxPackagingCost { get; set; }

	public decimal imrFdxShipCostMarkupPct { get; set; }

	public string imrFormID { get; set; }

	public string imrInspectionNotesRTF { get; set; }

	public string imrInspectionNotesText { get; set; }

	public string imrInventoryUnitOfMeasure { get; set; }

	public bool imrInactive { get; set; }

	public bool imrConfigured { get; set; }

	public bool imrFdxNonstandardContainer { get; set; }

	public bool imrFdxOneItemPerShipment { get; set; }

	public bool imrPreferredRefExists { get; set; }

	public bool imrPurchasableItem { get; set; }

	public bool imrSuppressShortDescription { get; set; }

	public bool imrUseQuotePrice { get; set; }

	public decimal imrLastDutyCost { get; set; }

	public decimal imrLastFreightCost { get; set; }

	public decimal imrLastLaborCost { get; set; }

	public decimal imrLastMaterialCost { get; set; }

	public decimal imrLastMiscCost { get; set; }

	public decimal imrLastOverheadCost { get; set; }

	public DateTime? imrLastReceiptDate { get; set; }

	public DateTime? imrLastRunDatePurchasePlanner { get; set; }

	public decimal imrLastSubcontractCost { get; set; }

	public DateTime? imrLastTransactionDate { get; set; }

	public short imrLeadTime { get; set; }

	public string imrLongDescriptionHtml { get; set; }

	public string imrLongDescriptionRtf { get; set; }

	public string imrLongDescriptionText { get; set; }

	public decimal imrManufacturingLotSize { get; set; }

	public decimal imrMaximumQuantity { get; set; }

	public decimal imrMinimumQuantity { get; set; }

	public DateTime? imrNetCostBeginDate { get; set; }

	public string imrNetCostCode { get; set; }

	public DateTime? imrNetCostEndDate { get; set; }

	public string imrPartID { get; set; }

	public string imrPartImageFileName { get; set; }

	public string imrPreferenceCriteria { get; set; }

	public string imrProducerDetermination { get; set; }

	public string imrProductCategoryID { get; set; }

	public short imrProductCategoryLineID { get; set; }

	public string imrProductionNotesRTF { get; set; }

	public string imrProductionNotesText { get; set; }

	public string imrPurchaseLocationID { get; set; }

	public string imrPurchaseUnitOfMeasure { get; set; }

	public decimal imrQuantityAllocated { get; set; }

	public decimal imrQuantityOnHand { get; set; }

	public decimal imrQuantityOnOrderPurchases { get; set; }

	public decimal imrQuantityOnOrderSales { get; set; }

	public decimal imrQuantityToInspect { get; set; }

	public decimal imrQuantityToReturn { get; set; }

	public decimal imrQuantityToReturnJob { get; set; }

	public byte imrRequiresInspection { get; set; }

	public byte[] imrRowVersion { get; set; }

	public decimal imrSheetSizeX { get; set; }

	public decimal imrSheetSizeY { get; set; }

	public string imrShortDescription { get; set; }

	public string imrSourceMethodID { get; set; }

	public string imrSourceRevisionID { get; set; }

	public decimal imrStandardDutyCost { get; set; }

	public decimal imrStandardFreightCost { get; set; }

	public decimal imrStandardLaborCost { get; set; }

	public decimal imrStandardMaterialCost { get; set; }

	public decimal imrStandardMiscCost { get; set; }

	public decimal imrStandardOverheadCost { get; set; }

	public decimal imrStandardSubcontractCost { get; set; }

	public string imrSupplierOrganizationID { get; set; }

	public decimal imrThickness { get; set; }

	public string imrUniversalProductCode { get; set; }

	public decimal imrVolume { get; set; }

	public decimal imrWeight { get; set; }

	public string imrWeightUnitOfMeasure { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
