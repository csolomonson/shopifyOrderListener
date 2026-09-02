using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using M1.Extensions;

namespace M1.Core;

public class AdditionalFilterParameterMultiValue : AdditionalFilterParameter
{
	public class FilterParameterValueItem
	{
		public List<object> Values = new List<object>();

		public string Text = string.Empty;

		public override string ToString()
		{
			return Text;
		}

		public string GetIDFromValues()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object value in Values)
			{
				if (value == DBNull.Value || value == null)
				{
					stringBuilder.Append(new string(' ', 30));
				}
				else
				{
					stringBuilder.Append(value.ToString().PadRight(30, ' '));
				}
			}
			return stringBuilder.ToString();
		}
	}

	public int InputSize;

	private FilterParameterValueItem currentValueItem;

	private FilterParameterValueItem _Value;

	private bool _InAllowMultiplesChange;

	private bool _AllowMultiples = true;

	public string[] ValueFields = new string[0];

	private object[] _CurrentValues = new object[0];

	public Dictionary<string, FilterParameterValueItem> PossibleValues = new Dictionary<string, FilterParameterValueItem>();

	public FilterParameterValueItem Value
	{
		get
		{
			return _Value;
		}
		set
		{
			if (_Value != value)
			{
				_Value = value;
				if (!_AllowMultiples)
				{
					OnFilterChanged();
				}
			}
		}
	}

	public bool InAllowMultiplesChange => _InAllowMultiplesChange;

	public bool AllowMultiples
	{
		get
		{
			return _AllowMultiples;
		}
		set
		{
			if (_AllowMultiples != value)
			{
				_AllowMultiples = value;
				bool inAllowMultiplesChange = _InAllowMultiplesChange;
				_InAllowMultiplesChange = true;
				OnFilterChanged();
				_InAllowMultiplesChange = inAllowMultiplesChange;
			}
		}
	}

	public object[] CurrentValues
	{
		get
		{
			return _CurrentValues;
		}
		set
		{
			_CurrentValues = value;
			setCurrentItem();
		}
	}

	public event EventHandler PossibleValuesChanged;

	public AdditionalFilterParameterMultiValue(string caption, DataRow row, string[] currentValueFields)
		: base(caption)
	{
		if (row != null && currentValueFields != null && currentValueFields.Length != 0 && row[currentValueFields[0]] != DBNull.Value && row[currentValueFields[0]].ToString().Trim().Length != 0)
		{
			object[] array = new object[currentValueFields.Length];
			for (int i = 0; i < currentValueFields.Length; i++)
			{
				array[i] = row[currentValueFields[i]];
			}
			CurrentValues = array;
		}
	}

	public override void VerifyCurrentAndDefaultFields(Dictionary<string, object> currentValueFields, Dictionary<string, object> defaultValues)
	{
		if (_Value != null && _Value == currentValueItem)
		{
			for (int i = 0; i < ValueFields.Length; i++)
			{
				if (!currentValueFields.ContainsKey(ValueFields[i]))
				{
					currentValueFields.Add(ValueFields[i], _Value.Values[i]);
				}
			}
		}
		else
		{
			if (_Value == null || PossibleValues.Count <= 1)
			{
				return;
			}
			for (int j = 0; j < ValueFields.Length; j++)
			{
				if (!defaultValues.ContainsKey(ValueFields[j]))
				{
					defaultValues.Add(ValueFields[j], _Value.Values[j]);
				}
			}
		}
	}

	private void setCurrentItem()
	{
		if (CurrentValues != null && CurrentValues.Length != 0)
		{
			currentValueItem = new FilterParameterValueItem();
			currentValueItem.Values.AddRange(CurrentValues);
			currentValueItem.Text = "Use Current Values";
		}
		else
		{
			currentValueItem = null;
		}
	}

	protected void OnPossibleValuesChanged()
	{
		this.PossibleValuesChanged?.Invoke(this, EventArgs.Empty);
	}

	public override void DataTableLoad(DataView view, M1BindingSource bindingSource)
	{
		PossibleValues.Clear();
		if (currentValueItem != null)
		{
			PossibleValues.Add(currentValueItem.GetIDFromValues(), currentValueItem);
		}
		foreach (DataRowView item in view)
		{
			FillFromDataRow(item.Row, bindingSource);
		}
		OnPossibleValuesChanged();
	}

	protected void FillFromDataRow(DataRow row, M1BindingSource bindingSource)
	{
		FilterParameterValueItem filterParameterValueItem = new FilterParameterValueItem();
		StringBuilder stringBuilder = new StringBuilder();
		string[] valueFields = ValueFields;
		foreach (string text in valueFields)
		{
			if (bindingSource.Fields[text].RelatedTableDescriptionField.Length == 0)
			{
				if (row[text] != DBNull.Value)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append('/');
					}
					if (!string.IsNullOrWhiteSpace(bindingSource.Fields[text].ValueList))
					{
						stringBuilder.Append(bindingSource.Fields[text].GetValueListText(row[text]));
					}
					else
					{
						stringBuilder.Append(row[text].ToString().Trim());
					}
				}
			}
			else
			{
				DataRow dataRow = bindingSource.Fields[text].RelatedTableGetDataRow(bindingSource.Fields[text].RelatedTableDescriptionField, bindingSource.GetDatabaseForRow(row), row);
				if (dataRow != null && dataRow[0] != DBNull.Value)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append('/');
					}
					stringBuilder.Append(dataRow[0].ToString().Trim());
				}
			}
			filterParameterValueItem.Values.Add(row[text]);
		}
		string iDFromValues = filterParameterValueItem.GetIDFromValues();
		if (!PossibleValues.ContainsKey(iDFromValues))
		{
			filterParameterValueItem.Text = stringBuilder.ToString();
			PossibleValues.Add(iDFromValues, filterParameterValueItem);
		}
	}

	protected override string ProcessFilterExpression(bool sql)
	{
		if (sql && SqlFilterExpression.Length != 0)
		{
			return SqlFilterExpression;
		}
		if (!sql && _Value != null && _Value.ToString().Length != 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(AdoFilterExpression);
			if (!_AllowMultiples)
			{
				for (int i = 0; i < ValueFields.Length; i++)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(" And ");
					}
					stringBuilder.Append(ValueFields[i] + "=" + _Value.Values[i].ToLinq());
				}
			}
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Insert(0, '(');
				stringBuilder.Append(')');
			}
			return stringBuilder.ToString();
		}
		if (!sql && _Value != null && string.IsNullOrEmpty(_Value.ToString()) && !IgnoreWhenEmpty && !AllowMultiples)
		{
			IEnumerable<string> values = ValueFields.Select((string valueField) => $"{valueField} = {string.Empty.ToLinq()}");
			return string.Join(" AND ", values);
		}
		return string.Empty;
	}
}
