using System;
using System.Runtime.Serialization;

namespace M1.Core;

[Serializable]
public class M1LoginInvalidUserIDOrPasswordException : M1LoginException
{
	public M1LoginInvalidUserIDOrPasswordException()
		: base("The system could not log you on. Make sure your User ID is correct, then type your password again. Letters in passwords must be typed using the correct case. Make sure that Caps Lock is not accidentally on.")
	{
	}

	public M1LoginInvalidUserIDOrPasswordException(string message)
		: base(message)
	{
	}

	public M1LoginInvalidUserIDOrPasswordException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected M1LoginInvalidUserIDOrPasswordException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
