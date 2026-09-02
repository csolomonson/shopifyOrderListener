namespace M1.Core;

public class M1EmailAttachment
{
	public string Path;

	public string Description;

	public bool DeleteAfterSend;

	public M1EmailAttachment()
	{
		Path = string.Empty;
		Description = string.Empty;
		DeleteAfterSend = false;
	}
}
