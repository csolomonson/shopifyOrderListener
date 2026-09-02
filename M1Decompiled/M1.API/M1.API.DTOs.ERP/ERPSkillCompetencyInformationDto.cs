using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPSkillCompetencyInformationDto
{
	public int lecColor { get; set; }

	public string lecCompetencyID { get; set; }

	public string lecCreatedBy { get; set; }

	public DateTime? lecCreatedDate { get; set; }

	public string lecDescription { get; set; }

	public Guid lecUniqueID { get; set; }

	public DateTime? lecInactiveDate { get; set; }

	public bool lecInactive { get; set; }

	public byte lecLevel { get; set; }

	public string lecLongDescriptionRtf { get; set; }

	public string lecLongDescriptionText { get; set; }

	public byte[] lecRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
