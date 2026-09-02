using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert OrganizationPaymentMethods to support unicode", "2013-10-17")]
public class v810RebuildOrganizationPaymentMethods
{
	public v810RebuildOrganizationPaymentMethods(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationPaymentMethods", new DmoField[19]
		{
			new DmoField("cmpOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmpLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmpContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmpPaymentMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmpPaymentType", "nvarchar", 2, 0, nullable: false),
			new DmoField("cmpAcctSuffix", "nvarchar", 4, 0, nullable: false),
			new DmoField("cmpVerisignPnRef", "nvarchar", 12, 0, nullable: false),
			new DmoField("cmpVerifiedDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmpCardType", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmpExpDateMMYY", "nvarchar", 4, 0, nullable: false),
			new DmoField("cmpCreditUsableDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmpCreditUsablePnRef", "nvarchar", 12, 0, nullable: false),
			new DmoField("cmpCardDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("cmpInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("cmpSAGEGUID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("cmpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmpInactive", "bit", 1, 0, nullable: false),
			new DmoField("cmpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("CMPORGANIZATIONID,CMPLOCATIONID,CMPCONTACTID,CMPPAYMENTMETHODID", unique: true),
			new DmoIndex("CMPUNIQUEID", unique: true),
			new DmoIndex("cmpOrganizationID", unique: false),
			new DmoIndex("cmpLocationID", unique: false),
			new DmoIndex("cmpPaymentMethodID", unique: false),
			new DmoIndex("cmpPaymentType", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
