using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("Approvals")]
[ComVisible(true)]
public class AppAxApprovals : IDisposable
{
	private IServiceProvider provider;

	private M1Database database;

	public AppAxApprovals(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		database = parentProvider.GetService(typeof(M1Database)) as M1Database;
	}

	public bool IsApprovalRequired(string table, object bindingSource)
	{
		return ApprovalWorkflow.IsApprovalRequired(ApprovalWorkflow.GetApprovalDefinition(table), (M1BindingSource)bindingSource);
	}

	public void CheckApprovals(string table, object bindingSource)
	{
		ApprovalWorkflow.CheckApprovals(ApprovalWorkflow.GetApprovalDefinition(table), (M1BindingSource)bindingSource);
	}

	public void TransferApprovals(string table, object bindingSource)
	{
		ApprovalWorkflow.TransferApprovals(ApprovalWorkflow.GetApprovalDefinition(table), (M1BindingSource)bindingSource);
	}

	public void TransferApprovalsDirect(DataRow row, string table, object transaction)
	{
		ApprovalWorkflow.TransferApprovalsDirect(database, ApprovalWorkflow.GetApprovalDefinition(table), row, (SqlTransaction)comNullCheck(transaction));
	}

	private object comNullCheck(object value)
	{
		if (value != DBNull.Value && value != null)
		{
			return value;
		}
		return null;
	}

	public void Dispose()
	{
		database = null;
		provider = null;
	}
}
