using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.DTOs.BOM.Job;

namespace M1.API.DTOs.BOM;

[Serializable]
[XmlRoot(ElementName = "jobmethodassembly")]
[DataContract(Namespace = "", Name = "jobmethodassembly")]
[XmlType(AnonymousType = true)]
public class CTMBOMJobMethodAssemblyDto
{
	[XmlElement(ElementName = "jobAssembly")]
	[DataMember(Name = "jobAssembly")]
	public BOMJobAssemblyDto JobAssembly { get; set; }

	[XmlArray(ElementName = "jobOperations")]
	[XmlArrayItem(ElementName = "jobOperation")]
	[DataMember(Name = "jobOperations")]
	public List<BOMJobOperationDto> JobOperations { get; set; }

	[XmlArray(ElementName = "jobMaterials")]
	[XmlArrayItem(ElementName = "jobMaterial")]
	[DataMember(Name = "jobMaterials")]
	public List<BOMJobMaterialDto> JobMaterials { get; set; }
}
