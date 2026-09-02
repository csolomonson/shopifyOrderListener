using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAssetTypePlantInformationDto
{
	public string fayAccumDeprGlAccountID { get; set; }

	public string fayAssetGlAccountID { get; set; }

	public string fayAssetTypeID { get; set; }

	public string fayAssetTypePlantID { get; set; }

	public string fayCreatedBy { get; set; }

	public DateTime? fayCreatedDate { get; set; }

	public string fayDepreciationGlAccountID { get; set; }

	public Guid fayUniqueID { get; set; }

	public string fayExpenseGlAccountID { get; set; }

	public string fayLossGlAccountID { get; set; }

	public string fayProfitGlAccountID { get; set; }

	public string fayRepairsGlAccountID { get; set; }

	public string fayRevaluationGlAccountID { get; set; }

	public byte[] fayRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
