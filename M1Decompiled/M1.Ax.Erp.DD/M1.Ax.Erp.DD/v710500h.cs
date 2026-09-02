using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.500", "Add Credit Limit Source related Field to Prod properties", "2009-04-01")]
public class v710500h
{
	public v710500h(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapCMCreditLimitSourceOrder"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapCMCreditLimitSourceOrder", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapCMCreditLimitSourceInv"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapCMCreditLimitSourceInv", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapCMCreditLimitSourceShip"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapCMCreditLimitSourceShip", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapCMCreditLimitSource"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapCMCreditLimitSourceOrder = (Case xapCMCreditLimitSource When 1 Then -1 When 2 Then -1 Else 0 End), xapCMCreditLimitSourceShip = (Case xapCMCreditLimitSource When 2 Then -1 When 3 Then -1 Else 0 End), xapCMCreditLimitSourceInv = (Case xapCMCreditLimitSource When 2 Then -1 When 3 Then -1 Else 0 End)");
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapCMCreditLimitSource", dropTriggers: true);
		}
	}
}
