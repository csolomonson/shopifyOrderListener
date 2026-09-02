using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("Payroll")]
[ComVisible(true)]
public class AppAxPayroll : IDisposable
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxPayroll(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public bool CalculateForm940(object transaction, int yearID, string plantID)
	{
		return new Payroll().CalculateForm940(_Database, (SqlTransaction)transaction, yearID, plantID);
	}

	public bool ProcessStateUITaxYearQuarter(object transaction, int yearID, string plantID, int quarterID)
	{
		return new Payroll().ProcessStateUITaxYearQuarter(_Database, (SqlTransaction)transaction, yearID, plantID, quarterID);
	}

	public bool ExportStateUITaxYearQuarter(int yearID, string plantID, int quarterID)
	{
		return new Payroll().ExportStateUITaxYearQuarter(_Database, yearID, plantID, quarterID);
	}

	public bool RefreshSessionAmount(int sessionID, object transaction)
	{
		return new Payroll().RefreshSessionAmount(_Database, sessionID, (SqlTransaction)transaction);
	}

	public bool STPSessionExists(int currentSessionId, string action = "")
	{
		return new SingleTouchPayroll().StpSessionExists(_Database, currentSessionId, action);
	}

	public bool STPUpdateSessionExists(int currentSessionId, string action = "")
	{
		return new SingleTouchPayroll().StpUpdateSessionExists(_Database, currentSessionId, action);
	}

	public bool STPProcessSession(M1BindingSource bindingSource)
	{
		return new SingleTouchPayroll().StpProcessSession(_Database, bindingSource);
	}

	public bool IsDateBetweenStpSessionDates(int stpTaxYear, DateTime date)
	{
		return new SingleTouchPayroll().IsDateBetweenStpSessionDates(stpTaxYear, date);
	}

	public DateTime GetMaxStpSessionDate(int stpTaxYear)
	{
		return PayrollHelpers.GetMaxStpSessionDate(stpTaxYear);
	}

	public DateTime GetMinStpSessionDate(int stpTaxYear)
	{
		return PayrollHelpers.GetMinStpSessionDate(stpTaxYear);
	}

	public string STPExportCSVCheck(object bindingSource)
	{
		M1BindingSource bindingSource2 = (M1BindingSource)bindingSource;
		return new SingleTouchPayroll().StpExportCSVCheck(bindingSource2);
	}

	public bool STPExportCSV(object bindingSource)
	{
		M1BindingSource bindingSource2 = (M1BindingSource)bindingSource;
		return new SingleTouchPayroll().StpExportCSV(bindingSource2);
	}

	public void STPClear(int sessionID)
	{
		new SingleTouchPayroll().StpClear(_Database, sessionID);
	}

	public string STPProcessPost(object bindingSource)
	{
		M1BindingSource bindingSource2 = (M1BindingSource)bindingSource;
		return new SingleTouchPayroll().StpProcessPost(bindingSource2);
	}

	public void STPCleanPreviousEmployeeId(object bindingSource, int sessionId)
	{
		M1BindingSource bindingSource2 = (M1BindingSource)bindingSource;
		new SingleTouchPayroll().StpCleanPreviousEmployeeId(bindingSource2, sessionId);
	}

	public string STPProcessGet(object bindingSource)
	{
		M1BindingSource bindingSource2 = (M1BindingSource)bindingSource;
		return new SingleTouchPayroll().StpProcessGet(bindingSource2);
	}

	public string STPCheckStatusProcess(object bindingSource)
	{
		M1BindingSource bindingSource2 = (M1BindingSource)bindingSource;
		return new SingleTouchPayroll().StpCheckStatusProcess(bindingSource2);
	}

	public bool STPIsUpdateAction(object bindingSource)
	{
		M1BindingSource bindingSource2 = (M1BindingSource)bindingSource;
		return new SingleTouchPayroll().StpIsUpdateAction(bindingSource2);
	}

	public void STPChangeEmployeesDates(object bindingSource)
	{
		M1BindingSource bindingSource2 = (M1BindingSource)bindingSource;
		new SingleTouchPayroll().StpChangeEmployeesDates(bindingSource2);
	}

	public void Dispose()
	{
		provider = null;
	}
}
