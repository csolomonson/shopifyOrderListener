using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert DMRClaims to support unicode", "2013-10-17")]
public class v810RebuildDMRClaims
{
	public v810RebuildDMRClaims(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRClaims", new DmoField[27]
		{
			new DmoField("dmpDMRClaimID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dmpPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dmpPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dmpProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dmpReference", "nvarchar", 30, 0, nullable: false),
			new DmoField("dmpSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dmpAPInvoiceLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dmpAPInvoiceContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dmpPurchaseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dmpPurchaseContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dmpStatus", "nvarchar", 1, 0, nullable: false),
			new DmoField("dmpClaimDate", "datetime", 14, 0, nullable: true),
			new DmoField("dmpRequestedDate", "datetime", 14, 0, nullable: true),
			new DmoField("dmpAuthorizedByEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dmpAuthorizationNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("dmpAuthorizationDate", "datetime", 14, 0, nullable: true),
			new DmoField("dmpClosedDate", "datetime", 14, 0, nullable: true),
			new DmoField("dmpClosedReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dmpProcessedByEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dmpCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dmpCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("dmpExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("dmpClaimTotalForeign", "money", 12, 2, nullable: false),
			new DmoField("dmpClaimTotal", "money", 12, 2, nullable: false),
			new DmoField("dmpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("dmpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("dmpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[11]
		{
			new DmoIndex("DMPDMRCLAIMID", unique: true),
			new DmoIndex("DMPUNIQUEID", unique: true),
			new DmoIndex("dmpPlantDepartmentID", unique: false),
			new DmoIndex("dmpPlantID", unique: false),
			new DmoIndex("dmpProjectID", unique: false),
			new DmoIndex("dmpSupplierOrganizationID", unique: false),
			new DmoIndex("dmpAPInvoiceLocationID", unique: false),
			new DmoIndex("dmpStatus", unique: false),
			new DmoIndex("dmpAuthorizedByEmployeeID", unique: false),
			new DmoIndex("dmpClosedReasonID", unique: false),
			new DmoIndex("dmpProcessedByEmployeeID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
