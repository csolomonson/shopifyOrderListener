using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.DTOs.Core;

namespace M1.API.DTOs.Custom;

[Serializable]
[DataContract(Namespace = "", Name = "warehouses")]
[XmlRoot(ElementName = "warehouses")]
[XmlType(AnonymousType = true)]
public class CTMWarehousesDto
{
	[DataMember(Name = "warehouses", Order = 1)]
	[XmlArray("warehouses")]
	[XmlArrayItem("warehouse")]
	public List<WarehouseDto> Warehouses { get; set; }
}
