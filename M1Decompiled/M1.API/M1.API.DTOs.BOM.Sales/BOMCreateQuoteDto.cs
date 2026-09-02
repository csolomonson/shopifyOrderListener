using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Sales;

[Serializable]
[DataContract(Namespace = "", Name = "quote")]
[XmlRoot(ElementName = "quote")]
[XmlType(AnonymousType = true)]
public class BOMCreateQuoteDto
{
	[XmlElement(ElementName = "quoteID")]
	[DataMember(Name = "quoteID", Order = 1)]
	[Required(ErrorMessage = "QuoteID is invalid or empty.")]
	public string QuoteID { get; set; }

	[XmlElement(ElementName = "customerOrganizationID")]
	[DataMember(Name = "customerOrganizationID", Order = 2)]
	[Required(ErrorMessage = "CustomerOrganizationID is invalid or empty.")]
	public string CustomerOrganizationID { get; set; }

	[XmlElement(ElementName = "shipOrganizationID")]
	[DataMember(Name = "shipOrganizationID", Order = 3)]
	[Required(ErrorMessage = "ShipOrganizationID is invalid or empty.")]
	public string ShipOrganizationID { get; set; }

	[XmlElement(ElementName = "quoterEmployeeID")]
	[DataMember(Name = "quoterEmployeeID", Order = 4)]
	public string QuoterEmployeeID { get; set; }

	[XmlElement(ElementName = "currencyRateID")]
	[DataMember(Name = "currencyRateID", Order = 5)]
	public string CurrencyRateID { get; set; }
}
