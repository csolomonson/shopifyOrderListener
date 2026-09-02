using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using M1.Core;
using M1.Extensions;
using Microsoft.Exchange.WebServices.Data;

namespace M1.Ax.Erp;

public class FollowUp
{
	public void RefreshFollowUpsFromExchange(M1Database database)
	{
		M1User m1User = database.GetService(typeof(M1User)) as M1User;
		string employeeIDforUserId = new Employee().GetEmployeeIDforUserId(database, m1User.ID);
		SqlCommand sqlCommand = database.NewSqlCommand("Select * from Followups Where cmfStatus < 3 And cmfAssignedToEmployeeID = @EmployeeID And Not cmfExchangeID Is Null Order By cmfOrganizationID,cmfLocationID,cmfFollowupID");
		sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar)).Value = employeeIDforUserId;
		SqlDataAdapter adapter;
		DataTable dataTable = database.GetDataTable(sqlCommand, fillSchema: false, out adapter);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		ExchangeUtilities exchangeUtilities = new ExchangeUtilities();
		ExchangeService exchangeService = exchangeUtilities.GetExchangeService(database);
		foreach (DataRow row in dataTable.Rows)
		{
			string text = row.Field<string>("cmfExchangeID");
			if (string.IsNullOrWhiteSpace(text))
			{
				continue;
			}
			ExchangeAppointment exchangeAppointment = ((row.Field<byte>("cmfFollowupType") != 1) ? exchangeUtilities.GetTask(exchangeService, text) : exchangeUtilities.GetAppointment(exchangeService, text));
			if (exchangeAppointment != null)
			{
				if (exchangeAppointment.Body != null && (row["cmfLongDescriptionText"] == DBNull.Value || !row.Field<string>("cmfLongDescriptionText").Equals(exchangeAppointment.Body)))
				{
					HtmlToText htmlToText = new HtmlToText();
					string text2 = (exchangeAppointment.Body.StartsWith("<html", StringComparison.InvariantCultureIgnoreCase) ? htmlToText.Convert(exchangeAppointment.Body) : exchangeAppointment.Body);
					RichTextBox richTextBox = new RichTextBox
					{
						Text = text2,
						Font = database.User.Settings.MemoFont
					};
					row.SetField("cmfLongDescriptionText", text2);
					row.SetField("cmfLongDescriptionRtf", richTextBox.Rtf);
				}
				if (exchangeAppointment.Subject.EndsWith(")") && exchangeAppointment.Subject.IndexOf('(') != -1)
				{
					exchangeAppointment.Subject = exchangeAppointment.Subject.Substring(0, exchangeAppointment.Subject.IndexOf('(')).TrimEnd(' ');
				}
				row.SetField("cmfShortDescription", exchangeAppointment.Subject.Substring(0, Math.Min(exchangeAppointment.Subject.Length, 50)));
				if (row.Field<byte>("cmfFollowupType") == 1)
				{
					row.SetField("cmfMeetingLocation", string.IsNullOrWhiteSpace(exchangeAppointment.MeetingLocation) ? string.Empty : exchangeAppointment.MeetingLocation.Substring(0, Math.Min(exchangeAppointment.MeetingLocation.Length, 50)));
					row.SetField("cmfDueDate", exchangeAppointment.End);
					row.SetField("cmfStartDate", exchangeAppointment.Start);
				}
				else
				{
					row.SetField("cmfStatus", (byte)((exchangeAppointment.Status == TaskStatus.InProgress) ? 2u : ((exchangeAppointment.Status != TaskStatus.Completed) ? 1u : 3u)));
					TimeSpan timeSpan = ((row["cmfDueDate"] != DBNull.Value) ? row.Field<DateTime>("cmfDueDate").TimeOfDay : TimeSpan.Zero);
					row.SetField("cmfDueDate", exchangeAppointment.End);
					if (row["cmfDueDate"] != DBNull.Value)
					{
						row.SetField("cmfDueDate", row.Field<DateTime>("cmfDueDate").AddSeconds(timeSpan.TotalSeconds));
					}
					timeSpan = ((row["cmfStartDate"] != DBNull.Value) ? row.Field<DateTime>("cmfStartDate").TimeOfDay : TimeSpan.Zero);
					row.SetField("cmfStartDate", exchangeAppointment.Start);
					if (row["cmfStartDate"] != DBNull.Value)
					{
						row.SetField("cmfStartDate", row.Field<DateTime>("cmfStartDate").AddSeconds(timeSpan.TotalSeconds));
					}
				}
				row.SetField("cmfPriority", (byte)((exchangeAppointment.Importance == Importance.Low) ? 1u : ((exchangeAppointment.Importance == Importance.High) ? 3u : 2u)));
			}
			else
			{
				row["cmfExchangeID"] = DBNull.Value;
			}
		}
		database.UpdateData(dataTable, adapter);
	}

	public void ExportFollowUpToExchange(M1Database database, DataRow row, SqlTransaction sqlTransaction, bool bDelete = false)
	{
		ExchangeUtilities exchangeUtilities = new ExchangeUtilities();
		if (bDelete)
		{
			string text = row.Field<string>("cmfExchangeID", DataRowVersion.Original);
			if (string.IsNullOrWhiteSpace(text))
			{
				return;
			}
			ExchangeService serviceForEmployee = getServiceForEmployee(database, row.Field<string>("cmfAssignedToEmployeeID", DataRowVersion.Original), sqlTransaction);
			try
			{
				if (row.Field<byte>("cmfFollowupType", DataRowVersion.Original) == 1)
				{
					exchangeUtilities.DeleteAppointment(serviceForEmployee, text);
				}
				else
				{
					exchangeUtilities.DeleteTask(serviceForEmployee, text);
				}
			}
			catch
			{
			}
			if (row.RowState != DataRowState.Deleted)
			{
				row["cmfExchangeID"] = DBNull.Value;
			}
			return;
		}
		ExchangeAppointment exchangeAppointment = new ExchangeAppointment(row.Field<string>("cmfExchangeID"));
		exchangeAppointment.SourceUniqueID = row.Field<Guid>("cmfUniqueID");
		exchangeAppointment.Body = row.Field<string>("cmfLongDescriptionText");
		exchangeAppointment.Subject = row.Field<string>("cmfShortDescription") + getCompanyForFollowup(database, row.Field<string>("cmfOrganizationID"), row.Field<string>("cmfLocationID"), row.Field<string>("cmfContactID"), sqlTransaction);
		exchangeAppointment.MeetingLocation = row.Field<string>("cmfMeetingLocation");
		exchangeAppointment.End = row.Field<DateTime?>("cmfDueDate");
		exchangeAppointment.Start = row.Field<DateTime?>("cmfStartDate");
		switch (row.Field<byte>("cmfStatus"))
		{
		case 2:
			exchangeAppointment.Status = TaskStatus.InProgress;
			break;
		case 3:
			exchangeAppointment.Status = TaskStatus.Completed;
			break;
		default:
			exchangeAppointment.Status = TaskStatus.NotStarted;
			break;
		}
		if (exchangeAppointment.End.HasValue && exchangeAppointment.End.Value > DateTime.Now)
		{
			exchangeAppointment.ReminderDueBy = exchangeAppointment.End.Value.Date.AddHours(8.0);
		}
		exchangeAppointment.IsReminderSet = exchangeAppointment.Status != TaskStatus.Completed;
		switch (row.Field<byte>("cmfPriority"))
		{
		case 1:
			exchangeAppointment.Importance = Importance.Low;
			break;
		case 3:
			exchangeAppointment.Importance = Importance.High;
			break;
		default:
			exchangeAppointment.Importance = Importance.Normal;
			break;
		}
		List<ExchangeAppointment> list = new List<ExchangeAppointment>();
		list.Add(exchangeAppointment);
		ExchangeService serviceForEmployee2 = getServiceForEmployee(database, row.Field<string>("cmfAssignedToEmployeeID"), sqlTransaction);
		if (serviceForEmployee2.Url != null)
		{
			if (row.Field<byte>("cmfFollowupType") == 1)
			{
				Folder folder = exchangeUtilities.GetFolder(serviceForEmployee2, WellKnownFolderName.Calendar);
				exchangeUtilities.ExportAppointments(list, serviceForEmployee2, folder);
			}
			else
			{
				Folder folder = exchangeUtilities.GetFolder(serviceForEmployee2, WellKnownFolderName.Tasks);
				exchangeUtilities.ExportTasks(list, serviceForEmployee2, folder);
			}
		}
		row["cmfExchangeID"] = exchangeAppointment.ID;
	}

	private string getCompanyForFollowup(M1Database database, string orgID, string locID, string contactID, SqlTransaction sqlTransaction)
	{
		StringBuilder stringBuilder = new StringBuilder();
		SqlCommand sqlCommand = database.NewSqlCommand("select cmlName from OrganizationLocations Where cmlOrganizationID = @OrgID and cmlLocationID = @LocID");
		sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = orgID;
		sqlCommand.Parameters.Add(new SqlParameter("@LocID", SqlDbType.NVarChar)).Value = locID;
		string value = Convert.ToString(database.ExecuteScalar(sqlCommand, sqlTransaction));
		if (!string.IsNullOrWhiteSpace(value))
		{
			stringBuilder.Append(value);
		}
		if (!string.IsNullOrWhiteSpace(contactID))
		{
			sqlCommand = database.NewSqlCommand("select cmcName from OrganizationContacts Where cmcOrganizationID = @OrgID and cmcLocationID = @LocID And cmcContactID = @ContactID");
			sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = orgID;
			sqlCommand.Parameters.Add(new SqlParameter("@LocID", SqlDbType.NVarChar)).Value = locID;
			sqlCommand.Parameters.Add(new SqlParameter("@ContactID", SqlDbType.NVarChar)).Value = contactID;
			value = Convert.ToString(database.ExecuteScalar(sqlCommand, sqlTransaction));
			if (!string.IsNullOrWhiteSpace(value))
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(" - ");
				}
				stringBuilder.Append(value);
			}
		}
		if (stringBuilder.Length != 0)
		{
			return "(" + stringBuilder.ToString() + ")";
		}
		return string.Empty;
	}

	private ExchangeService getServiceForEmployee(M1Database database, string employeeID, SqlTransaction sqlTransaction)
	{
		ExchangeUtilities exchangeUtilities = new ExchangeUtilities();
		if (string.IsNullOrWhiteSpace(employeeID))
		{
			M1User m1User = database.GetService(typeof(M1User)) as M1User;
			return exchangeUtilities.GetExchangeService(database, m1User.ID, sqlTransaction);
		}
		SqlCommand sqlCommand = database.NewSqlCommand("Select lmeUserID From Employees Where lmeEmployeeID = @EmployeeID");
		sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar)).Value = employeeID;
		string text = Convert.ToString(database.ExecuteScalar(sqlCommand, sqlTransaction));
		if (string.IsNullOrWhiteSpace(text))
		{
			M1User m1User2 = database.GetService(typeof(M1User)) as M1User;
			return exchangeUtilities.GetExchangeService(database, m1User2.ID, sqlTransaction);
		}
		return exchangeUtilities.GetExchangeService(database, text, sqlTransaction);
	}
}
