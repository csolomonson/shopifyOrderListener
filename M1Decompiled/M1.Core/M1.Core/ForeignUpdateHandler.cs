using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Extensions;

namespace M1.Core;

public class ForeignUpdateHandler : IDisposable
{
	protected class UpdateBinding
	{
		public string SourceField;

		public string DestinationField;

		public bool ReverseSign;

		public string EnabledExpression;

		public UpdateBinding(string sourceField, string destField, bool reverseSign, string enabledExpression)
		{
			SourceField = sourceField;
			DestinationField = destField;
			ReverseSign = reverseSign;
			EnabledExpression = enabledExpression;
		}
	}

	private FieldDefinition foreignLinkField;

	private M1BindingSource foreignBs;

	private DataRow originalRow;

	private bool? hasExpressions;

	protected List<UpdateBinding> FieldBindings = new List<UpdateBinding>();

	public void AttachFieldBinding(string sourceField, string destField, bool reverseSign, string enabledExpression)
	{
		foreach (UpdateBinding fieldBinding in FieldBindings)
		{
			if (fieldBinding.SourceField.Equals(sourceField, StringComparison.CurrentCultureIgnoreCase) && fieldBinding.DestinationField.Equals(destField, StringComparison.CurrentCultureIgnoreCase))
			{
				return;
			}
		}
		FieldBindings.Add(new UpdateBinding(sourceField, destField, reverseSign, enabledExpression));
	}

	public void Load(FieldDefinition field, bool allowEditing)
	{
		foreignLinkField = field;
		if (allowEditing && !string.IsNullOrWhiteSpace(foreignLinkField.RelatedTable))
		{
			foreignLinkField.BindingSource.ChangedRowsInit += BindingSource_ChangedRowsInit;
		}
	}

