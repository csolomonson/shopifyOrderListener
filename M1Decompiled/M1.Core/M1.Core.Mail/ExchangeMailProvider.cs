using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using Microsoft.Exchange.WebServices.Data;

namespace M1.Core.Mail;

public class ExchangeMailProvider : IMailProvider, IDisposable, IMailGetMatchingNames
{
	public delegate void AttachLogoToEmail<T>(T builder, string message, int numberImages);

	private ExchangeService _Service;

	public void Login(string mailServer, string password, M1UserSettings userSettings)
	{
		string providerEmailAddress = userSettings.ProviderEmailAddress;
		_Service = new ExchangeUtilities().GetExchangeService(mailServer, providerEmailAddress, password);
	}

	public string TestConnection(string mailServer, string user, string password, M1UserSettings userSettings)
	{
		return new ExchangeUtilities().TestConnection(mailServer, user, password);
	}

	public bool Send(MessageData message)
	{
		if (!string.IsNullOrWhiteSpace(message.From))
		{
			BuildMailMessage(message, _Service).SendAndSaveCopy();
			return true;
		}
		throw new M1MissingOrInvalidDataException("Originating email address was not set.\nCheck Email settings under User Options.");
	}

	public void Dispose()
	{
		_Service = null;
	}

	private EmailMessage BuildMailMessage(MessageData message, ExchangeService service)
	{
		EmailMessage emailMessage = new EmailMessage(service);
		emailMessage.Subject = message.Subject;
		emailMessage.From = new EmailAddress(message.From);
		switch (message.Importance)
		{
		case M1MessageImportance.High:
			emailMessage.Importance = Importance.High;
			break;
		case M1MessageImportance.Low:
			emailMessage.Importance = Importance.Low;
			break;
		default:
			emailMessage.Importance = Importance.Normal;
			break;
		}
		foreach (string item in message.CleanRecipients(message.Recipients))
		{
			emailMessage.ToRecipients.Add(MailManager.GetValidEmailAddress(item));
		}
		foreach (string item2 in message.CleanRecipients(message.CC))
		{
			emailMessage.CcRecipients.Add(MailManager.GetValidEmailAddress(item2));
		}
		foreach (string item3 in message.CleanRecipients(message.BCC))
		{
			emailMessage.BccRecipients.Add(MailManager.GetValidEmailAddress(item3));
		}
		if (message.Body.IsHtml)
		{
			string text = BuildHtmlWithMessage(emailMessage, message, AttachLogo);
			emailMessage.Body = new MessageBody(BodyType.HTML, text);
		}
		else
		{
			emailMessage.Body = new MessageBody(BodyType.Text, message.Body.Text);
		}
		foreach (MessageAttachment attachment in message.Attachments)
		{
			emailMessage.Attachments.AddFileAttachment(attachment.Description, attachment.GetData());
		}
		return emailMessage;
	}

	public void GetMatchingNames(string name, Dictionary<string, string> searchResults)
	{
		try
		{
			new ExchangeUtilities();
			foreach (NameResolution item in _Service.ResolveName(name, ResolveNameSearchLocation.DirectoryThenContacts, returnContactDetails: true))
			{
				Contact contact = item.Contact;
				string key;
				string value;
				if (contact != null)
				{
					key = ((!string.IsNullOrWhiteSpace(item.Mailbox?.Address)) ? item.Mailbox.Address : getAddress(contact.EmailAddresses));
					value = contact.DisplayName;
				}
				else
				{
					key = item.Mailbox.Address;
					value = item.Mailbox.Name;
				}
				if (!searchResults.ContainsKey(key))
				{
					searchResults.Add(key, value);
				}
			}
		}
		catch
		{
		}
	}

	private string getAddress(EmailAddressDictionary addresses)
	{
		EmailAddressKey[] array = new EmailAddressKey[3]
		{
			EmailAddressKey.EmailAddress1,
			EmailAddressKey.EmailAddress2,
			EmailAddressKey.EmailAddress3
		};
		foreach (EmailAddressKey key in array)
		{
			string text = addresses[key].Address.Split(':')[1];
			if (!text.StartsWith("spo", StringComparison.CurrentCultureIgnoreCase) && IsValidEmail(text))
			{
				return text;
			}
		}
		return "";
	}

	private bool IsValidEmail(string email)
	{
		try
		{
			return new MailAddress(email).Address == email;
		}
		catch
		{
			return false;
		}
	}

	private string BuildHtmlWithMessage(EmailMessage builder, MessageData message, AttachLogoToEmail<EmailMessage> attachLogo)
	{
		string text = message.Body.Html;
		if (!text.Contains("ImageLogo"))
		{
			return message.Body.Html;
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
		byte[] buffer = Convert.FromBase64String(HtmlFormat.GetImageBase64Format(message));
		(builder as EmailMessage).Attachments.AddFileAttachment("image.jpeg", new MemoryStream(buffer));
		(builder as EmailMessage).Attachments[i].ContentId = "ImageLogo" + i;
	}
}
