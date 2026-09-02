using System;
using System.Runtime.Serialization;

namespace M1.Core;

[Serializable]
public class M1ResourceWarningException : M1Exception
{
	public M1ResourceWarningException()
	{
	}

	public M1ResourceWarningException(string message)
		: base(message)
	{
	}

	public M1ResourceWarningException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected M1ResourceWarningException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
