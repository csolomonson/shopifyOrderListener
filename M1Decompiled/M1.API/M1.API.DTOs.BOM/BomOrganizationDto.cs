using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[DataContract(Namespace = "", Name = "organization")]
[XmlRoot(ElementName = "organization")]
[XmlType(AnonymousType = true)]
public class BomOrganizationDto
{
	[XmlElement(ElementName = "organizationID")]
	[DataMember(Name = "organizationID", Order = 1)]
	public string OrganizationID { get; set; }

	[XmlElement(ElementName = "name")]
	[DataMember(Name = "name", Order = 2)]
	public string Name { get; set; }
}
