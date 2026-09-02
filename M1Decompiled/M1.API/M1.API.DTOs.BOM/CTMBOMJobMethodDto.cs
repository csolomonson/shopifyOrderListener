using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.DTOs.BOM.Job;

namespace M1.API.DTOs.BOM;

[Serializable]
[XmlRoot(ElementName = "jobmethod")]
[DataContract(Namespace = "", Name = "jobmethod")]
[XmlType(AnonymousType = true)]
public class CTMBOMJobMethodDto
{
	[XmlElement(ElementName = "job")]
	[DataMember(Name = "job", Order = 1)]
	public BOMJobDto JobHeader { get; set; }

	[XmlArray(ElementName = "jobmethodassemblies")]
	[XmlArrayItem(ElementName = "jobmethodassembly")]
	[DataMember(Name = "jobmethodassemblies", Order = 2)]
	public List<CTMBOMJobMethodAssemblyDto> JobMethodAssemblies { get; set; }
}
