using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace M1.Core.Mail;

public class MapiMailMessage
{
	[StructLayout(LayoutKind.Sequential)]
	private class MapiFileDescriptor
	{
		public int reserved;

		public int flags;

		public int position;

		public string AttachmentPathName;

		public string AttachmentName;

		public IntPtr type = IntPtr.Zero;
	}

	public enum RecipientType
	{
		To = 1,
		CC,
		BCC
	}

	private class AttachmentInfo
	{
		public string FilePath = string.Empty;

		public string FileName = string.Empty;

		public bool DeleteAfterSend;

		public AttachmentInfo(string filePath, string fileName, bool deleteAfterSend)
		{
			FilePath = filePath;
			FileName = fileName;
			DeleteAfterSend = deleteAfterSend;
		}
	}

	internal class MAPIHelperInterop
	{
		[StructLayout(LayoutKind.Sequential)]
		public class MapiMessage
		{
			public int Reserved;

			public string Subject;

			public string NoteText;

			public string MessageType;

			public string DateReceived;

			public string ConversationID;

			public int Flags;

			public IntPtr Originator = IntPtr.Zero;

			public int RecipientCount;

			public IntPtr Recipients = IntPtr.Zero;

			public int FileCount;

			public IntPtr Files = IntPtr.Zero;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class MapiRecipDesc
		{
			public int Reserved;

			public int RecipientClass;

			public string Name;

			public string Address;

			public int eIDSize;

			public IntPtr EntryID = IntPtr.Zero;
		}

		public const int MAPI_LOGON_UI = 1;

		private MAPIHelperInterop()
		{
		}

		[DllImport("MAPI32.DLL", CharSet = CharSet.Ansi)]
		public static extern int MAPILogon(IntPtr hwnd, string prf, string pw, int flg, int rsv, ref IntPtr sess);

		[DllImport("MAPI32.DLL")]
		public static extern int MAPISendMail(IntPtr session, IntPtr hwnd, MapiMessage message, int flg, int rsv);
	}

	public string errorMessage = string.Empty;

	private string _subject;

	private string _body;

	private RecipientCollection _recipientCollection;

	private List<AttachmentInfo> _attachments;

	private ManualResetEvent _manualResetEvent;

	public bool ShowDialog = true;

	private IntPtr _Handle = IntPtr.Zero;

	public string Subject
	{
		get
		{
			return _subject;
		}
		set
		{
			_subject = value;
		}
	}

	public string Body
	{
		get
		{
			return _body;
		}
		set
		{
			_body = value;
		}
	}

	public RecipientCollection Recipients => _recipientCollection;

	public MapiMailMessage()
	{
		_attachments = new List<AttachmentInfo>();
		_recipientCollection = new RecipientCollection();
		_manualResetEvent = new ManualResetEvent(initialState: false);
	}

	public MapiMailMessage(string subject)
		: this()
	{
		_subject = subject;
	}

	public MapiMailMessage(string subject, string body, bool showDialog)
		: this()
	{
		_subject = subject;
		_body = body;
		ShowDialog = showDialog;
	}

	public void AddAttachment(string FilePath, string FileName, bool deleteAfterSend)
	{
		_attachments.Add(new AttachmentInfo(FilePath, FileName, deleteAfterSend));
	}

	public void Send()
	{
		Thread thread = new Thread((ThreadStart)_ShowMail);
		thread.IsBackground = true;
		thread.SetApartmentState(ApartmentState.STA);
		thread.Start();
		_manualResetEvent.WaitOne();
		_manualResetEvent.Reset();
	}

	public void Send(IntPtr handle)
	{
		_Handle = handle;
		Thread thread = new Thread((ThreadStart)_ShowMail);
		thread.IsBackground = true;
		thread.SetApartmentState(ApartmentState.STA);
		thread.Start();
		_manualResetEvent.WaitOne();
		_manualResetEvent.Reset();
	}

	private void _ShowMail(object ignore)
	{
		MAPIHelperInterop.MapiMessage mapiMessage = new MAPIHelperInterop.MapiMessage();
		using RecipientCollection.InteropRecipientCollection interopRecipientCollection = _recipientCollection.GetInteropRepresentation();
		mapiMessage.Subject = _subject;
		mapiMessage.NoteText = _body;
		mapiMessage.Recipients = interopRecipientCollection.Handle;
		mapiMessage.RecipientCount = _recipientCollection.Count;
		if (_attachments.Count > 0)
		{
			mapiMessage.Files = _AllocAttachments(out mapiMessage.FileCount);
		}
		_manualResetEvent.Set();
		int num = 0;
		num = ((!ShowDialog) ? MAPIHelperInterop.MAPISendMail(IntPtr.Zero, IntPtr.Zero, mapiMessage, 0, 0) : MAPIHelperInterop.MAPISendMail(IntPtr.Zero, _Handle, mapiMessage, 11, 0));
		if (_attachments.Count > 0)
		{
			_DeallocFiles(mapiMessage);
		}
		if (num != 0 && num != 1)
		{
			_LogErrorMapi(num);
			if (errorMessage.Length != 0)
			{
				MessageBox.Show(errorMessage, "Error");
			}
		}
	}

