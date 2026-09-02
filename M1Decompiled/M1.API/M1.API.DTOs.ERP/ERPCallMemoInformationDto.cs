using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPCallMemoInformationDto
{
	public string kbkCallID { get; set; }

	public string kbkCreatedBy { get; set; }

	public DateTime? kbkCreatedDate { get; set; }

	public Guid kbkUniqueID { get; set; }

	public string kbkLongDescriptionRtf { get; set; }

	public string kbkLongDescriptionText { get; set; }

	public DateTime? kbkMemoDate { get; set; }

	public byte[] kbkRowVersion { get; set; }

	public short kbkCallMemoID { get; set; }

	public string kbkShortDescription { get; set; }

	public bool kbkShowInCalls { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
