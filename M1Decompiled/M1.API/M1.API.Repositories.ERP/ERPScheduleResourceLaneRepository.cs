using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPScheduleResourceLaneRepository : APIBaseRepository, IERPScheduleResourceLaneRepository, IAPIBaseRepository, IDisposable
{
	public ERPScheduleResourceLaneRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesScheduleResourceLaneExist(Guid scheduleResourceLaneId)
	{
		InitializeParameterLists();
		base.filterList.Add("sxrUniqueID|C", scheduleResourceLaneId);
		base.selectList.Add("sxrUniqueID");
		return Task.FromResult(GetAsObject("ScheduleResourceLanes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPScheduleResourceLaneInformationDto>> GetAllScheduleResourceLanes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPScheduleResourceLaneInformationDto> collection = new List<ERPScheduleResourceLaneInformationDto>();
		InitializeParameterLists();
		string[] array = new string[9] { "sxrUniqueID", "sxrGroupUniqueID", "sxrLockedResourceUniqueID", "sxrResourceType", "sxrRowVersion", "sxrScheduleBranchID", "sxrScheduleTaskID", "sxrScheduleTreeID", "sxrScheduleResourceLaneID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ScheduleResourceLanes");
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
		using (DataTable dataTable = GetAsDataTable("ScheduleResourceLanes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPScheduleResourceLaneInformationDto eRPScheduleResourceLaneInformationDto = new ERPScheduleResourceLaneInformationDto();
				eRPScheduleResourceLaneInformationDto.sxrUniqueID = dataTable.Rows[i].Field<Guid>("sxrUniqueID");
				eRPScheduleResourceLaneInformationDto.sxrGroupUniqueID = dataTable.Rows[i].Field<Guid?>("sxrGroupUniqueID");
				eRPScheduleResourceLaneInformationDto.sxrLockedResourceUniqueID = dataTable.Rows[i].Field<Guid?>("sxrLockedResourceUniqueID");
				eRPScheduleResourceLaneInformationDto.sxrResourceType = dataTable.Rows[i].Field<byte>("sxrResourceType");
				eRPScheduleResourceLaneInformationDto.sxrRowVersion = dataTable.Rows[i].Field<byte[]>("sxrRowVersion");
				eRPScheduleResourceLaneInformationDto.sxrScheduleBranchID = dataTable.Rows[i].Field<int>("sxrScheduleBranchID");
				eRPScheduleResourceLaneInformationDto.sxrScheduleTaskID = dataTable.Rows[i].Field<int>("sxrScheduleTaskID");
				eRPScheduleResourceLaneInformationDto.sxrScheduleTreeID = dataTable.Rows[i].Field<int>("sxrScheduleTreeID");
				eRPScheduleResourceLaneInformationDto.sxrScheduleResourceLaneID = dataTable.Rows[i].Field<short>("sxrScheduleResourceLaneID");
				eRPScheduleResourceLaneInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPScheduleResourceLaneInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPScheduleResourceLaneInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPScheduleResourceLaneInformationDto> GetScheduleResourceLane(Guid scheduleResourceLaneId)
	{
		ERPScheduleResourceLaneInformationDto eRPScheduleResourceLaneInformationDto = new ERPScheduleResourceLaneInformationDto();
		InitializeParameterLists();
		string[] collection = new string[9] { "sxrUniqueID", "sxrGroupUniqueID", "sxrLockedResourceUniqueID", "sxrResourceType", "sxrRowVersion", "sxrScheduleBranchID", "sxrScheduleTaskID", "sxrScheduleTreeID", "sxrScheduleResourceLaneID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("sxrUniqueID|C", scheduleResourceLaneId);
		AddCustomFieldsToSelectList("ScheduleResourceLanes");
		using (DataTable dataTable = GetAsDataTable("ScheduleResourceLanes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPScheduleResourceLaneInformationDto);
			}
			eRPScheduleResourceLaneInformationDto.sxrUniqueID = dataTable.Rows[0].Field<Guid>("sxrUniqueID");
			eRPScheduleResourceLaneInformationDto.sxrGroupUniqueID = dataTable.Rows[0].Field<Guid?>("sxrGroupUniqueID");
			eRPScheduleResourceLaneInformationDto.sxrLockedResourceUniqueID = dataTable.Rows[0].Field<Guid?>("sxrLockedResourceUniqueID");
			eRPScheduleResourceLaneInformationDto.sxrResourceType = dataTable.Rows[0].Field<byte>("sxrResourceType");
			eRPScheduleResourceLaneInformationDto.sxrRowVersion = dataTable.Rows[0].Field<byte[]>("sxrRowVersion");
			eRPScheduleResourceLaneInformationDto.sxrScheduleBranchID = dataTable.Rows[0].Field<int>("sxrScheduleBranchID");
			eRPScheduleResourceLaneInformationDto.sxrScheduleTaskID = dataTable.Rows[0].Field<int>("sxrScheduleTaskID");
			eRPScheduleResourceLaneInformationDto.sxrScheduleTreeID = dataTable.Rows[0].Field<int>("sxrScheduleTreeID");
			eRPScheduleResourceLaneInformationDto.sxrScheduleResourceLaneID = dataTable.Rows[0].Field<short>("sxrScheduleResourceLaneID");
			eRPScheduleResourceLaneInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPScheduleResourceLaneInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPScheduleResourceLaneInformationDto);
	}
}
