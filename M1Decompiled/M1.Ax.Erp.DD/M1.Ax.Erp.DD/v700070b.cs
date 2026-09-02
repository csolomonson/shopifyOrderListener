using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.00.070", "Add Unique ID to AR Payment Epays table", "2008-03-15")]
public class v700070b
{
	public v700070b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARPaymentEPays", "areUniqueID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentEPays", "areUniqueID", "uniqueidentifier", 16, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARPaymentEPays Set areUniqueID = NewID()");
		}
	}
}
