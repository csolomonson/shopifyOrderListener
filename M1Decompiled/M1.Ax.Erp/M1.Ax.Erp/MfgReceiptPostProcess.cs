using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class MfgReceiptPostProcess : ProcessParameters
{
	public MfgReceiptPostProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		KeyValueFieldNames = new string[1] { "rmmMfgReceiptID" };
		KeyValueTableName = "MfgReceipts";
		Description = "Use this screen to post your unposted mfg/misc receipts to inventory.";
		GridID = "M1MASSPOSTMFGRECEIPTS";
		SecurityRole = "MFGRECEIPTPOST";
		HelpLink = "IM_PostMfgMiscReceipts.htm";
		ContinueMessage = "This will post the {0} selected mfg/misc receipt(s) to inventory. Once the record has been posted, you will be unable to edit that record. Are you sure you want to continue?";
		BindingSourceTable = string.Empty;
		DateTime today = DateTime.Today;
		today = today.AddDays(-today.Day + 1);
		DateTime value = today.AddMonths(1).AddDays(-1.0);
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Receipt Date")
		{
			IgnoreWhenEmpty = true,
			ValueField = "rmmReceiptDate",
			AdditionalFields = "rmmReceiptDate",
			ValueStart = today,
			ValueEnd = value
		});
		PromptFieldValidations.Add(new PromptFieldValidationBool("rmmPosted", fieldValue: true, "Mfg/misc Receipt is posted."));
	}

	public override void RunNegativeQtyOnHandMethod(StartProcessEventArgs arg)
	{
		List<string> mfgReceiptsIdList = (from itemValue in arg.SelectedItems
			where !itemValue.DiscardSave
			select itemValue.KeyValues[0].ToString()).ToList();
		PostMfgReceipts(mfgReceiptsIdList);
	}

	private void PostMfgReceipts(List<string> mfgReceiptsIdList)
	{
		if (!mfgReceiptsIdList.Any())
		{
			return;
		}
		M1Database m1Database = (M1Database)ServiceProvider.GetService(typeof(M1Database));
		MfgReceipt mfgReceipt = new MfgReceipt();
		foreach (string mfgReceiptsId in mfgReceiptsIdList)
		{
			using M1BindingSource m1BindingSource = new M1BindingSource(m1Database);
			m1BindingSource.DataSourceTable = "MfgReceipts";
			m1BindingSource.NavigateTo(m1Database, "rmmMfgReceiptID = " + M1Util.ConvertToSql(mfgReceiptsId));
			m1BindingSource.PrimaryTable.GetChildBindingSource("MfgReceiptComponents");
			mfgReceipt.PostMfgReceipt(m1BindingSource);
			m1BindingSource.SaveData();
		}
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		if (selectedItems.Count == 0)
		{
			return;
		}
		M1Database m1Database = (M1Database)ServiceProvider.GetService(typeof(M1Database));
		bool flag = (bool)m1Database.Props("IM")["xapIMAllowNegativeQtyOnHand"];
		List<string> messages = arg.Messages;
		MfgReceipt mfgReceipt = new MfgReceipt();
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		int num = 0;
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		foreach (ProcessSelectedItemValues item in selectedItems)
		{
			string text = item.KeyValues[0].ToString();
			using M1BindingSource m1BindingSource = new M1BindingSource(m1Database);
			m1BindingSource.DataSourceTable = "MfgReceipts";
			m1BindingSource.NavigateTo(m1Database, "rmmMfgReceiptID = " + M1Util.ConvertToSql(text));
			if (m1BindingSource == null)
			{
				continue;
			}
			if (m1Database.Props("GL").Field<bool>("xafGLCreateStockJournals") && !mfgReceipt.MfgReceiptPeriodCheck(m1BindingSource))
			{
				num++;
				if (stringBuilder2.Length != 0)
				{
					stringBuilder2.Append(",");
				}
				stringBuilder2.Append(text);
			}
			if (!num.Equals(0))
			{
				continue;
			}
			if (!string.IsNullOrWhiteSpace(mfgReceipt.MfgReceiptPostCheck(m1BindingSource)))
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(text);
			}
			else
			{
				m1BindingSource.PrimaryTable.GetChildBindingSource("MfgReceiptComponents");
				mfgReceipt.PostMfgReceipt(m1BindingSource);
				m1BindingSource.SaveData();
			}
			bool flag2 = mfgReceipt.MfgReceiptPostedCheck(m1BindingSource.Database, m1BindingSource.Transaction, m1BindingSource.CurrentAsDataRow.Field<string>("rmmMfgReceiptID"));
			bool flag3 = m1BindingSource.CurrentAsDataRow.Field<bool>("rmmReversalEntry");
			if (flag)
			{
				if (flag2)
				{
					continue;
				}
				byte b = m1BindingSource.CurrentAsDataRow.Field<byte>("rmmReceiptType");
				IDictionary<PartInformation, decimal> partInformantionAndQuantityToReturn = mfgReceipt.GetPartInformantionAndQuantityToReturn(m1BindingSource);
				bool flag4 = mfgReceipt.VerifyQuantityOnHand(m1BindingSource.Database, partInformantionAndQuantityToReturn).Any();
				bool flag5 = partInformantionAndQuantityToReturn.Any((KeyValuePair<PartInformation, decimal> keyValuePair) => keyValuePair.Key.IsSerialLotPart && keyValuePair.Key.HasNegativeQOH);
				bool flag6 = partInformantionAndQuantityToReturn.Any((KeyValuePair<PartInformation, decimal> keyValuePair) => keyValuePair.Key.IsBinInactive && keyValuePair.Key.HasNegativeQOH);
				if (flag3)
				{
					if (flag4 && b != 1 && !flag5)
					{
						if (!flag6)
						{
							list.Add(text);
							stringBuilder = stringBuilder.Replace((stringBuilder.ToString().Contains(",") ? "," : "") + text, "");
						}
					}
					else
					{
						item.DiscardSave = true;
					}
				}
				if (flag6)
				{
					list2.Add(text);
				}
				continue;
			}
			if (!flag2)
			{
				if (!mfgReceipt.GetMfgReceiptInactivePartBinsMessage(m1BindingSource.Database, m1BindingSource.CurrentAsDataRow, out var _))
				{
					list2.Add(text);
				}
				else
				{
					item.DiscardSave = true;
				}
			}
		}
		if (flag)
		{
			if (list.Any())
			{
				arg.ShowNegativeQtyOnHandMsg = true;
				arg.NegativeQtyOnHandMessages.Add("The following mfg/misc receipts will result in a negative quantity on hand.");
				arg.NegativeQtyOnHandMessages.Add("Are you sure?");
				arg.NegativeQtyOnHandMessages.AddRange(list);
			}
			else
			{
				arg.ShowNegativeQtyOnHandMsg = false;
			}
		}
		if (stringBuilder.Length != 0 || stringBuilder2.Length != 0)
		{
			messages.Add("The following mfg/misc receipts cannot be posted for one of the following reasons:");
		}
		if (stringBuilder.Length != 0)
		{
			messages.Add("- they are already posted");
			messages.Add("- the job has been closed");
			messages.Add("- the received parts have been issued");
			if (flag)
			{
				messages.Add("- there is a reversal for a serial/lot tracked part that will result in negative quantity on hand");
			}
			else
			{
				messages.Add("- there is insufficient remaining quantity");
			}
			if (list2.Any())
			{
				messages.Add("- an inactive bin location is assigned");
			}
			messages.Add(stringBuilder.ToString());
		}
		if (stringBuilder2.Length != 0)
		{
			messages.Add("- the fiscal period for the transaction date has been closed or does not exist:");
			messages.Add(stringBuilder2.ToString());
		}
	}
}
