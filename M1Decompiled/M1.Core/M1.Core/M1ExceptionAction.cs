using System;

namespace M1.Core;

[Serializable]
public class M1ExceptionAction
{
	public string Text = string.Empty;

	public object Data;

	public bool CloseOnAction;

	public IServiceProvider Provider;

	public M1ExceptionActionDelegate Action;

	public M1ExceptionAction(string text, object data, IServiceProvider provider, M1ExceptionActionDelegate action, bool closeOnAction)
	{
		Text = text;
		Data = data;
		Provider = provider;
		Action = action;
		CloseOnAction = closeOnAction;
	}
}
