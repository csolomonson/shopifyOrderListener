using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[XmlRoot(ElementName = "partMaterialGuid")]
[DataContract(Namespace = "", Name = "partMaterialGuid")]
[XmlType(AnonymousType = true)]
public class CTMBOMPartMaterialGuidDto
{
	[XmlElement(ElementName = "materialId")]
	[DataMember(Name = "materialId", Order = 1)]
	public int MaterialId { get; set; }

	[XmlElement(ElementName = "materialGuid")]
	[DataMember(Name = "materialGuid", Order = 2)]
	public string MaterialGuid { get; set; }
}
