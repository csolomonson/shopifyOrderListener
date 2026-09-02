using System;

namespace M1.API.DTOs.Custom;

public class PartRevisionInformationDto
{
	public string PartID { get; set; }

	public string PartRevisionID { get; set; }

	public string InventoryUnitOfMeasure { get; set; }

	public decimal Weight { get; set; }

	public string EasyOrderPartID { get; set; }

	public DateTime? BlanketPeriodBegin { get; set; }

	public DateTime? BlanketPeriodEnd { get; set; }

	public DateTime? NetCostBeginDate { get; set; }

	public DateTime? NetCostEndDate { get; set; }

	public string NetCostCode { get; set; }

	public string PreferenceCriteria { get; set; }

	public string ProducerDetermination { get; set; }

	public string CommodityCode { get; set; }

	public string CountryOfManufacture { get; set; }

	public string PartShortDescription { get; set; }

	public string PartLongDescriptionText { get; set; }

	public string WeightUnitOfMeasure { get; set; }

	public string PurchaseUnitOfMeasure { get; set; }

	public string SupplierOrganizationID { get; set; }

	public decimal ConversionFactor { get; set; }

	public DateTime? EffectiveStartDate { get; set; }

	public string CreatedBy { get; set; }

	public DateTime? CreatedDate { get; set; }

	public int LeadTime { get; set; }

	public DateTime? EffectiveEndDate { get; set; }

	public string PurchaseLocationId { get; set; }

	public decimal AverageLaborCost { get; set; }

	public decimal AverageOverheadCost { get; set; }

	public decimal AverageMaterialCost { get; set; }

	public decimal AverageSubcontractCost { get; set; }

	public decimal LastLaborCost { get; set; }

	public decimal LastOverheadCost { get; set; }

	public decimal LastMaterialCost { get; set; }

	public decimal LastSubcontractCost { get; set; }

	public decimal StandardLaborCost { get; set; }

	public decimal StandardOverheadCost { get; set; }

	public decimal StandardMaterialCost { get; set; }

	public decimal StandardSubcontractCost { get; set; }

	public decimal AverageDutyCost { get; set; }

	public decimal AverageFreightCost { get; set; }

	public decimal AverageMiscCost { get; set; }

	public decimal LastDutyCost { get; set; }

	public decimal LastFreightCost { get; set; }

	public decimal LastMiscCost { get; set; }

	public decimal StandardDutyCost { get; set; }

	public decimal StandardFreightCost { get; set; }

	public decimal StandardMiscCost { get; set; }

	public DateTime? LastTransactionDate { get; set; }

	public decimal? ManufacturingLotSize { get; set; }

	public bool? Inactive { get; set; }

	public DateTime? LastReceiptDate { get; set; }

	public bool? RequiresInspection { get; set; }

	public decimal? ExpenseSplitPercentTotal { get; set; }

	public decimal? AverageUnitCost { get; set; }

	public decimal? StandardUnitCost { get; set; }

	public decimal? LastUnitCost { get; set; }

	public decimal? SheetSizeX { get; set; }

	public decimal? SheetSizeY { get; set; }

	public decimal? BarLength { get; set; }

	public decimal? Thickness { get; set; }
}
