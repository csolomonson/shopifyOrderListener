using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ADODB;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportAppServer.ClientDoc;
using CrystalDecisions.ReportAppServer.Controllers;
using CrystalDecisions.ReportAppServer.DataDefModel;
using CrystalDecisions.ReportAppServer.DataSetConversion;
using CrystalDecisions.ReportAppServer.ReportDefModel;
using CrystalDecisions.Shared;
using M1.Core.Script;
using M1.Extensions;
using M1.Script.Interfaces;

namespace M1.Core.Report;

public static class ReportLoaderNew
{
	public static ReportProxy OpenReport(IServiceProvider provider, string reportName, string defaultValues)
	{
		string text = Path.Combine((provider.GetService(typeof(AppContext)) as AppContext).Reports.Location, reportName);
		if (!Path.HasExtension(text))
		{
			text = Path.ChangeExtension(text, "rpt");
		}
		ReportProxy reportProxy = new ReportProxy(text);
		ReportDocument reportDocument = new ReportDocument();
		reportDocument.Load(text);
		loadCrystalParameters(reportProxy, reportDocument.ReportClientDocument);
		reportProxy.ReportComments = reportDocument.ReportClientDocument.SummaryInfo.Comments;
		reportProxy.ReportTitle = reportDocument.ReportClientDocument.SummaryInfo.Title;
		LoadDataForParameters(provider, reportProxy);
		reportProxy.Addresses = loadAllPossibleAddressDefinitions(reportProxy, provider);
		LoadDataForSpecialParameters(provider, reportProxy);
		InitializeValues(provider, reportProxy);
		foreach (ReportAddressDefinition address in reportProxy.Addresses)
		{
			checkAddressFields(address, reportDocument.ReportClientDocument);
		}
		loadTableSourcesForReport(reportProxy.TableSources, reportDocument, Path.GetDirectoryName(text));
		reportProxy.OriginalSelectionFormula = reportDocument.RecordSelectionFormula;
		reportProxy.CrystalRefNew = reportDocument;
		SetParametersFromExpression(provider, reportProxy, defaultValues);
		return reportProxy;
	}

	public static void SaveParametersDataReport(IServiceProvider provider, ReportProxy report)
	{
		SaveParameterValues(provider, report);
	}

	public static void ProcessDataForReport(IServiceProvider provider, ReportProxy report)
	{
		M1Database m1Database = provider.GetService(typeof(M1Database)) as M1Database;
		M1DataDictionary m1DataDictionary = provider.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		SetReportFromToParameters(m1Database, report.Parameters, isExport: false);
		if (CheckIfParametersAreValid(report).Count != 0)
		{
			throw new M1Exception($"Report {report.ReportName} has invalid or empty parameters.");
		}
		ReportDocument crystalRefNew = report.CrystalRefNew;
		ReportWhere reportWhere = (report.FilterInfo = GetWhereClause(report.Parameters, crystalRefNew.ReportClientDocument.DatabaseController.Database, m1Database, -1, string.Empty));
		if (!string.IsNullOrWhiteSpace(report.OnRunCommand))
		{
			processCommand(provider, report.FilterInfo.SqlWhere, report.OnRunCommand);
		}
		if (crystalRefNew.HasSavedData)
		{
			crystalRefNew.ReportClientDocument.RowsetController.Refresh();
		}
		bool flag = report.Parameters.Contains("ConnectionInfo") && report.Parameters["ConnectionInfo"].Text.Equals("DataDictionary", StringComparison.CurrentCultureIgnoreCase);
		if (!report.IsVerified)
		{
			if (processLogin(crystalRefNew, report.Parameters, m1Database.GetService(typeof(AppContext)) as AppContext, flag ? m1DataDictionary.ID : m1Database.ID))
			{
				report.SelectionFormulaNeedsSetting = true;
			}
			FilterReportForBreakCondition(provider, report, null, isExport: false);
			if (report.SqlExpressionsWithParameters == null || report.SqlExpressionsWithParameters.Count == 0)
			{
				report.SqlExpressionsWithParameters = processSqlExpressions(crystalRefNew.ReportClientDocument, m1Database);
			}
			if (!report.IsVerified)
			{
				try
				{
					crystalRefNew.ReportClientDocument.VerifyDatabase();
				}
				catch (Exception ex)
				{
					if (ex.Message.IndexOf("No error.", StringComparison.CurrentCultureIgnoreCase) == -1)
					{
						throw;
					}
				}
				report.IsVerified = true;
			}
		}
		else
		{
			FilterReportForBreakCondition(provider, report, null, isExport: false);
		}
		if (report.SelectionFormulaNeedsSetting)
		{
			SetSelectionFormulas(report, reportWhere.CrystalWhere);
		}
		SaveParameterValues(provider, report);
		SetReportParameters(m1Database, crystalRefNew, report.Parameters, isExport: false);
		checkSqlExpressions(m1Database, report.SqlExpressionsWithParameters, report.Parameters, reportWhere.SqlWhere);
		RowsetCursor rowsetCursor = crystalRefNew.ReportClientDocument.RowsetController.CreateCursor(null);
		report.TotalRecordCount = rowsetCursor.Rowset.TotalRecordCount;
		report.Alerts = checkForAlerts(crystalRefNew.ReportClientDocument);
		checkForContactGroups(report, rowsetCursor, m1Database);
	}

	public static void SetParametersFromExpression(IServiceProvider provider, ReportProxy report, string defaultValues)
	{
		if (string.IsNullOrWhiteSpace(defaultValues))
		{
			return;
		}
		string text = "Field";
		using ScriptingBase scriptingBase = new ScriptingBase(provider);
		foreach (string item in M1Util.ParseFieldList(defaultValues, ','))
		{
			int num = item.IndexOf('=');
			if (num != -1)
			{
				string text2 = item.Substring(0, num).Replace("{?", "").Replace("}", "")
					.Trim();
				string code = item.Substring(num + 1).Trim();
				int num2 = 1;
				num = text2.IndexOf('.');
				if (num != -1)
				{
					string text3 = text2.Substring(num + 1);
					text2 = text2.Substring(0, num);
					num2 = Convert.ToInt16(text3.Substring(text.Length));
				}
				if (report.Parameters.Contains(text2))
				{
					CrystalParameter crystalParameter = report.Parameters[text2];
					crystalParameter.Data.FieldOptions[num2 - 1].Values.Clear();
					crystalParameter.Data.FieldOptions[num2 - 1].Values.Add(new object[1] { scriptingBase.Eval(code) });
				}
			}
		}
	}

	public static void FilterReportForBreakCondition(IServiceProvider provider, ReportProxy report, ReportAddress address, bool isExport)
	{
		M1Database m1Database = provider.GetService(typeof(M1Database)) as M1Database;
		attachDataToDb(report.Parameters, report.TableSources, report.CrystalRefNew.ReportClientDocument.SubreportController, report.CrystalRefNew.ReportClientDocument.DatabaseController.Database, report.CrystalRefNew.Database, report.CrystalRefNew, m1Database, getReportAddressSql(report, address));
		if (address == null)
		{
			report.CrystalRefNew.ReportClientDocument.DataDefController.SavedDataFilterController.SetFormulaText(string.Empty);
		}
		else
		{
			report.CrystalRefNew.ReportClientDocument.DataDefController.SavedDataFilterController.SetFormulaText(address.CrystalFilter);
		}
		Strings subreportNames = report.CrystalRefNew.ReportClientDocument.SubreportController.GetSubreportNames();
		for (int i = 0; i < report.CrystalRefNew.Subreports.Count; i++)
		{
			SubreportClientDocument subreport = report.CrystalRefNew.ReportClientDocument.SubreportController.GetSubreport(subreportNames[i]);
			attachDataToDb(report.Parameters, report.TableSources, report.CrystalRefNew.ReportClientDocument.SubreportController, report.CrystalRefNew.ReportClientDocument.SubreportController.GetSubreportDatabase(subreportNames[i]), report.CrystalRefNew.Subreports[subreportNames[i]].Database, report.CrystalRefNew.Subreports[subreportNames[i]], m1Database, getReportAddressSql(report, address));
			for (int j = 0; j < subreport.DataDefController.DataDefinition.ParameterFields.Count; j++)
			{
				if (subreport.DataDefController.DataDefinition.ParameterFields[j].Name.Equals("Flag_AddFilterToReport", StringComparison.CurrentCultureIgnoreCase))
				{
					if (address == null)
					{
						subreport.DataDefController.SavedDataFilterController.SetFormulaText(string.Empty);
					}
					else
					{
						subreport.DataDefController.SavedDataFilterController.SetFormulaText(address.CrystalFilter);
					}
					break;
				}
			}
			subreport = null;
		}
		SetReportParameters(m1Database, report.CrystalRefNew, report.Parameters, isExport);
	}

	private static void SetSelectionFormulas(ReportProxy report, string crystalWhere)
	{
		FilterController recordFilterController = report.CrystalRefNew.ReportClientDocument.DataDefController.RecordFilterController;
		if (string.IsNullOrWhiteSpace(report.OriginalSelectionFormula))
		{
			recordFilterController.SetFormulaText(crystalWhere);
		}
		else if (string.IsNullOrWhiteSpace(crystalWhere))
		{
			recordFilterController.SetFormulaText(report.OriginalSelectionFormula);
		}
		else
		{
			recordFilterController.SetFormulaText("(" + crystalWhere + ") And (" + report.OriginalSelectionFormula + ")");
		}
		Strings subreportNames = report.CrystalRefNew.ReportClientDocument.SubreportController.GetSubreportNames();
		for (int i = 0; i < report.CrystalRefNew.Subreports.Count; i++)
		{
			SubreportClientDocument subreport = report.CrystalRefNew.ReportClientDocument.SubreportController.GetSubreport(subreportNames[i]);
			for (int j = 0; j < subreport.DataDefController.DataDefinition.ParameterFields.Count; j++)
			{
				if (subreport.DataDefController.DataDefinition.ParameterFields[j].Name.Equals("Flag_AddFilterToReport", StringComparison.CurrentCultureIgnoreCase))
				{
					recordFilterController = subreport.DataDefController.RecordFilterController;
					if (string.IsNullOrWhiteSpace(report.OriginalSelectionFormula))
					{
						recordFilterController.SetFormulaText(crystalWhere);
						break;
					}
					if (string.IsNullOrWhiteSpace(crystalWhere))
					{
						recordFilterController.SetFormulaText(report.OriginalSelectionFormula);
						break;
					}
					recordFilterController.SetFormulaText("(" + crystalWhere + ") And (" + report.OriginalSelectionFormula + ")");
					break;
				}
			}
			subreport = null;
		}
	}

	private static void setTrayValue(PrinterSettings printerSettings, ReportProxy report)
	{
		System.Drawing.Printing.PaperSource paperSource = null;
		string value = report.PrintOptions.Tray.Trim();
		foreach (System.Drawing.Printing.PaperSource paperSource2 in printerSettings.PaperSources)
		{
			if (paperSource2.SourceName.Trim().Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				paperSource = paperSource2;
				break;
			}
		}
		if (paperSource != null)
		{
			report.CrystalRefNew.PrintOptions.CustomPaperSource = paperSource;
		}
		else
		{
			report.CrystalRefNew.PrintOptions.PaperSource = CrystalDecisions.Shared.PaperSource.Auto;
		}
	}

	private static void setDuplexValue(PrinterSettings printerSettings, ReportProxy report)
	{
		switch (report.PrintOptions.Duplex)
		{
		case 0:
		case 1:
			report.CrystalRefNew.PrintOptions.PrinterDuplex = (PrinterDuplex)report.PrintOptions.Duplex;
			break;
		case 2:
		case 3:
			report.CrystalRefNew.PrintOptions.PrinterDuplex = (printerSettings.CanDuplex ? ((PrinterDuplex)report.PrintOptions.Duplex) : PrinterDuplex.Default);
			break;
		default:
			report.CrystalRefNew.PrintOptions.PrinterDuplex = PrinterDuplex.Default;
			break;
		}
	}

