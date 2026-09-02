using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.EDI;

[DataContract(Namespace = "", Name = "edI856ASNsIn")]
[XmlRoot(ElementName = "edI856ASNsIn")]
public class EDI856ASNsIN
{
	[XmlElement(ElementName = "edI856ASNIn")]
	[DataMember(Name = "edI856ASNIn", Order = 1)]
	public List<EDI856ASNIN> EDI856ASNSet { get; set; } = new List<EDI856ASNIN>();
}
