using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ServiceContractOwners to support unicode", "2013-10-17")]
public class v810RebuildServiceContractOwners
{
	public v810RebuildServiceContractOwners(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ServiceContractOwners", new DmoField[36]
		{
			new DmoField("kboServiceContractID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kboServiceContractOwnerID", "smallint", 4, 0, nullable: false),
			new DmoField("kboCurrentOwner", "bit", 1, 0, nullable: false),
			new DmoField("kboFirstName", "nvarchar", 30, 0, nullable: false),
			new DmoField("kboMiddleName", "nvarchar", 20, 0, nullable: false),
			new DmoField("kboLastName", "nvarchar", 20, 0, nullable: false),
			new DmoField("kboOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kboAddressLine1", "nvarchar", 50, 0, nullable: false),
			new DmoField("kboAddressLine2", "nvarchar", 50, 0, nullable: false),
			new DmoField("kboAddressLine3", "nvarchar", 50, 0, nullable: false),
			new DmoField("kboCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("kboState", "nvarchar", 3, 0, nullable: false),
			new DmoField("kboPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("kboCountry", "nvarchar", 20, 0, nullable: false),
			new DmoField("kboHomePhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("kboWorkPhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("kboMobileNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("kboFaxNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("kboEmailAddress", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("kboDeliveryDate", "date", 14, 0, nullable: true),
			new DmoField("kboStartDate", "date", 14, 0, nullable: true),
			new DmoField("kboRegisteredDate", "date", 14, 0, nullable: true),
			new DmoField("kboPhysicalAddressLine1", "nvarchar", 50, 0, nullable: false),
			new DmoField("kboPhysicalAddressLine2", "nvarchar", 50, 0, nullable: false),
			new DmoField("kboPhysicalAddressLine3", "nvarchar", 50, 0, nullable: false),
			new DmoField("kboPhysicalCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("kboPhysicalState", "nvarchar", 3, 0, nullable: false),
			new DmoField("kboPhysicalPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("kboPhysicalCountry", "nvarchar", 20, 0, nullable: false),
			new DmoField("kboPhysicalLocationCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("kboPhysicalLocationState", "nvarchar", 3, 0, nullable: false),
			new DmoField("kboSameAsAbove", "bit", 1, 0, nullable: false),
			new DmoField("kboTermsAccepted", "bit", 1, 0, nullable: false),
			new DmoField("kboCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("kboCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("kboUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("KBOSERVICECONTRACTID,KBOSERVICECONTRACTOWNERID", unique: true),
			new DmoIndex("KBOUNIQUEID", unique: true),
			new DmoIndex("kboServiceContractID", unique: false),
			new DmoIndex("kboServiceContractOwnerID", unique: false),
			new DmoIndex("kboOrganizationID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
