using System;
using System.Runtime.Serialization;

namespace M1.Core;

[Serializable]
public class M1GridIdDoesNotExistException : M1Exception
{
	public M1GridIdDoesNotExistException()
		: base("Grid definition does not exist in DDGridDetails.")
	{
	}

	public M1GridIdDoesNotExistException(string message)
		: base(message)
	{
	}

	public M1GridIdDoesNotExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected M1GridIdDoesNotExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
