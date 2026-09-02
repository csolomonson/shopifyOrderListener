using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPWorkCenterSkillCompetencyInformationDto
{
	public string xbbCommentsRTF { get; set; }

	public string xbbCommentsText { get; set; }

	public string xbbCompetencyID { get; set; }

	public string xbbCreatedBy { get; set; }

	public DateTime? xbbCreatedDate { get; set; }

	public DateTime? xbbDateAchieved { get; set; }

	public DateTime? xbbDateExpires { get; set; }

	public Guid xbbUniqueID { get; set; }

	public byte[] xbbRowVersion { get; set; }

	public short xbbWorkCenterSkillCompetencyID { get; set; }

	public string xbbSkillID { get; set; }

	public string xbbWorkCenterID { get; set; }

	public short xbbWorkCenterSkillID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
