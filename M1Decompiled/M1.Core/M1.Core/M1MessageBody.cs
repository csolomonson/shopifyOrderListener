namespace M1.Core;

public class M1MessageBody
{
	private string _Html;

	private string _Text;

	private bool _IsHtml;

	public string Html => _Html;

	public string Text => _Text;

	public bool IsHtml => _IsHtml;

	public M1MessageBody(string text)
	{
		_Text = text;
		_Html = string.Empty;
		_IsHtml = false;
	}

	public M1MessageBody(string text, string html, bool isHtml)
	{
		_Text = text;
		_Html = html;
		_IsHtml = isHtml;
	}
}
