using System.Runtime.InteropServices;

namespace M1.Core.Report;

[ComVisible(true)]
public class ReportEmailOptions
{
	public string EmailWebLink = string.Empty;

	public string EmailSubject = string.Empty;

	public string EmailAttachmentName = string.Empty;

	public bool MultipleRecordsPerContact = true;

	public bool MultipleAttachmentsPerEmail;

	public string EmailBody = string.Empty;

	public string EmailContactField = string.Empty;
}
