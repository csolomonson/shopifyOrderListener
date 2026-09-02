using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.568", "Add fields to EMPLOYEES table", "2015-05-19")]
public class v800568aq
{
	public v800568aq(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EMPLOYEES", "lmeCountyCodeID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EMPLOYEES", "lmeCountyCodeID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
