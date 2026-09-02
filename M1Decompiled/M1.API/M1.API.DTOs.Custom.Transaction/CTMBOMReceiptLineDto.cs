using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.DTOs.BOM.Transaction;

namespace M1.API.DTOs.Custom.Transaction;

[Serializable]
[XmlRoot(ElementName = "receiptLine")]
[DataContract(Namespace = "", Name = "receiptLine")]
[XmlType(AnonymousType = true)]
public class CTMBOMReceiptLineDto
{
	[XmlElement(ElementName = "receipt")]
	[DataMember(Name = "receipt", Order = 1)]
	[Required(ErrorMessage = "Receipt is invalid or empty.")]
	public BOMReceiptDto Receipt { get; set; }

	[DataMember(Name = "receiptLines", Order = 2)]
	[XmlArray("receiptLines")]
	[XmlArrayItem("receiptLine")]
	public List<BOMReceiptLineDto> ReceiptLines { get; set; }

	public CTMBOMReceiptLineDto()
	{
		Receipt = new BOMReceiptDto();
		ReceiptLines = new List<BOMReceiptLineDto>();
	}
}
