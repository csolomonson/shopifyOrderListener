using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using M1.Extensions;

namespace M1.Core;

public class ForeignUpdateHandler2 : IDisposable
{
	protected class ForeignUpdateRowChange
	{
		public object[] Keys;

		public Dictionary<string, decimal> ChangeQuantities = new Dictionary<string, decimal>(StringComparer.CurrentCultureIgnoreCase);

		public Dictionary<string, bool> ChangeBools = new Dictionary<string, bool>(StringComparer.CurrentCultureIgnoreCase);
	}

	protected class UpdateBinding
	{
		public string SourceField;

		public string DestinationField;

		public bool ReverseSign;

		public UpdateBinding(string sourceField, string destField, bool reverseSign)
		{
			SourceField = sourceField;
			DestinationField = destField;
			ReverseSign = reverseSign;
		}
	}

	private FieldDefinition foreignLinkField;

	private M1BindingSource foreignBs;

	protected List<UpdateBinding> FieldBindings = new List<UpdateBinding>();

	private Dictionary<string, ForeignUpdateRowChange> RowChanges;

	public void AttachFieldBinding(string sourceField, string destField, bool reverseSign)
	{
		foreach (UpdateBinding fieldBinding in FieldBindings)
		{
			if (fieldBinding.SourceField.Equals(sourceField, StringComparison.CurrentCultureIgnoreCase) && fieldBinding.DestinationField.Equals(destField, StringComparison.CurrentCultureIgnoreCase))
			{
				return;
			}
		}
		FieldBindings.Add(new UpdateBinding(sourceField, destField, reverseSign));
	}

	public void Load(FieldDefinition field, bool allowEditing)
	{
		foreignLinkField = field;
		if (allowEditing && !string.IsNullOrWhiteSpace(foreignLinkField.RelatedTable))
		{
			foreignLinkField.BindingSource.ChangedRowsInit += BindingSource_ChangedRowsInit;
			foreignLinkField.BindingSource.SaveDataStarted += BindingSource_SaveDataStarted;
		}
	}

	private void BindingSource_ChangedRowsInit(object sender, EventArgs e)
	{
		RowChanges = new Dictionary<string, ForeignUpdateRowChange>(StringComparer.CurrentCultureIgnoreCase);
		if (foreignLinkField.BindingSource.ChangedRows.DeletedRows.Count != 0)
		{
			foreach (DataRow deletedRow in foreignLinkField.BindingSource.ChangedRows.DeletedRows)
			{
				changeQuantityInForeignRow(deletedRow, DataRowVersion.Original, add: false);
			}
		}
		if (foreignLinkField.BindingSource.ChangedRows.ChangedRows.Count != 0)
		{
			foreach (DataRow changedRow in foreignLinkField.BindingSource.ChangedRows.ChangedRows)
			{
				changeQuantityInForeignRow(changedRow, DataRowVersion.Original, add: false);
				changeQuantityInForeignRow(changedRow, DataRowVersion.Current, add: true);
			}
		}
		if (foreignLinkField.BindingSource.ChangedRows.AddedRows.Count == 0)
		{
			return;
		}
		foreach (DataRow addedRow in foreignLinkField.BindingSource.ChangedRows.AddedRows)
		{
			changeQuantityInForeignRow(addedRow, DataRowVersion.Current, add: true);
		}
	}

