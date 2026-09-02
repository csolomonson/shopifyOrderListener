using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[DataContract(Namespace = "", Name = "partassembly")]
[XmlRoot(ElementName = "partassembly")]
[XmlType(AnonymousType = true)]
public class BOMPartAssemblyDto
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

	[XmlElement(ElementName = "level")]
	[DataMember(Name = "level", Order = 4)]
	[Required(ErrorMessage = "Level is invalid or empty.")]
	public short Level { get; set; }

	[XmlElement(ElementName = "useMethod")]
	[DataMember(Name = "useMethod", Order = 5)]
	[Required(ErrorMessage = "UseMethod is invalid or empty.")]
	public bool UseMethod { get; set; }

	[XmlElement(ElementName = "parentAssemblyID")]
	[DataMember(Name = "parentAssemblyID", Order = 6)]
	[Required(ErrorMessage = "ParentAssemblyID is invalid or empty.")]
	public int ParentAssemblyID { get; set; }

	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 7)]
	[Required(ErrorMessage = "PartID is invalid or empty.")]
	public string PartID { get; set; }

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 8)]
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "partShortDescription")]
	[DataMember(Name = "partShortDescription", Order = 9)]
	[Required(ErrorMessage = "PartShortDescription is invalid or empty.")]
	public string PartShortDescription { get; set; }

	[XmlElement(ElementName = "unitOfMeasure")]
	[DataMember(Name = "unitOfMeasure", Order = 10)]
	public string UnitOfMeasure { get; set; }

	[XmlElement(ElementName = "quantityPerParent")]
	[DataMember(Name = "quantityPerParent", Order = 11)]
	public decimal QuantityPerParent { get; set; }

	[XmlElement(ElementName = "overlapOperationID")]
	[DataMember(Name = "overlapOperationID", Order = 12)]
	public int? OverlapOperationId { get; set; }

	[XmlElement(ElementName = "partLongDescription")]
	[DataMember(Name = "partLongDescription", Order = 13)]
	public string PartLongDescription { get; set; }
}
