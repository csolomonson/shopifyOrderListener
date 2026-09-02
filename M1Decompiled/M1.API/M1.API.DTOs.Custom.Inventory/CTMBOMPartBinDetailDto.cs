using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Inventory;

namespace M1.API.DTOs.Custom.Inventory;

[Serializable]
[XmlRoot("parts")]
[DataContract(Namespace = "", Name = "parts")]
public class CTMBOMPartBinDetailDto
{
	[XmlElement(ElementName = "part")]
	[DataMember(Name = "part", Order = 1)]
	[Required(ErrorMessage = "Part is invalid or empty.")]
	public BOMPartDto Part { get; set; }

	[DataMember(Name = "partBinDetails", Order = 2)]
	[XmlArray("partBinDetails")]
	[XmlArrayItem("partBinDetail")]
	public List<BOMPartBinDetailDto> PartBinDetails { get; set; }

	public CTMBOMPartBinDetailDto()
	{
		Part = new BOMPartDto();
		PartBinDetails = new List<BOMPartBinDetailDto>();
	}
}
