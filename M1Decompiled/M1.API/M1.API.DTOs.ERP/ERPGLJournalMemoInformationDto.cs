using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPGLJournalMemoInformationDto
{
	public string glmCreatedBy { get; set; }

	public DateTime? glmCreatedDate { get; set; }

	public Guid glmUniqueID { get; set; }

	public int glmGlJournalID { get; set; }

	public bool glmClosed { get; set; }

	public string glmLongDescriptionRtf { get; set; }

	public string glmLongDescriptionText { get; set; }

	public DateTime? glmMemoDate { get; set; }

	public byte[] glmRowVersion { get; set; }

	public short glmGlJournalMemoID { get; set; }

	public string glmShortDescription { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
