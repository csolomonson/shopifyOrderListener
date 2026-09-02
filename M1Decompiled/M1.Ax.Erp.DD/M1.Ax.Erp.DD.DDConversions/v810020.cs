using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.10.020", "Update code for references to App properties and methods", "")]
public class v810020
{
	public v810020(DDConversionParms parms)
	{
		parms.DmoDD.ConvertCustomControls(parms.DatabaseName, "frmCallQueue", "M1.Ax.Erp.Forms.Sales.Call.CallQueueForm", string.Empty, -1, -1, convertDataBindings: false, null, moveCustomControlsToBottom: false, parms.ConvertCustomFormCode);
		parms.DmoDD.ConvertCustomControls(parms.DatabaseName, "frmJobSplit", "M1.Ax.Erp.Forms.Production.Job.JobSplitForm", string.Empty, -1, -1, convertDataBindings: false, null, moveCustomControlsToBottom: false, parms.ConvertCustomFormCode);
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[13]
		{
			new TranslateInfo("App.CalculateQtyWithScrap", "App.JobFunctions.CalculateQtyWithScrap", ignoreCase: true),
			new TranslateInfo("App.CalculateProductionHours", "App.JobFunctions.CalculateProductionHours", ignoreCase: true),
			new TranslateInfo("App.CalculateDiscountAndDueDate", "App.Financial.CalculateDiscountAndDueDate", ignoreCase: true),
			new TranslateInfo("App.CalculateTaxOnTotal", "App.Financial.CalculateTaxOnTotal", ignoreCase: true),
			new TranslateInfo("App.CalculateTaxOnSubTotal", "App.Financial.CalculateTaxOnSubTotal", ignoreCase: true),
			new TranslateInfo("App.CalculateSecondaryTax", "App.Financial.CalculateSecondaryTax", ignoreCase: true),
			new TranslateInfo("App.ExchangeCurrency", "App.LegacyFunctions.ExchangeCurrency", ignoreCase: true),
			new TranslateInfo("App.RefreshCurrencyForDetails", "App.LegacyFunctions.RefreshCurrencyForDetails", ignoreCase: true),
			new TranslateInfo("App.GetExchangeRate", "App.Financial.GetExchangeRate", ignoreCase: true),
			new TranslateInfo("App.OrgMemoCheck", "App.LegacyFunctions.OrgMemoCheck", ignoreCase: true),
			new TranslateInfo("App.PartMemoCheck", "App.LegacyFunctions.PartMemoCheck", ignoreCase: true),
			new TranslateInfo("App.RunCustomAppCode", "App.Script.RunCustomAppCode", ignoreCase: true),
			new TranslateInfo("App.DatasetID", "Mid(App.Database,4)", ignoreCase: true)
		});
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[50]
		{
			new TranslateInfo("App.ShowAboutForm", "Forms.Show.About", ignoreCase: true),
			new TranslateInfo("App.ShowCallQueue", "Forms.Show.CallQueue", ignoreCase: true),
			new TranslateInfo("App.ShowChangeLog", "Forms.Show.ChangeLog", ignoreCase: true),
			new TranslateInfo("App.ShowChangePasswordForm", "Forms.Show.ChangePassword", ignoreCase: true),
			new TranslateInfo("App.ShowCompanyMessageForm", "Forms.Show.CompanyMessage", ignoreCase: true),
			new TranslateInfo("App.ShowConflictsForm", "Forms.Show.Conflicts", ignoreCase: true),
			new TranslateInfo("App.CreateQuickCall", "Forms.Show.QuickCall", ignoreCase: true),
			new TranslateInfo("App.ShowCreditCardPaymentNET1Form", "Forms.Show.CreditCardPaymentNET1", ignoreCase: true),
			new TranslateInfo("App.ShowCreditCardPaymentNET1WizardForm", "Forms.Show.CreditCardPaymentNET1Wizard", ignoreCase: true),
			new TranslateInfo("App.ShowCurrentActivityForm", "Forms.Show.CurrentActivity", ignoreCase: true),
			new TranslateInfo("App.ShowCurrentSqlConnectionsForm", "Forms.Show.CurrentSqlConnections", ignoreCase: true),
			new TranslateInfo("App.ShowCustomForm", "Forms.Show.CustomForm", ignoreCase: true),
			new TranslateInfo("App.ShowCustomizeGridForm", "Forms.Show.CustomizeGrid", ignoreCase: true),
			new TranslateInfo("App.ShowDatabaseOptionsForm", "Forms.Show.DatabaseOptions", ignoreCase: true),
			new TranslateInfo("App.ShowDataMapManager", "Forms.Show.DataMapManager", ignoreCase: true),
			new TranslateInfo("App.ShowDesignStudio", "Forms.Show.DesignStudio", ignoreCase: true),
			new TranslateInfo("App.ShowFormulaEditorForm", "Forms.Show.FormulaEditor", ignoreCase: true),
			new TranslateInfo("App.MapLocation", "Forms.Show.MapLocation", ignoreCase: true),
			new TranslateInfo("App.ShowOpenWithEditor", "Forms.Show.OpenWithEditor", ignoreCase: true),
			new TranslateInfo("App.ShowPayPalPasswordForm", "Forms.Show.PayPalPassword", ignoreCase: true),
			new TranslateInfo("App.ShowProductConfiguratorForm", "Forms.Show.ProductConfigurator", ignoreCase: true),
			new TranslateInfo("App.ShowPurgeForm", "Forms.Show.Purge", ignoreCase: true),
			new TranslateInfo("App.RunQuery", "Forms.Show.RunQuery", ignoreCase: true),
			new TranslateInfo("App.SaveAs", "Forms.Show.SaveAs", ignoreCase: true),
			new TranslateInfo("App.ShowScheduleMoveOperations", "Forms.Show.ScheduleMoveOperations", ignoreCase: true),
			new TranslateInfo("App.ShowSelectDatasetsForm", "Forms.Show.SelectDatasets", ignoreCase: true),
			new TranslateInfo("App.ShowSelectFieldsForm", "Forms.Show.SelectFields", ignoreCase: true),
			new TranslateInfo("App.ShowSelectGridForm", "Forms.Show.SelectGrid", ignoreCase: true),
			new TranslateInfo("App.ShowSetDefaultValueForm", "Forms.Show.SetDefaultValue", ignoreCase: true),
			new TranslateInfo("App.ShowSpellCheck", "Forms.Show.SpellCheck", ignoreCase: true),
			new TranslateInfo("App.ShowTableSecurityAccessWizard", "Forms.Show.TableSecurityAccessWizard", ignoreCase: true),
			new TranslateInfo("App.ShowUserAdministrationForm", "Forms.Show.UserAdministration", ignoreCase: true),
			new TranslateInfo("App.ShowUserOptionsForm", "Forms.Show.UserOptions", ignoreCase: true),
			new TranslateInfo("App.ShowValidationMsg", "Forms.Show.ValidationMessage", ignoreCase: true),
			new TranslateInfo("App.ShowWhereUsed", "Forms.Show.WhereUsed", ignoreCase: true),
			new TranslateInfo("App.OpenPhoneManager", "Forms.Show.PhoneManager", ignoreCase: true),
			new TranslateInfo("App.OpenM1Explorer", "Forms.Show.M1Explorer", ignoreCase: true),
			new TranslateInfo("App.OpenSearch", "Forms.Show.Search", ignoreCase: true),
			new TranslateInfo("App.OpenComSearch", "Forms.Show.Search", ignoreCase: true),
			new TranslateInfo("App.PromptToScheduleJob", "Forms.Show.ScheduleJob", ignoreCase: true),
			new TranslateInfo("App.PrintRelatedDocuments", "Forms.Show.PrintRelatedDocuments", ignoreCase: true),
			new TranslateInfo("App.PromptToSearch", "Forms.Show.Search", ignoreCase: true),
			new TranslateInfo("App.PromptForAppointmentColor", "Forms.CommonDialogs.AppointmentColor", ignoreCase: true),
			new TranslateInfo("App.ActivateExplorerObjectById", "Forms.Show.ExplorerObject", ignoreCase: true),
			new TranslateInfo("App.ActivateExplorerSearchByGridID", "Forms.Show.ExplorerSearch", ignoreCase: true),
			new TranslateInfo("App.OpenReport", "Forms.Report.Open", ignoreCase: true),
			new TranslateInfo("App.RunReport", "Forms.Report.Run", ignoreCase: true),
			new TranslateInfo("App.RunReportByTableKeys", "Forms.Report.RunByTableKeys", ignoreCase: true),
			new TranslateInfo("App.ShowImplementationCheckList", "Forms.OpenForm \"M1.Ax.Erp.Forms.ImplementationCheckList.ImplementationCheckListForm\"", ignoreCase: true),
			new TranslateInfo("App.CreateFolder", "App.IO.CreateFolder", ignoreCase: true)
		});
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[7]
		{
			new TranslateInfo("App.JobFunctions.ShowScheduleJobsForm", "Forms.Show.ScheduleJobs", ignoreCase: true),
			new TranslateInfo("App.JobFunctions.PromptToScheduleJob", "Forms.Show.ScheduleJob", ignoreCase: true),
			new TranslateInfo("App.JobFunctions.OpenJobWizard", "Forms.Show.JobWizard", ignoreCase: true),
			new TranslateInfo("App.JobFunctions.OpenSchedulingBoard", "Forms.Show.SchedulingBoard", ignoreCase: true),
			new TranslateInfo("App.TimecardFunctions.ShowLeaveBoard", "Forms.Show.LeaveBoard", ignoreCase: true),
			new TranslateInfo("App.POFunctions.OpenPOWizard", "Forms.Show.PurchasingWizard", ignoreCase: true),
			new TranslateInfo("Call App.PayrollFunctions.ShowLoadTaxTablesForm(\"Import\")", "Forms.OpenProcessForm \"M1.Ax.Erp.ImportTaxTableProcess\"", ignoreCase: true)
		});
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[4]
		{
			new TranslateInfo("App.ExportRs.MailMerge", "Forms.MailMerge", ignoreCase: true),
			new TranslateInfo("App.ExportRs.Prompt", "Forms.CommonDialogs.Export", ignoreCase: true),
			new TranslateInfo("App.ImportFunctions.ExportFile", "Forms.CommonDialogs.DataMapExport", ignoreCase: true),
			new TranslateInfo("App.ImportFunctions.ImportFile", "Forms.CommonDialogs.DataMapImport", ignoreCase: true)
		});
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[2]
		{
			new TranslateInfo("App.CommonDialogs.", "Forms.CommonDialogs.", ignoreCase: true),
			new TranslateInfo("App.ExportRs.", "App.Export.", ignoreCase: true)
		});
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[32]
		{
			new TranslateInfo("App.LoadForm", "Forms.LoadForm", ignoreCase: true),
			new TranslateInfo("App.LoadFormReturn", "Forms.LoadFormReturn", ignoreCase: true),
			new TranslateInfo("App.OpenForm", "Forms.OpenForm", ignoreCase: true),
			new TranslateInfo("App.ShowForm", "Forms.ShowForm", ignoreCase: true),
			new TranslateInfo("App.MsgWindow", "Forms.MessageWindow", ignoreCase: true),
			new TranslateInfo("App.MessageBox", "Forms.MessageBox", ignoreCase: true),
			new TranslateInfo("App.CloseForm", "Forms.CloseForm", ignoreCase: true),
			new TranslateInfo("App.ForceForegroundWindow", "Forms.ForceForegroundWindow", ignoreCase: true),
			new TranslateInfo("App.GetActiveHwnd", "Forms.GetActiveHwnd", ignoreCase: true),
			new TranslateInfo("App.GetFormFromHwnd", "Forms.GetFormFromHwnd", ignoreCase: true),
			new TranslateInfo("App.GetFormHwnd", "Forms.GetFormHwnd", ignoreCase: true),
			new TranslateInfo("App.IsFormOpen", "Forms.IsFormOpen", ignoreCase: true),
			new TranslateInfo("App.MousePointer", "Forms.MousePointer", ignoreCase: true),
			new TranslateInfo("App.PlaySound", "Forms.PlaySound", ignoreCase: true),
			new TranslateInfo("App.SetFocusToForm", "Forms.SetFocusToForm", ignoreCase: true),
			new TranslateInfo("App.OpenHelp", "Forms.OpenHelp", ignoreCase: true),
			new TranslateInfo("App.RunScript", "Forms.RunScript", ignoreCase: true),
			new TranslateInfo("App.OpenObject", "Forms.OpenObject", ignoreCase: true),
			new TranslateInfo("App.ShellPrintDocument", "Forms.ShellOpenDocument", ignoreCase: true),
			new TranslateInfo("App.ShellOpenDoc", "Forms.ShellOpenDocument", ignoreCase: true),
			new TranslateInfo("App.ShellExecute", "Forms.ShellExecute", ignoreCase: true),
			new TranslateInfo("App.SendMessageEx", "Forms.SendEmail", ignoreCase: true),
			new TranslateInfo("App.SendMessage", "Forms.SendEmail", ignoreCase: true),
			new TranslateInfo("App.SendHTMLMessage", "Forms.SendEmail", ignoreCase: true),
			new TranslateInfo("App.OpenBlankEmail", "Forms.SendEmail", ignoreCase: true),
			new TranslateInfo("App.SendCodeAsAttachment", "Forms.SendCodeAsAttachment", ignoreCase: true),
			new TranslateInfo("App.SendChangeRequestEmail", "Forms.Ax(\"ChangeRequests\").SendChangeRequestEmail", ignoreCase: true),
			new TranslateInfo("App.ExportFollowUpToOutlook", "Forms.Ax(\"FollowUps\").ExportFollowUpToOutlook", ignoreCase: true),
			new TranslateInfo("App.ExportScheduleToOutlook", "Forms.Ax(\"Jobs\").ExportScheduleToOutlook", ignoreCase: true),
			new TranslateInfo("App.ExportEmployeeToOutlook", "Forms.Ax(\"Employees\").ExportEmployeeToOutlook", ignoreCase: true),
			new TranslateInfo("App.RefreshFollowupsFromOutlook", "Forms.Ax(\"FollowUps\").RefreshFollowupsFromOutlook", ignoreCase: true),
			new TranslateInfo("App.Clipboard", "Forms.Clipboard", ignoreCase: true)
		});
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[6]
		{
			new TranslateInfo("App.AddQuotes", "App.Convert.StringToSql", ignoreCase: true),
			new TranslateInfo("App.ConvertNumberToSql", "App.Convert.NumberToSql", ignoreCase: true),
			new TranslateInfo("App.ConvertDateTimeToSql", "App.Convert.DateTimeToSql", ignoreCase: true),
			new TranslateInfo("App.ConvertDateToSQL", "App.Convert.DateToSql", ignoreCase: true),
			new TranslateInfo("App.ConvertUnknownToSQL", "App.Convert.ToSql", ignoreCase: true),
			new TranslateInfo("App.FormatUnknownForScript", "App.Convert.ToScript", ignoreCase: true)
		});
	}
}
