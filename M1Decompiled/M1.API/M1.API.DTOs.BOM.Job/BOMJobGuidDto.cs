using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Job;

[Serializable]
[DataContract(Namespace = "", Name = "job")]
[XmlRoot(ElementName = "job")]
[XmlType(AnonymousType = true)]
public class BOMJobGuidDto
{
	[XmlElement(ElementName = "jobId")]
	[DataMember(Name = "jobId", Order = 1)]
	public string JobId { get; set; }

	[XmlElement(ElementName = "jobGuid")]
	[DataMember(Name = "jobGuid", Order = 2)]
	public string JobGUID { get; set; }

	[XmlElement(ElementName = "partId")]
	[DataMember(Name = "partId", Order = 3)]
	public string PartId { get; set; }

	[XmlElement(ElementName = "partGuid")]
	[DataMember(Name = "partGuid", Order = 4)]
	public string PartGUID { get; set; }
}
