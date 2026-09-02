using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.EDI;

[DataContract(Namespace = "")]
[XmlRoot(ElementName = "edI856ASNIn")]
public class EDI856ASNIN
{
	[Required(ErrorMessage = "ShipmentNumber is invalid or empty.")]
	[DataMember(Name = "shipmentNumber", Order = 1)]
	[XmlElement(ElementName = "shipmentNumber")]
	public string ShipmentNumber { get; set; }

	[DataMember(Name = "ediUpdateStatus", Order = 2)]
	[Required(ErrorMessage = "EDIUpdateStatus is invalid or empty.")]
	[XmlElement(ElementName = "ediUpdateStatus")]
	public bool EDIUpdateStatus { get; set; }
}
