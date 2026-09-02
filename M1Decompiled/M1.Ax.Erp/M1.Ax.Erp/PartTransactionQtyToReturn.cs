using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp;

public class PartTransactionQtyToReturn : PartTransactionDefinition
{
	public PartTransactionQtyToReturn()
	{
		BinQuantityField = "imbQuantityToReturn";
		PartTransactionQuantityField = "imtQuantityToReturn";
		BinDetailQuantityType = QuantityType.ToReturn;
	}

	protected override void UpdateQuantity(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, decimal qtyChange, string jobID, int asmID, short jobMatID, short jobOpID, short compID, bool jobCompleteStatus, DataRow partTransactionRow)
	{
		base.UpdateQuantity(database, transaction, partID, revisionID, warehouseID, binID, qtyChange, jobID, asmID, jobMatID, jobOpID, compID, jobCompleteStatus, partTransactionRow);
		SqlCommand sqlCommand = database.NewSqlCommand("UPDATE PartRevisions SET imrQuantityToReturn = imrQuantityToReturn + @QtyChange WHERE imrPartID = @PartID And imrPartRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@QtyChange", SqlDbType.Decimal)).Value = qtyChange;
		database.ExecuteCommand(sqlCommand, transaction);
	}
}
