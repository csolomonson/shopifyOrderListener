using System;
using System.Runtime.InteropServices;

namespace M1.Core;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
[ComDefaultInterface(typeof(IComActionMessageEventArgs))]
public class ActionMessagesEventArgs : EventArgs, IComActionMessageEventArgs
{
	private object[] _Parameters;

	private object[] _ParametersEx;

	public string MessageID { get; set; }

	public object[] Parameters => _Parameters;

	public object[] ParametersEx => _ParametersEx;

	public object ParametersLength => _Parameters.Length;

	public object ParametersExLength => _ParametersEx.Length;

	public ActionMessagesEventArgs(string messageID, object[] parameters, object[] parametersEx)
	{
		MessageID = messageID;
		_Parameters = parameters;
		_ParametersEx = parametersEx;
	}

	object IComActionMessageEventArgs.Parameters(object index)
	{
		return _Parameters[Convert.ToInt32(index)];
	}

	object IComActionMessageEventArgs.ParametersEx(object index)
	{
		return _ParametersEx[Convert.ToInt32(index)];
	}

	public object GetParametersAsArray()
	{
		return _Parameters;
	}

	public object GetParametersExAsArray()
	{
		return _ParametersEx;
	}
}
