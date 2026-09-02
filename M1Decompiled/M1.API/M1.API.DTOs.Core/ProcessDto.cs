using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.Core;

[Serializable]
[DataContract(Namespace = "", Name = "process")]
[XmlRoot(ElementName = "process")]
[XmlType(AnonymousType = true)]
public class ProcessDto
{
	[XmlElement(ElementName = "processID")]
	[DataMember(Name = "processID", Order = 1)]
	[Required(ErrorMessage = "ProcessID is invalid or empty.")]
	public string ProcessID { get; set; }

	[XmlElement(ElementName = "shortDescription")]
	[DataMember(Name = "shortDescription", Order = 2)]
	[Required(ErrorMessage = "ShortDescription is invalid or empty.")]
	public string ShortDescription { get; set; }
}
