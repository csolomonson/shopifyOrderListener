using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.540", "Alter Job Priority field lengths", "2017-10-13")]
public class v92540a
{
	public v92540a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Jobs", "jmpJobPriorityID"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Jobs", "jmpJobPriorityID", "smallint", 3, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoJobPriorityID"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoJobPriorityID", "smallint", 3, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobPriorities", "jmjJobPriorityID"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobPriorities", "jmjJobPriorityID", "smallint", 3, 0, isNullable: false, parms.Messages);
		}
	}
}
