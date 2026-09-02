using System.Windows.Forms;

namespace M1.Core.Mail;

public static class ServerMessageFeedback
{
	public static void ProcessError(string message)
	{
		string empty = string.Empty;
		string caption = "Confirm";
		if (string.IsNullOrWhiteSpace(message))
		{
			empty = "Connection successful.";
		}
		else if (message.Contains("5.7.14"))
		{
			empty = "Please configure your Gmail account and turn on the \"Access for less secure apps\"";
			caption = "Gmail Security";
		}
		else if (message.Contains("5.7.8"))
		{
			empty = "Invalid username or password!";
			caption = "Authentication Fail";
		}
		else
		{
			empty = message;
		}
		MessageBox.Show(empty, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
	}
}
