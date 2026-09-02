using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPScheduleAllocationRepository : APIBaseRepository, IERPScheduleAllocationRepository, IAPIBaseRepository, IDisposable
{
	public ERPScheduleAllocationRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesScheduleAllocationExist(Guid scheduleAllocationId)
	{
		InitializeParameterLists();
		base.filterList.Add("sxdUniqueID|C", scheduleAllocationId);
		base.selectList.Add("sxdUniqueID");
		return Task.FromResult(GetAsObject("ScheduleAllocations", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPScheduleAllocationInformationDto>> GetAllScheduleAllocations(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPScheduleAllocationInformationDto> collection = new List<ERPScheduleAllocationInformationDto>();
		InitializeParameterLists();
		string[] array = new string[17]
		{
			"sxdDateType", "sxdEndActualDateTime", "sxdEndDate", "sxdEndMinute", "sxdUniqueID", "sxdGroupUniqueID", "sxdMinutes", "sxdResourceUniqueID", "sxdRowVersion", "sxdScheduleBranchID",
			"sxdScheduleResourceLaneID", "sxdScheduleTaskID", "sxdScheduleTreeID", "sxdScheduleAllocationID", "sxdStartActualDateTime", "sxdStartDate", "sxdStartMinute"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ScheduleAllocations");
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
		using (DataTable dataTable = GetAsDataTable("ScheduleAllocations", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPScheduleAllocationInformationDto eRPScheduleAllocationInformationDto = new ERPScheduleAllocationInformationDto();
				eRPScheduleAllocationInformationDto.sxdDateType = dataTable.Rows[i].Field<byte>("sxdDateType");
				eRPScheduleAllocationInformationDto.sxdEndActualDateTime = dataTable.Rows[i].Field<DateTime?>("sxdEndActualDateTime");
				eRPScheduleAllocationInformationDto.sxdEndDate = dataTable.Rows[i].Field<DateTime?>("sxdEndDate");
				eRPScheduleAllocationInformationDto.sxdEndMinute = dataTable.Rows[i].Field<short>("sxdEndMinute");
				eRPScheduleAllocationInformationDto.sxdUniqueID = dataTable.Rows[i].Field<Guid>("sxdUniqueID");
				eRPScheduleAllocationInformationDto.sxdGroupUniqueID = dataTable.Rows[i].Field<Guid?>("sxdGroupUniqueID");
				eRPScheduleAllocationInformationDto.sxdMinutes = dataTable.Rows[i].Field<int>("sxdMinutes");
				eRPScheduleAllocationInformationDto.sxdResourceUniqueID = dataTable.Rows[i].Field<Guid?>("sxdResourceUniqueID");
				eRPScheduleAllocationInformationDto.sxdRowVersion = dataTable.Rows[i].Field<byte[]>("sxdRowVersion");
				eRPScheduleAllocationInformationDto.sxdScheduleBranchID = dataTable.Rows[i].Field<int>("sxdScheduleBranchID");
				eRPScheduleAllocationInformationDto.sxdScheduleResourceLaneID = dataTable.Rows[i].Field<short>("sxdScheduleResourceLaneID");
				eRPScheduleAllocationInformationDto.sxdScheduleTaskID = dataTable.Rows[i].Field<int>("sxdScheduleTaskID");
				eRPScheduleAllocationInformationDto.sxdScheduleTreeID = dataTable.Rows[i].Field<int>("sxdScheduleTreeID");
				eRPScheduleAllocationInformationDto.sxdScheduleAllocationID = dataTable.Rows[i].Field<byte>("sxdScheduleAllocationID");
				eRPScheduleAllocationInformationDto.sxdStartActualDateTime = dataTable.Rows[i].Field<DateTime?>("sxdStartActualDateTime");
				eRPScheduleAllocationInformationDto.sxdStartDate = dataTable.Rows[i].Field<DateTime?>("sxdStartDate");
				eRPScheduleAllocationInformationDto.sxdStartMinute = dataTable.Rows[i].Field<short>("sxdStartMinute");
				eRPScheduleAllocationInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPScheduleAllocationInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPScheduleAllocationInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPScheduleAllocationInformationDto> GetScheduleAllocation(Guid scheduleAllocationId)
	{
		ERPScheduleAllocationInformationDto eRPScheduleAllocationInformationDto = new ERPScheduleAllocationInformationDto();
		InitializeParameterLists();
		string[] collection = new string[17]
		{
			"sxdDateType", "sxdEndActualDateTime", "sxdEndDate", "sxdEndMinute", "sxdUniqueID", "sxdGroupUniqueID", "sxdMinutes", "sxdResourceUniqueID", "sxdRowVersion", "sxdScheduleBranchID",
			"sxdScheduleResourceLaneID", "sxdScheduleTaskID", "sxdScheduleTreeID", "sxdScheduleAllocationID", "sxdStartActualDateTime", "sxdStartDate", "sxdStartMinute"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("sxdUniqueID|C", scheduleAllocationId);
		AddCustomFieldsToSelectList("ScheduleAllocations");
		using (DataTable dataTable = GetAsDataTable("ScheduleAllocations", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPScheduleAllocationInformationDto);
			}
			eRPScheduleAllocationInformationDto.sxdDateType = dataTable.Rows[0].Field<byte>("sxdDateType");
			eRPScheduleAllocationInformationDto.sxdEndActualDateTime = dataTable.Rows[0].Field<DateTime?>("sxdEndActualDateTime");
			eRPScheduleAllocationInformationDto.sxdEndDate = dataTable.Rows[0].Field<DateTime?>("sxdEndDate");
			eRPScheduleAllocationInformationDto.sxdEndMinute = dataTable.Rows[0].Field<short>("sxdEndMinute");
			eRPScheduleAllocationInformationDto.sxdUniqueID = dataTable.Rows[0].Field<Guid>("sxdUniqueID");
			eRPScheduleAllocationInformationDto.sxdGroupUniqueID = dataTable.Rows[0].Field<Guid?>("sxdGroupUniqueID");
			eRPScheduleAllocationInformationDto.sxdMinutes = dataTable.Rows[0].Field<int>("sxdMinutes");
			eRPScheduleAllocationInformationDto.sxdResourceUniqueID = dataTable.Rows[0].Field<Guid?>("sxdResourceUniqueID");
			eRPScheduleAllocationInformationDto.sxdRowVersion = dataTable.Rows[0].Field<byte[]>("sxdRowVersion");
			eRPScheduleAllocationInformationDto.sxdScheduleBranchID = dataTable.Rows[0].Field<int>("sxdScheduleBranchID");
			eRPScheduleAllocationInformationDto.sxdScheduleResourceLaneID = dataTable.Rows[0].Field<short>("sxdScheduleResourceLaneID");
			eRPScheduleAllocationInformationDto.sxdScheduleTaskID = dataTable.Rows[0].Field<int>("sxdScheduleTaskID");
			eRPScheduleAllocationInformationDto.sxdScheduleTreeID = dataTable.Rows[0].Field<int>("sxdScheduleTreeID");
			eRPScheduleAllocationInformationDto.sxdScheduleAllocationID = dataTable.Rows[0].Field<byte>("sxdScheduleAllocationID");
			eRPScheduleAllocationInformationDto.sxdStartActualDateTime = dataTable.Rows[0].Field<DateTime?>("sxdStartActualDateTime");
			eRPScheduleAllocationInformationDto.sxdStartDate = dataTable.Rows[0].Field<DateTime?>("sxdStartDate");
			eRPScheduleAllocationInformationDto.sxdStartMinute = dataTable.Rows[0].Field<short>("sxdStartMinute");
			eRPScheduleAllocationInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPScheduleAllocationInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPScheduleAllocationInformationDto);
	}
}
