using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.DTOs.BOM.Transaction;

namespace M1.API.DTOs.Custom.Transaction;

[Serializable]
[XmlRoot(ElementName = "materialIssueLine")]
[DataContract(Namespace = "", Name = "materialIssueLine")]
[XmlType(AnonymousType = true)]
public class CTMBOMMaterialIssueLineDto
{
	[XmlElement(ElementName = "materialIssue")]
	[DataMember(Name = "materialIssue", Order = 1)]
	[Required(ErrorMessage = "Material issue is invalid or empty.")]
	public BOMMaterialIssueDto MaterialIssue { get; set; }

	[DataMember(Name = "materialIssueLines", Order = 2)]
	[XmlArray("materialIssueLines")]
	[XmlArrayItem("materialIssueLine")]
	public List<BOMMaterialIssueLineDto> MaterialIssueLines { get; set; }

	public CTMBOMMaterialIssueLineDto()
	{
		MaterialIssue = new BOMMaterialIssueDto();
		MaterialIssueLines = new List<BOMMaterialIssueLineDto>();
	}
}
