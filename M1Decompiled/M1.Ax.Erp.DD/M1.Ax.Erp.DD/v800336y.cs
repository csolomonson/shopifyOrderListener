using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.336", "Add fields to ORGANIZATIONCONTACTS table", "2015-05-19")]
public class v800336y
{
	public v800336y(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEasyOrderEnabled"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEasyOrderEnabled", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcCreatedByEasyOrder"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcCreatedByEasyOrder", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOInitials"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOInitials", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOPrefix"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOPrefix", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOSurname"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOSurname", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOPassword"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOPassword", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOUserRole"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOUserRole", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEODefSupervisor"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEODefSupervisor", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOSubSupervisor"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOSubSupervisor", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOCustomerGroup"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOCustomerGroup", "nvarchar", 100, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOMultiShipAddress"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOMultiShipAddress", "nvarchar", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update OrganizationContacts Set cmcEOMultiShipAddress = '0'");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOReceiveOrderConfirmation"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOReceiveOrderConfirmation", "nvarchar", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update OrganizationContacts Set cmcEOReceiveOrderConfirmation = '0'");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOEditShippingAddress"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOEditShippingAddress", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOReceiveEMails"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOReceiveEMails", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOHTMLMail"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOHTMLMail", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOReminderOfOpenOrders"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOReminderOfOpenOrders", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOOrderAuthorisationMessage"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOOrderAuthorisationMessage", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOAuthorisationRequest"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOAuthorisationRequest", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOMayNotCreOrdTemp"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOMayNotCreOrdTemp", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOFirstName"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONCONTACTS", "cmcEOFirstName", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
