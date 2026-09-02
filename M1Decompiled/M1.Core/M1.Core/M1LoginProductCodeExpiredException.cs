using System;
using System.Runtime.Serialization;

namespace M1.Core;

[Serializable]
public class M1LoginProductCodeExpiredException : M1LoginException
{
	public M1LoginProductCodeExpiredException()
	{
	}

	public M1LoginProductCodeExpiredException(string message)
		: base(message)
	{
	}

	public M1LoginProductCodeExpiredException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected M1LoginProductCodeExpiredException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
