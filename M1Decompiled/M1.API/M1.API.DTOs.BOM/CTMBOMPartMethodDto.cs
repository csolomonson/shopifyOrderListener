using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[XmlRoot(ElementName = "partmethod")]
[DataContract(Namespace = "", Name = "partmethod")]
[XmlType(AnonymousType = true)]
public class CTMBOMPartMethodDto
{
	[XmlElement(ElementName = "part")]
	[DataMember(Name = "part", Order = 1)]
	public BOMPartDto Part { get; set; }

	[XmlElement(ElementName = "partRevision")]
	[DataMember(Name = "partRevision", Order = 2)]
	public BOMPartRevisionDto PartRevision { get; set; }

	[XmlArrayItem("partAssembly")]
	[DataMember(Name = "partAssemblies", Order = 3)]
	public List<BOMPartMethodAssemblyDto> PartAssemblies { get; set; }
}
