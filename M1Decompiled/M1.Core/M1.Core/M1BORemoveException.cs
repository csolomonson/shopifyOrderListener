using System;
using System.Runtime.Serialization;

namespace M1.Core;

[Serializable]
public class M1BORemoveException : M1Exception
{
	public string RowDescription = string.Empty;

	public string ValidationInfo = string.Empty;

	public M1BORemoveException()
	{
	}

	public M1BORemoveException(string message, string rowDescription, string validationInfo)
		: base(message)
	{
		RowDescription = rowDescription;
		ValidationInfo = validationInfo;
	}

	public M1BORemoveException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected M1BORemoveException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
