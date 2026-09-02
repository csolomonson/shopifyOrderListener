using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPMaterialIssueInformationDto
{
	public string iniMaterialIssueID { get; set; }

	public string iniCreatedBy { get; set; }

	public DateTime? iniCreatedDate { get; set; }

	public Guid iniUniqueID { get; set; }

	public bool iniPosted { get; set; }

	public bool iniReversalEntry { get; set; }

	public bool iniReversed { get; set; }

	public DateTime? iniMaterialIssueDate { get; set; }

	public DateTime? iniPostedDate { get; set; }

	public byte[] iniRowVersion { get; set; }

	public Guid iniSourceTableUniqueID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
