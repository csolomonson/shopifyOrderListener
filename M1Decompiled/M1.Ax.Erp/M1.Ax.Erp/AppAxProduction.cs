using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("Production")]
[ComVisible(true)]
public class AppAxProduction : IDisposable
{
	private IServiceProvider provider;

	private M1Database database;

	private M1User user;

	public byte BuyQuantityDecimals;

	public byte SellQuantityDecimals;

	public byte InventoryQuantityDecimals;

	private string _EmployeeID;

	private string _PlannerID;

	private double _BuyerAmount;

	private string _BuyerID;

	private string _InspectorID;

	private string _EngineerID;

	private string _SalesPersonID;

	private double _SalesPersonAmount;

	private string _PlantID;

	private string _PlantDepartmentID;

	private bool? _AvalaraActivated;

	private List<string> _EmployeeRoles;

	public bool AvalaraActivated
	{
		get
		{
			if (!_AvalaraActivated.HasValue)
			{
				_AvalaraActivated = IsAvalaraActivated(database);
			}
			return _AvalaraActivated.Value;
		}
	}

	public string EmployeeID
	{
		get
		{
			if (_EmployeeID == null)
			{
				Employee employee = new Employee();
				_EmployeeID = employee.GetEmployeeIDforUserId(database, user.ID);
			}
			return _EmployeeID;
		}
	}

	public string PlannerID
	{
		get
		{
			if (_PlannerID == null)
			{
				SqlCommand sqlCommand = database.NewSqlCommand("select TOP 1 lmeEmployeeID from Employees where lmePlannerEmployee = 1 and lmeUserID = @UserID and lmeTerminationDate IS NULL order by lmeEmployeeID");
				sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = user.ID;
				string text = (string)database.ExecuteScalar(sqlCommand);
				if (text != null)
				{
					_PlannerID = text.Trim();
				}
				else
				{
					_PlannerID = string.Empty;
				}
			}
			return _PlannerID;
		}
	}

	public string BuyerID
	{
		get
		{
			if (_BuyerID == null)
			{
				SqlCommand sqlCommand = database.NewSqlCommand("select TOP 1 lmeEmployeeID,lmePOApprovalAmount from Employees where lmeBuyerEmployee = 1 and lmeUserID = @UserID and lmeTerminationDate IS NULL order by lmeEmployeeID");
				sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = user.ID;
				DataTable dataTable = database.GetDataTable(sqlCommand);
				if (dataTable.Rows.Count != 0)
				{
					_BuyerID = dataTable.Rows[0].Field<string>("lmeEmployeeID").Trim();
					_BuyerAmount = (double)dataTable.Rows[0].Field<decimal>("lmePOApprovalAmount");
				}
				else
				{
					_BuyerID = string.Empty;
					_BuyerAmount = 0.0;
				}
			}
			return _BuyerID;
		}
	}

	public double BuyerAmount
	{
		get
		{
			if (_BuyerID == null)
			{
				_ = BuyerID;
			}
			return _BuyerAmount;
		}
	}

	public string InspectorID
	{
		get
		{
			if (_InspectorID == null)
			{
				SqlCommand sqlCommand = database.NewSqlCommand("select TOP 1 lmeEmployeeID from Employees where lmeInspectorEmployee = 1 and lmeUserID = @UserID and lmeTerminationDate IS NULL order by lmeEmployeeID");
				sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = user.ID;
				DataTable dataTable = database.GetDataTable(sqlCommand);
				if (dataTable.Rows.Count != 0)
				{
					_InspectorID = dataTable.Rows[0].Field<string>("lmeEmployeeID").Trim();
				}
				else
				{
					_InspectorID = string.Empty;
				}
			}
			return _InspectorID;
		}
	}

	public string EngineerID
	{
		get
		{
			if (_EngineerID == null)
			{
				SqlCommand sqlCommand = database.NewSqlCommand("select TOP 1 lmeEmployeeID from Employees where lmeEngineerEmployee = 1 and lmeUserID = @UserID and lmeTerminationDate IS NULL and lmeSalesEmployee = 1 order by lmeEmployeeID");
				sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = user.ID;
				string text = (string)database.ExecuteScalar(sqlCommand);
				if (text != null)
				{
					_EngineerID = text.Trim();
				}
				else
				{
					_EngineerID = string.Empty;
				}
			}
			return _EngineerID;
		}
	}

	public string SalesPersonID
	{
		get
		{
			if (_SalesPersonID == null)
			{
				SqlCommand sqlCommand = database.NewSqlCommand("select TOP 1 lmeEmployeeID,lmeSOApprovalAmount from Employees where lmeUserID = @UserID and lmeTerminationDate IS NULL and lmeSalesEmployee = 1 order by lmeEmployeeID");
				sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = user.ID;
				DataTable dataTable = database.GetDataTable(sqlCommand);
				if (dataTable.Rows.Count != 0)
				{
					_SalesPersonID = dataTable.Rows[0].Field<string>("lmeEmployeeID").Trim();
					_SalesPersonAmount = (double)dataTable.Rows[0].Field<decimal>("lmeSOApprovalAmount");
				}
				else
				{
					_SalesPersonID = string.Empty;
					_SalesPersonAmount = 0.0;
				}
			}
			return _SalesPersonID;
		}
	}

