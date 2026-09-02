using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[DataContract(Namespace = "", Name = "partMethodGuids")]
[XmlRoot(ElementName = "partMethodGuids")]
public class CTMBOMPartMethodGuidsDto
{
	[XmlArray("partMethodGuids")]
	[XmlArrayItem("partMethodGuid")]
	[DataMember(Name = "partMethodGuids", Order = 1)]
	public List<CTMBOMPartMethodGuidDto> PartMethodGuids { get; set; }
}
