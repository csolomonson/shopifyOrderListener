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

public class ERPSupplierRatingRepository : APIBaseRepository, IERPSupplierRatingRepository, IAPIBaseRepository, IDisposable
{
	public ERPSupplierRatingRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesSupplierRatingExist(Guid supplierRatingId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmsUniqueID|C", supplierRatingId);
		base.selectList.Add("cmsUniqueID");
		return Task.FromResult(GetAsObject("SupplierRatings", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPSupplierRatingInformationDto>> GetAllSupplierRatings(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPSupplierRatingInformationDto> collection = new List<ERPSupplierRatingInformationDto>();
		InitializeParameterLists();
		string[] array = new string[6] { "cmsSupplierRatingID", "cmsCreatedBy", "cmsCreatedDate", "cmsDescription", "cmsUniqueID", "cmsRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("SupplierRatings");
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
		using (DataTable dataTable = GetAsDataTable("SupplierRatings", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPSupplierRatingInformationDto eRPSupplierRatingInformationDto = new ERPSupplierRatingInformationDto();
				eRPSupplierRatingInformationDto.cmsSupplierRatingID = dataTable.Rows[i].Field<string>("cmsSupplierRatingID");
				eRPSupplierRatingInformationDto.cmsCreatedBy = dataTable.Rows[i].Field<string>("cmsCreatedBy");
				eRPSupplierRatingInformationDto.cmsCreatedDate = dataTable.Rows[i].Field<DateTime?>("cmsCreatedDate");
				eRPSupplierRatingInformationDto.cmsDescription = dataTable.Rows[i].Field<string>("cmsDescription");
				eRPSupplierRatingInformationDto.cmsUniqueID = dataTable.Rows[i].Field<Guid>("cmsUniqueID");
				eRPSupplierRatingInformationDto.cmsRowVersion = dataTable.Rows[i].Field<byte[]>("cmsRowVersion");
				eRPSupplierRatingInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPSupplierRatingInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPSupplierRatingInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPSupplierRatingInformationDto> GetSupplierRating(Guid supplierRatingId)
	{
		ERPSupplierRatingInformationDto eRPSupplierRatingInformationDto = new ERPSupplierRatingInformationDto();
		InitializeParameterLists();
		string[] collection = new string[6] { "cmsSupplierRatingID", "cmsCreatedBy", "cmsCreatedDate", "cmsDescription", "cmsUniqueID", "cmsRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("cmsUniqueID|C", supplierRatingId);
		AddCustomFieldsToSelectList("SupplierRatings");
		using (DataTable dataTable = GetAsDataTable("SupplierRatings", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPSupplierRatingInformationDto);
			}
			eRPSupplierRatingInformationDto.cmsSupplierRatingID = dataTable.Rows[0].Field<string>("cmsSupplierRatingID");
			eRPSupplierRatingInformationDto.cmsCreatedBy = dataTable.Rows[0].Field<string>("cmsCreatedBy");
			eRPSupplierRatingInformationDto.cmsCreatedDate = dataTable.Rows[0].Field<DateTime?>("cmsCreatedDate");
			eRPSupplierRatingInformationDto.cmsDescription = dataTable.Rows[0].Field<string>("cmsDescription");
			eRPSupplierRatingInformationDto.cmsUniqueID = dataTable.Rows[0].Field<Guid>("cmsUniqueID");
			eRPSupplierRatingInformationDto.cmsRowVersion = dataTable.Rows[0].Field<byte[]>("cmsRowVersion");
			eRPSupplierRatingInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPSupplierRatingInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPSupplierRatingInformationDto);
	}

	public Task<APIValidationInfoDto> SaveSupplierRating(ERPSupplierRatingDto supplierRating)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM SupplierRatings WHERE cmsUniqueID = " + M1Util.ConvertToLinq(supplierRating.cmsUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cmsSupplierRatingID"] = supplierRating.cmsSupplierRatingID.ToUpper();
				supplierRating.cmsUniqueID = ((supplierRating.cmsUniqueID == Guid.Empty) ? Guid.NewGuid() : supplierRating.cmsUniqueID);
				dataRow["cmsUniqueID"] = supplierRating.cmsUniqueID;
				dataRow["cmsCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cmsCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The SupplierRating could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (supplierRating.cmsRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the SupplierRating is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cmsRowVersion"], supplierRating.cmsRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the SupplierRating has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the SupplierRating again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["cmsDescription"] = supplierRating.cmsDescription;
			if (supplierRating.CustomFields != null && supplierRating.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in supplierRating.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the SupplierRating [{supplierRating.cmsUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the SupplierRating [{supplierRating.cmsUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
