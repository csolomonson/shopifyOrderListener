using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.735", "Add fields to MRPJobDetails table", "2018-07-02")]
public class v92735a
{
	public v92735a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPJobDetails", "mrjJobAssemblyID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPJobDetails", "mrjJobAssemblyID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPJobDetails", "mrjExistingJob"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPJobDetails", "mrjExistingJob", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
