namespace M1.Ax.Erp;

public class InspectionLineApprovalDefinition : ApprovalDefinition
{
	public InspectionLineApprovalDefinition()
	{
		ApprovalInstanceTable = "InspectionLineApprovals";
		ApprovalInstanceKeyFields = new string[3] { "qaaInspectionID", "qaaInspectionLineID", "qaaInspectionLineApprovalID" };
		ApprovalSourceTable = "EmployeeQAApprovals";
		ApprovalSourceEmployeeIDField = "lmbApprovalEmployeeID";
		ApprovalSourceKeyFields = new string[2] { "lmbEmployeeID", "lmbEmployeeQAApprovalID" };
		InstanceParentTable = "InspectionLines";
		InstanceStatusField = "qaaStatus";
		InstanceStatusDateField = "qaaStatusDate";
		InstanceDescriptionField = "qaaDescription";
		InstanceEmployeeIDField = "qaaApprovalEmployeeID";
		ParentTableKeys = new string[2] { "qalInspectionID", "qalInspectionLineID" };
		ParentStatusField = "qalApprovalStatus";
		ParentDecisionDateField = "qalApprovalDecisionDate";
		ParentApprovalRequestDateField = "qalApprovalRequestDate";
		ParentNextApprovalEmployeeIDField = "qalNextApprovalEmployeeID";
		ParentTotalField = "";
		SourceAmountField = "";
		ParentEmployeeIDField = "qalInspectorEmployeeID";
		ParentFormCollectionID = "Inspection";
		InstanceGridID = "M1INSPECTIONLINEAPPROVALSVIEW";
		HelpLink = "";
	}
}
