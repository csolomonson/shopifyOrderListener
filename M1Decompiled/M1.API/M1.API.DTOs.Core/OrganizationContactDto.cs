using System;

namespace M1.API.DTOs.Core;

public class OrganizationContactDto
{
	public string OrganizationID { get; set; }

	public string LocationID { get; set; }

	public string ContactID { get; set; }

	public string Name { get; set; }

	public string EMailAddress { get; set; }

	public string PhoneNumber { get; set; }

	public string MobileNumber { get; set; }

	public bool Inactive { get; set; }

	public string CreatedBy { get; set; }

	public DateTime? CreatedDate { get; set; }
}
