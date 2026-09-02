using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[DataContract(Namespace = "", Name = "partRevision")]
[XmlRoot(ElementName = "partRevision")]
[XmlType(AnonymousType = true)]
public class BOMPartRevisionDto
{
	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 1)]
	[Required(ErrorMessage = "PartID is invalid or empty.")]
	public string PartID { get; set; } = string.Empty;

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 2)]
	public string PartRevisionID { get; set; } = string.Empty;

	[XmlElement(ElementName = "shortDescription")]
	[DataMember(Name = "shortDescription", Order = 3)]
	[Required(ErrorMessage = "ShortDescription is invalid or empty.")]
	public string ShortDescription { get; set; } = string.Empty;

	[XmlElement(ElementName = "inventoryUnitOfMeasure")]
	[DataMember(Name = "inventoryUnitOfMeasure", Order = 4)]
	public string InventoryUnitOfMeasure { get; set; } = string.Empty;

	[XmlElement(ElementName = "purchaseUnitOfMeasure")]
	[DataMember(Name = "purchaseUnitOfMeasure", Order = 5)]
	public string PurchaseUnitOfMeasure { get; set; } = string.Empty;

	[XmlElement(ElementName = "supplierOrganizationID")]
	[DataMember(Name = "supplierOrganizationID", Order = 6)]
	public string SupplierOrganizationID { get; set; } = string.Empty;

	[XmlElement(ElementName = "conversionFactor")]
	[DataMember(Name = "conversionFactor", Order = 7)]
	public decimal ConversionFactor { get; set; }

	[XmlElement(ElementName = "effectiveStartDate")]
	[DataMember(Name = "effectiveStartDate", Order = 8)]
	public DateTime? EffectiveStartDate { get; set; }

	[XmlElement(ElementName = "leadTime")]
	[DataMember(Name = "leadTime", Order = 9)]
	public int LeadTime { get; set; }

	[XmlElement(ElementName = "effectiveEndDate")]
	[DataMember(Name = "effectiveEndDate", Order = 10)]
	public DateTime? EffectiveEndDate { get; set; }

	[XmlElement(ElementName = "purchaseLocationID")]
	[DataMember(Name = "purchaseLocationID", Order = 11)]
	public string PurchaseLocationId { get; set; }

	[XmlElement(ElementName = "averageLaborCost")]
	[DataMember(Name = "averageLaborCost", Order = 12)]
	public decimal? AverageLaborCost { get; set; }

	[XmlElement(ElementName = "averageOverheadCost")]
	[DataMember(Name = "averageOverheadCost", Order = 13)]
	public decimal? AverageOverheadCost { get; set; }

	[XmlElement(ElementName = "averageMaterialCost")]
	[DataMember(Name = "averageMaterialCost", Order = 14)]
	public decimal? AverageMaterialCost { get; set; }

	[XmlElement(ElementName = "averageSubcontractCost")]
	[DataMember(Name = "averageSubcontractCost", Order = 15)]
	public decimal? AverageSubcontractCost { get; set; }

	[XmlElement(ElementName = "lastLaborCost")]
	[DataMember(Name = "lastLaborCost", Order = 16)]
	public decimal? LastLaborCost { get; set; }

	[XmlElement(ElementName = "lastOverheadCost")]
	[DataMember(Name = "lastOverheadCost", Order = 17)]
	public decimal? LastOverheadCost { get; set; }

	[XmlElement(ElementName = "lastMaterialCost")]
	[DataMember(Name = "lastMaterialCost", Order = 18)]
	public decimal? LastMaterialCost { get; set; }

	[XmlElement(ElementName = "lastSubcontractCost")]
	[DataMember(Name = "lastSubcontractCost", Order = 19)]
	public decimal? LastSubcontractCost { get; set; }

	[XmlElement(ElementName = "standardLaborCost")]
	[DataMember(Name = "standardLaborCost", Order = 20)]
	public decimal? StandardLaborCost { get; set; }

	[XmlElement(ElementName = "standardOverheadCost")]
	[DataMember(Name = "standardOverheadCost", Order = 21)]
	public decimal? StandardOverheadCost { get; set; }

	[XmlElement(ElementName = "standardMaterialCost")]
	[DataMember(Name = "standardMaterialCost", Order = 22)]
	public decimal? StandardMaterialCost { get; set; }

	[XmlElement(ElementName = "standardSubcontractCost")]
	[DataMember(Name = "standardSubcontractCost", Order = 23)]
	public decimal? StandardSubcontractCost { get; set; }

	[XmlElement(ElementName = "averageDutyCost")]
	[DataMember(Name = "averageDutyCost", Order = 24)]
	public decimal? AverageDutyCost { get; set; }

	[XmlElement(ElementName = "averageFreightCost")]
	[DataMember(Name = "averageFreightCost", Order = 25)]
	public decimal? AverageFreightCost { get; set; }

	[XmlElement(ElementName = "averageMiscCost")]
	[DataMember(Name = "averageMiscCost", Order = 26)]
	public decimal? AverageMiscCost { get; set; }

	[XmlElement(ElementName = "lastDutyCost")]
	[DataMember(Name = "lastDutyCost", Order = 27)]
	public decimal? LastDutyCost { get; set; }

	[XmlElement(ElementName = "lastFreightCost")]
	[DataMember(Name = "lastFreightCost", Order = 28)]
	public decimal? LastFreightCost { get; set; }

	[XmlElement(ElementName = "lastMiscCost")]
	[DataMember(Name = "lastMiscCost", Order = 29)]
	public decimal? LastMiscCost { get; set; }

	[XmlElement(ElementName = "standardDutyCost")]
	[DataMember(Name = "standardDutyCost", Order = 30)]
	public decimal? StandardDutyCost { get; set; }

	[XmlElement(ElementName = "standardFreightCost")]
	[DataMember(Name = "standardFreightCost", Order = 31)]
	public decimal? StandardFreightCost { get; set; }

	[XmlElement(ElementName = "standardMiscCost")]
	[DataMember(Name = "standardMiscCost", Order = 32)]
	public decimal? StandardMiscCost { get; set; }

	[XmlElement(ElementName = "longDescription")]
	[DataMember(Name = "longDescription", Order = 33)]
	public string LongDescription { get; set; } = string.Empty;

	[XmlElement(ElementName = "lastTransactionDate")]
	[DataMember(Name = "lastTransactionDate", Order = 34)]
	public DateTime? LastTransactionDate { get; set; }

	[XmlElement(ElementName = "manufacturingLotSize")]
	[DataMember(Name = "manufacturingLotSize", Order = 35)]
	public decimal? ManufacturingLotSize { get; set; }

	[XmlElement(ElementName = "inactive")]
	[DataMember(Name = "inactive", Order = 36)]
	public bool? Inactive { get; set; }

	[XmlElement(ElementName = "lastReceiptDate")]
	[DataMember(Name = "lastReceiptDate", Order = 37)]
	public DateTime? LastReceiptDate { get; set; }

	[XmlElement(ElementName = "requiresInspection")]
	[DataMember(Name = "requiresInspection", Order = 38)]
	public bool? RequiresInspection { get; set; }

	[XmlElement(ElementName = "expenseSplitPercentTotal")]
	[DataMember(Name = "expenseSplitPercentTotal", Order = 39)]
	public decimal? ExpenseSplitPercentTotal { get; set; }

	[XmlElement(ElementName = "weight")]
	[DataMember(Name = "weight", Order = 40)]
	public decimal? Weight { get; set; }

	[XmlElement(ElementName = "weightUnitOfMeasure")]
	[DataMember(Name = "weightUnitOfMeasure", Order = 41)]
	public string WeightUnitOfMeasure { get; set; } = string.Empty;

	[XmlElement(ElementName = "createdBy")]
	[DataMember(Name = "createdBy", Order = 42)]
	public string CreatedBy { get; set; } = string.Empty;

	[XmlElement(ElementName = "createdDate")]
	[DataMember(Name = "createdDate", Order = 43)]
	public DateTime? CreatedDate { get; set; }

	[XmlElement(ElementName = "averageUnitCost")]
	[DataMember(Name = "averageUnitCost", Order = 44)]
	public decimal? AverageUnitCost { get; set; }

	[XmlElement(ElementName = "standardUnitCost")]
	[DataMember(Name = "standardUnitCost", Order = 45)]
	public decimal? StandardUnitCost { get; set; }

	[XmlElement(ElementName = "lastUnitCost")]
	[DataMember(Name = "lastUnitCost", Order = 46)]
	public decimal? LastUnitCost { get; set; }

	[XmlElement(ElementName = "sheetSizeX")]
	[DataMember(Name = "sheetSizeX", Order = 47)]
	public decimal? SheetSizeX { get; set; }

	[XmlElement(ElementName = "sheetSizeY")]
	[DataMember(Name = "sheetSizeY", Order = 48)]
	public decimal? SheetSizeY { get; set; }

	[XmlElement(ElementName = "barLength")]
	[DataMember(Name = "barLength", Order = 49)]
	public decimal? BarLength { get; set; }

	[XmlElement(ElementName = "thickness")]
	[DataMember(Name = "thickness", Order = 50)]
	public decimal? Thickness { get; set; }
}
