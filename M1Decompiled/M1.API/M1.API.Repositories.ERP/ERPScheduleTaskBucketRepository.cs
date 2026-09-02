using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPScheduleTaskBucketRepository : APIBaseRepository, IERPScheduleTaskBucketRepository, IAPIBaseRepository, IDisposable
{
	public ERPScheduleTaskBucketRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesScheduleTaskBucketExist(Guid scheduleTaskBucketId)
	{
		InitializeParameterLists();
		base.filterList.Add("sxeUniqueID|C", scheduleTaskBucketId);
		base.selectList.Add("sxeUniqueID");
		return Task.FromResult(GetAsObject("ScheduleTaskBuckets", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPScheduleTaskBucketInformationDto>> GetAllScheduleTaskBuckets(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPScheduleTaskBucketInformationDto> collection = new List<ERPScheduleTaskBucketInformationDto>();
		InitializeParameterLists();
		string[] array = new string[12]
		{
			"sxeCompletedMinutes", "sxeUniqueID", "sxeCompleted", "sxeMinutes", "sxePercentComplete", "sxeRowVersion", "sxeScheduleBranchID", "sxeScheduleTaskID", "sxeScheduleTreeID", "sxeScheduleTypeBucketID",
			"sxeScheduleTypeID", "sxeScheduleTaskBucketID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ScheduleTaskBuckets");
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
		using (DataTable dataTable = GetAsDataTable("ScheduleTaskBuckets", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPScheduleTaskBucketInformationDto eRPScheduleTaskBucketInformationDto = new ERPScheduleTaskBucketInformationDto();
				eRPScheduleTaskBucketInformationDto.sxeCompletedMinutes = dataTable.Rows[i].Field<int>("sxeCompletedMinutes");
				eRPScheduleTaskBucketInformationDto.sxeUniqueID = dataTable.Rows[i].Field<Guid>("sxeUniqueID");
				eRPScheduleTaskBucketInformationDto.sxeCompleted = dataTable.Rows[i].Field<bool>("sxeCompleted");
				eRPScheduleTaskBucketInformationDto.sxeMinutes = dataTable.Rows[i].Field<int>("sxeMinutes");
				eRPScheduleTaskBucketInformationDto.sxePercentComplete = dataTable.Rows[i].Field<int>("sxePercentComplete");
				eRPScheduleTaskBucketInformationDto.sxeRowVersion = dataTable.Rows[i].Field<byte[]>("sxeRowVersion");
				eRPScheduleTaskBucketInformationDto.sxeScheduleBranchID = dataTable.Rows[i].Field<int>("sxeScheduleBranchID");
				eRPScheduleTaskBucketInformationDto.sxeScheduleTaskID = dataTable.Rows[i].Field<int>("sxeScheduleTaskID");
				eRPScheduleTaskBucketInformationDto.sxeScheduleTreeID = dataTable.Rows[i].Field<int>("sxeScheduleTreeID");
				eRPScheduleTaskBucketInformationDto.sxeScheduleTypeBucketID = dataTable.Rows[i].Field<byte>("sxeScheduleTypeBucketID");
				eRPScheduleTaskBucketInformationDto.sxeScheduleTypeID = dataTable.Rows[i].Field<byte>("sxeScheduleTypeID");
				eRPScheduleTaskBucketInformationDto.sxeScheduleTaskBucketID = dataTable.Rows[i].Field<byte>("sxeScheduleTaskBucketID");
				eRPScheduleTaskBucketInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPScheduleTaskBucketInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPScheduleTaskBucketInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPScheduleTaskBucketInformationDto> GetScheduleTaskBucket(Guid scheduleTaskBucketId)
	{
		ERPScheduleTaskBucketInformationDto eRPScheduleTaskBucketInformationDto = new ERPScheduleTaskBucketInformationDto();
		InitializeParameterLists();
		string[] collection = new string[12]
		{
			"sxeCompletedMinutes", "sxeUniqueID", "sxeCompleted", "sxeMinutes", "sxePercentComplete", "sxeRowVersion", "sxeScheduleBranchID", "sxeScheduleTaskID", "sxeScheduleTreeID", "sxeScheduleTypeBucketID",
			"sxeScheduleTypeID", "sxeScheduleTaskBucketID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("sxeUniqueID|C", scheduleTaskBucketId);
		AddCustomFieldsToSelectList("ScheduleTaskBuckets");
		using (DataTable dataTable = GetAsDataTable("ScheduleTaskBuckets", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPScheduleTaskBucketInformationDto);
			}
			eRPScheduleTaskBucketInformationDto.sxeCompletedMinutes = dataTable.Rows[0].Field<int>("sxeCompletedMinutes");
			eRPScheduleTaskBucketInformationDto.sxeUniqueID = dataTable.Rows[0].Field<Guid>("sxeUniqueID");
			eRPScheduleTaskBucketInformationDto.sxeCompleted = dataTable.Rows[0].Field<bool>("sxeCompleted");
			eRPScheduleTaskBucketInformationDto.sxeMinutes = dataTable.Rows[0].Field<int>("sxeMinutes");
			eRPScheduleTaskBucketInformationDto.sxePercentComplete = dataTable.Rows[0].Field<int>("sxePercentComplete");
			eRPScheduleTaskBucketInformationDto.sxeRowVersion = dataTable.Rows[0].Field<byte[]>("sxeRowVersion");
			eRPScheduleTaskBucketInformationDto.sxeScheduleBranchID = dataTable.Rows[0].Field<int>("sxeScheduleBranchID");
			eRPScheduleTaskBucketInformationDto.sxeScheduleTaskID = dataTable.Rows[0].Field<int>("sxeScheduleTaskID");
			eRPScheduleTaskBucketInformationDto.sxeScheduleTreeID = dataTable.Rows[0].Field<int>("sxeScheduleTreeID");
			eRPScheduleTaskBucketInformationDto.sxeScheduleTypeBucketID = dataTable.Rows[0].Field<byte>("sxeScheduleTypeBucketID");
			eRPScheduleTaskBucketInformationDto.sxeScheduleTypeID = dataTable.Rows[0].Field<byte>("sxeScheduleTypeID");
			eRPScheduleTaskBucketInformationDto.sxeScheduleTaskBucketID = dataTable.Rows[0].Field<byte>("sxeScheduleTaskBucketID");
			eRPScheduleTaskBucketInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPScheduleTaskBucketInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPScheduleTaskBucketInformationDto);
	}
}
