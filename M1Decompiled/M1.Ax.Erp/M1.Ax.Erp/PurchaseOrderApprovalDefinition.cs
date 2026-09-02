namespace M1.Ax.Erp;

public class PurchaseOrderApprovalDefinition : ApprovalDefinition
{
	public PurchaseOrderApprovalDefinition()
	{
		ApprovalInstanceTable = "PurchaseOrderApprovals";
		ApprovalInstanceKeyFields = new string[2] { "pmaPurchaseOrderID", "pmaPurchaseOrderApprovalID" };
		ApprovalSourceTable = "EmployeePOApprovals";
		ApprovalSourceEmployeeIDField = "lmhApprovalEmployeeID";
		ApprovalSourceKeyFields = new string[2] { "lmhEmployeeID", "lmhEmployeePOApprovalID" };
		InstanceParentTable = "PurchaseOrders";
		InstanceStatusField = "pmaStatus";
		InstanceStatusDateField = "pmaStatusDate";
		InstanceDescriptionField = "pmaDescription";
		InstanceEmployeeIDField = "pmaApprovalEmployeeID";
		ParentTableKeys = new string[1] { "pmpPurchaseOrderID" };
		ParentStatusField = "pmpStatus";
		ParentReadyToPrintField = "pmpReadyToPrint";
		ParentDecisionDateField = "pmpApprovalDecisionDate";
		ParentApprovalRequestDateField = "pmpApprovalRequestDate";
		ParentNextApprovalEmployeeIDField = "pmpNextApprovalEmployeeID";
		ParentTotalField = "pmpOrderTotalBase";
		SourceAmountField = "lmePOApprovalAmount";
		ParentEmployeeIDField = "pmpBuyerEmployeeID";
		ParentFormCollectionID = "PO";
		InstanceGridID = "M1PURCHASEORDERAPPROVALSVIEW";
		HelpLink = "PM_POApproval.htm";
	}
}
