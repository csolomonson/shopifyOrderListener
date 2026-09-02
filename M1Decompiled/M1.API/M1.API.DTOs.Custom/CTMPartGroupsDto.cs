using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.DTOs.Core;

namespace M1.API.DTOs.Custom;

[Serializable]
[DataContract(Namespace = "", Name = "partgroups")]
[XmlRoot(ElementName = "partgroups")]
[XmlType(AnonymousType = true)]
public class CTMPartGroupsDto
{
	[DataMember(Name = "partGroups", Order = 1)]
	[XmlArray("partGroups")]
	[XmlArrayItem("partGroup")]
	public List<PartGroupDto> PartGroups { get; set; }
}
