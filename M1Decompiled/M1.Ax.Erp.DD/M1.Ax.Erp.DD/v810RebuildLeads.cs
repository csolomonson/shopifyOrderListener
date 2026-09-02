using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Leads to support unicode", "2013-10-17")]
public class v810RebuildLeads
{
	public v810RebuildLeads(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Leads", new DmoField[38]
		{
			new DmoField("lopLeadID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lopPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lopPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lopCustomerOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lopLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lopContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lopShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("lopLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lopLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lopMilestoneID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lopMilestoneDate", "date", 14, 0, nullable: true),
			new DmoField("lopLeadTotal", "money", 12, 2, nullable: false),
			new DmoField("lopLeadTotalForeign", "money", 12, 2, nullable: false),
			new DmoField("lopResponseMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lopMarketingProgramID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lopReferredBy", "nvarchar", 50, 0, nullable: false),
			new DmoField("lopQuoterEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lopLeadDate", "date", 14, 0, nullable: true),
			new DmoField("lopExpectedCloseDate", "date", 14, 0, nullable: true),
			new DmoField("lopExpirationDate", "date", 14, 0, nullable: true),
			new DmoField("lopStatus", "nvarchar", 1, 0, nullable: false),
			new DmoField("lopClosedReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lopClosedByEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lopClosedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lopProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lopProjectAreaID", "nvarchar", 15, 0, nullable: false),
			new DmoField("lopQuoteLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lopShipLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lopShipOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lopQuoteContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lopShipContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lopCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lopExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("lopCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("lopCreatedFromMobile", "bit", 1, 0, nullable: false),
			new DmoField("lopCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lopCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lopUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[20]
		{
			new DmoIndex("LOPLEADID", unique: true),
			new DmoIndex("LOPUNIQUEID", unique: true),
			new DmoIndex("lopPlantDepartmentID", unique: false),
			new DmoIndex("lopPlantID", unique: false),
			new DmoIndex("lopCustomerOrganizationID", unique: false),
			new DmoIndex("lopLocationID", unique: false),
			new DmoIndex("lopContactID", unique: false),
			new DmoIndex("lopMilestoneID", unique: false),
			new DmoIndex("lopResponseMethodID", unique: false),
			new DmoIndex("lopQuoterEmployeeID", unique: false),
			new DmoIndex("lopStatus", unique: false),
			new DmoIndex("lopClosedReasonID", unique: false),
			new DmoIndex("lopClosedByEmployeeID", unique: false),
			new DmoIndex("lopProjectID", unique: false),
			new DmoIndex("lopProjectAreaID", unique: false),
			new DmoIndex("lopQuoteLocationID", unique: false),
			new DmoIndex("lopShipLocationID", unique: false),
			new DmoIndex("lopShipOrganizationID", unique: false),
			new DmoIndex("lopQuoteContactID", unique: false),
			new DmoIndex("lopShipContactID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
