using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.036", "Add EFT Description fields to relevant tables", "2010-06-04")]
public class v800036c
{
	public v800036c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APPaymentHeaders", "aptEFTCode"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APPaymentHeaders", "aptEFTCode", "char", 12, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APPaymentHeaders", "aptEFTParticulars"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APPaymentHeaders", "aptEFTParticulars", "char", 12, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoEFTCode"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoEFTCode", "char", 12, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoEFTParticulars"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoEFTParticulars", "char", 12, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationLocations", "cmlEFTCode"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationLocations", "cmlEFTCode", "char", 12, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationLocations", "cmlEFTParticulars"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationLocations", "cmlEFTParticulars", "char", 12, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update APPaymentHeaders Set aptEFTDescription = Left(aptEFTDescription, 12) Where aptEFTDescription <> '' And (SELECT XADREGION FROM DATASETPROPERTIES) = 'NZ'");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Organizations Set cmoEFTDescription = Left(cmoEFTDescription, 12) Where cmoEFTDescription <> '' And (SELECT XADREGION FROM DATASETPROPERTIES) = 'NZ'");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update OrganizationLocations Set cmlEFTDescription = Left(cmlEFTDescription, 12) Where cmlEFTDescription <> '' And (SELECT XADREGION FROM DATASETPROPERTIES) = 'NZ'");
	}
}
