using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace M1.API.DTOs.BOM;

[Serializable]
[DataContract(Namespace = "", Name = "partmaterial")]
[XmlRoot(ElementName = "partmaterial")]
[XmlType(AnonymousType = true)]
public class BOMPartMaterialDto
{
	[XmlElement(ElementName = "methodID")]
	[DataMember(Name = "methodID", Order = 1)]
	[Required(ErrorMessage = "MethodID is invalid or empty.")]
	public string MethodID { get; set; }

	[XmlElement(ElementName = "methodRevisionID")]
	[DataMember(Name = "methodRevisionID", Order = 2)]
	public string MethodRevisionID { get; set; }

	[XmlElement(ElementName = "methodAssemblyID")]
	[DataMember(Name = "methodAssemblyID", Order = 3)]
	[Required(ErrorMessage = "MethodAssemblyID is invalid or empty.")]
	public int MethodAssemblyID { get; set; }

	[XmlElement(ElementName = "methodMaterialID")]
	[DataMember(Name = "methodMaterialID", Order = 4)]
	[Required(ErrorMessage = "MethodMaterialID is invalid or empty.")]
	public int MethodMaterialID { get; set; }

	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 5)]
	[Required(ErrorMessage = "PartID is invalid or empty.")]
	public string PartID { get; set; }

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 6)]
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "relatedPartOperationID")]
	[DataMember(Name = "relatedPartOperationID", Order = 7)]
	public int RelatedPartOperationID { get; set; }

	[XmlElement(ElementName = "partShortDescription")]
	[DataMember(Name = "partShortDescription", Order = 8)]
	[Required(ErrorMessage = "PartShortDescription is invalid or empty.")]
	public string PartShortDescription { get; set; }

	[XmlElement(ElementName = "unitOfMeasure")]
	[DataMember(Name = "unitOfMeasure", Order = 9)]
	public string UnitOfMeasure { get; set; }

	[XmlElement(ElementName = "estimatedUnitCost")]
	[DataMember(Name = "estimatedUnitCost", Order = 10)]
	public decimal EstimatedUnitCost { get; set; }

	[XmlElement(ElementName = "leadTime")]
	[DataMember(Name = "leadTime", Order = 11)]
	public short LeadTime { get; set; }

	[XmlElement(ElementName = "supplierOrganizationID")]
	[DataMember(Name = "supplierOrganizationID", Order = 12)]
	public string SupplierOrganizationID { get; set; }

	[XmlElement(ElementName = "purchaseLocationID")]
	[DataMember(Name = "purchaseLocationID", Order = 13)]
	public string PurchaseLocationID { get; set; }

	[XmlElement(ElementName = "quantityPerParent")]
	[DataMember(Name = "quantityPerParent", Order = 14)]
	public decimal QuantityPerAssembly { get; set; }

	[XmlElement(ElementName = "manualPart")]
	[DataMember(Name = "manualPart", Order = 15)]
	public bool ManualPart { get; set; }

	[XmlElement(ElementName = "createdBy")]
	[DataMember(Name = "createdBy", Order = 16)]
	[XmlIgnore]
	[JsonIgnore]
	public bool UseDefaultWarehouseAndBin { get; set; } = true;

	[XmlElement(ElementName = "partLongDescription")]
	[DataMember(Name = "partLongDescription", Order = 17)]
	public string PartLongDescription { get; set; }

	[XmlElement(ElementName = "backflush")]
	[DataMember(Name = "backflush", Order = 18)]
	public bool? BackFlush { get; set; }

	[XmlElement(ElementName = "scrapQuantity")]
	[DataMember(Name = "scrapQuantity", Order = 19)]
	public decimal? ScrapQuantity { get; set; }

	[XmlElement(ElementName = "scrapPercent")]
	[DataMember(Name = "scrapPercent", Order = 20)]
	public decimal? ScrapPercent { get; set; }
}
