using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;
using M1.ServiceCore.AxScript;

namespace M1.Ax.Erp.AxScript;

[AxScript("SFEClock")]
[ComVisible(true)]
public class AppAxSFEClock : IWebAxSFEClock
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxSFEClock(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public void ClockEmployeeIn(string employeeId, decimal shiftId, DateTime machineTime)
	{
		new SFEClock(_Database).ClockEmployeeIn(employeeId, shiftId, machineTime);
	}

	public void ClockEmployeeOut(string employeeId, DateTime machineTime)
	{
		new SFEClock(_Database).ClockEmployeeOut(employeeId, machineTime);
	}

	public void ClockEmployeeOutOfJob(DataTable tblPassedTimeCardLines, DateTime machineTime)
	{
		new SFEClock(_Database).ClockEmployeeOutOfJob(tblPassedTimeCardLines, machineTime);
	}

	public void IssueMaterialToJob(string jobId, int jobAssemblyId, int jobMaterialId, string partId, string partRevisionId, string warehouseId, string binId, decimal quantity, string lotNumber, string serialNumber)
	{
		new SFEClock(_Database).issueMaterialToJob(jobId, jobAssemblyId, jobMaterialId, partId, partRevisionId, warehouseId, binId, quantity, lotNumber, serialNumber);
	}

	public void ClockEmployeeInToIndirect(string employeeId, string indirectId, string workCenterId, DateTime machineTime)
	{
		new SFEClock(_Database).ClockEmployeeInToIndirect(employeeId, indirectId, workCenterId, machineTime);
	}

	public void ClockEmployeeInToJob(string employeeId, string jobId, decimal jobAssemblyId, decimal jobOperationId, decimal workType, DateTime machineTime, string workCenterId, string processId, int createdFromMobile, SqlTransaction sqlTran)
	{
		new SFEClock(_Database).ClockEmployeeInToJob(employeeId, jobId, jobAssemblyId, jobOperationId, workType, machineTime, workCenterId, processId, createdFromMobile, sqlTran);
	}
}
