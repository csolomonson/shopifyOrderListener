using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using M1.Extensions;
using Microsoft.Exchange.WebServices.Data;

namespace M1.Core.Mail;

public static class MailManager
{
	private class MailProcessing
	{
		public bool IsProcessing;

		public void Start(object data)
		{
			_ = (IServiceProvider)data;
			try
			{
				ProcessMessagesInQueue();
			}
			finally
			{
				_MessagesSent = 0;
			}
		}

		private void user_LoggingOut(object sender, LoggingOutEventArgs e)
		{
			if (mailThread != null)
			{
				mailThread = null;
			}
			mailProc = null;
		}

		private async System.Threading.Tasks.Task ProcessMessagesInQueue()
		{
			if (IsProcessing)
			{
				return;
			}
			Dictionary<Type, IMailProvider> dictionary = new Dictionary<Type, IMailProvider>();
			try
			{
				while (_MessageQueue.Count != 0)
				{
					IsProcessing = true;
					try
					{
						while (_MessageQueue.Count != 0)
						{
							MessageData messageData = _MessageQueue.Dequeue();
							M1Database m1Database = messageData.Provider.GetService(typeof(M1Database)) as M1Database;
							Type mailProviderType = GetMailProviderType(m1Database.Props("DS").Field<string>("xadMailProvider"));
							IMailProvider mailProvider;
							if (!dictionary.ContainsKey(mailProviderType))
							{
								mailProvider = (IMailProvider)Activator.CreateInstance(mailProviderType);
								M1User m1User = messageData.Provider.GetService(typeof(M1User)) as M1User;
								AppContext obj = messageData.Provider.GetService(typeof(AppContext)) as AppContext;
								string mailServer = m1Database.Props("DatasetProperties").Field<string>("xadMailServer");
								string password = obj?.DBServerManager.Decrypt(m1User.Settings.ProviderEmailPasswordEncrypted);
								M1UserSettings userSettings = m1User?.Settings;
								try
								{
									mailProvider.Login(mailServer, password, userSettings);
								}
								catch (M1OnlineAuthenticationException ex)
								{
									List<MessageData> collection = new List<MessageData>(_MessageQueue);
									_MessageQueue.Clear();
									List<MessageData> list = new List<MessageData> { messageData };
									list.AddRange(collection);
									M1ExceptionAction m1ExceptionAction = new M1ExceptionAction("Retry Email", list, messageData.Provider, ResendEmail, closeOnAction: true);
									ex.Data.Add("Retry", m1ExceptionAction);
									Tuple<string, string> message = new Tuple<string, string>("Unauthorized", ex.Message);
									OnMessageQueueChanged();
									ResendEmailHandler(m1ExceptionAction, message);
									return;
								}
								dictionary.Add(mailProviderType, mailProvider);
							}
							else
							{
								mailProvider = dictionary[mailProviderType];
							}
							try
							{
								if (mailProvider.Send(messageData))
								{
									m1Database.OnEmailMessageSent(new EmailMessageSentEventArgs(m1Database, messageData));
									_MessagesSent++;
								}
								OnMessageQueueChanged();
								messageData.Dispose();
							}
							catch (ServiceResponseException ex2)
							{
								List<MessageData> list2 = new List<MessageData>(_MessageQueue);
								_MessageQueue.Clear();
								List<MessageData> list3 = new List<MessageData>();
								list3.Add(messageData);
								list3.AddRange(list2);
								M1ExceptionAction m1ExceptionAction2 = new M1ExceptionAction("Retry Email", list3, messageData.Provider, ResendEmail, closeOnAction: true);
								ex2.Data.Add("Retry", m1ExceptionAction2);
								if (ex2.ErrorCode == ServiceError.ErrorSendAsDenied)
								{
									Tuple<string, string> message2 = new Tuple<string, string>("Unauthorized", ex2.Message);
									OnMessageQueueChanged();
									ResendEmailHandler(m1ExceptionAction2, message2);
									return;
								}
								if (list2.Count != 0)
								{
									ex2.Data.Add("Continue", new M1ExceptionAction("Continue", list2, messageData.Provider, ContinueEmails, closeOnAction: true));
								}
								OnMessageQueueChanged();
								if (MailManager.EmailSendException != null)
								{
									MailManager.EmailSendException(m1Database, new ThreadExceptionEventArgs(ex2));
									return;
								}
								throw;
							}
							catch (Exception ex3)
							{
								List<MessageData> list4 = new List<MessageData>(_MessageQueue);
								_MessageQueue.Clear();
								List<MessageData> list5 = new List<MessageData>();
								list5.Add(messageData);
								list5.AddRange(list4);
								M1ExceptionAction m1ExceptionAction3 = new M1ExceptionAction("Retry Email", list5, messageData.Provider, ResendEmail, closeOnAction: true);
								ex3.Data.Add("Retry", m1ExceptionAction3);
								if (ex3.Message.Contains("(401)"))
								{
									Tuple<string, string> message3 = new Tuple<string, string>("Unauthorized", "Please review your email address and password in user options before trying again.");
									OnMessageQueueChanged();
									ResendEmailHandler(m1ExceptionAction3, message3);
									return;
								}
								if (list4.Count != 0)
								{
									ex3.Data.Add("Continue", new M1ExceptionAction("Continue", list4, messageData.Provider, ContinueEmails, closeOnAction: true));
								}
								OnMessageQueueChanged();
								if (MailManager.EmailSendException != null)
								{
									MailManager.EmailSendException(m1Database, new ThreadExceptionEventArgs(ex3));
									return;
								}
								throw;
							}
						}
					}
					finally
					{
						foreach (KeyValuePair<Type, IMailProvider> item in dictionary)
						{
							item.Value.Dispose();
						}
						dictionary.Clear();
						IsProcessing = false;
					}
				}
			}
			finally
			{
				mailThread = null;
			}
		}
	}

