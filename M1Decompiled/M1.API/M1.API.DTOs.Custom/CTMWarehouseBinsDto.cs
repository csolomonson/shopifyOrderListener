using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.DTOs.Core;

namespace M1.API.DTOs.Custom;

[Serializable]
[DataContract(Namespace = "", Name = "warehouseBins")]
[XmlRoot(ElementName = "warehouseBins")]
[XmlType(AnonymousType = true)]
public class CTMWarehouseBinsDto
{
	[DataMember(Name = "warehouseBins", Order = 1)]
	[XmlArray("warehouseBins")]
	[XmlArrayItem("warehouseBin")]
	public List<WarehouseBinDto> WarehouseBins { get; set; }
}
