using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.DD.DBDefaults;

public class Workflows
{
	private int workFlowLineID;

	public Workflows(M1User m1User, ServerManager serverManager, string dataBase, string workFlowID, bool newList, string workFlowName)
	{
		string id = workFlowID.ToUpper();
		_ = string.Empty;
		int num = 0;
		CreateWorkFlowTable(serverManager, m1User, dataBase, newList, workFlowID, workFlowName);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		num = workFlowLineID;
		createDefaultImplementationChecklistAdd(id, workFlowLineID, 0, "-----General-----", 2m, "", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Users", 1m, "Call Forms.Show.UserAdministration", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		num = workFlowLineID;
		createDefaultImplementationChecklistAdd(id, workFlowLineID, 0, "-----Financials-----", 2m, "", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up GL Fiscal Years", 1m, "Forms.OpenObject \"GLFISCALYEAR\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up GL Divisions", 1m, "Forms.OpenObject \"GLDivision\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up GL Departments ", 1m, "Forms.OpenObject \"GLDepartment\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up GL Categories", 1m, "Forms.OpenObject \"GLCategory\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Chart Of Accounts", 1m, "Forms.OpenObject \"GLChart\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Currencies", 1m, "Forms.OpenObject \"Currency\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Part Groups", 1m, "Forms.OpenObject \"PartGroup\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Tax Codes", 1m, "Forms.OpenObject \"Tax\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Payment Terms ", 1m, "Forms.OpenObject \"PaymentTerm\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Aging Buckets", 1m, "Forms.OpenObject \"Aging\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Bank Accounts", 1m, "Forms.OpenObject \"BANKACCOUNT\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Entering Opening Balances", 1m, "Forms.OpenObject \"GLJOURNAL\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "First Bank Reconciliation", 1m, "Forms.OpenObject \"BANKREC\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Enter GL Budgets", 1m, "Forms.OpenObject \"GLFISCALYEAR\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Asset Type Maintenance", 1m, "Forms.OpenObject \"ASSETTYPE\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Fixed Asset Entry", 1m, "Forms.OpenObject \"ASSET\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		num = workFlowLineID;
		createDefaultImplementationChecklistAdd(id, workFlowLineID, 0, "-----Production-----", 2m, "", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Plants", 1m, "Forms.OpenObject \"Plant\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Production Departments", 1m, "Forms.OpenObject \"ProductionDepartment\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Processes", 1m, "Forms.OpenObject \"Process\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Work Centres", 1m, "Forms.OpenObject \"WorkCenter\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Production Calendars", 1m, "Forms.OpenObject \"PRODUCTIONCALENDAR\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		num = workFlowLineID;
		createDefaultImplementationChecklistAdd(id, workFlowLineID, 0, "-----Call/Lead Management-----", 2m, "", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Call Types", 1m, "Forms.OpenObject \"CallType\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Contact Methods", 1m, "Forms.OpenObject \"ContactMethod\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Priorities", 1m, "Forms.OpenObject \"Priority\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Service Contract Types", 1m, "Forms.OpenObject \"ServiceContractType\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Milestones", 1m, "Forms.OpenObject \"Milestone\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Marketing Programs", 1m, "Forms.OpenObject \"MarketingProgram\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		num = workFlowLineID;
		createDefaultImplementationChecklistAdd(id, workFlowLineID, 0, "-----HR Management-----", 2m, "", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Indirect Labour Codes", 1m, "Forms.OpenObject \"IndirectLabor\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Shifts", 1m, "Forms.OpenObject \"Shift\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Payroll Expense Codes", 1m, "Forms.OpenObject \"Expense\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Load Payroll Tax Tables", 1m, "Call Forms.Ax(\"PayrollFunctions\").ShowLoadTaxTablesForm(\"Import\")", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Payroll Income Taxes", 1m, "Forms.OpenObject \"IncomeTax\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Payroll Rates", 1m, "Forms.OpenObject \"PayrollRate\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Payroll Allowances", 1m, "Forms.OpenObject \"Allowance\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Payroll Deductions", 1m, "Forms.OpenObject \"Deduction\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Payroll Leave Accruals", 1m, "Forms.OpenObject \"LeaveAccrual\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Payroll Definitions ", 1m, "Forms.OpenObject \"PayrollDefinition\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Payroll Award Classifications", 1m, "Forms.OpenObject \"EmployeeAward\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Contact Titles", 1m, "Forms.OpenObject \"ContactTitle\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Attachment Types", 1m, "Forms.OpenObject \"AttachmentType\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Employees", 1m, "Forms.OpenObject \"Employee\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		num = workFlowLineID;
		createDefaultImplementationChecklistAdd(id, workFlowLineID, 0, "-----Contact Management-----", 2m, "", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Industry Types", 1m, "Forms.OpenObject \"IndustryType\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Contact Groups", 1m, "Forms.OpenObject \"ContactGroup\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Supplier Ratings", 1m, "Forms.OpenObject \"SupplierRating\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Shipping Payment Types", 1m, "Forms.OpenObject \"ShippingPaymentType\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Shipping Methods", 1m, "Forms.OpenObject \"ShippingMethod\"", m1User, serverManager, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Customer Groups", 1m, "Forms.OpenObject \"CustomerGroup\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Organizations", 1m, "Forms.OpenObject \"Organization\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		num = workFlowLineID;
		createDefaultImplementationChecklistAdd(id, workFlowLineID, 0, "-----Inventory Management-----", 2m, "", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Part Classes", 1m, "Forms.OpenObject \"PartClass\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Warehouses", 1m, "Forms.OpenObject \"Warehouse\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Cycle Codes", 1m, "Forms.OpenObject \"CycleCode\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Inventory Parts", 1m, "Forms.OpenObject \"Part\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Part Prices", 1m, "Forms.OpenObject \"PartPrice\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Service Contracts", 1m, "Forms.OpenObject \"ServiceContract\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Reasons", 1m, "Forms.OpenObject \"Reason\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up  Change Request Types", 1m, "Forms.OpenObject \"ChangeRequestType\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		num = workFlowLineID;
		createDefaultImplementationChecklistAdd(id, workFlowLineID, 0, "-----System Properties-----", 2m, "", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Database Options", 1m, "Call Forms.Show.DatabaseOptions(\"Dataset\")", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		num = workFlowLineID;
		createDefaultImplementationChecklistAdd(id, workFlowLineID, 0, "-----Open Data Load-----", 2m, "", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Recurring Payments", 1m, "Forms.OpenObject \"RecurringPayment\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Recurring Journals", 1m, "Forms.OpenObject \"RecurringJournal\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Set Up Recurring AR Invoices", 1m, "Forms.OpenObject \"ARRecurringInvoice\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "AR Open Invoice Load", 1m, "Forms.OpenObject \"AROpenInvoice\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "AP Open Invoice Load", 1m, "Forms.OpenObject \"APOpenInvoice\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Adjust Leave Hours", 1m, "Forms.OpenForm \"frmAdjustLeaveHours\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "Payroll Open Balance Load", 1m, "Forms.OpenObject \"PayrollOpenLoad\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "AP Unpresented Payment Load", 1m, "Forms.OpenObject \"APOpenPaymentSession\"", m1User, serverManager, dataBase);
		workFlowLineID = GetNextWorkFlowLineID_New(serverManager, m1User, dataBase);
		createDefaultImplementationChecklistAdd(id, workFlowLineID, num, "AR Outstanding Deposit Load", 1m, "Forms.OpenObject \"AROpenPaymentSession\"", m1User, serverManager, dataBase);
	}

	private int GetNextWorkFlowLineID_New(ServerManager sm, M1User m1User, string dataBase)
	{
		DataTable dataTable = sm.GetDataTable(null, m1User, dataBase, 0, "Select max(wflWorkFlowLineID) + 1 as maxID FROM WorkFlowLines", fillSchema: true, out var _);
		int result = 0;
		string empty = string.Empty;
		if (dataTable.Rows.Count > 0)
		{
			empty = dataTable.Rows[0]["maxID"].ToString();
			if (empty.Length == 0)
			{
				result = 1;
			}
			else if (empty.Length > 0)
			{
				result = Convert.ToInt32(dataTable.Rows[0]["maxID"].ToString());
			}
		}
		dataTable = null;
		return result;
	}

	private DataTable CreateWorkFlowTable(ServerManager serverManager, M1User m1User, string dataBase, bool newList, string workFlowID, string workFlowName)
	{
		DataTable dataTable = null;
		string empty = string.Empty;
		empty = ((!newList) ? "Select * From WorkFlows where 0 = 1" : ("Select * From WorkFlows Where wfpWorkFlowID = " + workFlowID.ToSql()));
		dataTable = serverManager.GetDataTable(null, m1User, dataBase, 0, empty, fillSchema: true, out var adapter);
		if (dataTable.Rows.Count == 0)
		{
			DataRow dataRow = dataTable.NewRow();
			dataRow.BeginEdit();
			dataRow.BlankRow();
			dataRow.SetField("wfpWorkFlowID", workFlowID);
			dataRow.SetField("wfpDescription", workFlowName);
			dataRow.SetField("wfpCreatedBy", m1User.ID.Trim());
			dataRow.SetField("wfpCreatedDate", DateTime.Now);
			dataRow.EndEdit();
			dataTable.Rows.Add(dataRow);
			serverManager.UpdateData(null, m1User, dataBase, new DataRow[1] { dataRow }, adapter);
		}
		return dataTable;
	}

	public bool createDefaultImplementationChecklistAdd(string id, int line, int parent, string desc, decimal type, string code, M1User m1User, ServerManager serverManager, string dataBase)
	{
		SqlDataAdapter adapter;
		DataTable dataTable = serverManager.GetDataTable(null, m1User, dataBase, 0, "Select * From WorkFlowLines Where wflWorkFlowID = " + id.ToSql() + " And wflWorkFlowLineID = " + line.ToSql(), fillSchema: true, out adapter);
		if (dataTable.Rows.Count == 0)
		{
			DataRow dataRow = dataTable.NewRow();
			dataRow.BeginEdit();
			dataRow.BlankRow();
			dataRow.SetField("wflWorkFlowID", id);
			dataRow.SetField("wflWorkFlowLineID", line);
			dataRow.SetField("wflDescription", desc);
			dataRow.SetField("wflType", type);
			dataRow.SetField("wflParentID", (decimal)parent);
			dataRow.SetField("wflCode", code);
			dataRow.SetField("wflCreatedBy", m1User.ID.Trim());
			if (parent != 0)
			{
				dataRow.SetField("wflStartDate", DateTime.Now);
				dataRow.SetField("wflDueDate", DateTime.Now);
			}
			dataRow.SetField("wflCreatedDate", DateTime.Now);
			dataRow.EndEdit();
			dataTable.Rows.Add(dataRow);
			serverManager.UpdateData(null, m1User, dataBase, new DataRow[1] { dataRow }, adapter);
		}
		return true;
	}

	private M1Database getM1DatabaseReference(M1User m1User, ServerManager serverManager, string database)
	{
		M1Database m1Database = new M1Database(m1User, serverManager);
		LoginCredentials loginCredentials = new LoginCredentials(m1User.ID, "notused");
		try
		{
			m1Database.Login(database, m1User, loginCredentials, readOnlyLogin: false);
		}
		catch
		{
		}
		return m1Database;
	}

	private int getNextWorkFlowLineID(M1Database m1Database)
	{
		int num = 0;
		if (!m1Database.IsOpen)
		{
			return workFlowLineID + 1;
		}
		return Convert.ToInt32(m1Database.NextIDs.GetNextIDForTable("WorkFlowLines"));
	}

	public bool createImplementationCheckList(M1User m1User, ServerManager serverManager, string dataBase, M1DataDictionary m1DataDictionary)
	{
		string text = "ImplementationCheckList";
		if (m1DataDictionary.GetDataTable("select * from ddtables where dtTable = " + text.Trim().ToUpper().ToSql()).Rows.Count > 0)
		{
			EnsureCreationOfImpCheckListTable(m1User, serverManager, dataBase, m1DataDictionary);
			HandlePopulationOfImpCheckList(m1User, serverManager, dataBase);
			return true;
		}
		throw new M1Exception("No Data Dictionary Record for " + text);
	}

	public void EnsureCreationOfImpCheckListTable(M1User m1User, ServerManager serverManager, string dataBase, M1DataDictionary m1DataDictionary)
	{
		Dmo dmo = new Dmo(m1User.Context, serverManager);
		if (!dmo.DoesTableExist(null, m1User, dataBase, "ImplementationCheckList"))
		{
			dmo.CreateTable(null, m1User, m1DataDictionary, dataBase, "ImplementationCheckList");
		}
	}

	public void HandlePopulationOfImpCheckList(M1User m1User, ServerManager serverManager, string dataBase)
	{
		ImpCheckListDetails impCheckListDetails = new ImpCheckListDetails();
		InitializeImpCheckListDetailsObject(impCheckListDetails);
		impCheckListDetails.Name = "-----General-----";
		impCheckListDetails.ChildNodes = new List<ImpCheckListDetailsChild>();
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Users", "Call Forms.Show.UserAdministration");
		createImplementationChecklistGroup(impCheckListDetails, m1User, serverManager, dataBase);
		impCheckListDetails = new ImpCheckListDetails();
		InitializeImpCheckListDetailsObject(impCheckListDetails);
		impCheckListDetails.Name = "-----Financials-----";
		impCheckListDetails.ChildNodes = new List<ImpCheckListDetailsChild>();
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up GL Fiscal Years", "Forms.OpenObject \"GLFISCALYEAR\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up GL Divisions", "Forms.OpenObject \"GLDivision\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up GL Departments ", "Forms.OpenObject \"GLDepartment\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up GL Categories", "Forms.OpenObject \"GLCategory\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Chart Of Accounts", "Forms.OpenObject \"GLChart\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Currencies", "Forms.OpenObject \"Currency\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Part Groups", "Forms.OpenObject \"PartGroup\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Tax Codes", "Forms.OpenObject \"Tax\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Payment Terms ", "Forms.OpenObject \"PaymentTerm\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Aging Buckets", "Forms.OpenObject \"Aging\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Bank Accounts", "Forms.OpenObject \"BANKACCOUNT\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Entering Opening Balances", "Forms.OpenObject \"GLJOURNAL\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "First Bank Reconciliation", "Forms.OpenObject \"BANKREC\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Enter GL Budgets", "Forms.OpenObject \"GLFISCALYEAR\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Asset Type Maintenance", "Forms.OpenObject \"ASSETTYPE\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Fixed Asset Entry", "Forms.OpenObject \"ASSET\"");
		createImplementationChecklistGroup(impCheckListDetails, m1User, serverManager, dataBase);
		impCheckListDetails = new ImpCheckListDetails();
		InitializeImpCheckListDetailsObject(impCheckListDetails);
		impCheckListDetails.Name = "-----Production-----";
		impCheckListDetails.ChildNodes = new List<ImpCheckListDetailsChild>();
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Plants", "Forms.OpenObject \"Plant\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Production Departments", "Forms.OpenObject \"ProductionDepartment\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Processes", "Forms.OpenObject \"Process\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Work Centres", "Forms.OpenObject \"WorkCenter\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Production Calendars", "Forms.OpenObject \"PRODUCTIONCALENDAR\"");
		createImplementationChecklistGroup(impCheckListDetails, m1User, serverManager, dataBase);
		impCheckListDetails = new ImpCheckListDetails();
		InitializeImpCheckListDetailsObject(impCheckListDetails);
		impCheckListDetails.Name = "-----Call/Lead Management-----";
		impCheckListDetails.ChildNodes = new List<ImpCheckListDetailsChild>();
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Call Types", "Forms.OpenObject \"CallType\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Contact Methods", "Forms.OpenObject \"ContactMethod\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Priorities", "Forms.OpenObject \"Priority\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Service Contract Types", "Forms.OpenObject \"ServiceContractType\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Milestones", "Forms.OpenObject \"Milestone\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Marketing Programs", "Forms.OpenObject \"MarketingProgram\"");
		createImplementationChecklistGroup(impCheckListDetails, m1User, serverManager, dataBase);
		impCheckListDetails = new ImpCheckListDetails();
		InitializeImpCheckListDetailsObject(impCheckListDetails);
		impCheckListDetails.Name = "-----HR Management-----";
		impCheckListDetails.ChildNodes = new List<ImpCheckListDetailsChild>();
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Indirect Labour Codes", "Forms.OpenObject \"IndirectLabor\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Shifts", "Forms.OpenObject \"Shift\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Payroll Expense Codes", "Forms.OpenObject \"Expense\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Load Payroll Tax Tables", "Call Forms.PayrollFunctions.ShowLoadTaxTablesForm(\"Import\")");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Payroll Income Taxes", "Forms.OpenObject \"IncomeTax\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Payroll Rates", "Forms.OpenObject \"PayrollRate\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Payroll Allowances", "Forms.OpenObject \"Allowance\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Payroll Deductions", "Forms.OpenObject \"Deduction\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Payroll Leave Accruals", "Forms.OpenObject \"LeaveAccrual\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Payroll Definitions ", "Forms.OpenObject \"PayrollDefinition\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Payroll Award Classifications", "Forms.OpenObject \"EmployeeAward\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Contact Titles", "Forms.OpenObject \"ContactTitle\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Attachment Types", "Forms.OpenObject \"AttachmentType\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Employees", "Forms.OpenObject \"Employee\"");
		createImplementationChecklistGroup(impCheckListDetails, m1User, serverManager, dataBase);
		impCheckListDetails = new ImpCheckListDetails();
		InitializeImpCheckListDetailsObject(impCheckListDetails);
		impCheckListDetails.Name = "-----Contact Management-----";
		impCheckListDetails.ChildNodes = new List<ImpCheckListDetailsChild>();
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Industry Types", "Forms.OpenObject \"IndustryType\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Contact Groups", "Forms.OpenObject \"ContactGroup\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Supplier Ratings", "Forms.OpenObject \"SupplierRating\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Shipping Payment Types", "Forms.OpenObject \"ShippingPaymentType\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Shipping Methods", "Forms.OpenObject \"ShippingMethod\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Customer Groups", "Forms.OpenObject \"CustomerGroup\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Organizations", "Forms.OpenObject \"Organization\"");
		createImplementationChecklistGroup(impCheckListDetails, m1User, serverManager, dataBase);
		impCheckListDetails = new ImpCheckListDetails();
		InitializeImpCheckListDetailsObject(impCheckListDetails);
		impCheckListDetails.Name = "-----Inventory Management-----";
		impCheckListDetails.ChildNodes = new List<ImpCheckListDetailsChild>();
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Part Classes", "Forms.OpenObject \"PartClass\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Warehouses", "Forms.OpenObject \"Warehouse\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Cycle Codes", "Forms.OpenObject \"CycleCode\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Inventory Parts", "Forms.OpenObject \"Part\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Part Prices", "Forms.OpenObject \"PartPrice\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Service Contracts", "Forms.OpenObject \"ServiceContract\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Reasons", "Forms.OpenObject \"Reason\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up  Change Request Types", "Forms.OpenObject \"ChangeRequestType\"");
		createImplementationChecklistGroup(impCheckListDetails, m1User, serverManager, dataBase);
		impCheckListDetails = new ImpCheckListDetails();
		InitializeImpCheckListDetailsObject(impCheckListDetails);
		impCheckListDetails.Name = "-----System Properties-----";
		impCheckListDetails.ChildNodes = new List<ImpCheckListDetailsChild>();
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Database Options", "Call Forms.PropsShowDataset");
		createImplementationChecklistGroup(impCheckListDetails, m1User, serverManager, dataBase);
		impCheckListDetails = new ImpCheckListDetails();
		InitializeImpCheckListDetailsObject(impCheckListDetails);
		impCheckListDetails.Name = "-----Open Data Load-----";
		impCheckListDetails.ChildNodes = new List<ImpCheckListDetailsChild>();
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Recurring Payments", "Forms.OpenObject \"RecurringPayment\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Recurring Journals", "Forms.OpenObject \"RecurringJournal\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Set Up Recurring AR Invoices", "Forms.OpenObject \"ARRecurringInvoice\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "AR Open Invoice Load", "Forms.OpenObject \"AROpenInvoice\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "AP Open Invoice Load", "Forms.OpenObject \"APOpenInvoice\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Adjust Leave Hours", "Forms.OpenForm \"frmAdjustLeaveHours\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "Payroll Open Balance Load", "Forms.OpenObject \"PayrollOpenLoad\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "AP Unpresented Payment Load", "Forms.OpenObject \"APOpenPaymentSession\"");
		CreateChildren_ForImpCheckList(impCheckListDetails.ChildNodes, "AR Outstanding Deposit Load", "Forms.OpenObject \"AROpenPaymentSession\"");
		createImplementationChecklistGroup(impCheckListDetails, m1User, serverManager, dataBase);
	}

	private void CreateChildren_ForImpCheckList(List<ImpCheckListDetailsChild> childNodes, string name, string code)
	{
		ImpCheckListDetailsChild impCheckListDetailsChild = new ImpCheckListDetailsChild();
		InitializeImpCheckListDetailsObject(impCheckListDetailsChild);
		impCheckListDetailsChild.Name = name;
		impCheckListDetailsChild.Code = code;
		childNodes.Add(impCheckListDetailsChild);
	}

	private void InitializeImpCheckListDetailsObject(IImpCheckList iImpCheckList)
	{
		int num = 0;
		string empty = string.Empty;
		DateTime now = DateTime.Now;
		iImpCheckList.ID = num;
		iImpCheckList.Name = empty;
		iImpCheckList.Code = empty;
		iImpCheckList.PercentDone = num;
		iImpCheckList.AssignedTo = empty;
		iImpCheckList.CreatedBy = empty;
		iImpCheckList.ParentID = num;
		iImpCheckList.CreatedDate = now;
	}

	private int GetNextImpCheckListID(ServerManager sm, M1User m1User, string dataBase)
	{
		DataTable dataTable = sm.GetDataTable(null, m1User, dataBase, 0, "Select max(xicImplementationCheckListID) + 1 as maxID FROM ImplementationCheckList", fillSchema: true, out var _);
		int result = 0;
		string empty = string.Empty;
		if (dataTable.Rows.Count > 0)
		{
			empty = dataTable.Rows[0]["maxID"].ToString();
			if (empty.Length == 0)
			{
				result = 1;
			}
			else if (empty.Length > 0)
			{
				result = Convert.ToInt32(dataTable.Rows[0]["maxID"].ToString());
			}
		}
		dataTable = null;
		return result;
	}

	private void SetImpCheckListDetailsObject(IImpCheckList iImpCheckList, ServerManager serverManager, M1User m1User, string database)
	{
		iImpCheckList.ID = GetNextImpCheckListID(serverManager, m1User, database);
		iImpCheckList.CreatedDate = DateTime.UtcNow.ToLocalTime();
		iImpCheckList.CreatedBy = m1User.ID;
	}

	public bool createImplementationChecklistGroup(ImpCheckListDetails impCheckListDetails, M1User m1User, ServerManager serverManager, string dataBase)
	{
		if (impCheckListDetails != null)
		{
			SetImpCheckListDetailsObject(impCheckListDetails, serverManager, m1User, dataBase);
			InsertRowIntoImpCheckListTable(impCheckListDetails, m1User, serverManager, dataBase);
			if (impCheckListDetails.ChildNodes != null && impCheckListDetails.ChildNodes.Count() > 0)
			{
				foreach (ImpCheckListDetailsChild childNode in impCheckListDetails.ChildNodes)
				{
					childNode.ParentID = impCheckListDetails.ID;
					SetImpCheckListDetailsObject(childNode, serverManager, m1User, dataBase);
					InsertRowIntoImpCheckListTable(childNode, m1User, serverManager, dataBase);
				}
			}
			return true;
		}
		return true;
	}

	public void InsertRowIntoImpCheckListTable(IImpCheckList impCheckList, M1User m1User, ServerManager serverManager, string dataBase)
	{
		SqlDataAdapter adapter;
		DataTable dataTable = serverManager.GetDataTable(null, m1User, dataBase, 0, "Select * From ImplementationCheckList Where 0 = 1", fillSchema: true, out adapter);
		if (dataTable.Rows.Count == 0)
		{
			DataRow dataRow = dataTable.NewRow();
			dataRow.BeginEdit();
			dataRow.BlankRow();
			dataRow.SetField("xicImplementationCheckListID", impCheckList.ID);
			dataRow.SetField("xicTask", impCheckList.Name);
			dataRow.SetField("xicPercentDone", (decimal)impCheckList.PercentDone);
			dataRow.SetField("xicAssignedTo", impCheckList.AssignedTo);
			dataRow.SetField("xicAction", impCheckList.Code);
			dataRow.SetField("xicCreatedBy", impCheckList.CreatedBy);
			dataRow.SetField("xicCreatedDate", impCheckList.CreatedDate);
			dataRow.SetField("xicParentID", impCheckList.ParentID);
			dataRow.EndEdit();
			dataTable.Rows.Add(dataRow);
			serverManager.UpdateData(null, m1User, dataBase, new DataRow[1] { dataRow }, adapter);
		}
	}
}
