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

public class ERPWarehouseRepository : APIBaseRepository, IERPWarehouseRepository, IAPIBaseRepository, IDisposable
{
	public ERPWarehouseRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesWarehouseExist(Guid warehouseId)
	{
		InitializeParameterLists();
		base.filterList.Add("imwUniqueID|C", warehouseId);
		base.selectList.Add("imwUniqueID");
		return Task.FromResult(GetAsObject("Warehouses", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPWarehouseInformationDto>> GetAllWarehouses(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPWarehouseInformationDto> collection = new List<ERPWarehouseInformationDto>();
		InitializeParameterLists();
		string[] array = new string[28]
		{
			"imwAddressLine1", "imwAddressLine2", "imwAddressLine3", "imwCity", "imwWarehouseID", "imwCountry", "imwCreatedBy", "imwCreatedDate", "imwDefaultBinCount", "imwEmailAddress",
			"imwUniqueID", "imwEstablishedDate", "imwFaxNumber", "imwInactiveDate", "imwInactive", "imwAvalaraAddressValidated", "imwDefaultWarehouse", "imwDoNotIncludeInJobCosts", "imwNonNettable", "imwName",
			"imwNonNettableType", "imwPhoneNumber", "imwPlantDepartmentID", "imwPlantID", "imwPostCode", "imwRowVersion", "imwShippingMethodID", "imwState"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Warehouses");
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
		using (DataTable dataTable = GetAsDataTable("Warehouses", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPWarehouseInformationDto eRPWarehouseInformationDto = new ERPWarehouseInformationDto();
				eRPWarehouseInformationDto.imwAddressLine1 = dataTable.Rows[i].Field<string>("imwAddressLine1");
				eRPWarehouseInformationDto.imwAddressLine2 = dataTable.Rows[i].Field<string>("imwAddressLine2");
				eRPWarehouseInformationDto.imwAddressLine3 = dataTable.Rows[i].Field<string>("imwAddressLine3");
				eRPWarehouseInformationDto.imwCity = dataTable.Rows[i].Field<string>("imwCity");
				eRPWarehouseInformationDto.imwWarehouseID = dataTable.Rows[i].Field<string>("imwWarehouseID");
				eRPWarehouseInformationDto.imwCountry = dataTable.Rows[i].Field<string>("imwCountry");
				eRPWarehouseInformationDto.imwCreatedBy = dataTable.Rows[i].Field<string>("imwCreatedBy");
				eRPWarehouseInformationDto.imwCreatedDate = dataTable.Rows[i].Field<DateTime?>("imwCreatedDate");
				eRPWarehouseInformationDto.imwDefaultBinCount = dataTable.Rows[i].Field<int>("imwDefaultBinCount");
				eRPWarehouseInformationDto.imwEmailAddress = dataTable.Rows[i].Field<string>("imwEmailAddress");
				eRPWarehouseInformationDto.imwUniqueID = dataTable.Rows[i].Field<Guid>("imwUniqueID");
				eRPWarehouseInformationDto.imwEstablishedDate = dataTable.Rows[i].Field<DateTime?>("imwEstablishedDate");
				eRPWarehouseInformationDto.imwFaxNumber = dataTable.Rows[i].Field<string>("imwFaxNumber");
				eRPWarehouseInformationDto.imwInactiveDate = dataTable.Rows[i].Field<DateTime?>("imwInactiveDate");
				eRPWarehouseInformationDto.imwInactive = dataTable.Rows[i].Field<bool>("imwInactive");
				eRPWarehouseInformationDto.imwAvalaraAddressValidated = dataTable.Rows[i].Field<bool>("imwAvalaraAddressValidated");
				eRPWarehouseInformationDto.imwDefaultWarehouse = dataTable.Rows[i].Field<bool>("imwDefaultWarehouse");
				eRPWarehouseInformationDto.imwDoNotIncludeInJobCosts = dataTable.Rows[i].Field<bool>("imwDoNotIncludeInJobCosts");
				eRPWarehouseInformationDto.imwNonNettable = dataTable.Rows[i].Field<bool>("imwNonNettable");
				eRPWarehouseInformationDto.imwName = dataTable.Rows[i].Field<string>("imwName");
				eRPWarehouseInformationDto.imwNonNettableType = dataTable.Rows[i].Field<byte>("imwNonNettableType");
				eRPWarehouseInformationDto.imwPhoneNumber = dataTable.Rows[i].Field<string>("imwPhoneNumber");
				eRPWarehouseInformationDto.imwPlantDepartmentID = dataTable.Rows[i].Field<string>("imwPlantDepartmentID");
				eRPWarehouseInformationDto.imwPlantID = dataTable.Rows[i].Field<string>("imwPlantID");
				eRPWarehouseInformationDto.imwPostCode = dataTable.Rows[i].Field<string>("imwPostCode");
				eRPWarehouseInformationDto.imwRowVersion = dataTable.Rows[i].Field<byte[]>("imwRowVersion");
				eRPWarehouseInformationDto.imwShippingMethodID = dataTable.Rows[i].Field<string>("imwShippingMethodID");
				eRPWarehouseInformationDto.imwState = dataTable.Rows[i].Field<string>("imwState");
				eRPWarehouseInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPWarehouseInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPWarehouseInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPWarehouseInformationDto> GetWarehouse(Guid warehouseId)
	{
		ERPWarehouseInformationDto eRPWarehouseInformationDto = new ERPWarehouseInformationDto();
		InitializeParameterLists();
		string[] collection = new string[28]
		{
			"imwAddressLine1", "imwAddressLine2", "imwAddressLine3", "imwCity", "imwWarehouseID", "imwCountry", "imwCreatedBy", "imwCreatedDate", "imwDefaultBinCount", "imwEmailAddress",
			"imwUniqueID", "imwEstablishedDate", "imwFaxNumber", "imwInactiveDate", "imwInactive", "imwAvalaraAddressValidated", "imwDefaultWarehouse", "imwDoNotIncludeInJobCosts", "imwNonNettable", "imwName",
			"imwNonNettableType", "imwPhoneNumber", "imwPlantDepartmentID", "imwPlantID", "imwPostCode", "imwRowVersion", "imwShippingMethodID", "imwState"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imwUniqueID|C", warehouseId);
		AddCustomFieldsToSelectList("Warehouses");
		using (DataTable dataTable = GetAsDataTable("Warehouses", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPWarehouseInformationDto);
			}
			eRPWarehouseInformationDto.imwAddressLine1 = dataTable.Rows[0].Field<string>("imwAddressLine1");
			eRPWarehouseInformationDto.imwAddressLine2 = dataTable.Rows[0].Field<string>("imwAddressLine2");
			eRPWarehouseInformationDto.imwAddressLine3 = dataTable.Rows[0].Field<string>("imwAddressLine3");
			eRPWarehouseInformationDto.imwCity = dataTable.Rows[0].Field<string>("imwCity");
			eRPWarehouseInformationDto.imwWarehouseID = dataTable.Rows[0].Field<string>("imwWarehouseID");
			eRPWarehouseInformationDto.imwCountry = dataTable.Rows[0].Field<string>("imwCountry");
			eRPWarehouseInformationDto.imwCreatedBy = dataTable.Rows[0].Field<string>("imwCreatedBy");
			eRPWarehouseInformationDto.imwCreatedDate = dataTable.Rows[0].Field<DateTime?>("imwCreatedDate");
			eRPWarehouseInformationDto.imwDefaultBinCount = dataTable.Rows[0].Field<int>("imwDefaultBinCount");
			eRPWarehouseInformationDto.imwEmailAddress = dataTable.Rows[0].Field<string>("imwEmailAddress");
			eRPWarehouseInformationDto.imwUniqueID = dataTable.Rows[0].Field<Guid>("imwUniqueID");
			eRPWarehouseInformationDto.imwEstablishedDate = dataTable.Rows[0].Field<DateTime?>("imwEstablishedDate");
			eRPWarehouseInformationDto.imwFaxNumber = dataTable.Rows[0].Field<string>("imwFaxNumber");
			eRPWarehouseInformationDto.imwInactiveDate = dataTable.Rows[0].Field<DateTime?>("imwInactiveDate");
			eRPWarehouseInformationDto.imwInactive = dataTable.Rows[0].Field<bool>("imwInactive");
			eRPWarehouseInformationDto.imwAvalaraAddressValidated = dataTable.Rows[0].Field<bool>("imwAvalaraAddressValidated");
			eRPWarehouseInformationDto.imwDefaultWarehouse = dataTable.Rows[0].Field<bool>("imwDefaultWarehouse");
			eRPWarehouseInformationDto.imwDoNotIncludeInJobCosts = dataTable.Rows[0].Field<bool>("imwDoNotIncludeInJobCosts");
			eRPWarehouseInformationDto.imwNonNettable = dataTable.Rows[0].Field<bool>("imwNonNettable");
			eRPWarehouseInformationDto.imwName = dataTable.Rows[0].Field<string>("imwName");
			eRPWarehouseInformationDto.imwNonNettableType = dataTable.Rows[0].Field<byte>("imwNonNettableType");
			eRPWarehouseInformationDto.imwPhoneNumber = dataTable.Rows[0].Field<string>("imwPhoneNumber");
			eRPWarehouseInformationDto.imwPlantDepartmentID = dataTable.Rows[0].Field<string>("imwPlantDepartmentID");
			eRPWarehouseInformationDto.imwPlantID = dataTable.Rows[0].Field<string>("imwPlantID");
			eRPWarehouseInformationDto.imwPostCode = dataTable.Rows[0].Field<string>("imwPostCode");
			eRPWarehouseInformationDto.imwRowVersion = dataTable.Rows[0].Field<byte[]>("imwRowVersion");
			eRPWarehouseInformationDto.imwShippingMethodID = dataTable.Rows[0].Field<string>("imwShippingMethodID");
			eRPWarehouseInformationDto.imwState = dataTable.Rows[0].Field<string>("imwState");
			eRPWarehouseInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPWarehouseInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPWarehouseInformationDto);
	}

	public Task<APIValidationInfoDto> SaveWarehouse(ERPWarehouseDto warehouse)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Warehouses WHERE imwUniqueID = " + M1Util.ConvertToLinq(warehouse.imwUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imwWarehouseID"] = warehouse.imwWarehouseID.ToUpper();
				warehouse.imwUniqueID = ((warehouse.imwUniqueID == Guid.Empty) ? Guid.NewGuid() : warehouse.imwUniqueID);
				dataRow["imwUniqueID"] = warehouse.imwUniqueID;
				dataRow["imwCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imwCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Warehouse could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (warehouse.imwRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Warehouse is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imwRowVersion"], warehouse.imwRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Warehouse has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Warehouse again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imwAddressLine1"] = warehouse.imwAddressLine1;
			dataRow["imwAddressLine2"] = warehouse.imwAddressLine2;
			dataRow["imwAddressLine3"] = warehouse.imwAddressLine3;
			dataRow["imwCity"] = warehouse.imwCity;
			dataRow["imwCountry"] = warehouse.imwCountry;
			dataRow["imwDefaultBinCount"] = warehouse.imwDefaultBinCount;
			dataRow["imwEmailAddress"] = warehouse.imwEmailAddress ?? dataRow["imwEmailAddress"];
			DataRow dataRow2 = dataRow;
			DateTime? imwEstablishedDate = warehouse.imwEstablishedDate;
			dataRow2["imwEstablishedDate"] = (imwEstablishedDate.HasValue ? ((object)imwEstablishedDate.GetValueOrDefault()) : dataRow["imwEstablishedDate"]);
			dataRow["imwFaxNumber"] = warehouse.imwFaxNumber;
			DataRow dataRow3 = dataRow;
			imwEstablishedDate = warehouse.imwInactiveDate;
			dataRow3["imwInactiveDate"] = (imwEstablishedDate.HasValue ? ((object)imwEstablishedDate.GetValueOrDefault()) : dataRow["imwInactiveDate"]);
			dataRow["imwInactive"] = warehouse.imwInactive;
			dataRow["imwAvalaraAddressValidated"] = warehouse.imwAvalaraAddressValidated;
			dataRow["imwDefaultWarehouse"] = warehouse.imwDefaultWarehouse;
			dataRow["imwDoNotIncludeInJobCosts"] = warehouse.imwDoNotIncludeInJobCosts;
			dataRow["imwNonNettable"] = warehouse.imwNonNettable;
			dataRow["imwName"] = warehouse.imwName;
			dataRow["imwNonNettableType"] = warehouse.imwNonNettableType;
			dataRow["imwPhoneNumber"] = warehouse.imwPhoneNumber;
			dataRow["imwPlantDepartmentID"] = warehouse.imwPlantDepartmentID;
			dataRow["imwPlantID"] = warehouse.imwPlantID;
			dataRow["imwPostCode"] = warehouse.imwPostCode;
			dataRow["imwShippingMethodID"] = warehouse.imwShippingMethodID;
			dataRow["imwState"] = warehouse.imwState;
			if (warehouse.CustomFields != null && warehouse.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in warehouse.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Warehouse [{warehouse.imwUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Warehouse [{warehouse.imwUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
