using System;
using System.Runtime.Serialization;

namespace M1.Core;

[Serializable]
public class ReportParameterException : M1Exception
{
	public ReportParameterException()
	{
	}

	public ReportParameterException(string message)
		: base(message)
	{
	}

	public ReportParameterException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ReportParameterException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
