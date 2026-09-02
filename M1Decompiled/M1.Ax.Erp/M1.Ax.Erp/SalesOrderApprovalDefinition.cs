namespace M1.Ax.Erp;

public class SalesOrderApprovalDefinition : ApprovalDefinition
{
	public SalesOrderApprovalDefinition()
	{
		ApprovalInstanceTable = "SalesOrderApprovals";
		ApprovalInstanceKeyFields = new string[2] { "omaSalesOrderID", "omaSalesOrderApprovalID" };
		ApprovalSourceTable = "EmployeeSOApprovals";
		ApprovalSourceEmployeeIDField = "lmoApprovalEmployeeID";
		ApprovalSourceKeyFields = new string[2] { "lmoEmployeeID", "lmoEmployeeSOApprovalID" };
		InstanceParentTable = "SalesOrders";
		InstanceStatusField = "omaStatus";
		InstanceStatusDateField = "omaStatusDate";
		InstanceDescriptionField = "omaDescription";
		InstanceEmployeeIDField = "omaApprovalEmployeeID";
		ParentTableKeys = new string[1] { "ompSalesOrderID" };
		ParentStatusField = "ompStatus";
		ParentReadyToPrintField = "ompReadyToPrint";
		ParentDecisionDateField = "ompApprovalDecisionDate";
		ParentApprovalRequestDateField = "ompApprovalRequestDate";
		ParentNextApprovalEmployeeIDField = "ompNextApprovalEmployeeID";
		ParentTotalField = "ompOrderTotalBase";
		SourceAmountField = "lmeSOApprovalAmount";
		ParentEmployeeChildTable = "SalesOrderSalespeople";
		ParentEmployeeIDField = "omiSalesEmployeeID";
		ParentFormCollectionID = "SalesOrder";
		InstanceGridID = "M1SALESORDERAPPROVALSVIEW";
		HelpLink = "";
	}
}
