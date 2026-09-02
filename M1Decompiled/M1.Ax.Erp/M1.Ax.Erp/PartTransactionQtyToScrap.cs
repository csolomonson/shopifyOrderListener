using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp;

public class PartTransactionQtyToScrap : PartTransactionDefinition
{
	public PartTransactionQtyToScrap()
	{
		BinQuantityField = "";
		PartTransactionQuantityField = "imtScrapQuantity";
		BinDetailQuantityType = QuantityType.None;
	}

	protected override void UpdateQuantity(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, decimal qtyChange, string jobID, int asmID, short jobMatID, short jobOpID, short compID, bool jobCompleteStatus, DataRow partTransactionRow)
	{
	}
}
