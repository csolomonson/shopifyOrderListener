using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;
using Microsoft.Exchange.WebServices.Data;

namespace M1.Ax.Erp;

[AxScript("Employee")]
[ComVisible(true)]
public class AppAxEmployee
{
	private IServiceProvider provider;

	public AppAxEmployee(IServiceProvider parentProvider)
	{
		provider = parentProvider;
	}

	public int SendEmployeeMessage(string cToList, string cSubject, string cMessageText, string cMessageRTF = "")
	{
		return new Employee().SendEmployeeMessage(provider.GetService(typeof(M1Database)) as M1Database, cToList.Trim(), cSubject, cMessageText, cMessageRTF).SentCount;
	}

	public void ExportTimecardsToExchange(string employeeID)
	{
		ExchangeUtilities exchangeUtilities = new ExchangeUtilities();
		TimecardExport timecardExport = new TimecardExport();
		M1Database m1Database = provider.GetService(typeof(M1Database)) as M1Database;
		SqlCommand sqlCommand = m1Database.NewSqlCommand("Select xawCalendarLocation From Employees Inner Join WorkCenters On lmeDefaultWorkCenterID = xawWorkCenterID Where lmeEmployeeID = @EmployeeID And lmeDefaultWorkCenterID <> '' and xawExportToCalendar <> 0");
		sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.VarChar)).Value = employeeID;
		string text = Convert.ToString(m1Database.ExecuteScalar(sqlCommand));
		if (!string.IsNullOrEmpty(text))
		{
			ExchangeService exchangeService = exchangeUtilities.GetExchangeService(m1Database);
			Folder publicFolderByPath = exchangeUtilities.GetPublicFolderByPath(exchangeService, text);
			timecardExport.ExportTimecardsForEmployee(m1Database, employeeID, exchangeService, publicFolderByPath);
		}
	}
}
