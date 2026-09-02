using M1.Core;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("SalesOrderLines")]
public class SalesOrderLinesChangeID : IChangeIDProcessing
{
	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
		M1Database database = parm.Database;
		string text = parm.OldKeyValues[0].ToString();
		string text2 = parm.NewKeyValues[0].ToString();
		SalesOrder salesOrder = new SalesOrder();
		if (text == text2)
		{
			salesOrder.RefreshOrderTotal(database, text, forceApprovalCheck: false);
			return;
		}
		salesOrder.RefreshOrderTotal(database, text, forceApprovalCheck: false);
		salesOrder.RefreshOrderTotal(database, text2, forceApprovalCheck: false);
	}
}
