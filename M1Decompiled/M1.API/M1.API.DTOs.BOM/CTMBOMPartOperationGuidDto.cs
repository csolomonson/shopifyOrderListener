using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[XmlRoot(ElementName = "partOperationGuid")]
[DataContract(Namespace = "", Name = "partOperationGuid")]
[XmlType(AnonymousType = true)]
public class CTMBOMPartOperationGuidDto
{
	[XmlElement(ElementName = "operationId")]
	[DataMember(Name = "operationId", Order = 1)]
	public int OperationId { get; set; }

	[XmlElement(ElementName = "operationGuid")]
	[DataMember(Name = "operationGuid", Order = 2)]
	public string OperationGuid { get; set; }
}
