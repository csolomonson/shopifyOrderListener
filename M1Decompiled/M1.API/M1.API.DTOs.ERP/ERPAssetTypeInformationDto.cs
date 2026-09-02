using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAssetTypeInformationDto
{
	public string fatAccumDeprGlAccountID { get; set; }

	public string fatAssetGlAccountID { get; set; }

	public string fatAssetTypeID { get; set; }

	public string fatCreatedBy { get; set; }

	public DateTime? fatCreatedDate { get; set; }

	public string fatDepreciationGlAccountID { get; set; }

	public string fatDescription { get; set; }

	public Guid fatUniqueID { get; set; }

	public string fatExpenseGlAccountID { get; set; }

	public string fatLossGlAccountID { get; set; }

	public string fatProfitGlAccountID { get; set; }

	public string fatRepairsGlAccountID { get; set; }

	public string fatRevaluationGlAccountID { get; set; }

	public byte[] fatRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
