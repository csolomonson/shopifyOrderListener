using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class ReceiptPostProcess : ProcessParameters
{
	public ReceiptPostProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		KeyValueFieldNames = new string[1] { "rmpReceiptID" };
		KeyValueTableName = "Receipts";
		Description = "Use this screen to post your unposted receipts to inventory.";
		GridID = "M1MASSPOSTRECEIPTS";
		SecurityRole = "RECEIPTPOST";
		HelpLink = "RM_PostReceipts.htm";
		ContinueMessage = "This will post the {0} selected receipt(s) to inventory. Once the record has been posted, you will be unable to edit that record. Are you sure you want to continue?";
		BindingSourceTable = string.Empty;
		DateTime today = DateTime.Today;
		today = today.AddDays(-today.Day + 1);
		DateTime value = today.AddMonths(1).AddDays(-1.0);
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Receipt Date")
		{
			IgnoreWhenEmpty = true,
			ValueField = "rmpReceiptDate",
			AdditionalFields = "rmpReceiptDate",
			ValueStart = today,
			ValueEnd = value
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Supplier", null, new string[1] { "rmpSupplierOrganizationID" })
		{
			ValueFields = new string[1] { "rmpSupplierOrganizationID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Plants", null, new string[2] { "rmpPlantID", "rmpPlantDepartmentID" })
		{
			AdditionalFields = "rmpPlantID,rmpPlantDepartmentID",
			ValueFields = new string[2] { "rmpPlantID", "rmpPlantDepartmentID" }
		});
		PromptFieldValidations.Add(new PromptFieldValidationBool("rmpPostedToGL", fieldValue: true, "Receipt is posted."));
	}

	public override void RunNegativeQtyOnHandMethod(StartProcessEventArgs arg)
	{
		List<string> receiptsIdList = (from itemValue in arg.SelectedItems
			where !itemValue.DiscardSave
			select itemValue.KeyValues[0].ToString()).ToList();
		PostReceipts(receiptsIdList);
	}

	private void PostReceipts(List<string> receiptsIdList)
	{
		if (!receiptsIdList.Any())
		{
			return;
		}
		M1Database m1Database = (M1Database)ServiceProvider.GetService(typeof(M1Database));
		Receipts receipts = new Receipts();
		using M1BindingSource m1BindingSource = new M1BindingSource(m1Database);
		foreach (string receiptsId in receiptsIdList)
		{
			m1BindingSource.DataSourceTable = "Receipts";
			m1BindingSource.NavigateTo(m1Database, "rmpReceiptID = " + M1Util.ConvertToSql(receiptsId));
			m1BindingSource.PrimaryTable.GetChildBindingSource("ReceiptLines").PrimaryTable.GetChildBindingSource("ReceiptComponents");
			receipts.PostReceipt(m1BindingSource);
			m1BindingSource.SaveData();
		}
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		M1Database m1Database = (M1Database)ServiceProvider.GetService(typeof(M1Database));
		List<string> messages = arg.Messages;
		bool flag = (bool)m1Database.Props("IM")["xapIMAllowNegativeQtyOnHand"];
		if (selectedItems.Count == 0)
		{
			return;
		}
		List<string> list = new List<string>();
		int num = 0;
		StringBuilder stringBuilder = new StringBuilder();
		bool flag2 = false;
		bool flag3 = false;
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		foreach (ProcessSelectedItemValues item in selectedItems)
		{
			string text = item.KeyValues[0].ToString();
			M1BindingSource m1BindingSource = new M1BindingSource(m1Database);
			m1BindingSource.DataSourceTable = "RECEIPTS";
			m1BindingSource.NavigateTo(m1Database, "rmpReceiptID = " + M1Util.ConvertToSql(text));
			if (m1BindingSource == null)
			{
				continue;
			}
			if (m1Database.Props("GL").Field<bool>("xafGLCreateStockJournals") && !new Receipts().ReceiptPeriodCheck(m1BindingSource))
			{
				num++;
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(text);
			}
			if (!num.Equals(0))
			{
				continue;
			}
			flag2 = new Receipts().NegativeSerialLotPartCheck(m1BindingSource);
			flag3 = !flag2 && new Receipts().ReceiptPostCheck(m1BindingSource);
			if (!flag3)
			{
				list.Add(text);
				string text2 = new Receipts().VerifyQuantityForInactiveBins(m1BindingSource.Database, m1BindingSource.CurrentAsDataRow.Field<string>("rmpReceiptID"));
				if (!string.IsNullOrEmpty(text2))
				{
					list3.Add(text);
				}
				string msg;
				bool flag4 = !new Receipts().GetMessageForInactivePartBins(m1BindingSource.Database, m1BindingSource.CurrentAsDataRow, out msg);
				if (flag && !flag3)
				{
					string messageForNegativeParts = new Receipts().GetMessageForNegativeParts(m1BindingSource);
					if (!flag2 && messageForNegativeParts.Length > 0 && !flag4 && text2.Length == 0)
					{
						list2.Add(text);
						list.Remove(text);
					}
					else
					{
						item.DiscardSave = true;
					}
				}
			}
			else
			{
				m1BindingSource.PrimaryTable.GetChildBindingSource("ReceiptLines").PrimaryTable.GetChildBindingSource("ReceiptComponents");
				new Receipts().PostReceipt(m1BindingSource);
				m1BindingSource.SaveData();
			}
		}
		if (flag)
		{
			if (list2.Any() && list3.Count == 0)
			{
				arg.NegativeQtyOnHandMessages.Add("The following receipts contain lines which will result in a negative quantity on hand.");
				arg.NegativeQtyOnHandMessages.Add("Are you sure?");
				arg.NegativeQtyOnHandMessages.AddRange(list2);
				arg.ShowNegativeQtyOnHandMsg = true;
			}
			else
			{
				arg.ShowNegativeQtyOnHandMsg = false;
			}
		}
		if (list.Count != 0 || stringBuilder.Length != 0)
		{
			messages.Add("The following receipts cannot be posted for one of the following reasons:");
		}
		if (list.Count != 0)
		{
			messages.Add("- they are already posted");
			messages.Add("- not linked to a landed cost");
			messages.Add("- the job has been closed");
			messages.Add("- the received parts have been issued");
			messages.Add(flag ? "- there is a reversal for a serial/lot tracked part that will result in negative quantity on hand" : "- there is insufficient quantity on hand");
			messages.Add("- there is a reversed receipt line with an inactive bin that will result in negative quantity on hand");
			messages.Add("- an inactive bin location is assigned to a receipt line");
			messages.Add(string.Join(",", list));
		}
		if (stringBuilder.Length != 0)
		{
			messages.Add("- the fiscal period for the transaction date has been closed or does not exist:");
			messages.Add(stringBuilder.ToString());
		}
	}
}
