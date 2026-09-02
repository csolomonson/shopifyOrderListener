using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class PurchaseAccounts
{
	public bool RefreshPurchaseOrderLineAccounts(M1BindingSource m1BindingSource, DataRow row)
	{
		M1BindingSource childBindingSource = m1BindingSource.PrimaryTable.GetChildBindingSource("PurchaseOrderAccounts");
		DataRow currentAsDataRow = m1BindingSource.PrimaryTable.GetParentBindingSource(row).CurrentAsDataRow;
		if (row != null && childBindingSource != null && currentAsDataRow != null)
		{
			childBindingSource.RemoveWhere(string.Empty, row, skipValidation: true);
			foreach (ExpenseAccounts item in getAccountsForPurchaseOrderLine(m1BindingSource, currentAsDataRow.Field<string>("pmpSupplierOrganizationID"), row.Field<string>("pmlPartID"), row.Field<string>("pmlPartRevisionID"), currentAsDataRow.Field<string>("pmpPlantID"), row.Field<byte>("pmlPurchaseType"), row.Field<byte>("pmlJobType"), row.Field<string>("pmlAssetTypeID"), row.Field<string>("pmlItemType")))
			{
				DataRow row2 = childBindingSource.AddNew(childBindingSource.Database, row, null, null) as DataRow;
				row2.SetField("pmxExpenseGLAccountID", item.ExpenseAccountID);
				row2.SetField("pmxPercent", item.Percent);
			}
			return true;
		}
		if (row != null && childBindingSource != null && currentAsDataRow == null)
		{
			childBindingSource.RemoveWhere(string.Empty, row, skipValidation: true);
			string value = row.Field<string>("pmlPurchaseOrderID");
			SqlCommand sqlCommand = m1BindingSource.Database.NewSqlCommand("SELECT pmpSupplierOrganizationID, pmpPlantID FROM PurchaseOrders WHERE pmpPurchaseOrderID = @PurchaseID");
			sqlCommand.Parameters.Add(new SqlParameter("@PurchaseID", SqlDbType.NVarChar)).Value = value;
			DataTable dataTable = m1BindingSource.Database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				DataRow row3 = dataTable.Rows[0];
				foreach (ExpenseAccounts item2 in getAccountsForPurchaseOrderLine(m1BindingSource, row3.Field<string>("pmpSupplierOrganizationID"), row.Field<string>("pmlPartID"), row.Field<string>("pmlPartRevisionID"), row3.Field<string>("pmpPlantID"), row.Field<byte>("pmlPurchaseType"), row.Field<byte>("pmlJobType"), row.Field<string>("pmlAssetTypeID"), row.Field<string>("pmlItemType")))
				{
					DataRow row4 = childBindingSource.AddNew(childBindingSource.Database, row, null, null) as DataRow;
					row4.SetField("pmxExpenseGLAccountID", item2.ExpenseAccountID);
					row4.SetField("pmxPercent", item2.Percent);
				}
				return true;
			}
			return false;
		}
		return false;
	}

	private List<ExpenseAccounts> getAccountsForPurchaseOrderLine(M1BindingSource m1BindingSource, string supplierID, string partID, string partRevisionID, string plantID, byte purchaseType, byte jobType, string assetType, string itemType)
	{
		List<ExpenseAccounts> list = new List<ExpenseAccounts>();
		SqlTransaction transaction = m1BindingSource.Transaction;
		M1Database database = m1BindingSource.Database;
		if (!string.IsNullOrWhiteSpace(assetType))
		{
			getAssetAccounts(m1BindingSource, transaction, list, assetType, itemType, purchaseType, plantID);
		}
		else if (!string.IsNullOrWhiteSpace(partID))
		{
			if (purchaseType > 3)
			{
				GetPartAccounts(database, transaction, list, partID, partRevisionID);
				if (list.Count == 0)
				{
					getCOGSAccount(database, transaction, list, partID, plantID, jobType);
				}
			}
			else
			{
				getCOGSAccount(database, transaction, list, partID, plantID, jobType);
				if (list.Count == 0)
				{
					GetPartAccounts(database, transaction, list, partID, partRevisionID);
				}
			}
		}
		if (list.Count == 0 && !string.IsNullOrWhiteSpace(supplierID))
		{
			GetSupplierAccounts(database, transaction, list, supplierID);
		}
		return list;
	}

	private List<ExpenseAccounts> getAssetAccounts(M1BindingSource m1BindingSource, SqlTransaction transaction, List<ExpenseAccounts> accounts, string assetType, string itemType, byte purchaseType, string plantID)
	{
		SqlCommand sqlCommand = m1BindingSource.Database.NewSqlCommand("select IsNull(fayAssetGLAccountID,fatAssetGLAccountID) As fayAssetGLAccountID,IsNull(fayRepairsGLAccountID,fatRepairsGLAccountID) As fayRepairsGLAccountID,IsNull(fayExpenseGLAccountID,fatExpenseGLAccountID) As fayExpenseGLAccountID from AssetTypes Left Outer Join AssetTypePlants On fatAssetTypeID = fayAssetTypeID And fayAssetTypePlantID = @PlantID where fatAssetTypeID = @AssetType");
		sqlCommand.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = plantID;
		sqlCommand.Parameters.Add(new SqlParameter("@AssetType", SqlDbType.NVarChar)).Value = assetType;
		DataTable dataTable = m1BindingSource.Database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				if (purchaseType.Equals(5))
				{
					accounts.Add(new ExpenseAccounts(row.Field<string>("fayRepairsGLAccountID"), 100m));
				}
				else if (!string.IsNullOrWhiteSpace(itemType))
				{
					if (itemType.Trim().ToUpper().Substring(0, 1)
						.Equals("P"))
					{
						accounts.Add(new ExpenseAccounts(row.Field<string>("fayExpenseGLAccountID"), 100m));
					}
					else
					{
						accounts.Add(new ExpenseAccounts(row.Field<string>("fayAssetGLAccountID"), 100m));
					}
				}
				else
				{
					accounts.Add(new ExpenseAccounts(row.Field<string>("fayAssetGLAccountID"), 100m));
				}
			}
		}
		return accounts;
	}

	public List<ExpenseAccounts> GetSupplierAccounts(M1Database database, SqlTransaction transaction, List<ExpenseAccounts> accounts, string supplierID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT xazExpenseGLAccountID, xazPercent FROM ExpenseAccountSplits WHERE xazSupplierOrganizationID = @SupplierID");
		sqlCommand.Parameters.Add(new SqlParameter("@SupplierID", SqlDbType.NVarChar)).Value = supplierID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				accounts.Add(new ExpenseAccounts(row.Field<string>("xazExpenseGLAccountID"), row.Field<decimal>("xazPercent")));
			}
		}
		return accounts;
	}

	public List<ExpenseAccounts> GetPartAccounts(M1Database database, SqlTransaction transaction, List<ExpenseAccounts> accounts, string partID, string partRevisionID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT xazExpenseGLAccountID, xazPercent FROM ExpenseAccountSplits WHERE xazPartID = @PartID And xazPartRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				accounts.Add(new ExpenseAccounts(row.Field<string>("xazExpenseGLAccountID"), row.Field<decimal>("xazPercent")));
			}
		}
		return accounts;
	}

	private List<ExpenseAccounts> getCOGSAccount(M1Database database, SqlTransaction transaction, List<ExpenseAccounts> accounts, string partID, string plantID, byte jobType)
	{
		COGSAccounts cOGSAccounts = new COGS().GetCOGSAccounts(database, transaction, partID, plantID, "", "");
		if (jobType.Equals(1))
		{
			if (!string.IsNullOrWhiteSpace(cOGSAccounts.WIPMaterialGLAccountID))
			{
				accounts.Add(new ExpenseAccounts(cOGSAccounts.WIPMaterialGLAccountID, 100m));
			}
		}
		else if (jobType.Equals(2))
		{
			if (!string.IsNullOrWhiteSpace(cOGSAccounts.WIPSubcontractGLAccountID))
			{
				accounts.Add(new ExpenseAccounts(cOGSAccounts.WIPSubcontractGLAccountID, 100m));
			}
		}
		else if (!string.IsNullOrWhiteSpace(cOGSAccounts.InventoryGLAccountID))
		{
			accounts.Add(new ExpenseAccounts(cOGSAccounts.InventoryGLAccountID, 100m));
		}
		return accounts;
	}

	public void RecalculateExpenseAmounts(M1BindingSource bindingSource)
	{
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		M1BindingSource childBindingSource = bindingSource.PrimaryTable.GetChildBindingSource("PurchaseOrderAccounts");
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		decimal num3 = currentAsDataRow.Field<decimal>("pmlExtendedCostBase");
		if (childBindingSource == null || currentAsDataRow == null)
		{
			return;
		}
		foreach (DataRowView item in childBindingSource)
		{
			num2 = M1Math.Round(item.Row.Field<decimal>("pmxPercent") / 100m * num3, 2);
			num += num2;
			if (num > 0m && num3 > 0m && num > num3)
			{
				num2 = num3 - (num - num2);
				if (num2 <= 0m)
				{
					num2 = default(decimal);
				}
				num = num3;
			}
			if (item.Row.Field<decimal>("pmxAmount") != num2)
			{
				item.Row.SetField("pmxAmount", num2);
			}
		}
		if (num != num3)
		{
			num2 = M1Math.Round(num3 - num, 2);
			childBindingSource.MoveLast();
			if (childBindingSource.CurrentAsDataRow != null)
			{
				childBindingSource.CurrentAsDataRow.SetField("pmxAmount", childBindingSource.CurrentAsDataRow.Field<decimal>("pmxAmount") + num2);
			}
		}
	}

	public List<ExpenseAccounts> getPurchaseOrderLineAccounts(M1Database database, SqlTransaction transaction, string poID, int poLineID)
	{
		List<ExpenseAccounts> list = new List<ExpenseAccounts>();
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT pmxExpenseGLAccountID, pmxPercent FROM PurchaseOrderAccounts WHERE pmxPurchaseOrderID = @PoID And pmxPurchaseOrderLineID = @PoLineID");
		sqlCommand.Parameters.Add(new SqlParameter("@PoID", SqlDbType.NVarChar)).Value = poID;
		sqlCommand.Parameters.Add(new SqlParameter("@PoLineID", SqlDbType.Int)).Value = poLineID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				list.Add(new ExpenseAccounts(row.Field<string>("pmxExpenseGLAccountID"), row.Field<decimal>("pmxPercent")));
			}
		}
		return list;
	}

	public bool AllowPurchaseExpenseAccounts(M1Database database, string purchaseType, string partID)
	{
		if (database.Props("GL").Field<bool>("xafGLCreateStockJournals") && !string.IsNullOrWhiteSpace(purchaseType))
		{
			if (!string.IsNullOrWhiteSpace(partID))
			{
				SqlCommand sqlCommand = new SqlCommand("Select impNonStockedItem from Parts where impPartID = @partID and impPhantomOrKitPart <> 1");
				sqlCommand.Parameters.Add(new SqlParameter("@partID", partID));
				object obj = database.ExecuteScalar(sqlCommand);
				if (obj != null && Convert.ToBoolean(obj))
				{
					if (!(purchaseType == "2") && !(purchaseType == "5"))
					{
						return false;
					}
					return true;
				}
				return purchaseType == "5";
			}
			if (!(purchaseType == "2") && !(purchaseType == "5"))
			{
				return false;
			}
			return true;
		}
		return true;
	}
}
