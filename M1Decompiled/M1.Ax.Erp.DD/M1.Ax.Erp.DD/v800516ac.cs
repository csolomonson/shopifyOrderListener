using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.516", "Add fields to ORGANIZATIONLOCATIONS table", "2015-05-19")]
public class v800516ac
{
	public v800516ac(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlBareCostOfDuty"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlBareCostOfDuty", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlBareTransportationCost"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlBareTransportationCost", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlIgnoreAvalara"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlIgnoreAvalara", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlUPSValidated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlUPSValidated", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlEDIIntegrated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlEDIIntegrated", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlFedExAccountNumber"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlFedExAccountNumber", "nvarchar", 15, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlFedEx3rdPartyOrganizationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlFedEx3rdPartyOrganizationID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlFedEx3rdPartyLocationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlFedEx3rdPartyLocationID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlFedExBillingOption"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONLOCATIONS", "cmlFedExBillingOption", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
