using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.EDI;

[DataContract(Namespace = "", Name = "edI850SalesOrders")]
[XmlRoot(ElementName = "edI850SalesOrders")]
public class EDI850SalesOrdersIN
{
	[XmlElement(ElementName = "edI850SalesOrder")]
	[DataMember(Name = "edI850SalesOrder", Order = 1)]
	public List<EDI850SalesOrderIN> EDISalesOrderSet { get; set; } = new List<EDI850SalesOrderIN>();
}
