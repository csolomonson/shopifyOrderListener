using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPChangeRequestGroupRepository : APIBaseRepository, IERPChangeRequestGroupRepository, IAPIBaseRepository, IDisposable
{
	public ERPChangeRequestGroupRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesChangeRequestGroupExist(Guid changeRequestGroupId)
	{
		InitializeParameterLists();
		base.filterList.Add("chgUniqueID|C", changeRequestGroupId);
		base.selectList.Add("chgUniqueID");
		return Task.FromResult(GetAsObject("ChangeRequestGroups", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPChangeRequestGroupInformationDto>> GetAllChangeRequestGroups(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPChangeRequestGroupInformationDto> collection = new List<ERPChangeRequestGroupInformationDto>();
		InitializeParameterLists();
		string[] array = new string[8] { "chgChangeRequestGroupID", "chgCreatedBy", "chgCreatedDate", "chgDescription", "chgUniqueID", "chgInactiveDate", "chgInactive", "chgRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ChangeRequestGroups");
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
		using (DataTable dataTable = GetAsDataTable("ChangeRequestGroups", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPChangeRequestGroupInformationDto eRPChangeRequestGroupInformationDto = new ERPChangeRequestGroupInformationDto();
				eRPChangeRequestGroupInformationDto.chgChangeRequestGroupID = dataTable.Rows[i].Field<string>("chgChangeRequestGroupID");
				eRPChangeRequestGroupInformationDto.chgCreatedBy = dataTable.Rows[i].Field<string>("chgCreatedBy");
				eRPChangeRequestGroupInformationDto.chgCreatedDate = dataTable.Rows[i].Field<DateTime?>("chgCreatedDate");
				eRPChangeRequestGroupInformationDto.chgDescription = dataTable.Rows[i].Field<string>("chgDescription");
				eRPChangeRequestGroupInformationDto.chgUniqueID = dataTable.Rows[i].Field<Guid>("chgUniqueID");
				eRPChangeRequestGroupInformationDto.chgInactiveDate = dataTable.Rows[i].Field<DateTime?>("chgInactiveDate");
				eRPChangeRequestGroupInformationDto.chgInactive = dataTable.Rows[i].Field<bool>("chgInactive");
				eRPChangeRequestGroupInformationDto.chgRowVersion = dataTable.Rows[i].Field<byte[]>("chgRowVersion");
				eRPChangeRequestGroupInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPChangeRequestGroupInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPChangeRequestGroupInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPChangeRequestGroupInformationDto> GetChangeRequestGroup(Guid changeRequestGroupId)
	{
		ERPChangeRequestGroupInformationDto eRPChangeRequestGroupInformationDto = new ERPChangeRequestGroupInformationDto();
		InitializeParameterLists();
		string[] collection = new string[8] { "chgChangeRequestGroupID", "chgCreatedBy", "chgCreatedDate", "chgDescription", "chgUniqueID", "chgInactiveDate", "chgInactive", "chgRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("chgUniqueID|C", changeRequestGroupId);
		AddCustomFieldsToSelectList("ChangeRequestGroups");
		using (DataTable dataTable = GetAsDataTable("ChangeRequestGroups", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPChangeRequestGroupInformationDto);
			}
			eRPChangeRequestGroupInformationDto.chgChangeRequestGroupID = dataTable.Rows[0].Field<string>("chgChangeRequestGroupID");
			eRPChangeRequestGroupInformationDto.chgCreatedBy = dataTable.Rows[0].Field<string>("chgCreatedBy");
			eRPChangeRequestGroupInformationDto.chgCreatedDate = dataTable.Rows[0].Field<DateTime?>("chgCreatedDate");
			eRPChangeRequestGroupInformationDto.chgDescription = dataTable.Rows[0].Field<string>("chgDescription");
			eRPChangeRequestGroupInformationDto.chgUniqueID = dataTable.Rows[0].Field<Guid>("chgUniqueID");
			eRPChangeRequestGroupInformationDto.chgInactiveDate = dataTable.Rows[0].Field<DateTime?>("chgInactiveDate");
			eRPChangeRequestGroupInformationDto.chgInactive = dataTable.Rows[0].Field<bool>("chgInactive");
			eRPChangeRequestGroupInformationDto.chgRowVersion = dataTable.Rows[0].Field<byte[]>("chgRowVersion");
			eRPChangeRequestGroupInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPChangeRequestGroupInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPChangeRequestGroupInformationDto);
	}
}
