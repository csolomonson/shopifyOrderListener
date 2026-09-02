using System;
using System.Reflection;
using System.Runtime.InteropServices;
using M1.Script.Interfaces;

namespace M1.Core.Script;

[ComVisible(true)]
public class ComScriptEngine : ScriptingBase, IScriptEngine
{
	public ComScriptEngine(IServiceProvider provider)
		: base(provider)
	{
	}

	public object Run(string procedureName, object myParams = null)
	{
		if (myParams == Missing.Value)
		{
			myParams = null;
		}
		if (string.Equals(procedureName, "M1DataControl_ViewDestroy", StringComparison.CurrentCultureIgnoreCase))
		{
			StopTimers();
		}
		return base.Run(procedureName, (object[])myParams);
	}
}
