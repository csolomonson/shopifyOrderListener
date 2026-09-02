using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace M1.API.DTOs.EDI;

[Serializable]
[DataContract(Namespace = "", Name = "edI810SACLine")]
public class EDI810OutboundInvoiceSACLineDto
{
	[DataMember(Name = "aC_Indicator", Order = 1)]
	public string AC_Indicator { get; set; }

	[DataMember(Name = "aC_Code", Order = 2)]
	public string AC_Code { get; set; }

	[DataMember(Name = "aC_Amount", Order = 3)]
	public decimal AC_Amount { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public string InvoiceNumber { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public short InvoiceLineID { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public short SalesOrderLineID { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public short SalesOrderDeliveryID { get; set; }
}
