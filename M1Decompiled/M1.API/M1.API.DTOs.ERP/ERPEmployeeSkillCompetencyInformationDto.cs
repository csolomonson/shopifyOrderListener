using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPEmployeeSkillCompetencyInformationDto
{
	public string lnpCommentsRTF { get; set; }

	public string lnpCommentsText { get; set; }

	public string lnpCompetencyID { get; set; }

	public string lnpCreatedBy { get; set; }

	public DateTime? lnpCreatedDate { get; set; }

	public DateTime? lnpDateAchieved { get; set; }

	public DateTime? lnpDateExpires { get; set; }

	public string lnpEmployeeID { get; set; }

	public short lnpEmployeeSkillID { get; set; }

	public Guid lnpUniqueID { get; set; }

	public byte[] lnpRowVersion { get; set; }

	public short lnpEmployeeSkillCompetencyID { get; set; }

	public string lnpSkillID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
