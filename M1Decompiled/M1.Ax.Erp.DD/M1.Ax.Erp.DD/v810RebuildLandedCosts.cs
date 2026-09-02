using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert LandedCosts to support unicode", "2013-10-17")]
public class v810RebuildLandedCosts
{
	public v810RebuildLandedCosts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCosts", new DmoField[29]
		{
			new DmoField("rmcLandedCostID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rmcLandedCostDate", "datetime", 14, 0, nullable: true),
			new DmoField("rmcClosed", "bit", 1, 0, nullable: false),
			new DmoField("rmcClosedDate", "date", 14, 0, nullable: true),
			new DmoField("rmcShipOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rmcShipLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmcShipContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmcConsigneeOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rmcConsigneeLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmcConsigneeContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmcLoadingPoint", "nvarchar", 30, 0, nullable: false),
			new DmoField("rmcDischargePoint", "nvarchar", 30, 0, nullable: false),
			new DmoField("rmcCarrierName", "nvarchar", 30, 0, nullable: false),
			new DmoField("rmcTrackingNumber", "nvarchar", 30, 0, nullable: false),
			new DmoField("rmcLandedCostChargesTotal", "money", 12, 2, nullable: false),
			new DmoField("rmcLandedCostPurchasesTotal", "money", 12, 2, nullable: false),
			new DmoField("rmcLandedCostTotal", "money", 12, 2, nullable: false),
			new DmoField("rmcLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("rmcLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("rmcPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("rmcPostedDate", "date", 14, 0, nullable: true),
			new DmoField("rmcGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("rmcGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("rmcReversalEntry", "bit", 1, 0, nullable: false),
			new DmoField("rmcReverseLandedCostID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rmcReversed", "bit", 1, 0, nullable: false),
			new DmoField("rmcCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("rmcCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("rmcUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("RMCLANDEDCOSTID", unique: true),
			new DmoIndex("RMCUNIQUEID", unique: true),
			new DmoIndex("rmcClosed", unique: false),
			new DmoIndex("rmcShipOrganizationID", unique: false),
			new DmoIndex("rmcConsigneeOrganizationID", unique: false),
			new DmoIndex("rmcGLFiscalYearID", unique: false),
			new DmoIndex("rmcGLFiscalYearPeriodID", unique: false),
			new DmoIndex("rmcReverseLandedCostID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