	private DataRow getDataRow(DataRow sourceRow, DataRowVersion rowVersion)
	{
		DataTable dataTable = foreignBs.GetDataTable();
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < foreignLinkField.RelatedFieldsAndCurrentFieldArray.Length; i++)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(" And ");
			}
			stringBuilder.Append(foreignLinkField.RelatedTableKeyFieldsArray[i] + " = " + M1Util.ConvertToLinq(sourceRow[foreignLinkField.RelatedFieldsAndCurrentFieldArray[i], rowVersion]));
		}
		DataRow[] array = dataTable.Select(stringBuilder.ToString());
		if (array.Length != 0)
		{
			return array[0];
		}
		return null;
	}

	private bool isEnabled(M1Database database, DataRow row, UpdateBinding binding)
	{
		if (!string.IsNullOrWhiteSpace(binding.EnabledExpression) && !foreignLinkField.Table.EvaluateScriptExpressionBool(binding.EnabledExpression, database, row))
		{
			return false;
		}
		return true;
	}

	private bool isExpression()
	{
		foreach (UpdateBinding fieldBinding in FieldBindings)
		{
			if (!string.IsNullOrWhiteSpace(fieldBinding.EnabledExpression))
			{
				return true;
			}
		}
		return false;
	}

	private void changeQuantityInForeignRow(M1Database database, DataRow sourceRow, DataRowVersion rowVersion, bool add)
	{
		if (!isRowChanged(sourceRow) || M1Util.IsNullOrEmpty(sourceRow[foreignLinkField.RelatedFieldsAndCurrentFieldArray[0], rowVersion]))
		{
			return;
		}
		DataRow dataRow = getDataRow(sourceRow, rowVersion);
		if (dataRow == null)
		{
			return;
		}
		if (!hasExpressions.HasValue)
		{
			hasExpressions = isExpression();
		}
		if (hasExpressions.Value)
		{
			if (originalRow == null)
			{
				DataTable dataTable = sourceRow.Table.Clone();
				originalRow = dataTable.NewRow();
			}
			if (sourceRow.RowState == DataRowState.Added)
			{
				originalRow.BlankRow();
			}
			else
			{
				foreach (DataColumn column in sourceRow.Table.Columns)
				{
					originalRow[column.ColumnName] = sourceRow[column, DataRowVersion.Original];
				}
			}
		}
		foreach (UpdateBinding fieldBinding in FieldBindings)
		{
			if (!isEnabled(database, (rowVersion == DataRowVersion.Original) ? originalRow : sourceRow, fieldBinding) || (!isLinkFieldChanged(sourceRow) && sourceRow[fieldBinding.SourceField, DataRowVersion.Original].Equals(sourceRow[fieldBinding.SourceField, DataRowVersion.Current]) && isEnabled(database, originalRow, fieldBinding) == isEnabled(database, sourceRow, fieldBinding)))
			{
				continue;
			}
			if (sourceRow.Table.Columns[fieldBinding.SourceField].DataType.Equals(typeof(decimal)))
			{
				decimal num = ((!fieldBinding.ReverseSign) ? sourceRow.Field<decimal>(fieldBinding.SourceField, rowVersion) : (-sourceRow.Field<decimal>(fieldBinding.SourceField, rowVersion)));
				if (num != 0m)
				{
					if (add)
					{
						foreignBs.SetPositionByDataRow(dataRow);
						dataRow.SetField(fieldBinding.DestinationField, dataRow.Field<decimal>(fieldBinding.DestinationField) + num);
					}
					else
					{
						foreignBs.SetPositionByDataRow(dataRow);
						dataRow.SetField(fieldBinding.DestinationField, dataRow.Field<decimal>(fieldBinding.DestinationField) - num);
					}
				}
			}
			else if (sourceRow.Table.Columns[fieldBinding.SourceField].DataType.Equals(typeof(bool)))
			{
				if (add)
				{
					foreignBs.SetPositionByDataRow(dataRow);
					dataRow.SetField(fieldBinding.DestinationField, sourceRow.Field<bool>(fieldBinding.SourceField, rowVersion));
				}
				else if (sourceRow.Field<bool>(fieldBinding.SourceField, rowVersion))
				{
					foreignBs.SetPositionByDataRow(dataRow);
					dataRow.SetField(fieldBinding.DestinationField, value: false);
				}
			}
		}
	}

	private void BindingSource_ChangedRowsInit(object sender, SaveDataStartedEventArgs e)
	{
		loadData(e.SqlTransaction);
		SqlTransaction transaction = foreignBs.Transaction;
		try
		{
			foreignBs.Transaction = e.SqlTransaction;
			if (foreignLinkField.BindingSource.ChangedRows.DeletedRows.Count != 0)
			{
				foreach (DataRow deletedRow in foreignLinkField.BindingSource.ChangedRows.DeletedRows)
				{
					changeQuantityInForeignRow(e.Database, deletedRow, DataRowVersion.Original, add: false);
				}
			}
			if (foreignLinkField.BindingSource.ChangedRows.ChangedRows.Count != 0)
			{
				foreach (DataRow changedRow in foreignLinkField.BindingSource.ChangedRows.ChangedRows)
				{
					changeQuantityInForeignRow(e.Database, changedRow, DataRowVersion.Original, add: false);
					changeQuantityInForeignRow(e.Database, changedRow, DataRowVersion.Current, add: true);
				}
			}
			if (foreignLinkField.BindingSource.ChangedRows.AddedRows.Count != 0)
			{
				foreach (DataRow addedRow in foreignLinkField.BindingSource.ChangedRows.AddedRows)
				{
					changeQuantityInForeignRow(e.Database, addedRow, DataRowVersion.Current, add: true);
				}
			}
			foreignBs.SaveData(e);
			foreignBs.ClearCache();
		}
		finally
		{
			foreignBs.Transaction = transaction;
		}
	}

	private void loadData(SqlTransaction transaction)
	{
		if (foreignBs == null)
		{
			foreignBs = new M1BindingSource(foreignLinkField.Database);
			foreignBs.Transaction = transaction;
			foreignBs.DataSourceTable = foreignLinkField.RelatedTable;
			foreignBs.Transaction = null;
		}
		foreignBs.ClearCache();
		List<string> list = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DataRow row in foreignLinkField.BindingSource.GetDataTable().Rows)
		{
			if (!isRowChanged(row))
			{
				continue;
			}
			if (row.RowState != DataRowState.Deleted)
			{
				stringBuilder.Length = 0;
				for (int i = 0; i < foreignLinkField.RelatedFieldsAndCurrentFieldArray.Length; i++)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(" And ");
					}
					stringBuilder.Append(foreignLinkField.RelatedTableKeyFieldsArray[i] + " = " + M1Util.ConvertToSql(row[foreignLinkField.RelatedFieldsAndCurrentFieldArray[i]]));
				}
				if (!list.Contains(stringBuilder.ToString(), StringComparer.CurrentCultureIgnoreCase))
				{
					list.Add(stringBuilder.ToString());
				}
			}
			if (row.RowState == DataRowState.Added)
			{
				continue;
			}
			stringBuilder.Length = 0;
			for (int j = 0; j < foreignLinkField.RelatedFieldsAndCurrentFieldArray.Length; j++)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(" And ");
				}
				stringBuilder.Append(foreignLinkField.RelatedTableKeyFieldsArray[j] + " = " + M1Util.ConvertToSql(row[foreignLinkField.RelatedFieldsAndCurrentFieldArray[j], DataRowVersion.Original]));
			}
			if (!list.Contains(stringBuilder.ToString(), StringComparer.CurrentCultureIgnoreCase))
			{
				list.Add(stringBuilder.ToString());
			}
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		foreach (string item in list)
		{
			if (stringBuilder2.Length != 0)
			{
				stringBuilder2.Append(" Or ");
			}
			stringBuilder2.Append("(" + item + ")");
		}
		if (stringBuilder2.Length != 0)
		{
			foreignBs.NavigateTo(new M1BindingSource.QueryDatabaseEventArgs(foreignLinkField.BindingSource.Database, foreignLinkField.BindingSource.Transaction), stringBuilder2.ToString(), string.Empty);
		}
	}

	private bool isLinkFieldChanged(DataRow row)
	{
		if (row.RowState == DataRowState.Added)
		{
			string[] relatedFieldsAndCurrentFieldArray = foreignLinkField.RelatedFieldsAndCurrentFieldArray;
			foreach (string columnName in relatedFieldsAndCurrentFieldArray)
			{
				if (!M1Util.IsNullOrEmpty(row[columnName]))
				{
					return true;
				}
			}
		}
		else if (row.RowState == DataRowState.Deleted)
		{
			string[] relatedFieldsAndCurrentFieldArray = foreignLinkField.RelatedFieldsAndCurrentFieldArray;
			foreach (string columnName2 in relatedFieldsAndCurrentFieldArray)
			{
				if (!M1Util.IsNullOrEmpty(row[columnName2, DataRowVersion.Original]))
				{
					return true;
				}
			}
		}
		else
		{
			string[] relatedFieldsAndCurrentFieldArray = foreignLinkField.RelatedFieldsAndCurrentFieldArray;
			foreach (string columnName3 in relatedFieldsAndCurrentFieldArray)
			{
				if (!row[columnName3].Equals(row[columnName3, DataRowVersion.Original]))
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool isRowChanged(DataRow row)
	{
		if (isLinkFieldChanged(row))
		{
			return true;
		}
		if (row.RowState == DataRowState.Added)
		{
			foreach (UpdateBinding fieldBinding in FieldBindings)
			{
				if (!M1Util.IsNullOrEmpty(row[fieldBinding.SourceField]) || !string.IsNullOrWhiteSpace(fieldBinding.EnabledExpression))
				{
					return true;
				}
			}
		}
		else if (row.RowState == DataRowState.Deleted)
		{
			foreach (UpdateBinding fieldBinding2 in FieldBindings)
			{
				if (!M1Util.IsNullOrEmpty(row[fieldBinding2.SourceField, DataRowVersion.Original]) || !string.IsNullOrWhiteSpace(fieldBinding2.EnabledExpression))
				{
					return true;
				}
			}
		}
		else
		{
			foreach (UpdateBinding fieldBinding3 in FieldBindings)
			{
				if (!row[fieldBinding3.SourceField].Equals(row[fieldBinding3.SourceField, DataRowVersion.Original]) || !string.IsNullOrWhiteSpace(fieldBinding3.EnabledExpression))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void Dispose()
	{
		if (foreignLinkField != null)
		{
			if (foreignLinkField.BindingSource != null)
			{
				foreignLinkField.BindingSource.ChangedRowsInit -= BindingSource_ChangedRowsInit;
			}
			foreignLinkField = null;
		}
		if (foreignBs != null)
		{
			foreignBs.Dispose();
			foreignBs = null;
		}
		originalRow = null;
	}
}
