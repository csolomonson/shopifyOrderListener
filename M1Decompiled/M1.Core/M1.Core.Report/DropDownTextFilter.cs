using System.Runtime.InteropServices;

namespace M1.Core.Report;

[ComVisible(true)]
public class DropDownTextFilter
{
	public string Text;

	public object Value;

	public string Filter;

	public DropDownTextFilter(string text, string filter, object value)
	{
		Text = text;
		Filter = filter;
		Value = value;
	}
}
