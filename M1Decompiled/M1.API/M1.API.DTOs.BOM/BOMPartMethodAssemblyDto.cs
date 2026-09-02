using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[XmlRoot(ElementName = "partAssembly")]
[DataContract(Namespace = "", Name = "partAssembly")]
[XmlType(AnonymousType = true)]
public class BOMPartMethodAssemblyDto
{
	[XmlElement(ElementName = "assembly")]
	[DataMember(Name = "assembly", Order = 1)]
	public BOMPartAssemblyDto PartAssembly { get; set; }

	[XmlArray("operations")]
	[XmlArrayItem("operation")]
	[DataMember(Name = "operations", Order = 2)]
	public List<BOMPartOperationDto> PartOperations { get; set; }

	[XmlArray("materials")]
	[XmlArrayItem("material")]
	[DataMember(Name = "materials", Order = 3)]
	public List<BOMPartMaterialDto> PartMaterials { get; set; }
}
