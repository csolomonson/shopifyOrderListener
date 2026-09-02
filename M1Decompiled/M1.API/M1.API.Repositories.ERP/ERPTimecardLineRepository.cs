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

public class ERPTimecardLineRepository : APIBaseRepository, IERPTimecardLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPTimecardLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesTimecardLineExist(Guid timecardLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("lmlUniqueID|C", timecardLineId);
		base.selectList.Add("lmlUniqueID");
		return Task.FromResult(GetAsObject("TimecardLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPTimecardLineInformationDto>> GetAllTimecardLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPTimecardLineInformationDto> collection = new List<ERPTimecardLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[44]
		{
			"lmlActualEndTime", "lmlActualStartTime", "lmlCompletionType", "lmlCreatedBy", "lmlCreatedDate", "lmlEmployeeID", "lmlUniqueID", "lmlExpenseID", "lmlGoodQuantity", "lmlIndirectLaborID",
			"lmlActive", "lmlCreatedFromPayrollSession", "lmlLaborHoursCalculated", "lmlMachineHoursCalculated", "lmlPostedToWip", "lmlSuspended", "lmlTransferredToPayroll", "lmlJobAssemblyID", "lmlJobID", "lmlJobOperationID",
			"lmlLaborCost", "lmlLaborDescriptionRtf", "lmlLaborDescriptionText", "lmlLaborHours", "lmlMachineHours", "lmlOverheadCost", "lmlProcessID", "lmlProjectAreaID", "lmlProjectID", "lmlReworkQuantity",
			"lmlReworkReasonID", "lmlRoundedEndTime", "lmlRoundedStartTime", "lmlRowVersion", "lmlScrapQuantity", "lmlScrapReasonID", "lmlTimecardLineID", "lmlSetupPercentCompleted", "lmlShiftID", "lmlSource",
			"lmlTimecardID", "lmlTimecardType", "lmlWorkCenterID", "lmlWorkType"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("TimecardLines");
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
		using (DataTable dataTable = GetAsDataTable("TimecardLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPTimecardLineInformationDto eRPTimecardLineInformationDto = new ERPTimecardLineInformationDto();
				eRPTimecardLineInformationDto.lmlActualEndTime = dataTable.Rows[i].Field<DateTime?>("lmlActualEndTime");
				eRPTimecardLineInformationDto.lmlActualStartTime = dataTable.Rows[i].Field<DateTime?>("lmlActualStartTime");
				eRPTimecardLineInformationDto.lmlCompletionType = dataTable.Rows[i].Field<byte>("lmlCompletionType");
				eRPTimecardLineInformationDto.lmlCreatedBy = dataTable.Rows[i].Field<string>("lmlCreatedBy");
				eRPTimecardLineInformationDto.lmlCreatedDate = dataTable.Rows[i].Field<DateTime?>("lmlCreatedDate");
				eRPTimecardLineInformationDto.lmlEmployeeID = dataTable.Rows[i].Field<string>("lmlEmployeeID");
				eRPTimecardLineInformationDto.lmlUniqueID = dataTable.Rows[i].Field<Guid>("lmlUniqueID");
				eRPTimecardLineInformationDto.lmlExpenseID = dataTable.Rows[i].Field<string>("lmlExpenseID");
				eRPTimecardLineInformationDto.lmlGoodQuantity = dataTable.Rows[i].Field<decimal>("lmlGoodQuantity");
				eRPTimecardLineInformationDto.lmlIndirectLaborID = dataTable.Rows[i].Field<string>("lmlIndirectLaborID");
				eRPTimecardLineInformationDto.lmlActive = dataTable.Rows[i].Field<bool>("lmlActive");
				eRPTimecardLineInformationDto.lmlCreatedFromPayrollSession = dataTable.Rows[i].Field<bool>("lmlCreatedFromPayrollSession");
				eRPTimecardLineInformationDto.lmlLaborHoursCalculated = dataTable.Rows[i].Field<bool>("lmlLaborHoursCalculated");
				eRPTimecardLineInformationDto.lmlMachineHoursCalculated = dataTable.Rows[i].Field<bool>("lmlMachineHoursCalculated");
				eRPTimecardLineInformationDto.lmlPostedToWip = dataTable.Rows[i].Field<bool>("lmlPostedToWip");
				eRPTimecardLineInformationDto.lmlSuspended = dataTable.Rows[i].Field<bool>("lmlSuspended");
				eRPTimecardLineInformationDto.lmlTransferredToPayroll = dataTable.Rows[i].Field<bool>("lmlTransferredToPayroll");
				eRPTimecardLineInformationDto.lmlJobAssemblyID = dataTable.Rows[i].Field<int>("lmlJobAssemblyID");
				eRPTimecardLineInformationDto.lmlJobID = dataTable.Rows[i].Field<string>("lmlJobID");
				eRPTimecardLineInformationDto.lmlJobOperationID = dataTable.Rows[i].Field<int>("lmlJobOperationID");
				eRPTimecardLineInformationDto.lmlLaborCost = dataTable.Rows[i].Field<decimal>("lmlLaborCost");
				eRPTimecardLineInformationDto.lmlLaborDescriptionRtf = dataTable.Rows[i].Field<string>("lmlLaborDescriptionRtf");
				eRPTimecardLineInformationDto.lmlLaborDescriptionText = dataTable.Rows[i].Field<string>("lmlLaborDescriptionText");
				eRPTimecardLineInformationDto.lmlLaborHours = dataTable.Rows[i].Field<decimal>("lmlLaborHours");
				eRPTimecardLineInformationDto.lmlMachineHours = dataTable.Rows[i].Field<decimal>("lmlMachineHours");
				eRPTimecardLineInformationDto.lmlOverheadCost = dataTable.Rows[i].Field<decimal>("lmlOverheadCost");
				eRPTimecardLineInformationDto.lmlProcessID = dataTable.Rows[i].Field<string>("lmlProcessID");
				eRPTimecardLineInformationDto.lmlProjectAreaID = dataTable.Rows[i].Field<string>("lmlProjectAreaID");
				eRPTimecardLineInformationDto.lmlProjectID = dataTable.Rows[i].Field<string>("lmlProjectID");
				eRPTimecardLineInformationDto.lmlReworkQuantity = dataTable.Rows[i].Field<decimal>("lmlReworkQuantity");
				eRPTimecardLineInformationDto.lmlReworkReasonID = dataTable.Rows[i].Field<string>("lmlReworkReasonID");
				eRPTimecardLineInformationDto.lmlRoundedEndTime = dataTable.Rows[i].Field<DateTime?>("lmlRoundedEndTime");
				eRPTimecardLineInformationDto.lmlRoundedStartTime = dataTable.Rows[i].Field<DateTime?>("lmlRoundedStartTime");
				eRPTimecardLineInformationDto.lmlRowVersion = dataTable.Rows[i].Field<byte[]>("lmlRowVersion");
				eRPTimecardLineInformationDto.lmlScrapQuantity = dataTable.Rows[i].Field<decimal>("lmlScrapQuantity");
				eRPTimecardLineInformationDto.lmlScrapReasonID = dataTable.Rows[i].Field<string>("lmlScrapReasonID");
				eRPTimecardLineInformationDto.lmlTimecardLineID = dataTable.Rows[i].Field<short>("lmlTimecardLineID");
				eRPTimecardLineInformationDto.lmlSetupPercentCompleted = dataTable.Rows[i].Field<short>("lmlSetupPercentCompleted");
				eRPTimecardLineInformationDto.lmlShiftID = dataTable.Rows[i].Field<short>("lmlShiftID");
				eRPTimecardLineInformationDto.lmlSource = dataTable.Rows[i].Field<byte>("lmlSource");
				eRPTimecardLineInformationDto.lmlTimecardID = dataTable.Rows[i].Field<int>("lmlTimecardID");
				eRPTimecardLineInformationDto.lmlTimecardType = dataTable.Rows[i].Field<byte>("lmlTimecardType");
				eRPTimecardLineInformationDto.lmlWorkCenterID = dataTable.Rows[i].Field<string>("lmlWorkCenterID");
				eRPTimecardLineInformationDto.lmlWorkType = dataTable.Rows[i].Field<byte>("lmlWorkType");
				eRPTimecardLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPTimecardLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPTimecardLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPTimecardLineInformationDto> GetTimecardLine(Guid timecardLineId)
	{
		ERPTimecardLineInformationDto eRPTimecardLineInformationDto = new ERPTimecardLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[44]
		{
			"lmlActualEndTime", "lmlActualStartTime", "lmlCompletionType", "lmlCreatedBy", "lmlCreatedDate", "lmlEmployeeID", "lmlUniqueID", "lmlExpenseID", "lmlGoodQuantity", "lmlIndirectLaborID",
			"lmlActive", "lmlCreatedFromPayrollSession", "lmlLaborHoursCalculated", "lmlMachineHoursCalculated", "lmlPostedToWip", "lmlSuspended", "lmlTransferredToPayroll", "lmlJobAssemblyID", "lmlJobID", "lmlJobOperationID",
			"lmlLaborCost", "lmlLaborDescriptionRtf", "lmlLaborDescriptionText", "lmlLaborHours", "lmlMachineHours", "lmlOverheadCost", "lmlProcessID", "lmlProjectAreaID", "lmlProjectID", "lmlReworkQuantity",
			"lmlReworkReasonID", "lmlRoundedEndTime", "lmlRoundedStartTime", "lmlRowVersion", "lmlScrapQuantity", "lmlScrapReasonID", "lmlTimecardLineID", "lmlSetupPercentCompleted", "lmlShiftID", "lmlSource",
			"lmlTimecardID", "lmlTimecardType", "lmlWorkCenterID", "lmlWorkType"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("lmlUniqueID|C", timecardLineId);
		AddCustomFieldsToSelectList("TimecardLines");
		using (DataTable dataTable = GetAsDataTable("TimecardLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPTimecardLineInformationDto);
			}
			eRPTimecardLineInformationDto.lmlActualEndTime = dataTable.Rows[0].Field<DateTime?>("lmlActualEndTime");
			eRPTimecardLineInformationDto.lmlActualStartTime = dataTable.Rows[0].Field<DateTime?>("lmlActualStartTime");
			eRPTimecardLineInformationDto.lmlCompletionType = dataTable.Rows[0].Field<byte>("lmlCompletionType");
			eRPTimecardLineInformationDto.lmlCreatedBy = dataTable.Rows[0].Field<string>("lmlCreatedBy");
			eRPTimecardLineInformationDto.lmlCreatedDate = dataTable.Rows[0].Field<DateTime?>("lmlCreatedDate");
			eRPTimecardLineInformationDto.lmlEmployeeID = dataTable.Rows[0].Field<string>("lmlEmployeeID");
			eRPTimecardLineInformationDto.lmlUniqueID = dataTable.Rows[0].Field<Guid>("lmlUniqueID");
			eRPTimecardLineInformationDto.lmlExpenseID = dataTable.Rows[0].Field<string>("lmlExpenseID");
			eRPTimecardLineInformationDto.lmlGoodQuantity = dataTable.Rows[0].Field<decimal>("lmlGoodQuantity");
			eRPTimecardLineInformationDto.lmlIndirectLaborID = dataTable.Rows[0].Field<string>("lmlIndirectLaborID");
			eRPTimecardLineInformationDto.lmlActive = dataTable.Rows[0].Field<bool>("lmlActive");
			eRPTimecardLineInformationDto.lmlCreatedFromPayrollSession = dataTable.Rows[0].Field<bool>("lmlCreatedFromPayrollSession");
			eRPTimecardLineInformationDto.lmlLaborHoursCalculated = dataTable.Rows[0].Field<bool>("lmlLaborHoursCalculated");
			eRPTimecardLineInformationDto.lmlMachineHoursCalculated = dataTable.Rows[0].Field<bool>("lmlMachineHoursCalculated");
			eRPTimecardLineInformationDto.lmlPostedToWip = dataTable.Rows[0].Field<bool>("lmlPostedToWip");
			eRPTimecardLineInformationDto.lmlSuspended = dataTable.Rows[0].Field<bool>("lmlSuspended");
			eRPTimecardLineInformationDto.lmlTransferredToPayroll = dataTable.Rows[0].Field<bool>("lmlTransferredToPayroll");
			eRPTimecardLineInformationDto.lmlJobAssemblyID = dataTable.Rows[0].Field<int>("lmlJobAssemblyID");
			eRPTimecardLineInformationDto.lmlJobID = dataTable.Rows[0].Field<string>("lmlJobID");
			eRPTimecardLineInformationDto.lmlJobOperationID = dataTable.Rows[0].Field<int>("lmlJobOperationID");
			eRPTimecardLineInformationDto.lmlLaborCost = dataTable.Rows[0].Field<decimal>("lmlLaborCost");
			eRPTimecardLineInformationDto.lmlLaborDescriptionRtf = dataTable.Rows[0].Field<string>("lmlLaborDescriptionRtf");
			eRPTimecardLineInformationDto.lmlLaborDescriptionText = dataTable.Rows[0].Field<string>("lmlLaborDescriptionText");
			eRPTimecardLineInformationDto.lmlLaborHours = dataTable.Rows[0].Field<decimal>("lmlLaborHours");
			eRPTimecardLineInformationDto.lmlMachineHours = dataTable.Rows[0].Field<decimal>("lmlMachineHours");
			eRPTimecardLineInformationDto.lmlOverheadCost = dataTable.Rows[0].Field<decimal>("lmlOverheadCost");
			eRPTimecardLineInformationDto.lmlProcessID = dataTable.Rows[0].Field<string>("lmlProcessID");
			eRPTimecardLineInformationDto.lmlProjectAreaID = dataTable.Rows[0].Field<string>("lmlProjectAreaID");
			eRPTimecardLineInformationDto.lmlProjectID = dataTable.Rows[0].Field<string>("lmlProjectID");
			eRPTimecardLineInformationDto.lmlReworkQuantity = dataTable.Rows[0].Field<decimal>("lmlReworkQuantity");
			eRPTimecardLineInformationDto.lmlReworkReasonID = dataTable.Rows[0].Field<string>("lmlReworkReasonID");
			eRPTimecardLineInformationDto.lmlRoundedEndTime = dataTable.Rows[0].Field<DateTime?>("lmlRoundedEndTime");
			eRPTimecardLineInformationDto.lmlRoundedStartTime = dataTable.Rows[0].Field<DateTime?>("lmlRoundedStartTime");
			eRPTimecardLineInformationDto.lmlRowVersion = dataTable.Rows[0].Field<byte[]>("lmlRowVersion");
			eRPTimecardLineInformationDto.lmlScrapQuantity = dataTable.Rows[0].Field<decimal>("lmlScrapQuantity");
			eRPTimecardLineInformationDto.lmlScrapReasonID = dataTable.Rows[0].Field<string>("lmlScrapReasonID");
			eRPTimecardLineInformationDto.lmlTimecardLineID = dataTable.Rows[0].Field<short>("lmlTimecardLineID");
			eRPTimecardLineInformationDto.lmlSetupPercentCompleted = dataTable.Rows[0].Field<short>("lmlSetupPercentCompleted");
			eRPTimecardLineInformationDto.lmlShiftID = dataTable.Rows[0].Field<short>("lmlShiftID");
			eRPTimecardLineInformationDto.lmlSource = dataTable.Rows[0].Field<byte>("lmlSource");
			eRPTimecardLineInformationDto.lmlTimecardID = dataTable.Rows[0].Field<int>("lmlTimecardID");
			eRPTimecardLineInformationDto.lmlTimecardType = dataTable.Rows[0].Field<byte>("lmlTimecardType");
			eRPTimecardLineInformationDto.lmlWorkCenterID = dataTable.Rows[0].Field<string>("lmlWorkCenterID");
			eRPTimecardLineInformationDto.lmlWorkType = dataTable.Rows[0].Field<byte>("lmlWorkType");
			eRPTimecardLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPTimecardLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPTimecardLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveTimecardLine(ERPTimecardLineDto timecardLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM TimecardLines WHERE lmlUniqueID = " + M1Util.ConvertToLinq(timecardLine.lmlUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["lmlTimecardID"] = timecardLine.lmlTimecardID;
				dataRow["lmlTimecardLineID"] = timecardLine.lmlTimecardLineID;
				timecardLine.lmlUniqueID = ((timecardLine.lmlUniqueID == Guid.Empty) ? Guid.NewGuid() : timecardLine.lmlUniqueID);
				dataRow["lmlUniqueID"] = timecardLine.lmlUniqueID;
				dataRow["lmlCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["lmlCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The TimecardLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (timecardLine.lmlRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the TimecardLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["lmlRowVersion"], timecardLine.lmlRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the TimecardLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the TimecardLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? lmlActualEndTime = timecardLine.lmlActualEndTime;
			dataRow2["lmlActualEndTime"] = (lmlActualEndTime.HasValue ? ((object)lmlActualEndTime.GetValueOrDefault()) : dataRow["lmlActualEndTime"]);
			DataRow dataRow3 = dataRow;
			lmlActualEndTime = timecardLine.lmlActualStartTime;
			dataRow3["lmlActualStartTime"] = (lmlActualEndTime.HasValue ? ((object)lmlActualEndTime.GetValueOrDefault()) : dataRow["lmlActualStartTime"]);
			dataRow["lmlCompletionType"] = timecardLine.lmlCompletionType;
			dataRow["lmlEmployeeID"] = timecardLine.lmlEmployeeID;
			dataRow["lmlExpenseID"] = timecardLine.lmlExpenseID;
			dataRow["lmlGoodQuantity"] = timecardLine.lmlGoodQuantity;
			dataRow["lmlIndirectLaborID"] = timecardLine.lmlIndirectLaborID;
			dataRow["lmlActive"] = timecardLine.lmlActive;
			dataRow["lmlCreatedFromPayrollSession"] = timecardLine.lmlCreatedFromPayrollSession;
			dataRow["lmlLaborHoursCalculated"] = timecardLine.lmlLaborHoursCalculated;
			dataRow["lmlMachineHoursCalculated"] = timecardLine.lmlMachineHoursCalculated;
			dataRow["lmlPostedToWip"] = timecardLine.lmlPostedToWip;
			dataRow["lmlSuspended"] = timecardLine.lmlSuspended;
			dataRow["lmlTransferredToPayroll"] = timecardLine.lmlTransferredToPayroll;
			dataRow["lmlJobAssemblyID"] = timecardLine.lmlJobAssemblyID;
			dataRow["lmlJobID"] = timecardLine.lmlJobID;
			dataRow["lmlJobOperationID"] = timecardLine.lmlJobOperationID;
			dataRow["lmlLaborCost"] = timecardLine.lmlLaborCost;
			dataRow["lmlLaborDescriptionRtf"] = timecardLine.lmlLaborDescriptionRtf ?? dataRow["lmlLaborDescriptionRtf"];
			dataRow["lmlLaborDescriptionText"] = timecardLine.lmlLaborDescriptionText ?? dataRow["lmlLaborDescriptionText"];
			dataRow["lmlLaborHours"] = timecardLine.lmlLaborHours;
			dataRow["lmlMachineHours"] = timecardLine.lmlMachineHours;
			dataRow["lmlOverheadCost"] = timecardLine.lmlOverheadCost;
			dataRow["lmlProcessID"] = timecardLine.lmlProcessID;
			dataRow["lmlProjectAreaID"] = timecardLine.lmlProjectAreaID;
			dataRow["lmlProjectID"] = timecardLine.lmlProjectID;
			dataRow["lmlReworkQuantity"] = timecardLine.lmlReworkQuantity;
			dataRow["lmlReworkReasonID"] = timecardLine.lmlReworkReasonID;
			DataRow dataRow4 = dataRow;
			lmlActualEndTime = timecardLine.lmlRoundedEndTime;
			dataRow4["lmlRoundedEndTime"] = (lmlActualEndTime.HasValue ? ((object)lmlActualEndTime.GetValueOrDefault()) : dataRow["lmlRoundedEndTime"]);
			DataRow dataRow5 = dataRow;
			lmlActualEndTime = timecardLine.lmlRoundedStartTime;
			dataRow5["lmlRoundedStartTime"] = (lmlActualEndTime.HasValue ? ((object)lmlActualEndTime.GetValueOrDefault()) : dataRow["lmlRoundedStartTime"]);
			dataRow["lmlScrapQuantity"] = timecardLine.lmlScrapQuantity;
			dataRow["lmlScrapReasonID"] = timecardLine.lmlScrapReasonID;
			dataRow["lmlSetupPercentCompleted"] = timecardLine.lmlSetupPercentCompleted;
			dataRow["lmlShiftID"] = timecardLine.lmlShiftID;
			dataRow["lmlSource"] = timecardLine.lmlSource;
			dataRow["lmlTimecardType"] = timecardLine.lmlTimecardType;
			dataRow["lmlWorkCenterID"] = timecardLine.lmlWorkCenterID;
			dataRow["lmlWorkType"] = timecardLine.lmlWorkType;
			if (timecardLine.CustomFields != null && timecardLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in timecardLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the TimecardLine [{timecardLine.lmlUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the TimecardLine [{timecardLine.lmlUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
