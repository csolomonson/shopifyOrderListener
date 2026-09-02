using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.752", "Add fields to PartMaterials table", "2018-07-26")]
public class v92752a
{
	public v92752a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartMaterials", "immManualPart"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartMaterials", "immManualPart", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
