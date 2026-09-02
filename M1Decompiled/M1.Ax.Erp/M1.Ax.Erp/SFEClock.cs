using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class SFEClock : IDisposable
{
	private struct shiftBreakTimes
	{
		public int shiftID;

		public DateTime shiftDate;

		public List<DateTime> breakStartTimes;

		public List<DateTime> breakEndTimes;
	}

	private M1User currentUser;

	private M1Database currentDatabase;

	public const int splitLaborHours = 1;

	public const int splitOverheadHours = 2;

	public SFEClock(M1Database associatedDatabase)
	{
		currentUser = associatedDatabase.GetService(typeof(M1User)) as M1User;
		currentDatabase = associatedDatabase;
	}

	public TimeSpan CalculateEstimatedTime(double quantity, double productionStandard, string standardFactor)
	{
		if (productionStandard == 0.0)
		{
			return new TimeSpan(0L);
		}
		return standardFactor switch
		{
			"HP" => TimeSpan.FromHours(quantity * productionStandard), 
			"HC" => TimeSpan.FromHours(quantity * productionStandard / 100.0), 
			"HM" => TimeSpan.FromHours(quantity * productionStandard / 1000.0), 
			"MP" => TimeSpan.FromMinutes(quantity * productionStandard), 
			"MC" => TimeSpan.FromMinutes(quantity * productionStandard / 100.0), 
			"MM" => TimeSpan.FromMinutes(quantity * productionStandard / 1000.0), 
			"PH" => TimeSpan.FromHours(quantity / productionStandard), 
			"PM" => TimeSpan.FromMinutes(quantity / productionStandard), 
			"TD" => TimeSpan.FromDays(productionStandard), 
			"TH" => TimeSpan.FromHours(productionStandard), 
			"TM" => TimeSpan.FromMinutes(productionStandard), 
			"SP" => TimeSpan.FromSeconds(quantity * productionStandard), 
			_ => new TimeSpan(0L), 
		};
	}

	public DateTime RoundClockInTime(DateTime ClockInTime, decimal ShiftID, DateTime ShiftDate)
	{
		int num = (int)ShiftDate.DayOfWeek;
		if (num == 0)
		{
			num = 7;
		}
		SqlCommand sqlCommand = new SqlCommand("select * from ShiftBreaks left outer join Shifts on lmsShiftID = lmtShiftID where lmtShiftID = @ShiftID and lmtDay = @Day");
		sqlCommand.Parameters.Add(new SqlParameter("ShiftID", ShiftID));
		sqlCommand.Parameters.Add(new SqlParameter("Day", num));
		DataTable dataTable = currentDatabase.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count > 0)
		{
			DataRow row = dataTable.Rows[0];
			long num2 = ClockInTime.Ticks - ShiftDate.Date.Ticks;
			long ticks = M1Time.ConvertHrsMinsToTimeSpan(row.Field<decimal>("lmtStartTime")).Ticks;
			long num3 = M1Time.ConvertHrsMinsToTimeSpan(row.Field<decimal>("lmtEndTime")).Ticks;
			if (num3 < ticks)
			{
				num3 += 864000000000L;
			}
			long num4 = ticks - (long)(600000000m * (decimal)row.Field<short>("lmsClockInWindow"));
			long num5 = ticks + (long)(600000000m * (decimal)row.Field<short>("lmsGraceTimeIn"));
			if (num2 > num4 && num2 < num5)
			{
				return new DateTime(ClockInTime.Date.Ticks + ticks);
			}
			if (num2 > ticks && num2 < num3)
			{
				if (row.Field<bool>("lmsRoundClockWithInShift"))
				{
					return M1Time.RoundTime(ClockInTime, row.Field<byte>("lmsRoundTo"), row.Field<string>("lmsRoundClockInDirection"));
				}
				return ClockInTime;
			}
			if (row.Field<bool>("lmsRoundOutsideOfShift"))
			{
				return M1Time.RoundTime(ClockInTime, row.Field<byte>("lmsRoundTo"), row.Field<string>("lmsRoundClockInDirection"));
			}
			return ClockInTime;
		}
		throw new M1Exception(ClockInTime.DayOfWeek.ToString() + " is not set up for shift '" + ShiftID);
	}

	public DateTime RoundClockOutTime(DateTime ClockOutTime, decimal ShiftID, DateTime ShiftDate)
	{
		decimal num = (int)ShiftDate.DayOfWeek;
		if (num == 0m)
		{
			num = 7m;
		}
		SqlCommand sqlCommand = new SqlCommand("select * from ShiftBreaks left outer join Shifts on lmsShiftID = lmtShiftID where lmtShiftID = @ShiftID and lmtDay = @Day");
		sqlCommand.Parameters.Add(new SqlParameter("ShiftID", ShiftID));
		sqlCommand.Parameters.Add(new SqlParameter("Day", num));
		DataTable dataTable = currentDatabase.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count > 0)
		{
			DataRow row = dataTable.Rows[0];
			long num2 = ClockOutTime.Ticks - ShiftDate.Date.Ticks;
			long ticks = M1Time.ConvertHrsMinsToTimeSpan(row.Field<decimal>("lmtStartTime")).Ticks;
			long num3 = M1Time.ConvertHrsMinsToTimeSpan(row.Field<decimal>("lmtEndTime")).Ticks;
			if (num3 <= ticks)
			{
				num3 += 864000000000L;
			}
			long num4 = num3 + (long)(600000000m * (decimal)row.Field<short>("lmsClockOutWindow"));
			long num5 = num3 - (long)(600000000m * (decimal)row.Field<short>("lmsGraceTimeOut"));
			if (num2 < num4 && num2 > num5)
			{
				return new DateTime(ClockOutTime.Date.Ticks + num3);
			}
			if (num2 > ticks && num2 < num3)
			{
				if (row.Field<bool>("lmsRoundClockWithInShift"))
				{
					return M1Time.RoundTime(ClockOutTime, row.Field<byte>("lmsRoundTo"), row.Field<string>("lmsRoundClockOutDirection"));
				}
				return ClockOutTime;
			}
			if (row.Field<bool>("lmsRoundOutsideOfShift"))
			{
				return M1Time.RoundTime(ClockOutTime, row.Field<byte>("lmsRoundTo"), row.Field<string>("lmsRoundClockOutDirection"));
			}
			return ClockOutTime;
		}
		throw new M1Exception(ShiftDate.DayOfWeek.ToString() + " is not set up for shift '" + ShiftID + "'.");
	}

	public void ClockEmployeeIn(string EmployeeID)
	{
		ClockEmployeeIn(EmployeeID, 0m, DateTime.Now);
	}

	public void ClockEmployeeIn(string EmployeeID, decimal ShiftID)
	{
		ClockEmployeeIn(EmployeeID, ShiftID, DateTime.Now);
	}

	public void ClockEmployeeIn(string EmployeeID, DateTime MachineTime)
	{
		ClockEmployeeIn(EmployeeID, 0m, MachineTime);
	}

	public void ClockEmployeeIn(string EmployeeID, decimal ShiftID, DateTime MachineTime)
	{
		DateTime dateTime = ((!currentDatabase.Props("DC").Field<bool>("xapDCUseServerTime")) ? MachineTime : ((DateTime)currentDatabase.ExecuteScalar("select GETDATE()")));
		SqlCommand sqlCommand = new SqlCommand("select * from Employees left outer join EmployeePersonalData on lmdEmployeeID = lmeEmployeeID left outer join PayrollDefinitions on lmdPayrollDefinitionID = lmrPayrollDefinitionID where lmeEmployeeID = @EmployeeID");
		sqlCommand.Parameters.Add(new SqlParameter("EmployeeID", EmployeeID));
		DataTable dataTable = currentDatabase.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count > 0)
		{
			DataRow dataRow = dataTable.Rows[0];
			SqlCommand sqlCommand2 = new SqlCommand("select * from timecards where lmpEmployeeID = @EmployeeID and lmpActive <> 0");
			sqlCommand2.Parameters.Add(new SqlParameter("EmployeeID", EmployeeID));
			SqlDataAdapter adapter;
			DataTable dataTable2 = currentDatabase.GetDataTable(sqlCommand2, fillSchema: false, out adapter);
			if (dataTable2.Rows.Count == 0)
			{
				DataRow dataRow2 = dataTable2.AddBlankRow();
				dataRow2["LMPACTIVE"] = -1;
				dataRow2["LMPACTUALSTARTTIME"] = dateTime;
				dataRow2["LMPCREATEDBY"] = "DC";
				dataRow2["LMPCREATEDDATE"] = dateTime;
				dataRow2["LMPEMPLOYEEID"] = EmployeeID;
				dataRow2["LMPPAIDDATE"] = dateTime;
				dataRow2["LMPPLANTID"] = dataRow["LMEPLANTID"];
				if (ShiftID == 0m)
				{
					dataRow2["lmpShiftID"] = dataRow["lmeDefaultShiftID"];
				}
				else
				{
					dataRow2["lmpShiftID"] = ShiftID;
				}
				dataRow2["LMPROUNDEDSTARTTIME"] = RoundClockInTime(dataRow2.Field<DateTime>("LMPACTUALSTARTTIME"), dataRow2.Field<short>("lmpShiftID"), dataRow2.Field<DateTime>("LMPACTUALSTARTTIME"));
				dataRow2["LMPSOURCE"] = 2;
				dataRow2["LMPTIMECARDDATE"] = dateTime;
				dataRow2["LMPTIMECARDID"] = currentDatabase.NextIDs.GetNextIDForTable("Timecards");
				if (dataRow["lmrOTPeriod1PayrollRateID"] != DBNull.Value)
				{
					dataRow2["lmpOTPeriod1PayrollRateID"] = dataRow["lmrOTPeriod1PayrollRateID"];
					dataRow2["lmpOTPeriod2PayrollRateID"] = dataRow["lmrOTPeriod2PayrollRateID"];
					dataRow2["lmpOTPeriod3PayrollRateID"] = dataRow["lmrOTPeriod3PayrollRateID"];
					dataRow2["lmpOTPeriod4PayrollRateID"] = dataRow["lmrOTPeriod4PayrollRateID"];
				}
				currentDatabase.UpdateData(dataTable2, adapter);
				return;
			}
			throw new M1Exception("Employee is already clocked in.");
		}
		throw new M1Exception("Employee ID does not exist.");
	}

	public TimeSpan SubtractBreakTimesFromTimePeriod(DateTime startDateTime, DateTime endDateTime, decimal shiftID, DateTime shiftStartDate, bool includePaidBreaks, bool includeUnpaidBreaks)
	{
		TimeSpan timeSpan = endDateTime - startDateTime;
		TimeSpan timeSpan2 = startDateTime - shiftStartDate.Date;
		TimeSpan timeSpan3 = endDateTime - shiftStartDate.Date;
		if (timeSpan2 > timeSpan3)
		{
			timeSpan3 += TimeSpan.FromDays(1.0);
		}
		decimal num = (int)shiftStartDate.DayOfWeek;
		if (num == 0m)
		{
			num = 7m;
		}
		SqlCommand sqlCommand = new SqlCommand("select * from ShiftBreaks where lmtshiftID = @ShiftID and lmtDay = @Day");
		sqlCommand.Parameters.Add(new SqlParameter("ShiftID", shiftID));
		sqlCommand.Parameters.Add(new SqlParameter("Day", num));
		DataTable dataTable = currentDatabase.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count > 0)
		{
			DataRow row = dataTable.Rows[0];
			TimeSpan timeSpan4 = M1Time.ConvertHrsMinsToTimeSpan(row.Field<decimal>("lmtStartTime"));
			TimeSpan timeSpan5 = TimeSpan.FromHours(0.0);
			for (int i = 1; i <= 3; i++)
			{
				if ((!includePaidBreaks || !row.Field<bool>("lmtBreak" + i + "Paid")) && (!includeUnpaidBreaks || row.Field<bool>("lmtBreak" + i + "Paid")))
				{
					continue;
				}
				decimal num2 = row.Field<decimal>("lmtBreak" + i + "StartTime");
				decimal num3 = row.Field<decimal>("lmtBreak" + i + "EndTime");
				if (num2 != num3)
				{
					TimeSpan timeSpan6 = M1Time.ConvertHrsMinsToTimeSpan(num2);
					TimeSpan timeSpan7 = M1Time.ConvertHrsMinsToTimeSpan(num3);
					if (timeSpan7 < timeSpan4)
					{
						timeSpan7 += TimeSpan.FromDays(1.0);
					}
					if (timeSpan6 < timeSpan4)
					{
						timeSpan6 += TimeSpan.FromDays(1.0);
					}
					if (timeSpan6 >= timeSpan2 && timeSpan7 <= timeSpan3)
					{
						timeSpan5 += timeSpan7 - timeSpan6;
					}
					else if (timeSpan7 > timeSpan3 && timeSpan6 > timeSpan2 && timeSpan6 < timeSpan3)
					{
						timeSpan5 += timeSpan3 - timeSpan6;
					}
					else if (timeSpan6 < timeSpan2 && timeSpan7 > timeSpan2 && timeSpan7 < timeSpan3)
					{
						timeSpan5 += timeSpan7 - timeSpan2;
					}
					else if (timeSpan6 < timeSpan2 && timeSpan7 > timeSpan3)
					{
						timeSpan5 += timeSpan3 - timeSpan2;
					}
				}
			}
			return timeSpan - timeSpan5;
		}
		throw new M1Exception("There are no shift breaks set up for shift '" + shiftID + "' on " + shiftStartDate.DayOfWeek.ToString() + ".");
	}

	public void ClockEmployeeOut(string EmployeeID)
	{
		ClockEmployeeOut(EmployeeID, DateTime.Now);
	}

	public void ClockEmployeeOut(string EmployeeID, DateTime MachineTime)
	{
		string[] array = new string[1];
		DateTime dateTime = ((!currentDatabase.Props("DC").Field<bool>("xapDCUseServerTime")) ? MachineTime : ((DateTime)currentDatabase.ExecuteScalar("select GETDATE()")));
		StringBuilder stringBuilder = new StringBuilder("select * from employees ");
		stringBuilder.Append(" left outer join employeePersonalData on lmeEmployeeID = lmdEmployeeID ");
		stringBuilder.Append(" where lmeEmployeeID = @EmployeeID ");
		SqlCommand sqlCommand = new SqlCommand(stringBuilder.ToString());
		sqlCommand.Parameters.Add(new SqlParameter("EmployeeID", EmployeeID));
		DataTable dataTable = currentDatabase.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			throw new M1Exception($"Employee ID {EmployeeID} does not exist.");
		}
		_ = dataTable.Rows[0];
		StringBuilder stringBuilder2 = new StringBuilder("select * from timecards ");
		stringBuilder2.Append(" where lmpEmployeeID = @EmployeeID and lmpActive <> 0 ");
		SqlCommand sqlCommand2 = new SqlCommand(stringBuilder2.ToString());
		sqlCommand2.Parameters.Add(new SqlParameter("EmployeeID", EmployeeID));
		SqlDataAdapter adapter;
		DataTable dataTable2 = currentDatabase.GetDataTable(sqlCommand2, fillSchema: false, out adapter);
		if (dataTable2.Rows.Count == 0)
		{
			throw new M1Exception("Employee '" + EmployeeID + "' is not clocked in.");
		}
		DataRow dataRow = dataTable2.Rows[0];
		StringBuilder stringBuilder3 = new StringBuilder("select * from TimecardLines ");
		stringBuilder3.Append(" where lmlActive = 1 and lmlTimecardID = @TimecardID ");
		SqlCommand sqlCommand3 = new SqlCommand(stringBuilder3.ToString());
		sqlCommand3.Parameters.Add(new SqlParameter("TimecardID", dataRow.Field<int>("lmpTimecardID")));
		SqlDataAdapter adapter2;
		DataTable dataTable3 = currentDatabase.GetDataTable(sqlCommand3, fillSchema: false, out adapter2);
		if (dataTable3.Rows.Count == 0)
		{
			array[0] = dataRow["lmpTimecardID"].ToString();
			dataTable3.Clear();
			dataRow["LMPACTIVE"] = 0;
			dataRow["LMPACTUALENDTIME"] = dateTime;
			dataRow["LMPROUNDEDENDTIME"] = RoundClockOutTime(dateTime, dataRow.Field<short>("lmpShiftID"), dataRow.Field<DateTime>("LMPTIMECARDDATE"));
			SqlCommand sqlCommand4 = new SqlCommand("select * from Shifts where lmsShiftID = @ShiftID");
			sqlCommand4.Parameters.Add(new SqlParameter("ShiftID", dataRow["lmpShiftID"]));
			DataTable dataTable4 = currentDatabase.GetDataTable(sqlCommand4);
			if (dataTable4.Rows.Count == 0)
			{
				throw new M1Exception("Shift '" + dataRow["lmpShiftID"].ToString() + "' doesn't exist.");
			}
			StringBuilder stringBuilder4 = new StringBuilder("select isnull(sum(lmlLaborHours),0) as lmlLaborHours, ");
			stringBuilder4.Append("isnull(sum(lmlMachineHours),0) as lmlMachineHours ");
			stringBuilder4.Append("from TimecardLines ");
			stringBuilder4.Append("where lmlTimecardID = @TimecardID ");
			SqlCommand sqlCommand5 = new SqlCommand(stringBuilder4.ToString());
			sqlCommand5.Parameters.Add(new SqlParameter("TimecardID", dataRow["lmpTimecardID"]));
			DataTable dataTable5 = currentDatabase.GetDataTable(sqlCommand5);
			decimal num = Math.Round(dataTable5.Rows[0].Field<decimal>("lmlLaborHours"), 2);
			decimal num2 = Math.Round((decimal)SubtractBreakTimesFromTimePeriod(dataRow.Field<DateTime>("lmpRoundedStartTime"), dataRow.Field<DateTime>("lmpRoundedEndTime"), dataRow.Field<short>("lmpShiftID"), dataRow.Field<DateTime>("lmpTimecardDate"), includePaidBreaks: true, includeUnpaidBreaks: true).TotalHours, 2);
			if (num2 > num)
			{
				DataRow dataRow2 = dataTable3.AddBlankRow();
				dataRow2["lmlTimecardID"] = dataRow["lmpTimecardID"];
				dataRow2["lmlShiftID"] = dataRow["lmpShiftID"];
				dataRow2["lmlEmployeeID"] = dataRow["lmpEmployeeID"];
				dataRow2["lmlLaborHours"] = num2 - num;
				dataRow2["lmlTimecardType"] = 3;
				dataRow2["lmlSource"] = 2;
				dataRow2["lmlActive"] = 0;
				dataRow2["lmlWorkCenterID"] = "IDLE";
				dataRow2["lmlIndirectLaborID"] = dataTable4.Rows[0]["lmsIdleTimeIndirectLaborID"];
				dataRow2["lmlLaborCost"] = Math.Round(dataTable.Rows[0].Field<decimal>("lmdLaborRate") * dataRow2.Field<decimal>("lmlLaborHours"), 2);
				dataRow2["lmlExpenseID"] = dataTable.Rows[0]["lmeIndirectExpenseID"];
				NextIDList nextIDs = currentDatabase.NextIDs;
				object[] keyValues = array;
				dataRow2["lmlTimecardLineID"] = nextIDs.GetNextIDForTable("TIMECARDLINES", keyValues);
				currentDatabase.UpdateData(dataTable3, adapter2);
			}
			dataRow["LMPMACHINEHOURS"] = dataTable5.Rows[0]["lmlMachineHours"];
			dataRow["LMPPAYROLLHOURS"] = num2;
			currentDatabase.UpdateData(dataTable2, adapter);
			return;
		}
		throw new M1Exception("Timecard '" + dataRow.Field<int>("lmpTimecardID") + "' has active timecard lines.");
	}

	public void ClockEmployeeInToJob(string employeeID, string jobID, decimal jobAssemblyID, decimal jobOperationID, decimal workType, DateTime machineTime, string workCenterID, string processID, int CreatedFromMobile, SqlTransaction sqlTran)
	{
		DateTime dateTime = ((!currentDatabase.Props("DC").Field<bool>("xapDCUseServerTime")) ? machineTime : ((DateTime)currentDatabase.ExecuteScalar("select GETDATE()")));
		SqlCommand sqlCommand = new SqlCommand("select lmpTimecardID, lmpTimecardDate, lmpShiftID from timecards where lmpEmployeeID = @EmployeeID and lmpActive <> 0");
		sqlCommand.Parameters.Add(new SqlParameter("EmployeeID", employeeID));
		DataTable dataTable = currentDatabase.GetDataTable(sqlCommand, sqlTran);
		if (dataTable.Rows.Count == 0)
		{
			throw new M1Exception($"Employee '{processID}' is not clocked in.");
		}
		DataRow dataRow = dataTable.Rows[0];
		SqlCommand sqlCommand2 = new SqlCommand("select jmoJobAssemblyID, jmoJobID, jmoJobOperationID, jmoWorkCenterID, jmoProcessID from JobOperations where jmoJobID = @Job and jmoJobAssemblyID = @JobAsm and jmoJobOperationID = @JobOp");
		sqlCommand2.Parameters.Add(new SqlParameter("Job", jobID));
		sqlCommand2.Parameters.Add(new SqlParameter("JobAsm", jobAssemblyID));
		sqlCommand2.Parameters.Add(new SqlParameter("JobOp", jobOperationID));
		DataTable dataTable2 = currentDatabase.GetDataTable(sqlCommand2, sqlTran);
		if (dataTable2.Rows.Count == 0)
		{
			throw new M1Exception($"Job '{jobID.ToString()}'  Assembly '{jobAssemblyID.ToString()}' Operation '{jobOperationID.ToString()}' does not exist.");
		}
		DataRow dataRow2 = dataTable2.Rows[0];
		SqlCommand sqlCommand3 = new SqlCommand("select * from TimecardLines where lmlActive = 1 and lmlTimecardID = @TimecardID and lmlJobID = @JobID and lmlJobAssemblyID = @JobAssemblyID and lmlJobOperationID = @JobOperationID and lmlWorkType = @WorkType");
		sqlCommand3.Parameters.Add(new SqlParameter("TimecardID", dataRow.Field<int>("lmpTimecardID")));
		sqlCommand3.Parameters.Add(new SqlParameter("JobID", jobID));
		sqlCommand3.Parameters.Add(new SqlParameter("JobAssemblyID", jobAssemblyID));
		sqlCommand3.Parameters.Add(new SqlParameter("JobOperationID", jobOperationID));
		sqlCommand3.Parameters.Add(new SqlParameter("WorkType", workType));
		SqlDataAdapter adapter;
		DataTable dataTable3 = currentDatabase.GetDataTable(sqlCommand3, fillSchema: true, out adapter, sqlTran);
		if (dataTable3.Rows.Count > 0)
		{
			throw new M1Exception(string.Format("Employee '{0}' is already clocked in to Job '{1}'", dataRow["lmpTimecardID"].ToString(), jobID.ToString()));
		}
		DataRow dataRow3 = dataTable3.AddBlankRow();
		dataRow3["LMLACTIVE"] = -1;
		dataRow3["LMLACTUALSTARTTIME"] = dateTime;
		dataRow3["LMLCREATEDBY"] = currentUser.ID;
		dataRow3["LMLCREATEDDATE"] = dateTime;
		dataRow3["LMLEMPLOYEEID"] = employeeID;
		dataRow3["LMLJOBASSEMBLYID"] = dataRow2["jmoJobAssemblyID"];
		dataRow3["LMLJOBID"] = dataRow2["jmoJobID"];
		dataRow3["LMLJOBOPERATIONID"] = dataRow2["jmoJobOperationID"];
		dataRow3["LMLPROCESSID"] = (string.IsNullOrEmpty(processID) ? dataRow2["jmoProcessID"] : processID);
		dataRow3["LMLROUNDEDSTARTTIME"] = RoundClockInTime(dateTime, dataRow.Field<short>("lmpShiftID"), dataRow.Field<DateTime>("lmpTimecardDate"));
		dataRow3["LMLSHIFTID"] = dataRow["lmpShiftID"];
		dataRow3["LMLSOURCE"] = 2;
		dataRow3["LMLTIMECARDID"] = dataRow["lmpTimecardID"];
		NextIDList nextIDs = currentDatabase.NextIDs;
		object[] keyValues = new string[1] { dataRow["lmpTimecardID"].ToString() };
		dataRow3["LMLTIMECARDLINEID"] = nextIDs.GetNextIDForTable("TimecardLines", keyValues);
		dataRow3["LMLTIMECARDTYPE"] = 1;
		dataRow3["LMLWORKCENTERID"] = (string.IsNullOrEmpty(workCenterID) ? dataRow2["jmoWorkCenterID"] : workCenterID);
		dataRow3["LMLWORKTYPE"] = workType;
		currentDatabase.UpdateData(new DataRow[1] { dataRow3 }, adapter, sqlTran);
	}

	public void ClockEmployeeInToIndirect(string employeeID, string indirectID, string workCenterID, DateTime machineTime)
	{
		DateTime dateTime = ((!currentDatabase.Props("DC").Field<bool>("xapDCUseServerTime")) ? machineTime : ((DateTime)currentDatabase.ExecuteScalar("select GETDATE()")));
		SqlCommand sqlCommand = new SqlCommand("select lmpTimecardID, lmpTimecardDate, lmpShiftID from timecards where lmpEmployeeID = @EmployeeID and lmpActive <> 0");
		sqlCommand.Parameters.Add(new SqlParameter("EmployeeID", employeeID));
		DataTable dataTable = currentDatabase.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			throw new M1Exception("Employee '" + employeeID + "' is not clocked in.");
		}
		DataRow dataRow = dataTable.Rows[0];
		StringBuilder stringBuilder = new StringBuilder(" select * from TimecardLines ");
		stringBuilder.Append("where lmlActive = 1 and lmlTimecardID = @TimecardID and lmlIndirectLaborID = @IndirectID ");
		SqlCommand sqlCommand2 = new SqlCommand(stringBuilder.ToString());
		sqlCommand2.Parameters.Add(new SqlParameter("TimecardID", dataRow.Field<int>("lmpTimecardID")));
		sqlCommand2.Parameters.Add(new SqlParameter("IndirectID", indirectID));
		SqlDataAdapter adapter;
		DataTable dataTable2 = currentDatabase.GetDataTable(sqlCommand2, fillSchema: true, out adapter);
		if (dataTable2.Rows.Count > 0)
		{
			throw new M1Exception("Employee '" + dataRow["lmpTimecardID"].ToString() + "' is already clocked in to indirect: '" + indirectID + "'");
		}
		DataRow dataRow2 = dataTable2.AddBlankRow();
		dataRow2["LMLACTIVE"] = -1;
		dataRow2["LMLACTUALSTARTTIME"] = dateTime;
		dataRow2["LMLCREATEDBY"] = currentUser.ID;
		dataRow2["LMLCREATEDDATE"] = dateTime;
		dataRow2["LMLEMPLOYEEID"] = employeeID;
		dataRow2["LMLINDIRECTLABORID"] = indirectID;
		dataRow2["LMLROUNDEDSTARTTIME"] = RoundClockInTime(dateTime, dataRow.Field<short>("lmpShiftID"), dataRow.Field<DateTime>("lmpTimecardDate"));
		dataRow2["LMLSHIFTID"] = dataRow["lmpShiftID"];
		dataRow2["LMLSOURCE"] = 2;
		dataRow2["LMLTIMECARDID"] = dataRow["lmpTimecardID"];
		NextIDList nextIDs = currentDatabase.NextIDs;
		object[] keyValues = new string[1] { dataRow["lmpTimecardID"].ToString() };
		dataRow2["LMLTIMECARDLINEID"] = nextIDs.GetNextIDForTable("TimecardLines", keyValues);
		dataRow2["LMLTIMECARDTYPE"] = 2;
		dataRow2["LMLWORKCENTERID"] = ((!string.IsNullOrEmpty(workCenterID)) ? workCenterID : "");
		currentDatabase.UpdateData(new DataRow[1] { dataRow2 }, adapter);
	}

	public void splitHoursForTimecardLine(decimal timecardID, decimal timecardLineID, bool recalculateAllOverlappingTimecards)
	{
		SqlCommand sqlCommand = new SqlCommand("select * from TimecardLines left outer join workcenters on xawWorkCenterID = lmlWorkCenterID where lmlTimecardID = @TimecardID and lmlTimecardLineID = @TimecardLineID");
		sqlCommand.Parameters.Add(new SqlParameter("TimecardID", timecardID));
		sqlCommand.Parameters.Add(new SqlParameter("TimecardLineID", timecardLineID));
		DataTable dataTable = currentDatabase.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			throw new M1Exception("Timecard '" + timecardID + "' line '" + timecardLineID + "' doesn't exist.");
		}
		DataRow dataRow = dataTable.Rows[0];
		SqlCommand sqlCommand2 = new SqlCommand("select * from timecards where lmpTimecardID = @TimecardID");
		sqlCommand2.Parameters.Add(new SqlParameter("TimecardID", timecardID));
		DataTable dataTable2 = currentDatabase.GetDataTable(sqlCommand2);
		if (dataTable2.Rows.Count == 0)
		{
			throw new M1Exception("Timecard '" + timecardID + "' doesn't exist.");
		}
		DataRow dataRow2 = dataTable2.Rows[0];
		SqlCommand sqlCommand3 = new SqlCommand("select * from EmployeePersonalData where lmdEmployeeID = @EmployeeID");
		sqlCommand3.Parameters.Add(new SqlParameter("EmployeeID", dataRow2["lmpEmployeeID"]));
		DataTable dataTable3 = currentDatabase.GetDataTable(sqlCommand3);
		if (dataTable3.Rows.Count == 0)
		{
			throw new M1Exception("Employee '" + dataRow2["lmpEmployeeID"].ToString() + "' has no labor rate.");
		}
		bool flag = currentDatabase.Props("DC").Field<byte>("xapDCLaborCalculationMethod") == 1;
		SqlCommand sqlCommand4 = ((!recalculateAllOverlappingTimecards) ? new SqlCommand("select * from timecardLines where lmlEmployeeID = @EmployeeID and lmlActive = 0 and lmlLaborHoursCalculated = 0 and lmlTimecardType <> 3 and (lmlRoundedStartTime < @EndTime and lmlRoundedEndTime > @StartTime)") : new SqlCommand("select * from timecardLines where lmlEmployeeID = @EmployeeID and lmlActive = 0 and lmlTimecardType <> 3 and (lmlRoundedStartTime < @EndTime and lmlRoundedEndTime > @StartTime)"));
		sqlCommand4.Parameters.Add(new SqlParameter("EmployeeID", dataRow2["lmpEmployeeID"]));
		sqlCommand4.Parameters.Add(new SqlParameter("EndTime", dataRow["lmlRoundedEndTime"]));
		sqlCommand4.Parameters.Add(new SqlParameter("StartTime", dataRow["lmlRoundedStartTime"]));
		SqlDataAdapter adapter;
		DataTable dataTable4 = currentDatabase.GetDataTable(sqlCommand4, fillSchema: true, out adapter);
		SqlCommand sqlCommand5 = new SqlCommand("select count(*) from timecardLines where lmlEmployeeID = @EmployeeID and and lmlActive = 1 and lmlRoundedStartDate < @EndTime");
		sqlCommand5.Parameters.Add(new SqlParameter("EmployeeID", ""));
		sqlCommand5.Parameters.Add(new SqlParameter("EndTime", ""));
		foreach (DataRow row in dataTable4.Rows)
		{
			sqlCommand5.Parameters["EmployeeID"].Value = dataRow["lmlEmployeeID"];
			sqlCommand5.Parameters["EndTime"].Value = row["lmlRoundedEndTime"];
			if (!flag || (int)currentDatabase.ExecuteScalar(sqlCommand5) == 0)
			{
				decimal num = (decimal)splitTimecardLineTime(row.Field<int>("lmlTimecardID"), row.Field<short>("lmlTimecardLineID"), 1, flag).Ticks / 36000000000m;
				row["lmlLaborHours"] = decimal.Round(num, 2);
				row["lmlLaborCost"] = decimal.Round(num * dataTable3.Rows[0].Field<decimal>("lmdLaborRate"), 2);
				row["lmlLaborHoursCalculated"] = -1;
			}
		}
		currentDatabase.UpdateData(dataTable4, adapter);
		if (dataRow.Field<byte>("lmlTimecardType") != 1)
		{
			return;
		}
		if (dataRow["xawOverheadRate"] == DBNull.Value)
		{
			throw new M1Exception("Work Center '" + dataRow["lmlWorkCenterID"]?.ToString() + "' does not exist.");
		}
		SqlCommand sqlCommand6 = ((!recalculateAllOverlappingTimecards) ? new SqlCommand("select * from timecardLines where lmlWorkCenterID = @WorkCenterID and lmlActive = 0 and lmlMachineHoursCalculated = 0 and lmlTimecardType = 1 and (lmlRoundedStartTime < @EndTime and lmlRoundedEndTime > @StartTime)") : new SqlCommand("select * from timecardLines where lmlWorkCenterID = @WorkCenterID and lmlActive = 0 and lmlTimecardType = 1 and (lmlRoundedStartTime < @EndTime and lmlRoundedEndTime > @StartTime)"));
		sqlCommand6.Parameters.Add(new SqlParameter("WorkCenterID", dataRow["lmlWorkCenterID"]));
		sqlCommand6.Parameters.Add(new SqlParameter("EndTime", dataRow["lmlRoundedEndTime"]));
		sqlCommand6.Parameters.Add(new SqlParameter("StartTime", dataRow["lmlRoundedStartTime"]));
		SqlDataAdapter adapter2;
		DataTable dataTable5 = currentDatabase.GetDataTable(sqlCommand6, fillSchema: true, out adapter2);
		SqlCommand sqlCommand7 = new SqlCommand("select count(*) from timecardLines where lmlWorkCenterID = @WorkCenterID and lmlActive = 1 and lmlRoundedStartDate < @EndTime");
		sqlCommand7.Parameters.Add(new SqlParameter("WorkCenterID", ""));
		sqlCommand7.Parameters.Add(new SqlParameter("EndTime", ""));
		foreach (DataRow row2 in dataTable5.Rows)
		{
			sqlCommand7.Parameters["WorkCenterID"].Value = dataRow["lmlWorkCenterID"];
			sqlCommand7.Parameters["EndTime"].Value = row2["lmlRoundedEndTime"];
			if (!flag || (int)currentDatabase.ExecuteScalar(sqlCommand7) == 0)
			{
				decimal num2 = (decimal)splitTimecardLineTime(row2.Field<int>("lmlTimecardID"), row2.Field<short>("lmlTimecardLineID"), 2, flag).Ticks / 36000000000m;
				row2["lmlMachineHours"] = decimal.Round(num2, 2);
				row2["lmlOverheadCost"] = decimal.Round(num2 * dataRow.Field<decimal>("xawOverheadRate"), 2);
				row2["lmlMachineHoursCalculated"] = -1;
			}
		}
		currentDatabase.UpdateData(dataTable5, adapter2);
	}

	public TimeSpan splitTimecardLineTime(int timecardID, short timecardLineID, int splitType, bool cycleTime)
	{
		long num = 0L;
		long num2 = 0L;
		shiftBreakTimes shiftBreakTimes2 = new shiftBreakTimes
		{
			breakStartTimes = new List<DateTime>(),
			breakEndTimes = new List<DateTime>()
		};
		List<DateTime> list = new List<DateTime>();
		List<shiftBreakTimes> list2 = new List<shiftBreakTimes>();
		SqlCommand sqlCommand = new SqlCommand("select * from timecardLines inner join Timecards on lmpTimecardID = lmlTimecardID where lmlTimecardID = @TimecardID and lmlTimecardLineID = @TimecardLineID");
		sqlCommand.Parameters.Add(new SqlParameter("TimecardID", timecardID));
		sqlCommand.Parameters.Add(new SqlParameter("TimecardLineID", timecardLineID));
		DataTable dataTable = currentDatabase.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			throw new M1Exception("Timecard '" + timecardID + "' line '" + timecardLineID + "' doesn't exist.");
		}
		DataRow dataRow = dataTable.Rows[0];
		if (splitType == 2 && dataRow.Field<byte>("lmlTimecardType") != 1)
		{
			return new TimeSpan(0L);
		}
		SqlCommand sqlCommand2;
		switch (splitType)
		{
		case 1:
			_ = "lmpEmployeeID = " + dataRow["lmpEmployeeID"].ToSql();
			sqlCommand2 = new SqlCommand("select lmlTimecardID, lmlTimecardLineID, lmlRoundedStartTime, lmlRoundedEndTime, lmlWorkType, lmlGoodQuantity, lmlSetupPercentCompleted, lmlActive, lmpShiftID, lmpTimecardDate, lmpShiftBreakID, jmoSetupHours, jmoProductionStandard, jmoStandardFactor, lmtBreak1StartTime, lmtBreak1EndTime, lmtBreak2StartTime, lmtBreak2EndTime, lmtBreak3StartTime, lmtBreak3EndTime from TimecardLines left outer join Timecards on lmlTimecardID = lmpTimecardID left outer join JobOperations on lmlJobID = jmoJobID and lmlJobAssemblyID = jmoJobAssemblyID and lmlJobOperationID = jmoJobOperationID left outer join ShiftBreaks on lmpShiftID = lmtShiftID and lmpShiftBreakID = lmtDay where lmpEmployeeID = @EmployeeID and ((lmlRoundedStartTime < @EndTime and lmlRoundedEndTime > @StartTime) or (lmlRoundedStartTime < @EndTime and lmlActive = 1))");
			sqlCommand2.Parameters.Add(new SqlParameter("EmployeeID", dataRow["lmpEmployeeID"]));
			sqlCommand2.Parameters.Add(new SqlParameter("EndTime", dataRow["lmlRoundedEndTime"]));
			sqlCommand2.Parameters.Add(new SqlParameter("StartTime", dataRow["lmlRoundedStartTime"]));
			break;
		case 2:
			_ = "lmlWorkCenterID = " + dataRow["lmlWorkCenterID"].ToSql();
			sqlCommand2 = new SqlCommand("select lmlTimecardID, lmlTimecardLineID, lmlRoundedStartTime, lmlRoundedEndTime, lmlWorkType, lmlGoodQuantity, lmlSetupPercentCompleted, lmlActive, lmpShiftID, lmpTimecardDate, lmpShiftBreakID, jmoSetupHours, jmoProductionStandard, jmoStandardFactor, lmtBreak1StartTime, lmtBreak1EndTime, lmtBreak2StartTime, lmtBreak2EndTime, lmtBreak3StartTime, lmtBreak3EndTime from TimecardLines left outer join Timecards on lmlTimecardID = lmpTimecardID left outer join JobOperations on lmlJobID = jmoJobID and lmlJobAssemblyID = jmoJobAssemblyID and lmlJobOperationID = jmoJobOperationID left outer join ShiftBreaks on lmpShiftID = lmtShiftID and lmpShiftBreakID = lmtDay where lmlWorkCenterID = @WorkCenterID and ((lmlRoundedStartTime < @EndTime and lmlRoundedEndTime > @StartTime) or (lmlRoundedStartTime < @EndTime and lmlActive = 1))");
			sqlCommand2.Parameters.Add(new SqlParameter("WorkCenterID", dataRow["lmlWorkCenterID"]));
			sqlCommand2.Parameters.Add(new SqlParameter("EndTime", dataRow["lmlRoundedEndTime"]));
			sqlCommand2.Parameters.Add(new SqlParameter("StartTime", dataRow["lmlRoundedStartTime"]));
			break;
		default:
			throw new M1Exception("'" + splitType + "' is not a valid split type (1 for labor split, 2 for overhead split)");
		}
		DataTable dataTable2 = currentDatabase.GetDataTable(sqlCommand2);
		dataTable2.Columns.Add("ActualDuration", typeof(long));
		dataTable2.Columns.Add("EstimatedDuration", typeof(long));
		DateTime dateTime2;
		foreach (DataRow rowOverlappingTimecardLine in dataTable2.Rows)
		{
			DateTime dateTime = rowOverlappingTimecardLine.Field<DateTime>("lmlRoundedStartTime");
			dateTime2 = ((!rowOverlappingTimecardLine.Field<bool>("lmlActive") && rowOverlappingTimecardLine["lmlRoundedEndTime"] != DBNull.Value) ? rowOverlappingTimecardLine.Field<DateTime>("lmlRoundedEndTime") : dataRow.Field<DateTime>("lmlRoundedEndTime"));
			if (!list.Contains(dateTime))
			{
				list.Add(dateTime);
			}
			if (!list.Contains(dateTime2))
			{
				list.Add(dateTime2);
			}
			rowOverlappingTimecardLine["ActualDuration"] = SubtractBreakTimesFromTimePeriod(dateTime, dateTime2, rowOverlappingTimecardLine.Field<short>("lmpShiftID"), rowOverlappingTimecardLine.Field<DateTime>("lmpTimecardDate"), includePaidBreaks: true, includeUnpaidBreaks: true).Ticks;
			switch (rowOverlappingTimecardLine.Field<byte>("lmlWorkType"))
			{
			case 1:
				rowOverlappingTimecardLine["EstimatedDuration"] = (long)((double)rowOverlappingTimecardLine.Field<decimal>("lmlSetupPercentCompleted") / 100.0 * (double)rowOverlappingTimecardLine.Field<decimal>("jmoSetupHours") * 36000000000.0);
				break;
			case 2:
				rowOverlappingTimecardLine["EstimatedDuration"] = CalculateEstimatedTime((double)rowOverlappingTimecardLine.Field<decimal>("lmlGoodQuantity"), (double)rowOverlappingTimecardLine.Field<decimal>("jmoProductionStandard"), rowOverlappingTimecardLine["jmoStandardFactor"].ToString()).Ticks;
				break;
			default:
				rowOverlappingTimecardLine["EstimatedDuration"] = rowOverlappingTimecardLine["ActualDuration"];
				break;
			}
			if (!list2.Exists((shiftBreakTimes times) => times.shiftID == rowOverlappingTimecardLine.Field<short>("lmpShiftID") && times.shiftDate == rowOverlappingTimecardLine.Field<DateTime>("lmpTimecardDate").Date))
			{
				shiftBreakTimes item = new shiftBreakTimes
				{
					shiftID = rowOverlappingTimecardLine.Field<short>("lmpShiftID"),
					shiftDate = rowOverlappingTimecardLine.Field<DateTime>("lmpTimecardDate").Date,
					breakStartTimes = new List<DateTime>(),
					breakEndTimes = new List<DateTime>()
				};
				if (rowOverlappingTimecardLine["lmtBreak1StartTime"] != DBNull.Value)
				{
					for (int num3 = 1; num3 <= 3; num3++)
					{
						DateTime dateTime3 = rowOverlappingTimecardLine.Field<DateTime>("lmpTimecardDate") + M1Time.ConvertHrsMinsToTimeSpan(rowOverlappingTimecardLine.Field<decimal>("lmtBreak" + num3 + "StartTime"));
						DateTime dateTime4 = rowOverlappingTimecardLine.Field<DateTime>("lmpTimecardDate") + M1Time.ConvertHrsMinsToTimeSpan(rowOverlappingTimecardLine.Field<decimal>("lmtBreak" + num3 + "EndTime"));
						if (dateTime3 != dateTime4)
						{
							if (!list.Contains(dateTime3))
							{
								list.Add(dateTime3);
							}
							item.breakStartTimes.Add(dateTime3);
							if (!list.Contains(dateTime4))
							{
								list.Add(dateTime4);
							}
							item.breakEndTimes.Add(dateTime4);
						}
					}
				}
				list2.Add(item);
			}
			if (rowOverlappingTimecardLine.Field<int>("lmlTimecardID") == timecardID && rowOverlappingTimecardLine.Field<short>("lmlTimecardLineID") == timecardLineID)
			{
				num = rowOverlappingTimecardLine.Field<long>("ActualDuration");
				num2 = rowOverlappingTimecardLine.Field<long>("EstimatedDuration");
				shiftBreakTimes2 = list2.Find((shiftBreakTimes times) => times.shiftID == rowOverlappingTimecardLine.Field<short>("lmpShiftID") && times.shiftDate == rowOverlappingTimecardLine.Field<DateTime>("lmpTimecardDate").Date);
			}
		}
		list.Sort();
		long num4 = 0L;
		dateTime2 = list.First();
		long num5 = 0L;
		foreach (DateTime item2 in list)
		{
			DateTime dateTime = dateTime2;
			dateTime2 = item2;
			long num6 = dateTime2.Ticks - dateTime.Ticks;
			if (!(dateTime < dateTime2) || !(dateTime >= dataRow.Field<DateTime>("lmlRoundedStartTime")) || (dataRow["lmlRoundedEndTime"] != DBNull.Value && !(dateTime2 <= dataRow.Field<DateTime>("lmlRoundedEndTime"))) || (shiftBreakTimes2.breakStartTimes.Contains(dateTime) && shiftBreakTimes2.breakEndTimes.Contains(dateTime2)))
			{
				continue;
			}
			int num7 = 0;
			num5 = 0L;
			foreach (DataRow rowOverlappingTimecardLine2 in dataTable2.Rows)
			{
				shiftBreakTimes item = list2.Find((shiftBreakTimes times) => times.shiftID == rowOverlappingTimecardLine2.Field<short>("lmpShiftID") && times.shiftDate == rowOverlappingTimecardLine2.Field<DateTime>("lmpTimecardDate").Date);
				if (rowOverlappingTimecardLine2.Field<DateTime>("lmlRoundedStartTime") <= dateTime && (rowOverlappingTimecardLine2["lmlroundedEndTime"] == DBNull.Value || rowOverlappingTimecardLine2.Field<DateTime>("lmlroundedEndTime") >= dateTime2) && (!item.breakStartTimes.Contains(dateTime) || !item.breakEndTimes.Contains(dateTime2)))
				{
					num7++;
					num5 += (long)((double)rowOverlappingTimecardLine2.Field<long>("EstimatedDuration") * (double)num6 / (double)rowOverlappingTimecardLine2.Field<long>("ActualDuration"));
				}
			}
			if (cycleTime && num5 > 0)
			{
				num4 += (long)((double)num2 * (double)num6 / (double)num * (double)num6 / (double)num5);
			}
			else if (num7 > 0)
			{
				num4 += num6 / num7;
			}
		}
		return new TimeSpan(num4);
	}

	public void ClockEmployeeOutOfJob(decimal timecardID, decimal timecardLineID)
	{
		ClockEmployeeOutOfJob(timecardID, timecardLineID, DateTime.Now, 1m, 0m);
	}

	public void ClockEmployeeOutOfJob(decimal timecardID, decimal timecardLineID, DateTime machineTime)
	{
		ClockEmployeeOutOfJob(timecardID, timecardLineID, machineTime, 1m, 0m);
	}

	public void ClockEmployeeOutOfJob(decimal timecardID, decimal timecardLineID, DateTime machineTime, decimal completionType, decimal goodQuantity)
	{
		DataTable dataTable = new DataTable();
		currentDatabase.Fill(dataTable, "select * from timecardLines where lmlTimecardID = " + timecardID.ToSql() + " and lmlTimecardLineID = " + timecardLineID.ToSql());
		if (dataTable.Rows.Count == 0)
		{
			throw new M1Exception("Timecard '" + timecardID + "' line '" + timecardLineID + "' doesn't exist.");
		}
		dataTable.Rows[0]["LMLCOMPLETIONTYPE"] = completionType;
		dataTable.Rows[0]["LMLGOODQUANTITY"] = goodQuantity;
		ClockEmployeeOutOfJob(dataTable, machineTime);
	}

	public void ClockEmployeeOutOfJob(DataTable tblPassedTimecardLines, DateTime machineTime)
	{
		DateTime dateTime = ((!currentDatabase.Props("DC").Field<bool>("xapDCUseServerTime")) ? machineTime : ((DateTime)currentDatabase.ExecuteScalar("select GETDATE()")));
		foreach (DataRow row2 in tblPassedTimecardLines.Rows)
		{
			SqlCommand sqlCommand = new SqlCommand("select * from timecardLines where lmlTimecardID = @TimecardID and lmlTimecardLineID = @TimecardLineID");
			sqlCommand.Parameters.Add(new SqlParameter("TimecardID", row2.Field<int>("lmlTimecardID")));
			sqlCommand.Parameters.Add(new SqlParameter("TimecardLineID", row2.Field<short>("lmlTimecardLineID")));
			SqlDataAdapter adapter;
			DataTable dataTable = currentDatabase.GetDataTable(sqlCommand, fillSchema: true, out adapter);
			if (dataTable.Rows.Count == 0)
			{
				throw new M1Exception("Timecard '" + row2["lmlTimecardID"].ToString() + "' line '" + row2["lmlTimecardLineID"].ToString() + "' doesn't exist.");
			}
			DataRow dataRow2 = dataTable.Rows[0];
			if (!dataRow2.Field<bool>("lmlActive"))
			{
				throw new M1Exception("Timecard '" + dataRow2["lmlTimecardID"].ToString() + "' line '" + dataRow2["lmlTimecardLineID"].ToString() + "' is not active.");
			}
			SqlCommand sqlCommand2 = new SqlCommand("select lmpEmployeeID, lmpShiftID, lmpTimecardDate, lmpActive from Timecards where lmpTimecardID = @TimecardID");
			sqlCommand2.Parameters.Add(new SqlParameter("TimecardID", row2.Field<int>("lmlTimecardID")));
			DataTable dataTable2 = currentDatabase.GetDataTable(sqlCommand2);
			if (dataTable2.Rows.Count == 0)
			{
				throw new M1Exception("Timecard '" + row2.Field<int>("lmlTimecardID") + "' doesn't exist.");
			}
			DataRow row = dataTable2.Rows[0];
			if (!row.Field<bool>("lmpActive"))
			{
				throw new M1Exception("Timecard '" + row2.Field<int>("lmlTimecardID") + "' is not active.");
			}
			SqlCommand sqlCommand3 = new SqlCommand("select lmeDirectExpenseID from employees where lmeEmployeeID = @EmployeeID");
			sqlCommand3.Parameters.Add(new SqlParameter("EmployeeID", row.Field<string>("lmpEmployeeID")));
			DataTable dataTable3 = currentDatabase.GetDataTable(sqlCommand3);
			if (dataTable3.Rows.Count == 0)
			{
				throw new M1Exception("Employee ID " + row.Field<string>("lmpEmployeeID") + " does not exist.");
			}
			DataRow dataRow3 = dataTable3.Rows[0];
			foreach (DataColumn column in tblPassedTimecardLines.Columns)
			{
				if (dataTable.Columns.Contains(column.ColumnName))
				{
					dataRow2[column.ColumnName] = row2[column];
				}
			}
			dataRow2["LMLACTIVE"] = 0;
			dataRow2["LMLACTUALENDTIME"] = dateTime;
			dataRow2["LMLEXPENSEID"] = dataRow3["lmeDirectExpenseID"];
			dataRow2["LMLROUNDEDENDTIME"] = RoundClockOutTime(dateTime, row.Field<short>("lmpShiftID"), row.Field<DateTime>("lmpTimecardDate"));
			currentDatabase.UpdateData(new DataRow[1] { dataRow2 }, adapter);
			int num = row2.Field<int>("lmlTimecardID");
			short num2 = row2.Field<short>("lmlTimecardLineID");
			splitHoursForTimecardLine(num, num2, recalculateAllOverlappingTimecards: false);
		}
	}

	public void issueMaterialToJob(string jobID, int jobAssemblyID, int jobMaterialID, string partID, string partRevisionID, string warehouseID, string binID, decimal quantity, string lotNumber, string serialNumber)
	{
		SqlCommand sqlCommand = new SqlCommand("select * from JobMaterials where jmmJobID = @JobID and jmmJobAssemblyID = @JobAssemblyID and jmmJobMaterialID = @JobMaterialID");
		sqlCommand.Parameters.Add(new SqlParameter("JobID", jobID));
		sqlCommand.Parameters.Add(new SqlParameter("JobAssemblyID", jobAssemblyID));
		sqlCommand.Parameters.Add(new SqlParameter("JobMaterialID", jobMaterialID));
		DataTable dataTable = currentDatabase.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			throw new M1Exception("The Job Material: " + jobID + " asm: " + jobAssemblyID + " mtl: " + jobMaterialID + " is not set up on the job.");
		}
		DataRow dataRow = dataTable.Rows[0];
		if (!partID.Trim().Equals(dataRow.Field<string>("jmmPartID").Trim(), StringComparison.CurrentCultureIgnoreCase) || !partRevisionID.Trim().Equals(dataRow.Field<string>("jmmPartRevisionID").Trim(), StringComparison.CurrentCultureIgnoreCase))
		{
			throw new M1Exception("The Job Material: " + jobID + " asm: " + jobAssemblyID + " mtl: " + jobMaterialID + " does not use part: " + partID + " rev: " + partRevisionID);
		}
		SqlCommand sqlCommand2 = new SqlCommand("select imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, imbBinQuantityOnHand, imbQuantityAllocated, imrQuantityOnHand, imrQuantityAllocated, imrInventoryUnitOfMeasure, impTrackLotNumbers, impTrackSerialNumbers, imrLastDutyCost + imrLastFreightCost + imrLastLaborCost + imrLastMaterialCost + imrLastMiscCost + imrLastOverheadCost + imrLastSubcontractCost as LastCost, imrAverageDutyCost + imrAverageFreightCost + imrAverageLaborCost + imrAverageMaterialCost + imrAverageMiscCost + imrAverageOverheadCost + imrAverageSubcontractCost as AverageCost, imrStandardDutyCost + imrStandardFreightCost + imrStandardLaborCost + imrStandardMaterialCost + imrStandardMiscCost + imrStandardOverheadCost + imrStandardSubcontractCost as StandardCost, ablLotNumberID, ablStatus, ablQuantityOnHand, imsSerialNumberID, imsStatus, imsPartWarehouseLocationID, imsPartBinID from PartBins left outer join PartWarehouseLocations on imbPartID = imlPartID and imbPartRevisionID = imlPartRevisionID and imbWarehouseID = imlPartWarehouseID left outer join PartRevisions on imbPartID = imrPartID and imbPartRevisionID = imrPartRevisionID left outer join Parts on imbPartID = impPartID left outer join LotNumbers on ablPartID = imbPartID and ablPartRevisionID = imbPartRevisionID and ablPartWarehouseLocationID = imbWarehouseID and ablPartBinID = imbPartBinID and ablLotNumberID = @LotNumber left outer join SerialNumbers on imsPartID = imbPartID and imsPartRevisionID = imbPartRevisionID and imsSerialNumberID = @SerialNumber where imbPartID = @PartID and imbPartRevisionID = @PartRevisionID and imbWarehouseID = @WarehouseID and imbPartBinID = @BinID");
		sqlCommand2.Parameters.Add(new SqlParameter("PartID", partID));
		sqlCommand2.Parameters.Add(new SqlParameter("PartRevisionID", partRevisionID));
		sqlCommand2.Parameters.Add(new SqlParameter("WarehouseID", warehouseID));
		sqlCommand2.Parameters.Add(new SqlParameter("BinID", binID));
		sqlCommand2.Parameters.Add(new SqlParameter("@LotNumber", lotNumber));
		sqlCommand2.Parameters.Add(new SqlParameter("@SerialNumber", serialNumber));
		DataTable dataTable2 = currentDatabase.GetDataTable(sqlCommand2);
		if (dataTable2.Rows.Count == 0)
		{
			throw new M1Exception("Part: " + partID + " Revision: " + partRevisionID + " Warehouse: " + warehouseID + " Bin: " + binID + " has not been set up in the system.");
		}
		DataRow dataRow2 = dataTable2.Rows[0];
		if (dataRow2.Field<decimal>("imbBinQuantityOnHand") < quantity)
		{
			throw new M1Exception("Part: " + partID + " Revision: " + partRevisionID + " Warehouse: " + warehouseID + " Bin: " + binID + " only has " + dataRow2.Field<decimal>("imbBinQuantityOnHand") + " on hand. (" + quantity + " required)");
		}
		if (dataRow2.Field<decimal>("imrQuantityOnHand") < quantity)
		{
			throw new M1Exception("Part: " + partID + " Revision: " + partRevisionID + " only has " + dataRow2.Field<decimal>("imrQuantityOnHand") + " on hand. (" + quantity + " required)");
		}
		if (dataRow2.Field<bool>("impTrackLotNumbers"))
		{
			if (lotNumber == null || lotNumber.Trim() == "")
			{
				throw new M1Exception("Part: " + partID + " is lot number tracked and no lot number was supplied.");
			}
			if (dataRow2["ablLotNumberID"] == DBNull.Value)
			{
				throw new M1Exception("Lot Number: " + lotNumber + " is not set up for Part: " + partID + ", Revision: " + partRevisionID + ", Warehouse: " + warehouseID + ", Bin: " + binID);
			}
			if (dataRow2.Field<decimal>("ablQuantityOnHand") < quantity)
			{
				throw new M1Exception("Part: " + partID + " Revision: " + partRevisionID + " Warehouse: " + warehouseID + " Bin: " + binID + " Lot: " + lotNumber + " only has " + dataRow2.Field<decimal>("ablQuantityOnHand") + " on hand. (" + quantity + " required)");
			}
		}
		if (dataRow2.Field<bool>("impTrackSerialNumbers"))
		{
			if (serialNumber == null || serialNumber.Trim() == "")
			{
				throw new M1Exception("Part " + partID + " is serial number tracked and no serial number was supplied.");
			}
			if (dataRow2["imsSerialNumberID"] == DBNull.Value)
			{
				throw new M1Exception("Serial Number: " + serialNumber + " is not set up for Part: " + partID + ", Revision: " + partRevisionID);
			}
			if (dataRow2.Field<byte>("imsStatus") != 2)
			{
				throw new M1Exception("Serial Number: " + serialNumber + " cannot be issued to the job because its status is not 'In Inventory'.");
			}
			if (quantity != 1m)
			{
				throw new M1Exception("Part " + partID + " is serial number tracked so the quantity to issue needs to be one.");
			}
		}
		decimal num = default(decimal);
		switch ((int)currentDatabase.Props("IM").Field<byte>("xapIMCostingMethod"))
		{
		case 1:
			num = dataRow2.Field<decimal>("AverageCost");
			break;
		case 2:
			num = dataRow2.Field<decimal>("LastCost");
			break;
		case 3:
			num = dataRow2.Field<decimal>("StandardCost");
			break;
		}
		SqlTransaction sqlTransaction = currentDatabase.BeginTransaction();
		try
		{
			SqlDataAdapter adapter;
			DataRow dataRow3 = currentDatabase.GetDataTable("select * from PartTransactions where 0=1", fillSchema: true, out adapter, sqlTransaction).AddBlankRow();
			dataRow3["imtPartTransactionID"] = currentDatabase.NextIDs.GetNextIDForTable("PartTransactions");
			dataRow3["imtTransactionType"] = 2;
			dataRow3["imtIssueType"] = 1;
			dataRow3["imtJobType"] = 1;
			dataRow3["imtJobID"] = jobID;
			dataRow3["imtJobAssemblyID"] = jobAssemblyID;
			dataRow3["imtJobMaterialID"] = jobMaterialID;
			dataRow3["imtPartID"] = partID;
			dataRow3["imtPartRevisionID"] = partRevisionID;
			dataRow3["imtPartWarehouseLocationID"] = warehouseID;
			dataRow3["imtPartBinID"] = binID;
			dataRow3["imtPreviousQuantityOnHand"] = dataRow2["imbBinQuantityOnHand"];
			dataRow3["imtInventoryQuantityReceived"] = quantity;
			dataRow3["imtInventoryUnitOfMeasure"] = dataRow2["imrInventoryUnitOfMeasure"];
			dataRow3["imtTransactionDate"] = DateTime.Now;
			dataRow3["imtUserID"] = currentUser.ID;
			dataRow3["imtUnitMaterialCost"] = num;
			dataRow3["imtSource"] = 3;
			dataRow3["imtEstUnitMaterialCost"] = dataRow["jmmEstimatedUnitCost"];
			dataRow3["imtEstUnitCostOfGoodsSold"] = dataRow3["imtEstUnitMaterialCost"];
			dataRow3["imtEstTotalMaterialCost"] = dataRow.Field<decimal>("jmmEstimatedUnitCost") * quantity;
			dataRow3["imtEstTotalCostOfGoodsSold"] = dataRow3["imtEstTotalMaterialCost"];
			dataRow3["imtActualUnitMaterialCost"] = num;
			dataRow3["imtActualUnitCostOfGoodsSold"] = num;
			dataRow3["imtActualTotalMaterialCost"] = num * quantity;
			dataRow3["imtActualTotalCostOfGoodsSold"] = num * quantity;
			dataRow3["imtCreatedBy"] = currentUser.ID;
			dataRow3["imtCreatedDate"] = DateTime.Now;
			dataRow3["imtUnitCostAverage"] = dataRow2["AverageCost"];
			dataRow3["imtUnitCostStandard"] = dataRow2["StandardCost"];
			dataRow3["imtUnitCostLast"] = dataRow2["LastCost"];
			currentDatabase.UpdateData(new DataRow[1] { dataRow3 }, adapter, sqlTransaction);
			SqlCommand sqlCommand3 = new SqlCommand("update PartBins set imbBinQuantityOnHand = imbBinQuantityOnHand - @Quantity, imbQuantityOnHand = imbQuantityOnHand - @Quantity where imbPartID = @PartID and imbPartRevisionID = @PartRevisionID and imbWarehouseID = @WarehouseID and imbPartBinID = @BinID");
			sqlCommand3.Parameters.Add(new SqlParameter("Quantity", quantity));
			sqlCommand3.Parameters.Add(new SqlParameter("PartID", partID));
			sqlCommand3.Parameters.Add(new SqlParameter("PartRevisionID", partRevisionID));
			sqlCommand3.Parameters.Add(new SqlParameter("WarehouseID", warehouseID));
			sqlCommand3.Parameters.Add(new SqlParameter("BinID", binID));
			currentDatabase.ExecuteCommand(sqlCommand3, sqlTransaction);
			SqlCommand sqlCommand4 = new SqlCommand("update PartBins set imbQuantityAllocated = imbQuantityAllocated - @Quantity where imbPartID = @PartID and imbPartRevisionID = @PartRevisionID and imbWarehouseID = @WarehouseID and imbPartBinID = @BinID");
			sqlCommand4.Parameters.Add(new SqlParameter("Quantity", quantity));
			sqlCommand4.Parameters.Add(new SqlParameter("PartID", dataRow["jmmPartID"]));
			sqlCommand4.Parameters.Add(new SqlParameter("PartRevisionID", dataRow["jmmPartRevisionID"]));
			sqlCommand4.Parameters.Add(new SqlParameter("WarehouseID", dataRow["jmmPartWarehouseLocationID"]));
			sqlCommand4.Parameters.Add(new SqlParameter("BinID", dataRow["jmmPartBinID"]));
			currentDatabase.ExecuteCommand(sqlCommand4, sqlTransaction);
			SqlCommand sqlCommand5 = new SqlCommand("update PartRevisions set imrQuantityOnHand = imrQuantityOnHand - @Quantity, imrQuantityAllocated = imrQuantityAllocated - @Quantity where imrPartID = @PartID and imrPartRevisionID = @PartRevisionID");
			sqlCommand5.Parameters.Add(new SqlParameter("Quantity", quantity));
			sqlCommand5.Parameters.Add(new SqlParameter("PartID", partID));
			sqlCommand5.Parameters.Add(new SqlParameter("PartRevisionID", partRevisionID));
			currentDatabase.ExecuteCommand(sqlCommand5, sqlTransaction);
			SqlCommand sqlCommand6 = new SqlCommand("update JobMaterials set jmmQuantityReceived = jmmQuantityReceived + @Quantity where jmmJobID = @JobID and jmmJobAssemblyID = @JobAssemblyID and jmmJobMaterialID = @JobMaterialID");
			sqlCommand6.Parameters.Add(new SqlParameter("Quantity", quantity));
			sqlCommand6.Parameters.Add(new SqlParameter("JobID", jobID));
			sqlCommand6.Parameters.Add(new SqlParameter("JobAssemblyID", jobAssemblyID));
			sqlCommand6.Parameters.Add(new SqlParameter("JobMaterialID", jobMaterialID));
			currentDatabase.ExecuteCommand(sqlCommand6, sqlTransaction);
			if (dataRow2.Field<bool>("impTrackLotNumbers"))
			{
				SqlDataAdapter adapter2;
				DataTable dataTable3 = currentDatabase.GetDataTable("select * from LotNumberTransactions where 0=1", fillSchema: false, out adapter2);
				DataRow dataRow4 = dataTable3.AddBlankRow();
				dataRow4["abtLotNumberID"] = lotNumber;
				dataRow4["abtLotNumberTransactionID"] = currentDatabase.NextIDs.GetNextIDForTable("LotNumberTransactions", new object[5] { partID, partRevisionID, warehouseID, binID, lotNumber });
				dataRow4["abtTransactionDate"] = DateTime.Now;
				dataRow4["abtQuantity"] = quantity;
				dataRow4["abtJobID"] = jobID;
				dataRow4["abtJobAssemblyID"] = jobAssemblyID;
				dataRow4["abtJobMaterialID"] = jobMaterialID;
				dataRow4["abtPartTransactionID"] = dataRow3["imtPartTransactionID"];
				dataRow4["abtPartID"] = partID;
				dataRow4["abtPartRevisionID"] = partRevisionID;
				dataRow4["abtPartWarehouseLocationID"] = warehouseID;
				dataRow4["abtPartBinID"] = binID;
				dataRow4["abtTransactionType"] = 4;
				currentDatabase.UpdateData(dataTable3, adapter2, sqlTransaction);
				SqlCommand sqlCommand7 = new SqlCommand("update LotNumbers set ablQuantityOnHand = ablQuantityOnHand - @Quantity where ablPartID = @PartID and ablPartRevisionID = @PartRevisionID and ablPartWarehouseLocationID = @WarehouseID and ablPartBinID = @BinID and ablLotNumberID = @LotNumber");
				sqlCommand7.Parameters.Add(new SqlParameter("@PartID", partID));
				sqlCommand7.Parameters.Add(new SqlParameter("@PartRevisionID", partRevisionID));
				sqlCommand7.Parameters.Add(new SqlParameter("@WarehouseID", warehouseID));
				sqlCommand7.Parameters.Add(new SqlParameter("@BinID", binID));
				sqlCommand7.Parameters.Add(new SqlParameter("@LotNumber", lotNumber));
				sqlCommand7.Parameters.Add(new SqlParameter("@Quantity", quantity));
				currentDatabase.ExecuteCommand(sqlCommand7, sqlTransaction);
			}
			if (dataRow2.Field<bool>("impTrackSerialNumbers"))
			{
				SqlDataAdapter adapter3;
				DataTable dataTable4 = currentDatabase.GetDataTable("select * from SerialNumberTransactions where 0=1", fillSchema: false, out adapter3);
				DataRow dataRow5 = dataTable4.AddBlankRow();
				dataRow5["sntSerialNumberID"] = serialNumber;
				dataRow5["sntSerialNumberTransactionID"] = currentDatabase.NextIDs.GetNextIDForTable("SerialNumberTransactions", new object[3] { partID, partRevisionID, serialNumber });
				dataRow5["sntTransactionDate"] = DateTime.Now;
				dataRow5["sntJobID"] = jobID;
				dataRow5["sntJobAssemblyID"] = jobAssemblyID;
				dataRow5["sntJobMaterialID"] = jobMaterialID;
				dataRow5["sntPartTransactionID"] = dataRow3["imtPartTransactionID"];
				dataRow5["sntPartID"] = partID;
				dataRow5["sntPartRevisionID"] = partRevisionID;
				dataRow5["sntPartWarehouseLocationID"] = warehouseID;
				dataRow5["sntPartBinID"] = binID;
				dataRow5["sntTransactionType"] = 4;
				currentDatabase.UpdateData(dataTable4, adapter3, sqlTransaction);
				SqlCommand sqlCommand8 = new SqlCommand("update SerialNumbers set imsStatus = 1 where imsPartID = @PartID and imsPartRevisionID = @PartRevisionID and imsSerialNumberID = @SerialNumber");
				sqlCommand8.Parameters.Add(new SqlParameter("@PartID", partID));
				sqlCommand8.Parameters.Add(new SqlParameter("@PartRevisionID", partRevisionID));
				sqlCommand8.Parameters.Add(new SqlParameter("@SerialNumber", serialNumber));
				currentDatabase.ExecuteCommand(sqlCommand8, sqlTransaction);
			}
			currentDatabase.CommitTransaction(sqlTransaction);
		}
		catch (Exception ex)
		{
			currentDatabase.RollbackTransaction(sqlTransaction);
			throw ex;
		}
	}

	public void Dispose()
	{
		currentUser = null;
		currentDatabase = null;
	}
}
