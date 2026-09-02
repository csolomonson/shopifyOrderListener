using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.205", "Add Misc Issue Reason to Part Transactions table", "2011-12-06")]
public class v800205
{
	public v800205(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartTransactions", "imtMiscIssueReasonID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartTransactions", "imtMiscIssueReasonID", "char", 5, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
