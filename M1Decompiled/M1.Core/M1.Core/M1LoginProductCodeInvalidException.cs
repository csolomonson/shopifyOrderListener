using System;
using System.Runtime.Serialization;

namespace M1.Core;

[Serializable]
public class M1LoginProductCodeInvalidException : M1LoginException
{
	public M1LoginProductCodeInvalidException()
	{
	}

	public M1LoginProductCodeInvalidException(string message)
		: base(message)
	{
	}

	public M1LoginProductCodeInvalidException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected M1LoginProductCodeInvalidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
