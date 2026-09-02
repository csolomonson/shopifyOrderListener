using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.Attributes;

namespace M1.API.DTOs.EDI;

[DataContract(Namespace = "", Name = "edI850SalesOrderLines")]
[XmlRoot(ElementName = "edI850SalesOrderLines")]
public class EDI850SalesOrderLinesIN
{
	[EnsureMinimumElements(1, ErrorMessage = "SalesOrderLines is invalid or empty.")]
	[XmlElement(ElementName = "edI850SalesOrderLine")]
	[DataMember(Name = "edI850SalesOrderLine", Order = 1)]
	public List<EDI850SalesOrderLineIN> EDISalesOrderLineSet { get; set; } = new List<EDI850SalesOrderLineIN>();
}
