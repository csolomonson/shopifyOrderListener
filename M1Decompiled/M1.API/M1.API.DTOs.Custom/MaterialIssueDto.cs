using System;

namespace M1.API.DTOs.Custom;

public class MaterialIssueDto
{
	public string MaterialIssueID { get; set; }

	public DateTime? MaterialIssueDate { get; set; }

	public DateTime? CreatedDate { get; set; }

	public string CreatedBy { get; set; }

	public DateTime? PostedDate { get; set; }

	public bool Posted { get; set; }

	public bool ReversalEntry { get; set; }

	public bool Reversed { get; set; }

	public Guid UniqueID { get; set; }

	public byte[] RowVersion { get; set; }
}
