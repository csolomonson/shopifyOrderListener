using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class SalesPeople
{
	public bool AddEmployee(M1BindingSource bindingSource, string employeeID, decimal percent)
	{
		if (bindingSource != null && !string.IsNullOrWhiteSpace(employeeID))
		{
			string text = bindingSource.PrimaryTable?.FieldPrefix;
			if (!string.IsNullOrWhiteSpace(text))
			{
				SqlCommand sqlCommand = bindingSource.Database.NewSqlCommand("select TOP 1 lmeEmployeeID,lmeSOApprovalAmount from Employees where lmeEmployeeID = @UserID and lmeTerminationDate IS NULL and lmeSalesEmployee = 1");
				sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = employeeID;
				if (bindingSource.Database.GetDataTable(sqlCommand).Rows.Count != 0 && bindingSource.GetDataView().ToTable().Select(text + "SalesEmployeeID = " + employeeID.ToLinq())
					.Length.Equals(0))
				{
					DataRow obj = (DataRow)bindingSource.AddNew();
					obj[text + "SalesEmployeeID"] = employeeID;
					obj[text + "Percent"] = percent;
					return true;
				}
			}
		}
		return false;
	}

	public void ClearEmployees(M1BindingSource bindingSource, DataRow parentRow)
	{
		if (bindingSource.Count != 0)
		{
			bindingSource.RemoveWhere(string.Empty, parentRow);
		}
	}
}
