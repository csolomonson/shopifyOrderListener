using System;
using System.Runtime.Serialization;

namespace M1.Core;

[Serializable]
public class M1LoginException : M1Exception
{
	public M1LoginException()
	{
	}

	public M1LoginException(string message)
		: base(message)
	{
	}

	public M1LoginException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected M1LoginException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
