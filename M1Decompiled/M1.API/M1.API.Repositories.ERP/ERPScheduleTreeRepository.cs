using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPScheduleTreeRepository : APIBaseRepository, IERPScheduleTreeRepository, IAPIBaseRepository, IDisposable
{
	public ERPScheduleTreeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesScheduleTreeExist(Guid scheduleTreeId)
	{
		InitializeParameterLists();
		base.filterList.Add("sxtUniqueID|C", scheduleTreeId);
		base.selectList.Add("sxtUniqueID");
		return Task.FromResult(GetAsObject("ScheduleTrees", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPScheduleTreeInformationDto>> GetAllScheduleTrees(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPScheduleTreeInformationDto> collection = new List<ERPScheduleTreeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"sxtCreatedBy", "sxtCreatedDate", "sxtDescription", "sxtUniqueID", "sxtGroupUniqueID", "sxtJobScenarioID", "sxtRowVersion", "sxtScheduleTreeID", "sxtSourceTable", "sxtSourceUniqueID",
			"sxtType"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ScheduleTrees");
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
		using (DataTable dataTable = GetAsDataTable("ScheduleTrees", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPScheduleTreeInformationDto eRPScheduleTreeInformationDto = new ERPScheduleTreeInformationDto();
				eRPScheduleTreeInformationDto.sxtCreatedBy = dataTable.Rows[i].Field<string>("sxtCreatedBy");
				eRPScheduleTreeInformationDto.sxtCreatedDate = dataTable.Rows[i].Field<DateTime?>("sxtCreatedDate");
				eRPScheduleTreeInformationDto.sxtDescription = dataTable.Rows[i].Field<string>("sxtDescription");
				eRPScheduleTreeInformationDto.sxtUniqueID = dataTable.Rows[i].Field<Guid>("sxtUniqueID");
				eRPScheduleTreeInformationDto.sxtGroupUniqueID = dataTable.Rows[i].Field<Guid>("sxtGroupUniqueID");
				eRPScheduleTreeInformationDto.sxtJobScenarioID = dataTable.Rows[i].Field<string>("sxtJobScenarioID");
				eRPScheduleTreeInformationDto.sxtRowVersion = dataTable.Rows[i].Field<byte[]>("sxtRowVersion");
				eRPScheduleTreeInformationDto.sxtScheduleTreeID = dataTable.Rows[i].Field<int>("sxtScheduleTreeID");
				eRPScheduleTreeInformationDto.sxtSourceTable = dataTable.Rows[i].Field<string>("sxtSourceTable");
				eRPScheduleTreeInformationDto.sxtSourceUniqueID = dataTable.Rows[i].Field<Guid>("sxtSourceUniqueID");
				eRPScheduleTreeInformationDto.sxtType = dataTable.Rows[i].Field<byte>("sxtType");
				eRPScheduleTreeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPScheduleTreeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPScheduleTreeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPScheduleTreeInformationDto> GetScheduleTree(Guid scheduleTreeId)
	{
		ERPScheduleTreeInformationDto eRPScheduleTreeInformationDto = new ERPScheduleTreeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"sxtCreatedBy", "sxtCreatedDate", "sxtDescription", "sxtUniqueID", "sxtGroupUniqueID", "sxtJobScenarioID", "sxtRowVersion", "sxtScheduleTreeID", "sxtSourceTable", "sxtSourceUniqueID",
			"sxtType"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("sxtUniqueID|C", scheduleTreeId);
		AddCustomFieldsToSelectList("ScheduleTrees");
		using (DataTable dataTable = GetAsDataTable("ScheduleTrees", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPScheduleTreeInformationDto);
			}
			eRPScheduleTreeInformationDto.sxtCreatedBy = dataTable.Rows[0].Field<string>("sxtCreatedBy");
			eRPScheduleTreeInformationDto.sxtCreatedDate = dataTable.Rows[0].Field<DateTime?>("sxtCreatedDate");
			eRPScheduleTreeInformationDto.sxtDescription = dataTable.Rows[0].Field<string>("sxtDescription");
			eRPScheduleTreeInformationDto.sxtUniqueID = dataTable.Rows[0].Field<Guid>("sxtUniqueID");
			eRPScheduleTreeInformationDto.sxtGroupUniqueID = dataTable.Rows[0].Field<Guid>("sxtGroupUniqueID");
			eRPScheduleTreeInformationDto.sxtJobScenarioID = dataTable.Rows[0].Field<string>("sxtJobScenarioID");
			eRPScheduleTreeInformationDto.sxtRowVersion = dataTable.Rows[0].Field<byte[]>("sxtRowVersion");
			eRPScheduleTreeInformationDto.sxtScheduleTreeID = dataTable.Rows[0].Field<int>("sxtScheduleTreeID");
			eRPScheduleTreeInformationDto.sxtSourceTable = dataTable.Rows[0].Field<string>("sxtSourceTable");
			eRPScheduleTreeInformationDto.sxtSourceUniqueID = dataTable.Rows[0].Field<Guid>("sxtSourceUniqueID");
			eRPScheduleTreeInformationDto.sxtType = dataTable.Rows[0].Field<byte>("sxtType");
			eRPScheduleTreeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPScheduleTreeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPScheduleTreeInformationDto);
	}
}
