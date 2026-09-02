using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.Core;

[Serializable]
[DataContract(Namespace = "", Name = "warehouseBin")]
[XmlRoot(ElementName = "warehouseBin")]
[XmlType(AnonymousType = true)]
public class WarehouseBinDto
{
	[XmlElement(ElementName = "warehouseID")]
	[DataMember(Name = "warehouseID", Order = 1)]
	[Required(ErrorMessage = "WarehouseID is invalid or empty.")]
	public string WarehouseID { get; set; }

	[XmlElement(ElementName = "warehouseBinID")]
	[DataMember(Name = "warehouseBinID", Order = 2)]
	[Required(ErrorMessage = "WarehouseBinID is invalid or empty.")]
	public string WarehouseBinID { get; set; }

	[XmlElement(ElementName = "description")]
	[DataMember(Name = "description", Order = 3)]
	[Required(ErrorMessage = "Description is invalid or empty.")]
	public string Description { get; set; }

	[XmlElement(ElementName = "defaultBin")]
	[DataMember(Name = "defaultBin", Order = 4)]
	public bool DefaultBin { get; set; }
}
