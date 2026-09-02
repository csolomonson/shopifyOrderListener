using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[DataContract(Namespace = "", Name = "part")]
[XmlRoot(ElementName = "part")]
[XmlType(AnonymousType = true)]
public class BOMPartDto
{
	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 1)]
	[Required(ErrorMessage = "PartID is invalid or empty.")]
	public string PartID { get; set; } = string.Empty;

	[XmlElement(ElementName = "shortDescription")]
	[DataMember(Name = "shortDescription", Order = 2)]
	[Required(ErrorMessage = "ShortDescription is invalid or empty.")]
	public string ShortDescription { get; set; } = string.Empty;

	[XmlElement(ElementName = "partType")]
	[DataMember(Name = "partType", Order = 3)]
	[Required(ErrorMessage = "PartType is invalid or empty.")]
	public byte PartType { get; set; }

	[XmlElement(ElementName = "partClassID")]
	[DataMember(Name = "partClassID", Order = 4)]
	public string PartClassID { get; set; } = string.Empty;

	[XmlElement(ElementName = "partGroupID")]
	[DataMember(Name = "partGroupID", Order = 5)]
	public string PartGroupID { get; set; } = string.Empty;

	[XmlElement(ElementName = "longDescription")]
	[DataMember(Name = "longDescription", Order = 6)]
	public string LongDescription { get; set; } = string.Empty;

	[XmlElement(ElementName = "deliveryType")]
	[DataMember(Name = "deliveryType", Order = 7)]
	public byte? DeliveryType { get; set; }

	[XmlElement(ElementName = "buyForInventory")]
	[DataMember(Name = "buyForInventory", Order = 8)]
	public bool? BuyForInventory { get; set; }

	[XmlElement(ElementName = "nonStockedItem")]
	[DataMember(Name = "nonStockedItem", Order = 9)]
	public bool? NonStockedItem { get; set; }
}
