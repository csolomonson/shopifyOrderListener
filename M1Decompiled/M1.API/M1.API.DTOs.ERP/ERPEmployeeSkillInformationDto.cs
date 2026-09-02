using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPEmployeeSkillInformationDto
{
	public string lnkCreatedBy { get; set; }

	public DateTime? lnkCreatedDate { get; set; }

	public string lnkDocuments { get; set; }

	public string lnkEmployeeID { get; set; }

	public Guid lnkUniqueID { get; set; }

	public string lnkNotesRTF { get; set; }

	public string lnkNotesText { get; set; }

	public byte[] lnkRowVersion { get; set; }

	public short lnkEmployeeSkillID { get; set; }

	public string lnkSkillID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
