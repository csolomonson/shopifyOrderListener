using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.218", "Add fields to ScheduleTrees table", "2017-04-11")]
public class v92218d
{
	public v92218d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTrees", "sxtJobScenarioID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTrees", "sxtJobScenarioID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTrees", new DmoIndex[1]
			{
				new DmoIndex("sxtJobScenarioID", unique: false)
			}, parms.Messages);
		}
	}
}
