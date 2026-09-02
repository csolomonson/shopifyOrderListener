using System;
using System.Runtime.InteropServices;

namespace M1.Core.Report;

[ComVisible(true)]
public class ReportPromptFieldInfo
{
	public string[] RelatedAndCurrentFieldArray;

	public string FieldName = string.Empty;

	public int FieldLength;

	public int FieldDecimals;

	public string FieldType = string.Empty;

	public string RelatedTable = string.Empty;

	public string[] RelatedTableReturnFields;

	public string RelatedTableSearchGridId;

	public string RelatedTableForeignFilter = string.Empty;

	private string _Caption = string.Empty;

	public string FieldModule = string.Empty;

	private DropDownTextFilter[] _ValueList;

	public string Caption
	{
		get
		{
			return _Caption;
		}
		set
		{
			_Caption = value;
		}
	}

	public DropDownTextFilter[] ValueList
	{
		get
		{
			return _ValueList;
		}
		set
		{
			_ValueList = value;
		}
	}

	public DropDownTextFilter GetValueListItem(object value)
	{
		if (_ValueList != null && value != null)
		{
			string text = value.ToString();
			DropDownTextFilter[] valueList = _ValueList;
			foreach (DropDownTextFilter dropDownTextFilter in valueList)
			{
				if (dropDownTextFilter.Value != null && dropDownTextFilter.Value.ToString().Equals(text, StringComparison.CurrentCultureIgnoreCase))
				{
					return dropDownTextFilter;
				}
			}
			if (short.TryParse(text, out var result))
			{
				result--;
				if (_ValueList.Length > result)
				{
					return _ValueList[result];
				}
			}
		}
		return null;
	}
}
