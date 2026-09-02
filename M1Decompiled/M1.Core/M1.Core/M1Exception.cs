using System;
using System.Runtime.Serialization;

namespace M1.Core;

[Serializable]
public class M1Exception : ApplicationException
{
	public M1Exception()
	{
	}

	public M1Exception(string message)
		: base(message)
	{
	}

	public M1Exception(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected M1Exception(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
