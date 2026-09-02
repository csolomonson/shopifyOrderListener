using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.Attributes;

namespace M1.API.DTOs.EDI;

[DataContract(Namespace = "", Name = "edI850SalesOrderDeliveries")]
[XmlRoot(ElementName = "edI850SalesOrderDeliveries")]
public class EDI850SalesOrderDeliveriesIN
{
	[EnsureMinimumElements(1, ErrorMessage = "SalesOrderDeliveries is invalid or empty.")]
	[XmlElement(ElementName = "edI850SalesOrderDelivery")]
	[DataMember(Name = "edI850SalesOrderDelivery", Order = 1)]
	public List<EDI850SalesOrderDeliveryIN> EDI850SalesOrderDeliverySet { get; set; } = new List<EDI850SalesOrderDeliveryIN>();
}
