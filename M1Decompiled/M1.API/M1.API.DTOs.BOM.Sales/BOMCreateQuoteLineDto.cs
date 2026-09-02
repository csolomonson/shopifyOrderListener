using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Sales;

[Serializable]
[DataContract(Namespace = "", Name = "quoteline")]
[XmlRoot(ElementName = "quoteline")]
[XmlType(AnonymousType = true)]
public class BOMCreateQuoteLineDto
{
	[XmlElement(ElementName = "quoteID")]
	[DataMember(Name = "quoteID", Order = 1)]
	[Required(ErrorMessage = "QuoteID is invalid or empty.")]
	public string QuoteID { get; set; }

	[XmlElement(ElementName = "quoteLineID")]
	[DataMember(Name = "quoteLineID", Order = 2)]
	[Required(ErrorMessage = "QuoteLineID is invalid or empty.")]
	public short QuoteLineID { get; set; }

	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 3)]
	[Required(ErrorMessage = "PartID is invalid or empty.")]
	public string PartID { get; set; }

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 4)]
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "unitOfMeasure")]
	[DataMember(Name = "unitOfMeasure", Order = 5)]
	[Required(ErrorMessage = "UnitOfMeasure is invalid or empty.")]
	public string UnitOfMeasure { get; set; }

	[XmlElement(ElementName = "supplierOrganizationID")]
	[DataMember(Name = "supplierOrganizationID", Order = 6)]
	public string SupplierOrganizationID { get; set; }

	[XmlElement(ElementName = "purchaseLocationID")]
	[DataMember(Name = "purchaseLocationID", Order = 7)]
	public string PurchaseLocationID { get; set; }

	[XmlElement(ElementName = "purchaseUnitCostBase")]
	[DataMember(Name = "purchaseUnitCostBase", Order = 8)]
	public decimal PurchaseUnitCostBase { get; set; }

	[XmlElement(ElementName = "firm")]
	[DataMember(Name = "firm", Order = 9)]
	public bool? Firm { get; set; }
}
