using M1.Core;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("SalesOrders")]
public class SalesOrdersChangeID : IChangeIDProcessing
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
		string orderId = parm.NewKeyValues[0].ToString();
		short changeIDType = parm.ChangeIDType;
		if (changeIDType == 2 || changeIDType == 3)
		{
			new SalesOrder().RefreshOrderTotal(database, orderId, forceApprovalCheck: false);
		}
	}
}
