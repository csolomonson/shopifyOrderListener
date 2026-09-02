using System;
using System.Runtime.Serialization;

namespace M1.Core;

[Serializable]
public class M1MissingOrInvalidDataException : M1Exception
{
	public M1MissingOrInvalidDataException()
	{
	}

	public M1MissingOrInvalidDataException(string message)
		: base(message)
	{
	}

	public M1MissingOrInvalidDataException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected M1MissingOrInvalidDataException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
