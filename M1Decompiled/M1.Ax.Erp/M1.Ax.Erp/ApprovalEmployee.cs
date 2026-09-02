namespace M1.Ax.Erp;

public class ApprovalEmployee
{
	public string EmployeeID;

	public byte Status;

	public decimal Amount;

	public ApprovalEmployee(string employeeID, byte status, decimal amount)
	{
		EmployeeID = employeeID;
		Status = status;
		Amount = amount;
	}
}
