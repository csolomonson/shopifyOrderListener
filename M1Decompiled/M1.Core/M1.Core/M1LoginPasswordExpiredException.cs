using System;
using System.Runtime.Serialization;

namespace M1.Core;

[Serializable]
public class M1LoginPasswordExpiredException : M1LoginException
{
	public M1LoginPasswordExpiredException()
	{
	}

	public M1LoginPasswordExpiredException(string message)
		: base(message)
	{
	}

	public M1LoginPasswordExpiredException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected M1LoginPasswordExpiredException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
