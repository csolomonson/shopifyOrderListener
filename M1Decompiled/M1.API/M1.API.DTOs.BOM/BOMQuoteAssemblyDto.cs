using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[DataContract(Namespace = "", Name = "quoteassembly")]
[XmlRoot(ElementName = "quoteassembly")]
[XmlType(AnonymousType = true)]
public class BOMQuoteAssemblyDto
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
	public int ParentAssemblyID { get; set; }

	[XmlElement(ElementName = "level")]
	[DataMember(Name = "level", Order = 5)]
	[Required(ErrorMessage = "Level is invalid or empty.")]
	public short Level { get; set; }

	[XmlElement(ElementName = "sourceMethodID")]
	[DataMember(Name = "sourceMethodID", Order = 6)]
	public string SourceMethodID { get; set; }

	[XmlElement(ElementName = "sourceRevisionID")]
	[DataMember(Name = "sourceRevisionID", Order = 7)]
	public string SourceRevisionID { get; set; }

	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 8)]
	[Required(ErrorMessage = "PartID is invalid or empty.")]
	public string PartID { get; set; }

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 9)]
	[Required(ErrorMessage = "PartRevisionID is invalid or empty.")]
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "unitOfMeasure")]
	[DataMember(Name = "unitOfMeasure", Order = 10)]
	public string UnitOfMeasure { get; set; }

	[XmlElement(ElementName = "partShortDescription")]
	[DataMember(Name = "partShortDescription", Order = 11)]
	[Required(ErrorMessage = "PartShortDescription is invalid or empty.")]
	public string PartShortDescription { get; set; }

	[XmlElement(ElementName = "quantityPerParent")]
	[DataMember(Name = "quantityPerParent", Order = 12)]
	[Required(ErrorMessage = "QuantityPerParent is invalid or empty.")]
	public decimal QuantityPerParent { get; set; }

	[XmlElement(ElementName = "closed")]
	[DataMember(Name = "closed", Order = 13)]
	public bool Closed { get; set; }

	[XmlElement(ElementName = "pullAllFromStock")]
	[DataMember(Name = "pullAllFromStock", Order = 14)]
	public bool PullAllFromStock { get; set; }

	[XmlElement(ElementName = "createdBy")]
	[DataMember(Name = "createdBy", Order = 15)]
	public string CreatedBy { get; set; }

	[XmlElement(ElementName = "createdDate")]
	[DataMember(Name = "createdDate", Order = 16)]
	public DateTime? CreatedDate { get; set; }

	[XmlElement(ElementName = "uniqueID")]
	[DataMember(Name = "uniqueID", Order = 17)]
	public Guid UniqueID { get; set; }

	[XmlElement(ElementName = "rowVersion")]
	[DataMember(Name = "rowVersion", Order = 18)]
	public byte[] RowVersion { get; set; }
}
