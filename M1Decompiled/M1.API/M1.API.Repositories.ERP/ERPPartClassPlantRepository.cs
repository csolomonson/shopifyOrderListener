using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPPartClassPlantRepository : APIBaseRepository, IERPPartClassPlantRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartClassPlantRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartClassPlantExist(Guid partClassPlantId)
	{
		InitializeParameterLists();
		base.filterList.Add("imfUniqueID|C", partClassPlantId);
		base.selectList.Add("imfUniqueID");
		return Task.FromResult(GetAsObject("PartClassPlants", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartClassPlantInformationDto>> GetAllPartClassPlants(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartClassPlantInformationDto> collection = new List<ERPPartClassPlantInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "imfPartClassPlantID", "imfCreatedBy", "imfCreatedDate", "imfUniqueID", "imfInventoryGlAccountID", "imfInvInInspectionGlAccountID", "imfInvInTransferGlAccountID", "imfInvToReturnGlAccountID", "imfPartClassID", "imfRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartClassPlants");
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
		using (DataTable dataTable = GetAsDataTable("PartClassPlants", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartClassPlantInformationDto eRPPartClassPlantInformationDto = new ERPPartClassPlantInformationDto();
				eRPPartClassPlantInformationDto.imfPartClassPlantID = dataTable.Rows[i].Field<string>("imfPartClassPlantID");
				eRPPartClassPlantInformationDto.imfCreatedBy = dataTable.Rows[i].Field<string>("imfCreatedBy");
				eRPPartClassPlantInformationDto.imfCreatedDate = dataTable.Rows[i].Field<DateTime?>("imfCreatedDate");
				eRPPartClassPlantInformationDto.imfUniqueID = dataTable.Rows[i].Field<Guid>("imfUniqueID");
				eRPPartClassPlantInformationDto.imfInventoryGlAccountID = dataTable.Rows[i].Field<string>("imfInventoryGlAccountID");
				eRPPartClassPlantInformationDto.imfInvInInspectionGlAccountID = dataTable.Rows[i].Field<string>("imfInvInInspectionGlAccountID");
				eRPPartClassPlantInformationDto.imfInvInTransferGlAccountID = dataTable.Rows[i].Field<string>("imfInvInTransferGlAccountID");
				eRPPartClassPlantInformationDto.imfInvToReturnGlAccountID = dataTable.Rows[i].Field<string>("imfInvToReturnGlAccountID");
				eRPPartClassPlantInformationDto.imfPartClassID = dataTable.Rows[i].Field<string>("imfPartClassID");
				eRPPartClassPlantInformationDto.imfRowVersion = dataTable.Rows[i].Field<byte[]>("imfRowVersion");
				eRPPartClassPlantInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartClassPlantInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartClassPlantInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartClassPlantInformationDto> GetPartClassPlant(Guid partClassPlantId)
	{
		ERPPartClassPlantInformationDto eRPPartClassPlantInformationDto = new ERPPartClassPlantInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "imfPartClassPlantID", "imfCreatedBy", "imfCreatedDate", "imfUniqueID", "imfInventoryGlAccountID", "imfInvInInspectionGlAccountID", "imfInvInTransferGlAccountID", "imfInvToReturnGlAccountID", "imfPartClassID", "imfRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("imfUniqueID|C", partClassPlantId);
		AddCustomFieldsToSelectList("PartClassPlants");
		using (DataTable dataTable = GetAsDataTable("PartClassPlants", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartClassPlantInformationDto);
			}
			eRPPartClassPlantInformationDto.imfPartClassPlantID = dataTable.Rows[0].Field<string>("imfPartClassPlantID");
			eRPPartClassPlantInformationDto.imfCreatedBy = dataTable.Rows[0].Field<string>("imfCreatedBy");
			eRPPartClassPlantInformationDto.imfCreatedDate = dataTable.Rows[0].Field<DateTime?>("imfCreatedDate");
			eRPPartClassPlantInformationDto.imfUniqueID = dataTable.Rows[0].Field<Guid>("imfUniqueID");
			eRPPartClassPlantInformationDto.imfInventoryGlAccountID = dataTable.Rows[0].Field<string>("imfInventoryGlAccountID");
			eRPPartClassPlantInformationDto.imfInvInInspectionGlAccountID = dataTable.Rows[0].Field<string>("imfInvInInspectionGlAccountID");
			eRPPartClassPlantInformationDto.imfInvInTransferGlAccountID = dataTable.Rows[0].Field<string>("imfInvInTransferGlAccountID");
			eRPPartClassPlantInformationDto.imfInvToReturnGlAccountID = dataTable.Rows[0].Field<string>("imfInvToReturnGlAccountID");
			eRPPartClassPlantInformationDto.imfPartClassID = dataTable.Rows[0].Field<string>("imfPartClassID");
			eRPPartClassPlantInformationDto.imfRowVersion = dataTable.Rows[0].Field<byte[]>("imfRowVersion");
			eRPPartClassPlantInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartClassPlantInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartClassPlantInformationDto);
	}
}
