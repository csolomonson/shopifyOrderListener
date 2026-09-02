using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.00.010", "Add rarSubcontractAmtForeign to RMAClaimProblems", "2008-02-20")]
public class v700010a
{
	public v700010a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimProblems", "rarSubcontractAmtForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimProblems", "rarSubcontractAmtForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update RMAClaimProblems Set rarSubcontractAmtForeign = rarSubcontractAmount");
		}
	}
}
