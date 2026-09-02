using System;
using System.Collections.Generic;

namespace M1.Core.Script;

public class VBEventHandlerInfo : IDisposable
{
	public ScriptingBase ScriptControl;

	protected Dictionary<string, string> MethodsToRun = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

	public VBEventHandlerInfo(ScriptingBase scriptControl)
	{
		ScriptControl = scriptControl;
	}

	public VBEventHandlerInfo(string methodToRun, ScriptingBase scriptControl)
	{
		ScriptControl = scriptControl;
		AddMethodToRun(methodToRun, string.Empty);
	}

	public void AddMethodToRun(string methodToRun, string appExtID)
	{
		if (!MethodsToRun.ContainsKey(methodToRun))
		{
			MethodsToRun.Add(methodToRun, appExtID);
		}
	}

	public void InfoHandlerDelegate(object sender, EventArgs e)
	{
		if (MethodsToRun == null)
		{
			return;
		}
		foreach (KeyValuePair<string, string> item in MethodsToRun)
		{
			if (ScriptControl != null && item.Key != null)
			{
				ScriptControl.ExecuteEvent(item.Key, sender, e);
			}
			if (MethodsToRun == null)
			{
				break;
			}
		}
	}

	public void Dispose()
	{
		ScriptControl = null;
		if (MethodsToRun != null)
		{
			MethodsToRun.Clear();
			MethodsToRun = null;
		}
	}
}
