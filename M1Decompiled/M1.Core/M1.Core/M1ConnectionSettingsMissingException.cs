using System;
using System.Runtime.Serialization;

namespace M1.Core;

[Serializable]
public class M1ConnectionSettingsMissingException : Exception
{
	public M1ConnectionSettingsMissingException()
	{
	}

	public M1ConnectionSettingsMissingException(string message)
		: base(message)
	{
	}

	public M1ConnectionSettingsMissingException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected M1ConnectionSettingsMissingException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
