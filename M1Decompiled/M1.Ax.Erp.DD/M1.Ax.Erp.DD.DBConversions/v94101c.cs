using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.4.101", "Add pull from stock flag on job material components", "2021-08-04")]
public class v94101c
{
	public v94101c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterialComponents", "jmtPullAllFromStock"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterialComponents", "jmtPullAllFromStock", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE JobMaterialComponents SET jmtPullAllFromStock = jmmPullAllFromStock FROM JobMaterialComponents INNER JOIN JobMaterials ON jmtJobID = jmmJobID AND jmtJobAssemblyID = jmmJobAssemblyID AND jmtJobMaterialID = jmmJobMaterialID;");
		}
	}
}