	private void _DeallocFiles(MAPIHelperInterop.MapiMessage message)
	{
		if (!(message.Files != IntPtr.Zero))
		{
			return;
		}
		Type typeFromHandle = typeof(MapiFileDescriptor);
		int num = Marshal.SizeOf(typeFromHandle);
		int num2 = (int)message.Files;
		for (int i = 0; i < message.FileCount; i++)
		{
			Marshal.DestroyStructure((IntPtr)num2, typeFromHandle);
			num2 += num;
		}
		Marshal.FreeHGlobal(message.Files);
		foreach (AttachmentInfo attachment in _attachments)
		{
			if (attachment.DeleteAfterSend && File.Exists(attachment.FilePath))
			{
				FileAttributes attributes = File.GetAttributes(attachment.FilePath);
				if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				{
					attributes = RemoveAttribute(attributes, FileAttributes.ReadOnly);
					File.SetAttributes(attachment.FilePath, attributes);
				}
				File.Delete(attachment.FilePath);
			}
		}
	}

	private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
	{
		return attributes & ~attributesToRemove;
	}

	private IntPtr _AllocAttachments(out int fileCount)
	{
		fileCount = 0;
		if (_attachments == null)
		{
			return IntPtr.Zero;
		}
		if (_attachments.Count <= 0 || _attachments.Count > 100)
		{
			return IntPtr.Zero;
		}
		int num = Marshal.SizeOf(typeof(MapiFileDescriptor));
		IntPtr intPtr = Marshal.AllocHGlobal(_attachments.Count * num);
		MapiFileDescriptor mapiFileDescriptor = new MapiFileDescriptor();
		mapiFileDescriptor.position = -1;
		int num2 = (int)intPtr;
		foreach (AttachmentInfo attachment in _attachments)
		{
			mapiFileDescriptor.AttachmentName = attachment.FileName;
			mapiFileDescriptor.AttachmentPathName = attachment.FilePath;
			Marshal.StructureToPtr(mapiFileDescriptor, (IntPtr)num2, fDeleteOld: false);
			num2 += num;
		}
		fileCount = _attachments.Count;
		return intPtr;
	}

	private void _ShowMail()
	{
		_ShowMail(null);
	}

	private void _LogErrorMapi(int errorCode)
	{
		switch (errorCode)
		{
		case 1:
			errorMessage = "User Aborted.";
			break;
		case 2:
			errorMessage = "MAPI Failure.";
			break;
		case 3:
			errorMessage = "Login Failure.";
			break;
		case 4:
			errorMessage = "MAPI Disk full.";
			break;
		case 5:
			errorMessage = "MAPI Insufficient memory.";
			break;
		case 6:
			errorMessage = "MAPI Block too small.";
			break;
		case 8:
			errorMessage = "MAPI Too many sessions.";
			break;
		case 9:
			errorMessage = "MAPI too many files.";
			break;
		case 10:
			errorMessage = "MAPI too many recipients.";
			break;
		case 11:
			errorMessage = "MAPI Attachment not found.";
			break;
		case 12:
			errorMessage = "MAPI Attachment open failure.";
			break;
		case 13:
			errorMessage = "MAPI Attachment Write Failure.";
			break;
		case 14:
			errorMessage = "MAPI Unknown recipient.";
			break;
		case 15:
			errorMessage = "MAPI Bad recipient type.";
			break;
		case 16:
			errorMessage = "MAPI No messages.";
			break;
		case 17:
			errorMessage = "MAPI Invalid message.";
			break;
		case 18:
			errorMessage = "MAPI Text too large.";
			break;
		case 19:
			errorMessage = "MAPI Invalid session.";
			break;
		case 20:
			errorMessage = "MAPI Type not supported.";
			break;
		case 21:
			errorMessage = "MAPI Ambiguous recipient.";
			break;
		case 22:
			errorMessage = "MAPI Message in use.";
			break;
		case 23:
			errorMessage = "MAPI Network failure.";
			break;
		case 24:
			errorMessage = "MAPI Invalid edit fields.";
			break;
		case 25:
			errorMessage = "MAPI Invalid Recipients.";
			break;
		case 26:
			errorMessage = "MAPI Not supported.";
			break;
		case 999:
			errorMessage = "MAPI No Library.";
			break;
		case 998:
			errorMessage = "MAPI Invalid parameter.";
			break;
		}
		errorMessage = "Error sending MAPI Email. Error: " + errorMessage + " (code = " + errorCode + ").";
	}
}
