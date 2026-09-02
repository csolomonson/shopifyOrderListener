using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;

namespace M1.Ax.Erp;

[ComVisible(true)]
public class Plant
{
	public class WarehousePlant
	{
		public string PlantID = string.Empty;

		public string PlantDepartmentID = string.Empty;
	}

	public string GetWhereUsedList(DataRow row, M1BindingSource bindingSource)
	{
		return bindingSource.PrimaryTable.ForeignKeyCheck(row, bindingSource.DataDictionary, bindingSource.Database, bindingSource.Fields, checkDeleteFilter: false);
	}

	public WarehousePlant GetWarehousePlant(M1Database database, SqlTransaction transaction, string warehouseID)
	{
		WarehousePlant warehousePlant = new WarehousePlant();
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT imwPlantID, imwPlantDepartmentID FROM Warehouses WHERE imwWarehouseID = @WarehouseID");
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			warehousePlant.PlantID = dataTable.Rows[0].Field<string>("imwPlantID");
			warehousePlant.PlantDepartmentID = dataTable.Rows[0].Field<string>("imwPlantDepartmentID");
		}
		return warehousePlant;
	}
}
