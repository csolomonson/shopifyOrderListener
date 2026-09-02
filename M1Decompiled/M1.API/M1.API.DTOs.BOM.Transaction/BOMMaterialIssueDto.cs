using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Transaction;

[Serializable]
[DataContract(Namespace = "", Name = "materialissue")]
[XmlRoot(ElementName = "materialissue")]
[XmlType(AnonymousType = true)]
public class BOMMaterialIssueDto
{
	[XmlElement(ElementName = "materialIssueID")]
	[DataMember(Name = "materialIssueID", Order = 1)]
	public string MaterialIssueID { get; set; }

	[XmlElement(ElementName = "materialIssueDate")]
	[DataMember(Name = "materialIssueDate", Order = 2)]
	public DateTime? MaterialIssueDate { get; set; }

	[XmlElement(ElementName = "createdDate")]
	[DataMember(Name = "createdDate", Order = 3)]
	public DateTime? CreatedDate { get; set; }

	[XmlElement(ElementName = "createdBy")]
	[DataMember(Name = "createdBy", Order = 4)]
	public string CreatedBy { get; set; }

	[XmlElement(ElementName = "postedDate")]
	[DataMember(Name = "postedDate", Order = 5)]
	public DateTime? PostedDate { get; set; }

	[XmlElement(ElementName = "posted")]
	[DataMember(Name = "posted", Order = 6)]
	public bool Posted { get; set; }

	[XmlElement(ElementName = "reversalEntry")]
	[DataMember(Name = "reversalEntry", Order = 7)]
	public bool ReversalEntry { get; set; }

	[XmlElement(ElementName = "reversed")]
	[DataMember(Name = "reversed", Order = 8)]
	public bool Reversed { get; set; }

	[XmlElement(ElementName = "uniqueID")]
	[DataMember(Name = "uniqueID", Order = 9)]
	public Guid UniqueID { get; set; }

	[XmlElement(ElementName = "rowVersion")]
	[DataMember(Name = "rowVersion", Order = 10)]
	public byte[] RowVersion { get; set; }
}
