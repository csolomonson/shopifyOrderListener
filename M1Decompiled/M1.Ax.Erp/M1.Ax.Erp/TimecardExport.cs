using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Core;
using Microsoft.Exchange.WebServices.Data;

namespace M1.Ax.Erp;

public class TimecardExport
{
	public void ExportTimecardsForEmployee(M1Database database, string employeeID, ExchangeService service, Folder exchangeFolder)
	{
		List<ExchangeAppointment> appointments = ProcessEmployeeData(database, employeeID);
		ExchangeUtilities exchangeUtilities = new ExchangeUtilities();
		exchangeUtilities.ExportAppointments(appointments, service, exchangeFolder);
		exchangeUtilities.RefreshExchangeIDs(appointments, database, "Timecards", "lmpExchangeID", "lmpUniqueID");
	}

	private List<ExchangeAppointment> ProcessEmployeeData(M1Database database, string employeeID)
	{
		List<ExchangeAppointment> list = new List<ExchangeAppointment>();
		SqlCommand sqlCommand = database.NewSqlCommand("select * from Timecards Inner Join Employees On lmpEmployeeID = lmeEmployeeID Inner Join LeaveAccruals On lmpLeaveAccrualID = pajLeaveAccrualID Inner Join WorkCenters On lmeDefaultWorkCenterID = xawWorkCenterID Where lmpEmployeeID = @EmployeeID and lmpLeaveAccrualID <> '' And lmpTransferredToPayroll = 0 And lmeDefaultWorkCenterID <> '' and xawExportToCalendar <> 0 order by lmpActualStartTime");
		sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar)).Value = employeeID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		string text = database.Props("PN").Field<string>("xapPACalendarExportFields");
		if (string.IsNullOrWhiteSpace(text))
		{
			text = "lmeEmployeeName,pajLeaveDescription";
		}
		database.GetService(typeof(M1DataDictionary));
		Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
		string[] array = text.Split(',');
		foreach (string text2 in array)
		{
			if (text2.Trim().Length != 0 && !dictionary.ContainsKey(text2))
			{
				dictionary.Add(text2, string.Empty);
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DataRow row in dataTable.Rows)
		{
			ExchangeAppointment exchangeAppointment = new ExchangeAppointment(row.Field<string>("lmpExchangeID"));
			exchangeAppointment.SourceUniqueID = row.Field<Guid>("lmpUniqueID");
			stringBuilder.Length = 0;
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(row[item.Key].ToString().Trim());
			}
			exchangeAppointment.Subject = stringBuilder.ToString();
			if (row["lmpActualStartTime"] == DBNull.Value || row["lmpActualEndTime"] == DBNull.Value)
			{
				exchangeAppointment.Start = row.Field<DateTime>("lmpTimecardDate");
				exchangeAppointment.End = row.Field<DateTime>("lmpTimecardDate");
			}
			else
			{
				exchangeAppointment.Start = row.Field<DateTime>("lmpActualStartTime");
				exchangeAppointment.End = row.Field<DateTime>("lmpActualEndTime");
			}
			if (exchangeAppointment.Start > exchangeAppointment.End)
			{
				exchangeAppointment.Start = exchangeAppointment.End;
			}
			list.Add(exchangeAppointment);
		}
		return list;
	}
}
