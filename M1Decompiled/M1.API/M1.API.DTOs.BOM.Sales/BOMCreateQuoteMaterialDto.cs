using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Sales;

[Serializable]
[DataContract(Namespace = "", Name = "quotematerial")]
[XmlRoot(ElementName = "quotematerial")]
[XmlType(AnonymousType = true)]
public class BOMCreateQuoteMaterialDto
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
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "unitOfMeasure")]
	[DataMember(Name = "unitOfMeasure", Order = 7)]
	[Required(ErrorMessage = "UnitOfMeasure is invalid or empty.")]
	public string UnitOfMeasure { get; set; }

	[XmlElement(ElementName = "quantityPerAssembly")]
	[DataMember(Name = "quantityPerAssembly", Order = 8)]
	public decimal QuantityPerAssembly { get; set; }

	[XmlElement(ElementName = "supplierOrganizationID")]
	[DataMember(Name = "supplierOrganizationID", Order = 9)]
	public string SupplierOrganizationID { get; set; }

	[XmlElement(ElementName = "purchaseLocationID")]
	[DataMember(Name = "purchaseLocationID", Order = 10)]
	public string PurchaseLocationID { get; set; }
}