	private static void setPaperSize(PrinterSettings printerSettings, ReportProxy report)
	{
		if (printerSettings.PaperSizes.Count != 0 && Enum.IsDefined(typeof(CrystalDecisions.Shared.PaperSize), printerSettings.DefaultPageSettings.PaperSize.RawKind))
		{
			report.CrystalRefNew.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)printerSettings.DefaultPageSettings.PaperSize.RawKind;
		}
	}

	public static void Print(ReportProxy report)
	{
		report.CrystalRefNew.PrintOptions.NoPrinter = false;
		report.CrystalRefNew.PrintOptions.PrinterName = report.PrintOptions.PrinterName;
		report.CrystalRefNew.PrintOptions.DissociatePageSizeAndPrinterPaperSize = true;
		PrinterSettings printerSettings = new PrinterSettings
		{
			PrinterName = report.PrintOptions.PrinterName
		};
		setTrayValue(printerSettings, report);
		setDuplexValue(printerSettings, report);
		setPaperSize(printerSettings, report);
		report.CrystalRefNew.PrintToPrinter(report.PrintOptions.Copies, report.PrintOptions.Collate, report.PrintOptions.StartPage, (report.PrintOptions.EndPage == 0) ? (-1) : report.PrintOptions.EndPage);
	}

	public static void Print(ReportProxy report, PrinterSettings printerSettings)
	{
		report.CrystalRefNew.PrintOptions.NoPrinter = false;
		report.CrystalRefNew.PrintOptions.PrinterName = printerSettings.PrinterName;
		report.CrystalRefNew.PrintOptions.DissociatePageSizeAndPrinterPaperSize = true;
		PrintLayoutSettings printLayoutSettings = new PrintLayoutSettings();
		printLayoutSettings.Scaling = PrintLayoutSettings.PrintScaling.Scale;
		report.CrystalRefNew.PrintToPrinter(printerSettings, printerSettings.DefaultPageSettings, reformatReportPageSettings: false, printLayoutSettings);
	}

	public static System.IO.Stream ExportToStream(IServiceProvider provider, ReportProxy report)
	{
		return ExportToStream(provider, report, null);
	}

	public static System.IO.Stream ExportToStream(IServiceProvider provider, ReportProxy report, ReportAddress address)
	{
		FilterReportForBreakCondition(provider, report, address, isExport: true);
		try
		{
			return report.CrystalRefNew.ExportToStream(ExportFormatType.PortableDocFormat);
		}
		catch (Exception ex)
		{
			if (ex.Message.IndexOf("No error.", StringComparison.CurrentCultureIgnoreCase) != -1)
			{
				return report.CrystalRefNew.ExportToStream(ExportFormatType.PortableDocFormat);
			}
			throw;
		}
	}

	public static void ExportToFile(IServiceProvider provider, ReportProxy report, ReportAddress address, string file, string format)
	{
		FilterReportForBreakCondition(provider, report, address, isExport: true);
		ExportFormatType formatType = (format.Equals("Word", StringComparison.CurrentCultureIgnoreCase) ? ExportFormatType.WordForWindows : (format.Equals("ExcelForm", StringComparison.CurrentCultureIgnoreCase) ? ExportFormatType.Excel : ((!format.Equals("Excel", StringComparison.CurrentCultureIgnoreCase)) ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord)));
		try
		{
			report.CrystalRefNew.ExportToDisk(formatType, file);
		}
		catch (Exception ex)
		{
			if (ex.Message.IndexOf("No error.", StringComparison.CurrentCultureIgnoreCase) != -1)
			{
				report.CrystalRefNew.ExportToDisk(formatType, file);
				return;
			}
			throw;
		}
	}

	private static List<string> checkForAlerts(ISCDReportClientDocument reportDoc)
	{
		Alerts triggeredAlerts = reportDoc.SearchController.GetTriggeredAlerts();
		List<string> list = new List<string>();
		foreach (Alert item in triggeredAlerts)
		{
			if (item.Enable)
			{
				list.Add(item.Message);
			}
		}
		return list;
	}

	private static void SetReportFromToParameters(IServiceProvider provider, CrystalParameterCollection parameters, bool isExport)
	{
		M1Database m1Database = provider.GetService(typeof(M1Database)) as M1Database;
		foreach (CrystalParameter parameter in parameters)
		{
			string name = parameter.Name;
			if (parameter.Name.StartsWith("FromValue_", StringComparison.CurrentCultureIgnoreCase))
			{
				if (parameter.Data.Fields.Count == 0)
				{
					parameter.Data.InstanceCount = 1;
					parameter.Data.Fields.Add(new ReportPromptFieldInfo());
				}
				object obj = checkFromToValueParameter(parameters, m1Database, parameter.Name.Substring(10), 0, parameter.Data.Fields[0]);
				parameter.Data.ClearValues();
				if (obj != null)
				{
					parameter.Data.AddValues(0, new object[1] { obj });
				}
			}
			else if (parameter.Name.StartsWith("ToValue_", StringComparison.CurrentCultureIgnoreCase))
			{
				if (parameter.Data.Fields.Count == 0)
				{
					parameter.Data.InstanceCount = 1;
					parameter.Data.Fields.Add(new ReportPromptFieldInfo());
				}
				object obj2 = checkFromToValueParameter(parameters, m1Database, parameter.Name.Substring(8), 1, parameter.Data.Fields[0]);
				parameter.Data.ClearValues();
				if (obj2 != null)
				{
					parameter.Data.AddValues(0, new object[1] { obj2 });
				}
			}
			else if (name.Equals("UserID", StringComparison.CurrentCultureIgnoreCase))
			{
				if (parameter.Data.Fields.Count == 0)
				{
					parameter.Data.InstanceCount = 1;
					parameter.Data.Fields.Add(new ReportPromptFieldInfo());
				}
				parameter.Data.ClearValues();
				parameter.Data.AddValues(0, new object[1] { ((M1User)provider.GetService(typeof(M1User))).ID });
			}
			else if (name.Equals("Region", StringComparison.CurrentCultureIgnoreCase))
			{
				if (parameter.Data.Fields.Count == 0)
				{
					parameter.Data.InstanceCount = 1;
					parameter.Data.Fields.Add(new ReportPromptFieldInfo());
				}
				parameter.Data.ClearValues();
				parameter.Data.AddValues(0, new object[1] { m1Database.Region });
			}
			else if (name.StartsWith("SecurityModuleValue_", StringComparison.CurrentCultureIgnoreCase))
			{
				M1DataDictionary m1DataDictionary = (M1DataDictionary)provider.GetService(typeof(M1DataDictionary));
				if (parameter.Data.Fields.Count == 0)
				{
					parameter.Data.InstanceCount = 1;
					parameter.Data.Fields.Add(new ReportPromptFieldInfo());
				}
				parameter.Data.ClearValues();
				parameter.Data.AddValues(0, new object[1] { m1DataDictionary.ProductCode.IsModulePurchased(name.Substring(20), m1Database) });
			}
			else if (name.StartsWith("App_", StringComparison.CurrentCultureIgnoreCase))
			{
				object obj3;
				if (name.Equals("App_HomeCurrencySymbol", StringComparison.CurrentCultureIgnoreCase))
				{
					obj3 = ((!string.IsNullOrWhiteSpace(m1Database.HomeCurrencySymbol)) ? m1Database.HomeCurrencySymbol : m1Database.SystemCurrencySymbol);
				}
				else
				{
					ScriptApp scriptApp = provider.GetService(typeof(ScriptApp)) as ScriptApp;
					obj3 = scriptApp.GetType().GetProperty(name.Substring(4)).GetValue(scriptApp, null);
				}
				if (parameter.Data.Fields.Count == 0)
				{
					parameter.Data.InstanceCount = 1;
					parameter.Data.Fields.Add(new ReportPromptFieldInfo());
				}
				parameter.Data.ClearValues();
				parameter.Data.AddValues(0, new object[1] { obj3 });
			}
			else if (name.StartsWith("Props_", StringComparison.CurrentCultureIgnoreCase) || name.StartsWith("Constants_", StringComparison.CurrentCultureIgnoreCase))
			{
				string[] array = name.Split('_');
				string empty = string.Empty;
				if (array.Length == 3)
				{
					empty = translateOldFieldName(array[2]);
					if (parameter.Data.Fields.Count == 0)
					{
						parameter.Data.InstanceCount = 1;
						parameter.Data.Fields.Add(new ReportPromptFieldInfo());
					}
					parameter.Data.ClearValues();
					object obj4 = ((!name.Equals("Props_Pn_xapQMNumberOfDecimals", StringComparison.CurrentCultureIgnoreCase)) ? m1Database.Props(array[1])[empty] : ((object)5));
					if (obj4 == DBNull.Value && m1Database.Props(array[1]).Table.Columns[empty].DataType == typeof(string))
					{
						obj4 = string.Empty;
					}
					if (obj4.GetType() == typeof(byte))
					{
						parameter.Data.AddValues(0, new object[1] { Convert.ToInt16(obj4) });
					}
					else
					{
						parameter.Data.AddValues(0, new object[1] { obj4 });
					}
				}
			}
			else
			{
				if (!name.StartsWith("DsInfo_", StringComparison.CurrentCultureIgnoreCase) && !name.StartsWith("DatasetProperties_", StringComparison.CurrentCultureIgnoreCase))
				{
					continue;
				}
				string[] array2 = name.Split('_');
				string empty2 = string.Empty;
				if (array2.Length == 2)
				{
					empty2 = translateOldFieldName(array2[1]);
					if (parameter.Data.Fields.Count == 0)
					{
						parameter.Data.InstanceCount = 1;
						parameter.Data.Fields.Add(new ReportPromptFieldInfo());
					}
					parameter.Data.ClearValues();
					object obj5 = m1Database.Props("DatasetProperties")[empty2];
					parameter.Data.AddValues(0, new object[1] { obj5 });
				}
			}
		}
	}

	private static string translateOldFieldName(string oldFieldName)
	{
		string result = oldFieldName;
		string[] array = new string[219]
		{
			"xqburm", "xqfdys", "xqftt2", "xqfttx", "xqlabm", "xqmtlm", "xqqtm", "xqrndt", "xqsubm", "xqtex2",
			"xqtext", "xqxdys", "xqcdsc", "xqqrm", "xofob", "xomkto", "xontax", "xotax", "xoslac", "xoum",
			"xoilij", "xoirij", "xjtime", "xjins", "xjins2", "xjstdf", "xjload", "xjoins", "xjoin2", "xjscst",
			"xjexcq", "xjiext", "xicstm", "xpcstm", "xptaxe", "xdspl", "xdspm", "xdpres", "xdplbd", "xdminb",
			"xdwrkq", "xdcseq", "xdclsp", "xdpcip", "xdpacp", "xdpaup", "xdpayl", "xrarac", "xrcsac", "xrfrac",
			"xrdsac", "xragem", "xraged", "xrtxfr", "xrgrbc", "xaapac", "xacsac", "xafrac", "xadsac", "xatxfr",
			"xaupdc", "xasdte", "xgyear", "xgperd", "xgreac", "xgimf", "xgiml", "DBDESC", "DBVERS", "DBCOLO",
			"DBCOMP", "DBADR1", "DBADR2", "DBCITY", "DBSTAT", "DBZIP", "DBCTRY", "DBREG", "DBCURR", "DBDIV",
			"DBCHTP", "DBDEPT", "DBPHON", "DBFAX", "DBEADD", "DBFDID", "DBBAIN", "DBBSB", "DBBANA", "DBBANO",
			"DBDEUN", "DBDEUI", "XDADDRESSLINE1", "XDADDRESSLINE2", "XDCITY", "XDNAME", "XDCOUNTRY", "XDDESCRIPTION", "XDEMAILADDRESS", "XDFAXNUMBER",
			"XDFEDERALID", "XDPHONENUMBER", "XDREGION", "XDSTATE", "XDVERSION", "XDPOSTCODE", "XDCURRENCYID", "XDGLDIVISIONID", "XDGLDEPARTMENTID", "XDGLCHARTPREFIX",
			"XDCOLOR", "XDCOMPANYMESSAGERTF", "XDCOMPANYMESSAGETEXT", "XDSELLQUANTITYDECIMALS", "XDBUYQUANTITYDECIMALS", "XDINVENTORYQUANTITYDECIMALS", "XDCOMPANYLOGO", "XDADDRESSLINE3", "XDSUPPRESSADDRESSONREPORTS", "XDEDITINEXPLORERS",
			"XDEXTENDEDSEARCHOPTIONS", "XDEXPORTFOLLOWUPS", "XDWEBGEARWEBSITEURL", "XDBANKACCOUNTID", "XFAPAPACCOUNT", "XFAPCASHACCOUNT", "XFAPDISCOUNTACCOUNT", "XFAPFREIGHTACCOUNT", "XFAPAPCOSTSTARTDATE", "XFAPTAXONFREIGHT",
			"XFAPUPDATEJOBCOSTS", "XFGLPAYROLLIMPORTFORMAT", "XFGLPAYROLLIMPORTLOCATION", "XFGLFISCALPERIOD", "XFGLRETAINEDEARNINGSACCOUNT", "XFGLFISCALYEAR", "XFAGEBYDAYSINMONTH", "XFAGINGMETHOD", "XFARARACCOUNT", "XFARCASHACCOUNT",
			"XFARDISCOUNTACCOUNT", "XFARFREIGHTACCOUNT", "XFARGROUPSHIPMENTSBYCUSTOMER", "XFARTAXONFREIGHT", "XFARAGINGID", "XFAPAGINGID", "XFAPDISCOUNTONTAX", "XFARSHOWDEPOSITS", "XFAPPAYMENTMAXLINESPERPAGE", "XPDCSFESHUTDOWNPASSWORD",
			"XPDCENABLEMINIMIZEBUTTONINSFE", "XPDCPAYROLLEXPORTLOCATION", "XPDCPROMPTFORLABORDESCRIPTION", "XPDCPROMPTFORREASON", "XPDCSPLITLABORHOURS", "XPDCSPLITMACHINEHOURS", "XPIMCOSTINGMETHOD", "XPJMINSIDEINSPECTIONLINERTF", "XPJMINSIDEINSPECTIONLINETEXT", "XPJMLOADRELIEFMETHOD",
			"XPJMOUTSIDEINSPECTIONLINETEXT", "XPJMOUTSIDEINSPECTIONLINERTF", "XPJMSTANDARDFACTOR", "XPJMTIMEFORMAT", "XPOMFOB", "XPOMMAKEID", "XPCMNONTAXREASONID", "XPOMSALESACCOUNT", "XPCMCUSTOMERTAXABLE", "XPOMUM",
			"XPPMCOSTINGMETHOD", "XPPMTAXEXEMPTNUMBER", "XPQMBURDENMARKUP", "XPQMFOLLOWUPDAYS", "XPQMQUOTEFOOTERMESSAGETEXT", "XPQMQUOTEFOOTERMESSAGERTF", "XPQMLABORMARKUP", "XPQMMATERIALMARKUP", "XPQMQUOTINGMETHOD", "XPQMNUMBEROFDECIMALS",
			"XPQMSUBCONTRACTMARKUP", "XPQMQUOTEHEADERMESSAGETEXT", "XPQMQUOTEHEADERMESSAGERTF", "XPQMEXPIRATIONDAYS", "XPDCENABLEWORKQUEUE", "XPJMSPLITCOSTS", "XPJMEXCESSQUANTITY", "XPJMINITIALEXTENSION", "XPQMADDITIONALCHARGETEXT", "XPOMINCLUDEORDERLINEINJOB",
			"XPOMINCLUDEORDERRELEASEINJOB", "XPDCPROMPTFORCLOCKINPASSWORD", "XPDCPROMPTFORACTIVITYPASSWORD", "XPDCPROMPTFORAUDITPASSWORD", "XPDCENABLECREATESEQUENCE", "XPQMQUOTINGMARKUP", "XPIMTRANSFERCUSTOMER", "XPCMORDERCREDITMESSAGE", "XPCMSHIPMENTCREDITMESSAGE", "XPCMCREDITLIMITSOURCE",
			"XPIMOVERWRITEMETHOD", "XPDCSHOWCURRENTJOBSONLY", "XPPMPURCHASETYPE", "XPDCUSESERVERTIME", "XPHDCONTACTMETHODID", "XPLORESPONSEMETHODID", "XPDCPAYCALCULATIONMETHOD", "XPDCREFRESHINTERVAL", "XPDCIDLETIMETHRESHHOLD", "XPJMSCHEDULETYPE",
			"XPJMSCHEDULEBOARDFIELDS", "XPQMREFRESHRATEINFO", "XPDCENABLEISSUEMATERIAL", "XPSMDELETEZEROSHIPMENTLINES", "XPCHCHANGEREQUESTTYPEID", "XPHDCALLTYPEID", "XPHDSALESCALLTYPEID", "XPOMAUTOCREATERELEASE", "XPQMQUOTEMARKUPTYPE"
		};
		string[] array2 = new string[219]
		{
			"xapQMOverheadMarkup", "xapQMFollowUpDays", "xapQMQuoteFooterMessageText", "xapQMQuoteFooterMessageRTF", "xapQMLaborMarkup", "xapQMMaterialMarkup", "xapQMQuotingMethod", "xapQMNumberOfDecimals", "xapQMSubcontractMarkup", "xapQMQuoteHeaderMessageText",
			"xapQMQuoteHeaderMessageRTF", "xapQMExpirationDays", "xapQMAdditionalChargeText", "xapQMQuotingMarkup", "xapOMFreeOnBoardDescription", "xapOMDeliveryType", "xapCMNonTaxReasonID", "xapCMCustomerTaxable", "xapOMSalesGLAccountID", "xapOMUnitOfMeasure",
			"xapOMIncludeOrderLineInJob", "xapOMIncludeOrderDeliveryInJob", "xapDCTimeFormat", "xapJMInsideInspectionLineRTF", "xapJMInsideInspectionLineText", "xapJMStandardFactor", "xapJMLoadReliefMethod", "xapJMOutsideInspectionLineRTF", "xapJMOutsideInspectionLineText", "xapJMSplitCosts",
			"xapJMExcessQuantity", "xapJMInitialExtension", "xapIMCostingMethod", "xapPMCostingMethod", "xapPMTaxExemptNumber", "xapDCSplitLaborHours", "xapDCSplitMachineHours", "xapDCPromptForReason", "xapDCPromptForLaborDescription", "xapDCEnableMinimizeButtonInSFE",
			"xapDCEnableWorkQueue", "xapDCEnableCreateSequence", "xapDCSFEShutdownPassword", "xapDCPromptForClockInPassword", "xapDCPromptForActivityPassword", "xapDCPromptForAuditPassword", "xapDCPayrollExportLocation", "xafARARGLAccountID", "xafARCashGLAccountID", "xafARFreightGLAccountID",
			"xafARDiscountGLAccountID", "xafAgingMethod", "xafAgeByDaysInMonth", "xafARTaxOnFreight", "xafARGroupShipmentsByCustomer", "xafAPAPGLAccountID", "xafAPCashGLAccountID", "xafAPFreightGLAccountID", "xafAPDiscountGLAccountID", "xafAPTaxOnFreight",
			"xafAPUpdateJobCosts", "xafAPAPCostStartDate", "xafGLFiscalYearID", "xafGLFiscalYearPeriodID", "xafGLRetainedEarningsAccountID", "xafGLPayrollImportFormat", "xafGLPayrollImportLocation", "xadDescription", "xadVersion", "xadColor",
			"xadName", "xadAddressLine1", "xadAddressLine2", "xadCity", "xadState", "xadPostCode", "xadCountry", "xadRegion", "xadCurrencyRateID", "xadGLDivisionID",
			"xadGLChartPrefix", "xadGLDepartmentID", "xadPhoneNumber", "xadFaxNumber", "xadEmailAddress", "xadFederalID", "xdBankInitials", "xdBSBNumber", "xdBankAccountName", "xdBankAccountNumber",
			"xdDirectEntryUserName", "xdDirectEntryUserID", "xadAddressLine1", "xadAddressLine2", "xadCity", "xadName", "xadCountry", "xadDescription", "xadEmailAddress", "xadFaxNumber",
			"xadFederalID", "xadPhoneNumber", "xadRegion", "xadState", "xadVersion", "xadPostCode", "xadCurrencyRateID", "xadGLDivisionID", "xadGLDepartmentID", "xadGLChartPrefix",
			"xadColor", "xadCompanyMessageRTF", "xadCompanyMessageText", "xadSellQuantityDecimals", "xadBuyQuantityDecimals", "xadInventoryQuantityDecimals", "xadCompanyLogo", "xadAddressLine3", "xadSuppressAddressOnReports", "xadEditInExplorers",
			"xadExtendedSearchOptions", "xadExportFollowups", "xadWebGearWebsiteURL", "xadBankAccountID", "xafAPAPGLAccountID", "xafAPCashGLAccountID", "xafAPDiscountGLAccountID", "xafAPFreightGLAccountID", "xafAPAPCostStartDate", "xafAPTaxOnFreight",
			"xafAPUpdateJobCosts", "xafGLPayrollImportFormat", "xafGLPayrollImportLocation", "xafGLFiscalYearPeriodID", "xafGLRetainedEarningsAccountID", "xafGLFiscalYearID", "xafAgeByDaysInMonth", "xafAgingMethod", "xafARARGLAccountID", "xafARCashGLAccountID",
			"xafARDiscountGLAccountID", "xafARFreightGLAccountID", "xafARGroupShipmentsByCustomer", "xafARTaxOnFreight", "xafARAgingBucketID", "xafAPAgingBucketID", "xafAPDiscountOnTax", "xafARShowDeposits", "xafAPPaymentMaxLinesPerPage", "xapDCSFEShutdownPassword",
			"xapDCEnableMinimizeButtonInSFE", "xapDCPayrollExportLocation", "xapDCPromptForLaborDescription", "xapDCPromptForReason", "xapDCSplitLaborHours", "xapDCSplitMachineHours", "xapIMCostingMethod", "xapJMInsideInspectionLineRTF", "xapJMInsideInspectionLineText", "xapJMLoadReliefMethod",
			"xapJMOutsideInspectionLineText", "xapJMOutsideInspectionLineRTF", "xapJMStandardFactor", "xapJMTimeFormat", "xapOMFreeOnBoardDescription", "xapOMDeliveryType", "xapCMNonTaxReasonID", "xapOMSalesGLAccountID", "xapCMCustomerTaxable", "xapOMUnitOfMeasure",
			"xapPMCostingMethod", "xapPMTaxExemptNumber", "xapQMOverheadMarkup", "xapQMFollowUpDays", "xapQMQuoteFooterMessageText", "xapQMQuoteFooterMessageRTF", "xapQMLaborMarkup", "xapQMMaterialMarkup", "xapQMQuotingMethod", "xapQMNumberOfDecimals",
			"xapQMSubcontractMarkup", "xapQMQuoteHeaderMessageText", "xapQMQuoteHeaderMessageRTF", "xapQMExpirationDays", "xapDCEnableWorkQueue", "xapJMSplitCosts", "xapJMExcessQuantity", "xapJMInitialExtension", "xapQMAdditionalChargeText", "xapOMIncludeOrderLineInJob",
			"xapOMIncludeOrderDeliveryInJob", "xapDCPromptForClockInPassword", "xapDCPromptForActivityPassword", "xapDCPromptForAuditPassword", "xapDCEnableCreateSequence", "xapQMQuotingMarkup", "xapIMTransferCustomer", "xapCMOrderCreditMessage", "xapCMShipmentCreditMessage", "xapCMCreditLimitSource",
			"xapIMOverwriteMethod", "xapDCShowCurrentJobsOnly", "xapPMPurchaseType", "xapDCUseServerTime", "xapHDContactMethodID", "xapLOResponseMethodID", "xapDCPayCalculationMethod", "xapDCRefreshInterval", "xapDCIdleTimeThreshhold", "xapJMScheduleType",
			"xapJMScheduleBoardFields", "xapQMRefreshRateInfo", "xapDCEnableIssueMaterial", "xapSMDeleteZeroShipmentLines", "xapCHChangeRequestTypeID", "xapHDCallTypeID", "xapHDSalesCallTypeID", "xapOMAutoCreateDelivery", "xapQMQuoteMarkupType"
		};
		oldFieldName = oldFieldName.Trim();
		for (int i = 0; i < array2.Length; i++)
		{
			if (oldFieldName.Equals(array[i], StringComparison.CurrentCultureIgnoreCase))
			{
				return array2[i];
			}
		}
		return result;
	}

	private static void SetReportParameters(IServiceProvider provider, ReportDocument reportDoc, CrystalParameterCollection parameters, bool isExport)
	{
		SetReportParametersFromData(provider, reportDoc.ReportClientDocument.DataDefController, reportDoc.ReportClientDocument.DataDefController.ParameterFieldController, parameters, null, isExport);
		reportDoc.ReportClientDocument.SubreportController.GetSubreportNames();
		foreach (string subreportName in reportDoc.ReportClientDocument.SubreportController.GetSubreportNames())
		{
			SubreportClientDocument subreport = reportDoc.ReportClientDocument.SubreportController.GetSubreport(subreportName);
			SetReportParametersFromData(provider, subreport.DataDefController, reportDoc.ReportClientDocument.DataDefController.ParameterFieldController, parameters, reportDoc.ReportClientDocument.SubreportController.GetSubreportLinks(subreportName), isExport);
		}
	}

	private static void SetReportParametersFromData(IServiceProvider provider, DataDefController dataDefController, ParameterFieldController parmController, CrystalParameterCollection parameters, SubreportLinks subreportLinks, bool isExport)
	{
		M1Database m1Database = provider.GetService(typeof(M1Database)) as M1Database;
		for (int i = 0; i < dataDefController.DataDefinition.ParameterFields.Count; i++)
		{
			CrystalDecisions.ReportAppServer.DataDefModel.ParameterField parameterField = (CrystalDecisions.ReportAppServer.DataDefModel.ParameterField)dataDefController.DataDefinition.ParameterFields[i];
			bool flag = false;
			bool flag2 = false;
			string name = parameterField.Name;
			if (subreportLinks != null)
			{
				for (int j = 0; j < subreportLinks.Count; j++)
				{
					if (subreportLinks[j].LinkedParameterName.Equals(parameterField.FormulaForm, StringComparison.CurrentCultureIgnoreCase))
					{
						flag = true;
						break;
					}
				}
			}
			else if (name.Equals("DisableLinks", StringComparison.CurrentCultureIgnoreCase))
			{
				parmController.SetCurrentValue(parameterField.ReportName, parameterField.Name, isExport);
				flag2 = true;
			}
			if (flag)
			{
				continue;
			}
			if (parameters.Contains(name))
			{
				CrystalParameter crystalParameter = parameters[name];
				if (crystalParameter.Data != null && crystalParameter.Data.InstanceCount != 0)
				{
					object obj = getFieldOptionValue(m1Database, crystalParameter.Data.FieldOptions[0], crystalParameter.Data.Fields.Count - 1, crystalParameter.Data.Fields[crystalParameter.Data.Fields.Count - 1].FieldType);
					if (crystalParameter.IsRange && crystalParameter.Data.InstanceCount == 2)
					{
						if (M1Util.IsNullOrEmpty(obj) && (crystalParameter.Data.Fields[0].FieldType.Equals("date", StringComparison.CurrentCultureIgnoreCase) || crystalParameter.Data.Fields[0].FieldType.Equals("datetime", StringComparison.CurrentCultureIgnoreCase) || crystalParameter.Data.Fields[0].FieldType.Equals("smalldatetime", StringComparison.CurrentCultureIgnoreCase)))
						{
							obj = new DateTime(1900, 1, 1);
						}
						object obj2 = getFieldOptionValue(m1Database, crystalParameter.Data.FieldOptions[1], crystalParameter.Data.Fields.Count - 1, crystalParameter.Data.Fields[crystalParameter.Data.Fields.Count - 1].FieldType);
						if (M1Util.IsNullOrEmpty(obj2) && (crystalParameter.Data.Fields[0].FieldType.Equals("date", StringComparison.CurrentCultureIgnoreCase) || crystalParameter.Data.Fields[0].FieldType.Equals("datetime", StringComparison.CurrentCultureIgnoreCase) || crystalParameter.Data.Fields[0].FieldType.Equals("smalldatetime", StringComparison.CurrentCultureIgnoreCase)))
						{
							obj2 = new DateTime(2099, 12, 31);
						}
						if (M1Util.IsNullOrEmpty(obj2))
						{
							string reportName = parameterField.ReportName;
							string name2 = parameterField.Name;
							ParameterFieldRangeValue obj3 = (ParameterFieldRangeValue)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("B01BA553-151F-438D-A825-DECAA5584DBB")));
							obj3.BeginValue = obj;
							obj3.LowerBoundType = CrRangeValueBoundTypeEnum.crRangeValueBoundTypeInclusive;
							obj3.EndValue = obj2;
							obj3.UpperBoundType = CrRangeValueBoundTypeEnum.crRangeValueBoundTypeNoBound;
							parmController.SetCurrentValue(reportName, name2, obj3);
						}
						else
						{
							bool flag3 = true;
							if (name.Contains("FiscalYearPeriodID"))
							{
								short num = Convert.ToInt16(crystalParameter.Data.FieldOptions[0].Values[0][0]);
								if (Convert.ToInt16(crystalParameter.Data.FieldOptions[1].Values[0][0]) > num)
								{
									flag3 = false;
								}
							}
							if (flag3)
							{
								if (!crystalParameter.Data.IsRangeValid(obj, obj2))
								{
									throw new ReportParameterException($"Parameter {crystalParameter.Data.Fields[0].Caption} has a from value greater than the to value");
								}
								string reportName2 = parameterField.ReportName;
								string name3 = parameterField.Name;
								ParameterFieldRangeValue obj4 = (ParameterFieldRangeValue)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("B01BA553-151F-438D-A825-DECAA5584DBB")));
								obj4.BeginValue = obj;
								obj4.LowerBoundType = CrRangeValueBoundTypeEnum.crRangeValueBoundTypeInclusive;
								obj4.EndValue = obj2;
								obj4.UpperBoundType = CrRangeValueBoundTypeEnum.crRangeValueBoundTypeInclusive;
								parmController.SetCurrentValue(reportName2, name3, obj4);
							}
						}
					}
					else if (crystalParameter.EnableMultipleValues && crystalParameter.Data.InstanceCount > 1)
					{
						Values values = (Values)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("BCAEDADC-7B32-4A0D-B01F-D220B34B7EA9")));
						object obj5 = null;
						for (int k = 0; k < crystalParameter.Data.FieldOptions.Length; k++)
						{
							obj5 = getFieldOptionValue(m1Database, crystalParameter.Data.FieldOptions[k], crystalParameter.Data.Fields.Count - 1, crystalParameter.Data.Fields[crystalParameter.Data.Fields.Count - 1].FieldType);
							if (M1Util.IsNullOrEmpty(obj5) && (crystalParameter.Data.Fields[0].FieldType.Equals("date", StringComparison.CurrentCultureIgnoreCase) || crystalParameter.Data.Fields[0].FieldType.Equals("datetime", StringComparison.CurrentCultureIgnoreCase) || crystalParameter.Data.Fields[0].FieldType.Equals("smalldatetime", StringComparison.CurrentCultureIgnoreCase)))
							{
								obj5 = ((k != 0) ? ((k != crystalParameter.Data.FieldOptions.Length - 1) ? ((object)DateTime.Today) : ((object)new DateTime(2099, 12, 31))) : ((object)new DateTime(1900, 1, 1)));
							}
							values.Add(obj5);
						}
						parmController.SetCurrentValues(parameterField.ReportName, parameterField.Name, values);
					}
					else if (crystalParameter.EnableMultipleValues && crystalParameter.Data.DisplayType == ReportDisplayType.Filter)
					{
						Values multipleFilterValues = GetMultipleFilterValues(crystalParameter, parameterField.Type, m1Database);
						parmController.SetCurrentValues(parameterField.ReportName, parameterField.Name, multipleFilterValues);
					}
					else
					{
						if (M1Util.IsNullOrEmpty(obj))
						{
							if (crystalParameter.Data.Fields[0].FieldType.Equals("date", StringComparison.CurrentCultureIgnoreCase) || crystalParameter.Data.Fields[0].FieldType.Equals("datetime", StringComparison.CurrentCultureIgnoreCase) || crystalParameter.Data.Fields[0].FieldType.Equals("smalldatetime", StringComparison.CurrentCultureIgnoreCase))
							{
								obj = DateTime.Today;
							}
							else if (crystalParameter.Data.Fields[0].FieldType.Equals("nvarchar", StringComparison.CurrentCultureIgnoreCase))
							{
								obj = ((obj == null || !(obj.ToString() == "0")) ? string.Empty : ((object)0));
							}
						}
						parmController.SetCurrentValue(parameterField.ReportName, parameterField.Name, obj);
					}
					flag2 = true;
				}
			}
			else if (name.Equals("UserID", StringComparison.CurrentCultureIgnoreCase))
			{
				parmController.SetCurrentValue(parameterField.ReportName, parameterField.Name, ((M1User)provider.GetService(typeof(M1User))).ID);
				flag2 = true;
			}
			else if (name.Equals("Region", StringComparison.CurrentCultureIgnoreCase))
			{
				parmController.SetCurrentValue(parameterField.ReportName, parameterField.Name, m1Database.Region);
				flag2 = true;
			}
			else if (name.StartsWith("SecurityModuleValue_", StringComparison.CurrentCultureIgnoreCase))
			{
				M1DataDictionary m1DataDictionary = (M1DataDictionary)provider.GetService(typeof(M1DataDictionary));
				parmController.SetCurrentValue(parameterField.ReportName, parameterField.Name, m1DataDictionary.ProductCode.IsModulePurchased(name.Substring(20), m1Database));
				flag2 = true;
			}
			else if (name.StartsWith("App_", StringComparison.CurrentCultureIgnoreCase))
			{
				object value;
				if (name.Equals("App_HomeCurrencySymbol", StringComparison.CurrentCultureIgnoreCase))
				{
					value = ((!string.IsNullOrWhiteSpace(m1Database.HomeCurrencySymbol)) ? m1Database.HomeCurrencySymbol : m1Database.SystemCurrencySymbol);
				}
				else
				{
					ScriptApp scriptApp = provider.GetService(typeof(ScriptApp)) as ScriptApp;
					value = scriptApp.GetType().GetProperty(name.Substring(4)).GetValue(scriptApp, null);
				}
				parmController.SetCurrentValue(parameterField.ReportName, parameterField.Name, value);
				flag2 = true;
			}
			else if (name.StartsWith("Props_", StringComparison.CurrentCultureIgnoreCase) || name.StartsWith("Constants_", StringComparison.CurrentCultureIgnoreCase))
			{
				string[] array = name.Split('_');
				if (array.Length == 3)
				{
					object obj = m1Database.Props(array[1])[array[2]];
					if (obj.GetType() == typeof(byte))
					{
						parmController.SetCurrentValue(parameterField.ReportName, parameterField.Name, Convert.ToInt16(obj));
					}
					else
					{
						parmController.SetCurrentValue(parameterField.ReportName, parameterField.Name, obj);
					}
					flag2 = true;
				}
			}
			else if (name.StartsWith("DsInfo_", StringComparison.CurrentCultureIgnoreCase) || name.StartsWith("DatasetProperties_", StringComparison.CurrentCultureIgnoreCase))
			{
				string[] array2 = name.Split('_');
				if (array2.Length == 2)
				{
					object obj = m1Database.Props("DatasetProperties")[array2[1]];
					parmController.SetCurrentValue(parameterField.ReportName, parameterField.Name, obj);
					flag2 = true;
				}
			}
			if (!flag2)
			{
				object defaultEmptyValue = GetDefaultEmptyValue(parameterField.Type);
				if (defaultEmptyValue != null)
				{
					parmController.SetCurrentValue(parameterField.ReportName, parameterField.Name, defaultEmptyValue);
				}
			}
		}
	}

	private static object GetDefaultEmptyValue(CrFieldValueTypeEnum fieldType)
	{
		return fieldType switch
		{
			CrFieldValueTypeEnum.crFieldValueTypeNumberField => 0, 
			CrFieldValueTypeEnum.crFieldValueTypeStringField => string.Empty, 
			CrFieldValueTypeEnum.crFieldValueTypeBooleanField => false, 
			_ => null, 
		};
	}

	private static Values GetMultipleFilterValues(CrystalParameter parameterContainer, CrFieldValueTypeEnum fieldValueType, M1Database database)
	{
		Values values = (Values)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("BCAEDADC-7B32-4A0D-B01F-D220B34B7EA9")));
		for (int i = 0; i < parameterContainer.Data.FieldOptions.Length; i++)
		{
			CrystalFieldOption crystalFieldOption = parameterContainer.Data.FieldOptions[i];
			for (int j = 0; j < crystalFieldOption.Values.Count; j++)
			{
				object fieldOptionForMultipleValues = GetFieldOptionForMultipleValues(database, crystalFieldOption, i, parameterContainer.Data.Fields[i].FieldType, j);
				if (!M1Util.IsNullOrEmpty(fieldOptionForMultipleValues))
				{
					values.Add(fieldOptionForMultipleValues);
				}
			}
		}
		if (values.Count == 0)
		{
			values.Add(GetDefaultEmptyValue(fieldValueType));
		}
		return values;
	}

	private static void SetReportParametersFromDataOld(IServiceProvider provider, ReportDocument reportDoc, CrystalParameterCollection parameters)
	{
		M1Database m1Database = provider.GetService(typeof(M1Database)) as M1Database;
		for (int i = 0; i < reportDoc.ParameterFields.Count; i++)
		{
			CrystalDecisions.Shared.ParameterField parameterField = reportDoc.ParameterFields[i];
			if ((parameterField.ParameterFieldUsage2 & ParameterFieldUsage2.InUse) != ParameterFieldUsage2.InUse)
			{
				continue;
			}
			string parameterFieldName = parameterField.ParameterFieldName;
			if (parameterFieldName.Equals("UserID", StringComparison.CurrentCultureIgnoreCase))
			{
				parameterField.CurrentValues.Clear();
				parameterField.CurrentValues.Add(new ParameterDiscreteValue
				{
					Value = ((M1User)provider.GetService(typeof(M1User))).ID
				});
				reportDoc.SetParameterValue(parameterField.Name, ((M1User)provider.GetService(typeof(M1User))).ID);
			}
			else if (parameterFieldName.Equals("Region", StringComparison.CurrentCultureIgnoreCase))
			{
				parameterField.CurrentValues.Clear();
				parameterField.CurrentValues.Add(new ParameterDiscreteValue
				{
					Value = m1Database.Region
				});
			}
			else if (parameterFieldName.StartsWith("SecurityModuleValue_", StringComparison.CurrentCultureIgnoreCase))
			{
				parameterField.CurrentValues.Clear();
				M1DataDictionary m1DataDictionary = (M1DataDictionary)provider.GetService(typeof(M1DataDictionary));
				parameterField.CurrentValues.Add(new ParameterDiscreteValue
				{
					Value = m1DataDictionary.ProductCode.IsModulePurchased(parameterFieldName.Substring(20), m1Database)
				});
			}
			else if (parameterFieldName.StartsWith("App_", StringComparison.CurrentCultureIgnoreCase))
			{
				parameterField.CurrentValues.Clear();
				if (parameterFieldName.Equals("App_HomeCurrencySymbol", StringComparison.CurrentCultureIgnoreCase))
				{
					if (string.IsNullOrWhiteSpace(m1Database.HomeCurrencySymbol))
					{
						parameterField.CurrentValues.Add(new ParameterDiscreteValue
						{
							Value = m1Database.SystemCurrencySymbol
						});
					}
					else
					{
						parameterField.CurrentValues.Add(new ParameterDiscreteValue
						{
							Value = m1Database.HomeCurrencySymbol
						});
					}
				}
				else
				{
					ScriptApp scriptApp = provider.GetService(typeof(ScriptApp)) as ScriptApp;
					PropertyInfo property = scriptApp.GetType().GetProperty(parameterFieldName.Substring(4));
					parameterField.CurrentValues.Add(new ParameterDiscreteValue
					{
						Value = property.GetValue(scriptApp, null)
					});
				}
			}
			else if (parameterFieldName.StartsWith("Props_", StringComparison.CurrentCultureIgnoreCase) || parameterFieldName.StartsWith("Constants_", StringComparison.CurrentCultureIgnoreCase))
			{
				parameterField.CurrentValues.Clear();
				string[] array = parameterFieldName.Split('_');
				if (array.Length == 3)
				{
					object obj = m1Database.Props(array[1])[array[2]];
					if (obj.GetType() == typeof(byte))
					{
						parameterField.CurrentValues.Add(new ParameterDiscreteValue
						{
							Value = Convert.ToInt16(obj)
						});
						reportDoc.SetParameterValue(parameterField.Name, Convert.ToInt16(obj));
					}
					else
					{
						parameterField.CurrentValues.Add(new ParameterDiscreteValue
						{
							Value = obj
						});
						reportDoc.SetParameterValue(parameterField.Name, obj);
					}
				}
			}
			else if (parameterFieldName.StartsWith("DsInfo_", StringComparison.CurrentCultureIgnoreCase) || parameterFieldName.StartsWith("DatasetProperties_", StringComparison.CurrentCultureIgnoreCase))
			{
				parameterField.CurrentValues.Clear();
				string[] array2 = parameterFieldName.Split('_');
				if (array2.Length == 2)
				{
					object obj = m1Database.Props("DatasetProperties")[array2[1]];
					parameterField.CurrentValues.Add(new ParameterDiscreteValue
					{
						Value = obj
					});
				}
			}
			else
			{
				if (!parameters.Contains(parameterFieldName))
				{
					continue;
				}
				parameterField.CurrentValues.Clear();
				CrystalParameter crystalParameter = parameters[parameterFieldName];
				if (crystalParameter.Data != null)
				{
					if (crystalParameter.IsRange && crystalParameter.Data.InstanceCount == 2)
					{
						object obj = getFieldOptionValue(m1Database, crystalParameter.Data.FieldOptions[0], 0, crystalParameter.Data.Fields[0].FieldType);
						if (M1Util.IsNullOrEmpty(obj) && (crystalParameter.Data.Fields[0].FieldType.Equals("date", StringComparison.CurrentCultureIgnoreCase) || crystalParameter.Data.Fields[0].FieldType.Equals("datetime", StringComparison.CurrentCultureIgnoreCase) || crystalParameter.Data.Fields[0].FieldType.Equals("smalldatetime", StringComparison.CurrentCultureIgnoreCase)))
						{
							obj = new DateTime(1900, 1, 1);
						}
						object obj2 = getFieldOptionValue(m1Database, crystalParameter.Data.FieldOptions[1], 0, crystalParameter.Data.Fields[0].FieldType);
						if (M1Util.IsNullOrEmpty(obj2) && (crystalParameter.Data.Fields[0].FieldType.Equals("date", StringComparison.CurrentCultureIgnoreCase) || crystalParameter.Data.Fields[0].FieldType.Equals("datetime", StringComparison.CurrentCultureIgnoreCase) || crystalParameter.Data.Fields[0].FieldType.Equals("smalldatetime", StringComparison.CurrentCultureIgnoreCase)))
						{
							obj2 = new DateTime(2099, 12, 31);
						}
						parameterField.CurrentValues.AddRange(obj, obj2, RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive);
					}
					else
					{
						object obj = getFieldOptionValue(m1Database, crystalParameter.Data.FieldOptions[0], 0, crystalParameter.Data.Fields[0].FieldType);
						if (M1Util.IsNullOrEmpty(obj) && (crystalParameter.Data.Fields[0].FieldType.Equals("date", StringComparison.CurrentCultureIgnoreCase) || crystalParameter.Data.Fields[0].FieldType.Equals("datetime", StringComparison.CurrentCultureIgnoreCase) || crystalParameter.Data.Fields[0].FieldType.Equals("smalldatetime", StringComparison.CurrentCultureIgnoreCase)))
						{
							obj = DateTime.Today;
						}
						parameterField.CurrentValues.Add(new ParameterDiscreteValue
						{
							Value = obj
						});
					}
				}
				if (!parameterField.HasCurrentValue && parameterField.ReportName.Length == 0)
				{
					if (parameterField.ParameterValueType == ParameterValueKind.StringParameter)
					{
						parameterField.CurrentValues.AddValue("");
					}
					else if (parameterField.ParameterValueType == ParameterValueKind.NumberParameter)
					{
						parameterField.CurrentValues.AddValue(0);
					}
					else if (parameterField.ParameterValueType == ParameterValueKind.BooleanParameter)
					{
						parameterField.CurrentValues.AddValue(false);
					}
				}
			}
		}
	}

	private static object checkFromToValueParameter(CrystalParameterCollection parameters, M1Database database, string fieldToCheck, int fieldOptionNumber, ReportPromptFieldInfo sourceFieldInfo)
	{
		for (int i = 0; i < parameters.Count; i++)
		{
			CrystalParameter crystalParameter = parameters[i];
			if (crystalParameter.Data == null || crystalParameter.Data.Fields == null || crystalParameter.Data.Fields.Count == 0)
			{
				continue;
			}
			for (int j = 0; j < crystalParameter.Data.Fields.Count; j++)
			{
				if (crystalParameter.Data.Fields[j].FieldName.Equals(fieldToCheck, StringComparison.CurrentCultureIgnoreCase))
				{
					sourceFieldInfo.FieldType = crystalParameter.Data.Fields[j].FieldType;
					return getFieldOptionValue(database, crystalParameter.Data.FieldOptions[fieldOptionNumber], j, crystalParameter.Data.Fields[j].FieldType);
				}
			}
		}
		return null;
	}

	private static object GetFieldOptionForMultipleValues(M1Database database, CrystalFieldOption fieldOption, int arrayIndex, string fieldType, int listIndex)
	{
		return getFieldOptionValue(database, fieldOption, arrayIndex, fieldType, listIndex);
	}

	private static object getFieldOptionValue(M1Database database, CrystalFieldOption fieldOption, int arrayIndex, string fieldType, int listIndex = 0)
	{
		object obj = ((fieldOption.Values != null && fieldOption.Values.Count != 0) ? fieldOption.Values[listIndex][arrayIndex] : ((fieldOption.DefaultValueExpressions != null && fieldOption.DefaultValueExpressions.Length > arrayIndex && !string.IsNullOrWhiteSpace(fieldOption.DefaultValueExpressions[arrayIndex])) ? database.ScriptingQuick.Eval(fieldOption.DefaultValueExpressions[arrayIndex]) : ((!fieldType.Equals("char", StringComparison.CurrentCultureIgnoreCase) && !fieldType.Equals("varchar", StringComparison.CurrentCultureIgnoreCase) && !fieldType.Equals("nchar", StringComparison.CurrentCultureIgnoreCase) && !fieldType.Equals("nvarchar", StringComparison.CurrentCultureIgnoreCase)) ? ((!fieldType.Equals("numeric", StringComparison.CurrentCultureIgnoreCase) && !fieldType.Equals("int", StringComparison.CurrentCultureIgnoreCase) && !fieldType.Equals("tinyint", StringComparison.CurrentCultureIgnoreCase) && !fieldType.Equals("smallint", StringComparison.CurrentCultureIgnoreCase) && !fieldType.Equals("money", StringComparison.CurrentCultureIgnoreCase) && !fieldType.Equals("byte", StringComparison.CurrentCultureIgnoreCase)) ? ((!fieldType.Equals("boolean", StringComparison.CurrentCultureIgnoreCase) && !fieldType.Equals("bit", StringComparison.CurrentCultureIgnoreCase)) ? null : ((object)false)) : ((object)0)) : string.Empty)));
		if (obj != null && obj.GetType() == typeof(byte))
		{
			obj = Convert.ToInt16(obj);
		}
		else if (fieldType.Equals("numeric", StringComparison.CurrentCultureIgnoreCase) || fieldType.Equals("int", StringComparison.CurrentCultureIgnoreCase))
		{
			obj = Convert.ToDecimal(obj);
		}
		else if ((fieldType.Equals("date", StringComparison.CurrentCultureIgnoreCase) || fieldType.Equals("datetime", StringComparison.CurrentCultureIgnoreCase) || fieldType.Equals("smalldatetime", StringComparison.CurrentCultureIgnoreCase)) && M1Util.IsNullOrEmpty(obj))
		{
			obj = null;
		}
		return obj;
	}

	private static string getFieldDefinitionFileName(CrystalDecisions.ReportAppServer.DataDefModel.Table crystalTable)
	{
		Strings propertyIDs = crystalTable.ConnectionInfo.Attributes.PropertyIDs;
		int num = propertyIDs.FindIndexOf("File Path");
		if (num == -1)
		{
			num = propertyIDs.FindIndexOf("QE_ServerDescription");
		}
		if (num != -1)
		{
			return ((ISCRPropertyBag)crystalTable.ConnectionInfo.Attributes).get_StringValue(propertyIDs[num]);
		}
		return string.Empty;
	}

	private static string getFieldDefinitionFileName(CrystalDecisions.CrystalReports.Engine.Table crystalTable)
	{
		for (int i = 0; i < crystalTable.LogOnInfo.ConnectionInfo.LogonProperties.Count; i++)
		{
			NameValuePair2 nameValuePair = (NameValuePair2)crystalTable.LogOnInfo.ConnectionInfo.LogonProperties[i];
			if (nameValuePair.Name.ToString().TrimEnd().Equals("File Path", StringComparison.CurrentCultureIgnoreCase))
			{
				return nameValuePair.Value.ToString();
			}
		}
		return crystalTable.LogOnInfo.ConnectionInfo.ServerName;
	}

	private static void loadTableSourcesForReport(Dictionary<string, ReportTableSource> tableSources, ReportDocument report, string reportLocation)
	{
		loadTableSourcesForDb(tableSources, reportLocation, report.ReportClientDocument.SubreportController, report.ReportClientDocument.DatabaseController.Database, report);
		Strings subreportNames = report.ReportClientDocument.SubreportController.GetSubreportNames();
		for (int i = 0; i < report.Subreports.Count; i++)
		{
			ReportDocument reportDoc = report.Subreports[subreportNames[i]];
			loadTableSourcesForDb(tableSources, reportLocation, report.ReportClientDocument.SubreportController, report.ReportClientDocument.SubreportController.GetSubreportDatabase(subreportNames[i]), reportDoc);
		}
	}

	private static void loadTableSourcesForDb(Dictionary<string, ReportTableSource> tableSources, string reportLocation, SubreportController subReportController, CrystalDecisions.ReportAppServer.DataDefModel.Database crystalDb, ReportDocument reportDoc)
	{
		for (int i = 0; i < crystalDb.Tables.Count; i++)
		{
			CrystalDecisions.ReportAppServer.DataDefModel.Table table = (CrystalDecisions.ReportAppServer.DataDefModel.Table)crystalDb.Tables[i];
			if (!isSqlTable(table.ConnectionInfo.Attributes))
			{
				loadTableSource(tableSources, reportLocation, reportDoc, table);
			}
		}
	}

	private static void loadTableSource(Dictionary<string, ReportTableSource> tableSources, string reportLocation, ReportDocument reportDoc, CrystalDecisions.ReportAppServer.DataDefModel.Table crystalTable)
	{
		if (tableSources.ContainsKey(reportDoc.Name + "." + crystalTable.Name))
		{
			return;
		}
		string fieldDefinitionFileName = getFieldDefinitionFileName(crystalTable);
		fieldDefinitionFileName = Path.GetFileNameWithoutExtension(fieldDefinitionFileName);
		fieldDefinitionFileName = Path.Combine(reportLocation, fieldDefinitionFileName);
		string text = (File.Exists(Path.ChangeExtension(fieldDefinitionFileName, "vbs")) ? Path.ChangeExtension(fieldDefinitionFileName, "vbs") : ((!File.Exists(Path.ChangeExtension(fieldDefinitionFileName, "sql"))) ? string.Empty : Path.ChangeExtension(fieldDefinitionFileName, "sql")));
		if (!string.IsNullOrWhiteSpace(text))
		{
			string text2 = File.ReadAllText(text);
			if (!string.IsNullOrWhiteSpace(text2))
			{
				string databaseDll = getDatabaseDll(crystalTable.ConnectionInfo.Attributes);
				tableSources.Add(reportDoc.Name + "." + crystalTable.Name, new ReportTableSource
				{
					DllName = databaseDll,
					FileName = Path.GetFileName(text),
					Text = text2
				});
			}
		}
	}

	private static void attachDataToTable(CrystalParameterCollection parameters, Dictionary<string, ReportTableSource> tableSources, SubreportController subReportController, ReportDocument reportDoc, M1Database database, string sqlWhere, CrystalDecisions.ReportAppServer.DataDefModel.Database crystalDb, CrystalDecisions.ReportAppServer.DataDefModel.Table crystalTable, CrystalDecisions.CrystalReports.Engine.Table oldTable)
	{
		ReportTableSource reportTableSource = tableSources[reportDoc.Name + "." + crystalTable.Name];
		string extension = Path.GetExtension(reportTableSource.FileName);
		if (extension.Equals(".vbs", StringComparison.CurrentCultureIgnoreCase))
		{
			string text = ProcessTextForClauses(parameters, database, reportTableSource.Text, sqlWhere, isScript: true, removeQuotes: false);
			using ReportScripting reportScripting = new ReportScripting(database);
			if (reportTableSource.DllName.Equals("crdb_fielddef.dll", StringComparison.CurrentCultureIgnoreCase))
			{
				object obj = reportScripting.ExecuteReportCodeRs(text);
				if (obj.GetType() == typeof(DataTable))
				{
					SetDataTableAsDataSourceForReport(subReportController, reportDoc, crystalTable, text, reportScripting);
				}
				else
				{
					oldTable.SetDataSource((Recordset)obj);
				}
			}
			else
			{
				SetDataTableAsDataSourceForReport(subReportController, reportDoc, crystalTable, text, reportScripting);
			}
			return;
		}
		if (extension.Equals(".sql", StringComparison.CurrentCultureIgnoreCase))
		{
			string text = ProcessTextForClauses(parameters, database, reportTableSource.Text, sqlWhere, isScript: false, removeQuotes: false);
			if (reportTableSource.DllName.Equals("crdb_fielddef.dll", StringComparison.CurrentCultureIgnoreCase))
			{
				Recordset recordset = new RecordsetClass();
				AppContext appContext = database.GetService(typeof(AppContext)) as AppContext;
				recordset.Open(text, appContext.DBServerManager.GetComConnection(database.ID, "M1"), CursorTypeEnum.adOpenForwardOnly, LockTypeEnum.adLockReadOnly);
				oldTable.SetDataSource(recordset);
				recordset.Close();
			}
			else
			{
				DataSet dataSet = database.GetDataSet(text);
				dataSet.Tables[0].TableName = crystalTable.Name;
				ISCRDataSet dataSource = DataSetConverter.Convert(dataSet);
				subReportController.SetDataSource(reportDoc.Name, dataSource);
			}
		}
	}

	private static void SetDataTableAsDataSourceForReport(SubreportController subReportController, ReportDocument reportDoc, CrystalDecisions.ReportAppServer.DataDefModel.Table crystalTable, string text, ReportScripting script)
	{
		DataTable dataTable = script.ExecuteReportCodeDT(text);
		DataSet dataset = new DataSet
		{
			Tables = { dataTable }
		};
		dataTable.TableName = crystalTable.Name;
		ISCRDataSet dataSource = DataSetConverter.Convert(dataset);
		if (string.IsNullOrWhiteSpace(reportDoc.Name))
		{
			reportDoc.SetDataSource(dataSource);
		}
		else
		{
			subReportController.SetDataSource(reportDoc.Name, dataSource);
		}
	}

	private static string ProcessTextForClauses(CrystalParameterCollection parameters, M1Database database, string text, string whereClause, bool isScript, bool removeQuotes)
	{
		if (!isScript)
		{
			text = text.Replace("\r", " ").Replace("\n", " ");
		}
		text = text.Replace("{?WHERECLAUSE}", whereClause, caseInsensitive: true);
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		for (int num = text.IndexOf("{!"); num != -1; num = text.IndexOf("{!"))
		{
			stringBuilder.Append(text.Substring(0, num));
			text = text.Substring(num);
			num = text.IndexOf("!}");
			if (num == -1)
			{
				break;
			}
			string text2 = text.Substring(0, num + 2);
			text = text.Substring(num + 2);
			text2 = text2.Substring(2, text2.Length - 4);
			text2 = CheckExpressionForParametersScript(parameters, text2, isScript);
			stringBuilder.Append(M1Util.ConvertToSql(database.ScriptingQuick.Eval(text2)));
		}
		stringBuilder.Append(text);
		text = stringBuilder.ToString();
		stringBuilder.Length = 0;
		for (int num = text.IndexOf("{?"); num != -1; num = text.IndexOf("{?"))
		{
			stringBuilder.Append(text.Substring(0, num));
			text = text.Substring(num);
			num = text.IndexOf("}");
			if (num == -1)
			{
				break;
			}
			string text2 = text.Substring(0, num + 1);
			text = text.Substring(num + 1);
			text2 = text2.Substring(2, text2.Length - 3);
			if (text.StartsWith("("))
			{
				num = text.IndexOf(')');
				if (num != -1)
				{
					text2 += text.Substring(0, num + 1);
					text = text.Substring(num + 1);
				}
			}
			if (text.StartsWith(".toArray", StringComparison.InvariantCultureIgnoreCase))
			{
				num = text.IndexOf(".");
				text = text.Substring(num + 8);
				flag = true;
			}
			else
			{
				flag = false;
			}
			text2 = GetReportParameterValue(parameters, text2, isScript, flag);
			if (removeQuotes)
			{
				text2 = text2.Replace("'", "");
			}
			if (text2.Length > 10 && text2.Substring(1, 10) == "DateSerial")
			{
				text2 = text2.Replace("\"", "");
			}
			stringBuilder.Append(text2);
		}
		stringBuilder.Append(text);
		return stringBuilder.ToString();
	}

	private static void setSourceToXML(ReportDocument topReportDoc, ReportDocument subReportDoc, DatabaseController dbController, M1Database database, DataSet ds, string ttxFilePath)
	{
		string vVal = Path.ChangeExtension(ttxFilePath, "xml");
		ISCRTable iSCRTable = topReportDoc.ReportClientDocument.SubreportController.GetSubreportDatabase(subReportDoc.Name).Tables[0];
		ISCRTable iSCRTable2 = iSCRTable.Clone();
		PropertyBag propertyBag = (PropertyBag)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("EC75982C-37CF-4F20-8736-92901B9FCAD7")));
		PropertyBag propertyBag2 = (PropertyBag)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("EC75982C-37CF-4F20-8736-92901B9FCAD7")));
		propertyBag2.Add("File Path ", vVal);
		propertyBag2.Add("Internal Connection ID", "{680eee31-a16e-4f48-8efa-8765193dccdd}");
		propertyBag.Add("Database DLL", "crdb_adoplus.dll");
		propertyBag.Add("QE_DatabaseName", "");
		propertyBag.Add("QE_DatabaseType", "");
		propertyBag.Add("QE_LogonProperties", propertyBag2);
		propertyBag.Add("QE_ServerDescription", "AR_Invoice_Edit_ttx");
		propertyBag.Add("QE_SQLDB", "False");
		propertyBag.Add("SSO Enabled", "False");
		CrystalDecisions.ReportAppServer.DataDefModel.ConnectionInfo connectionInfo = (CrystalDecisions.ReportAppServer.DataDefModel.ConnectionInfo)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("0F0EEB0E-21BE-4BE8-98EB-79B21E5B4DA5")));
		connectionInfo.Attributes = propertyBag;
		connectionInfo.Kind = CrConnectionInfoKindEnum.crConnectionInfoKindCRQE;
		topReportDoc.ReportClientDocument.DatabaseController.ReplaceConnection(iSCRTable.ConnectionInfo, connectionInfo, null, CrDBOptionsEnum.crDBOptionMapFieldByRowsetPosition);
		topReportDoc.VerifyDatabase();
		topReportDoc.SaveAs("c:\\m1dev\\m1.net - 9.00\\reports\\ar_invoice_edit\\test.rpt");
		iSCRTable2.ConnectionInfo = connectionInfo;
		topReportDoc.ReportClientDocument.SubreportController.SetTableLocation(subReportDoc.Name, iSCRTable, iSCRTable2);
	}

	private static string CheckExpressionForParametersScript(CrystalParameterCollection parameters, string expression, bool formatForScript)
	{
		for (int num = expression.IndexOf("{?"); num != -1; num = expression.IndexOf("{?"))
		{
			string text = expression.Substring(0, num);
			string text2 = expression.Substring(num);
			num = expression.IndexOf("}");
			if (num == -1)
			{
				break;
			}
			string text3 = expression.Substring(num);
			if (text2.StartsWith("("))
			{
				num = text2.IndexOf(')');
				if (num != -1)
				{
					text3 += text2.Substring(0, num);
					text2 = text2.Substring(num);
				}
			}
			expression = text + GetReportParameterValue(parameters, text3, formatForScript, isArray: false);
		}
		return expression;
	}

	private static string GetReportParameterValue(CrystalParameterCollection parameters, string parameter, bool formatForScript, bool isArray)
	{
		int num = -1;
		if (parameter.EndsWith(")"))
		{
			int num2 = parameter.IndexOf('(');
			if (num2 != -1)
			{
				string text = parameter.Substring(num2 + 1);
				text = text.Substring(0, text.Length - 1);
				num = Convert.ToInt32(text);
				parameter = parameter.Substring(0, num2);
			}
		}
		if (parameters.Contains(parameter))
		{
			CrystalParameter crystalParameter = parameters[parameter];
			if (num == -1)
			{
				num = 1;
			}
			object obj = null;
			if (crystalParameter.Data.FieldOptions[num - 1].Values.Count != 0)
			{
				obj = ((!isArray) ? crystalParameter.Data.FieldOptions[num - 1].Values[0][0] : (formatForScript ? ("Array(" + string.Join(",", crystalParameter.Data.FieldOptions[num - 1].Values.Select((object[] reportValue) => $"\"{reportValue[0]}\"")) + ")") : ((!parameter.StartsWith("Filter_", StringComparison.InvariantCultureIgnoreCase)) ? crystalParameter.Data.FieldOptions[num - 1].Values[0][0] : string.Concat(string.Join(",", crystalParameter.Data.FieldOptions[num - 1].Values.Select((object[] reportValue) => $"{M1Util.ConvertToLinq(reportValue[0])}"))))));
			}
			else if (crystalParameter.Data.FieldOptions[num - 1].DefaultValueExpressions != null && crystalParameter.Data.FieldOptions[num - 1].DefaultValueExpressions.Length != 0)
			{
				obj = crystalParameter.Data.FieldOptions[num - 1].DefaultValueExpressions[0];
			}
			if ((crystalParameter.Data.Fields[0].FieldType.Equals("date", StringComparison.CurrentCultureIgnoreCase) || crystalParameter.Data.Fields[0].FieldType.Equals("datetime", StringComparison.CurrentCultureIgnoreCase) || crystalParameter.Data.Fields[0].FieldType.Equals("smalldatetime", StringComparison.CurrentCultureIgnoreCase)) && M1Util.IsNullOrEmpty(obj) && crystalParameter.Data.InstanceCount == 2)
			{
				obj = ((num != 1) ? ((object)new DateTime(2099, 12, 31)) : ((object)new DateTime(1900, 1, 1)));
			}
			if (formatForScript && crystalParameter.Data.Fields[0].FieldType.Equals("date", StringComparison.CurrentCultureIgnoreCase) && !M1Util.IsNullOrEmpty(obj))
			{
				string shortDatePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
				object obj2 = crystalParameter.Data.FieldOptions[num - 1].Values[0][0];
				obj2 = obj2.ToString().Replace("{", "");
				obj2 = obj2.ToString().Substring(0, obj2.ToString().IndexOf(" "));
				DateTime dateTime = DateTime.ParseExact(obj2.ToString(), shortDatePattern, CultureInfo.InvariantCulture);
				obj = "DateSerial(" + dateTime.Year + "," + dateTime.Month + "," + dateTime.Day + ")";
			}
			if (isArray && formatForScript)
			{
				if (obj == null)
				{
					return "Array()";
				}
				return obj.ToString();
			}
			if (formatForScript)
			{
				return M1Util.ConvertToScript(obj);
			}
			if (obj != null && obj.ToString().StartsWith("'") && obj.ToString().EndsWith("'"))
			{
				return obj.ToString();
			}
			return M1Util.ConvertToSql(obj);
		}
		return parameter;
	}

	private static void loadCrystalParameters(ReportProxy reportContainer, ISCDReportClientDocument reportDoc)
	{
		for (int i = 0; i < reportDoc.DataDefController.DataDefinition.ParameterFields.Count; i++)
		{
			CrystalDecisions.ReportAppServer.DataDefModel.ParameterField parameterField = (CrystalDecisions.ReportAppServer.DataDefModel.ParameterField)reportDoc.DataDefController.DataDefinition.ParameterFields[i];
			if (string.IsNullOrWhiteSpace(parameterField.ReportName))
			{
				reportContainer.Parameters.Add(new CrystalParameter(parameterField.Name, parameterField.Description, Convert.ToInt32(parameterField.Type), parameterField.AllowMultiValue, parameterField.ValueRangeKind != CrParameterValueRangeKindEnum.crParameterValueRangeKindDiscrete, Convert.ToInt32((dynamic)((parameterField.MaximumValue == null) ? ((object)0) : parameterField.MaximumValue.Value)), parameterField.UseCount != 0));
			}
		}
	}

	private static ISCRField getFieldFromDefinition(CrystalDecisions.ReportAppServer.DataDefModel.Database crystalDb, string fieldName)
	{
		for (int i = 0; i < crystalDb.Tables.Count; i++)
		{
			ISCRTable iSCRTable = crystalDb.Tables[i];
			for (int j = 0; j < iSCRTable.DataFields.Count; j++)
			{
				ISCRField iSCRField = iSCRTable.DataFields[j];
				if (iSCRField.Name.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase))
				{
					return iSCRField;
				}
			}
		}
		return null;
	}

	public static string GetWhereClauseForDocumentKeys(ReportAddress address)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (address.TableInfo != null && address.TableInfo.DocumentKeyFields.Length != 0 && address.DocumentKeys.Count != 0)
		{
			for (int i = 0; i < address.DocumentKeys.Count; i++)
			{
				if (i != 0)
				{
					stringBuilder.Append(" Or (");
				}
				else
				{
					stringBuilder.Append("(");
				}
				for (int j = 0; j < address.TableInfo.DocumentKeyFields.Length; j++)
				{
					if (j != 0)
					{
						stringBuilder.Append(" And ");
					}
					stringBuilder.Append(address.TableInfo.DocumentKeyFields[j] + "=" + address.DocumentKeys[i][j].ToSql());
				}
				stringBuilder.Append(")");
			}
		}
		return stringBuilder.ToString();
	}

	private static void checkForContactGroups(ReportProxy report, RowsetCursor rowset, M1Database database)
	{
		ReportAddressDefinition addressDefinition = report.AddressDefinition;
		Dictionary<string, ReportAddress> dictionary = new Dictionary<string, ReportAddress>(StringComparer.CurrentCultureIgnoreCase);
		List<ReportAddress> list = new List<ReportAddress>();
		Dictionary<string, object[]> dictionary2 = new Dictionary<string, object[]>(StringComparer.CurrentCultureIgnoreCase);
		if (addressDefinition != null && !string.IsNullOrWhiteSpace(addressDefinition.DocumentTable))
		{
			if (!string.IsNullOrWhiteSpace(addressDefinition.AddressTable))
			{
				if (addressDefinition.AddressTable.Equals("OrganizationContacts", StringComparison.CurrentCultureIgnoreCase))
				{
					if (addressDefinition.AddressContactFields.Length == 3)
					{
						addressDefinition.AddressQuery = "Select cmlName As OrgName,Case When IsNull(cmcName,'') = '' Then cmlName Else cmcName End As ContactName,Case When IsNull(cmcFaxNumber,'') = '' Then cmlFaxNumber Else cmcFaxNumber End As FaxNumber,Case When IsNull(cmcEMailAddress,'') = '' Then cmlEmailAddress Else cmcEmailAddress End As EMailAddress From OrganizationLocations Left Outer Join OrganizationContacts On cmlOrganizationID = cmcOrganizationID And cmlLocationID=cmcLocationID And cmcContactID = @p2 Where cmlOrganizationID = @p0 And cmlLocationID = @p1";
					}
					else if (addressDefinition.AddressContactFields.Length == 2)
					{
						addressDefinition.AddressQuery = "Select cmlName As OrgName,cmlName As ContactName,cmlFaxNumber As FaxNumber,cmlEmailAddress As EMailAddress From OrganizationLocations Where cmlOrganizationID = @p0 And cmlLocationID = @p1";
					}
					else if (addressDefinition.AddressContactFields.Length == 1)
					{
						addressDefinition.AddressQuery = "Select cmlName As OrgName,cmlName As ContactName,cmlFaxNumber As FaxNumber,cmlEmailAddress As EMailAddress From OrganizationLocations Where cmlOrganizationID = @p0 And cmlLocationID = ''";
					}
				}
				else if (addressDefinition.AddressTable.Equals("Employees", StringComparison.CurrentCultureIgnoreCase))
				{
					string text = "Case When lmeUseEmail = 2 Then lmdPersonalEmailAddress Else lmeWorkEMailAddress End As EmailAddress";
					if (addressDefinition.DocumentTable.Equals("PayrollHeaders", StringComparison.InvariantCultureIgnoreCase))
					{
						text = "Case When lmeUseEmailPayslips = 2 Then lmdPersonalEmailAddress When lmeUseEmailPayslips = 1 Then lmeWorkEMailAddress When lmeUseEmail = 2 Then lmdPersonalEmailAddress Else lmeWorkEMailAddress End As EmailAddress";
					}
					addressDefinition.AddressQuery = "Select xadName As OrgName,lmeEmployeeName As ContactName,lmdFaxNumber As FaxNumber," + text + " From Employees Inner Join EmployeePersonalData On lmeEmployeeID = lmdEmployeeID, DatasetProperties Where lmeEmployeeID = @p0";
				}
			}
			List<object> list2 = new List<object>();
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			StringBuilder stringBuilder3 = new StringBuilder();
			StringBuilder stringBuilder4 = new StringBuilder();
			StringBuilder stringBuilder5 = new StringBuilder();
			StringBuilder stringBuilder6 = new StringBuilder();
			bool flag = true;
			RowsetMetaData rowsetMetaData = (RowsetMetaData)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("981FB6E6-6A02-42C9-862C-128A6CC1720A")));
			List<ISCRField> list3 = new List<ISCRField>();
			string[] addressContactFields = addressDefinition.AddressContactFields;
			foreach (string fieldName in addressContactFields)
			{
				ISCRField fieldFromDefinition = getFieldFromDefinition(report.CrystalRefNew.ReportClientDocument.DatabaseController.Database, fieldName);
				if (fieldFromDefinition != null)
				{
					rowsetMetaData.DataFields.Add(fieldFromDefinition);
					list3.Add(fieldFromDefinition);
				}
			}
			addressContactFields = addressDefinition.DocumentKeyFields;
			foreach (string fieldName2 in addressContactFields)
			{
				ISCRField fieldFromDefinition = getFieldFromDefinition(report.CrystalRefNew.ReportClientDocument.DatabaseController.Database, fieldName2);
				if (fieldFromDefinition != null)
				{
					rowsetMetaData.DataFields.Add(fieldFromDefinition);
				}
				else
				{
					flag = false;
				}
			}
			if (rowsetMetaData.DataFields.Count != 0)
			{
				RowsetCursor rowsetCursor = report.CrystalRefNew.ReportClientDocument.RowsetController.CreateCursor(null, rowsetMetaData);
				rowsetCursor.MoveTo(0);
				while (!rowsetCursor.IsEOF)
				{
					stringBuilder2.Length = 0;
					stringBuilder.Length = 0;
					list2.Clear();
					stringBuilder3.Length = 0;
					stringBuilder4.Length = 0;
					for (int j = 0; j < list3.Count; j++)
					{
						if (j != 0)
						{
							stringBuilder2.Append('|');
						}
						stringBuilder2.Append(((dynamic)rowsetCursor.CurrentRecord[j]).ToString());
						list2.Add((dynamic)rowsetCursor.CurrentRecord[j]);
						if (stringBuilder3.Length != 0)
						{
							stringBuilder3.Append(" And ");
						}
						stringBuilder3.Append(rowsetMetaData.DataFields[j].FormulaForm + "=" + ReportLoaderNew.convertToCrystal((dynamic)rowsetCursor.CurrentRecord[j]));
						if (stringBuilder4.Length != 0)
						{
							stringBuilder4.Append(" And ");
						}
						stringBuilder4.Append(rowsetMetaData.DataFields[j].Name + "=" + M1Util.ConvertToSql((dynamic)rowsetCursor.CurrentRecord[j]));
					}
					if (flag)
					{
						stringBuilder5.Length = 0;
						stringBuilder6.Length = 0;
						object[] array = new object[addressDefinition.DocumentKeyFields.Length];
						for (int k = 0; k < addressDefinition.DocumentKeyFields.Length; k++)
						{
							array[k] = rowsetCursor.CurrentRecord[list3.Count + k];
							if (k != 0)
							{
								stringBuilder.Append('|');
							}
							stringBuilder.Append(((dynamic)rowsetCursor.CurrentRecord[list3.Count + k]).ToString());
							if (stringBuilder5.Length != 0)
							{
								stringBuilder5.Append(" And ");
							}
							stringBuilder5.Append(rowsetMetaData.DataFields[list3.Count + k].FormulaForm + "=" + ReportLoaderNew.convertToCrystal((dynamic)rowsetCursor.CurrentRecord[list3.Count + k]));
							if (stringBuilder6.Length != 0)
							{
								stringBuilder6.Append(" And ");
							}
							stringBuilder6.Append(rowsetMetaData.DataFields[list3.Count + k].Name + "=" + M1Util.ConvertToSql((dynamic)rowsetCursor.CurrentRecord[list3.Count + k]));
						}
						if (!dictionary2.ContainsKey(stringBuilder.ToString()))
						{
							dictionary2.Add(stringBuilder.ToString(), array);
						}
						if (dictionary.ContainsKey(stringBuilder2.ToString()))
						{
							ReportAddress reportAddress = dictionary[stringBuilder2.ToString()];
							if (!isDocumentKeyInAddress(reportAddress, array))
							{
								reportAddress.DocumentKeys.Add(array);
								list.Add(getRelatedAddress(database, addressDefinition, list2.ToArray(), array, stringBuilder5.ToString(), stringBuilder6.ToString()));
							}
						}
						else
						{
							ReportAddress reportAddress = getRelatedAddress(database, addressDefinition, list2.ToArray(), array, stringBuilder3.ToString(), stringBuilder4.ToString());
							dictionary.Add(stringBuilder2.ToString(), reportAddress);
							list.Add(getRelatedAddress(database, addressDefinition, list2.ToArray(), array, stringBuilder5.ToString(), stringBuilder6.ToString()));
						}
					}
					rowsetCursor.MoveNext();
				}
			}
		}
		if (list.Count == 0)
		{
			dictionary.Add(string.Empty, getRelatedAddress(database, addressDefinition, new object[0], null, string.Empty, string.Empty));
			list.Add(getRelatedAddress(database, addressDefinition, new object[0], null, string.Empty, string.Empty));
		}
		processAddressProps(database, report, dictionary.Values.ToList());
		processAddressProps(database, report, list);
		if (addressDefinition != null)
		{
			if (addressDefinition.ScriptObj != null)
			{
				addressDefinition.ScriptObj.Dispose();
				addressDefinition.ScriptObj = null;
			}
			if (addressDefinition.RecordsetObj != null)
			{
				addressDefinition.RecordsetObj.Dispose();
				addressDefinition.RecordsetObj = null;
			}
		}
		if (report.EmailOptions.MultipleRecordsPerContact)
		{
			report.ContactGroups = dictionary.Values.ToArray();
		}
		else
		{
			report.ContactGroups = list.ToArray();
		}
		report.AllDocumentKeys = dictionary2.Values.ToArray();
	}

	private static bool isDocumentKeyInAddress(ReportAddress address, object[] documentKeys)
	{
		bool flag = false;
		foreach (object[] documentKey in address.DocumentKeys)
		{
			flag = true;
			for (int i = 0; i < documentKey.Length; i++)
			{
				if (!documentKeys[i].Equals(documentKey[i]))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				break;
			}
		}
		return flag;
	}

	private static void processAddressProps(M1Database database, ReportProxy report, List<ReportAddress> addresses)
	{
		foreach (ReportAddress address in addresses)
		{
			address.Subject = processEmbeddedExpressions(database, address, report.EmailOptions.EmailSubject);
			if (string.IsNullOrWhiteSpace(address.Subject))
			{
				address.Subject = report.ReportTitle;
			}
			address.Body = processEmbeddedExpressions(database, address, report.EmailOptions.EmailBody);
			address.AttachmentName = processEmbeddedExpressions(database, address, report.EmailOptions.EmailAttachmentName);
			if (string.IsNullOrWhiteSpace(address.AttachmentName) && !string.IsNullOrWhiteSpace(report.ReportTitle))
			{
				string addressDocumentID = getAddressDocumentID(address);
				if (string.IsNullOrWhiteSpace(addressDocumentID))
				{
					address.AttachmentName = report.ReportTitle;
				}
				else
				{
					address.AttachmentName = report.ReportTitle + " - " + addressDocumentID;
				}
			}
			if (string.IsNullOrWhiteSpace(address.AttachmentName))
			{
				address.AttachmentName = address.Subject;
			}
			if (string.IsNullOrWhiteSpace(address.AttachmentName))
			{
				address.AttachmentName = Path.GetFileNameWithoutExtension(report.ReportName);
			}
			address.AttachmentName = GetValidFileName(address.AttachmentName);
		}
	}

	private static string GetValidFileName(string inputString)
	{
		char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
		foreach (char oldChar in invalidFileNameChars)
		{
			inputString = inputString.Replace(oldChar, '_');
		}
		return inputString.PadRight(100).Substring(0, 100).Trim();
	}

	public static string CombineKeys(object[] keys, string separator)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (keys.Length != 0 && !string.IsNullOrWhiteSpace(keys[0].ToString()))
		{
			for (int i = 0; i < keys.Length; i++)
			{
				if (i != 0 && !string.IsNullOrWhiteSpace(separator))
				{
					stringBuilder.Append(separator);
				}
				stringBuilder.Append(keys[i].ToString());
			}
		}
		return stringBuilder.ToString();
	}

	private static string getAddressDocumentID(ReportAddress address)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (object[] documentKey in address.DocumentKeys)
		{
			string value = CombineKeys(documentKey, "-");
			if (!string.IsNullOrWhiteSpace(value))
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(value);
			}
		}
		return stringBuilder.ToString();
	}

	private static string processEmbeddedExpressions(M1Database database, ReportAddress address, string text)
	{
		if (!string.IsNullOrWhiteSpace(text) && address != null && address.TableInfo != null)
		{
			if (text.IndexOf("<DocumentID>", StringComparison.CurrentCultureIgnoreCase) != -1)
			{
				text = text.Replace("<DocumentID>", getAddressDocumentID(address), caseInsensitive: true);
			}
			if (text.StartsWith("\"") && text.IndexOf("Fields(", StringComparison.CurrentCultureIgnoreCase) == -1)
			{
				text = Convert.ToString(database.Scripting.Eval(text));
			}
			if (text.IndexOf("Fields(", StringComparison.CurrentCultureIgnoreCase) != -1 && !string.IsNullOrWhiteSpace(address.TableInfo.DocumentTable) && address.TableInfo.DocumentKeyFields.Length != 0 && !string.IsNullOrWhiteSpace(address.TableInfo.DocumentKeyFields[0]))
			{
				ReferencedFieldsList referencedFieldsList = new ReferencedFieldsList(text);
				SqlCommand sqlCommand;
				if (address.TableInfo.DocumentKeyFields.Length == 2)
				{
					sqlCommand = database.NewSqlCommand("Select " + referencedFieldsList.FieldList() + " From " + address.TableInfo.DocumentTable + " Where " + address.TableInfo.DocumentKeyFields[0] + " = @ID And " + address.TableInfo.DocumentKeyFields[1] + " = @ID2");
					sqlCommand.Parameters.Add(new SqlParameter("@ID", address.DocumentKeys[0][0]));
					sqlCommand.Parameters.Add(new SqlParameter("@ID2", address.DocumentKeys[0][1]));
				}
				else
				{
					sqlCommand = database.NewSqlCommand("Select " + referencedFieldsList.FieldList() + " From " + address.TableInfo.DocumentTable + " Where " + address.TableInfo.DocumentKeyFields[0] + " = @ID");
					sqlCommand.Parameters.Add(new SqlParameter("@ID", address.DocumentKeys[0][0]));
				}
				DataTable dataTable = database.GetDataTable(sqlCommand);
				if (dataTable.Rows.Count != 0)
				{
					if (address.TableInfo.ScriptObj == null)
					{
						address.TableInfo.ScriptObj = new ScriptingBase(database);
						address.TableInfo.ScriptObj.LoadEnvironment();
						address.TableInfo.RecordsetObj = new M1AdoRecordsetProxy();
						address.TableInfo.ScriptObj.AddObject("Fields", address.TableInfo.RecordsetObj.FieldsCollection);
					}
					address.TableInfo.RecordsetObj.LoadDataTable(dataTable);
					text = Convert.ToString(address.TableInfo.ScriptObj.Eval(text));
				}
			}
		}
		return text;
	}

	private static ReportAddress getRelatedAddress(M1Database database, ReportAddressDefinition addressDefinition, object[] addressKeys, object[] documentKey, string crystalFilter, string sqlFilter)
	{
		ReportAddress reportAddress = new ReportAddress();
		reportAddress.CrystalFilter = crystalFilter;
		reportAddress.SqlFilter = sqlFilter;
		reportAddress.TableInfo = addressDefinition;
		reportAddress.AddressKeys = addressKeys;
		reportAddress.DocumentKeys = new List<object[]>();
		if (documentKey != null && documentKey.Length != 0)
		{
			reportAddress.DocumentKeys.Add(documentKey);
		}
		if (addressDefinition != null && !string.IsNullOrWhiteSpace(addressDefinition.AddressQuery) && addressKeys != null && addressKeys.Length != 0)
		{
			SqlCommand sqlCommand = database.NewSqlCommand(addressDefinition.AddressQuery);
			for (int i = 0; i < addressKeys.Length; i++)
			{
				sqlCommand.Parameters.Add(new SqlParameter("@p" + i, addressKeys[i]));
			}
			for (int j = addressKeys.Length; j < addressDefinition.AddressContactFields.Length; j++)
			{
				sqlCommand.Parameters.Add(new SqlParameter("@p" + j, string.Empty));
			}
			using DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				DataRow row = dataTable.Rows[0];
				reportAddress.OrganizationName = row.Field<string>("OrgName");
				reportAddress.ContactName = row.Field<string>("ContactName");
				if (row.Field<string>("EmailAddress") != null)
				{
					reportAddress.Email = Regex.Replace(row.Field<string>("EmailAddress"), "\\s+", "");
				}
				reportAddress.Fax = row.Field<string>("FaxNumber");
			}
		}
		return reportAddress;
	}

	private static void formatReport(ISCDReportClientDocument reportDoc)
	{
		int width = reportDoc.ReportDefController.ReportDefinition.DetailArea.Sections[0].Width;
		formatObjects(reportDoc.ReportDefController.ReportObjectController, width, reportDoc.ReportDefController.ReportObjectController.GetReportObjectsByKind(CrReportObjectKindEnum.crReportObjectKindBox).OfType<CrystalDecisions.ReportAppServer.ReportDefModel.BoxObject>());
		foreach (string subreportName in reportDoc.SubreportController.GetSubreportNames())
		{
			SubreportClientDocument subreport = reportDoc.SubreportController.GetSubreport(subreportName);
			formatObjects(reportObjects: subreport.ReportDefController.ReportObjectController.GetReportObjectsByKind(CrReportObjectKindEnum.crReportObjectKindBox).OfType<CrystalDecisions.ReportAppServer.ReportDefModel.BoxObject>(), controller: subreport.ReportDefController.ReportObjectController, width: width);
		}
	}

	private static void formatObjects(ReportObjectController controller, int width, IEnumerable<CrystalDecisions.ReportAppServer.ReportDefModel.BoxObject> reportObjects)
	{
		foreach (CrystalDecisions.ReportAppServer.ReportDefModel.BoxObject reportObject in reportObjects)
		{
			if (reportObject.Height < 500)
			{
				reportObject.Bottom -= reportObject.Height - 15;
				reportObject.Height = 15;
			}
			else
			{
				reportObject.FillColor = (uint)ColorTranslator.ToOle(Color.White);
				reportObject.LineColor = (uint)ColorTranslator.ToOle(Color.Gray);
			}
			reportObject.Border.HasDropShadow = false;
			reportObject.CornerEllipseHeight = 0;
			reportObject.CornerEllipseWidth = 0;
			controller.Modify(reportObject, reportObject);
		}
	}

	private static List<SqlExpressionUpdateInfo> processSqlExpressions(ISCDReportClientDocument report, M1Database database)
	{
		List<SqlExpressionUpdateInfo> list = new List<SqlExpressionUpdateInfo>();
		Strings subreportNames = report.SubreportController.GetSubreportNames();
		updateSqlExpressions(report.DataDefController.DataDefinition, report.DataDefController.FormulaFieldController, list);
		foreach (string item in subreportNames)
		{
			SubreportClientDocument subreport = report.SubreportController.GetSubreport(item);
			updateSqlExpressions(subreport.DataDefController.DataDefinition, subreport.DataDefController.FormulaFieldController, list);
		}
		return list;
	}

	private static void checkSqlExpressions(M1Database database, List<SqlExpressionUpdateInfo> sqlExpressionsWithParameters, CrystalParameterCollection parameters, string whereClause)
	{
		if (sqlExpressionsWithParameters == null || sqlExpressionsWithParameters.Count == 0)
		{
			return;
		}
		foreach (SqlExpressionUpdateInfo sqlExpressionsWithParameter in sqlExpressionsWithParameters)
		{
			FormulaField formulaField = (FormulaField)sqlExpressionsWithParameter.FormulaField;
			formulaField.Text = ProcessTextForClauses(parameters, database, sqlExpressionsWithParameter.FormulaText.Replace("M1_M1", database.ID, caseInsensitive: true), whereClause, isScript: false, removeQuotes: true);
			((FormulaFieldController)sqlExpressionsWithParameter.FormulaFieldController).Modify(sqlExpressionsWithParameter.FormulaIndex, formulaField);
		}
	}

	private static bool processLogin(ReportDocument report, CrystalParameterCollection parameters, AppContext context, string databaseName)
	{
		ServerManager serverManager = ((parameters.Contains("ConnectionInfo") && parameters["ConnectionInfo"].Text.Equals("DataDictionary", StringComparison.CurrentCultureIgnoreCase)) ? context.DDServerManager : context.DBServerManager);
		CrystalDecisions.Shared.ConnectionInfo connectionInfo = new CrystalDecisions.Shared.ConnectionInfo
		{
			DatabaseName = databaseName,
			Password = serverManager.sqlPassword,
			UserID = serverManager.ConnectionInfo.SqlUserID,
			ServerName = serverManager.ConnectionInfo.Server,
			IntegratedSecurity = serverManager.ConnectionInfo.TrustedConnection
		};
		bool result = logonToDb(report.Database, connectionInfo);
		Strings subreportNames = report.ReportClientDocument.SubreportController.GetSubreportNames();
		for (int i = 0; i < report.Subreports.Count; i++)
		{
			logonToDb(report.Subreports[subreportNames[i]].Database, connectionInfo);
		}
		return result;
	}

	private static void deleteSqlExpressions(CrystalDecisions.ReportAppServer.DataDefModel.DataDefinition dataDefinition, FormulaFieldController formulaFieldController)
	{
		for (int num = dataDefinition.FormulaFields.Count - 1; num >= 0; num--)
		{
			FormulaField formulaField = (FormulaField)dataDefinition.FormulaFields[num];
			if (formulaField.Syntax == CrFormulaSyntaxEnum.crFormulaSyntaxSQL)
			{
				formulaFieldController.Remove(formulaField);
			}
		}
	}

	private static void updateSqlExpressions(CrystalDecisions.ReportAppServer.DataDefModel.DataDefinition dataDefinition, FormulaFieldController formulaFieldController, List<SqlExpressionUpdateInfo> sqlExpressionsWithParameters)
	{
		for (int i = 0; i < dataDefinition.FormulaFields.Count; i++)
		{
			FormulaField formulaField = (FormulaField)dataDefinition.FormulaFields[i];
			if (formulaField.Syntax == CrFormulaSyntaxEnum.crFormulaSyntaxSQL && formulaField.UseCount != 0 && (formulaField.Text.IndexOf("M1_M1", StringComparison.CurrentCultureIgnoreCase) != -1 || formulaField.Text.IndexOf("{?") != -1))
			{
				sqlExpressionsWithParameters.Add(new SqlExpressionUpdateInfo(formulaFieldController, i, formulaField, formulaField.Text));
			}
		}
	}

	private static string getDatabaseDll(PropertyBag attributes)
	{
		if (attributes.PropertyIDs.FindIndexOf("Database DLL") != -1)
		{
			return ((ISCRPropertyBag)attributes).get_StringValue("Database DLL");
		}
		return string.Empty;
	}

	private static string getDatabaseDll(DbConnectionAttributes attributes)
	{
		for (int i = 0; i < attributes.Collection.Count; i++)
		{
			NameValuePair2 nameValuePair = (NameValuePair2)attributes.Collection[i];
			if (nameValuePair.Name.ToString().Equals("Database DLL", StringComparison.CurrentCultureIgnoreCase))
			{
				return nameValuePair.Value.ToString();
			}
		}
		return string.Empty;
	}

	private static bool isSqlTable(PropertyBag attributes)
	{
		string databaseDll = getDatabaseDll(attributes);
		if (databaseDll.Equals("crdb_fielddef.dll", StringComparison.CurrentCultureIgnoreCase) || databaseDll.Equals("crdb_adoplus.dll", StringComparison.CurrentCultureIgnoreCase) || databaseDll.Equals("crdb_dataset.dll", StringComparison.CurrentCultureIgnoreCase))
		{
			return false;
		}
		return true;
	}

	private static bool isSqlTable(DbConnectionAttributes attributes)
	{
		string databaseDll = getDatabaseDll(attributes);
		if (databaseDll.Equals("crdb_fielddef.dll", StringComparison.CurrentCultureIgnoreCase) || databaseDll.Equals("crdb_adoplus.dll", StringComparison.CurrentCultureIgnoreCase) || databaseDll.Equals("crdb_dataset.dll", StringComparison.CurrentCultureIgnoreCase))
		{
			return false;
		}
		return true;
	}

	private static bool logonToDb(DatabaseController dbController, CrystalDecisions.ReportAppServer.DataDefModel.Database crystalDb, string databaseName, ServerManager manager)
	{
		ConnectionInfos connectionInfos = dbController.GetConnectionInfos();
		foreach (CrystalDecisions.ReportAppServer.DataDefModel.ConnectionInfo item in connectionInfos)
		{
			if (isSqlTable(item.Attributes))
			{
				_ = item.Attributes.PropertyIDs;
				((ISCRPropertyBag)item.Attributes).set_StringValue("QE_DatabaseName", databaseName);
				((ISCRPropertyBag)item.Attributes).set_StringValue("QE_ServerDescription", manager.ConnectionInfo.Server);
				PropertyBag propertyBag = (PropertyBag)(dynamic)((ISCRPropertyBag)item.Attributes).get_ObjectValue("QE_LogonProperties");
				((ISCRPropertyBag)propertyBag).set_StringValue("Initial Catalog", databaseName);
				((ISCRPropertyBag)propertyBag).set_StringValue("PreQEDatabaseName", databaseName);
				((ISCRPropertyBag)propertyBag).set_BoolValue("Integrated Security", manager.ConnectionInfo.TrustedConnection);
				((ISCRPropertyBag)propertyBag).set_StringValue("PreQEServerName", manager.ConnectionInfo.Server);
				((ISCRPropertyBag)propertyBag).set_StringValue("PreQEServerName", manager.ConnectionInfo.Server);
				((ISCRPropertyBag)item.Attributes).set_ObjectValue("QE_LogonProperties", (object)propertyBag);
			}
		}
		dbController.SetConnectionInfos(connectionInfos);
		return true;
	}

	private static bool logonToDb(CrystalDecisions.CrystalReports.Engine.Database crystalDb, CrystalDecisions.Shared.ConnectionInfo connectionInfo)
	{
		Dictionary<CrystalDecisions.CrystalReports.Engine.Table, TableLogOnInfo> dictionary = new Dictionary<CrystalDecisions.CrystalReports.Engine.Table, TableLogOnInfo>();
		for (int i = 0; i < crystalDb.Tables.Count; i++)
		{
			CrystalDecisions.CrystalReports.Engine.Table table = crystalDb.Tables[i];
			if (isSqlTable(table.LogOnInfo.ConnectionInfo.Attributes))
			{
				TableLogOnInfo logOnInfo = table.LogOnInfo;
				logOnInfo.ConnectionInfo = connectionInfo;
				dictionary.Add(table, logOnInfo);
			}
		}
		foreach (KeyValuePair<CrystalDecisions.CrystalReports.Engine.Table, TableLogOnInfo> item in dictionary)
		{
			item.Key.ApplyLogOnInfo(item.Value);
			item.Key.TestConnectivity();
		}
		return dictionary.Count != 0;
	}

	private static void attachDataToReport(CrystalParameterCollection parameters, Dictionary<string, ReportTableSource> tableSources, ReportDocument report, M1Database database, ReportWhere reportWhere)
	{
		attachDataToDb(parameters, tableSources, report.ReportClientDocument.SubreportController, report.ReportClientDocument.DatabaseController.Database, report.Database, report, database, reportWhere.SqlWhere);
		Strings subreportNames = report.ReportClientDocument.SubreportController.GetSubreportNames();
		for (int i = 0; i < report.Subreports.Count; i++)
		{
			ReportDocument reportDocument = report.Subreports[subreportNames[i]];
			attachDataToDb(parameters, tableSources, report.ReportClientDocument.SubreportController, report.ReportClientDocument.SubreportController.GetSubreportDatabase(subreportNames[i]), reportDocument.Database, reportDocument, database, reportWhere.SqlWhere);
			reportDocument = null;
		}
	}

	private static void attachDataToDb(CrystalParameterCollection parameters, Dictionary<string, ReportTableSource> tableSources, SubreportController subReportController, CrystalDecisions.ReportAppServer.DataDefModel.Database crystalDb, CrystalDecisions.CrystalReports.Engine.Database oldDb, ReportDocument reportDoc, M1Database database, string sqlWhere)
	{
		for (int i = 0; i < crystalDb.Tables.Count; i++)
		{
			CrystalDecisions.ReportAppServer.DataDefModel.Table table = (CrystalDecisions.ReportAppServer.DataDefModel.Table)crystalDb.Tables[i];
			if (!isSqlTable(table.ConnectionInfo.Attributes))
			{
				attachDataToTable(parameters, tableSources, subReportController, reportDoc, database, sqlWhere, crystalDb, table, oldDb.Tables[i]);
			}
		}
	}

	private static ReportWhere GetWhereClause(CrystalParameterCollection parameters, CrystalDecisions.ReportAppServer.DataDefModel.Database crystalDb, M1Database database, int dataRow, string extraWhere)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		int num = 0;
		ReportWhere reportWhere = new ReportWhere();
		foreach (CrystalParameter parameter in parameters)
		{
			if (parameter.Data == null || parameter.Data.Fields == null || parameter.Data.Fields.Count == 0)
			{
				continue;
			}
			if (GetWhereClauseForParameter(parameter, crystalDb, stringBuilder, stringBuilder2, dataRow))
			{
				num++;
			}
			string rowFilter = database.Security.GetRowFilter(parameter.Data.Fields.Last().RelatedTable);
			if (!string.IsNullOrWhiteSpace(rowFilter))
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(" And ");
				}
				stringBuilder.Append(rowFilter);
				string value = ParseExpression(crystalDb, rowFilter).Replace('%', '*');
				if (stringBuilder2.Length != 0)
				{
					stringBuilder2.Append(" And ");
				}
				stringBuilder2.Append(value);
			}
		}
		if (stringBuilder.Length == 0)
		{
			if (num == 0)
			{
				stringBuilder.Append("0=0");
			}
			else
			{
				stringBuilder.Append("0=1");
				stringBuilder2.Append("0=1");
			}
		}
		else if (!string.IsNullOrWhiteSpace(extraWhere))
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(" And ");
			}
			stringBuilder.Append(extraWhere);
			if (stringBuilder2.Length != 0)
			{
				stringBuilder2.Append(" And ");
			}
			stringBuilder2.Append(extraWhere);
		}
		reportWhere.SqlWhere = stringBuilder.ToString();
		reportWhere.CrystalWhere = stringBuilder2.ToString();
		return reportWhere;
	}

	private static int getMaxValuesCount(ReportParameterData parameter)
	{
		int num = 0;
		CrystalFieldOption[] fieldOptions = parameter.FieldOptions;
		foreach (CrystalFieldOption crystalFieldOption in fieldOptions)
		{
			if (crystalFieldOption.Values != null && crystalFieldOption.Values.Count > num)
			{
				num = crystalFieldOption.Values.Count;
			}
		}
		return num;
	}

	private static string ParseExpression(CrystalDecisions.ReportAppServer.DataDefModel.Database crystalDb, string expression)
	{
		string[] array = expression.Split(new string[14]
		{
			"AND", "OR", "=", "<>", ">=", "<=", ">", "<", "(", ")",
			",", "{", "}", " "
		}, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			ISCRField fieldFromDefinition = getFieldFromDefinition(crystalDb, array[i]);
			if (fieldFromDefinition == null)
			{
				continue;
			}
			string formulaForm = fieldFromDefinition.FormulaForm;
			expression = expression.Replace(array[i], (formulaForm.Length == 0) ? array[i] : formulaForm);
			if (fieldFromDefinition.Type == CrFieldValueTypeEnum.crFieldValueTypeDateTimeField && array.Length > i + 1)
			{
				int num = expression.IndexOf(formulaForm, StringComparison.CurrentCultureIgnoreCase);
				if (num != -1)
				{
					num = expression.IndexOf(array[i + 1], num);
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(expression.Substring(0, num));
					expression = expression.Substring(num + array[i + 1].Length);
					if (array[i + 1].Equals("GetDate", StringComparison.CurrentCultureIgnoreCase))
					{
						stringBuilder.Append("CurrentDateTime");
					}
					else
					{
						stringBuilder.Append(array[i + 1] + expression);
					}
					expression = stringBuilder.ToString();
				}
			}
			if (fieldFromDefinition.Type == CrFieldValueTypeEnum.crFieldValueTypeBooleanField && array.Length > i + 1)
			{
				int num2 = expression.IndexOf(formulaForm, StringComparison.CurrentCultureIgnoreCase);
				if (num2 != -1)
				{
					num2 = expression.IndexOf(array[i + 1], num2);
					StringBuilder stringBuilder2 = new StringBuilder();
					stringBuilder2.Append(expression.Substring(0, num2));
					expression = expression.Substring(num2 + array[i + 1].Length);
					if (array[i + 1].Equals("0"))
					{
						stringBuilder2.Append("False");
					}
					else
					{
						stringBuilder2.Append("True");
					}
					stringBuilder2.Append(expression);
					expression = stringBuilder2.ToString();
				}
			}
			if (array.Length > i + 2 && array[i + 1].Equals("is", StringComparison.CurrentCultureIgnoreCase) && array[i + 2].Equals("null", StringComparison.CurrentCultureIgnoreCase))
			{
				int num3 = expression.IndexOf(formulaForm, StringComparison.CurrentCultureIgnoreCase);
				if (num3 != -1)
				{
					StringBuilder stringBuilder3 = new StringBuilder();
					stringBuilder3.Append(expression.Substring(0, num3));
					stringBuilder3.Append("IsNull(" + formulaForm + ")");
					num3 = expression.IndexOf("null", num3 + formulaForm.Length);
					expression = expression.Substring(num3 + "null".Length);
					stringBuilder3.Append(expression);
					expression = stringBuilder3.ToString();
				}
			}
		}
		return expression;
	}

	private static bool GetWhereClauseForParameter(CrystalParameter parm, CrystalDecisions.ReportAppServer.DataDefModel.Database crystalDb, StringBuilder sqlBuilder, StringBuilder crystalBuilder, int dataRow)
	{
		ReportParameterData data = parm.Data;
		if (data.DisplayType == ReportDisplayType.DropDownselect || data.DisplayType == ReportDisplayType.DatasetSelect || data.DisplayType == ReportDisplayType.Label)
		{
			return false;
		}
		bool result = false;
		if (data.DisplayType == ReportDisplayType.DropDown)
		{
			DropDownTextFilter valueListItem = data.Fields[0].GetValueListItem(data.FieldOptions[0].Values[0][0]);
			if (valueListItem != null && !string.IsNullOrWhiteSpace(valueListItem.Filter))
			{
				if (sqlBuilder.Length != 0)
				{
					sqlBuilder.Append(" And ");
				}
				sqlBuilder.Append(valueListItem.Filter);
				if (crystalBuilder.Length != 0)
				{
					crystalBuilder.Append(" And ");
				}
				crystalBuilder.Append(ParseExpression(crystalDb, valueListItem.Filter));
			}
		}
		else if (data.DisplayType == ReportDisplayType.Prompt || data.DisplayType == ReportDisplayType.CheckBoxGroup)
		{
			string filter = data.FieldOptions[0].Filter;
			ReportPromptFieldInfo reportPromptFieldInfo = data.Fields[0];
			if (!string.IsNullOrWhiteSpace(filter))
			{
				string text = null;
				result = parm.Data.Required;
				if (!M1Util.IsNullOrEmpty(data.FieldOptions[0].Values[0][0]))
				{
					if (reportPromptFieldInfo.FieldType.Equals("bit", StringComparison.CurrentCultureIgnoreCase))
					{
						filter = filter.Replace("?", M1Util.ConvertToSql((DateTime)data.FieldOptions[0].Values[0][0]));
					}
					else if (reportPromptFieldInfo.FieldType.Equals("date", StringComparison.CurrentCultureIgnoreCase))
					{
						DateTime dateTime = (DateTime)data.FieldOptions[0].Values[0][0];
						text = filter.Replace("?", $"Date({dateTime.Year.ToString()},{dateTime.Month.ToString()},{dateTime.Day.ToString()})").Replace("DateAdd(d", "DateAdd('d'", caseInsensitive: true);
						filter = filter.Replace("?", M1Util.ConvertToSql((DateTime)data.FieldOptions[0].Values[0][0], dateAsDateOnly: true));
					}
					else if (reportPromptFieldInfo.FieldType.Equals("datetime", StringComparison.CurrentCultureIgnoreCase) || reportPromptFieldInfo.FieldType.Equals("smalldatetime", StringComparison.CurrentCultureIgnoreCase))
					{
						filter = filter.Replace("?", M1Util.ConvertToSql((DateTime)data.FieldOptions[0].Values[0][0]));
					}
					else
					{
						if (!reportPromptFieldInfo.FieldType.Equals("numeric", StringComparison.CurrentCultureIgnoreCase) && !reportPromptFieldInfo.FieldType.Equals("int", StringComparison.CurrentCultureIgnoreCase) && !reportPromptFieldInfo.FieldType.Equals("money", StringComparison.CurrentCultureIgnoreCase) && !reportPromptFieldInfo.FieldType.Equals("smallint", StringComparison.CurrentCultureIgnoreCase) && !reportPromptFieldInfo.FieldType.Equals("tinyint", StringComparison.CurrentCultureIgnoreCase) && !reportPromptFieldInfo.FieldType.Equals("bigint", StringComparison.CurrentCultureIgnoreCase))
						{
							throw new M1Exception($"Unsupported field type {reportPromptFieldInfo.FieldType} in GetWhereClauseForParameter");
						}
						filter = filter.Replace("?", M1Util.ConvertToSql(Convert.ToDecimal(data.FieldOptions[0].Values[0][0])));
					}
					if (sqlBuilder.Length != 0)
					{
						sqlBuilder.Append(" And ");
					}
					sqlBuilder.Append(filter);
					if (crystalBuilder.Length != 0)
					{
						crystalBuilder.Append(" And ");
					}
					if (text != null)
					{
						crystalBuilder.Append(ParseExpression(crystalDb, text));
					}
					else
					{
						crystalBuilder.Append(ParseExpression(crystalDb, filter));
					}
				}
			}
		}
		else if (data.InstanceCount != 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			StringBuilder stringBuilder3 = new StringBuilder();
			StringBuilder stringBuilder4 = new StringBuilder();
			_ = data.Fields[0];
			if (data.DisplayType == ReportDisplayType.Filter || data.DisplayType == ReportDisplayType.YearPeriod)
			{
				result = parm.Data.Required;
				string tableDot = data.Table + ".";
				int maxValuesCount = getMaxValuesCount(data);
				for (int i = 0; i < maxValuesCount; i++)
				{
					if (dataRow != -1 && maxValuesCount != 1 && dataRow != i)
					{
						continue;
					}
					stringBuilder3.Length = 0;
					stringBuilder4.Length = 0;
					for (int j = 0; j < data.FieldOptions.Length; j++)
					{
						CrystalFieldOption crystalFieldOption = data.FieldOptions[j];
						if (crystalFieldOption.Values == null || crystalFieldOption.Values.Count <= i || M1Util.IsNullOrEmpty(crystalFieldOption.Values[i][0]))
						{
							continue;
						}
						if (stringBuilder3.Length != 0)
						{
							stringBuilder3.Append(" And ");
						}
						if (stringBuilder4.Length != 0)
						{
							stringBuilder4.Append(" And ");
						}
						object[] values = crystalFieldOption.Values[i];
						string text2 = crystalFieldOption.Operator.Replace("=", string.Empty);
						stringBuilder3.Append("(");
						stringBuilder4.Append("(");
						if (crystalFieldOption.Operator.IndexOf('=') != -1)
						{
							GenerateWhereForFields(data, stringBuilder3, stringBuilder4, values, tableDot, data.Fields.Count - 1, "=");
						}
						if (!string.IsNullOrWhiteSpace(text2))
						{
							if (crystalFieldOption.Operator.IndexOf('=') != -1)
							{
								stringBuilder3.Append(" Or ");
								stringBuilder4.Append(" Or ");
							}
							for (int num = data.Fields.Count - 1; num >= 0; num--)
							{
								GenerateWhereForFields(data, stringBuilder3, stringBuilder4, values, tableDot, num, text2);
								if (num != 0)
								{
									stringBuilder3.Append(" Or ");
									stringBuilder4.Append(" Or ");
								}
							}
						}
						stringBuilder3.Append(")");
						stringBuilder4.Append(")");
					}
					if (stringBuilder3.Length != 0)
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.Append(" Or ");
						}
						stringBuilder.Append("(" + stringBuilder3.ToString() + ")");
					}
					if (stringBuilder4.Length != 0)
					{
						if (stringBuilder2.Length != 0)
						{
							stringBuilder2.Append(" Or ");
						}
						stringBuilder2.Append("(" + stringBuilder4.ToString() + ")");
					}
				}
				if (stringBuilder.Length != 0)
				{
					string text3 = stringBuilder.ToString();
					if (!string.IsNullOrWhiteSpace(data.FieldOptions[0].Filter))
					{
						text3 = data.FieldOptions[0].Filter.Replace("?", text3);
					}
					if (sqlBuilder.Length != 0)
					{
						sqlBuilder.Append(" And ");
					}
					sqlBuilder.Append("(" + text3 + ")");
				}
				if (stringBuilder2.Length != 0)
				{
					string text4 = stringBuilder2.ToString();
					if (!string.IsNullOrWhiteSpace(data.FieldOptions[0].Filter))
					{
						text4 = data.FieldOptions[0].Filter.Replace("?", text4);
					}
					if (crystalBuilder.Length != 0)
					{
						crystalBuilder.Append(" And ");
					}
					crystalBuilder.Append("(" + text4 + ")");
				}
			}
		}
		return result;
	}

	private static void GenerateWhereForFields(ReportParameterData parameter, StringBuilder sqlWhere, StringBuilder crystalWhere, object[] values, string tableDot, int finalFieldIndex, string finalOperator)
	{
		sqlWhere.Append("(");
		crystalWhere.Append("(");
		for (int i = 0; i < finalFieldIndex; i++)
		{
			if (parameter.Fields[i].FieldType.Equals("date", StringComparison.CurrentCultureIgnoreCase))
			{
				DateTime dateTime = Convert.ToDateTime(values[i]);
				DateTime dateTime2 = dateTime.AddDays(1.0);
				sqlWhere.Append(tableDot + parameter.Fields[i].FieldName + ">=" + M1Util.ConvertToSql(dateTime) + " And " + tableDot + parameter.Fields[i].FieldName + "<" + M1Util.ConvertToSql(dateTime2));
				crystalWhere.Append("{" + tableDot + parameter.Fields[i].FieldName + "}>=" + convertToCrystal(dateTime) + " And {" + tableDot + parameter.Fields[i].FieldName + "}<" + convertToCrystal(dateTime2));
			}
			else
			{
				sqlWhere.Append(tableDot + parameter.Fields[i].FieldName + "=" + M1Util.ConvertToSql(values[i]));
				crystalWhere.Append("{" + tableDot + parameter.Fields[i].FieldName + "}=" + convertToCrystal(values[i]));
			}
			sqlWhere.Append(" And ");
			crystalWhere.Append(" And ");
		}
		object obj = values[finalFieldIndex];
		if (parameter.Fields[finalFieldIndex].FieldType.Equals("date", StringComparison.CurrentCultureIgnoreCase))
		{
			if (finalOperator.Equals("="))
			{
				DateTime dateTime = Convert.ToDateTime(obj);
				DateTime dateTime2 = dateTime.AddDays(1.0);
				sqlWhere.Append(tableDot + parameter.Fields[finalFieldIndex].FieldName + ">=" + M1Util.ConvertToSql(dateTime) + " And " + tableDot + parameter.Fields[finalFieldIndex].FieldName + "<" + M1Util.ConvertToSql(dateTime2));
				crystalWhere.Append("{" + tableDot + parameter.Fields[finalFieldIndex].FieldName + "}>=" + convertToCrystal(dateTime) + " And {" + tableDot + parameter.Fields[finalFieldIndex].FieldName + "}<" + convertToCrystal(dateTime2));
			}
			else
			{
				sqlWhere.Append(tableDot + parameter.Fields[finalFieldIndex].FieldName + finalOperator + M1Util.ConvertToSql(obj));
				crystalWhere.Append("{" + tableDot + parameter.Fields[finalFieldIndex].FieldName + "}" + finalOperator + convertToCrystal(obj));
			}
		}
		else
		{
			sqlWhere.Append(tableDot + parameter.Fields[finalFieldIndex].FieldName + finalOperator + M1Util.ConvertToSql(obj));
			crystalWhere.Append("{" + tableDot + parameter.Fields[finalFieldIndex].FieldName + "}" + finalOperator + convertToCrystal(obj));
		}
		sqlWhere.Append(")");
		crystalWhere.Append(")");
	}

	private static string convertToCrystal(object value)
	{
		if (value.GetType() == typeof(DateTime))
		{
			DateTime dateTime = (DateTime)value;
			return "CDateTime(" + dateTime.Year + "," + dateTime.Month + "," + dateTime.Day + "," + dateTime.Hour + "," + dateTime.Minute + "," + dateTime.Second + ")";
		}
		return M1Util.ConvertToLinq(value);
	}

	private static void InitializeValues(IServiceProvider provider, ReportProxy reportContainer)
	{
		M1DataDictionary dd = provider.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		M1Database database = provider.GetService(typeof(M1Database)) as M1Database;
		LoadSavedSettings(dd, database, reportContainer);
		foreach (CrystalParameter parameter in reportContainer.Parameters)
		{
			CrystalFieldOption[] fieldOptions = parameter.Data.FieldOptions;
			foreach (CrystalFieldOption crystalFieldOption in fieldOptions)
			{
				crystalFieldOption.Values.Clear();
				if (parameter.EnableMultipleValues)
				{
					continue;
				}
				object[] array = new object[parameter.Data.Fields.Count];
				if (parameter.Data.CanBeSaved && parameter.Data.Fields.Count == 1)
				{
					array[0] = GetSavedValue(reportContainer, parameter.Name, getFieldOptionValue(database, crystalFieldOption, 0, parameter.Data.Fields[0].FieldType), parameter.Data.Fields[0].FieldType);
				}
				else
				{
					for (int j = 0; j < parameter.Data.Fields.Count; j++)
					{
						array[j] = getFieldOptionValue(database, crystalFieldOption, j, parameter.Data.Fields[j].FieldType);
					}
				}
				crystalFieldOption.Values.Add(array);
			}
			parameter.Data.OnDataChanged(EventArgs.Empty);
		}
		CheckIfParametersAreValid(reportContainer);
	}

	private static void SaveParameterValues(IServiceProvider provider, ReportProxy reportContainer)
	{
		foreach (CrystalParameter parameter in reportContainer.Parameters)
		{
			CrystalFieldOption[] fieldOptions = parameter.Data.FieldOptions;
			foreach (CrystalFieldOption crystalFieldOption in fieldOptions)
			{
				if (parameter.Data != null && parameter.Data.CanBeSaved && parameter.Data.Fields.Count == 1 && crystalFieldOption.Values.Count == 1)
				{
					object obj = crystalFieldOption.Values[0][0];
					SetSavedValue(value: (obj == null) ? string.Empty : ((!(obj.GetType() == typeof(string))) ? obj.ToString() : ("\"" + obj.ToString() + "\"")), reportContainer: reportContainer, name: parameter.Name);
				}
			}
		}
		SetSavedValue(reportContainer, "Printer", "\"" + reportContainer.PrintOptions.PrinterName + "\"");
		SetSavedValue(reportContainer, "Copies", reportContainer.PrintOptions.Copies.ToString());
		SetSavedValue(reportContainer, "Collate", reportContainer.PrintOptions.Collate.ToString());
		SetSavedValue(reportContainer, "Tray", reportContainer.PrintOptions.Tray);
		SetSavedValue(reportContainer, "Duplex", reportContainer.PrintOptions.Duplex.ToString());
		SetSavedValue(reportContainer, "SuppressRelatedDocumentsPrompt", reportContainer.PrintOptions.SuppressRelatedDocumentsPrompt.ToString());
		FlushSettingsToDb(provider, reportContainer);
	}

	private static int checkAddressFields(ReportAddressDefinition addressDefinition, ISCDReportClientDocument reportDoc)
	{
		int num = 0;
		if (addressDefinition.AddressContactFields != null && addressDefinition.AddressContactFields.Length != 0)
		{
			List<string> list = new List<string>();
			string[] addressContactFields = addressDefinition.AddressContactFields;
			foreach (string text in addressContactFields)
			{
				ISCRField fieldFromDefinition = getFieldFromDefinition(reportDoc.DatabaseController.Database, text);
				if (fieldFromDefinition == null)
				{
					num++;
					break;
				}
				CrystalDecisions.ReportAppServer.ReportDefModel.Section section = reportDoc.ReportDefController.ReportDefinition.ReportHeaderArea.Sections[0];
				if (section.Height == 0)
				{
					section.Height = 300;
					reportDoc.ReportDefController.ReportSectionController.SetProperty(section, CrReportSectionPropertyEnum.crReportSectionPropertyHeight, 300);
					if (!section.Format.EnableSuppress)
					{
						section.Format.EnableSuppress = true;
						reportDoc.ReportDefController.ReportSectionController.SetProperty(section, CrReportSectionPropertyEnum.crReportSectionPropertyFormat, section.Format);
					}
				}
				CrystalDecisions.ReportAppServer.ReportDefModel.FieldObject fieldObject = (CrystalDecisions.ReportAppServer.ReportDefModel.FieldObject)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("F415BBC8-9B90-4C03-8C8C-9008928D62D8")));
				fieldObject.DataSourceName = fieldFromDefinition.FormulaForm;
				fieldObject.FieldValueType = CrFieldValueTypeEnum.crFieldValueTypeStringField;
				fieldObject.Left = 0;
				fieldObject.Top = 0;
				fieldObject.Height = 0;
				fieldObject.Width = 0;
				fieldObject.Format.EnableSuppress = true;
				reportDoc.ReportDefController.ReportObjectController.Add(fieldObject, section);
				list.Add(text);
			}
			if (list.Count != addressDefinition.AddressContactFields.Length)
			{
				addressDefinition.AddressContactFields = list.ToArray();
			}
		}
		return num;
	}

	private static List<ReportAddressDefinition> loadAllPossibleAddressDefinitions(ReportProxy report, IServiceProvider provider)
	{
		string text = null;
		string[] documentKeyFields = null;
		foreach (CrystalParameter parameter in report.Parameters)
		{
			if (parameter.Data != null && parameter.Data.DisplayType == ReportDisplayType.Filter && !string.IsNullOrWhiteSpace(parameter.Data.ContactsTable))
			{
				text = parameter.Data.Table;
				documentKeyFields = parameter.Data.KeyFieldsArray;
				break;
			}
		}
		if (string.IsNullOrWhiteSpace(text))
		{
			foreach (CrystalParameter parameter2 in report.Parameters)
			{
				if (parameter2.Data != null && parameter2.Data.DisplayType == ReportDisplayType.Filter && !string.IsNullOrWhiteSpace(parameter2.Data.Table))
				{
					text = parameter2.Data.Table;
					documentKeyFields = parameter2.Data.KeyFieldsArray;
					break;
				}
			}
		}
		M1DataDictionary m1DataDictionary = provider.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		if (string.IsNullOrWhiteSpace(report.EmailOptions.EmailContactField) && !string.IsNullOrWhiteSpace(text))
		{
			SqlCommand sqlCommand = m1DataDictionary.NewSqlCommand("Select dtContactField From DDTables Where dtTable = @Table");
			sqlCommand.Parameters.Add(new SqlParameter("@Table", SqlDbType.NVarChar)).Value = text;
			string text2 = Convert.ToString(m1DataDictionary.ExecuteScalar(sqlCommand));
			if (!string.IsNullOrWhiteSpace(text2))
			{
				report.EmailOptions.EmailContactField = text2;
			}
		}
		List<ReportAddressDefinition> list = new List<ReportAddressDefinition>();
		if (!string.IsNullOrWhiteSpace(text))
		{
			ReportAddressDefinition reportAddressDefinition = new ReportAddressDefinition();
			reportAddressDefinition.DocumentTable = text;
			reportAddressDefinition.DocumentKeyFields = documentKeyFields;
			reportAddressDefinition.Caption = "None";
			list.Add(reportAddressDefinition);
			ScriptingBase scriptingBase = null;
			try
			{
				SqlCommand sqlCommand2 = m1DataDictionary.NewSqlCommand("Select dfField,dfCaption,dfRelatedFields,dfRelatedTable,dfModule,dfVisibleExpression,dfVisibleExpressionUser From DDFields Where (dfTable = @Table And (dfRelatedTable = 'OrganizationContacts' Or dfRelatedTable = 'Employees')) Or (dfField = @Field)");
				sqlCommand2.Parameters.Add(new SqlParameter("@Table", SqlDbType.NVarChar)).Value = text;
				sqlCommand2.Parameters.Add(new SqlParameter("@Field", SqlDbType.NVarChar)).Value = report.EmailOptions.EmailContactField;
				using DataTable dataTable = m1DataDictionary.GetDataTable(sqlCommand2);
				foreach (DataRow row in dataTable.Rows)
				{
					bool flag = true;
					if (!row.IsNull("dfVisibleExpression"))
					{
						if (scriptingBase == null)
						{
							scriptingBase = new ScriptingBase(provider);
							scriptingBase.LoadEnvironment();
						}
						if (!Convert.ToBoolean(scriptingBase.Eval(row.Field<string>("dfVisibleExpression"))))
						{
							flag = false;
						}
					}
					if (flag)
					{
						reportAddressDefinition = new ReportAddressDefinition();
						reportAddressDefinition.DocumentTable = text;
						reportAddressDefinition.DocumentKeyFields = documentKeyFields;
						reportAddressDefinition.AddressContactFields = ((string.IsNullOrWhiteSpace(row.Field<string>("dfRelatedFields")) ? string.Empty : (row.Field<string>("dfRelatedFields") + ",")) + row.Field<string>("dfField")).Split(',');
						reportAddressDefinition.AddressTable = row.Field<string>("dfRelatedTable");
						reportAddressDefinition.Caption = row.Field<string>("dfCaption");
						list.Add(reportAddressDefinition);
					}
				}
			}
			finally
			{
				if (scriptingBase != null)
				{
					scriptingBase.Dispose();
					scriptingBase = null;
				}
			}
		}
		return list;
	}

	private static ReportAddressDefinition loadAddressDefinition(ReportProxy report, IServiceProvider provider)
	{
		ReportAddressDefinition reportAddressDefinition = new ReportAddressDefinition();
		reportAddressDefinition.AddressContactFields = new string[0];
		foreach (CrystalParameter parameter in report.Parameters)
		{
			if (parameter.Data != null && parameter.Data.DisplayType == ReportDisplayType.Filter && !string.IsNullOrWhiteSpace(parameter.Data.ContactsTable))
			{
				reportAddressDefinition.DocumentTable = parameter.Data.Table;
				reportAddressDefinition.DocumentKeyFields = parameter.Data.KeyFieldsArray;
				reportAddressDefinition.AddressTable = parameter.Data.ContactsTable;
				reportAddressDefinition.AddressContactFields = parameter.Data.ContactFieldsArray;
				break;
			}
		}
		if (string.IsNullOrWhiteSpace(reportAddressDefinition.DocumentTable))
		{
			foreach (CrystalParameter parameter2 in report.Parameters)
			{
				if (parameter2.Data != null && parameter2.Data.DisplayType == ReportDisplayType.Filter && !string.IsNullOrWhiteSpace(parameter2.Data.Table))
				{
					reportAddressDefinition.DocumentTable = parameter2.Data.Table;
					reportAddressDefinition.DocumentKeyFields = parameter2.Data.KeyFieldsArray;
					break;
				}
			}
		}
		if (!string.IsNullOrWhiteSpace(report.EmailOptions.EmailContactField) && (reportAddressDefinition.AddressContactFields.Length == 0 || !reportAddressDefinition.AddressContactFields[reportAddressDefinition.AddressContactFields.Length - 1].Equals(report.EmailOptions.EmailContactField, StringComparison.CurrentCultureIgnoreCase)))
		{
			M1DataDictionary obj = provider.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
			SqlCommand sqlCommand = obj.NewSqlCommand("Select dfField,dfRelatedFields,dfRelatedTable From DDFields Where dfField = @Field");
			sqlCommand.Parameters.Add(new SqlParameter("@Field", SqlDbType.NVarChar)).Value = report.EmailOptions.EmailContactField;
			using DataTable dataTable = obj.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				DataRow row = dataTable.Rows[0];
				reportAddressDefinition.AddressContactFields = ((string.IsNullOrWhiteSpace(row.Field<string>("dfRelatedFields")) ? string.Empty : (row.Field<string>("dfRelatedFields") + ",")) + row.Field<string>("dfField")).Split(',');
				reportAddressDefinition.AddressTable = row.Field<string>("dfRelatedTable");
			}
		}
		return reportAddressDefinition;
	}

	private static void LoadDataForParameters(IServiceProvider provider, ReportProxy reportContainer)
	{
		M1DataDictionary m1DataDictionary = provider.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		M1Database database = provider.GetService(typeof(M1Database)) as M1Database;
		List<string> list = new List<string>();
		foreach (CrystalParameter parameter in reportContainer.Parameters)
		{
			parameter.Data = new ReportParameterData();
			if (parameter.Name.StartsWith("Filter_", StringComparison.CurrentCultureIgnoreCase))
			{
				parameter.Data.DisplayType = ReportDisplayType.Filter;
				loadFieldData(m1DataDictionary, database, parameter.Data, parameter.Name.Substring(7));
				if (!string.IsNullOrWhiteSpace(parameter.Text))
				{
					parameter.Data.Fields[0].Caption = parameter.Text.Substring(0, (parameter.Text.IndexOf("|") > 0) ? parameter.Text.IndexOf("|") : parameter.Text.Length);
				}
				if (parameter.Data.Fields.Count == 0)
				{
					parameter.Data.DisplayType = ReportDisplayType.Label;
					parameter.Data.InstanceCount = 1;
					ReportPromptFieldInfo reportPromptFieldInfo = new ReportPromptFieldInfo();
					reportPromptFieldInfo.FieldType = "nvarchar";
					reportPromptFieldInfo.Caption = $"-----Invalid field name in parameter {parameter.Name}----- {parameter.Text}";
					parameter.Data.Fields.Add(reportPromptFieldInfo);
					continue;
				}
				if ((parameter.Data.Fields[0].FieldType.Equals("datetime", StringComparison.CurrentCultureIgnoreCase) || parameter.Data.Fields[0].FieldType.Equals("smalldatetime", StringComparison.CurrentCultureIgnoreCase)) && parameter.ValueType == 9)
				{
					parameter.Data.Fields[0].FieldType = "date";
				}
				parameter.Data.InstanceCount = ((!parameter.IsRange) ? 1 : 2);
				if (!string.IsNullOrWhiteSpace(parameter.Text))
				{
					string[] array = parameter.Text.Split(';');
					for (int i = 0; i < array.Length; i++)
					{
						if (i < parameter.Data.InstanceCount)
						{
							string[] array2 = array[i].Split('|');
							if (array2.Length > 3 && !string.IsNullOrWhiteSpace(array2[3]))
							{
								parameter.Data.FieldOptions[i].Filter = array2[3];
							}
							if (array2.Length > 2 && !array2[2].Equals("False", StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrWhiteSpace(array2[2]))
							{
								parameter.Data.Required = true;
							}
							if (array2.Length > 1 && !string.IsNullOrWhiteSpace(array2[1]))
							{
								parameter.Data.FieldOptions[i].DefaultValueExpressions = array2[1].Split('~');
							}
						}
					}
				}
				if (parameter.Data.InstanceCount == 2)
				{
					parameter.Data.FieldOptions[0].Operator = ">=";
					parameter.Data.FieldOptions[1].Operator = "<=";
				}
				else
				{
					parameter.Data.FieldOptions[0].Operator = "=";
				}
			}
			else if (parameter.Name.StartsWith("Prompt_", StringComparison.CurrentCultureIgnoreCase))
			{
				parameter.Data.DisplayType = ReportDisplayType.Prompt;
				string text = parameter.Name.Substring(7);
				ReportPromptFieldInfo reportPromptFieldInfo2 = new ReportPromptFieldInfo();
				reportPromptFieldInfo2.FieldName = string.Empty;
				reportPromptFieldInfo2.RelatedAndCurrentFieldArray = new string[0];
				parameter.Data.Fields.Add(reportPromptFieldInfo2);
				if (parameter.ValueType == 11)
				{
					reportPromptFieldInfo2.FieldType = "nvarchar";
					if (parameter.MaximumValue > 0 && parameter.MaximumValue < 100)
					{
						reportPromptFieldInfo2.FieldLength = parameter.MaximumValue;
					}
					else
					{
						reportPromptFieldInfo2.FieldLength = 30;
					}
				}
				else if (parameter.ValueType == 9)
				{
					reportPromptFieldInfo2.FieldType = "date";
					reportPromptFieldInfo2.FieldLength = 8;
				}
				else if (parameter.ValueType == 15)
				{
					reportPromptFieldInfo2.FieldType = "datetime";
					reportPromptFieldInfo2.FieldLength = 8;
				}
				else if (parameter.ValueType == 6 || parameter.ValueType == 7)
				{
					reportPromptFieldInfo2.FieldType = "numeric";
					reportPromptFieldInfo2.FieldLength = 0;
					string[] array3 = text.Split('_');
					if (array3.Length > 3)
					{
						reportPromptFieldInfo2.FieldDecimals = Convert.ToInt32(array3[3]);
					}
					if (array3.Length > 2)
					{
						reportPromptFieldInfo2.FieldLength = Convert.ToInt32(array3[2]);
					}
					if (reportPromptFieldInfo2.FieldLength <= 0)
					{
						if (parameter.MaximumValue < 50)
						{
							reportPromptFieldInfo2.FieldLength = parameter.MaximumValue;
						}
						else
						{
							reportPromptFieldInfo2.FieldLength = 6;
						}
					}
				}
				else if (parameter.ValueType == 8)
				{
					reportPromptFieldInfo2.FieldType = "bit";
					reportPromptFieldInfo2.FieldLength = 1;
				}
				parameter.Data.InstanceCount = Convert.ToInt32(text.Substring(0, 1));
				string[] array = parameter.Text.Split(';');
				for (int j = 0; j < array.Length; j++)
				{
					if (j >= parameter.Data.InstanceCount)
					{
						continue;
					}
					string[] array2 = array[j].Split('|');
					if (array2.Length == 0)
					{
						continue;
					}
					if (!string.IsNullOrWhiteSpace(array2[0]))
					{
						reportPromptFieldInfo2.Caption = m1DataDictionary.Language.GetLocalString(array2[0]);
					}
					if (array2.Length <= 1)
					{
						continue;
					}
					if (!string.IsNullOrWhiteSpace(array2[1]))
					{
						parameter.Data.FieldOptions[j].DefaultValueExpressions = new string[1] { array2[1] };
					}
					if (array2.Length > 2)
					{
						if (!string.IsNullOrWhiteSpace(array2[2]))
						{
							parameter.Data.FieldOptions[j].Filter = array2[2];
						}
						if (array2.Length > 3 && !array2[3].Equals("False", StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrWhiteSpace(array2[3]))
						{
							parameter.Data.Required = true;
						}
					}
				}
			}
			else if (parameter.Name.StartsWith("DropDownSelect_", StringComparison.CurrentCultureIgnoreCase))
			{
				parameter.Data.DisplayType = ReportDisplayType.DropDownselect;
				parameter.Name.Substring(15);
				parameter.Data.CanBeSaved = true;
				string[] array = parameter.Text.Split(';');
				parameter.Data.InstanceCount = 1;
				ReportPromptFieldInfo reportPromptFieldInfo3 = new ReportPromptFieldInfo();
				reportPromptFieldInfo3.Caption = m1DataDictionary.Language.GetLocalString(array[0]);
				parameter.Data.Fields.Add(reportPromptFieldInfo3);
				if (array.Length > 1)
				{
					parameter.Data.RowSource = array[1];
					if (array.Length > 2)
					{
						parameter.Data.FieldOptions[0].DefaultValueExpressions = new string[1] { array[2] };
						if (array.Length > 3)
						{
							parameter.Data.Required = array[2].Equals("True", StringComparison.CurrentCultureIgnoreCase);
						}
					}
				}
				if (parameter.ValueType == 6)
				{
					reportPromptFieldInfo3.FieldType = "int";
				}
				else
				{
					reportPromptFieldInfo3.FieldType = "nvarchar";
				}
			}
			else if (parameter.Name.StartsWith("DropDown_", StringComparison.CurrentCultureIgnoreCase))
			{
				parameter.Data.DisplayType = ReportDisplayType.DropDown;
				parameter.Data.CanBeSaved = true;
				parameter.Data.Required = true;
				string[] array = parameter.Text.Split(';');
				List<DropDownTextFilter> list2 = new List<DropDownTextFilter>();
				ReportPromptFieldInfo reportPromptFieldInfo4 = new ReportPromptFieldInfo();
				reportPromptFieldInfo4.FieldName = string.Empty;
				reportPromptFieldInfo4.RelatedAndCurrentFieldArray = new string[0];
				reportPromptFieldInfo4.FieldType = ((parameter.ValueType == 11) ? "nvarchar" : "numeric");
				parameter.Data.InstanceCount = 1;
				if (array.Length != 0)
				{
					string[] array2 = array[0].Split('|');
					reportPromptFieldInfo4.Caption = m1DataDictionary.Language.GetLocalString(array2[0]);
					if (array2.Length > 1)
					{
						parameter.Data.FieldOptions[0].DefaultValueExpressions = new string[1] { array2[1] };
					}
					for (int k = 1; k < array.Length; k++)
					{
						if (!string.IsNullOrWhiteSpace(array[k]))
						{
							array2 = array[k].Split('|');
							DropDownTextFilter dropDownTextFilter = new DropDownTextFilter(m1DataDictionary.Language.GetLocalString(array2[0]), string.Empty, null);
							if (array2.Length > 1)
							{
								dropDownTextFilter.Filter = array2[1];
							}
							list2.Add(dropDownTextFilter);
						}
					}
				}
				if (parameter.Data.FieldOptions[0].DefaultValueExpressions == null)
				{
					parameter.Data.FieldOptions[0].DefaultValueExpressions = new string[1] { "1" };
				}
				foreach (CrystalParameter parameter2 in reportContainer.Parameters)
				{
					if (!parameter2.Name.StartsWith("Text_", StringComparison.CurrentCultureIgnoreCase))
					{
						continue;
					}
					string text2 = parameter2.Name.Substring(5);
					int num = text2.IndexOf('_');
					if (num != -1)
					{
						text2 = text2.Substring(num + 1);
					}
					if (!parameter.Name.Equals(text2, StringComparison.CurrentCultureIgnoreCase))
					{
						continue;
					}
					string[] array4 = parameter2.Text.Split(';');
					foreach (string text3 in array4)
					{
						if (!string.IsNullOrWhiteSpace(text3))
						{
							list2.Add(new DropDownTextFilter(m1DataDictionary.Language.GetLocalString(text3), string.Empty, null));
						}
					}
				}
				reportPromptFieldInfo4.ValueList = list2.ToArray();
				parameter.Data.Fields.Add(reportPromptFieldInfo4);
			}
			else if (parameter.Name.StartsWith("Label_", StringComparison.CurrentCultureIgnoreCase))
			{
				parameter.Data.DisplayType = ReportDisplayType.Label;
				parameter.Data.InstanceCount = 1;
				ReportPromptFieldInfo reportPromptFieldInfo5 = new ReportPromptFieldInfo();
				reportPromptFieldInfo5.FieldType = "nvarchar";
				reportPromptFieldInfo5.Caption = m1DataDictionary.Language.GetLocalString(parameter.Text);
				parameter.Data.Fields.Add(reportPromptFieldInfo5);
			}
			else if (parameter.Name.Equals("PrintRelatedDocuments", StringComparison.CurrentCultureIgnoreCase) || parameter.Name.Equals("PrintAttachments", StringComparison.CurrentCultureIgnoreCase))
			{
				parameter.Data.DisplayType = ReportDisplayType.CheckBoxGroup;
				parameter.Data.InstanceCount = 1;
				parameter.Data.CanBeSaved = true;
				ReportPromptFieldInfo reportPromptFieldInfo6 = new ReportPromptFieldInfo();
				reportPromptFieldInfo6.FieldType = "bit";
				reportPromptFieldInfo6.FieldLength = 1;
				reportPromptFieldInfo6.FieldModule = (parameter.Name.Equals("PrintAttachments", StringComparison.CurrentCultureIgnoreCase) ? "AT" : "DM");
				parameter.Data.Fields.Add(reportPromptFieldInfo6);
				string[] array = parameter.Text.Split('|');
				if (array.Length != 0)
				{
					reportPromptFieldInfo6.Caption = m1DataDictionary.Language.GetLocalString(array[0]);
					if (array.Length > 2)
					{
						parameter.Data.FieldOptions[0].DefaultValueExpressions = new string[1] { array[2] };
					}
				}
			}
			else if (parameter.Name.StartsWith("CheckboxGroup_", StringComparison.CurrentCultureIgnoreCase) || parameter.Name.StartsWith("EmailReviewBeforeSending", StringComparison.CurrentCultureIgnoreCase))
			{
				parameter.Data.DisplayType = ReportDisplayType.CheckBoxGroup;
				parameter.Data.InstanceCount = 1;
				string[] array = parameter.Name.Split('_');
				if (array.Length > 1)
				{
					parameter.Data.DisplayGroup = array[0] + "_" + array[1];
				}
				ReportPromptFieldInfo reportPromptFieldInfo7 = new ReportPromptFieldInfo();
				reportPromptFieldInfo7.FieldType = "bit";
				reportPromptFieldInfo7.FieldLength = 1;
				parameter.Data.Fields.Add(reportPromptFieldInfo7);
				parameter.Data.CanBeSaved = true;
				array = parameter.Text.Split(';');
				for (int m = 0; m < array.Length; m++)
				{
					if (m >= parameter.Data.InstanceCount)
					{
						continue;
					}
					string[] array2 = array[m].Split('|');
					if (array2.Length == 0)
					{
						continue;
					}
					reportPromptFieldInfo7.Caption = m1DataDictionary.Language.GetLocalString(array2[0]);
					if (array2.Length > 1)
					{
						if (!string.IsNullOrWhiteSpace(array2[1]))
						{
							parameter.Data.FieldOptions[m].DefaultValueExpressions = new string[1] { array2[1] };
						}
						if (array2.Length > 2)
						{
							reportPromptFieldInfo7.FieldModule = array2[2];
						}
					}
				}
			}
			else if (parameter.Name.StartsWith("YearPeriod_", StringComparison.CurrentCultureIgnoreCase))
			{
				parameter.Data.DisplayType = ReportDisplayType.YearPeriod;
				string[] array5 = parameter.Name.Split('_');
				if (array5.Length > 2)
				{
					loadFieldData(m1DataDictionary, database, parameter.Data, array5[2]);
				}
				else
				{
					loadFieldData(m1DataDictionary, database, parameter.Data, array5[1]);
				}
				if (parameter.IsRange)
				{
					parameter.Data.InstanceCount = 2;
					parameter.Data.FieldOptions[0].Operator = ">=";
					parameter.Data.FieldOptions[1].Operator = "<=";
				}
				else
				{
					parameter.Data.InstanceCount = 1;
					if (parameter.Name.EndsWith("_LessThanOrEqualTo", StringComparison.CurrentCultureIgnoreCase))
					{
						parameter.Data.FieldOptions[0].Operator = "<=";
					}
					else
					{
						parameter.Data.FieldOptions[0].Operator = "=";
					}
				}
				if (string.IsNullOrWhiteSpace(parameter.Text))
				{
					continue;
				}
				List<string> list3 = new List<string>();
				string[] array = parameter.Text.Split(';');
				if (array.Length != 0)
				{
					list3.Add(array[0]);
				}
				if (array.Length > 1)
				{
					list3.Add(array[1]);
				}
				parameter.Data.FieldOptions[0].DefaultValueExpressions = list3.ToArray();
				if (parameter.Data.FieldOptions.Length > 1)
				{
					list3.Clear();
					if (array.Length > 2)
					{
						list3.Add(array[2]);
					}
					if (array.Length > 3)
					{
						list3.Add(array[3]);
					}
					parameter.Data.FieldOptions[1].DefaultValueExpressions = list3.ToArray();
				}
			}
			else if (parameter.Name.Equals("SortBy", StringComparison.CurrentCultureIgnoreCase))
			{
				parameter.Data.DisplayType = ReportDisplayType.DropDown;
				ReportPromptFieldInfo reportPromptFieldInfo8 = new ReportPromptFieldInfo();
				reportPromptFieldInfo8.Caption = "Sort By";
				reportPromptFieldInfo8.FieldType = "numeric";
				reportPromptFieldInfo8.FieldLength = 2;
				string[] array = parameter.Text.Split(';');
				parameter.Data.Fields.Add(reportPromptFieldInfo8);
				parameter.Data.CanBeSaved = true;
				new List<string>();
				List<DropDownTextFilter> list4 = new List<DropDownTextFilter>();
				string[] array4 = array;
				foreach (string text4 in array4)
				{
					if (!string.IsNullOrWhiteSpace(text4))
					{
						list4.Add(new DropDownTextFilter(m1DataDictionary.Language.GetLocalString(text4), string.Empty, null));
					}
				}
				parameter.Data.InstanceCount = 1;
				parameter.Data.FieldOptions[0].DefaultValueExpressions = new string[1] { "1" };
				reportPromptFieldInfo8.ValueList = list4.ToArray();
			}
			else if (parameter.Name.Equals("DatasetSelect", StringComparison.CurrentCultureIgnoreCase))
			{
				parameter.Data.DisplayType = ReportDisplayType.DatasetSelect;
				ReportPromptFieldInfo reportPromptFieldInfo9 = new ReportPromptFieldInfo();
				reportPromptFieldInfo9.FieldType = "nvarchar";
				reportPromptFieldInfo9.FieldLength = 10;
				parameter.Data.Fields.Add(reportPromptFieldInfo9);
				parameter.Data.CanBeSaved = true;
				parameter.Data.InstanceCount = 1;
				MessageBox.Show("Unhandled DatasetSelect parameter on report " + reportContainer.ReportName, "Confirm", MessageBoxButtons.OK);
			}
			else if (parameter.Name.Equals("HelpLink", StringComparison.CurrentCultureIgnoreCase))
			{
				reportContainer.HelpLink = parameter.Text;
			}
			else if (parameter.Name.Equals("OnPrintCommand", StringComparison.CurrentCultureIgnoreCase))
			{
				reportContainer.OnPrintCommand = parameter.Text;
			}
			else if (parameter.Name.Equals("OnRunCommand", StringComparison.CurrentCultureIgnoreCase))
			{
				reportContainer.OnRunCommand = parameter.Text;
			}
			else if (parameter.Name.Equals("EmailWebLink", StringComparison.CurrentCultureIgnoreCase))
			{
				reportContainer.EmailOptions.EmailWebLink = parameter.Text;
			}
			else if (parameter.Name.Equals("EmailSubject", StringComparison.CurrentCultureIgnoreCase))
			{
				reportContainer.EmailOptions.EmailSubject = parameter.Text;
			}
			else if (parameter.Name.Equals("EmailAttachmentName", StringComparison.CurrentCultureIgnoreCase))
			{
				reportContainer.EmailOptions.EmailAttachmentName = parameter.Text;
			}
			else if (parameter.Name.Equals("EmailBody", StringComparison.CurrentCultureIgnoreCase))
			{
				reportContainer.EmailOptions.EmailBody = parameter.Text;
			}
			else if (parameter.Name.Equals("EmailContactField", StringComparison.CurrentCultureIgnoreCase))
			{
				reportContainer.EmailOptions.EmailContactField = parameter.Text;
			}
			else if (parameter.Name.StartsWith("SecurityModule_", StringComparison.CurrentCultureIgnoreCase))
			{
				list.Add(parameter.Name.Substring(15));
			}
		}
		reportContainer.SecurityModules = list.ToArray();
	}

	private static void LoadDataForSpecialParameters(IServiceProvider provider, ReportProxy reportContainer)
	{
		provider.GetService(typeof(M1DataDictionary));
		provider.GetService(typeof(M1Database));
		if (!reportContainer.Parameters.Contains("EmailContactField"))
		{
			reportContainer.Parameters.Add(new CrystalParameter("EmailContactField", "Contact Group", 1, enableMultiple: false, isRange: false, 3, inUse: false));
		}
		if (!reportContainer.Parameters.Contains("GroupContactType"))
		{
			reportContainer.Parameters.Add(new CrystalParameter("GroupContactType", "Group Type", 1, enableMultiple: false, isRange: false, 3, inUse: false));
		}
		foreach (CrystalParameter parameter in reportContainer.Parameters)
		{
			if (parameter.Name.Equals("EmailContactField", StringComparison.CurrentCultureIgnoreCase))
			{
				parameter.Data = new ReportParameterData();
				parameter.Data.DisplayType = ReportDisplayType.DropDown;
				ReportPromptFieldInfo reportPromptFieldInfo = new ReportPromptFieldInfo();
				reportPromptFieldInfo.Caption = "Contact Group";
				reportPromptFieldInfo.FieldType = "nvarchar";
				reportPromptFieldInfo.FieldLength = 1;
				parameter.Data.Fields.Add(reportPromptFieldInfo);
				parameter.Data.CanBeSaved = true;
				new List<string>();
				List<DropDownTextFilter> list = new List<DropDownTextFilter>();
				foreach (ReportAddressDefinition address in reportContainer.Addresses)
				{
					list.Add(new DropDownTextFilter(address.Caption, string.Empty, address.LastContactField));
				}
				parameter.Data.InstanceCount = 1;
				if (!string.IsNullOrWhiteSpace(reportContainer.EmailOptions.EmailContactField))
				{
					parameter.Data.FieldOptions[0].DefaultValueExpressions = new string[1] { M1Util.ConvertToScript(reportContainer.EmailOptions.EmailContactField) };
				}
				else if (reportContainer.Addresses.Count != 0)
				{
					parameter.Data.FieldOptions[0].DefaultValueExpressions = new string[1] { M1Util.ConvertToScript(reportContainer.Addresses[0].LastContactField) };
				}
				reportPromptFieldInfo.ValueList = list.ToArray();
				parameter.Data.DataChanged -= reportContainer.EmailContactFieldChanged;
				parameter.Data.DataChanged += reportContainer.EmailContactFieldChanged;
			}
			else if (parameter.Name.Equals("GroupContactType", StringComparison.CurrentCultureIgnoreCase))
			{
				parameter.Data = new ReportParameterData();
				parameter.Data.DisplayType = ReportDisplayType.DropDown;
				ReportPromptFieldInfo reportPromptFieldInfo2 = new ReportPromptFieldInfo();
				reportPromptFieldInfo2.Caption = "Group Type";
				reportPromptFieldInfo2.FieldType = "nvarchar";
				reportPromptFieldInfo2.FieldLength = 1;
				parameter.Data.Fields.Add(reportPromptFieldInfo2);
				parameter.Data.CanBeSaved = true;
				new List<string>();
				List<DropDownTextFilter> list2 = new List<DropDownTextFilter>();
				list2.Add(new DropDownTextFilter("Multiple records per contact", string.Empty, "multiple"));
				list2.Add(new DropDownTextFilter("One record/one attachment per contact", string.Empty, "oneandone"));
				list2.Add(new DropDownTextFilter("One record/multiple attachments per contact", string.Empty, "oneandmultiple"));
				parameter.Data.InstanceCount = 1;
				string text = (reportContainer.EmailOptions.MultipleRecordsPerContact ? M1Util.ConvertToScript("multiple") : ((!reportContainer.EmailOptions.MultipleAttachmentsPerEmail) ? M1Util.ConvertToScript("oneandone") : M1Util.ConvertToScript("oneandmultiple")));
				parameter.Data.FieldOptions[0].DefaultValueExpressions = new string[1] { text };
				parameter.Data.DataChanged -= reportContainer.GroupTypeChanged;
				parameter.Data.DataChanged += reportContainer.GroupTypeChanged;
				reportPromptFieldInfo2.ValueList = list2.ToArray();
			}
		}
	}

	private static void loadFieldData(M1DataDictionary dd, M1Database database, ReportParameterData parm, string fieldName)
	{
		SqlCommand sqlCommand = dd.NewSqlCommand("select a.dfTable,dtKeyFields,a.dfField," + dd.Language.GetdfCaptionField(database, "a") + ",a.dfRelatedTable,a.dfFFil,a.dfRelatedTableReturnField,c.dfRelatedFields As RelatedTableOtherFields,a.dfRelatedFields,a.dfRelatedTableSearchGridId,a.dfdbtype,a.dfLength,a.dfDecimals,a.dfModule,dtContactField,b.dfRelatedFields As ContactRelatedFields,b.dfRelatedTable As ContactsTable from DDFields a " + dd.Language.GetdfCaptionJoin(database, "a") + " Inner Join DDTables On a.dfTable = dtTable Left Outer Join DDFields b on b.dfField = dtContactField Left Outer Join DDFields c On a.dfRelatedTableReturnField = c.dfField where a.dfField = @FieldName");
		sqlCommand.Parameters.Add(new SqlParameter("@FieldName", SqlDbType.NVarChar)).Value = fieldName;
		DataTable dataTable = dd.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		DataRow dataRow = dataTable.Rows[0];
		parm.Table = dataRow.Field<string>("dfTable");
		parm.KeyFieldsArray = dataRow.Field<string>("dtKeyFields").Split(',');
		ReportPromptFieldInfo fieldInfo = new ReportPromptFieldInfo();
		fieldInfo.FieldName = fieldName;
		fieldInfo.RelatedTable = dataRow.Field<string>("dfRelatedTable");
		fieldInfo.RelatedTableSearchGridId = dataRow.Field<string>("dfRelatedTableSearchGridID");
		if (string.IsNullOrWhiteSpace(dataRow.Field<string>("RelatedTableOtherFields")))
		{
			fieldInfo.RelatedTableReturnFields = dataRow.Field<string>("dfRelatedTableReturnField").Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
		}
		else
		{
			fieldInfo.RelatedTableReturnFields = (dataRow.Field<string>("RelatedTableOtherFields") + "," + dataRow.Field<string>("dfRelatedTableReturnField")).Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
		}
		fieldInfo.RelatedTableForeignFilter = dataRow.Field<string>("dfFFil");
		fieldInfo.Caption = dd.Language.GetLocalString(dataRow.Field<string>("dfCaption"));
		fieldInfo.FieldType = dataRow.Field<string>("dfdbtype");
		fieldInfo.FieldLength = dataRow.Field<byte>("dfLength");
		fieldInfo.FieldModule = dataRow.Field<string>("dfModule");
		fieldInfo.FieldDecimals = dataRow.Field<byte>("dfDecimals");
		string text = dataRow.Field<string>("dfRelatedFields");
		if (!string.IsNullOrWhiteSpace(text))
		{
			string[] array = text.Split(',');
			foreach (string fieldName2 in array)
			{
				loadFieldData(dd, database, parm, fieldName2);
			}
			text = text + "," + fieldName;
		}
		else
		{
			text = fieldName;
		}
		fieldInfo.RelatedAndCurrentFieldArray = text.Split(',');
		if (dataRow["ContactRelatedFields"] != DBNull.Value)
		{
			if (!string.IsNullOrWhiteSpace(dataRow.Field<string>("ContactRelatedFields")))
			{
				parm.ContactFieldsArray = (dataRow.Field<string>("ContactRelatedFields") + "," + dataRow.Field<string>("dtContactField")).Split(',');
			}
			else
			{
				parm.ContactFieldsArray = new string[1] { dataRow.Field<string>("dtContactField") };
			}
			parm.ContactsTable = dataRow.Field<string>("ContactsTable");
		}
		if (parm.Fields.FindIndex((ReportPromptFieldInfo item) => item.FieldName.Equals(fieldInfo.FieldName, StringComparison.CurrentCultureIgnoreCase)) == -1)
		{
			parm.Fields.Add(fieldInfo);
		}
	}

	private static void LoadSavedSettings(M1DataDictionary dd, M1Database database, ReportProxy reportContainer)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
		M1User m1User = database.GetService(typeof(M1User)) as M1User;
		SqlCommand sqlCommand = new SqlCommand("SELECT drSettings FROM DDSecurityReports WHERE drUserID = @UserID AND drDataset = @DatabaseID AND drReport = @ReportID");
		sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = m1User.ID;
		sqlCommand.Parameters.Add(new SqlParameter("@DatabaseID", SqlDbType.NVarChar)).Value = database.ID;
		sqlCommand.Parameters.Add(new SqlParameter("@ReportID", SqlDbType.NVarChar)).Value = reportContainer.ReportBaseName;
		string text = Convert.ToString(dd.ExecuteScalar(sqlCommand));
		if (!string.IsNullOrWhiteSpace(text))
		{
			string[] array = text.Replace("\n", string.Empty).Split('\r');
			foreach (string text2 in array)
			{
				int num = text2.IndexOf('=');
				if (num != -1)
				{
					dictionary.Add(text2.Substring(0, num), text2.Substring(num + 1));
				}
			}
		}
		reportContainer.Settings = dictionary;
	}

	private static void FlushSettingsToDb(IServiceProvider provider, ReportProxy reportContainer)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<string, string> setting in reportContainer.Settings)
		{
			if (!string.IsNullOrWhiteSpace(setting.Value))
			{
				stringBuilder.Append(setting.Key + "=" + setting.Value + "\r\n");
			}
		}
		M1DataDictionary m1DataDictionary = provider.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		M1User m1User = provider.GetService(typeof(M1User)) as M1User;
		M1Database m1Database = provider.GetService(typeof(M1Database)) as M1Database;
		using SqlCommand sqlCommand = new SqlCommand("SELECT * FROM DDSecurityReports WHERE drUserID = @UserID AND drDataset = @DatabaseID AND drReport = @ReportID");
		sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = m1User.ID;
		sqlCommand.Parameters.Add(new SqlParameter("@DatabaseID", SqlDbType.NVarChar)).Value = m1Database.ID;
		sqlCommand.Parameters.Add(new SqlParameter("@ReportID", SqlDbType.NVarChar)).Value = reportContainer.ReportBaseName;
		SqlDataAdapter adapter = null;
		try
		{
			DataTable dataTable = m1DataDictionary.GetDataTable(sqlCommand, fillSchema: true, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["drUserID"] = m1User.ID;
				dataRow["drDataset"] = m1Database.ID;
				dataRow["drFolder"] = reportContainer.ReportFolder;
				dataRow["drReport"] = reportContainer.ReportBaseName;
				dataRow["drLevel"] = 0;
				dataTable.Rows.Add(dataRow);
			}
			else
			{
				dataRow = dataTable.Rows[0];
			}
			bool flag = false;
			if (stringBuilder.Length == 0)
			{
				if (!dataRow.IsNull("drSettings"))
				{
					dataRow.SetField<string>("drSettings", null);
					flag = true;
				}
			}
			else if (dataRow.IsNull("drSettings") || !dataRow.Field<string>("drSettings").Equals(stringBuilder.ToString()))
			{
				dataRow.SetField("drSettings", stringBuilder.ToString());
				flag = true;
			}
			if (flag)
			{
				m1DataDictionary.UpdateData(dataTable, adapter);
			}
		}
		finally
		{
			m1DataDictionary = null;
			if (adapter != null)
			{
				adapter.Dispose();
				adapter = null;
			}
		}
	}

	private static object GetSavedValue(ReportProxy reportContainer, string name, object defaultValue, string type)
	{
		if (reportContainer.Settings != null && reportContainer.Settings.ContainsKey(name))
		{
			string text = reportContainer.Settings[name];
			if (type.Equals("bit", StringComparison.CurrentCultureIgnoreCase))
			{
				return text.Equals("True", StringComparison.CurrentCultureIgnoreCase);
			}
			if (type.Equals("numeric", StringComparison.CurrentCultureIgnoreCase))
			{
				return Convert.ToDecimal(text);
			}
			if (type.Equals("int", StringComparison.CurrentCultureIgnoreCase))
			{
				return Convert.ToInt32(text);
			}
			if (type.Equals("short", StringComparison.CurrentCultureIgnoreCase))
			{
				return Convert.ToInt16(text);
			}
			if (type.Equals("byte", StringComparison.CurrentCultureIgnoreCase))
			{
				return Convert.ToByte(text);
			}
			if (type.Equals("char", StringComparison.CurrentCultureIgnoreCase) || type.Equals("nchar", StringComparison.CurrentCultureIgnoreCase) || type.Equals("varchar", StringComparison.CurrentCultureIgnoreCase) || type.Equals("nvarchar", StringComparison.CurrentCultureIgnoreCase))
			{
				if (text.StartsWith("'") || text.StartsWith("\""))
				{
					text = text.Substring(1);
					text = text.Substring(0, text.Length - 1);
				}
				return text;
			}
			throw new M1Exception($"Unknown type {type} in GetSavedValue.");
		}
		return defaultValue;
	}

	private static void SetSavedValue(ReportProxy reportContainer, string name, string value)
	{
		if (reportContainer.Settings.ContainsKey(name))
		{
			reportContainer.Settings[name] = value;
		}
		else
		{
			reportContainer.Settings.Add(name, value);
		}
	}

	public static bool IsReportFilteredOnTable(ReportProxy report, string tableName)
	{
		foreach (CrystalParameter parameter in report.Parameters)
		{
			if (parameter.Data == null || parameter.Data.Fields == null)
			{
				continue;
			}
			foreach (ReportPromptFieldInfo field in parameter.Data.Fields)
			{
				if (field.RelatedTable.Equals(tableName, StringComparison.CurrentCultureIgnoreCase))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static void SetParametersByKeys(IServiceProvider provider, ReportProxy report, string tableName, List<object[]> keys)
	{
		M1Database m1Database = provider.GetService(typeof(M1Database)) as M1Database;
		M1DataDictionary m1DataDictionary = provider.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		SqlCommand sqlCommand = m1DataDictionary.NewSqlCommand("Select dtKeyFields From DDTables Where dtTable = @Table");
		sqlCommand.Parameters.Add(new SqlParameter("@Table", SqlDbType.NVarChar)).Value = tableName;
		string text = Convert.ToString(m1DataDictionary.ExecuteScalar(sqlCommand));
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		string[] array = text.Split(',');
		foreach (object[] key in keys)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(" Or ");
			}
			stringBuilder.Append("(");
			for (int i = 0; i < array.Length; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(" And ");
				}
				stringBuilder.Append(array[i] + "=" + M1Util.ConvertToSql(key[i]));
			}
			stringBuilder.Append(")");
		}
		DataTable dataTable = m1Database.GetDataTable("Select * From " + tableName + " Where " + stringBuilder.ToString());
		SetParametersFromDataTable(m1DataDictionary, report, dataTable, tableName);
	}

	public static void SetParametersFromDataTable(M1DataDictionary dd, ReportProxy report, DataTable data, string tableName)
	{
		SetParametersFromDataTable(dd, report, data, tableName, null);
	}

	public static void SetParametersFromDataTable(M1DataDictionary dd, ReportProxy report, DataTable data, string tableName, Dictionary<string, string> fieldRelatedTables)
	{
		SqlCommand sqlCommand = dd.NewSqlCommand("Select dtKeyFields From DDTables Where dtTable = @TableName");
		sqlCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)).Value = tableName;
		string text = Convert.ToString(dd.ExecuteScalar(sqlCommand));
		string text2 = string.Empty;
		if (!string.IsNullOrWhiteSpace(text))
		{
			text2 = text.Split(',')[0];
		}
		CrystalParameter crystalParameter = null;
		string value = string.Empty;
		if (!string.IsNullOrWhiteSpace(text2) && data.Columns.Contains(text2))
		{
			crystalParameter = getParameterForField(report.Parameters, text2);
			if (crystalParameter != null)
			{
				value = crystalParameter.Name;
				for (int i = 0; i < crystalParameter.Data.InstanceCount; i++)
				{
					crystalParameter.Data.FieldOptions[i].Values.Clear();
				}
				foreach (DataRow row4 in data.Rows)
				{
					setParameterFromDataRow(crystalParameter, row4, null);
				}
			}
			SqlCommand sqlCommand2 = dd.NewSqlCommand("Select dtKeysAtThisLevel From DDTables Where dtTable = @TableName");
			sqlCommand2.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)).Value = tableName;
			if (Convert.ToInt16(dd.ExecuteScalar(sqlCommand2)) > 1)
			{
				string empty = string.Empty;
				string[] array = null;
				array = text.Split(',');
				if (array.Length > 1)
				{
					empty = array[1];
					if (!string.IsNullOrWhiteSpace(empty) && data.Columns.Contains(empty))
					{
						crystalParameter = getParameterForField(report.Parameters, empty);
						if (crystalParameter != null)
						{
							value = crystalParameter.Name;
							for (int j = 0; j < crystalParameter.Data.InstanceCount; j++)
							{
								crystalParameter.Data.FieldOptions[j].Values.Clear();
							}
							foreach (DataRow row5 in data.Rows)
							{
								setParameterFromDataRow(crystalParameter, row5, null);
							}
						}
					}
				}
			}
		}
		if (crystalParameter != null && !report.Parameters.Contains("FilterOnAllParameters"))
		{
			return;
		}
		for (int k = 0; k < report.Parameters.Count; k++)
		{
			crystalParameter = report.Parameters[k];
			if (crystalParameter.Data == null || crystalParameter.Data.Fields == null || crystalParameter.Data.Fields.Count == 0 || crystalParameter.Name.Equals(value, StringComparison.CurrentCultureIgnoreCase) || !areFieldsInDataTable(crystalParameter, data, fieldRelatedTables))
			{
				continue;
			}
			for (int l = 0; l < crystalParameter.Data.InstanceCount; l++)
			{
				crystalParameter.Data.FieldOptions[l].Values.Clear();
			}
			if (crystalParameter.EnableMultipleValues)
			{
				foreach (DataRow row6 in data.Rows)
				{
					setParameterFromDataRow(crystalParameter, row6, fieldRelatedTables);
				}
			}
			else
			{
				setParameterFromDataRow(crystalParameter, data.Rows[0], fieldRelatedTables);
			}
		}
	}

	private static string getOtherNameForParameterField(ReportPromptFieldInfo fieldInfo, DataTable data, Dictionary<string, string> fieldRelatedTables)
	{
		if (data.Columns.Contains(fieldInfo.FieldName))
		{
			return fieldInfo.FieldName;
		}
		if (fieldRelatedTables != null)
		{
			_ = string.Empty;
			foreach (KeyValuePair<string, string> fieldRelatedTable in fieldRelatedTables)
			{
				if (fieldRelatedTable.Value.Equals(fieldInfo.RelatedTable, StringComparison.CurrentCultureIgnoreCase))
				{
					return fieldRelatedTable.Key;
				}
			}
		}
		if (fieldInfo.RelatedTableReturnFields != null && fieldInfo.RelatedTableReturnFields.Length != 0 && data.Columns.Contains(fieldInfo.RelatedTableReturnFields[0]))
		{
			return fieldInfo.RelatedTableReturnFields[0];
		}
		return string.Empty;
	}

	private static bool areFieldsInDataTable(CrystalParameter parameter, DataTable data, Dictionary<string, string> fieldRelatedTables)
	{
		for (int i = 0; i < parameter.Data.Fields.Count; i++)
		{
			if (!data.Columns.Contains(getOtherNameForParameterField(parameter.Data.Fields[i], data, fieldRelatedTables)))
			{
				return false;
			}
		}
		return true;
	}

	private static void setParameterFromDataRow(CrystalParameter parameter, DataRow row, Dictionary<string, string> fieldRelatedTables)
	{
		object[] array = new object[parameter.Data.Fields.Count];
		for (int i = 0; i < parameter.Data.Fields.Count; i++)
		{
			string otherNameForParameterField = getOtherNameForParameterField(parameter.Data.Fields[i], row.Table, fieldRelatedTables);
			if (string.IsNullOrWhiteSpace(otherNameForParameterField))
			{
				array[i] = string.Empty;
			}
			else
			{
				array[i] = row[otherNameForParameterField];
			}
		}
		for (int j = 0; j < parameter.Data.InstanceCount; j++)
		{
			parameter.Data.FieldOptions[j].Values.Add(array);
		}
	}

	private static CrystalParameter getParameterForField(CrystalParameterCollection parameters, string fieldName)
	{
		for (int i = 0; i < parameters.Count; i++)
		{
			CrystalParameter crystalParameter = parameters[i];
			if (crystalParameter.Data == null || crystalParameter.Data.Fields == null || crystalParameter.Data.Fields.Count == 0)
			{
				continue;
			}
			foreach (ReportPromptFieldInfo field in crystalParameter.Data.Fields)
			{
				if (field.FieldName.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase) || (field.RelatedTableReturnFields != null && field.RelatedTableReturnFields.Length != 0 && field.RelatedTableReturnFields[0].Equals(fieldName, StringComparison.CurrentCultureIgnoreCase)))
				{
					return crystalParameter;
				}
			}
		}
		return null;
	}

	public static List<string> GetInvalidParameters(ReportProxy report)
	{
		List<string> list = new List<string>();
		foreach (CrystalParameter parameter in report.Parameters)
		{
			if (parameter.Data != null && !parameter.Data.CheckIsValid(null))
			{
				list.Add(parameter.Data.Fields[0].Caption);
			}
		}
		return list;
	}

	public static ErrorItemsList CheckIfParametersAreValid(ReportProxy report)
	{
		ErrorItemsList errorItemsList = new ErrorItemsList();
		ValidationInfo validationInfo = new ValidationInfo
		{
			RowDescription = $"Report {report.ReportTitle} has some missing parameters"
		};
		foreach (CrystalParameter parameter in report.Parameters)
		{
			if (parameter.Data != null)
			{
				parameter.Data.CheckIsValid(validationInfo.Errors);
			}
		}
		if (validationInfo.Errors.Count != 0)
		{
			errorItemsList.Add(validationInfo);
		}
		return errorItemsList;
	}

	public static void CheckOnPrintCommand(IServiceProvider provider, ReportProxy report)
	{
		if (!string.IsNullOrWhiteSpace(report.OnPrintCommand))
		{
			processCommand(provider, report.FilterInfo.SqlWhere, report.OnPrintCommand);
		}
	}

	private static string getReportAddressSql(ReportProxy report, ReportAddress address)
	{
		if (address == null || string.IsNullOrWhiteSpace(address.SqlFilter))
		{
			return report.FilterInfo.SqlWhere;
		}
		if (string.IsNullOrWhiteSpace(report.FilterInfo.SqlWhere))
		{
			return address.SqlFilter;
		}
		return "(" + report.FilterInfo.SqlWhere + ") And (" + address.SqlFilter + ")";
	}

	private static void processCommand(IServiceProvider provider, string whereClause, string command)
	{
		M1Database m1Database = provider.GetService(typeof(M1Database)) as M1Database;
		if (command.StartsWith("Update ", StringComparison.CurrentCultureIgnoreCase))
		{
			command = command.Replace("{?WHERECLAUSE}", whereClause, caseInsensitive: true);
			m1Database.ExecuteCommand(command);
			string text = command.Substring(7).Trim();
			text = text.Substring(0, text.IndexOfAny(new char[2] { ' ', ',' })).Trim();
			if (!string.IsNullOrWhiteSpace(text))
			{
				m1Database.OnTableChanged(new TableChangedEventArgs(text, null, null, null));
			}
		}
		else
		{
			if (!command.StartsWith("App.", StringComparison.CurrentCultureIgnoreCase) && !command.StartsWith("Forms.", StringComparison.CurrentCultureIgnoreCase))
			{
				return;
			}
			whereClause = M1Util.ConvertToScript(whereClause);
			command = command.Replace("{?WHERECLAUSE}", whereClause, caseInsensitive: true);
			using ScriptingBase scriptingBase = new ScriptingBase(m1Database);
			scriptingBase.LoadEnvironment();
			if (command.StartsWith("Forms.", StringComparison.CurrentCultureIgnoreCase))
			{
				scriptingBase.AddObject("Forms", m1Database.GetService(typeof(IForms)));
			}
			scriptingBase.ExecuteStatement("Call " + command);
		}
	}

	public static bool CheckAndUpdateParameters(string fileName, string[] parmChangeSource, string[] parmChangeDest)
	{
		bool flag = false;
		ReportDocument reportDocument = new ReportDocument();
		reportDocument.Load(fileName, OpenReportMethod.OpenReportByDefault);
		for (int i = 0; i < reportDocument.ReportClientDocument.DataDefController.DataDefinition.ParameterFields.Count; i++)
		{
			CrystalDecisions.ReportAppServer.DataDefModel.ParameterField parameterField = (CrystalDecisions.ReportAppServer.DataDefModel.ParameterField)reportDocument.ReportClientDocument.DataDefController.DataDefinition.ParameterFields[i];
			if (!string.IsNullOrWhiteSpace(parameterField.ReportName) || string.IsNullOrWhiteSpace(parameterField.Description))
			{
				continue;
			}
			for (int j = 0; j < parmChangeSource.Length; j++)
			{
				if (parameterField.Description.IndexOf(parmChangeSource[j], 0, StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					parameterField.Description = parameterField.Description.Replace(parmChangeSource[j], parmChangeDest[j], caseInsensitive: true);
					flag = true;
				}
			}
		}
		if (flag)
		{
			reportDocument.SaveAs(fileName);
		}
		reportDocument.Dispose();
		return flag;
	}
}
