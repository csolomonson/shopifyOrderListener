using System.Collections.Generic;

namespace M1.Core;

public class ProcessValidation
{
	public string MessageID = string.Empty;

	public string MessageText = string.Empty;

	public ProcessValidationMessageType MessageType;

	public string GridID = string.Empty;

	public bool? Result;

	public List<object[]> SelectedItems = new List<object[]>();

	public string[] SelectedItemFieldNames = new string[0];

	public ProcessValidation(string messageID, string messageText, ProcessValidationMessageType messageType)
	{
		MessageID = messageID;
		MessageText = messageText;
		MessageType = messageType;
	}

	public ProcessValidation(string messageID, string messageText, ProcessValidationMessageType messageType, string gridID)
	{
		MessageID = messageID;
		MessageText = messageText;
		MessageType = messageType;
		GridID = gridID;
	}
}
