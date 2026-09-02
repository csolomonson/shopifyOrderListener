using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPCallTypeRepository : APIBaseRepository, IERPCallTypeRepository, IAPIBaseRepository, IDisposable
{
	public ERPCallTypeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesCallTypeExist(Guid callTypeId)
	{
		InitializeParameterLists();
		base.filterList.Add("kbtUniqueID|C", callTypeId);
		base.selectList.Add("kbtUniqueID");
		return Task.FromResult(GetAsObject("CallTypes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPCallTypeInformationDto>> GetAllCallTypes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPCallTypeInformationDto> collection = new List<ERPCallTypeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[13]
		{
			"kbtCallStatus", "kbtCallTypeID", "kbtCreatedBy", "kbtCreatedDate", "kbtDescription", "kbtUniqueID", "kbtInactiveDate", "kbtInactive", "kbtBillableCall", "kbtFieldServiceCall",
			"kbtInboundCall", "kbtInternalOnlyCall", "kbtRowVersion"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("CallTypes");
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
		using (DataTable dataTable = GetAsDataTable("CallTypes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPCallTypeInformationDto eRPCallTypeInformationDto = new ERPCallTypeInformationDto();
				eRPCallTypeInformationDto.kbtCallStatus = dataTable.Rows[i].Field<string>("kbtCallStatus");
				eRPCallTypeInformationDto.kbtCallTypeID = dataTable.Rows[i].Field<string>("kbtCallTypeID");
				eRPCallTypeInformationDto.kbtCreatedBy = dataTable.Rows[i].Field<string>("kbtCreatedBy");
				eRPCallTypeInformationDto.kbtCreatedDate = dataTable.Rows[i].Field<DateTime?>("kbtCreatedDate");
				eRPCallTypeInformationDto.kbtDescription = dataTable.Rows[i].Field<string>("kbtDescription");
				eRPCallTypeInformationDto.kbtUniqueID = dataTable.Rows[i].Field<Guid>("kbtUniqueID");
				eRPCallTypeInformationDto.kbtInactiveDate = dataTable.Rows[i].Field<DateTime?>("kbtInactiveDate");
				eRPCallTypeInformationDto.kbtInactive = dataTable.Rows[i].Field<bool>("kbtInactive");
				eRPCallTypeInformationDto.kbtBillableCall = dataTable.Rows[i].Field<bool>("kbtBillableCall");
				eRPCallTypeInformationDto.kbtFieldServiceCall = dataTable.Rows[i].Field<bool>("kbtFieldServiceCall");
				eRPCallTypeInformationDto.kbtInboundCall = dataTable.Rows[i].Field<bool>("kbtInboundCall");
				eRPCallTypeInformationDto.kbtInternalOnlyCall = dataTable.Rows[i].Field<bool>("kbtInternalOnlyCall");
				eRPCallTypeInformationDto.kbtRowVersion = dataTable.Rows[i].Field<byte[]>("kbtRowVersion");
				eRPCallTypeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPCallTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPCallTypeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPCallTypeInformationDto> GetCallType(Guid callTypeId)
	{
		ERPCallTypeInformationDto eRPCallTypeInformationDto = new ERPCallTypeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[13]
		{
			"kbtCallStatus", "kbtCallTypeID", "kbtCreatedBy", "kbtCreatedDate", "kbtDescription", "kbtUniqueID", "kbtInactiveDate", "kbtInactive", "kbtBillableCall", "kbtFieldServiceCall",
			"kbtInboundCall", "kbtInternalOnlyCall", "kbtRowVersion"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("kbtUniqueID|C", callTypeId);
		AddCustomFieldsToSelectList("CallTypes");
		using (DataTable dataTable = GetAsDataTable("CallTypes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPCallTypeInformationDto);
			}
			eRPCallTypeInformationDto.kbtCallStatus = dataTable.Rows[0].Field<string>("kbtCallStatus");
			eRPCallTypeInformationDto.kbtCallTypeID = dataTable.Rows[0].Field<string>("kbtCallTypeID");
			eRPCallTypeInformationDto.kbtCreatedBy = dataTable.Rows[0].Field<string>("kbtCreatedBy");
			eRPCallTypeInformationDto.kbtCreatedDate = dataTable.Rows[0].Field<DateTime?>("kbtCreatedDate");
			eRPCallTypeInformationDto.kbtDescription = dataTable.Rows[0].Field<string>("kbtDescription");
			eRPCallTypeInformationDto.kbtUniqueID = dataTable.Rows[0].Field<Guid>("kbtUniqueID");
			eRPCallTypeInformationDto.kbtInactiveDate = dataTable.Rows[0].Field<DateTime?>("kbtInactiveDate");
			eRPCallTypeInformationDto.kbtInactive = dataTable.Rows[0].Field<bool>("kbtInactive");
			eRPCallTypeInformationDto.kbtBillableCall = dataTable.Rows[0].Field<bool>("kbtBillableCall");
			eRPCallTypeInformationDto.kbtFieldServiceCall = dataTable.Rows[0].Field<bool>("kbtFieldServiceCall");
			eRPCallTypeInformationDto.kbtInboundCall = dataTable.Rows[0].Field<bool>("kbtInboundCall");
			eRPCallTypeInformationDto.kbtInternalOnlyCall = dataTable.Rows[0].Field<bool>("kbtInternalOnlyCall");
			eRPCallTypeInformationDto.kbtRowVersion = dataTable.Rows[0].Field<byte[]>("kbtRowVersion");
			eRPCallTypeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPCallTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPCallTypeInformationDto);
	}
}
