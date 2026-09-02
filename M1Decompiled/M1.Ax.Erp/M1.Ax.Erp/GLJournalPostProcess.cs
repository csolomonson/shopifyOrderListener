using System;
using System.Collections.Generic;
using System.Text;
using M1.Core;
using M1.Core.Script;
using M1.Script.Interfaces;
using M1Classes92;

namespace M1.Ax.Erp;

public class GLJournalPostProcess : ProcessParameters
{
	public GLJournalPostProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		KeyValueFieldNames = new string[1] { "glpGLJournalID" };
		KeyValueTableName = "GLJournals";
		Description = "Use this screen to post your open GL Journals to the General Ledger.";
		GridID = "M1ADDFROMPOSTGLJOURNALS";
		SecurityRole = "GLPost";
		HelpLink = "gl_journalpost.htm";
		BindingSourceTable = string.Empty;
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Year/Periods", null, new string[2] { "glpGLFiscalYearID", "glpGLFiscalYearPeriodID" })
		{
			AdditionalFields = "glpGLFiscalYearID,glpGLFiscalYearPeriodID",
			ValueFields = new string[2] { "glpGLFiscalYearID", "glpGLFiscalYearPeriodID" },
			InputSize = 10
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Source", null, new string[1] { "glpSource" })
		{
			ValueFields = new string[1] { "glpSource" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Detail Source", null, new string[1] { "glpDetailSource" })
		{
			ValueFields = new string[1] { "glpDetailSource" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Organization", null, new string[1] { "glpOrganizationID" })
		{
			ValueFields = new string[1] { "glpOrganizationID" }
		});
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		if (selectedItems.Count == 0)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (ProcessSelectedItemValues item in selectedItems)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append(item.KeyValues[0].ToString());
		}
		if (stringBuilder.Length != 0)
		{
			clsGLFunctionsClass obj = new clsGLFunctionsClass();
			obj.SetReferences(ServiceProvider.GetService(typeof(ScriptApp)), ServiceProvider.GetService(typeof(IForms)));
			if (!obj.PostSelectedJournals(stringBuilder.ToString(), string.Empty))
			{
				arg.Messages.Add("Could not post journals " + stringBuilder.ToString());
				arg.Cancel = true;
			}
		}
	}
}
