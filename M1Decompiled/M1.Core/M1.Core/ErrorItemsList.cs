using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace M1.Core;

public class ErrorItemsList : BindingList<ValidationInfo>
{
	public ValidationInfo GetRowFieldErrorList(DataRow row, FieldDefinition field)
	{
		using (IEnumerator<ValidationInfo> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ValidationInfo current = enumerator.Current;
				if (current.Row == row && current.Field == field)
				{
					return current;
				}
			}
		}
		return null;
	}

	public void SetRowFieldErrorList(DataRow row, FieldDefinition field, ValidationInfo errorList)
	{
		SetRowFieldErrorList(row, field, errorList, clearExisting: true);
	}

	public void SetRowFieldErrorList(DataRow row, FieldDefinition field, ValidationInfo errorList, bool clearExisting)
	{
		ValidationInfo rowFieldErrorList = GetRowFieldErrorList(row, field);
		if (rowFieldErrorList == null)
		{
			if (errorList != null && errorList.Errors.Count != 0)
			{
				Add(new ValidationInfo(errorList));
			}
			else
			{
				RemoveDetachedRows(field);
			}
		}
		else if (errorList == null || errorList.Errors.Count == 0)
		{
			Remove(rowFieldErrorList);
			if (base.Count == 1 && base[0].Field == field)
			{
				DataRow row2 = base[0].Row;
				if (row2 != null && row2.RowState == DataRowState.Detached)
				{
					Remove(base[0]);
				}
			}
		}
		else
		{
			int newIndex = IndexOf(rowFieldErrorList);
			rowFieldErrorList.Copy(errorList, clearExisting);
			OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, newIndex));
		}
	}

	public void RemoveAllForSource(object source)
	{
		for (int num = base.Count - 1; num >= 0; num--)
		{
			ValidationInfo validationInfo = base[num];
			if (source is DataRow)
			{
				if (validationInfo.Row == source)
				{
					if (validationInfo.Errors != null)
					{
						for (int num2 = validationInfo.Errors.Count - 1; num2 >= 0; num2--)
						{
							validationInfo.Errors.RemoveAt(num2);
						}
					}
					RemoveAt(num);
				}
			}
			else if (validationInfo.Source == source)
			{
				if (validationInfo.Errors != null)
				{
					for (int num3 = validationInfo.Errors.Count - 1; num3 >= 0; num3--)
					{
						validationInfo.Errors.RemoveAt(num3);
					}
				}
				RemoveAt(num);
			}
		}
	}

	public void RemoveAllMemosForSource()
	{
		for (int num = base.Count - 1; num >= 0; num--)
		{
			ValidationInfo validationInfo = base[num];
			if (validationInfo.Errors != null && validationInfo.IsMemo)
			{
				RemoveAt(num);
			}
		}
	}

	private void RemoveDetachedRows(FieldDefinition field)
	{
		for (int i = 0; i < base.Count; i++)
		{
			DataRow row = base[i].Row;
			if (row != null && row.RowState == DataRowState.Detached && base[i].Field == field)
			{
				Remove(base[i]);
				RemoveDetachedRows(field);
				break;
			}
		}
	}

	public void RemoveValidationInfo(ValidationInfo validationInfo)
	{
		for (int num = base.Count - 1; num >= 0; num--)
		{
			ValidationInfo validationInfo2 = base[num];
			if (validationInfo2 == validationInfo)
			{
				if (validationInfo2.Errors != null)
				{
					for (int num2 = validationInfo2.Errors.Count - 1; num2 >= 0; num2--)
					{
						validationInfo2.Errors.RemoveAt(num2);
					}
				}
				RemoveAt(num);
			}
		}
	}

	public void RemoveSkippedErrors(Dictionary<ErrorItem.ErrorSource, bool> skippedErrors)
	{
		for (int i = 0; i < skippedErrors.Count; i++)
		{
			KeyValuePair<ErrorItem.ErrorSource, bool> keyValuePair = skippedErrors.ElementAt(i);
			for (int j = 0; j < base.Count; j++)
			{
				ValidationInfo validationInfo = base[j];
				for (int k = 0; k < validationInfo.Errors.Count; k++)
				{
					if (validationInfo.Errors[k].ErrorItemSource != 0 && validationInfo.Errors[k].ErrorItemSource == keyValuePair.Key)
					{
						validationInfo.Errors.RemoveAt(k);
						validationInfo.ErrorCount--;
						k--;
						skippedErrors[keyValuePair.Key] = true;
					}
				}
				if (validationInfo.ErrorCount == 0)
				{
					Remove(validationInfo);
					j--;
				}
			}
		}
	}
}
