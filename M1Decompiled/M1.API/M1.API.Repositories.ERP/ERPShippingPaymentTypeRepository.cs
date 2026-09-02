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

public class ERPShippingPaymentTypeRepository : APIBaseRepository, IERPShippingPaymentTypeRepository, IAPIBaseRepository, IDisposable
{
	public ERPShippingPaymentTypeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesShippingPaymentTypeExist(Guid shippingPaymentTypeId)
	{
		InitializeParameterLists();
		base.filterList.Add("xayUniqueID|C", shippingPaymentTypeId);
		base.selectList.Add("xayUniqueID");
		return Task.FromResult(GetAsObject("ShippingPaymentTypes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPShippingPaymentTypeInformationDto>> GetAllShippingPaymentTypes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPShippingPaymentTypeInformationDto> collection = new List<ERPShippingPaymentTypeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[9] { "xayShippingPaymentTypeID", "xayCreatedBy", "xayCreatedDate", "xayDescription", "xayUniqueID", "xayInactiveDate", "xayInactive", "xayDoNotXferShipCostsToAr", "xayRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ShippingPaymentTypes");
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
		using (DataTable dataTable = GetAsDataTable("ShippingPaymentTypes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPShippingPaymentTypeInformationDto eRPShippingPaymentTypeInformationDto = new ERPShippingPaymentTypeInformationDto();
				eRPShippingPaymentTypeInformationDto.xayShippingPaymentTypeID = dataTable.Rows[i].Field<string>("xayShippingPaymentTypeID");
				eRPShippingPaymentTypeInformationDto.xayCreatedBy = dataTable.Rows[i].Field<string>("xayCreatedBy");
				eRPShippingPaymentTypeInformationDto.xayCreatedDate = dataTable.Rows[i].Field<DateTime?>("xayCreatedDate");
				eRPShippingPaymentTypeInformationDto.xayDescription = dataTable.Rows[i].Field<string>("xayDescription");
				eRPShippingPaymentTypeInformationDto.xayUniqueID = dataTable.Rows[i].Field<Guid>("xayUniqueID");
				eRPShippingPaymentTypeInformationDto.xayInactiveDate = dataTable.Rows[i].Field<DateTime?>("xayInactiveDate");
				eRPShippingPaymentTypeInformationDto.xayInactive = dataTable.Rows[i].Field<bool>("xayInactive");
				eRPShippingPaymentTypeInformationDto.xayDoNotXferShipCostsToAr = dataTable.Rows[i].Field<bool>("xayDoNotXferShipCostsToAr");
				eRPShippingPaymentTypeInformationDto.xayRowVersion = dataTable.Rows[i].Field<byte[]>("xayRowVersion");
				eRPShippingPaymentTypeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPShippingPaymentTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPShippingPaymentTypeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPShippingPaymentTypeInformationDto> GetShippingPaymentType(Guid shippingPaymentTypeId)
	{
		ERPShippingPaymentTypeInformationDto eRPShippingPaymentTypeInformationDto = new ERPShippingPaymentTypeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[9] { "xayShippingPaymentTypeID", "xayCreatedBy", "xayCreatedDate", "xayDescription", "xayUniqueID", "xayInactiveDate", "xayInactive", "xayDoNotXferShipCostsToAr", "xayRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("xayUniqueID|C", shippingPaymentTypeId);
		AddCustomFieldsToSelectList("ShippingPaymentTypes");
		using (DataTable dataTable = GetAsDataTable("ShippingPaymentTypes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPShippingPaymentTypeInformationDto);
			}
			eRPShippingPaymentTypeInformationDto.xayShippingPaymentTypeID = dataTable.Rows[0].Field<string>("xayShippingPaymentTypeID");
			eRPShippingPaymentTypeInformationDto.xayCreatedBy = dataTable.Rows[0].Field<string>("xayCreatedBy");
			eRPShippingPaymentTypeInformationDto.xayCreatedDate = dataTable.Rows[0].Field<DateTime?>("xayCreatedDate");
			eRPShippingPaymentTypeInformationDto.xayDescription = dataTable.Rows[0].Field<string>("xayDescription");
			eRPShippingPaymentTypeInformationDto.xayUniqueID = dataTable.Rows[0].Field<Guid>("xayUniqueID");
			eRPShippingPaymentTypeInformationDto.xayInactiveDate = dataTable.Rows[0].Field<DateTime?>("xayInactiveDate");
			eRPShippingPaymentTypeInformationDto.xayInactive = dataTable.Rows[0].Field<bool>("xayInactive");
			eRPShippingPaymentTypeInformationDto.xayDoNotXferShipCostsToAr = dataTable.Rows[0].Field<bool>("xayDoNotXferShipCostsToAr");
			eRPShippingPaymentTypeInformationDto.xayRowVersion = dataTable.Rows[0].Field<byte[]>("xayRowVersion");
			eRPShippingPaymentTypeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPShippingPaymentTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPShippingPaymentTypeInformationDto);
	}

	public Task<APIValidationInfoDto> SaveShippingPaymentType(ERPShippingPaymentTypeDto shippingPaymentType)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ShippingPaymentTypes WHERE xayUniqueID = " + M1Util.ConvertToLinq(shippingPaymentType.xayUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xayShippingPaymentTypeID"] = shippingPaymentType.xayShippingPaymentTypeID.ToUpper();
				shippingPaymentType.xayUniqueID = ((shippingPaymentType.xayUniqueID == Guid.Empty) ? Guid.NewGuid() : shippingPaymentType.xayUniqueID);
				dataRow["xayUniqueID"] = shippingPaymentType.xayUniqueID;
				dataRow["xayCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xayCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ShippingPaymentType could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (shippingPaymentType.xayRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ShippingPaymentType is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xayRowVersion"], shippingPaymentType.xayRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ShippingPaymentType has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ShippingPaymentType again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xayDescription"] = shippingPaymentType.xayDescription;
			DataRow dataRow2 = dataRow;
			DateTime? xayInactiveDate = shippingPaymentType.xayInactiveDate;
			dataRow2["xayInactiveDate"] = (xayInactiveDate.HasValue ? ((object)xayInactiveDate.GetValueOrDefault()) : dataRow["xayInactiveDate"]);
			dataRow["xayInactive"] = shippingPaymentType.xayInactive;
			dataRow["xayDoNotXferShipCostsToAr"] = shippingPaymentType.xayDoNotXferShipCostsToAr;
			if (shippingPaymentType.CustomFields != null && shippingPaymentType.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in shippingPaymentType.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ShippingPaymentType [{shippingPaymentType.xayUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ShippingPaymentType [{shippingPaymentType.xayUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
