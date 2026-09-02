using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp;

public class PartQuantityQtyInTransit : PartQuantityDefinition
{
	protected override void UpdateQuantity(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, decimal qtyChange, DataRow sourceRow, DataRowVersion rowVersion)
	{
		if (string.IsNullOrWhiteSpace(partID))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("UPDATE PartWarehouseLocations SET imlQuantityInTransit = imlQuantityInTransit + @QtyChange From PartWarehouseLocations Inner Join Parts On imlPartID = impPartID WHERE imlPartID = @PartID And imlPartRevisionID = @RevisionID AND imlPartWarehouseID = @WarehouseID And impNonStockedItem = 0");
		sqlCommand.Parameters.Add(new SqlParameter("@QtyChange", SqlDbType.Decimal)).Value = qtyChange;
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		if (database.ExecuteCommand(sqlCommand, transaction) == 0)
		{
			SqlCommand sqlCommand2 = database.NewSqlCommand("Select impNonStockedItem From PartRevisions Inner Join Parts On imrPartID = impPartID Where imrPartID = @Part And imrPartRevisionID = @Revision");
			sqlCommand2.Parameters.Add(new SqlParameter("@Part", SqlDbType.NVarChar)).Value = partID;
			sqlCommand2.Parameters.Add(new SqlParameter("@Revision", SqlDbType.NVarChar)).Value = revisionID;
			DataTable dataTable = database.GetDataTable(sqlCommand2, transaction);
			if (dataTable.Rows.Count != 0 && !dataTable.Rows[0].Field<bool>("impNonStockedItem") && !string.IsNullOrWhiteSpace(warehouseID))
			{
				SqlCommand sqlCommand3 = database.NewSqlCommand("INSERT INTO PartWarehouseLocations (imlPartID,imlPartRevisionID,imlPartWarehouseID,imlQuantityInTransit) SELECT imrPartID,imrPartRevisionID,@WarehouseID,@QtyChange FROM PartRevisions WHERE imrPartID = @PartID And imrPartRevisionID = @RevisionID And imrPartID+imrPartRevisionID NOT IN (SELECT imlPartID+imlPartRevisionID FROM PartWarehouseLocations WHERE imlPartID = @PartID And imlPartRevisionID = @RevisionID And imlPartWarehouseID = @WarehouseID)");
				sqlCommand3.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
				sqlCommand3.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
				sqlCommand3.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
				sqlCommand3.Parameters.Add(new SqlParameter("@QtyChange", SqlDbType.Decimal)).Value = qtyChange;
				database.ExecuteCommand(sqlCommand3, transaction);
			}
		}
	}
}
