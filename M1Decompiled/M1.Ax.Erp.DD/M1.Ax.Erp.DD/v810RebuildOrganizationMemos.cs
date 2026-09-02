using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert OrganizationMemos to support unicode", "2013-10-17")]
public class v810RebuildOrganizationMemos
{
	public v810RebuildOrganizationMemos(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationMemos", new DmoField[29]
		{
			new DmoField("cmmOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmmLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmmContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmmOrganizationMemoID", "smallint", 4, 0, nullable: false),
			new DmoField("cmmMemoDate", "date", 14, 0, nullable: true),
			new DmoField("cmmShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("cmmLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("cmmLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("cmmShowInOrganizations", "bit", 1, 0, nullable: false),
			new DmoField("cmmShowInPriceAndAvailability", "bit", 1, 0, nullable: false),
			new DmoField("cmmShowInCalls", "bit", 1, 0, nullable: false),
			new DmoField("cmmShowInLeads", "bit", 1, 0, nullable: false),
			new DmoField("cmmShowInQuotes", "bit", 1, 0, nullable: false),
			new DmoField("cmmShowInSalesOrders", "bit", 1, 0, nullable: false),
			new DmoField("cmmShowInShipments", "bit", 1, 0, nullable: false),
			new DmoField("cmmShowInARInvoices", "bit", 1, 0, nullable: false),
			new DmoField("cmmShowInARPayments", "bit", 1, 0, nullable: false),
			new DmoField("cmmShowInRFQs", "bit", 1, 0, nullable: false),
			new DmoField("cmmShowInPurchaseOrders", "bit", 1, 0, nullable: false),
			new DmoField("cmmShowInReceipts", "bit", 1, 0, nullable: false),
			new DmoField("cmmShowInAPInvoices", "bit", 1, 0, nullable: false),
			new DmoField("cmmShowInAPPayments", "bit", 1, 0, nullable: false),
			new DmoField("cmmShowInRMAClaims", "bit", 1, 0, nullable: false),
			new DmoField("cmmShowInRMAReceipts", "bit", 1, 0, nullable: false),
			new DmoField("cmmShowInDMRClaims", "bit", 1, 0, nullable: false),
			new DmoField("cmmShowInDMRShipments", "bit", 1, 0, nullable: false),
			new DmoField("cmmCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmmCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmmUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[25]
		{
			new DmoIndex("CMMORGANIZATIONID,CMMLOCATIONID,CMMCONTACTID,CMMORGANIZATIONMEMOID", unique: true),
			new DmoIndex("CMMUNIQUEID", unique: true),
			new DmoIndex("cmmOrganizationID", unique: false),
			new DmoIndex("cmmLocationID", unique: false),
			new DmoIndex("cmmContactID", unique: false),
			new DmoIndex("cmmOrganizationMemoID", unique: false),
			new DmoIndex("cmmMemoDate", unique: false),
			new DmoIndex("cmmShowInOrganizations", unique: false),
			new DmoIndex("cmmShowInPriceAndAvailability", unique: false),
			new DmoIndex("cmmShowInCalls", unique: false),
			new DmoIndex("cmmShowInLeads", unique: false),
			new DmoIndex("cmmShowInQuotes", unique: false),
			new DmoIndex("cmmShowInSalesOrders", unique: false),
			new DmoIndex("cmmShowInShipments", unique: false),
			new DmoIndex("cmmShowInARInvoices", unique: false),
			new DmoIndex("cmmShowInARPayments", unique: false),
			new DmoIndex("cmmShowInRFQs", unique: false),
			new DmoIndex("cmmShowInPurchaseOrders", unique: false),
			new DmoIndex("cmmShowInReceipts", unique: false),
			new DmoIndex("cmmShowInAPInvoices", unique: false),
			new DmoIndex("cmmShowInAPPayments", unique: false),
			new DmoIndex("cmmShowInRMAClaims", unique: false),
			new DmoIndex("cmmShowInRMAReceipts", unique: false),
			new DmoIndex("cmmShowInDMRClaims", unique: false),
			new DmoIndex("cmmShowInDMRShipments", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
