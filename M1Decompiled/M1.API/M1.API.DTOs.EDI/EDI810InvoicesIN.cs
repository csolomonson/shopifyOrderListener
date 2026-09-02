using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.EDI;

[DataContract(Namespace = "", Name = "edI810InvoicesIn")]
[XmlRoot(ElementName = "edI810InvoicesIn")]
public class EDI810InvoicesIN
{
	[XmlElement(ElementName = "edI810InvoiceIn")]
	[DataMember(Name = "edI810InvoiceIn", Order = 1)]
	public List<EDI810InvoiceIN> EDI810InvoiceSet { get; set; } = new List<EDI810InvoiceIN>();
}
