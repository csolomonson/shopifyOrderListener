using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.009", "Add fields to Inspections table", "2016-11-04")]
public class v92009b
{
	public v92009b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Inspections", "qapReversalEntry"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Inspections", "qapReversalEntry", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Inspections", "qapPostedDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Inspections", "qapPostedDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Inspections", "qapPosted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Inspections", "qapPosted", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
