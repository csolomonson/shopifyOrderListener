using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Job;

[Serializable]
[DataContract(Namespace = "", Name = "jobs")]
[XmlRoot(ElementName = "jobs")]
public class BOMJobGuidsDto
{
	[XmlArray("jobs")]
	[XmlArrayItem("job")]
	[DataMember(Name = "jobs", Order = 1)]
	public List<BOMJobGuidDto> JobGuids { get; set; } = new List<BOMJobGuidDto>();
}
