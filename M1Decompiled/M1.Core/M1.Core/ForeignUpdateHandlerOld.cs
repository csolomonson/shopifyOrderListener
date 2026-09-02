using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using M1.Extensions;

namespace M1.Core;

public class ForeignUpdateHandlerOld : IDisposable
{
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
			foreignLinkField.BindingSource.RowUpdateAddBefore += BindingSource_RowUpdateAddBefore;
			foreignLinkField.BindingSource.RowUpdateSaveBefore += BindingSource_RowUpdateSaveBefore;
			foreignLinkField.BindingSource.RowUpdateDeleteBefore += BindingSource_RowUpdateDeleteBefore;
			foreignLinkField.BindingSource.SaveDataCompleted += BindingSource_SaveDataCompleted;
		}
	}

	private void BindingSource_SaveDataCompleted(object sender, SaveDataCompletedEventArgs e)
	{
		if (foreignBs != null)
		{
			foreignBs.OnSaveDataCompleted(e);
			foreignBs.ClearCache();
		}
	}

	private void BindingSource_ChangedRowsInit(object sender, EventArgs e)
	{
		loadData();
	}

	private void BindingSource_RowUpdateDeleteBefore(object sender, RowUpdateEventArgs e)
	{
		changeQuantityInForeignRow(e.Row, DataRowVersion.Original, add: false);
	}

	private void BindingSource_RowUpdateSaveBefore(object sender, RowUpdateEventArgs e)
	{
		changeQuantityInForeignRow(e.Row, DataRowVersion.Original, add: false);
		changeQuantityInForeignRow(e.Row, DataRowVersion.Current, add: true);
	}

	private void BindingSource_RowUpdateAddBefore(object sender, RowUpdateEventArgs e)
	{
		changeQuantityInForeignRow(e.Row, DataRowVersion.Current, add: true);
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

	private void changeQuantityInForeignRow(DataRow sourceRow, DataRowVersion rowVersion, bool add)
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
					if (add)
					{
						dataRow.SetField(fieldBinding.DestinationField, dataRow.Field<decimal>(fieldBinding.DestinationField) + num);
					}
					else
					{
						dataRow.SetField(fieldBinding.DestinationField, dataRow.Field<decimal>(fieldBinding.DestinationField) - num);
					}
				}
			}
			else if (sourceRow.Table.Columns[fieldBinding.SourceField].DataType.Equals(typeof(bool)))
			{
				if (add)
				{
					dataRow.SetField(fieldBinding.DestinationField, sourceRow.Field<bool>(fieldBinding.SourceField, rowVersion));
				}
				else if (sourceRow.Field<bool>(fieldBinding.SourceField, rowVersion))
				{
					dataRow.SetField(fieldBinding.DestinationField, value: false);
				}
			}
		}
	}

	private void BindingSource_SaveDataStarted(object sender, SaveDataStartedEventArgs e)
	{
		if (foreignBs != null)
		{
			foreignBs.SaveData(e);
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
				foreignLinkField.BindingSource.RowUpdateAddAfter -= BindingSource_RowUpdateAddBefore;
				foreignLinkField.BindingSource.RowUpdateSaveAfter -= BindingSource_RowUpdateSaveBefore;
				foreignLinkField.BindingSource.RowUpdateDeleteBefore -= BindingSource_RowUpdateDeleteBefore;
				foreignLinkField.BindingSource.SaveDataCompleted -= BindingSource_SaveDataCompleted;
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
