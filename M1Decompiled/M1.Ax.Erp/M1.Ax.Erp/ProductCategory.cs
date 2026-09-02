using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class ProductCategory
{
	public void DeleteProductCategoryLine(M1Database database, SqlTransaction transaction, string productCategoryID, int lineID)
	{
		if (string.IsNullOrWhiteSpace(productCategoryID))
		{
			throw new M1Exception("Product Category ID is required.");
		}
		bool flag = false;
		if (transaction == null)
		{
			flag = true;
			transaction = database.BeginTransaction();
		}
		try
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select insProductCategoryLineID,insParentLineID from ProductCategoryLines where insProductCategoryID = @ProductCategoryID");
			sqlCommand.Parameters.Add(new SqlParameter("@ProductCategoryID", SqlDbType.NVarChar)).Value = productCategoryID;
			SqlDataAdapter adapter;
			DataTable dataTable = database.GetDataTable(sqlCommand, fillSchema: true, out adapter, transaction);
			if (dataTable.Rows.Count != 0)
			{
				deleteNextLineLevel(database, transaction, dataTable, productCategoryID, lineID);
				deleteLine(database, transaction, productCategoryID, lineID, deleteLine: false);
			}
			if (flag)
			{
				database.CommitTransaction(transaction);
			}
		}
		catch
		{
			if (flag)
			{
				database.RollbackTransaction(transaction);
			}
			throw;
		}
	}

	private void deleteLine(M1Database database, SqlTransaction transaction, string productCategoryID, int lineID, bool deleteLine)
	{
		if (deleteLine)
		{
			SqlCommand sqlCommand = database.NewSqlCommand("DELETE FROM ProductCategoryLines WHERE insProductCategoryID = @ProductCategoryID AND insProductCategoryLineID = @LineID");
			sqlCommand.Parameters.Add(new SqlParameter("@ProductCategoryID", SqlDbType.NVarChar)).Value = productCategoryID;
			sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = lineID;
			database.ExecuteCommand(sqlCommand, transaction);
		}
	}

	private void deleteNextLineLevel(M1Database database, SqlTransaction transaction, DataTable linesTable, string productCategoryID, int parentLine)
	{
		DataRow[] array = linesTable.Select("insParentLineID = " + M1Util.ConvertToLinq(parentLine) + " and insProductCategoryLineID <> 0");
		foreach (DataRow dataRow in array)
		{
			deleteNextLineLevel(database, transaction, linesTable, productCategoryID, Convert.ToInt32(dataRow["insProductCategoryLineID"]));
			deleteLine(database, transaction, productCategoryID, Convert.ToInt32(dataRow["insProductCategoryLineID"]), deleteLine: true);
		}
	}
}
