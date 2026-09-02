using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[DataContract(Namespace = "", Name = "quotematerial")]
[XmlRoot(ElementName = "quotematerial")]
[XmlType(AnonymousType = true)]
public class BOMQuoteMaterialDto
{
	[XmlElement(ElementName = "quoteID")]
	[DataMember(Name = "quoteID", Order = 1)]
	[Required(ErrorMessage = "QuoteID is invalid or empty.")]
	public string QuoteID { get; set; }

	[XmlElement(ElementName = "quoteLineID")]
	[DataMember(Name = "quoteLineID", Order = 2)]
	[Required(ErrorMessage = "QuoteLineID is invalid or empty.")]
	public short QuoteLineID { get; set; }

	[XmlElement(ElementName = "quoteAssemblyID")]
	[DataMember(Name = "quoteAssemblyID", Order = 3)]
	public int QuoteAssemblyID { get; set; }

	[XmlElement(ElementName = "quoteMaterialID")]
	[DataMember(Name = "quoteMaterialID", Order = 4)]
	[Required(ErrorMessage = "QuoteMaterialID is invalid or empty.")]
	public int QuoteMaterialID { get; set; }

	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 5)]
	[Required(ErrorMessage = "PartID is invalid or empty.")]
	public string PartID { get; set; }

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 6)]
	[Required(ErrorMessage = "PartRevisionID is invalid or empty.")]
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "partWarehouseLocationID")]
	[DataMember(Name = "partWarehouseLocationID", Order = 7)]
	[Required(ErrorMessage = "PartWarehouseLocationID is invalid or empty.")]
	public string PartWarehouseLocationID { get; set; }

	[XmlElement(ElementName = "partBinID")]
	[DataMember(Name = "partBinID", Order = 8)]
	[Required(ErrorMessage = "PartBinID is invalid or empty.")]
	public string PartBinID { get; set; }

	[XmlElement(ElementName = "unitOfMeasure")]
	[DataMember(Name = "unitOfMeasure", Order = 9)]
	public string UnitOfMeasure { get; set; }

	[XmlElement(ElementName = "partShortDescription")]
	[DataMember(Name = "partShortDescription", Order = 10)]
	[Required(ErrorMessage = "PartShortDescription is invalid or empty.")]
	public string PartShortDescription { get; set; }

	[XmlElement(ElementName = "quantityPerAssembly")]
	[DataMember(Name = "quantityPerAssembly", Order = 11)]
	public decimal QuantityPerAssembly { get; set; }

	[XmlElement(ElementName = "scrapPercent")]
	[DataMember(Name = "scrapPercent", Order = 12)]
	public decimal ScrapPercent { get; set; }

	[XmlElement(ElementName = "scrapQuantity")]
	[DataMember(Name = "scrapQuantity", Order = 13)]
	public decimal ScrapQuantity { get; set; }

	[XmlElement(ElementName = "estimatedUnitCost")]
	[DataMember(Name = "estimatedUnitCost", Order = 14)]
	public decimal EstimatedUnitCost { get; set; }

	[XmlElement(ElementName = "supplierOrganizationID")]
	[DataMember(Name = "supplierOrganizationID", Order = 15)]
	public string SupplierOrganizationID { get; set; }

	[XmlElement(ElementName = "purchaseLocationID")]
	[DataMember(Name = "purchaseLocationID", Order = 16)]
	public string PurchaseLocationID { get; set; }

	[XmlElement(ElementName = "leadTime")]
	[DataMember(Name = "leadTime", Order = 17)]
	public short LeadTime { get; set; }

	[XmlElement(ElementName = "minimumCharge")]
	[DataMember(Name = "minimumCharge", Order = 18)]
	public decimal MinimumCharge { get; set; }

	[XmlElement(ElementName = "createdBy")]
	[DataMember(Name = "createdBy", Order = 19)]
	public string CreatedBy { get; set; }

	[XmlElement(ElementName = "createdDate")]
	[DataMember(Name = "createdDate", Order = 20)]
	public DateTime? CreatedDate { get; set; }

	[XmlElement(ElementName = "closed")]
	[DataMember(Name = "closed", Order = 21)]
	public bool Closed { get; set; }

	[XmlElement(ElementName = "uniqueID")]
	[DataMember(Name = "uniqueID", Order = 22)]
	public Guid UniqueID { get; set; }

	[XmlElement(ElementName = "rowVersion")]
	[DataMember(Name = "rowVersion", Order = 23)]
	public byte[] RowVersion { get; set; }
}
