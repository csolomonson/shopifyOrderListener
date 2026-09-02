using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPServiceContractTypeRepository : APIBaseRepository, IERPServiceContractTypeRepository, IAPIBaseRepository, IDisposable
{
	public ERPServiceContractTypeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesServiceContractTypeExist(Guid serviceContractTypeId)
	{
		InitializeParameterLists();
		base.filterList.Add("kbyUniqueID|C", serviceContractTypeId);
		base.selectList.Add("kbyUniqueID");
		return Task.FromResult(GetAsObject("ServiceContractTypes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPServiceContractTypeInformationDto>> GetAllServiceContractTypes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPServiceContractTypeInformationDto> collection = new List<ERPServiceContractTypeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[8] { "kbyServiceContractTypeID", "kbyCreatedBy", "kbyCreatedDate", "kbyDescription", "kbyUniqueID", "kbyInactiveDate", "kbyInactive", "kbyRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ServiceContractTypes");
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
		using (DataTable dataTable = GetAsDataTable("ServiceContractTypes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPServiceContractTypeInformationDto eRPServiceContractTypeInformationDto = new ERPServiceContractTypeInformationDto();
				eRPServiceContractTypeInformationDto.kbyServiceContractTypeID = dataTable.Rows[i].Field<string>("kbyServiceContractTypeID");
				eRPServiceContractTypeInformationDto.kbyCreatedBy = dataTable.Rows[i].Field<string>("kbyCreatedBy");
				eRPServiceContractTypeInformationDto.kbyCreatedDate = dataTable.Rows[i].Field<DateTime?>("kbyCreatedDate");
				eRPServiceContractTypeInformationDto.kbyDescription = dataTable.Rows[i].Field<string>("kbyDescription");
				eRPServiceContractTypeInformationDto.kbyUniqueID = dataTable.Rows[i].Field<Guid>("kbyUniqueID");
				eRPServiceContractTypeInformationDto.kbyInactiveDate = dataTable.Rows[i].Field<DateTime?>("kbyInactiveDate");
				eRPServiceContractTypeInformationDto.kbyInactive = dataTable.Rows[i].Field<bool>("kbyInactive");
				eRPServiceContractTypeInformationDto.kbyRowVersion = dataTable.Rows[i].Field<byte[]>("kbyRowVersion");
				eRPServiceContractTypeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPServiceContractTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPServiceContractTypeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPServiceContractTypeInformationDto> GetServiceContractType(Guid serviceContractTypeId)
	{
		ERPServiceContractTypeInformationDto eRPServiceContractTypeInformationDto = new ERPServiceContractTypeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[8] { "kbyServiceContractTypeID", "kbyCreatedBy", "kbyCreatedDate", "kbyDescription", "kbyUniqueID", "kbyInactiveDate", "kbyInactive", "kbyRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("kbyUniqueID|C", serviceContractTypeId);
		AddCustomFieldsToSelectList("ServiceContractTypes");
		using (DataTable dataTable = GetAsDataTable("ServiceContractTypes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPServiceContractTypeInformationDto);
			}
			eRPServiceContractTypeInformationDto.kbyServiceContractTypeID = dataTable.Rows[0].Field<string>("kbyServiceContractTypeID");
			eRPServiceContractTypeInformationDto.kbyCreatedBy = dataTable.Rows[0].Field<string>("kbyCreatedBy");
			eRPServiceContractTypeInformationDto.kbyCreatedDate = dataTable.Rows[0].Field<DateTime?>("kbyCreatedDate");
			eRPServiceContractTypeInformationDto.kbyDescription = dataTable.Rows[0].Field<string>("kbyDescription");
			eRPServiceContractTypeInformationDto.kbyUniqueID = dataTable.Rows[0].Field<Guid>("kbyUniqueID");
			eRPServiceContractTypeInformationDto.kbyInactiveDate = dataTable.Rows[0].Field<DateTime?>("kbyInactiveDate");
			eRPServiceContractTypeInformationDto.kbyInactive = dataTable.Rows[0].Field<bool>("kbyInactive");
			eRPServiceContractTypeInformationDto.kbyRowVersion = dataTable.Rows[0].Field<byte[]>("kbyRowVersion");
			eRPServiceContractTypeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPServiceContractTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPServiceContractTypeInformationDto);
	}
}
