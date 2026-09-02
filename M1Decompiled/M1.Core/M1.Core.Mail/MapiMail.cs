using System;
using System.Windows.Forms;

namespace M1.Core.Mail;

public class MapiMail
{
	private delegate IntPtr tempHandle(Form form);

	public bool SendMAPI(MessageData message, bool showDialog)
	{
		try
		{
			MapiMailMessage mapiMailMessage = new MapiMailMessage(message.Subject, message.Body.Text, showDialog);
			foreach (string item in message.CleanRecipients(message.Recipients))
			{
				if (!string.IsNullOrWhiteSpace(item))
				{
					mapiMailMessage.Recipients.Add(MailManager.GetValidEmailAddress(item), MapiMailMessage.RecipientType.To);
				}
			}
			foreach (string item2 in message.CleanRecipients(message.CC))
			{
				if (!string.IsNullOrWhiteSpace(item2))
				{
					mapiMailMessage.Recipients.Add(MailManager.GetValidEmailAddress(item2), MapiMailMessage.RecipientType.CC);
				}
			}
			foreach (string item3 in message.CleanRecipients(message.BCC))
			{
				if (!string.IsNullOrWhiteSpace(item3))
				{
					mapiMailMessage.Recipients.Add(MailManager.GetValidEmailAddress(item3), MapiMailMessage.RecipientType.BCC);
				}
			}
			foreach (MessageAttachment attachment in message.Attachments)
			{
				string filePath = attachment.CopyToFile();
				mapiMailMessage.AddAttachment(filePath, attachment.Description, deleteAfterSend: true);
			}
			if (mapiMailMessage.Recipients.Count == 0)
			{
				mapiMailMessage.ShowDialog = true;
				mapiMailMessage.Send(IntPtr.Zero);
			}
			else
			{
				tempHandle method = getHandle;
				IntPtr handle = (IntPtr)Application.OpenForms[0].Invoke(method, Application.OpenForms[0]);
				mapiMailMessage.Send(handle);
			}
			return true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			return false;
		}
	}

	private IntPtr getHandle(Form form)
	{
		return form.Handle;
	}
}
