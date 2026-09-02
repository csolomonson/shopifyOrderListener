using System.Data;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.108", "Convert claim problems to nonconformances", "2015-11-25")]
public class v900108e
{
	public v900108e(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "RmaClaimProblems"))
		{
			NonConformance nonConformance = new NonConformance();
			DataTable customFieldsbyTable = nonConformance.GetCustomFieldsbyTable(parms.Database);
			nonConformance.ConvertRMAClaimProblemsToNonConformances(customFields: v900108d.CreateCustomFields(parms, customFieldsbyTable, "uqar", "NonConformances"), database: parms.Database, transaction: null);
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "RmaClaimProblems");
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Jobs", "jmpRMAClaimProblemID"))
			{
				parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Jobs", "jmpRMAClaimProblemID", dropTriggers: true);
			}
		}
	}
}
