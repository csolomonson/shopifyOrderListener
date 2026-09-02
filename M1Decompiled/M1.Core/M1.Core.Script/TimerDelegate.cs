using System;
using System.Reflection;

namespace M1.Core.Script;

public class TimerDelegate
{
	public Delegate HandlerDelegate { get; set; }

	public object Component { get; set; }

	public EventInfo EventInfo { get; set; }
}
