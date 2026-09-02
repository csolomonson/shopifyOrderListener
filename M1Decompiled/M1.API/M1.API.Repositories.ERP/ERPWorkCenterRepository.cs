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

public class ERPWorkCenterRepository : APIBaseRepository, IERPWorkCenterRepository, IAPIBaseRepository, IDisposable
{
	public ERPWorkCenterRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesWorkCenterExist(Guid workCenterId)
	{
		InitializeParameterLists();
		base.filterList.Add("xawUniqueID|C", workCenterId);
		base.selectList.Add("xawUniqueID");
		return Task.FromResult(GetAsObject("WorkCenters", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPWorkCenterInformationDto>> GetAllWorkCenters(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPWorkCenterInformationDto> collection = new List<ERPWorkCenterInformationDto>();
		InitializeParameterLists();
		string[] array = new string[47]
		{
			"xawCalendarColor", "xawCalendarLocation", "xawWorkCenterID", "xawCreatedBy", "xawCreatedDate", "xawDayStartTimeFri", "xawDayStartTimeMon", "xawDayStartTimeSat", "xawDayStartTimeSun", "xawDayStartTimeThu",
			"xawDayStartTimeTue", "xawDayStartTimeWed", "xawDescription", "xawUniqueID", "xawFiniteTolerance", "xawHoursFri", "xawHoursMon", "xawHoursSat", "xawHoursSun", "xawHoursThu",
			"xawHoursTue", "xawHoursWed", "xawInactiveDate", "xawInactive", "xawEnableCalendar", "xawExcludeFromShopLoad", "xawExportToCalendar", "xawInfiniteCapacity", "xawOutsideProcessing", "xawSetMachineToLaborHours",
			"xawSplitMachineHours", "xawMoveTime", "xawNumberOfMachines", "xawOverheadCalculationType", "xawOverheadRate", "xawPeoplePerMachineProd", "xawPeoplePerMachineSetup", "xawPlantID", "xawProcessID", "xawProductionDepartmentID",
			"xawProductionStandard", "xawQueueTime", "xawQuotingRate", "xawRowVersion", "xawSetupHours", "xawStandardFactor", "xawStartHour"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("WorkCenters");
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
		using (DataTable dataTable = GetAsDataTable("WorkCenters", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPWorkCenterInformationDto eRPWorkCenterInformationDto = new ERPWorkCenterInformationDto();
				eRPWorkCenterInformationDto.xawCalendarColor = dataTable.Rows[i].Field<byte>("xawCalendarColor");
				eRPWorkCenterInformationDto.xawCalendarLocation = dataTable.Rows[i].Field<string>("xawCalendarLocation");
				eRPWorkCenterInformationDto.xawWorkCenterID = dataTable.Rows[i].Field<string>("xawWorkCenterID");
				eRPWorkCenterInformationDto.xawCreatedBy = dataTable.Rows[i].Field<string>("xawCreatedBy");
				eRPWorkCenterInformationDto.xawCreatedDate = dataTable.Rows[i].Field<DateTime?>("xawCreatedDate");
				eRPWorkCenterInformationDto.xawDayStartTimeFri = dataTable.Rows[i].Field<decimal>("xawDayStartTimeFri");
				eRPWorkCenterInformationDto.xawDayStartTimeMon = dataTable.Rows[i].Field<decimal>("xawDayStartTimeMon");
				eRPWorkCenterInformationDto.xawDayStartTimeSat = dataTable.Rows[i].Field<decimal>("xawDayStartTimeSat");
				eRPWorkCenterInformationDto.xawDayStartTimeSun = dataTable.Rows[i].Field<decimal>("xawDayStartTimeSun");
				eRPWorkCenterInformationDto.xawDayStartTimeThu = dataTable.Rows[i].Field<decimal>("xawDayStartTimeThu");
				eRPWorkCenterInformationDto.xawDayStartTimeTue = dataTable.Rows[i].Field<decimal>("xawDayStartTimeTue");
				eRPWorkCenterInformationDto.xawDayStartTimeWed = dataTable.Rows[i].Field<decimal>("xawDayStartTimeWed");
				eRPWorkCenterInformationDto.xawDescription = dataTable.Rows[i].Field<string>("xawDescription");
				eRPWorkCenterInformationDto.xawUniqueID = dataTable.Rows[i].Field<Guid>("xawUniqueID");
				eRPWorkCenterInformationDto.xawFiniteTolerance = dataTable.Rows[i].Field<decimal>("xawFiniteTolerance");
				eRPWorkCenterInformationDto.xawHoursFri = dataTable.Rows[i].Field<decimal>("xawHoursFri");
				eRPWorkCenterInformationDto.xawHoursMon = dataTable.Rows[i].Field<decimal>("xawHoursMon");
				eRPWorkCenterInformationDto.xawHoursSat = dataTable.Rows[i].Field<decimal>("xawHoursSat");
				eRPWorkCenterInformationDto.xawHoursSun = dataTable.Rows[i].Field<decimal>("xawHoursSun");
				eRPWorkCenterInformationDto.xawHoursThu = dataTable.Rows[i].Field<decimal>("xawHoursThu");
				eRPWorkCenterInformationDto.xawHoursTue = dataTable.Rows[i].Field<decimal>("xawHoursTue");
				eRPWorkCenterInformationDto.xawHoursWed = dataTable.Rows[i].Field<decimal>("xawHoursWed");
				eRPWorkCenterInformationDto.xawInactiveDate = dataTable.Rows[i].Field<DateTime?>("xawInactiveDate");
				eRPWorkCenterInformationDto.xawInactive = dataTable.Rows[i].Field<bool>("xawInactive");
				eRPWorkCenterInformationDto.xawEnableCalendar = dataTable.Rows[i].Field<bool>("xawEnableCalendar");
				eRPWorkCenterInformationDto.xawExcludeFromShopLoad = dataTable.Rows[i].Field<bool>("xawExcludeFromShopLoad");
				eRPWorkCenterInformationDto.xawExportToCalendar = dataTable.Rows[i].Field<bool>("xawExportToCalendar");
				eRPWorkCenterInformationDto.xawInfiniteCapacity = dataTable.Rows[i].Field<bool>("xawInfiniteCapacity");
				eRPWorkCenterInformationDto.xawOutsideProcessing = dataTable.Rows[i].Field<bool>("xawOutsideProcessing");
				eRPWorkCenterInformationDto.xawSetMachineToLaborHours = dataTable.Rows[i].Field<bool>("xawSetMachineToLaborHours");
				eRPWorkCenterInformationDto.xawSplitMachineHours = dataTable.Rows[i].Field<bool>("xawSplitMachineHours");
				eRPWorkCenterInformationDto.xawMoveTime = dataTable.Rows[i].Field<decimal>("xawMoveTime");
				eRPWorkCenterInformationDto.xawNumberOfMachines = dataTable.Rows[i].Field<short>("xawNumberOfMachines");
				eRPWorkCenterInformationDto.xawOverheadCalculationType = dataTable.Rows[i].Field<byte>("xawOverheadCalculationType");
				eRPWorkCenterInformationDto.xawOverheadRate = dataTable.Rows[i].Field<decimal>("xawOverheadRate");
				eRPWorkCenterInformationDto.xawPeoplePerMachineProd = dataTable.Rows[i].Field<short>("xawPeoplePerMachineProd");
				eRPWorkCenterInformationDto.xawPeoplePerMachineSetup = dataTable.Rows[i].Field<short>("xawPeoplePerMachineSetup");
				eRPWorkCenterInformationDto.xawPlantID = dataTable.Rows[i].Field<string>("xawPlantID");
				eRPWorkCenterInformationDto.xawProcessID = dataTable.Rows[i].Field<string>("xawProcessID");
				eRPWorkCenterInformationDto.xawProductionDepartmentID = dataTable.Rows[i].Field<string>("xawProductionDepartmentID");
				eRPWorkCenterInformationDto.xawProductionStandard = dataTable.Rows[i].Field<decimal>("xawProductionStandard");
				eRPWorkCenterInformationDto.xawQueueTime = dataTable.Rows[i].Field<decimal>("xawQueueTime");
				eRPWorkCenterInformationDto.xawQuotingRate = dataTable.Rows[i].Field<decimal>("xawQuotingRate");
				eRPWorkCenterInformationDto.xawRowVersion = dataTable.Rows[i].Field<byte[]>("xawRowVersion");
				eRPWorkCenterInformationDto.xawSetupHours = dataTable.Rows[i].Field<decimal>("xawSetupHours");
				eRPWorkCenterInformationDto.xawStandardFactor = dataTable.Rows[i].Field<string>("xawStandardFactor");
				eRPWorkCenterInformationDto.xawStartHour = dataTable.Rows[i].Field<decimal>("xawStartHour");
				eRPWorkCenterInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPWorkCenterInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPWorkCenterInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPWorkCenterInformationDto> GetWorkCenter(Guid workCenterId)
	{
		ERPWorkCenterInformationDto eRPWorkCenterInformationDto = new ERPWorkCenterInformationDto();
		InitializeParameterLists();
		string[] collection = new string[47]
		{
			"xawCalendarColor", "xawCalendarLocation", "xawWorkCenterID", "xawCreatedBy", "xawCreatedDate", "xawDayStartTimeFri", "xawDayStartTimeMon", "xawDayStartTimeSat", "xawDayStartTimeSun", "xawDayStartTimeThu",
			"xawDayStartTimeTue", "xawDayStartTimeWed", "xawDescription", "xawUniqueID", "xawFiniteTolerance", "xawHoursFri", "xawHoursMon", "xawHoursSat", "xawHoursSun", "xawHoursThu",
			"xawHoursTue", "xawHoursWed", "xawInactiveDate", "xawInactive", "xawEnableCalendar", "xawExcludeFromShopLoad", "xawExportToCalendar", "xawInfiniteCapacity", "xawOutsideProcessing", "xawSetMachineToLaborHours",
			"xawSplitMachineHours", "xawMoveTime", "xawNumberOfMachines", "xawOverheadCalculationType", "xawOverheadRate", "xawPeoplePerMachineProd", "xawPeoplePerMachineSetup", "xawPlantID", "xawProcessID", "xawProductionDepartmentID",
			"xawProductionStandard", "xawQueueTime", "xawQuotingRate", "xawRowVersion", "xawSetupHours", "xawStandardFactor", "xawStartHour"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xawUniqueID|C", workCenterId);
		AddCustomFieldsToSelectList("WorkCenters");
		using (DataTable dataTable = GetAsDataTable("WorkCenters", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPWorkCenterInformationDto);
			}
			eRPWorkCenterInformationDto.xawCalendarColor = dataTable.Rows[0].Field<byte>("xawCalendarColor");
			eRPWorkCenterInformationDto.xawCalendarLocation = dataTable.Rows[0].Field<string>("xawCalendarLocation");
			eRPWorkCenterInformationDto.xawWorkCenterID = dataTable.Rows[0].Field<string>("xawWorkCenterID");
			eRPWorkCenterInformationDto.xawCreatedBy = dataTable.Rows[0].Field<string>("xawCreatedBy");
			eRPWorkCenterInformationDto.xawCreatedDate = dataTable.Rows[0].Field<DateTime?>("xawCreatedDate");
			eRPWorkCenterInformationDto.xawDayStartTimeFri = dataTable.Rows[0].Field<decimal>("xawDayStartTimeFri");
			eRPWorkCenterInformationDto.xawDayStartTimeMon = dataTable.Rows[0].Field<decimal>("xawDayStartTimeMon");
			eRPWorkCenterInformationDto.xawDayStartTimeSat = dataTable.Rows[0].Field<decimal>("xawDayStartTimeSat");
			eRPWorkCenterInformationDto.xawDayStartTimeSun = dataTable.Rows[0].Field<decimal>("xawDayStartTimeSun");
			eRPWorkCenterInformationDto.xawDayStartTimeThu = dataTable.Rows[0].Field<decimal>("xawDayStartTimeThu");
			eRPWorkCenterInformationDto.xawDayStartTimeTue = dataTable.Rows[0].Field<decimal>("xawDayStartTimeTue");
			eRPWorkCenterInformationDto.xawDayStartTimeWed = dataTable.Rows[0].Field<decimal>("xawDayStartTimeWed");
			eRPWorkCenterInformationDto.xawDescription = dataTable.Rows[0].Field<string>("xawDescription");
			eRPWorkCenterInformationDto.xawUniqueID = dataTable.Rows[0].Field<Guid>("xawUniqueID");
			eRPWorkCenterInformationDto.xawFiniteTolerance = dataTable.Rows[0].Field<decimal>("xawFiniteTolerance");
			eRPWorkCenterInformationDto.xawHoursFri = dataTable.Rows[0].Field<decimal>("xawHoursFri");
			eRPWorkCenterInformationDto.xawHoursMon = dataTable.Rows[0].Field<decimal>("xawHoursMon");
			eRPWorkCenterInformationDto.xawHoursSat = dataTable.Rows[0].Field<decimal>("xawHoursSat");
			eRPWorkCenterInformationDto.xawHoursSun = dataTable.Rows[0].Field<decimal>("xawHoursSun");
			eRPWorkCenterInformationDto.xawHoursThu = dataTable.Rows[0].Field<decimal>("xawHoursThu");
			eRPWorkCenterInformationDto.xawHoursTue = dataTable.Rows[0].Field<decimal>("xawHoursTue");
			eRPWorkCenterInformationDto.xawHoursWed = dataTable.Rows[0].Field<decimal>("xawHoursWed");
			eRPWorkCenterInformationDto.xawInactiveDate = dataTable.Rows[0].Field<DateTime?>("xawInactiveDate");
			eRPWorkCenterInformationDto.xawInactive = dataTable.Rows[0].Field<bool>("xawInactive");
			eRPWorkCenterInformationDto.xawEnableCalendar = dataTable.Rows[0].Field<bool>("xawEnableCalendar");
			eRPWorkCenterInformationDto.xawExcludeFromShopLoad = dataTable.Rows[0].Field<bool>("xawExcludeFromShopLoad");
			eRPWorkCenterInformationDto.xawExportToCalendar = dataTable.Rows[0].Field<bool>("xawExportToCalendar");
			eRPWorkCenterInformationDto.xawInfiniteCapacity = dataTable.Rows[0].Field<bool>("xawInfiniteCapacity");
			eRPWorkCenterInformationDto.xawOutsideProcessing = dataTable.Rows[0].Field<bool>("xawOutsideProcessing");
			eRPWorkCenterInformationDto.xawSetMachineToLaborHours = dataTable.Rows[0].Field<bool>("xawSetMachineToLaborHours");
			eRPWorkCenterInformationDto.xawSplitMachineHours = dataTable.Rows[0].Field<bool>("xawSplitMachineHours");
			eRPWorkCenterInformationDto.xawMoveTime = dataTable.Rows[0].Field<decimal>("xawMoveTime");
			eRPWorkCenterInformationDto.xawNumberOfMachines = dataTable.Rows[0].Field<short>("xawNumberOfMachines");
			eRPWorkCenterInformationDto.xawOverheadCalculationType = dataTable.Rows[0].Field<byte>("xawOverheadCalculationType");
			eRPWorkCenterInformationDto.xawOverheadRate = dataTable.Rows[0].Field<decimal>("xawOverheadRate");
			eRPWorkCenterInformationDto.xawPeoplePerMachineProd = dataTable.Rows[0].Field<short>("xawPeoplePerMachineProd");
			eRPWorkCenterInformationDto.xawPeoplePerMachineSetup = dataTable.Rows[0].Field<short>("xawPeoplePerMachineSetup");
			eRPWorkCenterInformationDto.xawPlantID = dataTable.Rows[0].Field<string>("xawPlantID");
			eRPWorkCenterInformationDto.xawProcessID = dataTable.Rows[0].Field<string>("xawProcessID");
			eRPWorkCenterInformationDto.xawProductionDepartmentID = dataTable.Rows[0].Field<string>("xawProductionDepartmentID");
			eRPWorkCenterInformationDto.xawProductionStandard = dataTable.Rows[0].Field<decimal>("xawProductionStandard");
			eRPWorkCenterInformationDto.xawQueueTime = dataTable.Rows[0].Field<decimal>("xawQueueTime");
			eRPWorkCenterInformationDto.xawQuotingRate = dataTable.Rows[0].Field<decimal>("xawQuotingRate");
			eRPWorkCenterInformationDto.xawRowVersion = dataTable.Rows[0].Field<byte[]>("xawRowVersion");
			eRPWorkCenterInformationDto.xawSetupHours = dataTable.Rows[0].Field<decimal>("xawSetupHours");
			eRPWorkCenterInformationDto.xawStandardFactor = dataTable.Rows[0].Field<string>("xawStandardFactor");
			eRPWorkCenterInformationDto.xawStartHour = dataTable.Rows[0].Field<decimal>("xawStartHour");
			eRPWorkCenterInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPWorkCenterInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPWorkCenterInformationDto);
	}

	public Task<APIValidationInfoDto> SaveWorkCenter(ERPWorkCenterDto workCenter)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM WorkCenters WHERE xawUniqueID = " + M1Util.ConvertToLinq(workCenter.xawUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xawWorkCenterID"] = workCenter.xawWorkCenterID.ToUpper();
				workCenter.xawUniqueID = ((workCenter.xawUniqueID == Guid.Empty) ? Guid.NewGuid() : workCenter.xawUniqueID);
				dataRow["xawUniqueID"] = workCenter.xawUniqueID;
				dataRow["xawCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xawCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The WorkCenter could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (workCenter.xawRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the WorkCenter is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xawRowVersion"], workCenter.xawRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the WorkCenter has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the WorkCenter again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xawCalendarColor"] = workCenter.xawCalendarColor;
			dataRow["xawCalendarLocation"] = workCenter.xawCalendarLocation ?? dataRow["xawCalendarLocation"];
			dataRow["xawDayStartTimeFri"] = workCenter.xawDayStartTimeFri;
			dataRow["xawDayStartTimeMon"] = workCenter.xawDayStartTimeMon;
			dataRow["xawDayStartTimeSat"] = workCenter.xawDayStartTimeSat;
			dataRow["xawDayStartTimeSun"] = workCenter.xawDayStartTimeSun;
			dataRow["xawDayStartTimeThu"] = workCenter.xawDayStartTimeThu;
			dataRow["xawDayStartTimeTue"] = workCenter.xawDayStartTimeTue;
			dataRow["xawDayStartTimeWed"] = workCenter.xawDayStartTimeWed;
			dataRow["xawDescription"] = workCenter.xawDescription;
			dataRow["xawFiniteTolerance"] = workCenter.xawFiniteTolerance;
			dataRow["xawHoursFri"] = workCenter.xawHoursFri;
			dataRow["xawHoursMon"] = workCenter.xawHoursMon;
			dataRow["xawHoursSat"] = workCenter.xawHoursSat;
			dataRow["xawHoursSun"] = workCenter.xawHoursSun;
			dataRow["xawHoursThu"] = workCenter.xawHoursThu;
			dataRow["xawHoursTue"] = workCenter.xawHoursTue;
			dataRow["xawHoursWed"] = workCenter.xawHoursWed;
			DataRow dataRow2 = dataRow;
			DateTime? xawInactiveDate = workCenter.xawInactiveDate;
			dataRow2["xawInactiveDate"] = (xawInactiveDate.HasValue ? ((object)xawInactiveDate.GetValueOrDefault()) : dataRow["xawInactiveDate"]);
			dataRow["xawInactive"] = workCenter.xawInactive;
			dataRow["xawEnableCalendar"] = workCenter.xawEnableCalendar;
			dataRow["xawExcludeFromShopLoad"] = workCenter.xawExcludeFromShopLoad;
			dataRow["xawExportToCalendar"] = workCenter.xawExportToCalendar;
			dataRow["xawInfiniteCapacity"] = workCenter.xawInfiniteCapacity;
			dataRow["xawOutsideProcessing"] = workCenter.xawOutsideProcessing;
			dataRow["xawSetMachineToLaborHours"] = workCenter.xawSetMachineToLaborHours;
			dataRow["xawSplitMachineHours"] = workCenter.xawSplitMachineHours;
			dataRow["xawMoveTime"] = workCenter.xawMoveTime;
			dataRow["xawNumberOfMachines"] = workCenter.xawNumberOfMachines;
			dataRow["xawOverheadCalculationType"] = workCenter.xawOverheadCalculationType;
			dataRow["xawOverheadRate"] = workCenter.xawOverheadRate;
			dataRow["xawPeoplePerMachineProd"] = workCenter.xawPeoplePerMachineProd;
			dataRow["xawPeoplePerMachineSetup"] = workCenter.xawPeoplePerMachineSetup;
			dataRow["xawPlantID"] = workCenter.xawPlantID;
			dataRow["xawProcessID"] = workCenter.xawProcessID;
			dataRow["xawProductionDepartmentID"] = workCenter.xawProductionDepartmentID;
			dataRow["xawProductionStandard"] = workCenter.xawProductionStandard;
			dataRow["xawQueueTime"] = workCenter.xawQueueTime;
			dataRow["xawQuotingRate"] = workCenter.xawQuotingRate;
			dataRow["xawSetupHours"] = workCenter.xawSetupHours;
			dataRow["xawStandardFactor"] = workCenter.xawStandardFactor;
			dataRow["xawStartHour"] = workCenter.xawStartHour;
			if (workCenter.CustomFields != null && workCenter.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in workCenter.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the WorkCenter [{workCenter.xawUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the WorkCenter [{workCenter.xawUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
