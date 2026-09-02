using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Core;

namespace M1.Ax.Erp;

public class InspectionQueueProcess : ProcessParameters
{
	public InspectionQueueProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		MultipleDestinationRowsCreated = true;
		ShowRefresh = true;
		M1Database m1Database = ServiceProvider.GetService(typeof(M1Database)) as M1Database;
		if (m1Database?.Props("PN") != null && !Convert.ToInt16(m1Database.Props("PN")["xapQAInspQueueRefreshInterval"]).Equals(0))
		{
			AutoRefreshInterval = Convert.ToInt16(m1Database.Props("PN")["xapQAInspQueueRefreshInterval"]);
		}
		m1Database = null;
		KeyValueFieldNames = new string[2] { "qalInspectionID", "qalInspectionLineID" };
		KeyValueTableName = "InspectionLines";
		Description = "Use this screen to move inspection items from pending to open.";
		GridID = "M1ADDFROMINSPECTIONQUEUE";
		BindingSourceTable = "InspectionLines";
		HelpLink = "QM_InspQueue.htm";
		ContinueMessage = "This will change the status of the {0} selected inspections to open and remove them from the inspection queue. Selected inspections without an inspector will have their inspector set by the system if possible (to the current user if they are an inspector). Do you wish to continue?";
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Inspector", null, new string[1] { "qalInspectorEmployeeID" })
		{
			ValueFields = new string[1] { "qalInspectorEmployeeID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Part Class", null, new string[1] { "impPartClassID" })
		{
			ValueFields = new string[1] { "impPartClassID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("First off Inspections only?")
		{
			AdoFilterExpression = "qalFirstOffInspection <> 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "qalFirstOffInspection"
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Purchase Order ID", null, new string[1] { "rmlPurchaseOrderID" })
		{
			ValueFields = new string[1] { "rmlPurchaseOrderID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Receipt ID", null, new string[1] { "rmlReceiptID" })
		{
			ValueFields = new string[1] { "rmlReceiptID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Mfg Receipt ID", null, new string[1] { "rmmMfgReceiptID" })
		{
			ValueFields = new string[1] { "rmmMfgReceiptID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("RMA Claim ID", null, new string[1] { "rrlRmaClaimID" })
		{
			ValueFields = new string[1] { "rrlRmaClaimID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("RMA Receipt ID", null, new string[1] { "rrlRmaReceiptID" })
		{
			ValueFields = new string[1] { "rrlRmaReceiptID" }
		});
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		_ = BindingSource.CurrentAsDataRow;
		M1Database database = BindingSource.Database;
		StringBuilder stringBuilder = new StringBuilder();
		SqlDataAdapter adapter;
		DataTable dataTable = database.GetDataTable("Select * From InspectionLines Where " + text, fillSchema: true, out adapter);
		M1BindingSource m1BindingSource = new M1BindingSource(database);
		m1BindingSource.LoadDefinition(string.Empty, "InspectionLines", dataTable, true, loadDataNow: true);
		m1BindingSource.Query.DataAdapter = adapter;
		if (m1BindingSource.Count != 0)
		{
			foreach (DataRow row in m1BindingSource.GetDataTable().Rows)
			{
				if (row == null)
				{
					continue;
				}
				string text2 = "";
				ProcessSelectedItemValues itemValuesFromList = GetItemValuesFromList(selectedItems, row);
				if (itemValuesFromList.EditableValues.ContainsKey("qalInspectorEmployeeID"))
				{
					Inspection inspection = new Inspection();
					text2 = Convert.ToString(itemValuesFromList.EditableValues["qalInspectorEmployeeID"]);
					if (!string.IsNullOrWhiteSpace(text2))
					{
						if (!inspection.InspectorApprovedCheck(database, null, text2, inspComplete: false))
						{
							row.SetField("qalStatus", "O");
							row.SetField("qalInspectorEmployeeID", text2);
							row.SetField("qalInspectionDate", DateTime.Today);
						}
					}
					else
					{
						AppAxProduction appAxProduction = new AppAxProduction(database);
						if (!string.IsNullOrWhiteSpace(appAxProduction.InspectorID) && !inspection.InspectorApprovedCheck(database, null, text2, inspComplete: false))
						{
							row.SetField("qalStatus", "O");
							row.SetField("qalInspectorEmployeeID", appAxProduction.InspectorID);
							row.SetField("qalInspectionDate", DateTime.Today);
						}
					}
				}
				if (!string.IsNullOrWhiteSpace(row.Field<string>("qalInspectorEmployeeID")))
				{
					if (!string.IsNullOrWhiteSpace(row.Field<string>("qalInspectionID")))
					{
						List<object[]> keysCreated = arg.KeysCreated;
						object[] item = new string[1] { row.Field<string>("qalInspectionID") };
						keysCreated.Add(item);
					}
				}
				else
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.AppendLine(row.Field<string>("qalInspectionID"));
				}
			}
			m1BindingSource.SaveData();
			arg.OpenKeysWithObjectID = "Inspection";
		}
		if (stringBuilder.Length != 0)
		{
			messages.Add("The following inspections were not processed due to the inspector's quality approval status:\r\n");
			messages.Add(stringBuilder.ToString());
		}
	}
}
