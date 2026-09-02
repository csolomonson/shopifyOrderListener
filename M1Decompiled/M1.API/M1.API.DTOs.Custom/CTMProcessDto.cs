using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.DTOs.Core;

namespace M1.API.DTOs.Custom;

[Serializable]
[DataContract(Namespace = "", Name = "processes")]
[XmlRoot(ElementName = "processes")]
[XmlType(AnonymousType = true)]
public class CTMProcessDto
{
	[DataMember(Name = "processes", Order = 1)]
	[XmlArray("processes")]
	[XmlArrayItem("process")]
	public List<ProcessDto> Processes { get; set; }
}
