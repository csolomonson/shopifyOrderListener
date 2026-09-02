using System;
using System.Runtime.Serialization;

namespace M1.Core;

[Serializable]
public class M1SecurityException : M1Exception
{
	public M1SecurityException()
	{
	}

	public M1SecurityException(string message)
		: base(message)
	{
	}

	public M1SecurityException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected M1SecurityException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
