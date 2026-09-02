using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.065", "Add Field Service Credit limit fields", "2008-07-23")]
public class v710065b
{
	public v710065b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapCMFieldServiceCreditMessage"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapCMFieldServiceCreditMessage", "numeric", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ProductionProperties Set xapCMFieldServiceCreditMessage = 1");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapCMFieldServiceHoldMessage"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapCMFieldServiceHoldMessage", "numeric", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ProductionProperties Set xapCMFieldServiceHoldMessage= 1");
		}
	}
}
