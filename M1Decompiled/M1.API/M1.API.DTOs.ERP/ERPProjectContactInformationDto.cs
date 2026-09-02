using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPProjectContactInformationDto
{
	public string prcContactID { get; set; }

	public string prcContactTitleID { get; set; }

	public string prcCreatedBy { get; set; }

	public DateTime? prcCreatedDate { get; set; }

	public Guid prcUniqueID { get; set; }

	public string prcLocationID { get; set; }

	public string prcNotesRTF { get; set; }

	public string prcNotesText { get; set; }

	public string prcOrganizationID { get; set; }

	public string prcProjectID { get; set; }

	public byte[] prcRowVersion { get; set; }

	public short prcProjectContactID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
