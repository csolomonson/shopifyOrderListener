using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.Custom;

[Serializable]
[DataContract(Namespace = "", Name = "job")]
[XmlRoot]
[XmlType(AnonymousType = true)]
public class JobInformationDto
{
	[XmlElement(ElementName = "jobID")]
	[DataMember(Name = "jobID", Order = 1)]
	public string JobID { get; set; }

	[XmlElement(ElementName = "customerOrganizationID")]
	[DataMember(Name = "customerOrganizationID", Order = 2)]
	public string CustomerOrganizationID { get; set; }

	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 3)]
	public string PartID { get; set; }

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 4)]
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "partWareHouseLocationID")]
	[DataMember(Name = "partWareHouseLocationID", Order = 5)]
	public string PartWareHouseLocationID { get; set; }

	[XmlElement(ElementName = "partBinID")]
	[DataMember(Name = "partBinID", Order = 6)]
	public string PartBinID { get; set; }

	[XmlElement(ElementName = "orderQuantity")]
	[DataMember(Name = "orderQuantity", Order = 7)]
	public decimal OrderQuantity { get; set; }
}
