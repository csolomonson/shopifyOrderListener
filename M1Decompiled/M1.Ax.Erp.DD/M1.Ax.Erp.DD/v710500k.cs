using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.500", "Add Quality Register field", "2009-05-28")]
public class v710500k
{
	public v710500k(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QualityRegisters", "qanRMAClaimCreated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QualityRegisters", "qanRMAClaimCreated", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
