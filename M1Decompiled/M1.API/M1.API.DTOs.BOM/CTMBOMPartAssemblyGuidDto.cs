using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[XmlRoot(ElementName = "partAssemblyGuid")]
[DataContract(Namespace = "", Name = "partAssemblyGuid")]
[XmlType(AnonymousType = true)]
public class CTMBOMPartAssemblyGuidDto
{
	[XmlElement(ElementName = "assemblyId")]
	[DataMember(Name = "assemblyId", Order = 1)]
	public int AssemblyId { get; set; }

	[XmlElement(ElementName = "assemblyGuid")]
	[DataMember(Name = "assemblyGuid", Order = 2)]
	public string AssemblyGuid { get; set; }

	[XmlArray("partMaterialGuids")]
	[XmlArrayItem("partMaterialGuid")]
	[DataMember(Name = "partMaterialGuids", Order = 3)]
	public List<CTMBOMPartMaterialGuidDto> PartMaterialGuids { get; set; }

	[XmlArray("partOperationGuids")]
	[XmlArrayItem("partOperationGuid")]
	[DataMember(Name = "partOperationGuids", Order = 4)]
	public List<CTMBOMPartOperationGuidDto> PartOperationGuids { get; set; }
}
