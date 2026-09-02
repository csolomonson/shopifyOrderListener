using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.014", "Add fields to Employees table", "2014-12-18")]
public class v900014y
{
	public v900014y(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Employees", "lmeQAApprovalAmount"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Employees", "lmeQAApprovalAmount", dropTriggers: true);
		}
	}
}
