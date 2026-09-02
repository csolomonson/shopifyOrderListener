using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPScheduleBranchRepository : APIBaseRepository, IERPScheduleBranchRepository, IAPIBaseRepository, IDisposable
{
	public ERPScheduleBranchRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesScheduleBranchExist(Guid scheduleBranchId)
	{
		InitializeParameterLists();
		base.filterList.Add("sxbUniqueID|C", scheduleBranchId);
		base.selectList.Add("sxbUniqueID");
		return Task.FromResult(GetAsObject("ScheduleBranches", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPScheduleBranchInformationDto>> GetAllScheduleBranches(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPScheduleBranchInformationDto> collection = new List<ERPScheduleBranchInformationDto>();
		InitializeParameterLists();
		string[] array = new string[13]
		{
			"sxbCreatedBy", "sxbCreatedDate", "sxbCurrentLinkedTaskDateType", "sxbCurrentLinkedTaskID", "sxbUniqueID", "sxbOffsetMinutes", "sxbParentLinkedTaskDateType", "sxbParentLinkedTaskID", "sxbParentScheduleBranchID", "sxbRowVersion",
			"sxbScheduleTreeID", "sxbScheduleBranchID", "sxbSiblingBranchLink"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ScheduleBranches");
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
		using (DataTable dataTable = GetAsDataTable("ScheduleBranches", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPScheduleBranchInformationDto eRPScheduleBranchInformationDto = new ERPScheduleBranchInformationDto();
				eRPScheduleBranchInformationDto.sxbCreatedBy = dataTable.Rows[i].Field<string>("sxbCreatedBy");
				eRPScheduleBranchInformationDto.sxbCreatedDate = dataTable.Rows[i].Field<DateTime?>("sxbCreatedDate");
				eRPScheduleBranchInformationDto.sxbCurrentLinkedTaskDateType = dataTable.Rows[i].Field<byte>("sxbCurrentLinkedTaskDateType");
				eRPScheduleBranchInformationDto.sxbCurrentLinkedTaskID = dataTable.Rows[i].Field<int>("sxbCurrentLinkedTaskID");
				eRPScheduleBranchInformationDto.sxbUniqueID = dataTable.Rows[i].Field<Guid>("sxbUniqueID");
				eRPScheduleBranchInformationDto.sxbOffsetMinutes = dataTable.Rows[i].Field<int>("sxbOffsetMinutes");
				eRPScheduleBranchInformationDto.sxbParentLinkedTaskDateType = dataTable.Rows[i].Field<byte>("sxbParentLinkedTaskDateType");
				eRPScheduleBranchInformationDto.sxbParentLinkedTaskID = dataTable.Rows[i].Field<int>("sxbParentLinkedTaskID");
				eRPScheduleBranchInformationDto.sxbParentScheduleBranchID = dataTable.Rows[i].Field<int>("sxbParentScheduleBranchID");
				eRPScheduleBranchInformationDto.sxbRowVersion = dataTable.Rows[i].Field<byte[]>("sxbRowVersion");
				eRPScheduleBranchInformationDto.sxbScheduleTreeID = dataTable.Rows[i].Field<int>("sxbScheduleTreeID");
				eRPScheduleBranchInformationDto.sxbScheduleBranchID = dataTable.Rows[i].Field<int>("sxbScheduleBranchID");
				eRPScheduleBranchInformationDto.sxbSiblingBranchLink = dataTable.Rows[i].Field<byte>("sxbSiblingBranchLink");
				eRPScheduleBranchInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPScheduleBranchInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPScheduleBranchInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPScheduleBranchInformationDto> GetScheduleBranch(Guid scheduleBranchId)
	{
		ERPScheduleBranchInformationDto eRPScheduleBranchInformationDto = new ERPScheduleBranchInformationDto();
		InitializeParameterLists();
		string[] collection = new string[13]
		{
			"sxbCreatedBy", "sxbCreatedDate", "sxbCurrentLinkedTaskDateType", "sxbCurrentLinkedTaskID", "sxbUniqueID", "sxbOffsetMinutes", "sxbParentLinkedTaskDateType", "sxbParentLinkedTaskID", "sxbParentScheduleBranchID", "sxbRowVersion",
			"sxbScheduleTreeID", "sxbScheduleBranchID", "sxbSiblingBranchLink"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("sxbUniqueID|C", scheduleBranchId);
		AddCustomFieldsToSelectList("ScheduleBranches");
		using (DataTable dataTable = GetAsDataTable("ScheduleBranches", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPScheduleBranchInformationDto);
			}
			eRPScheduleBranchInformationDto.sxbCreatedBy = dataTable.Rows[0].Field<string>("sxbCreatedBy");
			eRPScheduleBranchInformationDto.sxbCreatedDate = dataTable.Rows[0].Field<DateTime?>("sxbCreatedDate");
			eRPScheduleBranchInformationDto.sxbCurrentLinkedTaskDateType = dataTable.Rows[0].Field<byte>("sxbCurrentLinkedTaskDateType");
			eRPScheduleBranchInformationDto.sxbCurrentLinkedTaskID = dataTable.Rows[0].Field<int>("sxbCurrentLinkedTaskID");
			eRPScheduleBranchInformationDto.sxbUniqueID = dataTable.Rows[0].Field<Guid>("sxbUniqueID");
			eRPScheduleBranchInformationDto.sxbOffsetMinutes = dataTable.Rows[0].Field<int>("sxbOffsetMinutes");
			eRPScheduleBranchInformationDto.sxbParentLinkedTaskDateType = dataTable.Rows[0].Field<byte>("sxbParentLinkedTaskDateType");
			eRPScheduleBranchInformationDto.sxbParentLinkedTaskID = dataTable.Rows[0].Field<int>("sxbParentLinkedTaskID");
			eRPScheduleBranchInformationDto.sxbParentScheduleBranchID = dataTable.Rows[0].Field<int>("sxbParentScheduleBranchID");
			eRPScheduleBranchInformationDto.sxbRowVersion = dataTable.Rows[0].Field<byte[]>("sxbRowVersion");
			eRPScheduleBranchInformationDto.sxbScheduleTreeID = dataTable.Rows[0].Field<int>("sxbScheduleTreeID");
			eRPScheduleBranchInformationDto.sxbScheduleBranchID = dataTable.Rows[0].Field<int>("sxbScheduleBranchID");
			eRPScheduleBranchInformationDto.sxbSiblingBranchLink = dataTable.Rows[0].Field<byte>("sxbSiblingBranchLink");
			eRPScheduleBranchInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPScheduleBranchInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPScheduleBranchInformationDto);
	}
}
