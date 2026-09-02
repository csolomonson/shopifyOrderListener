using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.DTOs.BOM.Sales;

namespace M1.API.DTOs.Custom.Sales;

[Serializable]
[XmlRoot("quotes")]
[DataContract(Namespace = "", Name = "quotes")]
public class CTMBOMQuoteLineDto
{
	[XmlElement(ElementName = "quote")]
	[DataMember(Name = "quote", Order = 1)]
	[Required(ErrorMessage = "Quote is invalid or empty.")]
	public BOMQuoteDto Quote { get; set; }

	[DataMember(Name = "quoteLines", Order = 2)]
	[XmlArray("quoteLines")]
	[XmlArrayItem("quoteLine")]
	public List<BOMQuoteLineDto> QuoteLines { get; set; }

	public CTMBOMQuoteLineDto()
	{
		Quote = new BOMQuoteDto();
		QuoteLines = new List<BOMQuoteLineDto>();
	}
}
