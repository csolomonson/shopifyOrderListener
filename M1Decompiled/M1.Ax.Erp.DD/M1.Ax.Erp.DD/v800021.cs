using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.021", "Add wgwContractOwnerEdit to WebGearProperties", "2010-04-19")]
public class v800021
{
	public v800021(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WebGearProperties", "wgwContractOwnerEdit"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WebGearProperties", "wgwContractOwnerEdit", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
