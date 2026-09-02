using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.DTOs.BOM.Job;

namespace M1.API.DTOs.Custom;

[Serializable]
[XmlRoot("jobs")]
[DataContract(Namespace = "", Name = "jobs")]
public class CTMBOMJobAssemblyDto
{
	[XmlElement(ElementName = "job")]
	[DataMember(Name = "job", Order = 1)]
	[Required(ErrorMessage = "Job is invalid or empty.")]
	public JobInformationDto Job { get; set; }

	[DataMember(Name = "jobAssemblies", Order = 2)]
	[XmlArray("jobAssemblies")]
	[XmlArrayItem("jobAssemblies")]
	public List<BOMJobAssemblyDto> JobAssemblies { get; set; }

	public CTMBOMJobAssemblyDto()
	{
		Job = new JobInformationDto();
		JobAssemblies = new List<BOMJobAssemblyDto>();
	}
}
