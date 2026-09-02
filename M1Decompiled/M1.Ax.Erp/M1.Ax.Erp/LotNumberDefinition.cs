using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class LotNumberDefinition : SerialOrLotNumberBase
{
	public bool FilterUnallocated;

	public LotNumberDefinition()
	{
		curType = 'L';
		TrPrefix = "abt";
		TPrefix = "Lot";
		HPrefix = "abl";
		StatusPrefix = "abs";
	}

	public M1BindingSource GetAvailableAsBindingSource(M1Database database, DataRow row)
	{
		GetAvailableAsTable(database, row);
		M1BindingSource m1BindingSource = new M1BindingSource(database, isManuallyAdded: true);
		m1BindingSource.LoadDefinition(string.Empty, "LOTNUMBERS", availableTable, false, loadDataNow: false);
		return m1BindingSource;
	}

	public override void GetAvailableAsTable(M1Database database, DataRow row)
	{
		string empty = string.Empty;
		string arg = string.Empty;
		string[] relatedFieldsAndCurrentFieldArray = base.Field.BindingSource.Fields[PartBinField].RelatedFieldsAndCurrentFieldArray;
		_ = string.Empty;
		string text = string.Empty;
		if (Parameters != null)
		{
			text = Parameters.ToString().Trim().ToUpper();
		}
		if (row == null || row.Field<string>(relatedFieldsAndCurrentFieldArray[0]).Trim().Length == 0)
		{
			empty = base.BuildFilter(null, null, includeWhBin: true);
			availableLastQuantity = default(decimal);
		}
		else
		{
			empty = ((!base.Field.TableName.ToUpper().Equals("WAREHOUSERECEIPTLINES", StringComparison.CurrentCultureIgnoreCase) && !base.Field.TableName.ToUpper().Equals("WAREHOUSERECEIPTCOMPONENTS", StringComparison.CurrentCultureIgnoreCase)) ? base.BuildFilter(relatedFieldsAndCurrentFieldArray, row, includeWhBin: true) : base.BuildFilter(relatedFieldsAndCurrentFieldArray, row, includeWhBin: false));
			availableLastQuantity = row.Field<decimal>(base.Field.FieldName);
			string text2 = string.Empty;
			bool flag = false;
			empty = ((empty.Length == 0) ? "absQuantity <> 0 " : (empty + " AND absQuantity <> 0 "));
			if (IsNegativeTransaction(row.Field<decimal>(base.Field.FieldName)))
			{
				if (AvailableFilterNegativeExpression.Length != 0)
				{
					text2 = base.Field.Table.EvaluateScriptExpression(AvailableFilterNegativeExpression, database, row).ToString();
				}
				flag = true;
			}
			else
			{
				if (AvailableFilterPositiveExpression.Length != 0)
				{
					text2 = base.Field.Table.EvaluateScriptExpression(AvailableFilterPositiveExpression, database, row).ToString();
				}
				flag = false;
			}
			if (text2.Length != 0)
			{
				empty += $" And {text2} ";
			}
			if ((text.Contains("INCLUDEUNASSIGNED") && !FilterUnallocated) || (text.Contains("INCLUDEPOSITIVEUNASSIGNED") && !flag && !FilterUnallocated) || (base.Field.BindingSource.PrimaryTable.TableName.Equals("QUANTITYADJUSTMENTS", StringComparison.CurrentCultureIgnoreCase) && !FilterUnallocated))
			{
				arg = "Union All  Select Distinct ablLotNumberID, ablPartID, ablPartRevisionID, ablExpirationDate, ablInactive, ablLotNumberID as absLotNumberID, ablPartID as absPartID, ablPartRevisionID as absPartRevisionID, '' as absPartWarehouseLocationID, '' as absPartBinID, CAST(0 As TinyInt) as absStatus, CAST(0 As Decimal) as absQuantity From LotNumbers Left Join LotNumberStatuses on ablPartID=absPartID And ablPartRevisionID=absPartRevisionID And ablLotNumberID=absLotNumberID And absQuantity <> 0 Where ( ( " + HPrefix + "PartID = " + row.Field<string>(relatedFieldsAndCurrentFieldArray[0]).Trim().ToSql() + " And " + HPrefix + "PartRevisionID = " + row.Field<string>(relatedFieldsAndCurrentFieldArray[1]).Trim().ToSql() + " And ablInactive = 0  ) )  And (ablPartID+ablPartRevisionID+ablLotNumberID NOT IN (Select abtPartID+abtPartRevisionID+abtLotNumberID From LotNumberTransactions Where ((abtTransactionType = " + TransactionType + " And abtTableUniqueID = " + row.Field<Guid>(base.Field.Table.FieldPrefix + "UniqueID").ToSql() + ") OR (abtTransactionType = '1')) And abtPartID = ablPartID and abtPartRevisionID = ablPartRevisionID and abtLotNumberID = ablLotNumberID )) And (ablPartID+ablPartRevisionID+ablLotNumberID NOT IN (Select absPartID+absPartRevisionID+absLotNumberID From LotNumberStatuses Where  absPartID = ablPartID And absPartRevisionID = ablPartRevisionID " + ((text2.Length == 0) ? string.Empty : (" AND " + text2)) + "))";
			}
		}
		string text3 = "ablLotNumberID, ablPartID, ablPartRevisionID, ablExpirationDate, ablInactive, absLotNumberID, absPartID, absPartRevisionID, absPartWarehouseLocationID, absPartBinID, absStatus, absQuantity";
		if (availableTable == null)
		{
			availableTable = database.GetDataTable(string.Format("Select " + text3 + " From LotNumbers Left Join LotNumberStatuses on ablPartID=absPartID And ablPartRevisionID=absPartRevisionID And ablLotNumberID=absLotNumberID Where {0} {1} And ablInactive = 0 Order By ablLotNumberID", empty, arg));
		}
		else
		{
			availableTable.Rows.Clear();
			database.Fill(availableTable, string.Format("Select " + text3 + " From LotNumbers Left Join LotNumberStatuses on ablPartID=absPartID And ablPartRevisionID=absPartRevisionID And ablLotNumberID=absLotNumberID Where {0} {1} And ablInactive = 0 Order By ablLotNumberID", empty, arg));
		}
		List<DataTable> deletedItems = new List<DataTable>();
		List<DataTable> insertedItems = new List<DataTable>();
		GetAllChangedRows(deletedItems, insertedItems, typeof(LotNumberDefinition));
		if (row != null)
		{
			MergeAvailable(relatedFieldsAndCurrentFieldArray, row, deletedItems, insertedItems);
		}
		RefreshQuantityAvailable();
	}

	public void Remove(M1Database database, DataRow currentRow, DataRow lotNumberRowToRemove, SqlTransaction transaction)
	{
		string value = lotNumberRowToRemove.Field<string>("abtLotNumberID").Trim();
		Guid g = currentRow.Field<Guid>(base.Field.Table.UniqueField);
		DataTable dataTable = childBindingSource.GetDataTable();
		TransactionChangedEventArgs e = new TransactionChangedEventArgs
		{
			Database = database,
			CurrentRow = currentRow
		};
		foreach (DataRow row in dataTable.Rows)
		{
			if (row.RowState != DataRowState.Deleted && row.Field<Guid>("abtTableUniqueID").Equals(g) && row.Field<string>("abtLotNumberID").Trim().Equals(value, StringComparison.CurrentCultureIgnoreCase))
			{
				if (IsLatestTransaction(database, row))
				{
					e.TransactionRowChanged = row;
					OnRemoved(e);
					row.Delete();
					base.Field.Validate(database, currentRow, transaction, base.Field.BindingSource.CurrentAsDataRow == currentRow);
					break;
				}
				throw new M1Exception(RemoveFailMessage(lotNumberRowToRemove.Field<string>("abtLotNumberID").Trim()));
			}
		}
	}

	public void ValidateBeforeActivateInactive(bool inactive, ref string msg, ref string msgH)
	{
		if (inactive)
		{
			msg = "Do you want to make this lot number active for the selected partbin?";
			msgH = "Lot Number Activate Confirmation";
		}
		else
		{
			msg = "Do you want to make this lot number inactive for the selected partbin?";
			msgH = "Lot Number Inactivate Confirmation";
		}
	}

	public bool IsLotInActive(M1Database database, SqlTransaction transaction, string partID, string revisionID, string lotNumberID)
	{
		return new LotNumber().IsLotInactive(database, transaction, partID, revisionID, lotNumberID);
	}

	public bool IsLotUnassigned(M1Database database, SqlTransaction transaction, string partID, string revisionID, string lotNumberID)
	{
		return new LotNumber().IsLotUnassigned(database, transaction, partID, revisionID, lotNumberID);
	}

	public int CanInactivate(M1Database database, SqlTransaction transaction, string partID, string revisionID, string lotNumberID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT IsNull(Count(absLotNumberID),0) FROM LotNumberStatuses WHERE absPartID = @PartID AND absPartRevisionID = @PartRevisionID AND absLotNumberID = @LotNumberID AND absStatus NOT IN (3,4,6,8) AND absQuantity <> 0");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@LotNumberID", SqlDbType.NVarChar)).Value = lotNumberID;
		return Convert.ToInt16(database.ExecuteScalar(sqlCommand, transaction));
	}

	public string GetLotNumbersUsedInJobWhere(string lotNumberID)
	{
		return $"ablLotNumberID In (Select abtLotNumberID From LotNumberTransactions Where abtTransactionType IN (4,20) And abtJobID In (Select abtJobID From LotNumberTransactions Where abtTransactionType In (3,40) And abtLotNumberID = {lotNumberID.ToSql()}))";
	}

	public virtual string GetLotNumbersCreatedByJobWhere(string lotNumberID)
	{
		return $"ablLotNumberID In (Select abtLotNumberID From LotNumberTransactions Where abtTransactionType In (3,40) And abtJobID In (Select abtJobID From LotNumberTransactions Where abtTransactionType IN (4,20) And abtLotNumberID = {lotNumberID.ToSql()}))";
	}

	public DataRow AddRow(M1Database database, DataRow currentRow, DataRow lotNumberRowToAdd, SqlTransaction transaction)
	{
		if (childBindingSource != null && !IsSelected(currentRow, lotNumberRowToAdd.Field<string>("ablLotNumberID")))
		{
			DataRow dataRow = base.AddTransactionRow(database, currentRow, lotNumberRowToAdd);
			if (IsLatestTransaction(database, dataRow))
			{
				childBindingSource.AddNew(database, null, null, dataRow);
				TransactionChangedEventArgs e = new TransactionChangedEventArgs();
				e.Database = database;
				e.CurrentRow = currentRow;
				e.TransactionRowChanged = dataRow;
				OnAdded(e);
				base.Field.Validate(database, currentRow, transaction, base.Field.BindingSource.CurrentAsDataRow == currentRow);
				return dataRow;
			}
			throw new M1Exception(AddRowIsNotLatestTransactionMessage(dataRow));
		}
		return null;
	}

	public override void LoadComplete(FieldCollection fields, bool allowEditing)
	{
		if (PartBinField.Length != 0 && TransactionType != 0 && base.Field.DataDictionary != null && base.Field.DataDictionary.ProductCode.IsModulePurchased("LN", base.Field.Database) && allowEditing)
		{
			if (childBindingSource == null)
			{
				childBindingSource = new M1BindingSource(base.Field.BindingSource.Database, base.Field.BindingSource.Transaction);
				childBindingSource.LoadDefinition(new QueryDefinition(base.Field.Database, string.Empty, "LOTNUMBERTRANSACTIONS")
				{
					FieldList = "*,CAST(abtStatus As TinyInt) As OriginalStatus,CAST(abtQuantity As Decimal) As OriginalQuantity,CAST((Select ISNULL(ablExpirationDate, NULL) from LotNumbers Where ablLotNumberID = abtLotNumberID And ablPartID = abtPartID and ablPartRevisionID = abtPartRevisionID) As Date) As ablExpirationDate"
				}, false, loadDataNow: false);
				base.LoadComplete(fields, true);
			}
		}
		else if (childBindingSource != null)
		{
			base.LoadComplete(fields, false);
			childBindingSource.Dispose();
			childBindingSource = null;
		}
	}

	public void ActivateNumber(M1Database database, DataRow lotNumber, bool activate)
	{
		SqlTransaction sqlTransaction = database.BeginTransaction();
		try
		{
			byte transType = (byte)((!activate) ? 9 : 10);
			SetInactiveStatus(database, sqlTransaction, lotNumber.Field<string>("ablPartID"), lotNumber.Field<string>("ablPartRevisionID"), lotNumber.Field<string>("ablLotNumberID"), !activate);
			AddLotTransaction(database, sqlTransaction, lotNumber.Field<string>("ablPartID"), lotNumber.Field<string>("ablPartRevisionID"), string.Empty, string.Empty, lotNumber.Field<string>("ablLotNumberID"), 0m, 0, transType, string.Empty, Guid.Empty, string.Empty, 0, 0, 0, negativeTrans: false);
			database.CommitTransaction(sqlTransaction);
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			throw;
		}
	}

	public void AddLotTransaction(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, string lotNumberID, decimal quantity, byte status, byte transType, string sourceTable, Guid sourceGUID, string jobID, int asmID, int jobMatID, int jobMatCompID, bool negativeTrans)
	{
		AddLotTransaction(database, transaction, partID, revisionID, warehouseID, binID, lotNumberID, quantity, status, transType, sourceTable, sourceGUID, jobID, asmID, jobMatID, jobMatCompID, negativeTrans, DateTime.Now);
	}

	public void AddLotTransaction(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, string lotNumberID, decimal quantity, byte status, byte transType, string sourceTable, Guid sourceGUID, string jobID, int asmID, int jobMatID, int jobMatCompID, bool negativeTrans, DateTime transactionDate)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("INSERT INTO LotNumberTransactions(");
		stringBuilder.Append("abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionType, abtQuantity, ");
		stringBuilder.Append("abtTransactionDate, abtCreatedBy, abtCreatedDate, abtStatus, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtTableName, abtTableUniqueID, abtNegativeTransaction )");
		stringBuilder.Append("VALUES(@LotNumberID, @PartID, @PartRevisionID, @WarehouseID, @BinID, @TransactionType, @Quantity, ");
		stringBuilder.Append("@TransactionDate, @CreatedBy, @CreatedDate, @Status, @JobID, @AsmID, @SeqID, @CompID, @SourceTable, @SourceGUID, @NegativeTransaction)");
		SqlCommand sqlCommand = database.NewSqlCommand(stringBuilder.ToString());
		sqlCommand.Parameters.Add(new SqlParameter("@LotNumberID", SqlDbType.NVarChar)).Value = lotNumberID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
		sqlCommand.Parameters.Add(new SqlParameter("@TransactionType", SqlDbType.SmallInt)).Value = transType;
		sqlCommand.Parameters.Add(new SqlParameter("@Quantity", SqlDbType.Decimal)).Value = quantity;
		sqlCommand.Parameters.Add(new SqlParameter("@TransactionDate", SqlDbType.DateTime)).Value = transactionDate;
		sqlCommand.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.NVarChar)).Value = database.User.ID;
		sqlCommand.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime)).Value = DateTime.Now;
		sqlCommand.Parameters.Add(new SqlParameter("@Status", SqlDbType.SmallInt)).Value = status;
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.VarChar)).Value = jobID;
		sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
		sqlCommand.Parameters.Add(new SqlParameter("@SeqID", SqlDbType.SmallInt)).Value = jobMatID;
		sqlCommand.Parameters.Add(new SqlParameter("@CompID", SqlDbType.SmallInt)).Value = jobMatCompID;
		sqlCommand.Parameters.Add(new SqlParameter("@SourceTable", SqlDbType.VarChar)).Value = sourceTable;
		sqlCommand.Parameters.Add(new SqlParameter("@SourceGUID", SqlDbType.UniqueIdentifier)).Value = sourceGUID;
		sqlCommand.Parameters.Add(new SqlParameter("@NegativeTransaction", SqlDbType.Decimal)).Value = negativeTrans;
		database.ExecuteCommand(sqlCommand, transaction);
	}
}
