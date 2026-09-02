using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPReasonRepository : APIBaseRepository, IERPReasonRepository, IAPIBaseRepository, IDisposable
{
	public ERPReasonRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesReasonExist(Guid reasonId)
	{
		InitializeParameterLists();
		base.filterList.Add("xarUniqueID|C", reasonId);
		base.selectList.Add("xarUniqueID");
		return Task.FromResult(GetAsObject("Reasons", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPReasonInformationDto>> GetAllReasons(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPReasonInformationDto> collection = new List<ERPReasonInformationDto>();
		InitializeParameterLists();
		string[] array = new string[9] { "xarReasonID", "xarCreatedBy", "xarCreatedDate", "xarDescription", "xarUniqueID", "xarReasonGlAccountID", "xarReasonType", "xarRowVersion", "xarScrapGlAccountID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Reasons");
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
		using (DataTable dataTable = GetAsDataTable("Reasons", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPReasonInformationDto eRPReasonInformationDto = new ERPReasonInformationDto();
				eRPReasonInformationDto.xarReasonID = dataTable.Rows[i].Field<string>("xarReasonID");
				eRPReasonInformationDto.xarCreatedBy = dataTable.Rows[i].Field<string>("xarCreatedBy");
				eRPReasonInformationDto.xarCreatedDate = dataTable.Rows[i].Field<DateTime?>("xarCreatedDate");
				eRPReasonInformationDto.xarDescription = dataTable.Rows[i].Field<string>("xarDescription");
				eRPReasonInformationDto.xarUniqueID = dataTable.Rows[i].Field<Guid>("xarUniqueID");
				eRPReasonInformationDto.xarReasonGlAccountID = dataTable.Rows[i].Field<string>("xarReasonGlAccountID");
				eRPReasonInformationDto.xarReasonType = dataTable.Rows[i].Field<string>("xarReasonType");
				eRPReasonInformationDto.xarRowVersion = dataTable.Rows[i].Field<byte[]>("xarRowVersion");
				eRPReasonInformationDto.xarScrapGlAccountID = dataTable.Rows[i].Field<string>("xarScrapGlAccountID");
				eRPReasonInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPReasonInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPReasonInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPReasonInformationDto> GetReason(Guid reasonId)
	{
		ERPReasonInformationDto eRPReasonInformationDto = new ERPReasonInformationDto();
		InitializeParameterLists();
		string[] collection = new string[9] { "xarReasonID", "xarCreatedBy", "xarCreatedDate", "xarDescription", "xarUniqueID", "xarReasonGlAccountID", "xarReasonType", "xarRowVersion", "xarScrapGlAccountID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("xarUniqueID|C", reasonId);
		AddCustomFieldsToSelectList("Reasons");
		using (DataTable dataTable = GetAsDataTable("Reasons", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPReasonInformationDto);
			}
			eRPReasonInformationDto.xarReasonID = dataTable.Rows[0].Field<string>("xarReasonID");
			eRPReasonInformationDto.xarCreatedBy = dataTable.Rows[0].Field<string>("xarCreatedBy");
			eRPReasonInformationDto.xarCreatedDate = dataTable.Rows[0].Field<DateTime?>("xarCreatedDate");
			eRPReasonInformationDto.xarDescription = dataTable.Rows[0].Field<string>("xarDescription");
			eRPReasonInformationDto.xarUniqueID = dataTable.Rows[0].Field<Guid>("xarUniqueID");
			eRPReasonInformationDto.xarReasonGlAccountID = dataTable.Rows[0].Field<string>("xarReasonGlAccountID");
			eRPReasonInformationDto.xarReasonType = dataTable.Rows[0].Field<string>("xarReasonType");
			eRPReasonInformationDto.xarRowVersion = dataTable.Rows[0].Field<byte[]>("xarRowVersion");
			eRPReasonInformationDto.xarScrapGlAccountID = dataTable.Rows[0].Field<string>("xarScrapGlAccountID");
			eRPReasonInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPReasonInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPReasonInformationDto);
	}
}
