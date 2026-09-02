using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Utilities;
using M1.Extensions;

namespace M1.API.Repositories.ERP;

public class ERPTaxCodePlantRepository : APIBaseRepository, IERPTaxCodePlantRepository, IAPIBaseRepository, IDisposable
{
	public ERPTaxCodePlantRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesTaxCodePlantExist(Guid taxCodePlantId)
	{
		InitializeParameterLists();
		base.filterList.Add("xtpUniqueID|C", taxCodePlantId);
		base.selectList.Add("xtpUniqueID");
		return Task.FromResult(GetAsObject("TaxCodePlants", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPTaxCodePlantInformationDto>> GetAllTaxCodePlants(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPTaxCodePlantInformationDto> collection = new List<ERPTaxCodePlantInformationDto>();
		InitializeParameterLists();
		string[] array = new string[7] { "xtpAccrualGlAccountID", "xtpCreatedBy", "xtpCreatedDate", "xtpUniqueID", "xtpPlantID", "xtpRowVersion", "xtpTaxCodeID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("TaxCodePlants");
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
		using (DataTable dataTable = GetAsDataTable("TaxCodePlants", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPTaxCodePlantInformationDto eRPTaxCodePlantInformationDto = new ERPTaxCodePlantInformationDto();
				eRPTaxCodePlantInformationDto.xtpAccrualGlAccountID = dataTable.Rows[i].Field<string>("xtpAccrualGlAccountID");
				eRPTaxCodePlantInformationDto.xtpCreatedBy = dataTable.Rows[i].Field<string>("xtpCreatedBy");
				eRPTaxCodePlantInformationDto.xtpCreatedDate = dataTable.Rows[i].Field<DateTime?>("xtpCreatedDate");
				eRPTaxCodePlantInformationDto.xtpUniqueID = dataTable.Rows[i].Field<Guid>("xtpUniqueID");
				eRPTaxCodePlantInformationDto.xtpPlantID = dataTable.Rows[i].Field<string>("xtpPlantID");
				eRPTaxCodePlantInformationDto.xtpRowVersion = dataTable.Rows[i].Field<byte[]>("xtpRowVersion");
				eRPTaxCodePlantInformationDto.xtpTaxCodeID = dataTable.Rows[i].Field<string>("xtpTaxCodeID");
				eRPTaxCodePlantInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPTaxCodePlantInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPTaxCodePlantInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPTaxCodePlantInformationDto> GetTaxCodePlant(Guid taxCodePlantId)
	{
		ERPTaxCodePlantInformationDto eRPTaxCodePlantInformationDto = new ERPTaxCodePlantInformationDto();
		InitializeParameterLists();
		string[] collection = new string[7] { "xtpAccrualGlAccountID", "xtpCreatedBy", "xtpCreatedDate", "xtpUniqueID", "xtpPlantID", "xtpRowVersion", "xtpTaxCodeID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("xtpUniqueID|C", taxCodePlantId);
		AddCustomFieldsToSelectList("TaxCodePlants");
		using (DataTable dataTable = GetAsDataTable("TaxCodePlants", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPTaxCodePlantInformationDto);
			}
			eRPTaxCodePlantInformationDto.xtpAccrualGlAccountID = dataTable.Rows[0].Field<string>("xtpAccrualGlAccountID");
			eRPTaxCodePlantInformationDto.xtpCreatedBy = dataTable.Rows[0].Field<string>("xtpCreatedBy");
			eRPTaxCodePlantInformationDto.xtpCreatedDate = dataTable.Rows[0].Field<DateTime?>("xtpCreatedDate");
			eRPTaxCodePlantInformationDto.xtpUniqueID = dataTable.Rows[0].Field<Guid>("xtpUniqueID");
			eRPTaxCodePlantInformationDto.xtpPlantID = dataTable.Rows[0].Field<string>("xtpPlantID");
			eRPTaxCodePlantInformationDto.xtpRowVersion = dataTable.Rows[0].Field<byte[]>("xtpRowVersion");
			eRPTaxCodePlantInformationDto.xtpTaxCodeID = dataTable.Rows[0].Field<string>("xtpTaxCodeID");
			eRPTaxCodePlantInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPTaxCodePlantInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPTaxCodePlantInformationDto);
	}

	public Task<APIValidationInfoDto> SaveTaxCodePlant(ERPTaxCodePlantDto taxCodePlant)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM TaxCodePlants WHERE xtpUniqueID = " + M1Util.ConvertToLinq(taxCodePlant.xtpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xtpTaxCodeID"] = taxCodePlant.xtpTaxCodeID.ToUpper();
				dataRow["xtpPlantID"] = taxCodePlant.xtpPlantID.ToUpper();
				taxCodePlant.xtpUniqueID = ((taxCodePlant.xtpUniqueID == Guid.Empty) ? Guid.NewGuid() : taxCodePlant.xtpUniqueID);
				dataRow["xtpUniqueID"] = taxCodePlant.xtpUniqueID;
				dataRow["xtpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xtpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The TaxCodePlant could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (taxCodePlant.xtpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the TaxCodePlant is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xtpRowVersion"], taxCodePlant.xtpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the TaxCodePlant has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the TaxCodePlant again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xtpAccrualGlAccountID"] = taxCodePlant.xtpAccrualGlAccountID;
			if (taxCodePlant.CustomFields != null && taxCodePlant.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in taxCodePlant.CustomFields)
				{
					if (dataTable.Columns.Contains(customField.Key))
					{
						dataRow[customField.Key] = customField.Value;
					}
				}
			}
			dataRow.EndEdit();
			if (flag)
			{
				dataTable.Rows.Add(dataRow);
			}
			if (base.M1database.UpdateData(dataTable, adapter))
			{
				if (flag)
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Created;
				}
				else
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.OK;
				}
			}
			else
			{
				aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.InternalServerError;
			}
		}
		catch (SqlException ex)
		{
			SqlErrorResult httpStatusCodeForSqlException = SqlExceptionMapper.GetHttpStatusCodeForSqlException(ex);
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the TaxCodePlant [{taxCodePlant.xtpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the TaxCodePlant [{taxCodePlant.xtpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
