using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp;

public class PartTransactionQtyOnHand : PartTransactionDefinition
{
	public PartTransactionQtyOnHand()
	{
		BinQuantityField = "imbQuantityOnHand";
		PartTransactionQuantityField = "imtInventoryQuantityReceived";
		BinDetailQuantityType = QuantityType.OnHand;
	}

	protected override void UpdateQuantity(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, decimal qtyChange, string jobID, int asmID, short jobMatID, short jobOpID, short compID, bool jobCompleteStatus, DataRow partTransactionRow)
	{
		base.UpdateQuantity(database, transaction, partID, revisionID, warehouseID, binID, qtyChange, jobID, asmID, jobMatID, jobOpID, compID, jobCompleteStatus, partTransactionRow);
		SqlCommand sqlCommand = database.NewSqlCommand("UPDATE PartRevisions SET imrQuantityOnHand = imrQuantityOnHand + @QtyChange WHERE imrPartID = @PartID And imrPartRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@QtyChange", SqlDbType.Decimal)).Value = qtyChange;
		database.ExecuteCommand(sqlCommand, transaction);
		SqlCommand sqlCommand2 = database.NewSqlCommand("UPDATE PartBins SET imbBinQuantityOnHand = imbQuantityOnHand / imbConversionFactor WHERE imbPartID = @PartID And imbPartRevisionID = @RevisionID AND imbWarehouseID = @WarehouseID AND imbPartBinID = @BinID And imbConversionFactor <> 0");
		sqlCommand2.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand2.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand2.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand2.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
		database.ExecuteCommand(sqlCommand2, transaction);
		SqlCommand sqlCommand3 = database.NewSqlCommand("Select imbQuantityOnHand From PartBins Where imbWarehouseID = @WarehouseID AND imbPartBinID = @BinID AND imbPartID = @PartID and imbPartRevisionID = @RevisionID");
		sqlCommand3.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand3.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand3.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand3.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
		decimal num = Convert.ToDecimal(database.ExecuteScalar(sqlCommand3, transaction));
		SqlCommand sqlCommand4 = database.NewSqlCommand("UPDATE WarehouseBins SET inbHasQOHQTI = @UpdatedValue From WarehouseBins WHERE inbWarehouseID = @WarehouseID AND inbWarehouseBinID = @BinID");
		sqlCommand4.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand4.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
		if (num > 0m)
		{
			sqlCommand4.Parameters.Add(new SqlParameter("@UpdatedValue", SqlDbType.Bit)).Value = true;
			database.ExecuteCommand(sqlCommand4, transaction);
			return;
		}
		string queryString = "Select count(*) as PartCount From PartBins Where imbWarehouseID = @WarehouseID AND imbPartBinID = @BinID AND imbPartID <> @PartID and imbPartRevisionID <> @RevisionID AND (imbQuantityOnHand > 0 or imbQuantityToInspect > 0)";
		SqlCommand sqlCommand5 = database.NewSqlCommand(queryString);
		sqlCommand5.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand5.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
		sqlCommand5.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand5.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		if (Convert.ToInt32(database.ExecuteScalar(sqlCommand5, transaction)) == 0)
		{
			sqlCommand4.Parameters.Add(new SqlParameter("@UpdatedValue", SqlDbType.Bit)).Value = false;
			database.ExecuteCommand(sqlCommand4, transaction);
		}
	}
}
