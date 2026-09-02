using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.EDI;

[DataContract(Namespace = "")]
[XmlRoot(ElementName = "edI810InvoiceIn")]
public class EDI810InvoiceIN
{
	[Required(ErrorMessage = "InvoiceNumber is invalid or empty.")]
	[DataMember(Name = "invoiceNumber", Order = 1)]
	[XmlElement(ElementName = "invoiceNumber")]
	public string InvoiceNumber { get; set; }

	[DataMember(Name = "ediUpdateStatus", Order = 2)]
	[Required(ErrorMessage = "EDIUpdateStatus is invalid or empty.")]
	[XmlElement(ElementName = "ediUpdateStatus")]
	public bool EDIUpdateStatus { get; set; }
}
