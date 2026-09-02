using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPRFQMemoInformationDto
{
	public string rqkCreatedBy { get; set; }

	public DateTime? rqkCreatedDate { get; set; }

	public Guid rqkUniqueID { get; set; }

	public bool rqkClosed { get; set; }

	public string rqkLongDescriptionRtf { get; set; }

	public string rqkLongDescriptionText { get; set; }

	public DateTime? rqkMemoDate { get; set; }

	public string rqkRfqID { get; set; }

	public byte[] rqkRowVersion { get; set; }

	public short rqkRfqMemoID { get; set; }

	public string rqkShortDescription { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
