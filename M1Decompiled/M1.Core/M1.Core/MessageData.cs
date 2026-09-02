using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using M1.Extensions;

namespace M1.Core;

public class MessageData : IDisposable
{
	private string _Subject = string.Empty;

	private bool _CreateCall;

	private M1MessageImportance _Importance = M1MessageImportance.Normal;

	private M1MessageBody _Body;

	public List<string> Recipients;

	public List<string> CC;

	public List<string> BCC;

	public List<MessageAttachment> Attachments;

	private bool _MailMerge;

	private string _TemplateFile = string.Empty;

	private string _MessageGroup = string.Empty;

	private IServiceProvider _Provider;

	private string _documentTable = string.Empty;

	private List<object[]> _documentKeys;

	private string[] _documentKeyFields;

	public string Subject
	{
		get
		{
			return _Subject;
		}
		set
		{
			_Subject = value;
			OnSubjectChanged();
		}
	}

	public bool CreateCall
	{
		get
		{
			return _CreateCall;
		}
		set
		{
			_CreateCall = value;
		}
	}

	public M1MessageImportance Importance
	{
		get
		{
			return _Importance;
		}
		set
		{
			_Importance = value;
		}
	}

	public M1MessageBody Body
	{
		get
		{
			return _Body;
		}
		set
		{
			_Body = value;
		}
	}

	public string From { get; set; }

	public string RecipientsText
	{
		get
		{
			return ListToString(Recipients);
		}
		set
		{
			TranslateStringToList(value, Recipients);
		}
	}

	public string CCText
	{
		get
		{
			return ListToString(CC);
		}
		set
		{
			TranslateStringToList(value, CC);
		}
	}

	public string BCCText
	{
		get
		{
			return ListToString(BCC);
		}
		set
		{
			TranslateStringToList(value, BCC);
		}
	}

	public bool MailMerge
	{
		get
		{
			return _MailMerge;
		}
		set
		{
			_MailMerge = value;
		}
	}

	public string TemplateFile
	{
		get
		{
			return _TemplateFile;
		}
		set
		{
			_TemplateFile = value;
		}
	}

	public string MessageGroup
	{
		get
		{
			return _MessageGroup;
		}
		set
		{
			_MessageGroup = value;
		}
	}

	public IServiceProvider Provider
	{
		get
		{
			return _Provider;
		}
		set
		{
			_Provider = value;
		}
	}

	public string DocumentTable
	{
		get
		{
			return _documentTable;
		}
		set
		{
			_documentTable = value;
		}
	}

	public List<object[]> DocumentKeys
	{
		get
		{
			return _documentKeys;
		}
		set
		{
			_documentKeys = value;
		}
	}

	public string[] DocumentKeyFields
	{
		get
		{
			return _documentKeyFields;
		}
		set
		{
			_documentKeyFields = value;
		}
	}

	public string ReviewText => Subject + " (" + RecipientsText + ")";

	public event EventHandler SubjectChanged;

	protected void OnSubjectChanged()
	{
		this.SubjectChanged?.Invoke(this, EventArgs.Empty);
	}

	public MessageData(IServiceProvider provider, string recipients, string cc, string bcc, string subject, string body, string attachmentTitle, string attachmentFileName, string documentTable, string[] documentKeyFields, List<object[]> documentKeys)
	{
		Provider = provider;
		M1User obj = provider.GetService(typeof(M1User)) as M1User;
		M1Database m1Database = provider.GetService(typeof(M1Database)) as M1Database;
		CreateCall = m1Database.Props("HD").Field<bool>("xapHDCreateCallForEmails");
		M1DataDictionary dataDictionary = obj.DataDictionary;
		From = m1Database.UserEmailAddress;
		Subject = LanguageChooser.ChooseLanguage(dataDictionary, subject);
		string signature = obj.Settings.Signature;
		new HtmlToText();
		string text = RemoveTag(signature, "<style", "</style>");
		text = text.Replace("\r\n", "");
		if (body == null)
		{
			body = string.Empty;
		}
		if (body.Length == 0 || ContainHtml(body) || ContainText(text))
		{
			body = Regex.Replace(body, "(\\r\\\\?)", "<br />");
			HtmlFormat.AddElementToHTMLDocument(ref body, signature);
			HtmlToText htmlToText = new HtmlToText();
			Body = new M1MessageBody(htmlToText.Convert(body), body, isHtml: true);
		}
		else
		{
			Body = new M1MessageBody(body);
		}
		Recipients = new List<string>();
		TranslateStringToList(recipients, Recipients);
		CC = new List<string>();
		TranslateStringToList(cc, CC);
		BCC = new List<string>();
		TranslateStringToList(bcc, BCC);
		Attachments = new List<MessageAttachment>();
		if (!string.IsNullOrWhiteSpace(attachmentFileName))
		{
			foreach (M1EmailAttachment item in processAttachments(attachmentFileName, attachmentTitle))
			{
				AddAttachment(item.Path, item.Description);
				if (item.DeleteAfterSend && File.Exists(item.Path))
				{
					FileAttributes attributes = File.GetAttributes(item.Path);
					if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
					{
						attributes = RemoveAttribute(attributes, FileAttributes.ReadOnly);
						File.SetAttributes(item.Path, attributes);
					}
					File.Delete(item.Path);
				}
			}
		}
		if (!string.IsNullOrWhiteSpace(documentTable))
		{
			DocumentTable = documentTable;
			DocumentKeyFields = documentKeyFields;
			DocumentKeys = documentKeys;
		}
	}

