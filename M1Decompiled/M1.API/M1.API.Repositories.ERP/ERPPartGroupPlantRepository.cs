using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPPartGroupPlantRepository : APIBaseRepository, IERPPartGroupPlantRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartGroupPlantRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartGroupPlantExist(Guid partGroupPlantId)
	{
		InitializeParameterLists();
		base.filterList.Add("imvUniqueID|C", partGroupPlantId);
		base.selectList.Add("imvUniqueID");
		return Task.FromResult(GetAsObject("PartGroupPlants", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartGroupPlantInformationDto>> GetAllPartGroupPlants(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartGroupPlantInformationDto> collection = new List<ERPPartGroupPlantInformationDto>();
		InitializeParameterLists();
		string[] array = new string[13]
		{
			"imvArDepositGlAccountID", "imvPartGroupPlantID", "imvCogsLaborGlAccountID", "imvCogsMaterialGlAccountID", "imvCogsOverheadGlAccountID", "imvCogsSubcontractGlAccountID", "imvCreatedBy", "imvCreatedDate", "imvDiscountGlAccountID", "imvUniqueID",
			"imvPartGroupID", "imvRowVersion", "imvSalesGlAccountID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartGroupPlants");
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
		using (DataTable dataTable = GetAsDataTable("PartGroupPlants", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartGroupPlantInformationDto eRPPartGroupPlantInformationDto = new ERPPartGroupPlantInformationDto();
				eRPPartGroupPlantInformationDto.imvArDepositGlAccountID = dataTable.Rows[i].Field<string>("imvArDepositGlAccountID");
				eRPPartGroupPlantInformationDto.imvPartGroupPlantID = dataTable.Rows[i].Field<string>("imvPartGroupPlantID");
				eRPPartGroupPlantInformationDto.imvCogsLaborGlAccountID = dataTable.Rows[i].Field<string>("imvCogsLaborGlAccountID");
				eRPPartGroupPlantInformationDto.imvCogsMaterialGlAccountID = dataTable.Rows[i].Field<string>("imvCogsMaterialGlAccountID");
				eRPPartGroupPlantInformationDto.imvCogsOverheadGlAccountID = dataTable.Rows[i].Field<string>("imvCogsOverheadGlAccountID");
				eRPPartGroupPlantInformationDto.imvCogsSubcontractGlAccountID = dataTable.Rows[i].Field<string>("imvCogsSubcontractGlAccountID");
				eRPPartGroupPlantInformationDto.imvCreatedBy = dataTable.Rows[i].Field<string>("imvCreatedBy");
				eRPPartGroupPlantInformationDto.imvCreatedDate = dataTable.Rows[i].Field<DateTime?>("imvCreatedDate");
				eRPPartGroupPlantInformationDto.imvDiscountGlAccountID = dataTable.Rows[i].Field<string>("imvDiscountGlAccountID");
				eRPPartGroupPlantInformationDto.imvUniqueID = dataTable.Rows[i].Field<Guid>("imvUniqueID");
				eRPPartGroupPlantInformationDto.imvPartGroupID = dataTable.Rows[i].Field<string>("imvPartGroupID");
				eRPPartGroupPlantInformationDto.imvRowVersion = dataTable.Rows[i].Field<byte[]>("imvRowVersion");
				eRPPartGroupPlantInformationDto.imvSalesGlAccountID = dataTable.Rows[i].Field<string>("imvSalesGlAccountID");
				eRPPartGroupPlantInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartGroupPlantInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartGroupPlantInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartGroupPlantInformationDto> GetPartGroupPlant(Guid partGroupPlantId)
	{
		ERPPartGroupPlantInformationDto eRPPartGroupPlantInformationDto = new ERPPartGroupPlantInformationDto();
		InitializeParameterLists();
		string[] collection = new string[13]
		{
			"imvArDepositGlAccountID", "imvPartGroupPlantID", "imvCogsLaborGlAccountID", "imvCogsMaterialGlAccountID", "imvCogsOverheadGlAccountID", "imvCogsSubcontractGlAccountID", "imvCreatedBy", "imvCreatedDate", "imvDiscountGlAccountID", "imvUniqueID",
			"imvPartGroupID", "imvRowVersion", "imvSalesGlAccountID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imvUniqueID|C", partGroupPlantId);
		AddCustomFieldsToSelectList("PartGroupPlants");
		using (DataTable dataTable = GetAsDataTable("PartGroupPlants", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartGroupPlantInformationDto);
			}
			eRPPartGroupPlantInformationDto.imvArDepositGlAccountID = dataTable.Rows[0].Field<string>("imvArDepositGlAccountID");
			eRPPartGroupPlantInformationDto.imvPartGroupPlantID = dataTable.Rows[0].Field<string>("imvPartGroupPlantID");
			eRPPartGroupPlantInformationDto.imvCogsLaborGlAccountID = dataTable.Rows[0].Field<string>("imvCogsLaborGlAccountID");
			eRPPartGroupPlantInformationDto.imvCogsMaterialGlAccountID = dataTable.Rows[0].Field<string>("imvCogsMaterialGlAccountID");
			eRPPartGroupPlantInformationDto.imvCogsOverheadGlAccountID = dataTable.Rows[0].Field<string>("imvCogsOverheadGlAccountID");
			eRPPartGroupPlantInformationDto.imvCogsSubcontractGlAccountID = dataTable.Rows[0].Field<string>("imvCogsSubcontractGlAccountID");
			eRPPartGroupPlantInformationDto.imvCreatedBy = dataTable.Rows[0].Field<string>("imvCreatedBy");
			eRPPartGroupPlantInformationDto.imvCreatedDate = dataTable.Rows[0].Field<DateTime?>("imvCreatedDate");
			eRPPartGroupPlantInformationDto.imvDiscountGlAccountID = dataTable.Rows[0].Field<string>("imvDiscountGlAccountID");
			eRPPartGroupPlantInformationDto.imvUniqueID = dataTable.Rows[0].Field<Guid>("imvUniqueID");
			eRPPartGroupPlantInformationDto.imvPartGroupID = dataTable.Rows[0].Field<string>("imvPartGroupID");
			eRPPartGroupPlantInformationDto.imvRowVersion = dataTable.Rows[0].Field<byte[]>("imvRowVersion");
			eRPPartGroupPlantInformationDto.imvSalesGlAccountID = dataTable.Rows[0].Field<string>("imvSalesGlAccountID");
			eRPPartGroupPlantInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartGroupPlantInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartGroupPlantInformationDto);
	}
}
