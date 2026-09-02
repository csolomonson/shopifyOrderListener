using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPSkillInformationDto
{
	public string lesSkillID { get; set; }

	public string lesCreatedBy { get; set; }

	public DateTime? lesCreatedDate { get; set; }

	public string lesDescription { get; set; }

	public Guid lesUniqueID { get; set; }

	public DateTime? lesInactiveDate { get; set; }

	public bool lesInactive { get; set; }

	public string lesLongDescriptionRtf { get; set; }

	public string lesLongDescriptionText { get; set; }

	public byte[] lesRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
