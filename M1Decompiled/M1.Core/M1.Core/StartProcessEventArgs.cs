using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace M1.Core;

[ComVisible(true)]
public class StartProcessEventArgs : CancelEventArgs
{
	public ProcessCheckValidation CheckValidationForSave;

	public List<string> Messages = new List<string>();

	public ErrorItemsList ValidationMessages;

	public List<object[]> PromptFieldValues = new List<object[]>();

	public List<ProcessSelectedItemValues> SelectedItems = new List<ProcessSelectedItemValues>();

	public Dictionary<string, object> DefaultFieldValues;

	public List<object[]> KeysCreated = new List<object[]>();

	public string OpenKeysWithObjectID = string.Empty;

	public M1BindingSource BindingSource;

	public Dictionary<ErrorItem.ErrorSource, bool> SkippedErrors = new Dictionary<ErrorItem.ErrorSource, bool>();

	public ActionMessagesEventArgs ActionMessagesArgs;

	public List<string> NegativeQtyOnHandMessages = new List<string>();

	public bool ShowNegativeQtyOnHandMsg;

	public List<string> FilterErrorRegex { get; set; } = new List<string>();

	public StartProcessEventArgs()
	{
	}

	public StartProcessEventArgs(List<object[]> promptFieldValues, List<ProcessSelectedItemValues> selectedItemValues, List<string> messages, ProcessCheckValidation checkValidation)
	{
		CheckValidationForSave = checkValidation;
		if (messages == null)
		{
			Messages = new List<string>();
		}
		else
		{
			Messages = messages;
		}
		SelectedItems = selectedItemValues;
		PromptFieldValues = promptFieldValues;
	}

	public void ApplyFilterRegex()
	{
		if (ValidationMessages == null)
		{
			return;
		}
		foreach (string item in FilterErrorRegex)
		{
			Regex regex = new Regex(item, RegexOptions.IgnoreCase);
			for (int num = ValidationMessages.Count - 1; num >= 0; num--)
			{
				for (int num2 = ValidationMessages[num].Errors.Count - 1; num2 >= 0; num2--)
				{
					ErrorItem errorItem = ValidationMessages[num].Errors[num2];
					if (regex.Match(errorItem.ErrorText).Success)
					{
						ValidationMessages[num].Errors.RemoveAt(num2);
					}
				}
				if (ValidationMessages[num].Errors.Count == 0)
				{
					ValidationMessages.RemoveAt(num);
				}
			}
		}
	}
}
