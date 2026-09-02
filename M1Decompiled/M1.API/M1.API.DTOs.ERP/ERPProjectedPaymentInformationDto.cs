using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPProjectedPaymentInformationDto
{
	public decimal gloAmount { get; set; }

	public DateTime? gloClosedDate { get; set; }

	public string gloCreatedBy { get; set; }

	public DateTime? gloCreatedDate { get; set; }

	public string gloDescription { get; set; }

	public Guid gloUniqueID { get; set; }

	public DateTime? gloIgnoreAfterDate { get; set; }

	public bool gloClosed { get; set; }

	public string gloOrganizationID { get; set; }

	public DateTime? gloPaymentDate { get; set; }

	public byte gloPaymentType { get; set; }

	public string gloPlantDepartmentID { get; set; }

	public string gloPlantID { get; set; }

	public byte[] gloRowVersion { get; set; }

	public int gloProjectedPaymentID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