	public static ResendEmailDelegate ResendEmailHandler;

	public static Queue<MessageData> _MessageQueue;

	public static EventHandler MessageQueueChanged;

	public static int _MessagesSent;

	private static Thread mailThread;

	private static MailProcessing mailProc;

	public static event EventHandler<ThreadExceptionEventArgs> EmailSendException;

	public static void SendCodeAsAttachment(IServiceProvider provider, string recipients, string subject, string body, string attachmentTitle, string code)
	{
		SendCodeAsAttachment(provider, recipients, subject, body, attachmentTitle, code, string.Empty);
	}

	public static void SendCodeAsAttachment(IServiceProvider provider, string recipients, string subject, string body, string attachmentTitle, string code, string messageGroup)
	{
		string text = M1Util.GenerateTempFileName("m1p");
		M1Util.CreateM1PFile(text, code);
		SendEmail(provider, recipients, subject, body, attachmentTitle, text + ":DELETE", messageGroup);
	}

	public static void SendEmail(IServiceProvider provider, string recipients, string subject, string body, string attachmentTitle, string attachmentFileName)
	{
		SendEmail(provider, recipients, subject, body, attachmentTitle, attachmentFileName, string.Empty);
	}

	public static void SendEmail(IServiceProvider provider, string recipients, string subject, string body, string attachmentTitle, string attachmentFileName, string messageGroup)
	{
		SendEmail(provider, recipients, subject, body, attachmentTitle, attachmentFileName, messageGroup, string.Empty);
	}

	public static void SendEmail(IServiceProvider provider, string recipients, string subject, string body, string attachmentTitle, string attachmentFileName, string messageGroup, string templateFile)
	{
		SendEmail(new MessageData(provider, recipients, string.Empty, string.Empty, subject, body, attachmentTitle, attachmentFileName, string.Empty, null, null)
		{
			TemplateFile = templateFile,
			MessageGroup = messageGroup
		});
	}

