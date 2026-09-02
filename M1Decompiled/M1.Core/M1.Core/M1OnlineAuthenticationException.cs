using System;
using System.Runtime.Serialization;

namespace M1.Core;

[Serializable]
public class M1OnlineAuthenticationException : M1Exception
{
	public M1OnlineAuthenticationException()
	{
	}

	public M1OnlineAuthenticationException(string message)
		: base(message)
	{
	}

	public M1OnlineAuthenticationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected M1OnlineAuthenticationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
