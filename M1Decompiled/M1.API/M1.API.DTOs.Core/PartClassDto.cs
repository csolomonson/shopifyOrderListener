using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.Core;

[Serializable]
[DataContract(Namespace = "", Name = "partClass")]
[XmlRoot(ElementName = "partClass")]
[XmlType(AnonymousType = true)]
public class PartClassDto
{
	[XmlElement(ElementName = "partClassID")]
	[DataMember(Name = "partClassID", Order = 1)]
	[Required(ErrorMessage = "PartClassID is invalid or empty.")]
	public string PartClassID { get; set; }

	[XmlElement(ElementName = "description")]
	[DataMember(Name = "description", Order = 2)]
	[Required(ErrorMessage = "Description is invalid or empty.")]
	public string Description { get; set; }
}
