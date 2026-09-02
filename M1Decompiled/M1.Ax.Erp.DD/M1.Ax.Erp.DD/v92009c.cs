using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.009", "Add fields to InspectionComponents table", "2016-11-04")]
public class v92009c
{
	public v92009c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionComponents", "qamPosted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionComponents", "qamPosted", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