	private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
	{
		return attributes & ~attributesToRemove;
	}

	private bool ContainHtml(string body)
	{
		bool result = false;
		if (body.StartsWith("<html", StringComparison.CurrentCultureIgnoreCase) || body.StartsWith("<!DOCTYPE html>", StringComparison.CurrentCultureIgnoreCase))
		{
			result = true;
		}
		return result;
	}

	private static string RemoveTag(string html, string startTag, string endTag)
	{
		html = html.Replace("<br />", "");
		bool flag;
		do
		{
			flag = false;
			int num = html.IndexOf(startTag, 0, StringComparison.CurrentCultureIgnoreCase);
			if (num >= 0)
			{
				int num2 = html.IndexOf(endTag, num + 1, StringComparison.CurrentCultureIgnoreCase);
				if (num2 > num)
				{
					html = html.Remove(num, num2 - num + endTag.Length);
					flag = true;
				}
			}
		}
		while (flag);
		return html;
	}

	private bool ContainText(string signature)
	{
		return !signature.Contains("<p></p>");
	}

	public List<string> CleanRecipients(List<string> recipients)
	{
		List<string> list = new List<string>();
		foreach (string recipient in recipients)
		{
			list.Add(StripEmbeddedData(recipient));
		}
		return list;
	}

	public string StripEmbeddedData(string recipient)
	{
		int num = recipient.IndexOf("[Org:", StringComparison.CurrentCultureIgnoreCase);
		if (num != -1)
		{
			recipient = recipient.Substring(0, num);
		}
		return recipient;
	}

	public void AddAttachment(string fileName, string description)
	{
		Attachments.Add(new MessageAttachment(fileName, description));
	}

	public void TranslateStringToList(string items, List<string> list)
	{
		list.Clear();
		if (string.IsNullOrWhiteSpace(items))
		{
			return;
		}
		string[] array = items.Split(';');
		foreach (string text in array)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				list.Add(text.Trim());
			}
		}
	}

	public string ListToString(List<string> items)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string item in items)
		{
			stringBuilder.Append(item + "; ");
		}
		return stringBuilder.ToString();
	}

	public override string ToString()
	{
		return ReviewText;
	}

	private List<M1EmailAttachment> processAttachments(string attachmentFileName, string attachmentTitle)
	{
		List<M1EmailAttachment> list = new List<M1EmailAttachment>();
		if (attachmentFileName != null && attachmentFileName.Trim().Length != 0)
		{
			string[] array = attachmentFileName.Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);
			string[] array2 = attachmentTitle.Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);
			int num = 0;
			string[] array3 = array;
			foreach (string text in array3)
			{
				M1EmailAttachment m1EmailAttachment = new M1EmailAttachment();
				if (text.EndsWith(":delete", StringComparison.CurrentCultureIgnoreCase))
				{
					m1EmailAttachment.DeleteAfterSend = true;
					m1EmailAttachment.Path = text.Substring(0, text.Length - 7);
				}
				else
				{
					m1EmailAttachment.Path = text;
				}
				if (array2.Length > num)
				{
					m1EmailAttachment.Description = array2[num];
				}
				else
				{
					m1EmailAttachment.Description = m1EmailAttachment.Path;
				}
				list.Add(m1EmailAttachment);
				num++;
			}
		}
		return list;
	}

	public void Dispose()
	{
		if (Attachments == null)
		{
			return;
		}
		foreach (MessageAttachment attachment in Attachments)
		{
			attachment.Dispose();
		}
		Attachments.Clear();
	}
}
