using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.EDI;

[DataContract(Namespace = "")]
[XmlRoot]
public class M1Location
{
	[DataMember(Name = "isM1ID")]
	[XmlElement(ElementName = "isM1ID")]
	public bool IsM1ID { get; set; }

	[DataMember(Name = "value")]
	[XmlElement(ElementName = "value")]
	public string Value { get; set; }
}
