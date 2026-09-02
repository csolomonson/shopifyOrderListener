using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.038", "Add Cost Override to Quote/Job Materials", "2013-09-19")]
public class v810038c
{
	public v810038c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteMaterials", "qmmCostOverride"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteMaterials", "qmmCostOverride", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmCostOverride"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmCostOverride", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
