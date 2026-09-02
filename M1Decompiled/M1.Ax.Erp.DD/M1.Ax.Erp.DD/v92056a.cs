using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.056", "Add Total component cost field to ReceiptLines table", "2016-12-19")]
public class v92056a
{
	public v92056a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlTotalComponentCosts"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlTotalComponentCosts", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ReceiptLines Set rmlTotalComponentCosts = DetailAmount From ReceiptLines Inner Join (Select rmoReceiptID,rmoReceiptLineID,Sum(rmoExtendedCostBase) As DetailAmount From ReceiptComponents Group By rmoReceiptID,rmoReceiptLineID) As DetailTable On RMLRECEIPTID = rmoReceiptID And RMLRECEIPTLINEID = rmoReceiptLineID");
		}
	}
}
