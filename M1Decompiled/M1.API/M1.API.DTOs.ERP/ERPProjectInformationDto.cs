using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPProjectInformationDto
{
	public DateTime? prpClosedDate { get; set; }

	public string prpProjectID { get; set; }

	public string prpContactID { get; set; }

	public string prpCreatedBy { get; set; }

	public DateTime? prpCreatedDate { get; set; }

	public DateTime? prpDueDate { get; set; }

	public Guid prpUniqueID { get; set; }

	public bool prpClosed { get; set; }

	public string prpLocationID { get; set; }

	public string prpLongDescriptionRtf { get; set; }

	public string prpLongDescriptionText { get; set; }

	public string prpOrganizationID { get; set; }

	public DateTime? prpProjectDate { get; set; }

	public string prpProjectManagerEmployeeID { get; set; }

	public string prpProjectTypeID { get; set; }

	public byte[] prpRowVersion { get; set; }

	public string prpShortDescription { get; set; }

	public string prpStatus { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
