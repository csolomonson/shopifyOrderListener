using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.DTOs.BOM.Transaction;

namespace M1.API.DTOs.Custom.Transaction;

[Serializable]
[XmlRoot("receipts")]
[DataContract(Namespace = "", Name = "receipts")]
public class CTMBOMReceiptDto
{
	[XmlElement(ElementName = "receipt")]
	[DataMember(Name = "receipt", Order = 1)]
	public BOMReceiptDto ReceiptHeader { get; set; }

	[XmlElement(ElementName = "receiptLines")]
	[XmlArrayItem(ElementName = "receiptLine")]
	[DataMember(Name = "receiptLines", Order = 2)]
	public List<CTMBOMReceiptLineDto> ReceiptLines { get; set; }
}
