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

public class ERPShipmentPackageDetailRepository : APIBaseRepository, IERPShipmentPackageDetailRepository, IAPIBaseRepository, IDisposable
{
	public ERPShipmentPackageDetailRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesShipmentPackageDetailExist(Guid shipmentPackageDetailId)
	{
		InitializeParameterLists();
		base.filterList.Add("spdUniqueID|C", shipmentPackageDetailId);
		base.selectList.Add("spdUniqueID");
		return Task.FromResult(GetAsObject("ShipmentPackageDetails", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPShipmentPackageDetailInformationDto>> GetAllShipmentPackageDetails(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPShipmentPackageDetailInformationDto> collection = new List<ERPShipmentPackageDetailInformationDto>();
		InitializeParameterLists();
		string[] array = new string[18]
		{
			"spdCommodityDescription", "spdCountryOfManufacture", "spdCreatedBy", "spdCreatedDate", "spdUniqueID", "spdPartID", "spdPartRevisionID", "spdQuantity", "SPDRowVersion", "spdShipmentID",
			"spdShipmentIDNumber", "spdShipmentLineID", "spdShipmentPackageID", "spdShipmentPackageLineID", "spdTotalPriceBase", "spdTotalPriceForeign", "spdWeight", "spdWeightUnitOfMeasure"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ShipmentPackageDetails");
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
		using (DataTable dataTable = GetAsDataTable("ShipmentPackageDetails", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPShipmentPackageDetailInformationDto eRPShipmentPackageDetailInformationDto = new ERPShipmentPackageDetailInformationDto();
				eRPShipmentPackageDetailInformationDto.spdCommodityDescription = dataTable.Rows[i].Field<string>("spdCommodityDescription");
				eRPShipmentPackageDetailInformationDto.spdCountryOfManufacture = dataTable.Rows[i].Field<string>("spdCountryOfManufacture");
				eRPShipmentPackageDetailInformationDto.spdCreatedBy = dataTable.Rows[i].Field<string>("spdCreatedBy");
				eRPShipmentPackageDetailInformationDto.spdCreatedDate = dataTable.Rows[i].Field<DateTime?>("spdCreatedDate");
				eRPShipmentPackageDetailInformationDto.spdUniqueID = dataTable.Rows[i].Field<Guid>("spdUniqueID");
				eRPShipmentPackageDetailInformationDto.spdPartID = dataTable.Rows[i].Field<string>("spdPartID");
				eRPShipmentPackageDetailInformationDto.spdPartRevisionID = dataTable.Rows[i].Field<string>("spdPartRevisionID");
				eRPShipmentPackageDetailInformationDto.spdQuantity = dataTable.Rows[i].Field<decimal>("spdQuantity");
				eRPShipmentPackageDetailInformationDto.SPDRowVersion = dataTable.Rows[i].Field<byte[]>("SPDRowVersion");
				eRPShipmentPackageDetailInformationDto.spdShipmentID = dataTable.Rows[i].Field<string>("spdShipmentID");
				eRPShipmentPackageDetailInformationDto.spdShipmentIDNumber = dataTable.Rows[i].Field<string>("spdShipmentIDNumber");
				eRPShipmentPackageDetailInformationDto.spdShipmentLineID = dataTable.Rows[i].Field<short>("spdShipmentLineID");
				eRPShipmentPackageDetailInformationDto.spdShipmentPackageID = dataTable.Rows[i].Field<int>("spdShipmentPackageID");
				eRPShipmentPackageDetailInformationDto.spdShipmentPackageLineID = dataTable.Rows[i].Field<int>("spdShipmentPackageLineID");
				eRPShipmentPackageDetailInformationDto.spdTotalPriceBase = dataTable.Rows[i].Field<decimal>("spdTotalPriceBase");
				eRPShipmentPackageDetailInformationDto.spdTotalPriceForeign = dataTable.Rows[i].Field<decimal>("spdTotalPriceForeign");
				eRPShipmentPackageDetailInformationDto.spdWeight = dataTable.Rows[i].Field<decimal>("spdWeight");
				eRPShipmentPackageDetailInformationDto.spdWeightUnitOfMeasure = dataTable.Rows[i].Field<string>("spdWeightUnitOfMeasure");
				eRPShipmentPackageDetailInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPShipmentPackageDetailInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPShipmentPackageDetailInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPShipmentPackageDetailInformationDto> GetShipmentPackageDetail(Guid shipmentPackageDetailId)
	{
		ERPShipmentPackageDetailInformationDto eRPShipmentPackageDetailInformationDto = new ERPShipmentPackageDetailInformationDto();
		InitializeParameterLists();
		string[] collection = new string[18]
		{
			"spdCommodityDescription", "spdCountryOfManufacture", "spdCreatedBy", "spdCreatedDate", "spdUniqueID", "spdPartID", "spdPartRevisionID", "spdQuantity", "SPDRowVersion", "spdShipmentID",
			"spdShipmentIDNumber", "spdShipmentLineID", "spdShipmentPackageID", "spdShipmentPackageLineID", "spdTotalPriceBase", "spdTotalPriceForeign", "spdWeight", "spdWeightUnitOfMeasure"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("spdUniqueID|C", shipmentPackageDetailId);
		AddCustomFieldsToSelectList("ShipmentPackageDetails");
		using (DataTable dataTable = GetAsDataTable("ShipmentPackageDetails", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPShipmentPackageDetailInformationDto);
			}
			eRPShipmentPackageDetailInformationDto.spdCommodityDescription = dataTable.Rows[0].Field<string>("spdCommodityDescription");
			eRPShipmentPackageDetailInformationDto.spdCountryOfManufacture = dataTable.Rows[0].Field<string>("spdCountryOfManufacture");
			eRPShipmentPackageDetailInformationDto.spdCreatedBy = dataTable.Rows[0].Field<string>("spdCreatedBy");
			eRPShipmentPackageDetailInformationDto.spdCreatedDate = dataTable.Rows[0].Field<DateTime?>("spdCreatedDate");
			eRPShipmentPackageDetailInformationDto.spdUniqueID = dataTable.Rows[0].Field<Guid>("spdUniqueID");
			eRPShipmentPackageDetailInformationDto.spdPartID = dataTable.Rows[0].Field<string>("spdPartID");
			eRPShipmentPackageDetailInformationDto.spdPartRevisionID = dataTable.Rows[0].Field<string>("spdPartRevisionID");
			eRPShipmentPackageDetailInformationDto.spdQuantity = dataTable.Rows[0].Field<decimal>("spdQuantity");
			eRPShipmentPackageDetailInformationDto.SPDRowVersion = dataTable.Rows[0].Field<byte[]>("SPDRowVersion");
			eRPShipmentPackageDetailInformationDto.spdShipmentID = dataTable.Rows[0].Field<string>("spdShipmentID");
			eRPShipmentPackageDetailInformationDto.spdShipmentIDNumber = dataTable.Rows[0].Field<string>("spdShipmentIDNumber");
			eRPShipmentPackageDetailInformationDto.spdShipmentLineID = dataTable.Rows[0].Field<short>("spdShipmentLineID");
			eRPShipmentPackageDetailInformationDto.spdShipmentPackageID = dataTable.Rows[0].Field<int>("spdShipmentPackageID");
			eRPShipmentPackageDetailInformationDto.spdShipmentPackageLineID = dataTable.Rows[0].Field<int>("spdShipmentPackageLineID");
			eRPShipmentPackageDetailInformationDto.spdTotalPriceBase = dataTable.Rows[0].Field<decimal>("spdTotalPriceBase");
			eRPShipmentPackageDetailInformationDto.spdTotalPriceForeign = dataTable.Rows[0].Field<decimal>("spdTotalPriceForeign");
			eRPShipmentPackageDetailInformationDto.spdWeight = dataTable.Rows[0].Field<decimal>("spdWeight");
			eRPShipmentPackageDetailInformationDto.spdWeightUnitOfMeasure = dataTable.Rows[0].Field<string>("spdWeightUnitOfMeasure");
			eRPShipmentPackageDetailInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPShipmentPackageDetailInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPShipmentPackageDetailInformationDto);
	}

	public Task<APIValidationInfoDto> SaveShipmentPackageDetail(ERPShipmentPackageDetailDto shipmentPackageDetail)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ShipmentPackageDetails WHERE spdUniqueID = " + M1Util.ConvertToLinq(shipmentPackageDetail.spdUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["spdShipmentID"] = shipmentPackageDetail.spdShipmentID.ToUpper();
				dataRow["spdShipmentLineID"] = shipmentPackageDetail.spdShipmentLineID;
				dataRow["spdShipmentPackageLineID"] = shipmentPackageDetail.spdShipmentPackageLineID;
				shipmentPackageDetail.spdUniqueID = ((shipmentPackageDetail.spdUniqueID == Guid.Empty) ? Guid.NewGuid() : shipmentPackageDetail.spdUniqueID);
				dataRow["spdUniqueID"] = shipmentPackageDetail.spdUniqueID;
				dataRow["spdCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["spdCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ShipmentPackageDetail could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (shipmentPackageDetail.spdRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ShipmentPackageDetail is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["spdRowVersion"], shipmentPackageDetail.spdRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ShipmentPackageDetail has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ShipmentPackageDetail again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["spdCommodityDescription"] = shipmentPackageDetail.spdCommodityDescription;
			dataRow["spdCountryOfManufacture"] = shipmentPackageDetail.spdCountryOfManufacture;
			dataRow["spdPartID"] = shipmentPackageDetail.spdPartID;
			dataRow["spdPartRevisionID"] = shipmentPackageDetail.spdPartRevisionID;
			dataRow["spdQuantity"] = shipmentPackageDetail.spdQuantity;
			dataRow["spdShipmentIDNumber"] = shipmentPackageDetail.spdShipmentIDNumber;
			dataRow["spdShipmentPackageID"] = shipmentPackageDetail.spdShipmentPackageID;
			dataRow["spdTotalPriceBase"] = shipmentPackageDetail.spdTotalPriceBase;
			dataRow["spdTotalPriceForeign"] = shipmentPackageDetail.spdTotalPriceForeign;
			dataRow["spdWeight"] = shipmentPackageDetail.spdWeight;
			dataRow["spdWeightUnitOfMeasure"] = shipmentPackageDetail.spdWeightUnitOfMeasure;
			if (shipmentPackageDetail.CustomFields != null && shipmentPackageDetail.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in shipmentPackageDetail.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ShipmentPackageDetail [{shipmentPackageDetail.spdUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ShipmentPackageDetail [{shipmentPackageDetail.spdUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
