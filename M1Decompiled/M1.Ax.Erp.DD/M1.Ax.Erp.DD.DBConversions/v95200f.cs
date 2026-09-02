using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.200", "Add mrjPartPlantID to MRPJobDetails", "2021-12-16")]
public class v95200f
{
	public v95200f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPJobDetails", "mrjPartPlantID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPJobDetails", "mrjPartPlantID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
