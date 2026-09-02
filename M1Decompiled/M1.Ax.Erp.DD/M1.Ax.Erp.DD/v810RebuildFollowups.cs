using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Followups to support unicode", "2013-10-17")]
public class v810RebuildFollowups
{
	public v810RebuildFollowups(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Followups", new DmoField[38]
		{
			new DmoField("cmfFollowupID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfFollowupType", "tinyint", 1, 0, nullable: false),
			new DmoField("cmfOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmfContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmfAttachedToEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfMeetingLocation", "nvarchar", 50, 0, nullable: false),
			new DmoField("cmfShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("cmfLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("cmfLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("cmfStartDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmfDueDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmfAssignedToEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfStatus", "tinyint", 1, 0, nullable: false),
			new DmoField("cmfCompletedDate", "date", 14, 0, nullable: true),
			new DmoField("cmfPriority", "tinyint", 1, 0, nullable: false),
			new DmoField("cmfCallID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfLeadID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfQuoteID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmfShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfARInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfRFQID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfPurchaseOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfReceiptID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfAPInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfRMAClaimID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfDMRClaimID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfProjectAreaID", "nvarchar", 15, 0, nullable: false),
			new DmoField("cmfAssetID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfChangeRequestID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmfCreatedFromMobile", "bit", 1, 0, nullable: false),
			new DmoField("cmfCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmfCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmfUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("cmfExchangeID", "nvarchar(max)", 50, 0, nullable: true)
		}, new DmoIndex[27]
		{
			new DmoIndex("CMFFOLLOWUPID", unique: true),
			new DmoIndex("CMFUNIQUEID", unique: true),
			new DmoIndex("cmfOrganizationID", unique: false),
			new DmoIndex("cmfLocationID", unique: false),
			new DmoIndex("cmfContactID", unique: false),
			new DmoIndex("cmfAttachedToEmployeeID", unique: false),
			new DmoIndex("cmfStartDate", unique: false),
			new DmoIndex("cmfDueDate", unique: false),
			new DmoIndex("cmfAssignedToEmployeeID", unique: false),
			new DmoIndex("cmfStatus", unique: false),
			new DmoIndex("cmfCallID", unique: false),
			new DmoIndex("cmfLeadID", unique: false),
			new DmoIndex("cmfQuoteID", unique: false),
			new DmoIndex("cmfSalesOrderID", unique: false),
			new DmoIndex("cmfJobID", unique: false),
			new DmoIndex("cmfShipmentID", unique: false),
			new DmoIndex("cmfARInvoiceID", unique: false),
			new DmoIndex("cmfRFQID", unique: false),
			new DmoIndex("cmfPurchaseOrderID", unique: false),
			new DmoIndex("cmfReceiptID", unique: false),
			new DmoIndex("cmfAPInvoiceID", unique: false),
			new DmoIndex("cmfProjectID", unique: false),
			new DmoIndex("cmfRMAClaimID", unique: false),
			new DmoIndex("cmfDMRClaimID", unique: false),
			new DmoIndex("cmfProjectAreaID", unique: false),
			new DmoIndex("cmfAssetID", unique: false),
			new DmoIndex("cmfChangeRequestID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
