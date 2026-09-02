using System.Collections.Generic;
using System.Data;

namespace M1.Core;

public class ErrorItem
{
	public enum MsgTypeEnum
	{
		Error = 1,
		Warning,
		Information
	}

	public enum ErrorSource
	{
		Serial = 1,
		Lot
	}

	public List<ErrorItem> Errors;

	public M1BindingSource BindingSource;

	public DataRow Row;

	public FieldDefinition Field;

	public string ErrorText = string.Empty;

	public MsgTypeEnum MsgType;

	public int ErrorNumber;

	public ErrorSource ErrorItemSource;

	public ErrorItem(M1BindingSource bindingSource, DataRow row, FieldDefinition field, int errorNumber, string errorText, MsgTypeEnum msgType)
	{
		BindingSource = bindingSource;
		Row = row;
		Field = field;
		ErrorText = errorText;
		ErrorNumber = errorNumber;
		MsgType = msgType;
	}

	public bool IsMessage()
	{
		if (MsgType == MsgTypeEnum.Information)
		{
			if (Errors != null)
			{
				return Errors.Count == 0;
			}
			return true;
		}
		return false;
	}

	public bool IsWarning()
	{
		if (MsgType == MsgTypeEnum.Warning)
		{
			if (Errors != null)
			{
				return Errors.Count == 0;
			}
			return true;
		}
		return false;
	}

	public bool IsError()
	{
		if (MsgType == MsgTypeEnum.Error)
		{
			if (Errors != null)
			{
				return Errors.Count == 0;
			}
			return true;
		}
		return false;
	}

	public override string ToString()
	{
		string text = string.Empty;
		if (BindingSource != null && !BindingSource.IsTopLevel)
		{
			string empty = string.Empty;
			if (Field == null)
			{
				empty = BindingSource.PrimaryTable.LastKeyField.ToUpper();
				if (empty.Length != 0)
				{
					text = BindingSource.Fields[empty].Caption + " " + Row[empty].ToString() + " - ";
				}
			}
			else
			{
				empty = Field.Table.LastKeyField.ToUpper();
				if (empty.Length != 0)
				{
					text = BindingSource.Fields[empty].Caption + " " + Row[empty].ToString() + " - ";
				}
			}
		}
		return text + ErrorText;
	}
}
