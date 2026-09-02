using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.051", "Update Plant ID from Material Issues in PartTransactions", "2016-05-05")]
public class v91051a
{
	public v91051a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PartTransactions set imtPlantID = injPlantID from MaterialIssueLines inner join PartTransactions on injUniqueID = imtTableUniqueID inner join MaterialIssues on injMaterialIssueID = iniMaterialIssueID where imtPlantID <> injPlantID");
		}
	}
}
