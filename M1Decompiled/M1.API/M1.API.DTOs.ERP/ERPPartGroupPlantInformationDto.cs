using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartGroupPlantInformationDto
{
	public string imvArDepositGlAccountID { get; set; }

	public string imvPartGroupPlantID { get; set; }

	public string imvCogsLaborGlAccountID { get; set; }

	public string imvCogsMaterialGlAccountID { get; set; }

	public string imvCogsOverheadGlAccountID { get; set; }

	public string imvCogsSubcontractGlAccountID { get; set; }

	public string imvCreatedBy { get; set; }

	public DateTime? imvCreatedDate { get; set; }

	public string imvDiscountGlAccountID { get; set; }

	public Guid imvUniqueID { get; set; }

	public string imvPartGroupID { get; set; }

	public byte[] imvRowVersion { get; set; }

	public string imvSalesGlAccountID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
