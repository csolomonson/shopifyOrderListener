using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class ShipmentPostProcess : ProcessParameters
{
	public ShipmentPostProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		KeyValueFieldNames = new string[1] { "smpShipmentID" };
		KeyValueTableName = "Shipments";
		Description = "Use this screen to post your unposted shipments to inventory.";
		GridID = "M1MASSPOSTSHIPMENTS";
		SecurityRole = "SHIPMENTPOST";
		HelpLink = "SM_PostShipments.htm";
		ContinueMessage = "This will post the {0} selected shipment(s) to inventory. Once the record has been posted, you will be unable to edit that record. Are you sure you want to continue?";
		BindingSourceTable = string.Empty;
		DateTime today = DateTime.Today;
		today = today.AddDays(-today.Day + 1);
		DateTime value = today.AddMonths(1).AddDays(-1.0);
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Shipment Date")
		{
			IgnoreWhenEmpty = true,
			ValueField = "smpShipDate",
			AdditionalFields = "smpShipDate",
			ValueStart = today,
			ValueEnd = value
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Customer", null, new string[1] { "smpCustomerOrganizationID" })
		{
			ValueFields = new string[1] { "smpCustomerOrganizationID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Plants", null, new string[2] { "smpPlantID", "smpPlantDepartmentID" })
		{
			AdditionalFields = "smpPlantID,smpPlantDepartmentID",
			ValueFields = new string[2] { "smpPlantID", "smpPlantDepartmentID" }
		});
		PromptFieldValidations.Add(new PromptFieldValidationBool("smpPostedToGL", fieldValue: true, "Shipment is posted."));
	}

	public override void RunNegativeQtyOnHandMethod(StartProcessEventArgs arg)
	{
		List<string> shipmentIDList = (from itemValue in arg.SelectedItems
			where !itemValue.DiscardSave
			select itemValue.KeyValues[0].ToString()).ToList();
		PostShipments(shipmentIDList);
	}

	private void PostShipments(List<string> shipmentIDList)
	{
		if (!shipmentIDList.Any())
		{
			return;
		}
		M1Database m1Database = (M1Database)ServiceProvider.GetService(typeof(M1Database));
		foreach (string shipmentID in shipmentIDList)
		{
			using M1BindingSource m1BindingSource = new M1BindingSource(m1Database);
			m1BindingSource.DataSourceTable = "SHIPMENTS";
			m1BindingSource.NavigateTo(m1Database, "smpShipmentID = " + M1Util.ConvertToSql(shipmentID));
			m1BindingSource.PrimaryTable.GetChildBindingSource("ShipmentLines");
			new Shipments().PostShipment(m1BindingSource);
			m1BindingSource.SaveData();
		}
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		if (!selectedItems.Any())
		{
			return;
		}
		List<string> messages = arg.Messages;
		M1Database m1Database = (M1Database)ServiceProvider.GetService(typeof(M1Database));
		bool flag = (bool)m1Database.Props("IM")["xapIMAllowNegativeQtyOnHand"];
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		StringBuilder stringBuilder2 = new StringBuilder();
		List<string> list = new List<string>();
		foreach (ProcessSelectedItemValues item in selectedItems)
		{
			Shipments shipments = new Shipments();
			string text = item.KeyValues[0].ToString();
			using M1BindingSource m1BindingSource = new M1BindingSource(m1Database);
			m1BindingSource.DataSourceTable = "SHIPMENTS";
			m1BindingSource.NavigateTo(m1Database, "smpShipmentID = " + M1Util.ConvertToSql(text));
			bool flag2 = true;
			if (m1BindingSource == null)
			{
				continue;
			}
			if (m1Database.Props("GL").Field<bool>("xafGLCreateStockJournals") && !shipments.ShipmentPeriodCheck(m1BindingSource))
			{
				num++;
				flag2 = false;
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
			if (!string.IsNullOrWhiteSpace(shipments.PostShipmentCheck(m1BindingSource)))
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(text);
			}
			else
			{
				m1BindingSource.PrimaryTable.GetChildBindingSource("ShipmentLines");
				shipments.PostShipment(m1BindingSource);
				m1BindingSource.SaveData();
			}
			if (!flag)
			{
				continue;
			}
			bool num2 = shipments.ShipmentPostedCheck(m1BindingSource.Database, m1BindingSource.Transaction, m1BindingSource.CurrentAsDataRow.Field<string>("smpShipmentID"));
			bool flag3 = m1BindingSource.CurrentAsDataRow.Field<bool>("smpReversalEntry");
			if (!num2 && !flag3)
			{
				IDictionary<PartInformation, decimal> dictionaryPartQuantities = shipments.GetDictionaryPartQuantities(m1BindingSource);
				bool num3 = Shipments.VerifyQuantityAgainstInventory(m1BindingSource, dictionaryPartQuantities).Any();
				DataTable dataTable = m1BindingSource.PrimaryTable.GetChildBindingSource("ShipmentLines").GetDataTable();
				bool flag4 = m1BindingSource.Database.Props("GL").Field<bool>("xafGLCreateStockJournals") && shipments.VerifyIfNonStockedPartAndDeliveryType(m1BindingSource, dataTable);
				bool flag5 = DateTime.Compare(m1BindingSource.CurrentAsDataRow.Field<DateTime>("smpShipDate"), DateTime.Now) <= 0;
				bool flag6 = dictionaryPartQuantities.Any((KeyValuePair<PartInformation, decimal> keyValuePair) => keyValuePair.Key.IsBinInactive && keyValuePair.Key.HasNegativeQOH);
				if (num3 && !flag4 && flag5 && flag2 && !flag6)
				{
					list.Add(text);
					stringBuilder = stringBuilder.Replace((stringBuilder.ToString().Contains(",") ? "," : "") + text, "");
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
				arg.NegativeQtyOnHandMessages.Add("The following shipments contain delivery lines which will result in a negative quantity on hand.");
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
			messages.Add("The following shipments cannot be posted for one of the following reasons:");
		}
		if (stringBuilder.Length != 0)
		{
			messages.Add("- they are already posted");
			if (!flag)
			{
				messages.Add("- have insufficient quantity on hand");
			}
			messages.Add("- are non-stocked parts not from PTO/MTO deliveries");
			if (flag)
			{
				messages.Add("- there is a future dated shipment transaction that will result in negative quantity on hand");
				messages.Add("- posting the shipment will result in a negative quantity on hand for an inactive bin");
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
