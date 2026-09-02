using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;

namespace M1.Core;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
[ComDefaultInterface(typeof(IComValidationInfo))]
public class ValidationInfo : IComValidationInfo
{
	public M1BindingSource BindingSource;

	public M1Database Database;

	public DataRow Row;

	public object Source;

	public FieldDefinition Field;

	public List<ErrorItem> Errors = new List<ErrorItem>();

	public int ErrorCount;

	public int WarningCount;

	public int MessageCount;

	public string RowDescription = string.Empty;

	public bool IsMemo;

	public ValidationInfo()
	{
	}

	public ValidationInfo(ValidationInfo source)
	{
		Copy(source);
	}

	public ValidationInfo(M1BindingSource bindingSource, object source, DataRow row, FieldDefinition field, bool isMemo = false)
	{
		BindingSource = bindingSource;
		Source = source;
		Row = row;
		Field = field;
		IsMemo = isMemo;
	}

	public void Copy(ValidationInfo source)
	{
		Copy(source, clearExisting: true);
	}

	public void Copy(ValidationInfo source, bool clearExisting)
	{
		BindingSource = source.BindingSource;
		Row = source.Row;
		Field = source.Field;
		ErrorCount = source.ErrorCount;
		WarningCount = source.WarningCount;
		MessageCount = source.MessageCount;
		IsMemo = source.IsMemo;
		if (clearExisting)
		{
			Errors.Clear();
		}
		Errors.AddRange(source.Errors);
	}

	public void Clear()
	{
		Errors.Clear();
		ErrorCount = 0;
		WarningCount = 0;
		MessageCount = 0;
	}

	public void AddError(string errorText)
	{
		if (errorText != null && errorText.Length != 0)
		{
			Errors.Add(new ErrorItem(BindingSource, Row, Field, 0, errorText, ErrorItem.MsgTypeEnum.Error));
			ErrorCount++;
		}
	}

	public void AddError(string errorText, bool setAsModified)
	{
		if (errorText != null && errorText.Length != 0)
		{
			if (setAsModified && Row.RowState == DataRowState.Unchanged)
			{
				Row.SetModified();
			}
			Errors.Add(new ErrorItem(BindingSource, Row, Field, 0, errorText, ErrorItem.MsgTypeEnum.Error));
			ErrorCount++;
		}
	}

	public void AddWarning(string errorText)
	{
		if (errorText != null && errorText.Length != 0)
		{
			Errors.Add(new ErrorItem(BindingSource, Row, Field, 0, errorText, ErrorItem.MsgTypeEnum.Warning));
			WarningCount++;
		}
	}

	public void AddMessage(string errorText)
	{
		if (errorText != null && errorText.Length != 0)
		{
			Errors.Add(new ErrorItem(BindingSource, Row, Field, 0, errorText, ErrorItem.MsgTypeEnum.Information));
			MessageCount++;
		}
	}

	public void AddError(string errorText, ErrorItem.ErrorSource errorSource)
	{
		if (errorText != null && errorText.Length != 0)
		{
			ErrorItem errorItem = new ErrorItem(BindingSource, Row, Field, 0, errorText, ErrorItem.MsgTypeEnum.Error);
			errorItem.ErrorItemSource = errorSource;
			Errors.Add(errorItem);
			ErrorCount++;
		}
	}

	public string GetRowDescription()
	{
		if (BindingSource != null && BindingSource.IsDefinitionLoaded && Row != null && Row.RowState != DataRowState.Detached)
		{
			string result = string.Empty;
			string text = BindingSource.PrimaryTable.LastKeyField.ToUpper();
			if (text.Length != 0)
			{
				result = BindingSource.Fields[text].RelatedFieldsFormatCaptionAndCurrentValues(Row);
			}
			return result;
		}
		return RowDescription;
	}

	public override string ToString()
	{
		string text = string.Empty;
		foreach (ErrorItem error in Errors)
		{
			text = text + error.ToString() + "\r\n";
		}
		return text;
	}
}
