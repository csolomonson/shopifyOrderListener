using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class SerialNumberDefinition : SerialOrLotNumberBase
{
	public SerialNumberDefinition()
	{
		curType = 'S';
		TrPrefix = "snt";
		TPrefix = "Serial";
		HPrefix = "ims";
		StatusPrefix = "sns";
	}

	public M1BindingSource GetAvailableAsBindingSource(M1Database database, DataRow row)
	{
		GetAvailableAsTable(database, row);
		M1BindingSource m1BindingSource = new M1BindingSource(database, isManuallyAdded: true);
		m1BindingSource.LoadDefinition(string.Empty, "SERIALNUMBERS", availableTable, false, loadDataNow: false);
		return m1BindingSource;
	}

	public override void GetAvailableAsTable(M1Database database, DataRow row)
	{
		string arg = string.Empty;
		string[] relatedFieldsAndCurrentFieldArray = base.Field.BindingSource.Fields[PartBinField].RelatedFieldsAndCurrentFieldArray;
		string text = string.Empty;
		if (Parameters != null)
		{
			text = Parameters.Trim().ToUpper();
		}
		string text2;
		if (row == null || row.Field<string>(relatedFieldsAndCurrentFieldArray[0]).Trim().Length == 0)
		{
			text2 = base.BuildFilter(null, null, includeWhBin: true);
			availableLastQuantity = default(decimal);
		}
		else
		{
			text2 = ((!base.Field.TableName.ToUpper().Equals("WarehouseReceiptLines", StringComparison.CurrentCultureIgnoreCase) && !base.Field.TableName.ToUpper().Equals("WarehouseReceiptComponents", StringComparison.CurrentCultureIgnoreCase)) ? base.BuildFilter(relatedFieldsAndCurrentFieldArray, row, includeWhBin: true) : base.BuildFilter(relatedFieldsAndCurrentFieldArray, row, includeWhBin: false));
			availableLastQuantity = row.Field<decimal>(base.Field.FieldName);
			bool flag = false;
			string text3 = string.Empty;
			text2 = ((text2.Length != 0) ? (text2 + " AND snsQuantity <> 0 ") : "snsQuantity <> 0 ");
			if (IsNegativeTransaction(row.Field<decimal>(base.Field.FieldName)))
			{
				if (AvailableFilterNegativeExpression.Length != 0)
				{
					text3 = base.Field.Table.EvaluateScriptExpression(AvailableFilterNegativeExpression, database, row).ToString();
				}
				flag = true;
			}
			else
			{
				if (AvailableFilterPositiveExpression.Length != 0)
				{
					text3 = base.Field.Table.EvaluateScriptExpression(AvailableFilterPositiveExpression, database, row).ToString();
				}
				flag = false;
			}
			if (text3.Length != 0)
			{
				text2 = string.Concat(text2, " And " + text3 + " ");
			}
			if (text.Contains("INCLUDEUNASSIGNED") || (text.ToUpper().Contains("INCLUDEPOSITIVEUNASSIGNED") && !flag))
			{
				arg = "UNION ALL SELECT imsSerialNumberID, imsPartID, imsPartRevisionID, imsExpirationDate, imsInactive, imsSerialNumberID AS snsSerialNumberID, imsPartID AS snsPartID, imsPartRevisionID AS snsPartRevisionID, '' AS snsPartWarehouseLocationID, '' AS snsPartBinID, CAST(0 AS TinyInt) AS snsStatus, CAST(0 AS Decimal) AS snsQuantity FROM SerialNumbers LEFT JOIN SerialNumberStatuses ON imsPartID = snsPartID AND imsPartRevisionID = snsPartRevisionID AND imsSerialNumberID = snsSerialNumberID AND snsQuantity <> 0 WHERE " + HPrefix + "PartID = " + row.Field<string>(relatedFieldsAndCurrentFieldArray[0]).Trim().ToSql() + " AND " + HPrefix + "PartRevisionID = " + row.Field<string>(relatedFieldsAndCurrentFieldArray[1]).Trim().ToSql() + " AND imsInactive = 0 AND snsSerialNumberID IS NULL AND NOT EXISTS (SELECT snsSerialNumberID FROM SerialNumberStatuses sns_inner WHERE sns_inner.snsQuantity <> 0 AND sns_inner.snsPartID = imsPartID AND sns_inner.snsPartRevisionID = imsPartRevisionID AND sns_inner.snsSerialNumberID = imsSerialNumberID) " + string.Format("AND NOT EXISTS (SELECT sntSerialNumberID FROM SerialNumberTransactions WHERE (sntTransactionType = {0} OR sntTransactionType = 3) AND sntTableUniqueID = {1} AND sntPartID = imsPartID AND sntPartRevisionID = imsPartRevisionID AND sntSerialNumberID = imsSerialNumberID ) ", TransactionType, row.Field<Guid>(base.Field.Table.FieldPrefix + "UniqueID").ToSql());
			}
		}
		string text4 = " imsSerialNumberID, imsPartID, imsPartRevisionID, imsExpirationDate, imsInactive, snsSerialNumberID, snsPartID, snsPartRevisionID, snsPartWarehouseLocationID, snsPartBinID, snsStatus, snsQuantity";
		if (availableTable == null)
		{
			availableTable = database.GetDataTable(string.Format("SELECT " + text4 + " FROM SerialNumbers INNER JOIN SerialNumberStatuses ON imsPartID=snsPartID AND imsPartRevisionID=snsPartRevisionID AND imsSerialNumberID=snsSerialNumberID WHERE {0} {1} AND imsInactive = 0 ORDER BY imsSerialNumberID", text2, arg));
		}
		else
		{
			availableTable.Rows.Clear();
			database.Fill(availableTable, string.Format("SELECT " + text4 + " FROM SerialNumbers INNER JOIN SerialNumberStatuses ON imsPartID=snsPartID AND imsPartRevisionID=snsPartRevisionID AND imsSerialNumberID=snsSerialNumberID WHERE {0} {1} AND imsInactive = 0 ORDER BY imsSerialNumberID", text2, arg));
		}
		List<DataTable> deletedItems = new List<DataTable>();
		List<DataTable> insertedItems = new List<DataTable>();
		GetAllChangedRows(deletedItems, insertedItems, typeof(SerialNumberDefinition));
		if (row != null)
		{
			MergeAvailable(relatedFieldsAndCurrentFieldArray, row, deletedItems, insertedItems);
		}
		RefreshQuantityAvailable();
	}

	public void Remove(M1Database database, DataRow currentRow, DataRow serialNumberRowToRemove, SqlTransaction transaction)
	{
		string value = serialNumberRowToRemove.Field<string>("sntSerialNumberID").Trim();
		Guid g = currentRow.Field<Guid>(base.Field.Table.UniqueField);
		DataTable dataTable = childBindingSource.GetDataTable();
		TransactionChangedEventArgs e = new TransactionChangedEventArgs
		{
			Database = database,
			CurrentRow = currentRow
		};
		foreach (DataRow row in dataTable.Rows)
		{
			if (row.RowState != DataRowState.Deleted && row.Field<Guid>("sntTableUniqueID").Equals(g) && row.Field<string>("sntSerialNumberID").Trim().Equals(value, StringComparison.CurrentCultureIgnoreCase))
			{
				if (IsLatestTransaction(database, row))
				{
					e.TransactionRowChanged = row;
					OnRemoved(e);
					row.Delete();
					base.Field.Validate(database, currentRow, transaction, base.Field.BindingSource.CurrentAsDataRow == currentRow);
					break;
				}
				throw new M1Exception(RemoveFailMessage(serialNumberRowToRemove.Field<string>("sntSerialNumberID").Trim()));
			}
		}
	}

	public DataRow AddRow(M1Database database, DataRow currentRow, DataRow serialNumberRowToAdd, SqlTransaction transaction)
	{
		if (childBindingSource != null && !IsSelected(currentRow, serialNumberRowToAdd.Field<string>("imsSerialNumberID")))
		{
			DataRow dataRow = base.AddTransactionRow(database, currentRow, serialNumberRowToAdd);
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
		if (PartBinField.Length != 0 && TransactionType != 0 && base.Field.DataDictionary != null && base.Field.DataDictionary.ProductCode.IsModulePurchased("SN", base.Field.Database) && allowEditing)
		{
			if (childBindingSource == null)
			{
				childBindingSource = new M1BindingSource(base.Field.BindingSource.Database, base.Field.BindingSource.Transaction);
				childBindingSource.LoadDefinition(new QueryDefinition(base.Field.Database, string.Empty, "SERIALNUMBERTRANSACTIONS")
				{
					FieldList = "*,CAST(sntStatus As TinyInt) As OriginalStatus,CAST(sntQuantity As Decimal) As OriginalQuantity,CAST((Select ISNULL(imsExpirationDate, NULL) from SerialNumbers Where imsSerialNumberID = sntSerialNumberID And imsPartID = sntPartID and imsPartRevisionID = sntPartRevisionID) As Date) As imsExpirationDate"
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

	public byte GetCurrentStatus(M1Database database, SqlTransaction transaction, string partID, string revisionID, string serialNumberID)
	{
		return new SerialNumber().GetCurrentStatus(database, transaction, partID, revisionID, serialNumberID);
	}

	public string CanInactivate(byte status)
	{
		return status switch
		{
			1 => TPrefix + " number is assigned to a job and cannot be marked as inactive", 
			2 => TPrefix + " number is in inventory and cannot be marked as inactive", 
			5 => TPrefix + " number is in inspection and cannot be marked as inactive", 
			7 => TPrefix + " number is in to return and cannot be marked as inactive", 
			_ => string.Empty, 
		};
	}

	public void ActivateNumber(M1Database database, string partID, string revisionID, string serialNumberID, bool activate)
	{
		SqlTransaction sqlTransaction = database.BeginTransaction();
		try
		{
			byte transType = (byte)((!activate) ? 9 : 10);
			SetInactiveStatus(database, sqlTransaction, partID, revisionID, serialNumberID, !activate);
			AddSerialTransaction(database, sqlTransaction, partID, revisionID, string.Empty, string.Empty, serialNumberID, 1m, 0, transType, string.Empty, Guid.Empty, string.Empty, 0, 0, 0, negativeTrans: false);
			database.CommitTransaction(sqlTransaction);
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			throw;
		}
	}

	public void AddSerialTransaction(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, string serialNumberID, decimal quantity, byte status, byte transType, string sourceTable, Guid sourceGUID, string jobID, int asmID, int jobMatID, int jobMatCompID, bool negativeTrans)
	{
		AddSerialTransaction(database, transaction, partID, revisionID, warehouseID, binID, serialNumberID, quantity, status, transType, sourceTable, sourceGUID, jobID, asmID, jobMatID, jobMatCompID, negativeTrans, DateTime.Now);
	}

	public void AddSerialTransaction(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, string serialNumberID, decimal quantity, byte status, byte transType, string sourceTable, Guid sourceGUID, string jobID, int asmID, int jobMatID, int jobMatCompID, bool negativeTrans, DateTime transactionDate)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("INSERT INTO SerialNumberTransactions(");
		stringBuilder.Append("sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionType, sntQuantity, ");
		stringBuilder.Append("sntTransactionDate, sntCreatedBy, sntCreatedDate, sntStatus, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntTableName, sntTableUniqueID, sntNegativeTransaction )");
		stringBuilder.Append("VALUES(@SerialNumberID, @PartID, @PartRevisionID, @WarehouseID, @BinID, @TransactionType, @Quantity, ");
		stringBuilder.Append("@TransactionDate, @CreatedBy, @CreatedDate, @Status, @JobID, @AsmID, @SeqID, @CompID, @SourceTable, @SourceGUID, @NegativeTransaction)");
		SqlCommand sqlCommand = database.NewSqlCommand(stringBuilder.ToString());
		sqlCommand.Parameters.Add(new SqlParameter("@SerialNumberID", SqlDbType.NVarChar)).Value = serialNumberID;
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

	public string GetSerialNumbersUsedInJobWhere(string SerialNumberID)
	{
		return $"imsSerialNumberID In (Select sntSerialNumberID From SerialNumberTransactions Where sntTransactionType IN (4,20) And sntJobID In (Select sntJobID From SerialNumberTransactions Where sntTransactionType In (3,40) And sntSerialNumberID = {SerialNumberID.ToSql()}))";
	}

	public virtual string GetSerialNumbersCreatedByJobWhere(string SerialNumberID)
	{
		return $"imsSerialNumberID In (Select sntSerialNumberID From SerialNumberTransactions Where sntTransactionType In (3,40) And sntJobID In (Select sntJobID From SerialNumberTransactions Where sntTransactionType IN (4,20) And sntSerialNumberID = {SerialNumberID.ToSql()}))";
	}
}
