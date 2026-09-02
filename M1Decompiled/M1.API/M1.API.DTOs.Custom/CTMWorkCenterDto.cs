using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.DTOs.Core;

namespace M1.API.DTOs.Custom;

[Serializable]
[DataContract(Namespace = "", Name = "workCenters")]
[XmlRoot(ElementName = "workCenters")]
[XmlType(AnonymousType = true)]
public class CTMWorkCenterDto
{
	[DataMember(Name = "workCenters", Order = 1)]
	[XmlArray("workCenters")]
	[XmlArrayItem("workCenter")]
	public List<WorkCenterDto> WorkCenters { get; set; }
}
