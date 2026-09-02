using System;
using System.Runtime.Serialization;

namespace M1.Core;

[Serializable]
public class M1LoginDataDictionaryDoesNotExistException : M1LoginException
{
	public M1LoginDataDictionaryDoesNotExistException()
	{
	}

	public M1LoginDataDictionaryDoesNotExistException(string message)
		: base(message)
	{
	}

	public M1LoginDataDictionaryDoesNotExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected M1LoginDataDictionaryDoesNotExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
