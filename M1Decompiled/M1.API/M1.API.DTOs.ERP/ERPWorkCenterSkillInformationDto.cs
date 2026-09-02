using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPWorkCenterSkillInformationDto
{
	public string xbaCreatedBy { get; set; }

	public DateTime? xbaCreatedDate { get; set; }

	public string xbaDocuments { get; set; }

	public Guid xbaUniqueID { get; set; }

	public string xbaNotesRTF { get; set; }

	public string xbaNotesText { get; set; }

	public byte[] xbaRowVersion { get; set; }

	public short xbaWorkCenterSkillID { get; set; }

	public string xbaSkillID { get; set; }

	public string xbaWorkCenterID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
