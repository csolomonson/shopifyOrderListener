using System;
using System.Runtime.Serialization;

namespace M1.Core;

[Serializable]
public class M1LoginInvalidVersionException : M1LoginException
{
	public M1LoginInvalidVersionException()
	{
	}

	public M1LoginInvalidVersionException(string message)
		: base(message)
	{
	}

	public M1LoginInvalidVersionException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected M1LoginInvalidVersionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
