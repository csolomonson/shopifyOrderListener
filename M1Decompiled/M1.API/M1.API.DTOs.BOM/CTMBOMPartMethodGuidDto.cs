using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[XmlRoot(ElementName = "partMethodGuid")]
[DataContract(Namespace = "", Name = "partMethodGuid")]
[XmlType(AnonymousType = true)]
public class CTMBOMPartMethodGuidDto
{
	[XmlElement(ElementName = "partId")]
	[DataMember(Name = "partId", Order = 1)]
	public string PartId { get; set; }

	[XmlElement(ElementName = "partGuid")]
	[DataMember(Name = "partGuid", Order = 2)]
	public string PartGuid { get; set; }

	[XmlArray("partRevisionGuids")]
	[XmlArrayItem("partRevisionGuid")]
	[DataMember(Name = "partRevisionGuids", Order = 3)]
	public List<CTMBOMPartRevisionGuidDto> PartRevisionGuids { get; set; }
}
