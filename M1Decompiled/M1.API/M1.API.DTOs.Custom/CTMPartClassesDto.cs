using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.DTOs.Core;

namespace M1.API.DTOs.Custom;

[Serializable]
[DataContract(Namespace = "", Name = "partclasses")]
[XmlRoot(ElementName = "partclasses")]
[XmlType(AnonymousType = true)]
public class CTMPartClassesDto
{
	[DataMember(Name = "partClasses", Order = 1)]
	[XmlArray("partClasses")]
	[XmlArrayItem("partClass")]
	public List<PartClassDto> PartClasses { get; set; }
}
