using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using M1.Core.Report;

namespace M1.Core;

public class Reports
{
	private AppContext currentContext;

	public string Location
	{
		get
		{
			if (currentContext.IsHosted)
			{
				return currentContext.Metadata.FileShareLocation + "Reports\\";
			}
			return currentContext.Server.Location + "Reports\\";
		}
	}

	public event EventHandler<ReportAfterPrintEventArgs> AfterPrint;

	public Reports(AppContext context)
	{
		currentContext = context;
	}

	public string FormatReportName(string reportName, string prefix)
	{
		if (reportName.ToUpper().StartsWith(prefix.ToUpper()))
		{
			reportName = reportName.Substring(prefix.Length);
		}
		if (reportName.ToUpper().EndsWith(".RPT"))
		{
			reportName = reportName.Substring(0, reportName.Length - 4);
		}
		return reportName.Replace("_", " ");
	}

	public List<string> GetReportFolders()
	{
		List<string> list = new List<string>();
		DirectoryInfo[] directories = new DirectoryInfo(Location).GetDirectories("*.*");
		foreach (DirectoryInfo directoryInfo in directories)
		{
			list.Add(directoryInfo.Name);
		}
		return list;
	}

	public List<FileInfo> GetReportsForTemplate(string folderName, string fileExtensions)
	{
		List<FileInfo> list = new List<FileInfo>();
		folderName = folderName.Trim();
		if (folderName.Length != 0)
		{
			int num = folderName.IndexOf('\\');
			if (num > 0)
			{
				folderName = folderName.Substring(0, num);
			}
			if (folderName.EndsWith(".RPT", StringComparison.CurrentCultureIgnoreCase))
			{
				folderName = folderName.Remove(folderName.Length - 4);
			}
			if (fileExtensions.Length == 0)
			{
				fileExtensions = "*.RPT";
			}
			if (Directory.Exists(Location + folderName + "\\"))
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(Location + folderName + "\\");
				string[] array = fileExtensions.Split('|');
				string simpleExt;
				foreach (string text in array)
				{
					simpleExt = text.Replace("*", "");
					list.AddRange(from s in directoryInfo.GetFiles(text)
						where s.Extension.EndsWith(simpleExt, StringComparison.CurrentCultureIgnoreCase)
						select s);
				}
			}
		}
		return list;
	}

	public string[] OnAfterPrint(ReportProxy report, object provider, string whereClause, string outputType)
	{
		EventHandler<ReportAfterPrintEventArgs> eventHandler = this.AfterPrint;
		if (eventHandler != null)
		{
			ReportAfterPrintEventArgs e = new ReportAfterPrintEventArgs(report, (IServiceProvider)provider, whereClause, outputType);
			eventHandler(this, e);
			return e.Files.ToArray();
		}
		return new string[0];
	}

	public CrystalParameterCollection CreateParameterCollection()
	{
		return new CrystalParameterCollection();
	}

	public ReportProxy OpenReport(IServiceProvider provider, string reportName, string defaultValues)
	{
		return ReportLoaderNew.OpenReport(provider, reportName, defaultValues);
	}

	private string checkForDefault(M1Database database, string reportName)
	{
		if (reportName.IndexOf('\\') == -1)
		{
			string text = reportName;
			string defaultReportForFolder = database.GetDefaultReportForFolder(text);
			reportName = ((!string.IsNullOrWhiteSpace(defaultReportForFolder)) ? (text + "\\" + defaultReportForFolder) : (text + "\\" + text));
		}
		return reportName;
	}

	private bool DoReportCheck(M1Database database, string reportName)
	{
		string folder = string.Empty;
		int num = reportName.IndexOf('\\');
		if (num != -1)
		{
			folder = reportName.Substring(0, num);
			reportName = reportName.Substring(num + 1);
		}
		if (database.Security.GetReportAccessLevel(folder, reportName) == SecurityAccessLevel.None)
		{
			return false;
		}
		return true;
	}

	private bool doReportModuleCheck(string reportName, M1Database database)
	{
		reportName = reportName.Trim();
		if (reportName.Length > 2)
		{
			M1DataDictionary m1DataDictionary = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
			bool result = true;
			string text = reportName.Substring(0, 2);
			if (m1DataDictionary.ProductCode.AllModules.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 && !m1DataDictionary.ProductCode.IsModulePurchased(text, database))
			{
				string text2 = reportName;
				int num = reportName.IndexOf('\\');
				if (num != -1)
				{
					text2 = reportName.Substring(0, num);
				}
				if (!text2.Equals("PA_PAYROLL_TIMECARD_HOURS", StringComparison.CurrentCultureIgnoreCase))
				{
					result = false;
				}
			}
			return result;
		}
		return false;
	}

	public bool IsCustomReport(string reportName, string reportFolder)
	{
		if (reportFolder.StartsWith("CR_", StringComparison.CurrentCultureIgnoreCase) || reportFolder.StartsWith("CX_", StringComparison.CurrentCultureIgnoreCase))
		{
			return true;
		}
		if (!reportName.Equals(reportFolder, StringComparison.CurrentCultureIgnoreCase))
		{
			switch (reportName.ToUpper())
			{
			case "AR_STATEMENT_PRINT_DETAIL":
			case "GL_TRIAL_BALANCE_MOVEMENT":
			case "PM_PO_PRINT_GROUP_BY_PART":
			case "AP_PAYMENT_STUB_CHECK_STUB_PRINT":
			case "AP_PAYMENT_STUB_STUB_CHECK_PRINT":
			case "AP_PAYMENT_CHECK_STUB_STUB_PRINT":
			case "PA_PAYG_SUMMARY_2009":
			case "AR_INVOICE_PRINT_WEB":
			case "PS_SALE_RECEIPT_TAPE":
			case "PA_PAYG_SUMMARY_2010":
			case "PA_PAYG_SUMMARY_2007":
			case "GL_BALANCE_SHEET_LANDSCAPE":
			case "RQ_RFQ_PRINT_GROUP_BY_PART":
			case "HD_KB_PRINT_WEB":
			case "PM_PO_PRINT_WEB":
			case "PA_W2_PRINT_2004":
			case "PA_FORM_941_2009":
			case "PA_W2_PRINT_2009":
			case "PA_W3_PRINT 2009":
			case "PA_W2_PRINT_2007":
			case "PA_W2_PRINT_2008":
			case "PA_W3_PRINT 2010":
			case "SM_SHIPMENT_PRINT_GROUP_BY_PART":
			case "GL_INCOME_STATEMENT_LANDSCAPE":
			case "AR_INVOICE_PRINT_GROUP_BY_PART":
			case "HD_CALL_PRINT_WEB":
			case "OM_ORDER_ACKNOWLEDGMENT_WEB":
			case "QM_QUOTE_PRINT_WEB":
			case "SM_SHIPMENT_PRINT_WEB":
			case "IM_PART_LABELS_INV_COUNT":
			case "IM_PART_WHSE_BIN_LABELS":
			case "IM_INV_COUNT_WORKSHEET_WITH_LOT_NUMBERS":
				return false;
			default:
				return true;
			}
		}
		return false;
	}
}
