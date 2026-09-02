using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.015", "Add fields to JobAssemblies table", "2015-01-27")]
public class v900015c
{
	public v900015c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobAssemblies", "jmaAssemblyOverlap"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobAssemblies", "jmaAssemblyOverlap", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobAssemblies", "jmaOverlapDestinationLink"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobAssemblies", "jmaOverlapDestinationLink", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobAssemblies", "jmaOverlap"))
			{
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update JobAssemblies Set jmaOverlapDestinationLink = Case When jmaOverlap = 1 Then 3 When jmaOverlap = 2 Then 4 Else 0 End From JobAssemblies Where jmaOverlap <> 0");
			}
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobAssemblies", "jmaOverlapSourceLink"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobAssemblies", "jmaOverlapSourceLink", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobAssemblies", "jmaOverlap"))
			{
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update JobAssemblies Set jmaOverlapSourceLink = Case When jmaOverlap = 1 Then 3 When jmaOverlap = 2 Then 4 Else 0 End From JobAssemblies Where jmaOverlap <> 0");
			}
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobAssemblies", "jmaOverlapOffsetTime"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobAssemblies", "jmaOverlapOffsetTime", "numeric", 8, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
