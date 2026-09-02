using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class MaterialIssuePostProcess : ProcessParameters
{
	public MaterialIssuePostProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		KeyValueFieldNames = new string[1] { "iniMaterialIssueID" };
		KeyValueTableName = "MaterialIssues";
		Description = "Use this screen to post your unposted material issues to inventory.";
		GridID = "M1MASSPOSTMATERIALISSUE";
		SecurityRole = "MATISSUEPOST";
		HelpLink = "IM_PostMatlIssues.htm";
		ContinueMessage = "This will post the {0} selected material issue(s) to inventory. Once the record has been posted, you will be unable to edit that record. Are you sure you want to continue?";
		BindingSourceTable = string.Empty;
		DateTime today = DateTime.Today;
		today = today.AddDays(-today.Day + 1);
		DateTime value = today.AddMonths(1).AddDays(-1.0);
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Issue Date")
		{
			IgnoreWhenEmpty = true,
			ValueField = "iniMaterialIssueDate",
			AdditionalFields = "iniMaterialIssueDate",
			ValueStart = today,
			ValueEnd = value
		});
		PromptFieldValidations.Add(new PromptFieldValidationBool("iniPosted", fieldValue: true, "Material Issue is posted."));
	}

	public override void RunNegativeQtyOnHandMethod(StartProcessEventArgs arg)
	{
		List<string> materialIssues = (from itemValue in arg.SelectedItems
			where !itemValue.DiscardSave
			select itemValue.KeyValues[0].ToString()).ToList();
		PostMaterialIssues(materialIssues);
	}

	private void PostMaterialIssues(List<string> materialIssues)
	{
		if (!materialIssues.Any())
		{
			return;
		}
		M1Database m1Database = (M1Database)ServiceProvider.GetService(typeof(M1Database));
		foreach (string materialIssue in materialIssues)
		{
			using M1BindingSource m1BindingSource = new M1BindingSource(m1Database);
			m1BindingSource.DataSourceTable = "MaterialIssues";
			m1BindingSource.NavigateTo(m1Database, "iniMaterialIssueID = " + M1Util.ConvertToSql(materialIssue));
			m1BindingSource.PrimaryTable.GetChildBindingSource("MaterialIssueLines").PrimaryTable.GetChildBindingSource("MaterialIssueComponents");
			new MaterialIssue().PostMaterialIssue(m1BindingSource);
			m1BindingSource.SaveData();
		}
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		M1Database m1Database = (M1Database)ServiceProvider.GetService(typeof(M1Database));
		List<string> list = new List<string>();
		bool flag = (bool)m1Database.Props("IM")["xapIMAllowNegativeQtyOnHand"];
		if (selectedItems.Count == 0)
		{
			return;
		}
		int num = 0;
		StringBuilder stringBuilder = new StringBuilder();
		int num2 = 0;
		StringBuilder stringBuilder2 = new StringBuilder();
		string empty = string.Empty;
		string empty2 = string.Empty;
		bool flag2 = false;
		bool flag3 = false;
		List<string> list2 = new List<string>();
		foreach (ProcessSelectedItemValues item in selectedItems)
		{
			using M1BindingSource m1BindingSource = new M1BindingSource(m1Database);
			string text = item.KeyValues[0].ToString();
			m1BindingSource.DataSourceTable = "MaterialIssues";
			m1BindingSource.NavigateTo(m1Database, "iniMaterialIssueID = " + M1Util.ConvertToSql(text));
			if (m1BindingSource == null)
			{
				continue;
			}
			if (m1Database.Props("GL").Field<bool>("xafGLCreateStockJournals") && !new MaterialIssue().MaterialIssuePeriodCheck(m1BindingSource))
			{
				num2++;
				if (stringBuilder2.Length != 0)
				{
					stringBuilder2.Append(",");
				}
				stringBuilder2.Append(text);
			}
			if (!num2.Equals(0))
			{
				continue;
			}
			string value = new MaterialIssue().PostMaterialIssueCheck(m1BindingSource);
			empty2 = new MaterialIssue().VerifyInactiveBinsForReturnToJob(m1BindingSource);
			empty = new MaterialIssue().VerifyInactiveBinsMiscOrJobIssue(m1BindingSource);
			if (!string.IsNullOrWhiteSpace(value) || !string.IsNullOrEmpty(empty2) || !string.IsNullOrEmpty(empty))
			{
				num++;
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(text);
				if (!string.IsNullOrEmpty(empty2))
				{
					flag3 = true;
				}
				if (!string.IsNullOrEmpty(empty))
				{
					flag2 = true;
				}
				if (flag)
				{
					if (new MaterialIssue().CheckFutureDatePost(m1BindingSource.CurrentAsDataRow))
					{
						list2.Add(text);
						item.DiscardSave = true;
					}
					else
					{
						list.Add(text);
					}
				}
			}
			else
			{
				m1BindingSource.PrimaryTable.GetChildBindingSource("MaterialIssueLines").PrimaryTable.GetChildBindingSource("MaterialIssueComponents");
				new MaterialIssue().PostMaterialIssue(m1BindingSource);
				m1BindingSource.SaveData();
			}
		}
		if (flag)
		{
			if (list.Any() && !flag3 && !flag2)
			{
				arg.NegativeQtyOnHandMessages.Add("The following Material Issues contain delivery lines which will result in a negative quantity on hand.");
				arg.NegativeQtyOnHandMessages.Add("Are you sure?");
				arg.NegativeQtyOnHandMessages.AddRange(list);
				arg.ShowNegativeQtyOnHandMsg = true;
			}
			else
			{
				arg.ShowNegativeQtyOnHandMsg = false;
			}
		}
		if ((stringBuilder.Length != 0 && !flag) || stringBuilder2.Length != 0 || list2.Count > 0 || flag2 || flag3)
		{
			messages.Add("The following material issues cannot be posted for one of the following reasons:");
		}
		if ((stringBuilder.Length != 0 && !flag) || list2.Any() || flag3 || flag2)
		{
			messages.Add("- they are already posted");
			if (list2.Any())
			{
				messages.Add("- this is a future dated material issue that will result in negative quantity on hand");
				messages.Add(string.Join(",", list2));
			}
			else
			{
				messages.Add("- there is insufficient remaining quantity");
				if (flag2)
				{
					messages.Add("- posting the material issue will result in a negative quantity on hand for an inactive bin");
				}
				if (flag3)
				{
					messages.Add("- an inactive bin location is assigned to a material issue line");
				}
				messages.Add(stringBuilder.ToString());
			}
		}
		if (stringBuilder2.Length != 0)
		{
			messages.Add("- the fiscal period for the transaction date has been closed or does not exist:");
			messages.Add(stringBuilder2.ToString());
		}
	}
}
