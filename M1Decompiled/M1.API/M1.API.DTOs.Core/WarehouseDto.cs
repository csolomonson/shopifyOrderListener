using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.Core;

[Serializable]
[DataContract(Namespace = "", Name = "warehouse")]
[XmlRoot(ElementName = "warehouse")]
[XmlType(AnonymousType = true)]
public class WarehouseDto
{
	[XmlElement(ElementName = "warehouseID")]
	[DataMember(Name = "warehouseID", Order = 1)]
	[Required(ErrorMessage = "WarehouseID is invalid or empty.")]
	public string WarehouseID { get; set; }

	[XmlElement(ElementName = "name")]
	[DataMember(Name = "name", Order = 2)]
	[Required(ErrorMessage = "Name is invalid or empty.")]
	public string Name { get; set; }

	[XmlElement(ElementName = "plantID")]
	[DataMember(Name = "plantID", Order = 3)]
	public string PlantID { get; set; }

	[XmlElement(ElementName = "defaultWarehouse")]
	[DataMember(Name = "defaultWarehouse", Order = 4)]
	public bool DefaultWarehouse { get; set; }
}
