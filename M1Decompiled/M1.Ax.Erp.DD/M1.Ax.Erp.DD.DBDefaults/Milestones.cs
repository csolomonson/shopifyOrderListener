using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.DD.DBDefaults;

[DBCreateDefault("Create default Milestones")]
public class Milestones
{
	public Milestones(DBCreateDefaultParms parm)
	{
		createDefaultMilestonesAdd("UL", "Unqualified Lead", 0.0, "Lead has been entered in the system but no qualification has occurred.", parm);
		createDefaultMilestonesAdd("QL", "Qualified Lead", 0.1, "The lead has been qualified and is a valid sales opportunity.", parm);
		createDefaultMilestonesAdd("QO", "Qualified Opportunity", 0.2, "Budget and decision to buy has been determined and confirmed.", parm);
		createDefaultMilestonesAdd("PAT", "Plan Agreed To", 0.25, "Prospect has bought into the sales process plan.", parm);
		createDefaultMilestonesAdd("EP", "Evaluating Product", 0.3, "Product is being evaluated along with other products.", parm);
		createDefaultMilestonesAdd("SL", "Short Listed", 0.4, "Product and company has been selected as part of a short list for final evaluation.", parm);
		createDefaultMilestonesAdd("RS", "Recommended Supplier", 0.65, "Recommended supplier from short list process. Next stage is final proposal configuration and acceptance.", parm);
		createDefaultMilestonesAdd("WD", "Waiting for Decision", 0.75, "Proposed solution with quotation has been submitted to prospect for final decision.", parm);
		createDefaultMilestonesAdd("WS", "Waiting for Signatures", 0.85, "Proposed solution has been accepted and final management signoff is required.", parm);
		createDefaultMilestonesAdd("WIN", "Win", 1.0, "The opportunity is now a sale.", parm);
	}

	private void createDefaultMilestonesAdd(string id, string desc, double confidence, string extra, DBCreateDefaultParms parm)
	{
		SqlDataAdapter adapter;
		DataTable dataTable = parm.ServerManager.GetDataTable(null, parm.User, parm.DatabaseName, 0, "Select * From Milestones", fillSchema: true, out adapter);
		DataRow dataRow = dataTable.NewRow();
		dataRow.BeginEdit();
		dataRow.BlankRow();
		dataRow.SetField("losMilestoneID", id);
		dataRow.SetField("losShortDescription", desc);
		dataRow.SetField("losLongDescriptionText", extra);
		dataRow.SetField("losLongDescriptionRTF", "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033\r\\viewkind4\\uc1\\pard\\f0\\fs20 " + extra.Replace("\r", "\r\\par ") + "\r\\par }");
		dataRow.SetField("losConfidenceFactor", confidence);
		dataRow.SetField("losCreatedDate", DateTime.Now);
		dataRow.SetField("losCreatedBy", parm.User.ID);
		dataRow.EndEdit();
		dataTable.Rows.Add(dataRow);
		parm.ServerManager.UpdateData(null, parm.User, parm.DatabaseName, new DataRow[1] { dataRow }, adapter);
	}
}
