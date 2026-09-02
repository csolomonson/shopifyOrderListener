using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using M1.Extensions;

namespace M1.Core.Report;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDispatch)]
public class ReportParameterData
{
	protected bool _IsValid = true;

	public List<ReportPromptFieldInfo> Fields = new List<ReportPromptFieldInfo>();

	private int _InstanceCount;

	public string Table = string.Empty;

	public string[] KeyFieldsArray;

	public string[] ContactFieldsArray;

	public string ContactsTable;

	public bool CanBeSaved;

	protected bool _Required;

	public string RowSource = string.Empty;

	public string DisplayGroup = string.Empty;

	private ReportDisplayType _DisplayType;

	public CrystalFieldOption[] FieldOptions = new CrystalFieldOption[0];

	public bool IsValid
	{
		get
		{
			return _IsValid;
		}
		set
		{
			if (_IsValid != value)
			{
				_IsValid = value;
				OnIsValidChanged(EventArgs.Empty);
			}
		}
	}

	public int FieldCount => _InstanceCount;

	public int InstanceCount
	{
		get
		{
			return _InstanceCount;
		}
		set
		{
			_InstanceCount = value;
			List<CrystalFieldOption> list = new List<CrystalFieldOption>();
			list.Clear();
			for (int i = 0; i < _InstanceCount; i++)
			{
				list.Add(new CrystalFieldOption());
			}
			FieldOptions = list.ToArray();
		}
	}

	public bool Required
	{
		get
		{
			return _Required;
		}
		set
		{
			_Required = value;
			CheckIsValid(null);
		}
	}

	public ReportDisplayType DisplayType
	{
		get
		{
			return _DisplayType;
		}
		set
		{
			_DisplayType = value;
		}
	}

	public event EventHandler IsValidChanged;

	public event EventHandler DataChanged;

	public bool CheckIsValid(List<ErrorItem> errors)
	{
		bool flag = true;
		if (Required)
		{
			CrystalFieldOption[] fieldOptions = FieldOptions;
			foreach (CrystalFieldOption crystalFieldOption in fieldOptions)
			{
				if (crystalFieldOption.Values.Count == 0 || crystalFieldOption.Values[0].Length == 0 || M1Util.IsNullOrEmpty(crystalFieldOption.Values[0][0]))
				{
					flag = false;
				}
			}
			if (!flag)
			{
				errors?.Add(new ErrorItem(null, null, null, 0, $"{Fields[0].Caption} is required", ErrorItem.MsgTypeEnum.Error));
			}
		}
		if (InstanceCount == 2 && FieldOptions.Length == 2 && FieldOptions[0].Values.Count != 0 && !M1Util.IsNullOrEmpty(FieldOptions[0].Values[0][0]) && FieldOptions[1].Values.Count != 0 && !M1Util.IsNullOrEmpty(FieldOptions[1].Values[0][0]) && !IsRangeValid(FieldOptions[0].Values[0][0], FieldOptions[1].Values[0][0]))
		{
			flag = false;
			errors?.Add(new ErrorItem(null, null, null, 0, $"Parameter {Fields[0].Caption} has a from value greater than the to value", ErrorItem.MsgTypeEnum.Error));
		}
		IsValid = flag;
		return IsValid;
	}

	protected void OnIsValidChanged(EventArgs e)
	{
		this.IsValidChanged?.Invoke(this, e);
	}

	public bool IsRangeValid(object lowerValue, object upperValue)
	{
		if (FieldDefinition.IsFieldTypeADate(FieldDefinition.charToFieldType(Fields[0].FieldType)))
		{
			if (Convert.ToDateTime(upperValue) < Convert.ToDateTime(lowerValue))
			{
				return false;
			}
		}
		else if (FieldDefinition.IsFieldTypeAString(FieldDefinition.charToFieldType(Fields[0].FieldType)))
		{
			if (Convert.ToString(upperValue).CompareTo(Convert.ToString(lowerValue)) < 0)
			{
				return false;
			}
		}
		else if (FieldDefinition.IsFieldTypeANumber(FieldDefinition.charToFieldType(Fields[0].FieldType)) && Convert.ToDecimal(upperValue) < Convert.ToDecimal(lowerValue))
		{
			return false;
		}
		return true;
	}

	public void OnDataChanged(EventArgs e)
	{
		this.DataChanged?.Invoke(this, e);
	}

	public void ClearValues()
	{
		CrystalFieldOption[] fieldOptions = FieldOptions;
		for (int i = 0; i < fieldOptions.Length; i++)
		{
			fieldOptions[i].Values.Clear();
		}
	}

	public void AddValue(object value)
	{
		FieldOptions[0].Values.Add(new object[1] { value });
	}

	public void AddValues(int index, object[] values)
	{
		FieldOptions[index].Values.Add(values);
	}

	public int GetValuesCount(int fieldOption)
	{
		return FieldOptions[fieldOption].Values.Count;
	}

	public object GetValue(int fieldOption, int values, int index)
	{
		return FieldOptions[fieldOption].Values[values][index];
	}

	public CrystalFieldOption GetFieldOption(int index)
	{
		if (index == -1)
		{
			return FieldOptions[FieldOptions.Length - 1];
		}
		return FieldOptions[index];
	}

	public ReportPromptFieldInfo GetFieldInfo(int index)
	{
		if (index == -1)
		{
			return Fields[Fields.Count - 1];
		}
		return Fields[index];
	}

	public DropDownTextFilter[] GetValueList(int index)
	{
		return Fields[index].ValueList;
	}
}
