using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[XmlRoot("parts")]
[DataContract(Namespace = "", Name = "parts")]
public class CTMBOMPartRevisionDto
{
	[XmlElement(ElementName = "part")]
	[DataMember(Name = "part", Order = 1)]
	[Required(ErrorMessage = "Part is invalid or empty.")]
	public BOMPartDto Part { get; set; }

	[DataMember(Name = "partRevisions", Order = 2)]
	[XmlArray("partRevisions")]
	[XmlArrayItem("partRevision")]
	public List<BOMPartRevisionDto> PartRevisions { get; set; }

	public CTMBOMPartRevisionDto()
	{
		Part = new BOMPartDto();
		PartRevisions = new List<BOMPartRevisionDto>();
	}
}
