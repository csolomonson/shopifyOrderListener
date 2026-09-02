using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPReasonInformationDto
{
	public string xarReasonID { get; set; }

	public string xarCreatedBy { get; set; }

	public DateTime? xarCreatedDate { get; set; }

	public string xarDescription { get; set; }

	public Guid xarUniqueID { get; set; }

	public string xarReasonGlAccountID { get; set; }

	public string xarReasonType { get; set; }

	public byte[] xarRowVersion { get; set; }

	public string xarScrapGlAccountID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
