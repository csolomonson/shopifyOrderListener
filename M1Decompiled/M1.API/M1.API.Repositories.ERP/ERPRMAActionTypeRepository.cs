using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPRMAActionTypeRepository : APIBaseRepository, IERPRMAActionTypeRepository, IAPIBaseRepository, IDisposable
{
	public ERPRMAActionTypeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesRMAActionTypeExist(Guid rMAActionTypeId)
	{
		InitializeParameterLists();
		base.filterList.Add("ratUniqueID|C", rMAActionTypeId);
		base.selectList.Add("ratUniqueID");
		return Task.FromResult(GetAsObject("RMAActionTypes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPRMAActionTypeInformationDto>> GetAllRMAActionTypes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPRMAActionTypeInformationDto> collection = new List<ERPRMAActionTypeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[4] { "ratRmaActionTypeID", "ratDescription", "ratUniqueID", "ratRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("RMAActionTypes");
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
		using (DataTable dataTable = GetAsDataTable("RMAActionTypes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPRMAActionTypeInformationDto eRPRMAActionTypeInformationDto = new ERPRMAActionTypeInformationDto();
				eRPRMAActionTypeInformationDto.ratRmaActionTypeID = dataTable.Rows[i].Field<string>("ratRmaActionTypeID");
				eRPRMAActionTypeInformationDto.ratDescription = dataTable.Rows[i].Field<string>("ratDescription");
				eRPRMAActionTypeInformationDto.ratUniqueID = dataTable.Rows[i].Field<Guid>("ratUniqueID");
				eRPRMAActionTypeInformationDto.ratRowVersion = dataTable.Rows[i].Field<byte[]>("ratRowVersion");
				eRPRMAActionTypeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPRMAActionTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPRMAActionTypeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPRMAActionTypeInformationDto> GetRMAActionType(Guid rMAActionTypeId)
	{
		ERPRMAActionTypeInformationDto eRPRMAActionTypeInformationDto = new ERPRMAActionTypeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[4] { "ratRmaActionTypeID", "ratDescription", "ratUniqueID", "ratRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("ratUniqueID|C", rMAActionTypeId);
		AddCustomFieldsToSelectList("RMAActionTypes");
		using (DataTable dataTable = GetAsDataTable("RMAActionTypes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPRMAActionTypeInformationDto);
			}
			eRPRMAActionTypeInformationDto.ratRmaActionTypeID = dataTable.Rows[0].Field<string>("ratRmaActionTypeID");
			eRPRMAActionTypeInformationDto.ratDescription = dataTable.Rows[0].Field<string>("ratDescription");
			eRPRMAActionTypeInformationDto.ratUniqueID = dataTable.Rows[0].Field<Guid>("ratUniqueID");
			eRPRMAActionTypeInformationDto.ratRowVersion = dataTable.Rows[0].Field<byte[]>("ratRowVersion");
			eRPRMAActionTypeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPRMAActionTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPRMAActionTypeInformationDto);
	}
}
