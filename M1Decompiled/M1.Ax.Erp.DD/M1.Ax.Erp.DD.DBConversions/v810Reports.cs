using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using M1.Core;
using M1RepBitConv;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("8.10.058", "Convert reports for bit field changes", "2013-10-31")]
public class v810Reports
{
	public v810Reports(DBConversionParms parms)
	{
		if (parms != null && parms.DatabaseName.Equals("M1_M1", StringComparison.CurrentCultureIgnoreCase))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, "M1_M1", "ALTER DATABASE M1_M1 Set MULTI_USER");
			Execute(parms);
		}
	}

	public void Execute(DBConversionParms parms)
	{
		string cBackupFolder = Path.Combine(parms.User.Context.Reports.Location, "Backup\\");
		List<string> list = new List<string>();
		foreach (string reportFolder in parms.User.Context.Reports.GetReportFolders())
		{
			if (reportFolder.Equals("Backup", StringComparison.CurrentCultureIgnoreCase))
			{
				continue;
			}
			foreach (FileInfo item in parms.User.Context.Reports.GetReportsForTemplate(reportFolder, string.Empty))
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(item.Name);
				if (parms.User.Context.Reports.IsCustomReport(fileNameWithoutExtension, reportFolder))
				{
					list.Add(Path.Combine(parms.User.Context.Reports.Location, reportFolder, item.Name));
				}
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		List<string> list2 = new List<string>();
		foreach (DataRow row in parms.DataDictionary.GetDataTable("Select dfField From DDFields Where dfDBType = 'bit' Order By dfTable,dfField").Rows)
		{
			list2.Add(row.Field<string>("dfField"));
		}
		if (list2.Count == 0)
		{
			return;
		}
		App2 app = (App2)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("AFB98190-A4AE-4146-A34C-2D63367A0AA1")));
		app.SetLogon(parms.ServerManager.ConnectionInfo.Server, parms.ServerManager.ConnectionInfo.SqlUserID, parms.ServerManager.sqlPassword, parms.ServerManager.ConnectionInfo.TrustedConnection);
		list2.ToArray();
		string[] parmChangeSource = new string[14]
		{
			"App.UserPlantID", "App.QuoteFunctions", "App.GetYear", "App.GetPeriod", "App.JobFunctions", "App.ConvertDateToSql", "App.PartFunctions", "App.AddQuotes", "App.ConvertDateTimeToSql", "App.ConvertDateToSQL",
			"App.ConvertNumberToSql", "App.ConvertUnknownToSQL", "App.OpenSearch", "App.OpenObject"
		};
		string[] parmChangeDest = new string[14]
		{
			"App.Ax(\"Production\").PlantID", "App.Ax(\"Quote\")", "App.Ax(\"Financial\").GetYear", "App.Ax(\"Financial\").GetPeriod", "App.Ax(\"JobFunctions\")", "App.Convert.DateToSql", "App.Ax(\"PartFunctions\")", "App.Convert.StringToSql", "App.Convert.DateTimeToSql", "App.Convert.DateToSql",
			"App.Convert.NumberToSql", "App.Convert.ToSql", "Forms.Show.Search", "Forms.OpenObject"
		};
		AddFieldsAndChangesList(app, list2, parmChangeSource, parmChangeDest);
		foreach (string item2 in list)
		{
			parms.OnStatusUpdated(new DBConversionStatusUpdatedEventArgs("Updating report " + item2));
			string text = app.UpdateReport(item2, cBackupFolder);
			if (!string.IsNullOrWhiteSpace(text))
			{
				parms.Messages.Add(text);
				app = (App2)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("AFB98190-A4AE-4146-A34C-2D63367A0AA1")));
				app.SetLogon(parms.ServerManager.ConnectionInfo.Server, parms.ServerManager.ConnectionInfo.SqlUserID, parms.ServerManager.sqlPassword, parms.ServerManager.ConnectionInfo.TrustedConnection);
				AddFieldsAndChangesList(app, list2, parmChangeSource, parmChangeDest);
			}
		}
		try
		{
			app.Shutdown();
		}
		catch (Exception)
		{
		}
		app = null;
	}

	private static void AddFieldsAndChangesList(App2 repConv, List<string> bitFieldsList, string[] parmChangeSource, string[] parmChangeDest)
	{
		foreach (string bitFields in bitFieldsList)
		{
			repConv.AddBitField(bitFields);
		}
		string[] array = parmChangeDest;
		foreach (string dest in array)
		{
			repConv.AddDest(dest);
		}
		array = parmChangeSource;
		foreach (string source in array)
		{
			repConv.AddSource(source);
		}
	}
}
