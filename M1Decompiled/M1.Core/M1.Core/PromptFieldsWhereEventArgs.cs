using System;
using System.Collections.Generic;

namespace M1.Core;

public class PromptFieldsWhereEventArgs : EventArgs
{
	public string QueryFormat;

	public List<object[]> KeyValues;

	public string[] FieldNames;

	public string Where;

	public PromptFieldsWhereEventArgs(List<object[]> keyValues, string[] fieldNames, string queryFormat)
	{
		KeyValues = keyValues;
		FieldNames = fieldNames;
		QueryFormat = queryFormat;
	}
}
