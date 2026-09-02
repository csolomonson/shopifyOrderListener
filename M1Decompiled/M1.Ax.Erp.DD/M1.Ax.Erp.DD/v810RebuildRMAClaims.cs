using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert RMAClaims to support unicode", "2013-10-17")]
public class v810RebuildRMAClaims
{
	public v810RebuildRMAClaims(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaims", new DmoField[50]
		{
			new DmoField("rapRMAClaimID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rapPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rapPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rapProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rapReference", "nvarchar", 30, 0, nullable: false),
			new DmoField("rapCustomerOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rapARInvoiceLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rapARInvoiceContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rapShipOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rapShipLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rapShipContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rapResellerOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rapResellerLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rapResellerContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rapPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("rapPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("rapSerialNumberID", "nvarchar", 30, 0, nullable: false),
			new DmoField("rapPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("rapStatus", "nvarchar", 1, 0, nullable: false),
			new DmoField("rapClaimDate", "datetime", 14, 0, nullable: true),
			new DmoField("rapRequestedDate", "datetime", 14, 0, nullable: true),
			new DmoField("rapLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("rapLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("rapAuthorizedByEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rapAuthorizationNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("rapAuthorizationDate", "datetime", 14, 0, nullable: true),
			new DmoField("rapClosedDate", "datetime", 14, 0, nullable: true),
			new DmoField("rapClosedReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rapProcessedByEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rapLaborRate", "numeric", 8, 4, nullable: false),
			new DmoField("rapLaborRateForeign", "numeric", 8, 4, nullable: false),
			new DmoField("rapCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rapCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("rapExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("rapPartsTotalForeign", "money", 12, 2, nullable: false),
			new DmoField("rapLaborTotalForeign", "money", 12, 2, nullable: false),
			new DmoField("rapSubcontractTotalForeign", "money", 12, 2, nullable: false),
			new DmoField("rapDiscountAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("rapFreightAmount", "money", 12, 2, nullable: false),
			new DmoField("rapFreightAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("rapClaimTotalForeign", "money", 12, 2, nullable: false),
			new DmoField("rapPartsTotal", "money", 12, 2, nullable: false),
			new DmoField("rapLaborTotal", "money", 12, 2, nullable: false),
			new DmoField("rapSubcontractTotal", "money", 12, 2, nullable: false),
			new DmoField("rapDiscountAmount", "money", 12, 2, nullable: false),
			new DmoField("rapClaimTotal", "money", 12, 2, nullable: false),
			new DmoField("rapPayTo", "tinyint", 1, 0, nullable: false),
			new DmoField("rapCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("rapCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("rapUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[16]
		{
			new DmoIndex("RAPRMACLAIMID", unique: true),
			new DmoIndex("RAPUNIQUEID", unique: true),
			new DmoIndex("rapPlantDepartmentID", unique: false),
			new DmoIndex("rapPlantID", unique: false),
			new DmoIndex("rapProjectID", unique: false),
			new DmoIndex("rapCustomerOrganizationID", unique: false),
			new DmoIndex("rapARInvoiceLocationID", unique: false),
			new DmoIndex("rapShipOrganizationID", unique: false),
			new DmoIndex("rapResellerOrganizationID", unique: false),
			new DmoIndex("rapPartID", unique: false),
			new DmoIndex("rapPartRevisionID", unique: false),
			new DmoIndex("rapSerialNumberID", unique: false),
			new DmoIndex("rapStatus", unique: false),
			new DmoIndex("rapAuthorizedByEmployeeID", unique: false),
			new DmoIndex("rapClosedReasonID", unique: false),
			new DmoIndex("rapProcessedByEmployeeID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
