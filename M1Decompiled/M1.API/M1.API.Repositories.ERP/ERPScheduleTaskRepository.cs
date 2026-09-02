using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPScheduleTaskRepository : APIBaseRepository, IERPScheduleTaskRepository, IAPIBaseRepository, IDisposable
{
	public ERPScheduleTaskRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesScheduleTaskExist(Guid scheduleTaskId)
	{
		InitializeParameterLists();
		base.filterList.Add("sxkUniqueID|C", scheduleTaskId);
		base.selectList.Add("sxkUniqueID");
		return Task.FromResult(GetAsObject("ScheduleTasks", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPScheduleTaskInformationDto>> GetAllScheduleTasks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPScheduleTaskInformationDto> collection = new List<ERPScheduleTaskInformationDto>();
		InitializeParameterLists();
		string[] array = new string[23]
		{
			"sxkCreatedBy", "sxkCreatedDate", "sxkCurrentTaskDateType", "sxkEndActualDateTime", "sxkEndDate", "sxkEndMinute", "sxkUniqueID", "sxkExchangeID", "sxkLinkedTaskDateType", "sxkLinkedTaskID",
			"sxkMinutes", "sxkOffsetMinutes", "sxkPlantDepartmentID", "sxkPlantID", "sxkProcessID", "sxkRowVersion", "sxkScheduleBranchID", "sxkScheduleTreeID", "sxkScheduleTypeID", "sxkScheduleTaskID",
			"sxkStartActualDateTime", "sxkStartDate", "sxkStartMinute"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ScheduleTasks");
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
		using (DataTable dataTable = GetAsDataTable("ScheduleTasks", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPScheduleTaskInformationDto eRPScheduleTaskInformationDto = new ERPScheduleTaskInformationDto();
				eRPScheduleTaskInformationDto.sxkCreatedBy = dataTable.Rows[i].Field<string>("sxkCreatedBy");
				eRPScheduleTaskInformationDto.sxkCreatedDate = dataTable.Rows[i].Field<DateTime?>("sxkCreatedDate");
				eRPScheduleTaskInformationDto.sxkCurrentTaskDateType = dataTable.Rows[i].Field<byte>("sxkCurrentTaskDateType");
				eRPScheduleTaskInformationDto.sxkEndActualDateTime = dataTable.Rows[i].Field<DateTime?>("sxkEndActualDateTime");
				eRPScheduleTaskInformationDto.sxkEndDate = dataTable.Rows[i].Field<DateTime?>("sxkEndDate");
				eRPScheduleTaskInformationDto.sxkEndMinute = dataTable.Rows[i].Field<short>("sxkEndMinute");
				eRPScheduleTaskInformationDto.sxkUniqueID = dataTable.Rows[i].Field<Guid>("sxkUniqueID");
				eRPScheduleTaskInformationDto.sxkExchangeID = dataTable.Rows[i].Field<string>("sxkExchangeID");
				eRPScheduleTaskInformationDto.sxkLinkedTaskDateType = dataTable.Rows[i].Field<byte>("sxkLinkedTaskDateType");
				eRPScheduleTaskInformationDto.sxkLinkedTaskID = dataTable.Rows[i].Field<int>("sxkLinkedTaskID");
				eRPScheduleTaskInformationDto.sxkMinutes = dataTable.Rows[i].Field<int>("sxkMinutes");
				eRPScheduleTaskInformationDto.sxkOffsetMinutes = dataTable.Rows[i].Field<int>("sxkOffsetMinutes");
				eRPScheduleTaskInformationDto.sxkPlantDepartmentID = dataTable.Rows[i].Field<string>("sxkPlantDepartmentID");
				eRPScheduleTaskInformationDto.sxkPlantID = dataTable.Rows[i].Field<string>("sxkPlantID");
				eRPScheduleTaskInformationDto.sxkProcessID = dataTable.Rows[i].Field<string>("sxkProcessID");
				eRPScheduleTaskInformationDto.sxkRowVersion = dataTable.Rows[i].Field<byte[]>("sxkRowVersion");
				eRPScheduleTaskInformationDto.sxkScheduleBranchID = dataTable.Rows[i].Field<int>("sxkScheduleBranchID");
				eRPScheduleTaskInformationDto.sxkScheduleTreeID = dataTable.Rows[i].Field<int>("sxkScheduleTreeID");
				eRPScheduleTaskInformationDto.sxkScheduleTypeID = dataTable.Rows[i].Field<byte>("sxkScheduleTypeID");
				eRPScheduleTaskInformationDto.sxkScheduleTaskID = dataTable.Rows[i].Field<int>("sxkScheduleTaskID");
				eRPScheduleTaskInformationDto.sxkStartActualDateTime = dataTable.Rows[i].Field<DateTime?>("sxkStartActualDateTime");
				eRPScheduleTaskInformationDto.sxkStartDate = dataTable.Rows[i].Field<DateTime?>("sxkStartDate");
				eRPScheduleTaskInformationDto.sxkStartMinute = dataTable.Rows[i].Field<short>("sxkStartMinute");
				eRPScheduleTaskInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPScheduleTaskInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPScheduleTaskInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPScheduleTaskInformationDto> GetScheduleTask(Guid scheduleTaskId)
	{
		ERPScheduleTaskInformationDto eRPScheduleTaskInformationDto = new ERPScheduleTaskInformationDto();
		InitializeParameterLists();
		string[] collection = new string[23]
		{
			"sxkCreatedBy", "sxkCreatedDate", "sxkCurrentTaskDateType", "sxkEndActualDateTime", "sxkEndDate", "sxkEndMinute", "sxkUniqueID", "sxkExchangeID", "sxkLinkedTaskDateType", "sxkLinkedTaskID",
			"sxkMinutes", "sxkOffsetMinutes", "sxkPlantDepartmentID", "sxkPlantID", "sxkProcessID", "sxkRowVersion", "sxkScheduleBranchID", "sxkScheduleTreeID", "sxkScheduleTypeID", "sxkScheduleTaskID",
			"sxkStartActualDateTime", "sxkStartDate", "sxkStartMinute"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("sxkUniqueID|C", scheduleTaskId);
		AddCustomFieldsToSelectList("ScheduleTasks");
		using (DataTable dataTable = GetAsDataTable("ScheduleTasks", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPScheduleTaskInformationDto);
			}
			eRPScheduleTaskInformationDto.sxkCreatedBy = dataTable.Rows[0].Field<string>("sxkCreatedBy");
			eRPScheduleTaskInformationDto.sxkCreatedDate = dataTable.Rows[0].Field<DateTime?>("sxkCreatedDate");
			eRPScheduleTaskInformationDto.sxkCurrentTaskDateType = dataTable.Rows[0].Field<byte>("sxkCurrentTaskDateType");
			eRPScheduleTaskInformationDto.sxkEndActualDateTime = dataTable.Rows[0].Field<DateTime?>("sxkEndActualDateTime");
			eRPScheduleTaskInformationDto.sxkEndDate = dataTable.Rows[0].Field<DateTime?>("sxkEndDate");
			eRPScheduleTaskInformationDto.sxkEndMinute = dataTable.Rows[0].Field<short>("sxkEndMinute");
			eRPScheduleTaskInformationDto.sxkUniqueID = dataTable.Rows[0].Field<Guid>("sxkUniqueID");
			eRPScheduleTaskInformationDto.sxkExchangeID = dataTable.Rows[0].Field<string>("sxkExchangeID");
			eRPScheduleTaskInformationDto.sxkLinkedTaskDateType = dataTable.Rows[0].Field<byte>("sxkLinkedTaskDateType");
			eRPScheduleTaskInformationDto.sxkLinkedTaskID = dataTable.Rows[0].Field<int>("sxkLinkedTaskID");
			eRPScheduleTaskInformationDto.sxkMinutes = dataTable.Rows[0].Field<int>("sxkMinutes");
			eRPScheduleTaskInformationDto.sxkOffsetMinutes = dataTable.Rows[0].Field<int>("sxkOffsetMinutes");
			eRPScheduleTaskInformationDto.sxkPlantDepartmentID = dataTable.Rows[0].Field<string>("sxkPlantDepartmentID");
			eRPScheduleTaskInformationDto.sxkPlantID = dataTable.Rows[0].Field<string>("sxkPlantID");
			eRPScheduleTaskInformationDto.sxkProcessID = dataTable.Rows[0].Field<string>("sxkProcessID");
			eRPScheduleTaskInformationDto.sxkRowVersion = dataTable.Rows[0].Field<byte[]>("sxkRowVersion");
			eRPScheduleTaskInformationDto.sxkScheduleBranchID = dataTable.Rows[0].Field<int>("sxkScheduleBranchID");
			eRPScheduleTaskInformationDto.sxkScheduleTreeID = dataTable.Rows[0].Field<int>("sxkScheduleTreeID");
			eRPScheduleTaskInformationDto.sxkScheduleTypeID = dataTable.Rows[0].Field<byte>("sxkScheduleTypeID");
			eRPScheduleTaskInformationDto.sxkScheduleTaskID = dataTable.Rows[0].Field<int>("sxkScheduleTaskID");
			eRPScheduleTaskInformationDto.sxkStartActualDateTime = dataTable.Rows[0].Field<DateTime?>("sxkStartActualDateTime");
			eRPScheduleTaskInformationDto.sxkStartDate = dataTable.Rows[0].Field<DateTime?>("sxkStartDate");
			eRPScheduleTaskInformationDto.sxkStartMinute = dataTable.Rows[0].Field<short>("sxkStartMinute");
			eRPScheduleTaskInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPScheduleTaskInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPScheduleTaskInformationDto);
	}
}