	public double SalesPersonAmount
	{
		get
		{
			if (_SalesPersonID == null)
			{
				_ = SalesPersonID;
			}
			return _SalesPersonAmount;
		}
	}

	public string PlantDepartmentID
	{
		get
		{
			if (_PlantDepartmentID == null)
			{
				_ = PlantID;
			}
			return _PlantDepartmentID;
		}
	}

	public string PlantID
	{
		get
		{
			if (_PlantID == null || _PlantDepartmentID == null)
			{
				SqlCommand sqlCommand = database.NewSqlCommand("SELECT TOP 1 lmeEmployeeID,lmePlantID,lmePlantDepartmentID FROM Employees WHERE lmeUserID = @UserID and lmeTerminationDate IS NULL order by lmeEmployeeID");
				sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = user.ID;
				DataTable dataTable = database.GetDataTable(sqlCommand);
				if (dataTable.Rows.Count != 0)
				{
					_PlantID = dataTable.Rows[0].Field<string>("lmePlantID").Trim();
					_PlantDepartmentID = dataTable.Rows[0].Field<string>("lmePlantDepartmentID").Trim();
				}
				else
				{
					_PlantID = string.Empty;
					_PlantDepartmentID = string.Empty;
				}
			}
			return _PlantID;
		}
	}

	public AppAxProduction(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		database = provider.GetService(typeof(M1Database)) as M1Database;
		user = provider.GetService(typeof(M1User)) as M1User;
		loadProps();
		database.PropsRefreshed += Database_PropsRefreshed;
		database.LoggingOut += Database_LoggingOut;
	}

	private void Database_LoggingOut(object sender, LoggingOutEventArgs e)
	{
		if (database != null)
		{
			database.PropsRefreshed -= Database_PropsRefreshed;
			database.LoggingOut -= Database_LoggingOut;
		}
		database = null;
	}

	private void Database_PropsRefreshed(object sender, EventArgs e)
	{
		loadProps();
	}

	private void loadProps()
	{
		DataRow row = database.Props("DatasetProperties");
		BuyQuantityDecimals = row.Field<byte>("xadBuyQuantityDecimals");
		SellQuantityDecimals = row.Field<byte>("xadSellQuantityDecimals");
		InventoryQuantityDecimals = row.Field<byte>("xadInventoryQuantityDecimals");
	}

	private bool IsAvalaraActivated(M1Database database)
	{
		if (database.Security.IsInRole("CUSTOMMODULE:5"))
		{
			DataTable dataTable = database.GetDataTable("SELECT xafAvalaraAccountID, xafAvalaraURL, xafAvalaraCompanyCode, xafAvalaraLicenseKey FROM FinancialProperties");
			if (dataTable.Rows.Count != 0 && dataTable.Rows[0].Field<string>("xafAvalaraAccountID").Trim().Length > 0 && dataTable.Rows[0].Field<string>("xafAvalaraURL").Trim().Length > 0 && dataTable.Rows[0].Field<string>("xafAvalaraCompanyCode").Trim().Length > 0 && dataTable.Rows[0].Field<string>("xafAvalaraLicenseKey").Trim().Length > 0)
			{
				return true;
			}
		}
		return false;
	}

	public string IsValidTime(decimal value, string caption)
	{
		if (value != 0m && (value > 24m || value * 100m % 100m > 59m))
		{
			return $"{caption} {value.ToString()} is not a valid time format. The valid range of values are 00:00 to 24:00.";
		}
		return string.Empty;
	}

	private void loadRoles()
	{
		_EmployeeRoles = new List<string>();
		SqlCommand sqlCommand = database.NewSqlCommand("select TOP 1 lmeEmployeeID,lmePlannerEmployee,lmeBuyerEmployee,lmeInspectorEmployee,lmeEngineerEmployee,lmeSalesEmployee from Employees where lmeUserID = @UserID and lmeTerminationDate IS NULL order by lmeEmployeeID");
		sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = user.ID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			DataRow row = dataTable.Rows[0];
			if (row.Field<bool>("lmePlannerEmployee"))
			{
				_EmployeeRoles.Add("Planner");
			}
			if (row.Field<bool>("lmeBuyerEmployee"))
			{
				_EmployeeRoles.Add("Buyer");
			}
			if (row.Field<bool>("lmeInspectorEmployee"))
			{
				_EmployeeRoles.Add("Inspector");
			}
			if (row.Field<bool>("lmeEngineerEmployee"))
			{
				_EmployeeRoles.Add("Engineer");
			}
			if (row.Field<bool>("lmeSalesEmployee"))
			{
				_EmployeeRoles.Add("Sales");
			}
		}
	}

	public bool IsInEmployeeRole(string role)
	{
		if (_EmployeeRoles == null)
		{
			loadRoles();
		}
		return _EmployeeRoles.Contains(role, StringComparer.CurrentCultureIgnoreCase);
	}

	public void RefreshTable(string tableName)
	{
		database.OnTableChanged(new TableChangedEventArgs(tableName, null, null, null));
	}

	public void Dispose()
	{
		database = null;
		provider = null;
	}
}