	private DataRow getDataRow(ForeignUpdateRowChange change)
	{
		DataTable dataTable = foreignBs.GetDataTable();
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < foreignLinkField.RelatedFieldsAndCurrentFieldArray.Length; i++)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(" And ");
			}
			stringBuilder.Append(foreignLinkField.RelatedTableKeyFieldsArray[i] + " = " + M1Util.ConvertToLinq(change.Keys[i]));
		}
		DataRow[] array = dataTable.Select(stringBuilder.ToString());
		if (array.Length != 0)
		{
			return array[0];
		}
		return null;
	}

	private string getKeyString(object[] keys)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (object obj in keys)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append("|");
			}
			stringBuilder.Append(obj.ToString());
		}
		return stringBuilder.ToString();
	}

	private object[] getRowKey(DataRow row)
	{
		object[] array = new object[foreignLinkField.RelatedFieldsAndCurrentFieldArray.Length];
		for (int i = 0; i < foreignLinkField.RelatedFieldsAndCurrentFieldArray.Length; i++)
		{
			array[i] = row[foreignLinkField.RelatedFieldsAndCurrentFieldArray[i]];
		}
		return array;
	}

	private ForeignUpdateRowChange getChangesInstanceForRow(DataRow row)
	{
		object[] rowKey = getRowKey(row);
		string keyString = getKeyString(rowKey);
		if (RowChanges.ContainsKey(keyString))
		{
			return RowChanges[keyString];
		}
		ForeignUpdateRowChange foreignUpdateRowChange = new ForeignUpdateRowChange();
		foreignUpdateRowChange.Keys = rowKey;
		RowChanges.Add(keyString, foreignUpdateRowChange);
		return foreignUpdateRowChange;
	}

	private void processChanges()
	{
		foreach (KeyValuePair<string, ForeignUpdateRowChange> rowChange in RowChanges)
		{
			DataRow dataRow = getDataRow(rowChange.Value);
			if (dataRow == null)
			{
				continue;
			}
			foreach (KeyValuePair<string, decimal> changeQuantity in rowChange.Value.ChangeQuantities)
			{
				dataRow.SetField(changeQuantity.Key, dataRow.Field<decimal>(changeQuantity.Key) + changeQuantity.Value);
			}
			foreach (KeyValuePair<string, bool> changeBool in rowChange.Value.ChangeBools)
			{
				dataRow.SetField(changeBool.Key, changeBool.Value);
			}
		}
	}

	private void changeQuantityInForeignRow(DataRow sourceRow, DataRowVersion rowVersion, bool add)
	{
		if (!isRowChanged(sourceRow) || M1Util.IsNullOrEmpty(sourceRow[foreignLinkField.RelatedFieldsAndCurrentFieldArray[0], rowVersion]))
		{
			return;
		}
		foreach (UpdateBinding fieldBinding in FieldBindings)
		{
			if (!isLinkFieldChanged(sourceRow) && sourceRow[fieldBinding.SourceField, DataRowVersion.Original].Equals(sourceRow[fieldBinding.SourceField, DataRowVersion.Current]))
			{
				continue;
			}
			if (sourceRow.Table.Columns[fieldBinding.SourceField].DataType.Equals(typeof(decimal)))
			{
				decimal num = ((!fieldBinding.ReverseSign) ? sourceRow.Field<decimal>(fieldBinding.SourceField, rowVersion) : (-sourceRow.Field<decimal>(fieldBinding.SourceField, rowVersion)));
				if (num != 0m)
				{
					if (!add)
					{
						num = -num;
					}
					ForeignUpdateRowChange changesInstanceForRow = getChangesInstanceForRow(sourceRow);
					if (changesInstanceForRow.ChangeQuantities.ContainsKey(fieldBinding.DestinationField))
					{
						changesInstanceForRow.ChangeQuantities[fieldBinding.DestinationField] += num;
					}
					else
					{
						changesInstanceForRow.ChangeQuantities.Add(fieldBinding.DestinationField, num);
					}
				}
			}
			else
			{
				if (!sourceRow.Table.Columns[fieldBinding.SourceField].DataType.Equals(typeof(bool)))
				{
					continue;
				}
				if (add)
				{
					ForeignUpdateRowChange changesInstanceForRow = getChangesInstanceForRow(sourceRow);
					if (changesInstanceForRow.ChangeQuantities.ContainsKey(fieldBinding.DestinationField))
					{
						changesInstanceForRow.ChangeBools[fieldBinding.DestinationField] = sourceRow.Field<bool>(fieldBinding.SourceField, rowVersion);
					}
					else
					{
						changesInstanceForRow.ChangeBools.Add(fieldBinding.DestinationField, sourceRow.Field<bool>(fieldBinding.SourceField, rowVersion));
					}
				}
				else if (sourceRow.Field<bool>(fieldBinding.SourceField, rowVersion))
				{
					ForeignUpdateRowChange changesInstanceForRow = getChangesInstanceForRow(sourceRow);
					if (changesInstanceForRow.ChangeQuantities.ContainsKey(fieldBinding.DestinationField))
					{
						changesInstanceForRow.ChangeBools[fieldBinding.DestinationField] = false;
					}
					else
					{
						changesInstanceForRow.ChangeBools.Add(fieldBinding.DestinationField, value: false);
					}
				}
			}
		}
	}

	private void BindingSource_SaveDataStarted(object sender, SaveDataStartedEventArgs e)
	{
		if (foreignBs != null)
		{
			loadData();
			processChanges();
			foreignBs.SaveData(e);
			foreignBs.ClearCache();
		}
	}

	private void loadData()
	{
		if (foreignBs == null)
		{
			foreignBs = new M1BindingSource(foreignLinkField.Database);
			foreignBs.DataSourceTable = foreignLinkField.RelatedTable;
		}
		foreignBs.ClearCache();
		new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		foreach (KeyValuePair<string, ForeignUpdateRowChange> rowChange in RowChanges)
		{
			stringBuilder.Length = 0;
			for (int i = 0; i < foreignLinkField.RelatedFieldsAndCurrentFieldArray.Length; i++)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(" And ");
				}
				stringBuilder.Append(foreignLinkField.RelatedTableKeyFieldsArray[i] + " = " + M1Util.ConvertToSql(rowChange.Value.Keys[i]));
			}
			if (stringBuilder2.Length != 0)
			{
				stringBuilder2.Append(" Or ");
			}
			stringBuilder2.Append("(" + stringBuilder.ToString() + ")");
		}
		foreignBs.NavigateTo(new M1BindingSource.QueryDatabaseEventArgs(foreignLinkField.BindingSource.Database, foreignLinkField.BindingSource.Transaction), stringBuilder2.ToString(), string.Empty);
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
				if (!M1Util.IsNullOrEmpty(row[fieldBinding.SourceField]))
				{
					return true;
				}
			}
		}
		else
		{
			foreach (UpdateBinding fieldBinding2 in FieldBindings)
			{
				if (!row[fieldBinding2.SourceField].Equals(row[fieldBinding2.SourceField, DataRowVersion.Original]))
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
				foreignLinkField.BindingSource.SaveDataStarted -= BindingSource_SaveDataStarted;
			}
			foreignLinkField.Dispose();
			foreignLinkField = null;
		}
		if (foreignBs != null)
		{
			foreignBs = null;
			foreignBs.Dispose();
		}
	}
}
