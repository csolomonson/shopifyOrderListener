using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[XmlRoot(ElementName = "partRevisionGuid")]
[DataContract(Namespace = "", Name = "partRevisionGuid")]
[XmlType(AnonymousType = true)]
public class CTMBOMPartRevisionGuidDto
{
	[XmlElement(ElementName = "revisionId")]
	[DataMember(Name = "revisionId", Order = 1)]
	public string RevisionId { get; set; }

	[XmlElement(ElementName = "revisionGuid")]
	[DataMember(Name = "revisionGuid", Order = 2)]
	public string RevisionGuid { get; set; }

	[XmlArray("partAssemblyGuids")]
	[XmlArrayItem("partAssemblyGuid")]
	[DataMember(Name = "partAssemblyGuids", Order = 3)]
	public List<CTMBOMPartAssemblyGuidDto> PartAssemblyGuids { get; set; }
}
