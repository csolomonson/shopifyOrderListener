using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert OrganizationContacts to support unicode", "2013-10-17")]
public class v810RebuildOrganizationContacts
{
	public v810RebuildOrganizationContacts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationContacts", new DmoField[45]
		{
			new DmoField("cmcOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmcLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmcContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmcContactTitleID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmcName", "nvarchar", 50, 0, nullable: false),
			new DmoField("cmcPhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmcAlternatePhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmcMobileNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmcFaxNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmcEMailAddress", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("cmcWebLoginEnabled", "bit", 1, 0, nullable: false),
			new DmoField("cmcWebUserID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmcWebPassword", "nvarchar", 80, 0, nullable: false),
			new DmoField("cmcWebTemplate", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmcWebExpirationDate", "date", 14, 0, nullable: true),
			new DmoField("cmcNoMailings", "bit", 1, 0, nullable: false),
			new DmoField("cmcCorrespondenceMethod", "nvarchar", 1, 0, nullable: false),
			new DmoField("cmcNoteRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("cmcNoteText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("cmcInactive", "bit", 1, 0, nullable: false),
			new DmoField("cmcInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("cmcCreatedFromMobile", "bit", 1, 0, nullable: false),
			new DmoField("cmcCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmcCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmcUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("cmcEasyOrderEnabled", "bit", 1, 0, nullable: false),
			new DmoField("cmcCreatedByEasyOrder", "bit", 1, 0, nullable: false),
			new DmoField("cmcEOInitials", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmcEOPrefix", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmcEOSurname", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmcEOPassword", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmcEOUserRole", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmcEODefSupervisor", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmcEOSubSupervisor", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmcEOCustomerGroup", "nvarchar", 100, 0, nullable: false),
			new DmoField("cmcEOMultiShipAddress", "nvarchar", 1, 0, nullable: false),
			new DmoField("cmcEOReceiveOrderConfirmation", "nvarchar", 1, 0, nullable: false),
			new DmoField("cmcEOEditShippingAddress", "bit", 1, 0, nullable: false),
			new DmoField("cmcEOReceiveEMails", "bit", 1, 0, nullable: false),
			new DmoField("cmcEOHTMLMail", "bit", 1, 0, nullable: false),
			new DmoField("cmcEOReminderOfOpenOrders", "bit", 1, 0, nullable: false),
			new DmoField("cmcEOOrderAuthorisationMessage", "bit", 1, 0, nullable: false),
			new DmoField("cmcEOAuthorisationRequest", "bit", 1, 0, nullable: false),
			new DmoField("cmcEOMayNotCreOrdTemp", "bit", 1, 0, nullable: false),
			new DmoField("cmcEOFirstName", "nvarchar", 20, 0, nullable: false)
		}, new DmoIndex[11]
		{
			new DmoIndex("CMCORGANIZATIONID,CMCLOCATIONID,CMCCONTACTID", unique: true),
			new DmoIndex("CMCUNIQUEID", unique: true),
			new DmoIndex("cmcOrganizationID", unique: false),
			new DmoIndex("cmcLocationID", unique: false),
			new DmoIndex("cmcContactID", unique: false),
			new DmoIndex("cmcContactTitleID", unique: false),
			new DmoIndex("cmcName", unique: false),
			new DmoIndex("cmcWebLoginEnabled", unique: false),
			new DmoIndex("cmcWebUserID", unique: false),
			new DmoIndex("cmcNoMailings", unique: false),
			new DmoIndex("cmcInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
