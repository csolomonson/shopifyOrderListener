using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp;

public class PartTransactionQtyToInspect : PartTransactionDefinition
{
	public PartTransactionQtyToInspect()
	{
		BinQuantityField = "imbQuantityToInspect";
		PartTransactionQuantityField = "imtQuantityToInspect";
		BinDetailQuantityType = QuantityType.ToInspect;
	}

	protected override void UpdateQuantity(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, decimal qtyChange, string jobID, int asmID, short jobMatID, short jobOpID, short compID, bool jobCompleteStatus, DataRow partTransactionRow)
	{
		base.UpdateQuantity(database, transaction, partID, revisionID, warehouseID, binID, qtyChange, jobID, asmID, jobMatID, jobOpID, compID, jobCompleteStatus, partTransactionRow);
		UpdateQtyToInspectInPartRevisions(database, transaction, partID, revisionID, qtyChange);
		SqlCommand sqlCommand = database.NewSqlCommand("Select imbQuantityToInspect From PartBins Where imbWarehouseID = @WarehouseID AND imbPartBinID = @BinID AND imbPartID = @PartID and imbPartRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
		decimal num = Convert.ToDecimal(database.ExecuteScalar(sqlCommand, transaction));
		SqlCommand sqlCommand2 = database.NewSqlCommand("UPDATE WarehouseBins SET inbHasQOHQTI = @UpdatedValue From WarehouseBins WHERE inbWarehouseID = @WarehouseID AND inbWarehouseBinID = @BinID");
		sqlCommand2.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand2.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
		if (num > 0m)
		{
			sqlCommand2.Parameters.Add(new SqlParameter("@UpdatedValue", SqlDbType.Bit)).Value = true;
			database.ExecuteCommand(sqlCommand2, transaction);
			return;
		}
		string queryString = "Select count(*) as PartCount From PartBins Where imbWarehouseID = @WarehouseID AND imbPartBinID = @BinID AND imbPartID <> @PartID and imbPartRevisionID <> @RevisionID AND (imbQuantityOnHand > 0 or imbQuantityToInspect > 0)";
		SqlCommand sqlCommand3 = database.NewSqlCommand(queryString);
		sqlCommand3.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand3.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
		sqlCommand3.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand3.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		if (Convert.ToInt32(database.ExecuteScalar(sqlCommand3, transaction)) == 0)
		{
			sqlCommand2.Parameters.Add(new SqlParameter("@UpdatedValue", SqlDbType.Bit)).Value = false;
			database.ExecuteCommand(sqlCommand2, transaction);
		}
	}

	protected override void UpdateWarehouseAndBin(M1Database database, SqlTransaction transaction, DataRow row, string partID, string revisionID, decimal quantity)
	{
		string text = row.Field<string>(_binField.RelatedFieldsAndCurrentFieldArray[2], DataRowVersion.Original);
		string text2 = row.Field<string>(_binField.FieldName, DataRowVersion.Original);
		string value = row.Field<string>(_binField.RelatedFieldsAndCurrentFieldArray[2], DataRowVersion.Current);
		string value2 = row.Field<string>(_binField.FieldName, DataRowVersion.Current);
		if (!text.Equals(value, StringComparison.CurrentCultureIgnoreCase) || !text2.Equals(value2, StringComparison.CurrentCultureIgnoreCase))
		{
			quantity *= -1m;
			UpdateQtyInPartBins(database, transaction, partID, revisionID, text, text2, quantity);
			UpdateQtyToInspectInPartRevisions(database, transaction, partID, revisionID, quantity);
			UpdatePartBinDetails(database, transaction, partID, revisionID, text, text2, quantity);
		}
	}

	private void UpdateQtyToInspectInPartRevisions(M1Database database, SqlTransaction transaction, string partID, string revisionID, decimal qtyChange)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("UPDATE PartRevisions SET imrQuantityToInspect = imrQuantityToInspect + @QtyChange WHERE imrPartID = @PartID And imrPartRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@QtyChange", SqlDbType.Decimal)).Value = qtyChange;
		database.ExecuteCommand(sqlCommand, transaction);
	}

	private void UpdatePartBinDetails(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, decimal qtyChange)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT TOP 1 imgRemainingQuantity, imgOriginalQuantity, imgUniqueID FROM PartBinDetails WHERE imgPartID = @PartID and imgPartRevisionID = @RevisionID AND imgWarehouseID = @WarehouseID AND imgPartBinID = @PartBin AND imgQuantityType = @QuantityType ORDER BY imgPartBinDetailID DESC");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartBin", SqlDbType.NVarChar)).Value = binID;
		sqlCommand.Parameters.Add(new SqlParameter("@QuantityType", SqlDbType.Decimal)).Value = Convert.ToInt32(QuantityType.ToInspect);
		SqlDataAdapter adapter;
		DataTable dataTable = database.GetDataTable(sqlCommand, fillSchema: false, out adapter, transaction);
		if (dataTable.Rows.Count == 1)
		{
			Guid guid = dataTable.Rows[0].Field<Guid>("imgUniqueID");
			SqlCommand sqlCommand2 = database.NewSqlCommand("UPDATE PartBinDetails SET imgRemainingQuantity = imgRemainingQuantity + @QtyChange WHERE imgUniqueID = @UniqueID");
			sqlCommand2.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = guid;
			sqlCommand2.Parameters.Add(new SqlParameter("@QtyChange", SqlDbType.Decimal)).Value = qtyChange;
			database.ExecuteCommand(sqlCommand2, transaction);
		}
	}
}
