using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Utilities;
using M1.Extensions;

namespace M1.API.Repositories.ERP;

public class ERPTimecardRepository : APIBaseRepository, IERPTimecardRepository, IAPIBaseRepository, IDisposable
{
	public ERPTimecardRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesTimecardExist(Guid timecardId)
	{
		InitializeParameterLists();
		base.filterList.Add("lmpUniqueID|C", timecardId);
		base.selectList.Add("lmpUniqueID");
		return Task.FromResult(GetAsObject("Timecards", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPTimecardInformationDto>> GetAllTimecards(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPTimecardInformationDto> collection = new List<ERPTimecardInformationDto>();
		InitializeParameterLists();
		string[] array = new string[46]
		{
			"lmpActualEndTime", "lmpActualStartTime", "lmpCreatedBy", "lmpCreatedDate", "lmpEmployeeID", "lmpUniqueID", "lmpExchangeID", "lmpActive", "lmpAutoClockedOut", "lmpCreatedFromPayrollSession",
			"lmpPostedToWip", "lmpTransferredToPayroll", "lmpLastEndTime", "lmpLeaveAccrualID", "lmpMachineHours", "lmpNoteRtf", "lmpNoteText", "lmpOtherHours", "lmpOtherPayrollRateID", "lmpOTPeriod1Hours",
			"lmpOTPeriod1PayrollRateID", "lmpOTPeriod2Hours", "lmpOTPeriod2PayrollRateID", "lmpOTPeriod3Hours", "lmpOTPeriod3PayrollRateID", "lmpOTPeriod4Hours", "lmpOTPeriod4PayrollRateID", "lmpPaidDate", "lmpPayrollHours", "lmpPlantDepartmentID",
			"lmpPlantID", "lmpPostedDate", "lmpProjectID", "lmpRoundedEndTime", "lmpRoundedStartTime", "lmpRowVersion", "lmpTimecardID", "lmpShiftBreakID", "lmpShiftID", "lmpSource",
			"lmpStandardHours", "lmpStandardPayrollRateID", "lmpTimecardDate", "lmpTotalPayrollHours", "lmpTransferredDate", "lmpUtcOffset"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Timecards");
		List<string> list = new List<string>();
		string[] fields = ((base.selectList.Count != array.Count()) ? base.selectList.ToArray() : array);
		if (orderBy != null && orderBy.Length > 0)
		{
			ParseAndAddOrderByFields(orderBy, list, fields);
		}
		if (list.Count == 0)
		{
			list = new List<string> { "1" };
		}
		if (filter != null && filter.Length != 0)
		{
			ParseAndAddFilter(filter, base.filterList, fields);
		}
		using (DataTable dataTable = GetAsDataTable("Timecards", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPTimecardInformationDto eRPTimecardInformationDto = new ERPTimecardInformationDto();
				eRPTimecardInformationDto.lmpActualEndTime = dataTable.Rows[i].Field<DateTime?>("lmpActualEndTime");
				eRPTimecardInformationDto.lmpActualStartTime = dataTable.Rows[i].Field<DateTime?>("lmpActualStartTime");
				eRPTimecardInformationDto.lmpCreatedBy = dataTable.Rows[i].Field<string>("lmpCreatedBy");
				eRPTimecardInformationDto.lmpCreatedDate = dataTable.Rows[i].Field<DateTime?>("lmpCreatedDate");
				eRPTimecardInformationDto.lmpEmployeeID = dataTable.Rows[i].Field<string>("lmpEmployeeID");
				eRPTimecardInformationDto.lmpUniqueID = dataTable.Rows[i].Field<Guid>("lmpUniqueID");
				eRPTimecardInformationDto.lmpExchangeID = dataTable.Rows[i].Field<string>("lmpExchangeID");
				eRPTimecardInformationDto.lmpActive = dataTable.Rows[i].Field<bool>("lmpActive");
				eRPTimecardInformationDto.lmpAutoClockedOut = dataTable.Rows[i].Field<bool>("lmpAutoClockedOut");
				eRPTimecardInformationDto.lmpCreatedFromPayrollSession = dataTable.Rows[i].Field<bool>("lmpCreatedFromPayrollSession");
				eRPTimecardInformationDto.lmpPostedToWip = dataTable.Rows[i].Field<bool>("lmpPostedToWip");
				eRPTimecardInformationDto.lmpTransferredToPayroll = dataTable.Rows[i].Field<bool>("lmpTransferredToPayroll");
				eRPTimecardInformationDto.lmpLastEndTime = dataTable.Rows[i].Field<DateTime?>("lmpLastEndTime");
				eRPTimecardInformationDto.lmpLeaveAccrualID = dataTable.Rows[i].Field<string>("lmpLeaveAccrualID");
				eRPTimecardInformationDto.lmpMachineHours = dataTable.Rows[i].Field<decimal>("lmpMachineHours");
				eRPTimecardInformationDto.lmpNoteRtf = dataTable.Rows[i].Field<string>("lmpNoteRtf");
				eRPTimecardInformationDto.lmpNoteText = dataTable.Rows[i].Field<string>("lmpNoteText");
				eRPTimecardInformationDto.lmpOtherHours = dataTable.Rows[i].Field<decimal>("lmpOtherHours");
				eRPTimecardInformationDto.lmpOtherPayrollRateID = dataTable.Rows[i].Field<string>("lmpOtherPayrollRateID");
				eRPTimecardInformationDto.lmpOTPeriod1Hours = dataTable.Rows[i].Field<decimal>("lmpOTPeriod1Hours");
				eRPTimecardInformationDto.lmpOTPeriod1PayrollRateID = dataTable.Rows[i].Field<string>("lmpOTPeriod1PayrollRateID");
				eRPTimecardInformationDto.lmpOTPeriod2Hours = dataTable.Rows[i].Field<decimal>("lmpOTPeriod2Hours");
				eRPTimecardInformationDto.lmpOTPeriod2PayrollRateID = dataTable.Rows[i].Field<string>("lmpOTPeriod2PayrollRateID");
				eRPTimecardInformationDto.lmpOTPeriod3Hours = dataTable.Rows[i].Field<decimal>("lmpOTPeriod3Hours");
				eRPTimecardInformationDto.lmpOTPeriod3PayrollRateID = dataTable.Rows[i].Field<string>("lmpOTPeriod3PayrollRateID");
				eRPTimecardInformationDto.lmpOTPeriod4Hours = dataTable.Rows[i].Field<decimal>("lmpOTPeriod4Hours");
				eRPTimecardInformationDto.lmpOTPeriod4PayrollRateID = dataTable.Rows[i].Field<string>("lmpOTPeriod4PayrollRateID");
				eRPTimecardInformationDto.lmpPaidDate = dataTable.Rows[i].Field<DateTime?>("lmpPaidDate");
				eRPTimecardInformationDto.lmpPayrollHours = dataTable.Rows[i].Field<decimal>("lmpPayrollHours");
				eRPTimecardInformationDto.lmpPlantDepartmentID = dataTable.Rows[i].Field<string>("lmpPlantDepartmentID");
				eRPTimecardInformationDto.lmpPlantID = dataTable.Rows[i].Field<string>("lmpPlantID");
				eRPTimecardInformationDto.lmpPostedDate = dataTable.Rows[i].Field<DateTime?>("lmpPostedDate");
				eRPTimecardInformationDto.lmpProjectID = dataTable.Rows[i].Field<string>("lmpProjectID");
				eRPTimecardInformationDto.lmpRoundedEndTime = dataTable.Rows[i].Field<DateTime?>("lmpRoundedEndTime");
				eRPTimecardInformationDto.lmpRoundedStartTime = dataTable.Rows[i].Field<DateTime?>("lmpRoundedStartTime");
				eRPTimecardInformationDto.lmpRowVersion = dataTable.Rows[i].Field<byte[]>("lmpRowVersion");
				eRPTimecardInformationDto.lmpTimecardID = dataTable.Rows[i].Field<int>("lmpTimecardID");
				eRPTimecardInformationDto.lmpShiftBreakID = dataTable.Rows[i].Field<byte>("lmpShiftBreakID");
				eRPTimecardInformationDto.lmpShiftID = dataTable.Rows[i].Field<short>("lmpShiftID");
				eRPTimecardInformationDto.lmpSource = dataTable.Rows[i].Field<byte>("lmpSource");
				eRPTimecardInformationDto.lmpStandardHours = dataTable.Rows[i].Field<decimal>("lmpStandardHours");
				eRPTimecardInformationDto.lmpStandardPayrollRateID = dataTable.Rows[i].Field<string>("lmpStandardPayrollRateID");
				eRPTimecardInformationDto.lmpTimecardDate = dataTable.Rows[i].Field<DateTime?>("lmpTimecardDate");
				eRPTimecardInformationDto.lmpTotalPayrollHours = dataTable.Rows[i].Field<decimal>("lmpTotalPayrollHours");
				eRPTimecardInformationDto.lmpTransferredDate = dataTable.Rows[i].Field<DateTime?>("lmpTransferredDate");
				eRPTimecardInformationDto.lmpUtcOffset = dataTable.Rows[i].Field<short?>("lmpUtcOffset");
				eRPTimecardInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPTimecardInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPTimecardInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPTimecardInformationDto> GetTimecard(Guid timecardId)
	{
		ERPTimecardInformationDto eRPTimecardInformationDto = new ERPTimecardInformationDto();
		InitializeParameterLists();
		string[] collection = new string[46]
		{
			"lmpActualEndTime", "lmpActualStartTime", "lmpCreatedBy", "lmpCreatedDate", "lmpEmployeeID", "lmpUniqueID", "lmpExchangeID", "lmpActive", "lmpAutoClockedOut", "lmpCreatedFromPayrollSession",
			"lmpPostedToWip", "lmpTransferredToPayroll", "lmpLastEndTime", "lmpLeaveAccrualID", "lmpMachineHours", "lmpNoteRtf", "lmpNoteText", "lmpOtherHours", "lmpOtherPayrollRateID", "lmpOTPeriod1Hours",
			"lmpOTPeriod1PayrollRateID", "lmpOTPeriod2Hours", "lmpOTPeriod2PayrollRateID", "lmpOTPeriod3Hours", "lmpOTPeriod3PayrollRateID", "lmpOTPeriod4Hours", "lmpOTPeriod4PayrollRateID", "lmpPaidDate", "lmpPayrollHours", "lmpPlantDepartmentID",
			"lmpPlantID", "lmpPostedDate", "lmpProjectID", "lmpRoundedEndTime", "lmpRoundedStartTime", "lmpRowVersion", "lmpTimecardID", "lmpShiftBreakID", "lmpShiftID", "lmpSource",
			"lmpStandardHours", "lmpStandardPayrollRateID", "lmpTimecardDate", "lmpTotalPayrollHours", "lmpTransferredDate", "lmpUtcOffset"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("lmpUniqueID|C", timecardId);
		AddCustomFieldsToSelectList("Timecards");
		using (DataTable dataTable = GetAsDataTable("Timecards", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPTimecardInformationDto);
			}
			eRPTimecardInformationDto.lmpActualEndTime = dataTable.Rows[0].Field<DateTime?>("lmpActualEndTime");
			eRPTimecardInformationDto.lmpActualStartTime = dataTable.Rows[0].Field<DateTime?>("lmpActualStartTime");
			eRPTimecardInformationDto.lmpCreatedBy = dataTable.Rows[0].Field<string>("lmpCreatedBy");
			eRPTimecardInformationDto.lmpCreatedDate = dataTable.Rows[0].Field<DateTime?>("lmpCreatedDate");
			eRPTimecardInformationDto.lmpEmployeeID = dataTable.Rows[0].Field<string>("lmpEmployeeID");
			eRPTimecardInformationDto.lmpUniqueID = dataTable.Rows[0].Field<Guid>("lmpUniqueID");
			eRPTimecardInformationDto.lmpExchangeID = dataTable.Rows[0].Field<string>("lmpExchangeID");
			eRPTimecardInformationDto.lmpActive = dataTable.Rows[0].Field<bool>("lmpActive");
			eRPTimecardInformationDto.lmpAutoClockedOut = dataTable.Rows[0].Field<bool>("lmpAutoClockedOut");
			eRPTimecardInformationDto.lmpCreatedFromPayrollSession = dataTable.Rows[0].Field<bool>("lmpCreatedFromPayrollSession");
			eRPTimecardInformationDto.lmpPostedToWip = dataTable.Rows[0].Field<bool>("lmpPostedToWip");
			eRPTimecardInformationDto.lmpTransferredToPayroll = dataTable.Rows[0].Field<bool>("lmpTransferredToPayroll");
			eRPTimecardInformationDto.lmpLastEndTime = dataTable.Rows[0].Field<DateTime?>("lmpLastEndTime");
			eRPTimecardInformationDto.lmpLeaveAccrualID = dataTable.Rows[0].Field<string>("lmpLeaveAccrualID");
			eRPTimecardInformationDto.lmpMachineHours = dataTable.Rows[0].Field<decimal>("lmpMachineHours");
			eRPTimecardInformationDto.lmpNoteRtf = dataTable.Rows[0].Field<string>("lmpNoteRtf");
			eRPTimecardInformationDto.lmpNoteText = dataTable.Rows[0].Field<string>("lmpNoteText");
			eRPTimecardInformationDto.lmpOtherHours = dataTable.Rows[0].Field<decimal>("lmpOtherHours");
			eRPTimecardInformationDto.lmpOtherPayrollRateID = dataTable.Rows[0].Field<string>("lmpOtherPayrollRateID");
			eRPTimecardInformationDto.lmpOTPeriod1Hours = dataTable.Rows[0].Field<decimal>("lmpOTPeriod1Hours");
			eRPTimecardInformationDto.lmpOTPeriod1PayrollRateID = dataTable.Rows[0].Field<string>("lmpOTPeriod1PayrollRateID");
			eRPTimecardInformationDto.lmpOTPeriod2Hours = dataTable.Rows[0].Field<decimal>("lmpOTPeriod2Hours");
			eRPTimecardInformationDto.lmpOTPeriod2PayrollRateID = dataTable.Rows[0].Field<string>("lmpOTPeriod2PayrollRateID");
			eRPTimecardInformationDto.lmpOTPeriod3Hours = dataTable.Rows[0].Field<decimal>("lmpOTPeriod3Hours");
			eRPTimecardInformationDto.lmpOTPeriod3PayrollRateID = dataTable.Rows[0].Field<string>("lmpOTPeriod3PayrollRateID");
			eRPTimecardInformationDto.lmpOTPeriod4Hours = dataTable.Rows[0].Field<decimal>("lmpOTPeriod4Hours");
			eRPTimecardInformationDto.lmpOTPeriod4PayrollRateID = dataTable.Rows[0].Field<string>("lmpOTPeriod4PayrollRateID");
			eRPTimecardInformationDto.lmpPaidDate = dataTable.Rows[0].Field<DateTime?>("lmpPaidDate");
			eRPTimecardInformationDto.lmpPayrollHours = dataTable.Rows[0].Field<decimal>("lmpPayrollHours");
			eRPTimecardInformationDto.lmpPlantDepartmentID = dataTable.Rows[0].Field<string>("lmpPlantDepartmentID");
			eRPTimecardInformationDto.lmpPlantID = dataTable.Rows[0].Field<string>("lmpPlantID");
			eRPTimecardInformationDto.lmpPostedDate = dataTable.Rows[0].Field<DateTime?>("lmpPostedDate");
			eRPTimecardInformationDto.lmpProjectID = dataTable.Rows[0].Field<string>("lmpProjectID");
			eRPTimecardInformationDto.lmpRoundedEndTime = dataTable.Rows[0].Field<DateTime?>("lmpRoundedEndTime");
			eRPTimecardInformationDto.lmpRoundedStartTime = dataTable.Rows[0].Field<DateTime?>("lmpRoundedStartTime");
			eRPTimecardInformationDto.lmpRowVersion = dataTable.Rows[0].Field<byte[]>("lmpRowVersion");
			eRPTimecardInformationDto.lmpTimecardID = dataTable.Rows[0].Field<int>("lmpTimecardID");
			eRPTimecardInformationDto.lmpShiftBreakID = dataTable.Rows[0].Field<byte>("lmpShiftBreakID");
			eRPTimecardInformationDto.lmpShiftID = dataTable.Rows[0].Field<short>("lmpShiftID");
			eRPTimecardInformationDto.lmpSource = dataTable.Rows[0].Field<byte>("lmpSource");
			eRPTimecardInformationDto.lmpStandardHours = dataTable.Rows[0].Field<decimal>("lmpStandardHours");
			eRPTimecardInformationDto.lmpStandardPayrollRateID = dataTable.Rows[0].Field<string>("lmpStandardPayrollRateID");
			eRPTimecardInformationDto.lmpTimecardDate = dataTable.Rows[0].Field<DateTime?>("lmpTimecardDate");
			eRPTimecardInformationDto.lmpTotalPayrollHours = dataTable.Rows[0].Field<decimal>("lmpTotalPayrollHours");
			eRPTimecardInformationDto.lmpTransferredDate = dataTable.Rows[0].Field<DateTime?>("lmpTransferredDate");
			eRPTimecardInformationDto.lmpUtcOffset = dataTable.Rows[0].Field<short?>("lmpUtcOffset");
			eRPTimecardInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPTimecardInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPTimecardInformationDto);
	}

	public Task<APIValidationInfoDto> SaveTimecard(ERPTimecardDto timecard)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Timecards WHERE lmpUniqueID = " + M1Util.ConvertToLinq(timecard.lmpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["lmpTimecardID"] = timecard.lmpTimecardID;
				timecard.lmpUniqueID = ((timecard.lmpUniqueID == Guid.Empty) ? Guid.NewGuid() : timecard.lmpUniqueID);
				dataRow["lmpUniqueID"] = timecard.lmpUniqueID;
				dataRow["lmpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["lmpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Timecard could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (timecard.lmpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Timecard is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["lmpRowVersion"], timecard.lmpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Timecard has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Timecard again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? lmpActualEndTime = timecard.lmpActualEndTime;
			dataRow2["lmpActualEndTime"] = (lmpActualEndTime.HasValue ? ((object)lmpActualEndTime.GetValueOrDefault()) : dataRow["lmpActualEndTime"]);
			DataRow dataRow3 = dataRow;
			lmpActualEndTime = timecard.lmpActualStartTime;
			dataRow3["lmpActualStartTime"] = (lmpActualEndTime.HasValue ? ((object)lmpActualEndTime.GetValueOrDefault()) : dataRow["lmpActualStartTime"]);
			dataRow["lmpEmployeeID"] = timecard.lmpEmployeeID;
			dataRow["lmpExchangeID"] = timecard.lmpExchangeID ?? dataRow["lmpExchangeID"];
			dataRow["lmpActive"] = timecard.lmpActive;
			dataRow["lmpAutoClockedOut"] = timecard.lmpAutoClockedOut;
			dataRow["lmpCreatedFromPayrollSession"] = timecard.lmpCreatedFromPayrollSession;
			dataRow["lmpPostedToWip"] = timecard.lmpPostedToWip;
			dataRow["lmpTransferredToPayroll"] = timecard.lmpTransferredToPayroll;
			DataRow dataRow4 = dataRow;
			lmpActualEndTime = timecard.lmpLastEndTime;
			dataRow4["lmpLastEndTime"] = (lmpActualEndTime.HasValue ? ((object)lmpActualEndTime.GetValueOrDefault()) : dataRow["lmpLastEndTime"]);
			dataRow["lmpLeaveAccrualID"] = timecard.lmpLeaveAccrualID;
			dataRow["lmpMachineHours"] = timecard.lmpMachineHours;
			dataRow["lmpNoteRtf"] = timecard.lmpNoteRtf ?? dataRow["lmpNoteRtf"];
			dataRow["lmpNoteText"] = timecard.lmpNoteText ?? dataRow["lmpNoteText"];
			dataRow["lmpOtherHours"] = timecard.lmpOtherHours;
			dataRow["lmpOtherPayrollRateID"] = timecard.lmpOtherPayrollRateID;
			dataRow["lmpOTPeriod1Hours"] = timecard.lmpOTPeriod1Hours;
			dataRow["lmpOTPeriod1PayrollRateID"] = timecard.lmpOTPeriod1PayrollRateID;
			dataRow["lmpOTPeriod2Hours"] = timecard.lmpOTPeriod2Hours;
			dataRow["lmpOTPeriod2PayrollRateID"] = timecard.lmpOTPeriod2PayrollRateID;
			dataRow["lmpOTPeriod3Hours"] = timecard.lmpOTPeriod3Hours;
			dataRow["lmpOTPeriod3PayrollRateID"] = timecard.lmpOTPeriod3PayrollRateID;
			dataRow["lmpOTPeriod4Hours"] = timecard.lmpOTPeriod4Hours;
			dataRow["lmpOTPeriod4PayrollRateID"] = timecard.lmpOTPeriod4PayrollRateID;
			DataRow dataRow5 = dataRow;
			lmpActualEndTime = timecard.lmpPaidDate;
			dataRow5["lmpPaidDate"] = (lmpActualEndTime.HasValue ? ((object)lmpActualEndTime.GetValueOrDefault()) : dataRow["lmpPaidDate"]);
			dataRow["lmpPayrollHours"] = timecard.lmpPayrollHours;
			dataRow["lmpPlantDepartmentID"] = timecard.lmpPlantDepartmentID;
			dataRow["lmpPlantID"] = timecard.lmpPlantID;
			DataRow dataRow6 = dataRow;
			lmpActualEndTime = timecard.lmpPostedDate;
			dataRow6["lmpPostedDate"] = (lmpActualEndTime.HasValue ? ((object)lmpActualEndTime.GetValueOrDefault()) : dataRow["lmpPostedDate"]);
			dataRow["lmpProjectID"] = timecard.lmpProjectID;
			DataRow dataRow7 = dataRow;
			lmpActualEndTime = timecard.lmpRoundedEndTime;
			dataRow7["lmpRoundedEndTime"] = (lmpActualEndTime.HasValue ? ((object)lmpActualEndTime.GetValueOrDefault()) : dataRow["lmpRoundedEndTime"]);
			DataRow dataRow8 = dataRow;
			lmpActualEndTime = timecard.lmpRoundedStartTime;
			dataRow8["lmpRoundedStartTime"] = (lmpActualEndTime.HasValue ? ((object)lmpActualEndTime.GetValueOrDefault()) : dataRow["lmpRoundedStartTime"]);
			dataRow["lmpShiftBreakID"] = timecard.lmpShiftBreakID;
			dataRow["lmpShiftID"] = timecard.lmpShiftID;
			dataRow["lmpSource"] = timecard.lmpSource;
			dataRow["lmpStandardHours"] = timecard.lmpStandardHours;
			dataRow["lmpStandardPayrollRateID"] = timecard.lmpStandardPayrollRateID;
			DataRow dataRow9 = dataRow;
			lmpActualEndTime = timecard.lmpTimecardDate;
			dataRow9["lmpTimecardDate"] = (lmpActualEndTime.HasValue ? ((object)lmpActualEndTime.GetValueOrDefault()) : dataRow["lmpTimecardDate"]);
			dataRow["lmpTotalPayrollHours"] = timecard.lmpTotalPayrollHours;
			DataRow dataRow10 = dataRow;
			lmpActualEndTime = timecard.lmpTransferredDate;
			dataRow10["lmpTransferredDate"] = (lmpActualEndTime.HasValue ? ((object)lmpActualEndTime.GetValueOrDefault()) : dataRow["lmpTransferredDate"]);
			DataRow dataRow11 = dataRow;
			short? lmpUtcOffset = timecard.lmpUtcOffset;
			dataRow11["lmpUtcOffset"] = (lmpUtcOffset.HasValue ? ((object)lmpUtcOffset.GetValueOrDefault()) : dataRow["lmpUtcOffset"]);
			if (timecard.CustomFields != null && timecard.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in timecard.CustomFields)
				{
					if (dataTable.Columns.Contains(customField.Key))
					{
						dataRow[customField.Key] = customField.Value;
					}
				}
			}
			dataRow.EndEdit();
			if (flag)
			{
				dataTable.Rows.Add(dataRow);
			}
			if (base.M1database.UpdateData(dataTable, adapter))
			{
				if (flag)
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Created;
				}
				else
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.OK;
				}
			}
			else
			{
				aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.InternalServerError;
			}
		}
		catch (SqlException ex)
		{
			SqlErrorResult httpStatusCodeForSqlException = SqlExceptionMapper.GetHttpStatusCodeForSqlException(ex);
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Timecard [{timecard.lmpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Timecard [{timecard.lmpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
