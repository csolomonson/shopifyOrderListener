using System;
using System.Collections.Generic;
using Limilabs.Client.SMTP;
using Limilabs.Mail;
using Limilabs.Mail.Headers;

namespace M1.Core.Mail;

public class SmtpMailProvider : IMailProvider, IDisposable
{
	public delegate void AttachLogoToEmail<T>(T builder, string message, int numberImages);

	protected Smtp SmtpServer;

	public virtual void Login(string mailServer, string password, M1UserSettings userSettings)
	{
		string providerEmailAddress = userSettings.ProviderEmailAddress;
		SmtpServer = new Smtp();
		SmtpServer.ConnectSSL(mailServer);
		SmtpServer.Login(providerEmailAddress, password);
	}

	public virtual string TestConnection(string mailServer, string user, string password, M1UserSettings userSettings)
	{
		Smtp smtp = new Smtp();
		try
		{
			smtp.ConnectSSL(mailServer);
			smtp.Login(user, password);
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
		return string.Empty;
	}

	public bool Send(MessageData message)
	{
		return SmtpServer.SendMessage(BuildMailMessage(message)).IsPositive;
	}

	public void Dispose()
	{
		if (SmtpServer != null)
		{
			SmtpServer.Close();
			SmtpServer = null;
		}
	}

	private IMail BuildMailMessage(MessageData message)
	{
		MailBuilder mailBuilder = new MailBuilder();
		mailBuilder.Subject = message.Subject;
		mailBuilder.From.Add(new MailBox(message.From));
		switch (message.Importance)
		{
		case M1MessageImportance.High:
			mailBuilder.Importance = MimeImportance.High;
			break;
		case M1MessageImportance.Low:
			mailBuilder.Importance = MimeImportance.Low;
			break;
		default:
			mailBuilder.Importance = MimeImportance.Normal;
			break;
		}
		foreach (string item in message.CleanRecipients(message.Recipients))
		{
			mailBuilder.To.Add(new MailBox(MailManager.GetValidEmailAddress(item)));
		}
		foreach (string item2 in message.CleanRecipients(message.CC))
		{
			mailBuilder.Cc.Add(new MailBox(MailManager.GetValidEmailAddress(item2)));
		}
		foreach (string item3 in message.CleanRecipients(message.BCC))
		{
			mailBuilder.Bcc.Add(new MailBox(MailManager.GetValidEmailAddress(item3)));
		}
		if (message.Body.IsHtml)
		{
			mailBuilder.Html = BuildHtmlWithMessage(mailBuilder, message, AttachLogo);
		}
		else
		{
			mailBuilder.Text = message.Body.Text;
		}
		foreach (MessageAttachment attachment in message.Attachments)
		{
			mailBuilder.AddAttachment(attachment.GetData()).ContentDisposition.FileName = attachment.Description;
		}
		return mailBuilder.Create();
	}

	private string BuildHtmlWithMessage(MailBuilder builder, MessageData message, AttachLogoToEmail<MailBuilder> attachLogo)
	{
		string text = message.Body.Html;
		if (!text.Contains("ImageLogo"))
		{
			return text;
		}
		string imgHtmlWithCID = HtmlFormat.GetImgHtmlWithCID("ImageLogo");
		List<string> imageOrPath = HtmlFormat.GetImageOrPath(text);
		int num = 0;
		foreach (string item in imageOrPath)
		{
			attachLogo(builder, item, num);
			text = text.Replace(item, imgHtmlWithCID + num);
			num++;
		}
		return text;
	}

	public void AttachLogo<T>(T builder, string message, int i)
	{
		byte[] data = Convert.FromBase64String(HtmlFormat.GetImageBase64Format(message));
		(builder as MailBuilder).AddVisual(data).ContentId = "ImageLogo" + i;
	}
}
