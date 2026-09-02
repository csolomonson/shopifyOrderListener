using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using M1.Core.Mail;
using Microsoft.Exchange.WebServices.Data;

namespace M1.Core;

public class ExchangeUtilities
{
	public static string _ExchangeServerUrl = string.Empty;

	private string _mailProvider = string.Empty;

	public ExchangeService GetExchangeService(M1Database database)
	{
		M1User m1User = database.GetService(typeof(M1User)) as M1User;
		_mailProvider = database.Props("DS").Field<string>("xadMailProvider");
		return GetExchangeService(database, m1User.ID, null);
	}

	public ExchangeService GetExchangeService(M1Database database, string userID, SqlTransaction sqlTransaction)
	{
		if (database.Props("DS").Field<string>("xadMailProvider").Equals("OFFICE365", StringComparison.CurrentCultureIgnoreCase))
		{
			return new ExchangeOnlineMailProvider().GetExchangeOnlineService(database.User.Settings);
		}
		M1User m1User = database.GetService(typeof(M1User)) as M1User;
		AppContext appContext = database.GetService(typeof(AppContext)) as AppContext;
		M1UserSettings m1UserSettings;
		if (m1User.ID.Equals(userID, StringComparison.CurrentCultureIgnoreCase))
		{
			m1UserSettings = m1User.Settings;
		}
		else
		{
			m1UserSettings = new M1UserSettings(database);
			m1UserSettings.LoadSettings(m1UserSettings.GetUserProperties(database.GetService(typeof(M1DataDictionary)) as M1DataDictionary, userID));
		}
		string email;
		string password;
		if (!string.IsNullOrWhiteSpace(m1UserSettings.ProviderEmailAddress))
		{
			email = m1UserSettings.ProviderEmailAddress;
			password = appContext.DBServerManager.Decrypt(m1UserSettings.ProviderEmailPasswordEncrypted);
		}
		else
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select Top 1 lmeWorkEmailAddress from Employees Where lmeUserID = @UserID and lmeTerminationDate IS NULL order by lmeEmployeeID");
			sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userID;
			email = Convert.ToString(database.ExecuteScalar(sqlCommand, sqlTransaction));
			password = string.Empty;
		}
		return GetExchangeService(database.Props("DS").Field<string>("xadMailServer"), email, password);
	}

	public ExchangeService GetExchangeService(string mailServer, string email, string password)
	{
		ExchangeService exchangeService = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
		if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
		{
			exchangeService.UseDefaultCredentials = false;
			exchangeService.Credentials = new WebCredentials(email, password);
		}
		else
		{
			exchangeService.UseDefaultCredentials = true;
		}
		if (_mailProvider.Equals("EXCHANGE2019"))
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
		}
		ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
		if (string.IsNullOrWhiteSpace(_ExchangeServerUrl))
		{
			if (!string.IsNullOrWhiteSpace(mailServer) && mailServer.EndsWith("/EWS/Exchange.asmx", StringComparison.CurrentCultureIgnoreCase))
			{
				_ExchangeServerUrl = mailServer;
				exchangeService.Url = new Uri(_ExchangeServerUrl, UriKind.Absolute);
			}
			else if (!string.IsNullOrWhiteSpace(email))
			{
				exchangeService.AutodiscoverUrl(email, (string redirect) => true);
				_ExchangeServerUrl = exchangeService.Url.AbsoluteUri;
			}
		}
		else
		{
			exchangeService.Url = new Uri(_ExchangeServerUrl, UriKind.Absolute);
		}
		return exchangeService;
	}

	public string Autodiscover(string emailAddress, string password, bool isExchange2019 = false)
	{
		ExchangeService exchangeService = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
		if (!string.IsNullOrWhiteSpace(emailAddress) && !string.IsNullOrWhiteSpace(password))
		{
			exchangeService.UseDefaultCredentials = false;
			exchangeService.Credentials = new WebCredentials(emailAddress, password);
		}
		else
		{
			exchangeService.UseDefaultCredentials = true;
		}
		if (isExchange2019)
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
		}
		ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
		exchangeService.AutodiscoverUrl(emailAddress, (string redirect) => true);
		return exchangeService.Url.AbsoluteUri;
	}

	public string TestConnection(string serverUrl, string email, string pwd)
	{
		try
		{
			ExchangeService exchangeService = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
			if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(pwd))
			{
				exchangeService.UseDefaultCredentials = false;
				exchangeService.Credentials = new WebCredentials(email, pwd);
			}
			else
			{
				exchangeService.UseDefaultCredentials = true;
			}
			if (_mailProvider.Equals("EXCHANGE2019"))
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			}
			ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
			if (string.IsNullOrEmpty(_ExchangeServerUrl))
			{
				if (!string.IsNullOrEmpty(serverUrl) && serverUrl.EndsWith("/EWS/Exchange.asmx", StringComparison.CurrentCultureIgnoreCase))
				{
					_ExchangeServerUrl = serverUrl;
					exchangeService.Url = new Uri(_ExchangeServerUrl, UriKind.Absolute);
				}
				else if (email != null)
				{
					exchangeService.AutodiscoverUrl(email);
					_ExchangeServerUrl = exchangeService.Url.AbsoluteUri;
				}
			}
			else
			{
				exchangeService.Url = new Uri(_ExchangeServerUrl, UriKind.Absolute);
			}
			Folder.Bind(exchangeService, WellKnownFolderName.Inbox, new PropertySet());
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
		return string.Empty;
	}

	public ExchangeFolder LoadPublicFolders(ExchangeService service)
	{
		FolderView folderView = new FolderView(100);
		folderView.PropertySet = new PropertySet(BasePropertySet.IdOnly);
		folderView.PropertySet.Add(FolderSchema.DisplayName);
		folderView.Traversal = FolderTraversal.Shallow;
		FindFoldersResults findFoldersResults = service.FindFolders(WellKnownFolderName.PublicFoldersRoot, folderView);
		ExchangeFolder exchangeFolder = new ExchangeFolder("All Public Folders", "");
		foreach (Folder item in findFoldersResults)
		{
			loadSubFoldersIntoTree(item, exchangeFolder, folderView);
		}
		return exchangeFolder;
	}

	private void loadSubFoldersIntoTree(Folder parentFolder, ExchangeFolder eFolder, FolderView fview)
	{
		ExchangeFolder exchangeFolder = new ExchangeFolder(parentFolder.DisplayName, "");
		eFolder.Folders.Add(exchangeFolder);
		foreach (Folder item in parentFolder.FindFolders(fview))
		{
			loadSubFoldersIntoTree(item, exchangeFolder, fview);
		}
	}

	public Folder GetPublicFolderByPath(ExchangeService service, string ewsFolderPath)
	{
		if (ewsFolderPath.StartsWith("Public Folders\\", StringComparison.CurrentCultureIgnoreCase))
		{
			ewsFolderPath.Substring(15);
		}
		if (ewsFolderPath.StartsWith("Favorites\\", StringComparison.CurrentCultureIgnoreCase))
		{
			ewsFolderPath.Substring(19);
		}
		if (ewsFolderPath.StartsWith("All Public Folders\\", StringComparison.CurrentCultureIgnoreCase))
		{
			ewsFolderPath.Substring(19);
		}
		string[] array = ewsFolderPath.Split('\\');
		Folder folder = null;
		Folder folder2 = null;
		for (int i = 0; i < array.Count(); i++)
		{
			if (i == 0)
			{
				folder = GetTopLevelFolder(service, array[i]);
				folder2 = folder;
			}
			else
			{
				folder2 = GetFolder(service, folder.Id, array[i]);
				folder = folder2;
			}
		}
		return folder2;
	}

	public Folder GetFolder(ExchangeService service, WellKnownFolderName wellKnownfolder)
	{
		return Folder.Bind(service, wellKnownfolder);
	}

	private Folder GetTopLevelFolder(ExchangeService service, string folderName)
	{
		FolderView view = new FolderView(3);
		foreach (Folder item in service.FindFolders(WellKnownFolderName.PublicFoldersRoot, view))
		{
			if (folderName.Equals(item.DisplayName, StringComparison.InvariantCultureIgnoreCase))
			{
				return item;
			}
		}
		throw new Exception("Top Level Folder not found: " + folderName);
	}

	private Folder GetFolder(ExchangeService service, FolderId ParentFolderId, string folderName)
	{
		FolderView view = new FolderView(int.MaxValue);
		foreach (Folder item in service.FindFolders(ParentFolderId, view))
		{
			if (folderName.Equals(item.DisplayName, StringComparison.InvariantCultureIgnoreCase))
			{
				return item;
			}
		}
		throw new Exception("Folder not found: " + folderName);
	}

	public void RefreshExchangeIDs(List<ExchangeAppointment> appointments, M1Database database, string table, string exchangeIDField, string uniqueIDField)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Update " + table + " Set " + exchangeIDField + " = @ID Where " + uniqueIDField + " = @UniqueID");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar));
		sqlCommand.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier));
		foreach (ExchangeAppointment appointment in appointments)
		{
			if (appointment.IDChanged)
			{
				sqlCommand.Parameters["@ID"].Value = appointment.ID;
				sqlCommand.Parameters["@UniqueID"].Value = appointment.SourceUniqueID;
				database.ExecuteCommand(sqlCommand);
			}
		}
	}

	public void DeleteTask(ExchangeService service, string itemID)
	{
		Task.Bind(service, new ItemId(itemID), new PropertySet()).Delete(DeleteMode.HardDelete);
	}

	public void DeleteAppointment(ExchangeService service, string itemID)
	{
		Appointment.Bind(service, new ItemId(itemID), new PropertySet()).Delete(DeleteMode.HardDelete);
	}

	public ExchangeAppointment GetAppointment(ExchangeService service, string itemID)
	{
		ExchangeAppointment exchangeAppointment = new ExchangeAppointment(itemID);
		Appointment appointment;
		try
		{
			appointment = Appointment.Bind(service, new ItemId(itemID), new PropertySet(ItemSchema.Subject, AppointmentSchema.Start, AppointmentSchema.End, ItemSchema.Body, AppointmentSchema.StartTimeZone, ItemSchema.Importance, ItemSchema.IsReminderSet, ItemSchema.ReminderDueBy, AppointmentSchema.Location));
		}
		catch
		{
			return null;
		}
		exchangeAppointment.Body = appointment.Body;
		exchangeAppointment.Subject = appointment.Subject;
		exchangeAppointment.MeetingLocation = appointment.Location;
		exchangeAppointment.Start = appointment.Start;
		exchangeAppointment.End = appointment.End;
		exchangeAppointment.Importance = appointment.Importance;
		return exchangeAppointment;
	}

	public ExchangeAppointment GetTask(ExchangeService service, string itemID)
	{
		ExchangeAppointment exchangeAppointment = new ExchangeAppointment(itemID);
		Task task;
		try
		{
			task = Task.Bind(service, new ItemId(exchangeAppointment.ID), new PropertySet(ItemSchema.Subject, TaskSchema.StartDate, TaskSchema.DueDate, ItemSchema.Body, TaskSchema.Status, ItemSchema.Importance, ItemSchema.IsReminderSet, ItemSchema.ReminderDueBy));
		}
		catch
		{
			return null;
		}
		exchangeAppointment.Body = task.Body;
		exchangeAppointment.Subject = task.Subject;
		exchangeAppointment.Start = task.StartDate;
		exchangeAppointment.End = task.DueDate;
		exchangeAppointment.Status = task.Status;
		exchangeAppointment.Importance = task.Importance;
		return exchangeAppointment;
	}

	public void ExportAppointments(List<ExchangeAppointment> appointments, ExchangeService service, Folder exchangeFolder)
	{
		foreach (ExchangeAppointment appointment2 in appointments)
		{
			Appointment appointment;
			if (!string.IsNullOrWhiteSpace(appointment2.ID))
			{
				try
				{
					appointment = Appointment.Bind(service, new ItemId(appointment2.ID), new PropertySet(ItemSchema.Subject, AppointmentSchema.Start, AppointmentSchema.End, ItemSchema.Body, AppointmentSchema.StartTimeZone, ItemSchema.Importance, ItemSchema.IsReminderSet, ItemSchema.ReminderDueBy, AppointmentSchema.Location));
				}
				catch
				{
					appointment2.ID = string.Empty;
					appointment = new Appointment(service);
				}
			}
			else
			{
				appointment = new Appointment(service);
			}
			appointment.Subject = appointment2.Subject;
			appointment.Body = appointment2.Body;
			appointment.Location = appointment2.MeetingLocation;
			appointment.StartTimeZone = TimeZoneInfo.Local;
			if (appointment2.Start.HasValue)
			{
				appointment.Start = appointment2.Start.Value;
			}
			else
			{
				appointment.Start = appointment2.End.Value;
			}
			appointment.End = appointment2.End.Value;
			appointment.Importance = appointment2.Importance;
			appointment.IsReminderSet = appointment2.IsReminderSet;
			if (appointment2.ReminderDueBy.HasValue)
			{
				appointment.ReminderDueBy = appointment2.ReminderDueBy.Value;
			}
			if (!string.IsNullOrWhiteSpace(appointment2.ID))
			{
				appointment.Update(ConflictResolutionMode.AlwaysOverwrite, SendInvitationsOrCancellationsMode.SendToNone);
			}
			else
			{
				appointment.Save(exchangeFolder.Id, SendInvitationsMode.SendToNone);
			}
			appointment2.ID = appointment.Id.UniqueId;
		}
	}

	public void ExportTasks(List<ExchangeAppointment> appointments, ExchangeService service, Folder exchangeFolder)
	{
		foreach (ExchangeAppointment appointment in appointments)
		{
			Task task;
			if (!string.IsNullOrWhiteSpace(appointment.ID))
			{
				try
				{
					task = Task.Bind(service, new ItemId(appointment.ID), new PropertySet(ItemSchema.Subject, TaskSchema.StartDate, TaskSchema.DueDate, ItemSchema.Body, TaskSchema.Status, ItemSchema.Importance, ItemSchema.IsReminderSet, ItemSchema.ReminderDueBy));
				}
				catch
				{
					appointment.ID = string.Empty;
					task = new Task(service);
				}
			}
			else
			{
				task = new Task(service);
			}
			task.Subject = appointment.Subject;
			task.Body = appointment.Body;
			task.StartDate = appointment.Start;
			task.DueDate = appointment.End;
			task.Status = appointment.Status;
			task.Importance = appointment.Importance;
			task.IsReminderSet = appointment.IsReminderSet;
			if (appointment.ReminderDueBy.HasValue)
			{
				task.ReminderDueBy = appointment.ReminderDueBy.Value;
			}
			if (!string.IsNullOrWhiteSpace(appointment.ID))
			{
				task.Update(ConflictResolutionMode.AlwaysOverwrite);
			}
			else
			{
				task.Save(exchangeFolder.Id);
			}
			appointment.ID = task.Id.UniqueId;
		}
	}
}
