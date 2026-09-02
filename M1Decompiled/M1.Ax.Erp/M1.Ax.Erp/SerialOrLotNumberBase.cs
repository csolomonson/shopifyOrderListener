using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class SerialOrLotNumberBase : FieldExtension
{
	public class RowsAddedEventArgs : EventArgs
	{
		public DataTable SelectedTable;

		public DataTable AvailableTable;

		public RowsAddedEventArgs(DataTable selectedTable, DataTable availableTable)
		{
			SelectedTable = selectedTable;
			AvailableTable = availableTable;
		}
	}

	public class TransactionChangedEventArgs : EventArgs
	{
		public M1Database Database;

		public DataRow CurrentRow;

		public DataRow TransactionRowChanged;
	}

	public class RowChangedEventArgs : EventArgs
	{
		public M1Database Database;

		public DataRow Row;

		public RowChangedEventArgs(M1Database database, DataRow row)
		{
			Database = database;
			Row = row;
		}
	}

	public M1BindingSource childBindingSource;

	protected string[] availableLastPartInfo = new string[4];

	protected DataTable availableTable;

	protected decimal availableLastQuantity;

	protected string TrPrefix;

	protected string TPrefix;

	protected string HPrefix;

	protected string StatusPrefix = string.Empty;

	protected char curType;

	protected DataTable availableData { get; set; }

	[Browsable(false)]
	public virtual bool IsEnabled
	{
		get
		{
			return childBindingSource != null;
		}
		private set
		{
		}
	}

	public event EventHandler<RowChangedEventArgs> RowChanged;

	public event EventHandler<RowsAddedEventArgs> RowsAdded;

	public event EventHandler<TransactionChangedEventArgs> Added;

	public event EventHandler<TransactionChangedEventArgs> Removed;

	public virtual DataTable LoadLotOrSerialNumbers(M1Database database, string id)
	{
		return LoadLotOrSerialNumbers(database, null, id);
	}

	public virtual DataTable LoadLotOrSerialNumbers(M1Database database, SqlTransaction transaction, string id)
	{
		SqlCommand sqlCommand;
		if (!string.IsNullOrEmpty(id.Trim()))
		{
			sqlCommand = database.NewSqlCommand("SELECT * FROM " + TPrefix + "Numbers WHERE " + HPrefix + TPrefix + "NumberID = @NumberID");
			sqlCommand.Parameters.Add(new SqlParameter("@NumberID", SqlDbType.NVarChar)).Value = id;
		}
		else
		{
			sqlCommand = database.NewSqlCommand("SELECT * FROM " + TPrefix + "Numbers WHERE 0=1");
		}
		if (availableData == null)
		{
			availableData = database.GetDataTable(sqlCommand, transaction);
		}
		else
		{
			availableData.Rows.Clear();
			database.Fill(availableData, sqlCommand, transaction);
		}
		return availableData;
	}

	public override void Dispose()
	{
		if (childBindingSource != null)
		{
			if (base.Field != null)
			{
				LoadComplete(base.Field.BindingSource.Fields, add: false);
			}
			childBindingSource?.Dispose();
			childBindingSource = null;
		}
		availableLastPartInfo = null;
		availableTable = null;
		availableData = null;
		this.RowChanged = null;
		this.RowsAdded = null;
		this.Added = null;
		this.Removed = null;
		base.Dispose();
	}

	public virtual string BuildWhereClause(string prefixType, string prefixColumn, bool header)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("{0}PartID  = @PartID", prefixColumn);
		stringBuilder.AppendFormat(" AND {0}PartRevisionID  = @PartRevisionID", prefixColumn);
		if (!header)
		{
			stringBuilder.AppendFormat(" AND {0}PartWarehouseLocationID  = @PartWarehouseLocationID", prefixColumn);
			stringBuilder.AppendFormat(" AND {0}PartBinID  = @PartBinID", prefixColumn);
		}
		stringBuilder.AppendFormat(" AND {0}{1}NumberID  = @NumberID", prefixColumn, prefixType);
		return stringBuilder.ToString();
	}

	public virtual int GetTransactionCount(M1Database database, DataRow row, SqlTransaction transaction)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("SELECT COUNT(*) AS reccount FROM  {0}NumberTransactions ", TPrefix);
		stringBuilder.AppendFormat(" WHERE {0}", BuildWhereClause(TPrefix, TrPrefix, header: false));
		SqlCommand command = database.NewSqlCommand(stringBuilder.ToString());
		AddKeyFieldAsParameters(ref command, row);
		return Convert.ToInt32(database.ExecuteScalar(command, transaction));
	}

	public void DeleteSelectedNumber(M1Database database, DataRow numberRow)
	{
		SqlTransaction sqlTransaction = database.BeginTransaction();
		try
		{
			if (GetTransactionCount(database, numberRow, sqlTransaction) > 0)
			{
				database.CommitTransaction(sqlTransaction);
				throw new Exception($"The {TPrefix} number for the selected part bin cannot be deleted as there are transactions against it");
			}
			DeleteSerialOrLotNumbers(database, numberRow, sqlTransaction, curType);
			database.CommitTransaction(sqlTransaction);
		}
		catch (SqlException ex)
		{
			database.RollbackTransaction(sqlTransaction);
			throw ex;
		}
		catch (Exception ex2)
		{
			throw ex2;
		}
	}

	public virtual void DeleteSerialOrLotNumbers(M1Database database, DataRow row, SqlTransaction transaction, char type)
	{
		SqlCommand command = new SqlCommand();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("DELETE FROM {0}NumberTransactions WHERE {1}  ;", TPrefix, BuildWhereClause(TPrefix, TrPrefix, header: false));
		stringBuilder.AppendFormat("DELETE FROM {0}Numbers WHERE {1} ;", TPrefix, BuildWhereClause(TPrefix, HPrefix, header: true));
		stringBuilder.AppendFormat("DELETE FROM {0}NumberStatuses WHERE {1} ;", TPrefix, BuildWhereClause(TPrefix, StatusPrefix, header: false));
		command.CommandText = stringBuilder.ToString();
		AddKeyFieldAsParameters(ref command, row);
		command.CommandType = CommandType.Text;
		database.ExecuteCommand(command, transaction);
	}

	public virtual decimal GetStatusQuantity(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, string numberID, byte status)
	{
		SqlCommand sqlCommand = database.NewSqlCommand(string.Format("SELECT {0}Quantity FROM {1}NumberStatuses WHERE {0}PartID = @PartID And {0}PartRevisionID = @PartRevisionID And {0}PartWarehouseLocationID = @PartWarehouseLocationID And {0}PartBinID = @PartBinID And {0}{1}NumberID = @NumberID And {0}Status = @Status", StatusPrefix, TPrefix));
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", partID));
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", revisionID));
		sqlCommand.Parameters.Add(new SqlParameter("@PartWarehouseLocationID", warehouseID));
		sqlCommand.Parameters.Add(new SqlParameter("@PartBinID", binID));
		sqlCommand.Parameters.Add(new SqlParameter("@NumberID", numberID));
		sqlCommand.Parameters.Add(new SqlParameter("@Status", status));
		return Convert.ToDecimal(database.ExecuteScalar(sqlCommand, transaction));
	}

	public virtual void AddKeyFieldAsParameters(ref SqlCommand command, DataRow row)
	{
		command.Parameters.Add(new SqlParameter("@PartID", row.Field<string>($"{TrPrefix}PartID").Trim()));
		command.Parameters.Add(new SqlParameter("@PartRevisionID", row.Field<string>($"{TrPrefix}PartRevisionID").Trim()));
		command.Parameters.Add(new SqlParameter("@PartWarehouseLocationID", availableLastPartInfo[2]));
		command.Parameters.Add(new SqlParameter("@PartBinID", availableLastPartInfo[3]));
		command.Parameters.Add(new SqlParameter("@NumberID", row.Field<string>($"{TrPrefix}{TPrefix}NumberID").Trim()));
	}

	public virtual string BuildFilter(string[] partFieldsArray, DataRow row, bool includeWhBin)
	{
		string result = "(0=1)";
		if (row != null)
		{
			result = ((!includeWhBin) ? string.Format(" {2}PartID = {0} And {2}PartRevisionID = {1} ", row.Field<string>(partFieldsArray[0]).Trim().ToSql(), row.Field<string>(partFieldsArray[1]).Trim().ToSql(), StatusPrefix) : string.Format(" {2}PartID = {0} And {2}PartRevisionID = {1} And {2}PartWarehouseLocationID = {3} And {2}PartBinID = {4} ", row.Field<string>(partFieldsArray[0]).Trim().ToSql(), row.Field<string>(partFieldsArray[1]).Trim().ToSql(), StatusPrefix, row.Field<string>(partFieldsArray[2]).Trim().ToSql(), row.Field<string>(partFieldsArray[3]).Trim().ToSql()));
			availableLastPartInfo[0] = row.Field<string>(partFieldsArray[0]).Trim();
			availableLastPartInfo[1] = row.Field<string>(partFieldsArray[1]).Trim();
			availableLastPartInfo[2] = row.Field<string>(partFieldsArray[2]).Trim();
			availableLastPartInfo[3] = row.Field<string>(partFieldsArray[3]).Trim();
		}
		else
		{
			availableLastPartInfo[0] = string.Empty;
			availableLastPartInfo[1] = string.Empty;
			availableLastPartInfo[2] = string.Empty;
			availableLastPartInfo[3] = string.Empty;
		}
		return result;
	}

	public virtual DataTable GetSelectedItems()
	{
		if (childBindingSource != null)
		{
			return childBindingSource.GetDataTable();
		}
		return null;
	}

	public override bool IsRequired(M1Database database, DataRow row)
	{
		bool flag = base.IsRequired(database, row);
		if (!flag && row != null && row.RowState != DataRowState.Detached && PartBinField.Length != 0)
		{
			string text = base.Field.BindingSource.Fields[PartBinField].RelatedFieldsAndCurrentFieldArray[0];
			if (row.Field<string>(text).Trim().Length != 0)
			{
				DataRow dataRow = base.Field.BindingSource.Fields[text].RelatedTableGetDataRow($"impTrack{TPrefix}Numbers", database, row);
				if (dataRow != null && dataRow.Field<bool>($"impTrack{TPrefix}Numbers"))
				{
					flag = true;
				}
			}
		}
		return flag;
	}

	public virtual void GetAllChangedRows(List<DataTable> deletedItems, List<DataTable> insertedItems, Type actionType)
	{
		foreach (FieldDefinition field in base.Field.Table.BindingSource.Fields)
		{
			if (field.FieldExtensions == null)
			{
				continue;
			}
			foreach (FieldExtension fieldExtension in field.FieldExtensions)
			{
				if (!(fieldExtension.GetType() == actionType))
				{
					continue;
				}
				SerialOrLotNumberBase serialOrLotNumberBase = (SerialOrLotNumberBase)fieldExtension;
				if (!serialOrLotNumberBase.IsEnabled)
				{
					continue;
				}
				DataTable selectedItems = serialOrLotNumberBase.GetSelectedItems();
				if (selectedItems != null)
				{
					DataTable changes = selectedItems.GetChanges(DataRowState.Deleted);
					if (changes != null)
					{
						deletedItems.Add(changes);
					}
					changes = selectedItems.GetChanges(DataRowState.Added);
					if (changes != null)
					{
						insertedItems.Add(changes);
					}
				}
			}
		}
	}

	public virtual bool IsSelected(DataRow currentRow, string NumberID)
	{
		Guid g = currentRow.Field<Guid>(base.Field.Table.UniqueField);
		foreach (DataRow row in childBindingSource.GetDataTable().Rows)
		{
			if (row.RowState != DataRowState.Deleted && row.Field<Guid>($"{TrPrefix}TableUniqueID").Equals(g) && row.Field<string>($"{TrPrefix}{TPrefix}NumberID").Trim().Equals(NumberID.Trim()))
			{
				return true;
			}
		}
		return false;
	}

	public virtual DataRow FindFromAvailableList(DataRow currentRow, DataRow Row)
	{
		string value;
		string value2;
		string value3;
		string value4;
		string text;
		decimal totalQuantitySelected;
		if (Row.HasVersion(DataRowVersion.Original))
		{
			value = Row.Field<string>($"{TrPrefix}PartID", DataRowVersion.Original).Trim();
			value2 = Row.Field<string>($"{TrPrefix}PartRevisionID", DataRowVersion.Original).Trim();
			value3 = Row.Field<string>($"{TrPrefix}PartWarehouseLocationID", DataRowVersion.Original).Trim();
			value4 = Row.Field<string>($"{TrPrefix}PartBinID", DataRowVersion.Original).Trim();
			text = Row.Field<string>($"{TrPrefix}{TPrefix}NumberID", DataRowVersion.Original).Trim();
			byte status = Row.Field<byte>($"{TrPrefix}Status", DataRowVersion.Original);
			totalQuantitySelected = GetTotalQuantitySelected(GetType(), Row.Field<string>($"{TrPrefix}PartID", DataRowVersion.Original), Row.Field<string>($"{TrPrefix}PartRevisionID", DataRowVersion.Original), Row.Field<string>($"{TrPrefix}PartWarehouseLocationID", DataRowVersion.Original), Row.Field<string>($"{TrPrefix}PartBinID", DataRowVersion.Original), Row.Field<string>($"{TrPrefix}{TPrefix}NumberID", DataRowVersion.Original), status);
		}
		else
		{
			value = Row.Field<string>($"{TrPrefix}PartID").Trim();
			value2 = Row.Field<string>($"{TrPrefix}PartRevisionID").Trim();
			value3 = Row.Field<string>($"{TrPrefix}PartWarehouseLocationID").Trim();
			value4 = Row.Field<string>($"{TrPrefix}PartBinID").Trim();
			text = Row.Field<string>($"{TrPrefix}{TPrefix}NumberID").Trim();
			byte status = Row.Field<byte>($"{TrPrefix}Status");
			totalQuantitySelected = GetTotalQuantitySelected(GetType(), Row.Field<string>($"{TrPrefix}PartID"), Row.Field<string>($"{TrPrefix}PartRevisionID"), Row.Field<string>($"{TrPrefix}PartWarehouseLocationID"), Row.Field<string>($"{TrPrefix}PartBinID"), Row.Field<string>($"{TrPrefix}{TPrefix}NumberID"), status);
		}
		currentRow.Field<Guid>(base.Field.Table.UniqueField);
		DataRow result = null;
		foreach (DataRow row in availableTable.Rows)
		{
			if (row.Field<string>($"{StatusPrefix}PartID").Trim().Equals(value) && row.Field<string>($"{StatusPrefix}PartRevisionID").Trim().Equals(value2) && row.Field<string>($"{StatusPrefix}PartWarehouseLocationID").Trim().Equals(value3) && row.Field<string>($"{StatusPrefix}PartBinID").Trim().Equals(value4) && row.Field<string>($"{StatusPrefix}{TPrefix}NumberID").Trim().Equals(text))
			{
				if (Parameters.ToUpper().Contains("INCLUDEUNASSIGNED"))
				{
					if (IsSelected(currentRow, text) || curType.Equals('S') || curType.Equals('L'))
					{
						result = row;
						break;
					}
				}
				else if (row.Field<decimal>(StatusPrefix + "Quantity") <= totalQuantitySelected || IsSelected(currentRow, text) || curType.Equals('S'))
				{
					result = row;
					break;
				}
			}
			else
			{
				if ((!row.Field<byte>($"{StatusPrefix}Status").Equals(0) && !base.Field.TableName.ToUpper().Equals("WAREHOUSERECEIPTLINES", StringComparison.CurrentCultureIgnoreCase) && !base.Field.TableName.ToUpper().Equals("WAREHOUSERECEIPTCOMPONENTS", StringComparison.CurrentCultureIgnoreCase)) || !row.Field<string>($"{StatusPrefix}PartID").Trim().Equals(value) || !row.Field<string>($"{StatusPrefix}PartRevisionID").Trim().Equals(value2) || !row.Field<string>($"{StatusPrefix}{TPrefix}NumberID").Trim().Equals(text))
				{
					continue;
				}
				if (Parameters.ToUpper().Contains("INCLUDEUNASSIGNED"))
				{
					if (IsSelected(currentRow, text))
					{
						if (curType.Equals('S'))
						{
							result = row;
							break;
						}
						if (row.Field<decimal>(StatusPrefix + "Quantity") <= totalQuantitySelected)
						{
							result = row;
							break;
						}
					}
					else if (row.Field<decimal>(StatusPrefix + "Quantity") <= totalQuantitySelected)
					{
						result = row;
						break;
					}
				}
				else if (row.Field<decimal>(StatusPrefix + "Quantity") <= totalQuantitySelected)
				{
					result = row;
					break;
				}
			}
		}
		return result;
	}

	public virtual void CancelBindingSourceEdit()
	{
		if (childBindingSource != null)
		{
			childBindingSource.CancelEdit();
		}
	}

	public virtual void ClearBindingSourceCache()
	{
		if (childBindingSource != null)
		{
			childBindingSource.ClearCache();
		}
	}

	public virtual void AddToAvailableList(string[] relatedFields, DataRow currentRow, DataRow Row)
	{
		if (availableTable == null)
		{
			return;
		}
		DataRowVersion version = ((!Row.HasVersion(DataRowVersion.Original)) ? DataRowVersion.Current : DataRowVersion.Original);
		string text = Row.Field<string>($"{TrPrefix}PartID", version).Trim();
		string text2 = Row.Field<string>($"{TrPrefix}PartRevisionID", version).Trim();
		string text3 = Row.Field<string>($"{TrPrefix}PartWarehouseLocationID", version).Trim();
		string text4 = Row.Field<string>($"{TrPrefix}PartBinID", version).Trim();
		string text5 = Row.Field<string>($"{TrPrefix}{TPrefix}NumberID", version).Trim();
		DateTime? dateTime = Row.Field<DateTime?>($"{HPrefix}ExpirationDate", version);
		byte b = Row.Field<byte>($"{TrPrefix}Status", version);
		foreach (string text6 in relatedFields)
		{
			if (currentRow != null && currentRow.Table != null && currentRow.Table.Columns.Contains(text6))
			{
				string obj = ((currentRow[text6] != null) ? currentRow[text6].ToString() : string.Empty);
				string value = ((text6.IndexOf("PartID", StringComparison.InvariantCultureIgnoreCase) >= 0) ? text : ((text6.IndexOf("PartRevisionID", StringComparison.InvariantCultureIgnoreCase) >= 0) ? text2 : ((text6.IndexOf("Warehouse", StringComparison.InvariantCultureIgnoreCase) >= 0) ? text3 : ((text6.IndexOf("PartBinID", StringComparison.InvariantCultureIgnoreCase) >= 0) ? text4 : string.Empty))));
				if (!obj.Equals(value, StringComparison.InvariantCultureIgnoreCase))
				{
					return;
				}
			}
		}
		foreach (DataRow row in availableTable.Rows)
		{
			if (row.Field<string>($"{StatusPrefix}PartID").Trim().Equals(text) && row.Field<string>($"{StatusPrefix}PartRevisionID").Trim().Equals(text2) && row.Field<string>($"{StatusPrefix}PartWarehouseLocationID").Trim().Equals(text3) && row.Field<string>($"{StatusPrefix}PartBinID").Trim().Equals(text4) && row.Field<string>($"{StatusPrefix}{TPrefix}NumberID").Trim().Equals(text5) && object.Equals(row.Field<DateTime?>($"{HPrefix}ExpirationDate"), dateTime))
			{
				return;
			}
		}
		DataRow dataRow = availableTable.NewRow().BlankRow();
		dataRow.BeginEdit();
		dataRow.SetField($"{StatusPrefix}PartID", Row.Field<string>($"{TrPrefix}PartID", version));
		dataRow.SetField($"{StatusPrefix}PartRevisionID", Row.Field<string>($"{TrPrefix}PartRevisionID", version));
		dataRow.SetField($"{StatusPrefix}PartWarehouseLocationID", Row.Field<string>($"{TrPrefix}PartWarehouseLocationID", version));
		dataRow.SetField($"{StatusPrefix}PartBinID", Row.Field<string>($"{TrPrefix}PartBinID", version));
		dataRow.SetField($"{StatusPrefix}{TPrefix}NumberID", text5);
		dataRow.SetField($"{StatusPrefix}Status", b);
		decimal statusQuantity = GetStatusQuantity(childBindingSource.Database, null, text, text2, text3, text4, text5, b);
		decimal totalQuantitySelected = GetTotalQuantitySelected(GetType(), text, text2, text3, text4, text5, b);
		decimal num = statusQuantity - totalQuantitySelected;
		if (curType.Equals('L') && num < 0m)
		{
			dataRow.SetField($"{StatusPrefix}Quantity", statusQuantity);
		}
		if (num > 0m)
		{
			dataRow.SetField($"{StatusPrefix}Quantity", num);
		}
		dataRow.SetField($"{HPrefix}PartID", Row.Field<string>($"{TrPrefix}PartID", version));
		dataRow.SetField($"{HPrefix}PartRevisionID", Row.Field<string>($"{TrPrefix}PartRevisionID", version));
		dataRow.SetField($"{HPrefix}{TPrefix}NumberID", text5);
		dataRow.SetField(HPrefix + "ExpirationDate", dateTime);
		dataRow.EndEdit();
		availableTable.Rows.Add(dataRow);
	}

	public virtual bool IsLatestTransaction(M1Database database, DataRow row)
	{
		DateTime dateTime = DateTime.Today.AddHours(23.0).AddMinutes(59.0).AddSeconds(59.0);
		string queryString = " SELECT IsNull(Count(*),0) FROM " + TPrefix + "NumberTransactions WHERE " + TrPrefix + "PartID = @PartID AND " + TrPrefix + "PartRevisionID = @PartRevisionID AND " + TrPrefix + "PartWarehouseLocationID = @PartWarehouseLocationID AND " + TrPrefix + "PartBinID = @BinID AND " + TrPrefix + TPrefix + "NumberID = @NumberID AND " + TrPrefix + "TransactionType <> 19 And " + TrPrefix + "TransactionDate > @TransactionDate";
		SqlCommand sqlCommand = database.NewSqlCommand(queryString);
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = row.Field<string>(TrPrefix + "PartID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = row.Field<string>(TrPrefix + "PartRevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartWarehouseLocationID", SqlDbType.NVarChar)).Value = row.Field<string>(TrPrefix + "PartWarehouseLocationID");
		sqlCommand.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = row.Field<string>(TrPrefix + "PartBinID");
		sqlCommand.Parameters.Add(new SqlParameter("@NumberID", SqlDbType.NVarChar)).Value = row.Field<string>(TrPrefix + TPrefix + "NumberID");
		sqlCommand.Parameters.Add(new SqlParameter("@TransactionDate", SqlDbType.DateTime)).Value = ((row[TrPrefix + "TransactionDate"] == DBNull.Value) ? dateTime : row.Field<DateTime>(TrPrefix + "TransactionDate"));
		return Convert.ToInt32(database.ExecuteScalar(sqlCommand)) == 0;
	}

	public override void Validate(M1Database database, DataRow row, ValidationInfo errorInfo)
	{
		if ((!string.IsNullOrWhiteSpace(base.Field.VisibleExpression) && !base.Field.Table.EvaluateScriptExpressionBool(base.Field.VisibleExpression, base.Field.Database, row)) || (row != null && row.Table.Columns.Contains(base.Field.FieldName.Substring(0, 3) + "ReversalEntry") && row.Field<bool>(base.Field.FieldName.Substring(0, 3) + "ReversalEntry")) || (row != null && row.Table.Columns.Contains(base.Field.FieldName.Substring(0, 3) + "Reversed") && row.Field<bool>(base.Field.FieldName.Substring(0, 3) + "Reversed")))
		{
			return;
		}
		if (row != null && row.Table.Columns.Contains(base.Field.FieldName.Substring(0, 3) + "Reversed"))
		{
			DataColumn dataColumn = (from DataColumn col in row.Table.Columns
				where col.ColumnName.StartsWith(col.ColumnName.Substring(0, 3) + "Reverse") && col.ColumnName.EndsWith("ID") && col.DataType == typeof(string)
				select col).FirstOrDefault();
			if (dataColumn != null && row.Field<string>(dataColumn.ColumnName) != string.Empty)
			{
				return;
			}
		}
		base.Validate(database, row, errorInfo);
		if (childBindingSource == null || !IsRequired(database, row))
		{
			return;
		}
		if (errorInfo == null)
		{
			errorInfo = new ValidationInfo();
		}
		string rowFilter = $"{TrPrefix}TableUniqueID = {row.Field<Guid>(base.Field.Table.UniqueField).ToLinq()}";
		if (!(childBindingSource.List.GetType() == typeof(DataView)))
		{
			return;
		}
		decimal num = default(decimal);
		string text = base.Field.BindingSource.Fields[PartBinField].RelatedFieldsFormatCaptionAndCurrentValues(row);
		decimal num2 = default(decimal);
		DataView dataView = new DataView(((DataView)childBindingSource.List).Table, rowFilter, string.Empty, DataViewRowState.CurrentRows);
		if (dataView.Count > 0)
		{
			decimal num3 = default(decimal);
			foreach (DataRowView item in dataView)
			{
				num3 += Convert.ToDecimal(item.Row[TPrefix.Equals("Serial") ? "sntQuantity" : "abtQuantity"]);
			}
			num2 = num3;
		}
		if (TPrefix.Equals("Serial"))
		{
			num = Math.Abs(Convert.ToDecimal(row[base.Field.FieldName]));
			if (Convert.ToDecimal(row[base.Field.FieldName]) % 1m != 0m)
			{
				errorInfo.AddError($"{base.Field.Caption} has a non whole number entered. Only whole numbers are allowed for serial numbers ( {num.ToString()} entered )");
			}
		}
		else
		{
			num = Convert.ToDecimal(row[base.Field.FieldName]);
		}
		if (TPrefix.Equals("Lot") && base.Field.BindingSource.PrimaryTable.TableName.ToString().Equals("QUANTITYADJUSTMENTS", StringComparison.CurrentCultureIgnoreCase) && row.Field<byte>("inqAdjustmentType").Equals(1))
		{
			if (num2 > row.Field<decimal>("inqCountedQuantity"))
			{
				errorInfo.AddError(string.Format("{0} has too many {1} numbers selected for {2} ( {3}  selected - {4} required )", base.Field.Caption, TPrefix, text, num2.ToString(), row.Field<decimal>("inqCountedQuantity").ToString()));
			}
			else if (num2 < row.Field<decimal>("inqCountedQuantity") && !AllowMismatchedQuantity)
			{
				errorInfo.AddError(string.Format("{0} does not have enough {1} numbers selected for {2} ( {3} selected - {4}  required )", base.Field.Caption, TPrefix, text, num2.ToString(), row.Field<decimal>("inqCountedQuantity").ToString()));
			}
		}
		else if (num2 != num)
		{
			if (num2 > num)
			{
				errorInfo.AddError($"{base.Field.Caption} has too many {TPrefix} numbers selected for {text}  ( {num2.ToString()}  selected - {num.ToString()} required)");
			}
			else if ((num2 < num && !AllowMismatchedQuantity) || (num2 < num && AllowMismatchedQuantity && base.Field.BindingSource.PrimaryTable.TableName.ToString().Equals("InspectionLines", StringComparison.CurrentCultureIgnoreCase) && string.IsNullOrEmpty(row.Field<string>("qalSourceTableName"))))
			{
				string errorText = $"{base.Field.Caption} does not have enough {TPrefix} numbers selected for {text} ( {num2.ToString()} selected - {num.ToString()}  required )";
				if (TPrefix.Equals("Serial"))
				{
					errorInfo.AddError(errorText, ErrorItem.ErrorSource.Serial);
				}
				else
				{
					errorInfo.AddError(errorText, ErrorItem.ErrorSource.Lot);
				}
			}
		}
		foreach (DataRowView item2 in dataView)
		{
			if (item2.Row[$"{HPrefix}ExpirationDate"] != DBNull.Value && item2.Row.Field<DateTime>($"{HPrefix}ExpirationDate").CompareTo(DateTime.Now) < 0)
			{
				errorInfo.AddWarning($"{TPrefix} number {item2.Row.Field<string>($"{TrPrefix}{TPrefix}NumberID").Trim()} has expired ");
			}
		}
	}

	public virtual bool CheckAvailableFromBase(M1Database database, DataRow row, bool forceRefresh)
	{
		bool flag = false;
		if (row != null)
		{
			string[] relatedFieldsAndCurrentFieldArray = base.Field.BindingSource.Fields[PartBinField].RelatedFieldsAndCurrentFieldArray;
			if (!availableLastPartInfo[0].Equals(row.Field<string>(relatedFieldsAndCurrentFieldArray[0]).Trim()) || !availableLastPartInfo[1].Equals(row.Field<string>(relatedFieldsAndCurrentFieldArray[1]).Trim()) || !availableLastPartInfo[2].Equals(row.Field<string>(relatedFieldsAndCurrentFieldArray[2]).Trim()) || !availableLastPartInfo[3].Equals(row.Field<string>(relatedFieldsAndCurrentFieldArray[3]).Trim()))
			{
				flag = true;
			}
			if (!flag && ((availableLastQuantity < 0m && row.Field<decimal>(base.Field.FieldName) >= 0m) || (availableLastQuantity >= 0m && row.Field<decimal>(base.Field.FieldName) < 0m)))
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
		}
		return flag;
	}

	public virtual void UpdateHeaderTable(M1Database database, DataRow row, SqlTransaction sqlTransaction)
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		_ = string.Empty;
		_ = string.Empty;
		string empty3 = string.Empty;
		if (row.RowState == DataRowState.Deleted)
		{
			empty = row.Field<string>($"{TrPrefix}PartID", DataRowVersion.Original).Trim();
			empty2 = row.Field<string>($"{TrPrefix}PartRevisionID", DataRowVersion.Original).Trim();
			row.Field<string>($"{TrPrefix}PartWarehouseLocationID", DataRowVersion.Original).Trim();
			row.Field<string>($"{TrPrefix}PartBinID", DataRowVersion.Original).Trim();
			empty3 = row.Field<string>($"{TrPrefix}{TPrefix}NumberID", DataRowVersion.Original).Trim();
		}
		else
		{
			if (!isAllowedRefreshStatus(database, row, sqlTransaction))
			{
				return;
			}
			empty = row.Field<string>($"{TrPrefix}PartID").Trim();
			empty2 = row.Field<string>($"{TrPrefix}PartRevisionID").Trim();
			row.Field<string>($"{TrPrefix}PartWarehouseLocationID").Trim();
			row.Field<string>($"{TrPrefix}PartBinID").Trim();
			empty3 = row.Field<string>($"{TrPrefix}{TPrefix}NumberID").Trim();
		}
		RefreshStatuses(database, sqlTransaction, empty, empty2, empty3);
	}

	private bool isAllowedRefreshStatus(M1Database database, DataRow row, SqlTransaction sqlTransaction)
	{
		if (base.Field.TableName.Equals("InspectionLines", StringComparison.CurrentCultureIgnoreCase))
		{
			DataRow currentAsDataRow = base.Field.BindingSource.CurrentAsDataRow;
			if (currentAsDataRow != null && string.IsNullOrEmpty(currentAsDataRow.Field<string>("qalSourceTableName")) && !currentAsDataRow.Field<bool>("qalManualInspectionFinalized"))
			{
				return false;
			}
		}
		return true;
	}

	private bool isAlreadyPosted(SerialOrLotNumberBase seriallLotExt)
	{
		if (string.IsNullOrEmpty(base.Field?.TableName) || seriallLotExt.Field.BindingSource.CurrentAsDataRow == null)
		{
			return false;
		}
		if (base.Field.TableName.Equals("InspectionComponents", StringComparison.CurrentCultureIgnoreCase) && seriallLotExt.Field.FieldName.Equals("qamComponentQtyToInspect", StringComparison.InvariantCultureIgnoreCase) && (base.Field.FieldName.Equals("qamInvQuantityAccepted", StringComparison.InvariantCultureIgnoreCase) || base.Field.FieldName.Equals("qamInvQuantityToScrap", StringComparison.InvariantCultureIgnoreCase)))
		{
			DataRow currentAsDataRow = seriallLotExt.Field.BindingSource.CurrentAsDataRow;
			if (currentAsDataRow != null && string.IsNullOrEmpty(currentAsDataRow.Field<string>("qamSourceTableName")) && currentAsDataRow.Field<bool>("qamManualInspectionFinalized"))
			{
				return true;
			}
		}
		return false;
	}

	public byte GetLatestTransactionType(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, string ID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT Top 1 IsNull(" + TrPrefix + "TransactionType,0) FROM " + TPrefix + "NumberTransactions WHERE " + TrPrefix + "PartID = @PartID AND " + TrPrefix + "PartRevisionID = @PartRevisionID AND " + TrPrefix + "PartWarehouseLocationID = @PartWarehouseLocationID AND " + TrPrefix + "PartBinID = @PartBinID AND " + TrPrefix + TPrefix + "NumberID = @ID AND " + TrPrefix + "TransactionType <> 19 ORDER BY " + TrPrefix + "TransactionDate Desc, " + TrPrefix + TPrefix + "NumberTransactionID Desc");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartWarehouseLocationID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartBinID", SqlDbType.NVarChar)).Value = binID;
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = ID;
		return Convert.ToByte(database.ExecuteScalar(sqlCommand, transaction));
	}

	public byte GetActiveStatusFromTransactions(M1Database database, SqlTransaction transaction, string partID, string revisionID, string ID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT Top 1 IsNull(" + TrPrefix + "TransactionType,0) FROM " + TPrefix + "NumberTransactions WHERE " + TrPrefix + "PartID = @PartID AND " + TrPrefix + "PartRevisionID = @PartRevisionID AND " + TrPrefix + "TransactionType IN (9,10) AND " + TrPrefix + TPrefix + "NumberID = @ID ORDER BY " + TrPrefix + "TransactionDate Desc, " + TrPrefix + TPrefix + "NumberTransactionID Desc");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = ID;
		return Convert.ToByte(database.ExecuteScalar(sqlCommand, transaction));
	}

	public DateTime? GetLastInActiveDateFromTransactions(M1Database database, SqlTransaction transaction, string partID, string revisionID, string ID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT Top 1 " + TrPrefix + "TransactionDate FROM " + TPrefix + "NumberTransactions WHERE " + TrPrefix + "PartID = @PartID AND " + TrPrefix + "PartRevisionID = @PartRevisionID AND " + TrPrefix + "TransactionType = 9 AND " + TrPrefix + TPrefix + "NumberID = @ID ORDER BY " + TrPrefix + "TransactionDate Desc, " + TrPrefix + TPrefix + "NumberTransactionID Desc");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = ID;
		return Convert.ToDateTime(database.ExecuteScalar(sqlCommand, transaction));
	}

	public byte GetLatestStatus(M1Database database, SqlTransaction transaction, string partID, string revisionID, string ID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT Top 1 IsNull(" + TrPrefix + "Status,0), " + TrPrefix + "PartWarehouseLocationID, " + TrPrefix + "PartBinID FROM " + TPrefix + "NumberTransactions WHERE " + TrPrefix + "PartID = @PartID AND " + TrPrefix + "PartRevisionID = @PartRevisionID AND " + TrPrefix + TPrefix + "NumberID = @ID AND " + TrPrefix + "TransactionType <> 19 ORDER BY " + TrPrefix + "TransactionDate Desc, " + TrPrefix + TPrefix + "NumberTransactionID Desc");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = ID;
		return Convert.ToByte(database.ExecuteScalar(sqlCommand, transaction));
	}

	public void RemoveSerialNumber(M1Database database, SqlTransaction transaction, string serialNumberID, string partID, string revisionID, string warehouseID, string partBinID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("DELETE FROM SerialNumberStatuses WHERE snsSerialNumberID=@SerialNumberID AND snsPartID=@PartID AND snsPartRevisionID=@RevisionID AND snsPartWarehouseLocationID=@WarehouseID AND snsPartBinID=@PartBinID");
		sqlCommand.Parameters.Add(new SqlParameter("@SerialNumberID", SqlDbType.NVarChar)).Value = serialNumberID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartBinID", SqlDbType.NVarChar)).Value = partBinID;
		database.ExecuteCommand(sqlCommand, transaction);
	}

	public void RemoveLotNumber(M1Database database, SqlTransaction transaction, string lotNumberID, string partID, string revisionID, string warehouseID, string partBinID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("DELETE FROM LotNumberStatuses WHERE absLotNumberID=@LotNumberID AND absPartID=@PartID AND absPartRevisionID=@RevisionID AND absPartWarehouseLocationID=@WarehouseID AND absPartBinID=@PartBinID");
		sqlCommand.Parameters.Add(new SqlParameter("@LotNumberID", SqlDbType.NVarChar)).Value = lotNumberID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartBinID", SqlDbType.NVarChar)).Value = partBinID;
		database.ExecuteCommand(sqlCommand, transaction);
	}

	public void RefreshStatuses(M1Database database, SqlTransaction transaction, string partID, string revisionID, string ID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select " + HPrefix + TPrefix + "NumberID From " + TPrefix + "Numbers Where " + HPrefix + "PartID = @PartID And " + HPrefix + "PartRevisionID = @RevisionID And " + HPrefix + TPrefix + "NumberID = @ID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = ID;
		if (database.ExecuteScalar(sqlCommand, transaction) == null)
		{
			return;
		}
		SqlCommand sqlCommand2 = database.NewSqlCommand("Delete From " + TPrefix + "NumberStatuses Where " + StatusPrefix + "PartID = @PartID And " + StatusPrefix + "PartRevisionID = @RevisionID And " + StatusPrefix + TPrefix + "NumberID = @ID");
		sqlCommand2.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand2.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand2.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = ID;
		database.ExecuteScalar(sqlCommand2, transaction);
		if (curType.Equals('L'))
		{
			SqlCommand insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType IN (1,41,47)),0) - IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType IN (3, 40)),0) ) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 1);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select abtQuantity From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtLotNumberTransactionID = IsNull((Select Top 1 abtLotNumberTransactionID From LotNumberTransactions Where abtLotNumberID = @Id and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And (abtTransactionType = 27 Or abtTransactionType = 46) Order By abtTransactionDate Desc,abtLotNumberTransactionID Desc),0)),0) + IsNull((Select SUM(Case When abtTransactionType In (2,3,26,16,12,72,73) Then abtQuantity When abtTransactionType In (41,21,5,8,6,7,66) Then -abtQuantity When abtTransactionType In (14) And abtTableName IN ('INSPECTIONLINES','INSPECTIONCOMPONENTS') Then -abtQuantity When abtTransactionType In (4,20,21,17,22,23) And abtTableName IN ('MATERIALISSUELINES','MATERIALISSUECOMPONENTS','SFE') Then -abtQuantity Else 0 End) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionDate >= IsNull((Select Top 1 abtTransactionDate From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And (abtTransactionType = 27 Or abtTransactionType = 46) Order By abtTransactionDate Desc,abtLotNumberTransactionID Desc),0)),0)) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 2);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (4,20)),0) - IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (72)),0)) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 3);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate)\r\n                                                            SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate\r\n                                                            FROM(\r\n                                                                    SELECT TOP 1 abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,abtCreatedDate, \r\n\t                                                                    (\r\n\t\t                                                                    ISNULL(\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t(SELECT CASE \r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tWHEN a1.abtQuantity < 0 THEN 0\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tELSE a1.abtQuantity\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tEND\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tFROM(\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tSELECT ISNULL(SUM(abtQuantity), 0) abtQuantity\r\n\t\t\t\t\t\t\t\t                                                            FROM LotNumberTransactions\r\n\t\t\t\t\t\t\t\t                                                            WHERE abtLotNumberID = @ID AND abtPartID = @PartID AND abtPartRevisionID = @RevisionID  \t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t                                                            AND abtTransactionType IN (5,40,42,11)\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t) a1\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t)\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t, 0)\r\n\t\t                                                                    -\r\n\t\t                                                                    ISNULL(\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t(SELECT CASE \r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tWHEN a1.abtQuantity < 0 THEN 0\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tELSE a1.abtQuantity\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tEND\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tFROM(\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tSELECT ISNULL(SUM(abtQuantity), 0) abtQuantity\r\n\t\t\t\t\t\t\t\t                                                            FROM LotNumberTransactions \r\n\t\t\t\t\t\t\t\t                                                            WHERE abtLotNumberID = @ID AND abtPartID = @PartID AND abtPartRevisionID = @RevisionID \r\n\t\t\t\t\t\t\t\t                                                            AND abtTransactionType in (67)\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t) a1\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t)\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t, 0)\r\n\t\t                                                                    -\r\n\t\t                                                                    ISNULL(\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t(SELECT CASE \r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tWHEN a1.abtQuantity < 0 THEN 0\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tELSE a1.abtQuantity\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tEND\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tFROM(\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tSELECT ISNULL(SUM(abtQuantity), 0) abtQuantity\r\n\t\t\t\t\t\t\t\t                                                            FROM LotNumberTransactions \r\n\t\t\t\t\t\t\t\t                                                            WHERE abtLotNumberID = @ID AND abtPartID = @PartID AND abtPartRevisionID = @RevisionID \r\n\t\t\t\t\t\t\t\t                                                            AND abtTransactionType IN (14,2) AND abtTableName IN ('RMARECEIPTLINES','RMARECEIPTCOMPONENTS')\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t) a1\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t)\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t, 0)\r\n\t                                                                    ) AS QTY\r\n                                                                    FROM LotNumberTransactions  \r\n                                                                    WHERE abtLotNumberID = @ID AND abtPartID = @PartID AND abtPartRevisionID = @RevisionID\r\n                                                                    GROUP BY abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,abtCreatedDate\r\n                                                                    ORDER BY abtCreatedDate DESC\r\n                                                                ) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 4);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (14)),0) - IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (53,54,55,56,57,58,59,60) And abtTableName IN ('INSPECTIONLINES','INSPECTIONCOMPONENTS')),0)) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 5);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (17,22,23,44)),0)) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 6);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (18,24,25,45)),0) - IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (5,40,42) And abtTableName IN ('DMRSHIPMENTLINES','DMRSHIPMENTCOMPONENTS')),0)) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 7);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select SUM(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType = 6 And abtTransactionDate >= IsNull((Select Top 1 abtTransactionDate From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID  Order By abtTransactionDate Desc,abtLotNumberTransactionID Desc),0)),0)) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 10);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select sum(Case When abtNegativeTransaction = 1 Then abtQuantity * -1 Else abtQuantity End) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (28,29,30,31,32,33,70,71)),0) - IsNull((Select sum(Case When abtNegativeTransaction = 1 Then abtQuantity * -1 Else abtQuantity End) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (4,20,21,17,22,23,72,73) And abtTableName IN ('MATERIALISSUELINES','MATERIALISSUECOMPONENTS')),0)) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 11);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (34,35,36,37,38,39)),0) - IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (3,23,20,4,2,14) And abtTableName IN ('MFGRECEIPTS','MFGRECEIPTCOMPONENTS')),0)) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 12);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select SUM(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType = 7 And abtTransactionDate >= IsNull((Select Top 1 abtTransactionDate From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID  Order By abtTransactionDate Desc,abtLotNumberTransactionID Desc),0)),0)) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 13);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (8)),0) - IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtTransactionType in (26)),0)) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 14);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (48,49,50)),0) - IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (2,4,14) And abtTableName IN ('RECEIPTLINES','RECEIPTCOMPONENTS')),0)) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 15);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (51,52)),0) - IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (5,40) And abtTableName IN ('SHIPMENTLINES','SHIPMENTCOMPONENTS')),0)) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 16);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (53,54,55,56,57,58,59,60)),0) - IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (16,4,23,22,17,18,24) And abtTableName IN ('INSPECTIONLINES','INSPECTIONCOMPONENTS')),0)) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 17);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (61,62,63)),0) - IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (5,42,40) And abtTableName IN ('DMRSHIPMENTLINES','DMRSHIPMENTCOMPONENTS')),0)) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 18);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (64,65)),0) - IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (2,14) And abtTableName IN ('RMARECEIPTLINES','RMARECEIPTCOMPONENTS')),0)) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 19);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (66)),0) - IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (11) And abtTableName IN ('WAREHOUSETRANSFERLINES','WAREHOUSETRANSFERCOMPONENTS')),0)) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 20);
			database.ExecuteCommand(insertCommand, transaction);
			insertCommand = database.NewSqlCommand("INSERT INTO LotNumberStatuses (absPartID,absPartRevisionID,absPartWarehouseLocationID,absPartBinID,absLotNumberID,absStatus,absQuantity,absCreatedBy,absCreatedDate) SELECT abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,@Status,CASE WHEN QTY < 0 THEN 0 ELSE QTY END,@CreatedBy,@CreatedDate FROM (Select abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID,(IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (67)),0) - IsNull((Select sum(abtQuantity) From LotNumberTransactions Where abtLotNumberID = X.abtLotNumberID and abtPartID = X.abtPartID And abtPartRevisionID = X.abtPartRevisionID And abtPartWarehouseLocationID = X.abtPartWarehouseLocationID And abtPartBinID = X.abtPartBinID And abtTransactionType in (12) And abtTableName IN ('WAREHOUSERECEIPTLINES','WAREHOUSERECEIPTCOMPONENTS')),0)) As QTY From LotNumberTransactions X Where abtLotNumberID = @ID and abtPartID = @PartID And abtPartRevisionID = @RevisionID Group By abtPartID,abtPartRevisionID,abtPartWarehouseLocationID,abtPartBinID,abtLotNumberID) X Where X.QTY > 0");
			setParametersForRefreshStatuses(ref insertCommand, database, partID, revisionID, ID, 21);
			database.ExecuteCommand(insertCommand, transaction);
		}
		else
		{
			byte b = 0;
			b = GetLatestStatus(database, transaction, partID, revisionID, ID);
			if (b != 0)
			{
				SqlCommand insertCommand2 = database.NewSqlCommand("INSERT INTO SerialNumberStatuses (snsPartID,snsPartRevisionID,snsPartWarehouseLocationID,snsPartBinID,snsSerialNumberID,snsStatus,snsQuantity,snsCreatedBy,snsCreatedDate) SELECT Top 1 sntPartID,sntPartRevisionID,sntPartWarehouseLocationID,sntPartBinID,sntSerialNumberID,sntStatus,1,@CreatedBy,@CreatedDate FROM SerialNumberTransactions WHERE sntPartID = @PartID AND sntPartRevisionID = @RevisionID AND sntSerialNumberID = @ID AND sntTransactionType <> 19 ORDER BY sntTransactionDate Desc, sntSerialNumberTransactionID Desc");
				setParametersForRefreshStatuses(ref insertCommand2, database, partID, revisionID, ID, b);
				database.ExecuteCommand(insertCommand2, transaction);
			}
		}
	}

	private void setParametersForRefreshStatuses(ref SqlCommand insertCommand, M1Database database, string partID, string revisionID, string ID, byte status)
	{
		insertCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		insertCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		insertCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = ID;
		insertCommand.Parameters.Add(new SqlParameter("@Status", SqlDbType.SmallInt)).Value = status;
		insertCommand.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.NVarChar)).Value = database.User.ID;
		insertCommand.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime)).Value = DateTime.Now;
	}

	public void DeleteStatus(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, string ID, byte status)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("DELETE FROM " + TPrefix + "NumberStatuses WHERE " + StatusPrefix + "PartID = @PartID And " + StatusPrefix + "PartRevisionID = @RevisionID And " + StatusPrefix + "PartWarehouseLocationID = @PartWarehouseLocationID And " + StatusPrefix + "PartBinID = @BinID And " + StatusPrefix + TPrefix + "NumberID = @ID And " + StatusPrefix + "Status = @Status And " + StatusPrefix + "Quantity = 0");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartWarehouseLocationID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = ID;
		sqlCommand.Parameters.Add(new SqlParameter("@Status", SqlDbType.SmallInt)).Value = status;
		database.ExecuteCommand(sqlCommand, transaction);
	}

	public void SetInactiveStatus(M1Database database, SqlTransaction transaction, string partID, string revisionID, string ID, bool inactive)
	{
		SetInactiveStatus(database, transaction, partID, revisionID, ID, inactive, DateTime.Now);
	}

	public void SetInactiveStatus(M1Database database, SqlTransaction transaction, string partID, string revisionID, string ID, bool inactive, DateTime inactiveDate)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("UPDATE " + TPrefix + "Numbers SET " + HPrefix + "Inactive = @Inactive, " + HPrefix + "InactiveDate = @InactiveDate WHERE " + HPrefix + "PartID = @PartID And " + HPrefix + "PartRevisionID = @RevisionID AND " + HPrefix + TPrefix + "NumberID = @ID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = ID;
		sqlCommand.Parameters.Add(new SqlParameter("@Inactive", SqlDbType.Bit)).Value = inactive;
		sqlCommand.Parameters.Add(new SqlParameter("@InactiveDate", SqlDbType.DateTime)).Value = inactiveDate;
		database.ExecuteCommand(sqlCommand, transaction);
	}

	public virtual DataRow AddTransactionRow(M1Database database, DataRow currentRow, DataRow rowToAdd)
	{
		DataRow dataRow = childBindingSource.NewRow(database, null, null, doSetDefaultValues: true);
		dataRow.SetField(TrPrefix + "TableName", base.Field.Table.TableName.ToUpper());
		dataRow.SetField(TrPrefix + "TableUniqueID", currentRow.Field<Guid>(base.Field.Table.UniqueField));
		dataRow.SetField(TrPrefix + "PartID", rowToAdd.Field<string>(StatusPrefix + "PartID"));
		dataRow.SetField(TrPrefix + "PartRevisionID", rowToAdd.Field<string>(StatusPrefix + "PartRevisionID"));
		if (Convert.ToByte(rowToAdd[StatusPrefix + "Status"]).Equals(0) || base.Field.TableName.Equals("WarehouseReceiptLines", StringComparison.CurrentCultureIgnoreCase) || base.Field.TableName.Equals("WarehouseReceiptComponents", StringComparison.CurrentCultureIgnoreCase))
		{
			dataRow.SetField(TrPrefix + "PartWarehouseLocationID", availableLastPartInfo[2]);
			dataRow.SetField(TrPrefix + "PartBinID", availableLastPartInfo[3]);
		}
		else
		{
			dataRow.SetField(TrPrefix + "PartWarehouseLocationID", rowToAdd.Field<string>(StatusPrefix + "PartWarehouseLocationID"));
			dataRow.SetField(TrPrefix + "PartBinID", rowToAdd.Field<string>(StatusPrefix + "PartBinID"));
		}
		dataRow.SetField(TrPrefix + TPrefix + "NumberID", rowToAdd.Field<string>(StatusPrefix + TPrefix + "NumberID"));
		dataRow.SetField(TrPrefix + "TransactionType", TransactionType);
		if (RelatedJobField.Length != 0)
		{
			childBindingSource.SkipForeignKeyChecks = true;
			FieldDefinition fieldDefinition = base.Field.BindingSource.Fields[RelatedJobField];
			if (fieldDefinition.RelatedTable.Equals("JobMaterialComponents", StringComparison.CurrentCultureIgnoreCase))
			{
				dataRow.SetField(TrPrefix + "JobID", currentRow.Field<string>(fieldDefinition.RelatedFieldsAndCurrentFieldArray[0]));
				dataRow.SetField(TrPrefix + "JobAssemblyID", currentRow.Field<int>(fieldDefinition.RelatedFieldsAndCurrentFieldArray[1]));
				dataRow.SetField(TrPrefix + "JobMaterialID", currentRow.Field<int>(fieldDefinition.RelatedFieldsAndCurrentFieldArray[2]));
				dataRow.SetField(TrPrefix + "JobMaterialComponentID", currentRow.Field<int>(fieldDefinition.FieldName));
			}
			else if (fieldDefinition.RelatedTable.Equals("JobMaterials", StringComparison.CurrentCultureIgnoreCase))
			{
				dataRow.SetField(TrPrefix + "JobID", currentRow.Field<string>(fieldDefinition.RelatedFieldsAndCurrentFieldArray[0]));
				dataRow.SetField(TrPrefix + "JobAssemblyID", currentRow.Field<int>(fieldDefinition.RelatedFieldsAndCurrentFieldArray[1]));
				dataRow.SetField(TrPrefix + "JobMaterialID", currentRow.Field<int>(fieldDefinition.FieldName));
			}
			else if (fieldDefinition.RelatedTable.Equals("JobAssemblies", StringComparison.CurrentCultureIgnoreCase))
			{
				dataRow.SetField(TrPrefix + "JobID", currentRow.Field<string>(fieldDefinition.RelatedFieldsAndCurrentFieldArray[0]));
				dataRow.SetField(TrPrefix + "JobAssemblyID", currentRow.Field<int>(fieldDefinition.FieldName));
			}
			else if (fieldDefinition.RelatedTable.Equals("Jobs", StringComparison.CurrentCultureIgnoreCase))
			{
				dataRow.SetField(TrPrefix + "JobID", currentRow.Field<string>(fieldDefinition.FieldName));
			}
			childBindingSource.SkipForeignKeyChecks = false;
		}
		DateTime? value;
		if (TransactionDateField.Length != 0)
		{
			value = currentRow.Field<DateTime?>(TransactionDateField);
		}
		else
		{
			DateTime? documentDate = base.Field.Table.GetDocumentDate(database, currentRow, null, DataRowVersion.Default);
			value = (documentDate.HasValue ? Convert.ToDateTime(documentDate) : DateTime.Now);
		}
		if (!value.HasValue)
		{
			value = DateTime.Now;
		}
		dataRow.SetField(TrPrefix + "TransactionDate", value);
		bool flag;
		byte value2;
		if (IsNegativeTransaction(currentRow.Field<decimal>(base.Field.FieldName)))
		{
			flag = true;
			value2 = Convert.ToByte(StatusNegative);
		}
		else
		{
			flag = false;
			value2 = Convert.ToByte(StatusPositive);
		}
		dataRow.SetField(TrPrefix + "NegativeTransaction", flag);
		dataRow.SetField(TrPrefix + "Status", value2);
		if (TPrefix.Equals("SERIAL", StringComparison.CurrentCultureIgnoreCase))
		{
			dataRow.SetField(TrPrefix + "Quantity", 1m);
		}
		else
		{
			dataRow.SetField(TrPrefix + "Quantity", GetQuantityForSelectedLotNumber(database, currentRow, rowToAdd.Field<decimal>(StatusPrefix + "Quantity"), flag));
		}
		dataRow.SetField(HPrefix + "ExpirationDate", rowToAdd.Field<DateTime?>(HPrefix + "ExpirationDate"));
		dataRow.SetField("OriginalStatus", rowToAdd.Field<byte>(StatusPrefix + "Status"));
		return dataRow;
	}

	public virtual bool IsNegativeTransaction(decimal qty)
	{
		return qty < 0m;
	}

	public decimal GetQuantityForSelectedLotNumber(M1Database database, DataRow row, decimal availableQty, bool negativeTrans)
	{
		decimal result = default(decimal);
		if (childBindingSource != null)
		{
			string rowFilter = $"{TrPrefix}TableUniqueID = {row.Field<Guid>(base.Field.Table.UniqueField).ToLinq()}";
			if (childBindingSource.List.GetType() == typeof(DataView))
			{
				decimal num = Convert.ToDecimal(row[base.Field.FieldName]);
				decimal num2 = default(decimal);
				decimal num3 = default(decimal);
				DataView dataView = new DataView(((DataView)childBindingSource.List).Table, rowFilter, string.Empty, DataViewRowState.CurrentRows);
				if (dataView.Count <= 0)
				{
					if (ReverseSign && !negativeTrans)
					{
						if (availableQty >= num)
						{
							return num;
						}
						return availableQty;
					}
					if (base.Field.TableName.Equals("INVENTORYCOUNTLINES", StringComparison.CurrentCultureIgnoreCase) || base.Field.TableName.Equals("QUANTITYADJUSTMENTS", StringComparison.CurrentCultureIgnoreCase))
					{
						return availableQty;
					}
					return num;
				}
				decimal num4 = default(decimal);
				foreach (DataRowView item in dataView)
				{
					num4 += Convert.ToDecimal(item.Row[TPrefix.Equals("Serial") ? "sntQuantity" : "abtQuantity"]);
				}
				num3 = num4;
				num2 = num - num3;
				if (ReverseSign && !negativeTrans)
				{
					if (availableQty >= num2)
					{
						return num2;
					}
					if (!(availableQty < 0m))
					{
						return availableQty;
					}
					result = default(decimal);
				}
				else
				{
					if (base.Field.TableName.Equals("INVENTORYCOUNTLINES", StringComparison.CurrentCultureIgnoreCase) || base.Field.TableName.Equals("QUANTITYADJUSTMENTS", StringComparison.CurrentCultureIgnoreCase))
					{
						return availableQty;
					}
					if (!(num2 < 0m))
					{
						return num2;
					}
					result = default(decimal);
				}
			}
		}
		return result;
	}

	public decimal GetTotalQuantitySelected(Type actionType, string partID, string revisionID, string warehouseID, string binID, string numberID, byte status)
	{
		TableDefinition table = base.Field.Table;
		decimal result = default(decimal);
		foreach (FieldDefinition field in table.BindingSource.Fields)
		{
			if (field.FieldExtensions == null)
			{
				continue;
			}
			foreach (FieldExtension fieldExtension in field.FieldExtensions)
			{
				if (!(fieldExtension.GetType() == actionType))
				{
					continue;
				}
				SerialOrLotNumberBase serialOrLotNumberBase = (SerialOrLotNumberBase)fieldExtension;
				if (!serialOrLotNumberBase.IsEnabled)
				{
					continue;
				}
				DataRow[] array = serialOrLotNumberBase.GetSelectedItems().Select(TrPrefix + "PartID = " + partID.ToLinq() + " And " + TrPrefix + "PartRevisionID = " + revisionID.ToLinq() + " And " + TrPrefix + "PartWarehouseLocationID = " + warehouseID.ToLinq() + " And " + TrPrefix + "PartBinID = " + binID.ToLinq() + " And " + TrPrefix + TPrefix + "NumberID = " + numberID.ToLinq() + " And " + TrPrefix + "Status = " + status.ToLinq());
				if (array == null)
				{
					continue;
				}
				DataRow[] array2 = array;
				foreach (DataRow row in array2)
				{
					if ((string.IsNullOrWhiteSpace(serialOrLotNumberBase.Field.VisibleExpression) || serialOrLotNumberBase.Field.Table.EvaluateScriptExpressionBool(serialOrLotNumberBase.Field.VisibleExpression, field.Database, field.BindingSource.CurrentAsDataRow)) && !isAlreadyPosted(serialOrLotNumberBase))
					{
						result += row.Field<decimal>(TrPrefix + "Quantity");
					}
				}
			}
		}
		return result;
	}

	public void OnRowsAdded(DataTable selectedTable, M1Database database, DataRow row)
	{
		if (database != null && row != null && availableTable != null)
		{
			CheckAvailable(database, row, forceRefresh: true);
			this.RowsAdded?.Invoke(this, new RowsAddedEventArgs(selectedTable, availableTable));
		}
	}

	protected void OnAdded(TransactionChangedEventArgs e)
	{
		this.Added?.Invoke(this, e);
	}

	protected void OnRemoved(TransactionChangedEventArgs e)
	{
		this.Removed?.Invoke(this, e);
	}

	public void OnRowChanged(RowChangedEventArgs e)
	{
		this.RowChanged?.Invoke(this, e);
	}

	public void CheckAvailable(M1Database database, DataRow row, bool forceRefresh)
	{
		if (availableTable != null && (CheckAvailableFromBase(database, row, forceRefresh) || forceRefresh))
		{
			GetAvailableAsTable(database, row);
		}
	}

	public virtual void GetAvailableAsTable(M1Database database, DataRow row)
	{
	}

	protected override void PartBin_ValueChanged(object sender, FieldDefinition.FieldValueChangedEventArgs e)
	{
		RemoveCurrentItems(e.Database, e.Row, e.SqlTransaction);
		if (e.IsCurrentRow)
		{
			CheckAvailable(e.Database, e.Row, forceRefresh: false);
		}
		base.PartBin_ValueChanged(sender, e);
	}

	protected void RemoveCurrentItems(M1Database database, DataRow currentRow, SqlTransaction transaction)
	{
		string rowFilter = $"{TrPrefix}TableUniqueID = {currentRow.Field<Guid>(base.Field.Table.UniqueField).ToLinq()}";
		if (childBindingSource.List.GetType() == typeof(DataView))
		{
			DataView dataView = new DataView(((DataView)childBindingSource.List).Table, rowFilter, string.Empty, DataViewRowState.CurrentRows);
			TransactionChangedEventArgs e = new TransactionChangedEventArgs
			{
				Database = database,
				CurrentRow = currentRow
			};
			foreach (DataRowView item in dataView)
			{
				if (IsLatestTransaction(database, item.Row))
				{
					e.TransactionRowChanged = item.Row;
					OnRemoved(e);
					childBindingSource.Remove(database, item.Row, isTopLevel: false);
					continue;
				}
				throw new M1Exception(RemoveFailMessage(item.Row.Field<string>(TrPrefix + TPrefix + "NumberID").Trim()));
			}
		}
		base.Field.Validate(database, currentRow, transaction, base.Field.BindingSource.CurrentAsDataRow == currentRow);
	}

	protected override void BindingSource_CurrentChanged(object sender, EventArgs e)
	{
		DataRow currentAsDataRow = base.Field.BindingSource.CurrentAsDataRow;
		M1Database currentDatabase = base.Field.BindingSource.CurrentDatabase;
		FilterToRow(currentDatabase, currentAsDataRow);
		base.BindingSource_CurrentChanged(sender, e);
	}

	protected void FilterToRow(M1Database database, DataRow row)
	{
		FilterToRow(row);
		OnRowChanged(new RowChangedEventArgs(database, row));
		CheckAvailable(database, row, forceRefresh: false);
	}

	public virtual void QueryDatabase(M1BindingSource.QueryDatabaseEventArgs e)
	{
		if (childBindingSource != null)
		{
			if (e.TopLevelDataRow == null)
			{
				childBindingSource.UpdateFilter("0=1");
				return;
			}
			baseQuery(e);
			FilterToRow(e.Database, e.ParentDataRow);
		}
	}

	protected virtual void baseQuery(M1BindingSource.QueryDatabaseEventArgs e)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		string queryFilter = string.Empty;
		TableDefinition parentTable = e.TopLevelTables.GetParentTable(base.Field.Table);
		if (parentTable != null)
		{
			string empty = string.Empty;
			for (int i = 0; i < parentTable.KeyFieldsArray.Length; i++)
			{
				empty = $"{parentTable.KeyFieldsArray[i]} = {base.Field.Table.KeyFieldsArray[i]}";
				if (stringBuilder2.Length == 0)
				{
					stringBuilder2.Append(empty);
				}
				else
				{
					stringBuilder2.AppendFormat(" And {0}", empty);
				}
				empty = $"{parentTable.KeyFieldsArray[i]} = {e.TopLevelDataRow[parentTable.KeyFieldsArray[i]].ToSql()}";
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(empty);
				}
				else
				{
					stringBuilder.AppendFormat(" And {0}", empty);
				}
			}
			StringBuilder stringBuilder3 = new StringBuilder();
			stringBuilder3.AppendFormat(" {0}TableName = {1} ", TrPrefix, base.Field.Table.TableName.ToSql());
			stringBuilder3.AppendFormat(" And {0}TableUniqueID In (Select {1} ", TrPrefix, base.Field.Table.UniqueField);
			stringBuilder3.AppendFormat(" From {0}", base.Field.Table.TableName);
			if (!parentTable.TableName.Equals(base.Field.Table.TableName, StringComparison.CurrentCultureIgnoreCase))
			{
				stringBuilder3.AppendFormat(" Inner Join {0}  On {1}", parentTable.TableName, stringBuilder2.ToString());
			}
			stringBuilder3.AppendFormat(" Where {0} ) And {1}TransactionType = {2}", stringBuilder.ToString(), TrPrefix, TransactionType.ToSql());
			queryFilter = stringBuilder3.ToString();
		}
		childBindingSource.NavigateTo(e, queryFilter, string.Empty);
	}

	public virtual void FilterToRow(DataRow row)
	{
		if (childBindingSource.DataSource != null)
		{
			if (row == null)
			{
				((DataView)childBindingSource.DataSource).RowFilter = "0=1";
				return;
			}
			string rowFilter = TrPrefix + "TableUniqueID = " + row.Field<Guid>(base.Field.Table.UniqueField).ToLinq();
			((DataView)childBindingSource.DataSource).RowFilter = rowFilter;
		}
	}

	public virtual string AddRowIsNotLatestTransactionMessage(DataRow row)
	{
		return TPrefix + " number " + row.Field<string>(TrPrefix + TPrefix + "NumberID").Trim() + " could not be added because it is being referenced in transactions with a later date than " + row.Field<DateTime>(TrPrefix + "TransactionDate").ToString() + " ";
	}

	public virtual string RemoveFailMessage(string id)
	{
		return TPrefix + " number " + id + " could not be removed because there have been additional transactions made against it.";
	}

	public virtual void FieldBindingSource_SaveDataStarted(object sender, SaveDataStartedEventArgs e)
	{
		childBindingSource.SaveData(e);
	}

	public virtual void FieldBindingSource_SaveDataCompleted(object sender, SaveDataCompletedEventArgs e)
	{
		childBindingSource.OnSaveDataCompleted(e);
	}

	public virtual void FieldBindingSource_CurrencyModeChanged(object sender, EventArgs e)
	{
		childBindingSource.CurrencyMode = base.Field.BindingSource.CurrencyMode;
	}

	public virtual void FieldBindingSource_CacheCleared(object sender, EventArgs e)
	{
		ClearBindingSourceCache();
	}

	public virtual void FieldBindingSource_EditCancelled(object sender, EventArgs e)
	{
		CancelBindingSourceEdit();
	}

	public override void LoadComplete(FieldCollection fields, bool add)
	{
		base.LoadComplete(fields, add);
		if (add)
		{
			base.Field.BindingSource.SaveDataCompleted += FieldBindingSource_SaveDataCompleted;
			base.Field.BindingSource.CurrencyModeChanged += FieldBindingSource_CurrencyModeChanged;
			base.Field.BindingSource.CacheCleared += FieldBindingSource_CacheCleared;
			base.Field.BindingSource.EditCancelled += FieldBindingSource_EditCancelled;
			base.Field.BindingSource.SaveDataStarted += FieldBindingSource_SaveDataStarted;
			base.Field.BindingSource.RemoveStarted += FieldBindingSource_RemoveStarted;
			base.Field.BindingSource.ListChanged += FieldBindingSource_ListChanged;
			base.Field.BindingSource.RowActivated += FieldBindingSource_RowActivated;
			childBindingSource.RowUpdateAddBefore += childBindingSource_RowUpdateAddBefore;
			childBindingSource.RowUpdateAddAfter += childBindingSource_RowUpdateAddAfter;
			childBindingSource.RowUpdateDeleteAfter += childBindingSource_RowUpdateDeleteAfter;
			childBindingSource.RowUpdateSaveAfter += childBindingSource_RowUpdateSaveAfter;
			childBindingSource.RowUpdateSaveBefore += childBindingSource_RowUpdateSaveBefore;
			base.Field.ValueChanged += Field_ValueChanged;
			if (TransactionDateField.Length != 0 && fields.Contains(TransactionDateField))
			{
				fields[TransactionDateField].ValueChanged += TransactionDateField_ValueChanged;
			}
			{
				foreach (FieldDefinition field in fields)
				{
					if (field.FieldExtensions == null)
					{
						continue;
					}
					foreach (FieldExtension fieldExtension in field.FieldExtensions)
					{
						if (fieldExtension.GetType() == GetType() && fieldExtension.PartBinField.Length != 0 && fieldExtension.TransactionType != 0 && !fieldExtension.Equals(this))
						{
							SerialOrLotNumberBase obj = (SerialOrLotNumberBase)fieldExtension;
							obj.Added += OtherTrackedField_Number_Added;
							obj.Removed += OtherTrackedField_Number_Removed;
						}
					}
				}
				return;
			}
		}
		base.Field.BindingSource.SaveDataCompleted -= FieldBindingSource_SaveDataCompleted;
		base.Field.BindingSource.CurrencyModeChanged -= FieldBindingSource_CurrencyModeChanged;
		base.Field.BindingSource.CacheCleared -= FieldBindingSource_CacheCleared;
		base.Field.BindingSource.EditCancelled -= FieldBindingSource_EditCancelled;
		base.Field.BindingSource.SaveDataStarted -= FieldBindingSource_SaveDataStarted;
		base.Field.BindingSource.RemoveStarted -= FieldBindingSource_RemoveStarted;
		base.Field.BindingSource.ListChanged -= FieldBindingSource_ListChanged;
		base.Field.BindingSource.RowActivated -= FieldBindingSource_RowActivated;
		childBindingSource.RowUpdateAddBefore -= childBindingSource_RowUpdateAddBefore;
		childBindingSource.RowUpdateAddAfter -= childBindingSource_RowUpdateAddAfter;
		childBindingSource.RowUpdateDeleteAfter -= childBindingSource_RowUpdateDeleteAfter;
		childBindingSource.RowUpdateSaveAfter -= childBindingSource_RowUpdateSaveAfter;
		childBindingSource.RowUpdateSaveBefore -= childBindingSource_RowUpdateSaveBefore;
		base.Field.BindingSource.Fields[PartBinField].ValueChanged -= PartBin_ValueChanged;
	}

	private void updateDateFieldForRow(DataRow row)
	{
		if (childBindingSource == null)
		{
			return;
		}
		string rowFilter = $"{TrPrefix}TableUniqueID = {row.Field<Guid>(base.Field.Table.UniqueField).ToLinq()}";
		DataView dataView = new DataView(((DataView)childBindingSource.List).Table, rowFilter, string.Empty, DataViewRowState.CurrentRows);
		if (dataView.Count == 0)
		{
			return;
		}
		DateTime? value = row.Field<DateTime?>(TransactionDateField);
		foreach (DataRowView item in dataView)
		{
			item.Row.SetField($"{TrPrefix}TransactionDate", value);
		}
	}

	private void Field_ValueChanged(object sender, FieldDefinition.FieldValueChangedEventArgs e)
	{
		decimal num = Convert.ToDecimal(e.PreviousValue);
		decimal num2 = e.Row.Field<decimal>(base.Field.FieldName);
		bool flag = false;
		if (num2 < 0m && num >= 0m)
		{
			flag = true;
		}
		else if (num2 >= 0m && num < 0m)
		{
			flag = true;
		}
		if (flag)
		{
			RemoveCurrentItems(e.Database, e.Row, e.SqlTransaction);
			if (e.IsCurrentRow)
			{
				CheckAvailable(e.Database, e.Row, forceRefresh: false);
			}
		}
	}

	private void TransactionDateField_ValueChanged(object sender, FieldDefinition.FieldValueChangedEventArgs e)
	{
		updateDateFieldForRow(e.Row);
	}

	private void childBindingSource_RowUpdateAddBefore(object sender, RowUpdateEventArgs e)
	{
		if (!Parameters.ToUpper().Contains("INCLUDEUNASSIGNED") && ReverseSign && e.Row.Field<byte>("OriginalStatus") != 0 && !base.Field.TableName.Equals("WAREHOUSERECEIPTLINES", StringComparison.CurrentCultureIgnoreCase) && !base.Field.TableName.Equals("WAREHOUSERECEIPTCOMPONENTS", StringComparison.CurrentCultureIgnoreCase) && !base.Field.TableName.Equals("INSPECTIONLINES", StringComparison.CurrentCultureIgnoreCase))
		{
			decimal num = default(decimal);
			byte b = 0;
			b = ((!curType.Equals('L')) ? GetLatestStatus(e.Database, e.SqlTransaction, e.Row.Field<string>(TrPrefix + "PartID"), e.Row.Field<string>(TrPrefix + "PartRevisionID"), e.Row.Field<string>(TrPrefix + TPrefix + "NumberID")) : e.Row.Field<byte>("OriginalStatus"));
			decimal totalQuantitySelected = GetTotalQuantitySelected(GetType(), e.Row.Field<string>(TrPrefix + "PartID"), e.Row.Field<string>(TrPrefix + "PartRevisionID"), e.Row.Field<string>(TrPrefix + "PartWarehouseLocationID"), e.Row.Field<string>(TrPrefix + "PartBinID"), e.Row.Field<string>(TrPrefix + TPrefix + "NumberID"), b);
			num = GetStatusQuantity(e.Database, e.SqlTransaction, e.Row.Field<string>(TrPrefix + "PartID"), e.Row.Field<string>(TrPrefix + "PartRevisionID"), e.Row.Field<string>(TrPrefix + "PartWarehouseLocationID"), e.Row.Field<string>(TrPrefix + "PartBinID"), e.Row.Field<string>(TrPrefix + TPrefix + "NumberID"), b);
			if (totalQuantitySelected > num)
			{
				MessageBox.Show("The total quantity entered (" + Convert.ToDecimal(totalQuantitySelected) + ") for " + TPrefix + " number " + e.Row.Field<string>(TrPrefix + TPrefix + "NumberID") + " is greater than the quantity available (" + Convert.ToDecimal(num) + ")", "Confirm Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				e.Cancel = true;
			}
		}
	}

	private void childBindingSource_RowUpdateSaveBefore(object sender, RowUpdateEventArgs e)
	{
	}

	private void childBindingSource_RowUpdateSaveAfter(object sender, RowUpdateEventArgs e)
	{
		UpdateHeaderTable(e.Database, e.Row, e.SqlTransaction);
	}

	private void childBindingSource_RowUpdateDeleteAfter(object sender, RowUpdateEventArgs e)
	{
		UpdateHeaderTable(e.Database, e.Row, e.SqlTransaction);
	}

	private void childBindingSource_RowUpdateAddAfter(object sender, RowUpdateEventArgs e)
	{
		UpdateHeaderTable(e.Database, e.Row, e.SqlTransaction);
	}

	private void FieldBindingSource_ListChanged(object sender, ListChangedEventArgs e)
	{
		FilterToRow(base.Field.BindingSource.CurrentDatabase, base.Field.BindingSource.CurrentAsDataRow);
	}

	private void OtherTrackedField_Number_Removed(object sender, TransactionChangedEventArgs e)
	{
		AddToAvailableList(new string[0], e.CurrentRow, e.TransactionRowChanged);
	}

	private void OtherTrackedField_Number_Added(object sender, TransactionChangedEventArgs e)
	{
		RemoveFromAvailableList(e.CurrentRow, e.TransactionRowChanged);
	}

	private void FieldBindingSource_RemoveStarted(object sender, RemoveEventArgs e)
	{
		RemoveCurrentItems(e.Database, e.Row, e.SqlTransaction);
	}

	private void FieldBindingSource_RowActivated(object sender, M1BindingSource.QueryDatabaseEventArgs e)
	{
		QueryDatabase(e);
	}

	public virtual void MergeAvailable(string[] relatedFields, DataRow currentRow, List<DataTable> deletedItems, List<DataTable> insertedItems)
	{
		bool flag = false;
		if (currentRow.Field<decimal>(base.Field.FieldName) < 0m)
		{
			flag = true;
		}
		foreach (DataTable deletedItem in deletedItems)
		{
			if (deletedItem == null)
			{
				continue;
			}
			foreach (DataRow row in deletedItem.Rows)
			{
				if ((!row.Field<bool>($"{TrPrefix}NegativeTransaction", DataRowVersion.Original) && !flag) || (row.Field<bool>($"{TrPrefix}NegativeTransaction", DataRowVersion.Original) && flag))
				{
					DataRow dataRow2 = FindFromAvailableList(currentRow, row);
					string numberID = row.Field<string>($"{TrPrefix}{TPrefix}NumberID", DataRowVersion.Original).Trim();
					if (dataRow2 == null && !IsSelected(currentRow, numberID))
					{
						AddToAvailableList(relatedFields, currentRow, row);
					}
				}
				else
				{
					RemoveFromAvailableList(currentRow, row);
				}
			}
		}
		foreach (DataTable insertedItem in insertedItems)
		{
			if (insertedItem == null)
			{
				continue;
			}
			foreach (DataRow row2 in insertedItem.Rows)
			{
				RemoveFromAvailableList(currentRow, row2);
				if (curType.Equals('L'))
				{
					string numberID2 = row2.Field<string>($"{TrPrefix}{TPrefix}NumberID", DataRowVersion.Current).Trim();
					if (!IsSelected(currentRow, numberID2) && ((!row2.Field<bool>($"{TrPrefix}NegativeTransaction", DataRowVersion.Current) && !flag) || (row2.Field<bool>($"{TrPrefix}NegativeTransaction", DataRowVersion.Current) && flag)) && FindFromAvailableList(currentRow, row2) == null)
					{
						AddToAvailableList(relatedFields, currentRow, row2);
					}
				}
			}
		}
	}

	public void RefreshQuantityAvailable()
	{
		foreach (DataRow row in availableTable.Rows)
		{
			string partID = row.Field<string>(StatusPrefix + "PartID").Trim();
			string revisionID = row.Field<string>(StatusPrefix + "PartRevisionID").Trim();
			string warehouseID = row.Field<string>(StatusPrefix + "PartWarehouseLocationID").Trim();
			string binID = row.Field<string>(StatusPrefix + "PartBinID").Trim();
			string numberID = row.Field<string>(StatusPrefix + TPrefix + "NumberID").Trim();
			byte status = row.Field<byte>(StatusPrefix + "Status");
			decimal statusQuantity = GetStatusQuantity(childBindingSource.Database, null, partID, revisionID, warehouseID, binID, numberID, status);
			decimal totalQuantitySelected = GetTotalQuantitySelected(GetType(), partID, revisionID, warehouseID, binID, numberID, status);
			decimal num = statusQuantity - totalQuantitySelected;
			if (curType.Equals('L'))
			{
				if (num < 0m)
				{
					row.SetField($"{StatusPrefix}Quantity", statusQuantity);
				}
				else
				{
					row.SetField(StatusPrefix + "Quantity", num);
				}
			}
			else if (num != row.Field<decimal>(StatusPrefix + "Quantity"))
			{
				row.SetField(StatusPrefix + "Quantity", num);
			}
		}
	}

	public virtual void RemoveFromAvailableList(DataRow currentRow, DataRow lotNumberRow)
	{
		if (availableTable != null)
		{
			DataRow dataRow = FindFromAvailableList(currentRow, lotNumberRow);
			if (dataRow != null)
			{
				availableTable.Rows.Remove(dataRow);
			}
		}
	}
}
