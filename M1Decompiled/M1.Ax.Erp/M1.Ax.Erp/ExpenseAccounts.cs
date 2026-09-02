using System.Runtime.InteropServices;

namespace M1.Ax.Erp;

[ComVisible(true)]
public class ExpenseAccounts
{
	public string ExpenseAccountID;

	public decimal Percent;

	public ExpenseAccounts(string expenseAccount, decimal percent)
	{
		ExpenseAccountID = expenseAccount;
		Percent = percent;
	}
}
