using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Graph;

namespace M1.Core.Mail;

public class Office365MailProvider : IMailProvider, IDisposable
{
	private string outlookOnlineUrl = "https://outlook.office.com/";

	private string composeMailUrl = "mail/deeplink/compose/";

	public static GraphServiceClient Client;

	public Office365MailProvider()
	{
		Client = AuthenticationHelper.GetAuthenticatedClientForUser();
	}

	public void Dispose()
	{
		throw new NotImplementedException();
	}

	public void Login(string mailServer, string password, M1UserSettings userSettings)
	{
		throw new NotImplementedException();
	}

	public string TestConnection(string mailServer, string user, string password, M1UserSettings userSettings)
	{
		throw new NotImplementedException();
	}

	public bool Send(MessageData message)
	{
		throw new NotImplementedException();
	}

	public async Task DisplayMail(MessageData message)
	{
		List<Microsoft.Graph.Recipient> recipients = new List<Microsoft.Graph.Recipient>();
		message.Recipients.ForEach(delegate(string recipient)
		{
			recipients.Add(new Microsoft.Graph.Recipient
			{
				EmailAddress = new EmailAddress
				{
					Address = recipient
				}
			});
		});
		Message message2 = new Message
		{
			Subject = message.Subject,
			ToRecipients = recipients,
			Body = new ItemBody
			{
				Content = message.Body.Html,
				ContentType = BodyType.Html
			}
		};
		MessageAttachment attachment = message.Attachments.FirstOrDefault();
		AddAttachment(message2, attachment);
		Message message3 = await Client.Me.Messages.Request().AddAsync(message2);
		System.Diagnostics.Process.Start(outlookOnlineUrl + composeMailUrl + HttpUtility.UrlEncode(message3.Id));
	}

	private void AddAttachment(Message message, MessageAttachment attachment)
	{
		if (attachment != null)
		{
			FileAttachment item = new FileAttachment
			{
				Name = attachment.Description,
				ContentType = "text/plain",
				ContentBytes = attachment.GetData()
			};
			IMessageAttachmentsCollectionPage attachments = new MessageAttachmentsCollectionPage { item };
			message.Attachments = attachments;
		}
	}

	public void AttachLogo<T>(T builder, string message, int numberImages)
	{
		throw new NotImplementedException();
	}
}
