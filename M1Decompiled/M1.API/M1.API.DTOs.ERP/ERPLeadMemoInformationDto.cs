using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPLeadMemoInformationDto
{
	public string lokCreatedBy { get; set; }

	public DateTime? lokCreatedDate { get; set; }

	public Guid lokUniqueID { get; set; }

	public string lokLeadID { get; set; }

	public string lokLongDescriptionRtf { get; set; }

	public string lokLongDescriptionText { get; set; }

	public DateTime? lokMemoDate { get; set; }

	public byte[] lokRowVersion { get; set; }

	public short lokLeadMemoID { get; set; }

	public string lokShortDescription { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
