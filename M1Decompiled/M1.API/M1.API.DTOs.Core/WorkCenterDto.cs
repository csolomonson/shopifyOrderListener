using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.Core;

[Serializable]
[DataContract(Namespace = "", Name = "workCenter")]
[XmlRoot(ElementName = "workCenter")]
[XmlType(AnonymousType = true)]
public class WorkCenterDto
{
	[XmlElement(ElementName = "workCenterID")]
	[DataMember(Name = "workCenterID", Order = 1)]
	[Required(ErrorMessage = "WorkCenterID is invalid or empty.")]
	public string WorkCenterID { get; set; }

	[XmlElement(ElementName = "description")]
	[DataMember(Name = "description", Order = 2)]
	[Required(ErrorMessage = "Description is invalid or empty.")]
	public string Description { get; set; }
}