	public static void SendEmail(MessageData message)
	{
		AddMessageToQueue(message);
	}

	public static void SendEmail(IEnumerable<MessageData> messages)
	{
		AddMessageToQueue(messages);
	}

	private static string cleanRecipients(string recipients)
	{
		for (int num = recipients.IndexOf("[Org:"); num != -1; num = recipients.IndexOf("[Org:"))
		{
			int num2 = recipients.IndexOf(']', num);
			recipients = ((num2 == -1) ? recipients.Substring(0, num) : (recipients.Substring(0, num) + recipients.Substring(num2 + 1)));
		}
		return recipients;
	}

	public static Type GetMailProviderType(string mailProvider)
	{
		if (mailProvider.Equals("GMAIL", StringComparison.CurrentCultureIgnoreCase))
		{
			return typeof(GmailMailProvider);
		}
		if (mailProvider.Equals("SMTP", StringComparison.CurrentCultureIgnoreCase))
		{
			return typeof(SmtpMailProvider);
		}
		if (mailProvider.Equals("MAPI", StringComparison.CurrentCultureIgnoreCase) || mailProvider.Equals("OUTLOOK", StringComparison.CurrentCultureIgnoreCase))
		{
			return typeof(MapiMailProvider);
		}
		if (mailProvider.Equals("EXCHANGE", StringComparison.CurrentCultureIgnoreCase) || mailProvider.Equals("EXCHANGE2019", StringComparison.CurrentCultureIgnoreCase))
		{
			return typeof(ExchangeMailProvider);
		}
		if (mailProvider.Equals("OFFICE365", StringComparison.CurrentCultureIgnoreCase))
		{
			return typeof(ExchangeOnlineMailProvider);
		}
		return null;
	}

	private static void OnMessageQueueChanged()
	{
		MessageQueueChanged?.Invoke(null, EventArgs.Empty);
	}

	public static void AddMessageToQueue(MessageData message)
	{
		if (_MessageQueue == null)
		{
			_MessageQueue = new Queue<MessageData>();
		}
		_MessageQueue.Enqueue(message);
		StartMailProcessor(message.Provider);
		OnMessageQueueChanged();
	}

	public static void AddMessageToQueue(IEnumerable<MessageData> messages)
	{
		if (messages == null)
		{
			return;
		}
		if (_MessageQueue == null)
		{
			_MessageQueue = new Queue<MessageData>();
		}
		IServiceProvider serviceProvider = null;
		foreach (MessageData message in messages)
		{
			if (serviceProvider == null)
			{
				serviceProvider = message.Provider;
			}
			_MessageQueue.Enqueue(message);
		}
		StartMailProcessor(serviceProvider);
		OnMessageQueueChanged();
	}

	public static string GetValidEmailAddress(string emailAddress)
	{
		int num = emailAddress.IndexOf('<');
		int num2 = emailAddress.IndexOf('>');
		if (num != -1 && num2 != -1)
		{
			num++;
			return emailAddress.Substring(num, num2 - num);
		}
		return emailAddress;
	}

	private static string ContinueEmails(M1ExceptionAction action)
	{
		foreach (MessageData item in (List<MessageData>)action.Data)
		{
			_MessageQueue.Enqueue(item);
		}
		StartMailProcessor(action.Provider);
		return string.Empty;
	}

	private static string ResendEmail(M1ExceptionAction action)
	{
		foreach (MessageData item in (List<MessageData>)action.Data)
		{
			SendEmail(item);
		}
		return string.Empty;
	}

	private static void StartMailProcessor(IServiceProvider provider)
	{
		if (mailProc == null)
		{
			mailProc = new MailProcessing();
		}
		if (!mailProc.IsProcessing && mailThread == null)
		{
			mailThread = new Thread(mailProc.Start);
			mailThread.IsBackground = true;
			mailThread.Start(provider);
		}
	}
}
