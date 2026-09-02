using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPLeadInformationDto
{
	public string lopClosedByEmployeeID { get; set; }

	public DateTime? lopClosedDate { get; set; }

	public string lopClosedReasonID { get; set; }

	public string lopLeadID { get; set; }

	public string lopContactID { get; set; }

	public string lopCreatedBy { get; set; }

	public DateTime? lopCreatedDate { get; set; }

	public string lopCurrencyRateID { get; set; }

	public string lopCustomerOrganizationID { get; set; }

	public Guid lopUniqueID { get; set; }

	public decimal lopExchangeRate { get; set; }

	public DateTime? lopExpectedCloseDate { get; set; }

	public DateTime? lopExpirationDate { get; set; }

	public bool lopCreatedFromMobile { get; set; }

	public bool lopCustomRate { get; set; }

	public DateTime? lopLeadDate { get; set; }

	public decimal lopLeadTotal { get; set; }

	public decimal lopLeadTotalForeign { get; set; }

	public string lopLocationID { get; set; }

	public string lopLongDescriptionRtf { get; set; }

	public string lopLongDescriptionText { get; set; }

	public string lopMarketingProgramID { get; set; }

	public DateTime? lopMilestoneDate { get; set; }

	public string lopMilestoneID { get; set; }

	public string lopPlantDepartmentID { get; set; }

	public string lopPlantID { get; set; }

	public string lopProjectAreaID { get; set; }

	public string lopProjectID { get; set; }

	public string lopQuoteContactID { get; set; }

	public string lopQuoteLocationID { get; set; }

	public string lopQuoterEmployeeID { get; set; }

	public string lopReferredBy { get; set; }

	public string lopResponseMethodID { get; set; }

	public byte[] lopRowVersion { get; set; }

	public string lopShipContactID { get; set; }

	public string lopShipLocationID { get; set; }

	public string lopShipOrganizationID { get; set; }

	public string lopShortDescription { get; set; }

	public decimal lopSplitPercentTotal { get; set; }

	public string lopStatus { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
