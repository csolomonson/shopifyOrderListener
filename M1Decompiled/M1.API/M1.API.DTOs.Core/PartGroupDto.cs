using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.Core;

[Serializable]
[DataContract(Namespace = "", Name = "partGroup")]
[XmlRoot(ElementName = "partGroup")]
[XmlType(AnonymousType = true)]
public class PartGroupDto
{
	[XmlElement(ElementName = "partGroupID")]
	[DataMember(Name = "partGroupID", Order = 1)]
	[Required(ErrorMessage = "PartGroupID is invalid or empty.")]
	public string PartGroupID { get; set; }

	[XmlElement(ElementName = "description")]
	[DataMember(Name = "description", Order = 2)]
	[Required(ErrorMessage = "Description is invalid or empty.")]
	public string Description { get; set; }
}
