using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Sales;

[Serializable]
[DataContract(Namespace = "", Name = "quoteassembly")]
[XmlRoot(ElementName = "quoteassembly")]
[XmlType(AnonymousType = true)]
public class BOMCreateQuoteAssemblyDto
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
	[Required(ErrorMessage = "QuoteAssemblyID is invalid or empty.")]
	public int QuoteAssemblyID { get; set; }

	[XmlElement(ElementName = "parentAssemblyID")]
	[DataMember(Name = "parentAssemblyID", Order = 4)]
	[Required(ErrorMessage = "ParentAssemblyID is invalid or empty.")]
	public int ParentAssemblyID { get; set; }

	[XmlElement(ElementName = "level")]
	[DataMember(Name = "level", Order = 5)]
	[Required(ErrorMessage = "Level is invalid or empty.")]
	public short Level { get; set; }

	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 6)]
	[Required(ErrorMessage = "PartID is invalid or empty.")]
	public string PartID { get; set; }

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 7)]
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "unitOfMeasure")]
	[DataMember(Name = "unitOfMeasure", Order = 8)]
	[Required(ErrorMessage = "UnitOfMeasure is invalid or empty.")]
	public string UnitOfMeasure { get; set; }

	[XmlElement(ElementName = "quantityPerParent")]
	[DataMember(Name = "quantityPerParent", Order = 9)]
	[Required(ErrorMessage = "QuantityPerParent is invalid or empty.")]
	public decimal QuantityPerParent { get; set; }

	[XmlElement(ElementName = "pullAllFromStock")]
	[DataMember(Name = "pullAllFromStock", Order = 10)]
	public bool PullAllFromStock { get; set; }
}
