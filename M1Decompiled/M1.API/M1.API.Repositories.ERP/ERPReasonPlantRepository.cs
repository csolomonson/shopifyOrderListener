using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPReasonPlantRepository : APIBaseRepository, IERPReasonPlantRepository, IAPIBaseRepository, IDisposable
{
	public ERPReasonPlantRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesReasonPlantExist(Guid reasonPlantId)
	{
		InitializeParameterLists();
		base.filterList.Add("xajUniqueID|C", reasonPlantId);
		base.selectList.Add("xajUniqueID");
		return Task.FromResult(GetAsObject("ReasonPlants", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPReasonPlantInformationDto>> GetAllReasonPlants(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPReasonPlantInformationDto> collection = new List<ERPReasonPlantInformationDto>();
		InitializeParameterLists();
		string[] array = new string[8] { "xajReasonPlantID", "xajCreatedBy", "xajCreatedDate", "xajUniqueID", "xajReasonGlAccountID", "xajReasonID", "xajRowVersion", "xajScrapGlAccountID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ReasonPlants");
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
		using (DataTable dataTable = GetAsDataTable("ReasonPlants", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPReasonPlantInformationDto eRPReasonPlantInformationDto = new ERPReasonPlantInformationDto();
				eRPReasonPlantInformationDto.xajReasonPlantID = dataTable.Rows[i].Field<string>("xajReasonPlantID");
				eRPReasonPlantInformationDto.xajCreatedBy = dataTable.Rows[i].Field<string>("xajCreatedBy");
				eRPReasonPlantInformationDto.xajCreatedDate = dataTable.Rows[i].Field<DateTime?>("xajCreatedDate");
				eRPReasonPlantInformationDto.xajUniqueID = dataTable.Rows[i].Field<Guid>("xajUniqueID");
				eRPReasonPlantInformationDto.xajReasonGlAccountID = dataTable.Rows[i].Field<string>("xajReasonGlAccountID");
				eRPReasonPlantInformationDto.xajReasonID = dataTable.Rows[i].Field<string>("xajReasonID");
				eRPReasonPlantInformationDto.xajRowVersion = dataTable.Rows[i].Field<byte[]>("xajRowVersion");
				eRPReasonPlantInformationDto.xajScrapGlAccountID = dataTable.Rows[i].Field<string>("xajScrapGlAccountID");
				eRPReasonPlantInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPReasonPlantInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPReasonPlantInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPReasonPlantInformationDto> GetReasonPlant(Guid reasonPlantId)
	{
		ERPReasonPlantInformationDto eRPReasonPlantInformationDto = new ERPReasonPlantInformationDto();
		InitializeParameterLists();
		string[] collection = new string[8] { "xajReasonPlantID", "xajCreatedBy", "xajCreatedDate", "xajUniqueID", "xajReasonGlAccountID", "xajReasonID", "xajRowVersion", "xajScrapGlAccountID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("xajUniqueID|C", reasonPlantId);
		AddCustomFieldsToSelectList("ReasonPlants");
		using (DataTable dataTable = GetAsDataTable("ReasonPlants", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPReasonPlantInformationDto);
			}
			eRPReasonPlantInformationDto.xajReasonPlantID = dataTable.Rows[0].Field<string>("xajReasonPlantID");
			eRPReasonPlantInformationDto.xajCreatedBy = dataTable.Rows[0].Field<string>("xajCreatedBy");
			eRPReasonPlantInformationDto.xajCreatedDate = dataTable.Rows[0].Field<DateTime?>("xajCreatedDate");
			eRPReasonPlantInformationDto.xajUniqueID = dataTable.Rows[0].Field<Guid>("xajUniqueID");
			eRPReasonPlantInformationDto.xajReasonGlAccountID = dataTable.Rows[0].Field<string>("xajReasonGlAccountID");
			eRPReasonPlantInformationDto.xajReasonID = dataTable.Rows[0].Field<string>("xajReasonID");
			eRPReasonPlantInformationDto.xajRowVersion = dataTable.Rows[0].Field<byte[]>("xajRowVersion");
			eRPReasonPlantInformationDto.xajScrapGlAccountID = dataTable.Rows[0].Field<string>("xajScrapGlAccountID");
			eRPReasonPlantInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPReasonPlantInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPReasonPlantInformationDto);
	}
}
