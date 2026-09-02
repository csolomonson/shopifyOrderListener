using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp;

public class PartQuantityQtyAllocated : PartQuantityDefinition
{
	public PartQuantityQtyAllocated()
	{
		BinQuantityField = "imbQuantityAllocated";
	}

	protected override void UpdateQuantity(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, decimal qtyChange, DataRow sourceRow, DataRowVersion rowVersion)
	{
		base.UpdateQuantity(database, transaction, partID, revisionID, warehouseID, binID, qtyChange, sourceRow, rowVersion);
		SqlCommand sqlCommand = database.NewSqlCommand("UPDATE PartRevisions SET imrQuantityAllocated = imrQuantityAllocated + @QtyChange WHERE imrPartID = @PartID And imrPartRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@QtyChange", SqlDbType.Decimal)).Value = qtyChange;
		database.ExecuteCommand(sqlCommand, transaction);
	}
}
