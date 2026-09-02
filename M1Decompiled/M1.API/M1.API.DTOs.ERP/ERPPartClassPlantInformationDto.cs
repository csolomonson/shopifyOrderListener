using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartClassPlantInformationDto
{
	public string imfPartClassPlantID { get; set; }

	public string imfCreatedBy { get; set; }

	public DateTime? imfCreatedDate { get; set; }

	public Guid imfUniqueID { get; set; }

	public string imfInventoryGlAccountID { get; set; }

	public string imfInvInInspectionGlAccountID { get; set; }

	public string imfInvInTransferGlAccountID { get; set; }

	public string imfInvToReturnGlAccountID { get; set; }

	public string imfPartClassID { get; set; }

	public byte[] imfRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
