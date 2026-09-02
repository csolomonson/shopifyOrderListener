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

public class ERPShipmentPackageRepository : APIBaseRepository, IERPShipmentPackageRepository, IAPIBaseRepository, IDisposable
{
	public ERPShipmentPackageRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesShipmentPackageExist(Guid shipmentPackageId)
	{
		InitializeParameterLists();
		base.filterList.Add("spaUniqueID|C", shipmentPackageId);
		base.selectList.Add("spaUniqueID");
		return Task.FromResult(GetAsObject("ShipmentPackages", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPShipmentPackageInformationDto>> GetAllShipmentPackages(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPShipmentPackageInformationDto> collection = new List<ERPShipmentPackageInformationDto>();
		InitializeParameterLists();
		string[] array = new string[30]
		{
			"spaCarrier", "spaCreatedBy", "spaCreatedDate", "spaCustomerPackageID", "spaEdi856CustomLabel", "spaUniqueID", "spaFedExPackageTypes", "spaAdditionalHandlingRequired", "spaLargePackage", "spaVerbalConfirmationRequired",
			"spaLabelFilePath", "spaPackageDimensionsUom", "spaPackageHeight", "spaPackageLength", "spaPackageRate", "spaPackageRateForeign", "spaPackageValue", "spaPackageValueForeign", "spaPackageWeight", "spaPackageWeightUom",
			"spaPackageWidth", "spaReference1", "spaReference2", "SPArowVersion", "spaShipmentPackageID", "spaShipmentID", "spaShipmentIDNumber", "spaShippingMethodID", "spaTrackingNo", "spaUpsPackageTypes"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ShipmentPackages");
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
		using (DataTable dataTable = GetAsDataTable("ShipmentPackages", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPShipmentPackageInformationDto eRPShipmentPackageInformationDto = new ERPShipmentPackageInformationDto();
				eRPShipmentPackageInformationDto.spaCarrier = dataTable.Rows[i].Field<string>("spaCarrier");
				eRPShipmentPackageInformationDto.spaCreatedBy = dataTable.Rows[i].Field<string>("spaCreatedBy");
				eRPShipmentPackageInformationDto.spaCreatedDate = dataTable.Rows[i].Field<DateTime?>("spaCreatedDate");
				eRPShipmentPackageInformationDto.spaCustomerPackageID = dataTable.Rows[i].Field<string>("spaCustomerPackageID");
				eRPShipmentPackageInformationDto.spaEdi856CustomLabel = dataTable.Rows[i].Field<string>("spaEdi856CustomLabel");
				eRPShipmentPackageInformationDto.spaUniqueID = dataTable.Rows[i].Field<Guid>("spaUniqueID");
				eRPShipmentPackageInformationDto.spaFedExPackageTypes = dataTable.Rows[i].Field<string>("spaFedExPackageTypes");
				eRPShipmentPackageInformationDto.spaAdditionalHandlingRequired = dataTable.Rows[i].Field<bool>("spaAdditionalHandlingRequired");
				eRPShipmentPackageInformationDto.spaLargePackage = dataTable.Rows[i].Field<bool>("spaLargePackage");
				eRPShipmentPackageInformationDto.spaVerbalConfirmationRequired = dataTable.Rows[i].Field<bool>("spaVerbalConfirmationRequired");
				eRPShipmentPackageInformationDto.spaLabelFilePath = dataTable.Rows[i].Field<string>("spaLabelFilePath");
				eRPShipmentPackageInformationDto.spaPackageDimensionsUom = dataTable.Rows[i].Field<string>("spaPackageDimensionsUom");
				eRPShipmentPackageInformationDto.spaPackageHeight = dataTable.Rows[i].Field<int>("spaPackageHeight");
				eRPShipmentPackageInformationDto.spaPackageLength = dataTable.Rows[i].Field<int>("spaPackageLength");
				eRPShipmentPackageInformationDto.spaPackageRate = dataTable.Rows[i].Field<decimal>("spaPackageRate");
				eRPShipmentPackageInformationDto.spaPackageRateForeign = dataTable.Rows[i].Field<decimal>("spaPackageRateForeign");
				eRPShipmentPackageInformationDto.spaPackageValue = dataTable.Rows[i].Field<decimal>("spaPackageValue");
				eRPShipmentPackageInformationDto.spaPackageValueForeign = dataTable.Rows[i].Field<decimal>("spaPackageValueForeign");
				eRPShipmentPackageInformationDto.spaPackageWeight = dataTable.Rows[i].Field<decimal>("spaPackageWeight");
				eRPShipmentPackageInformationDto.spaPackageWeightUom = dataTable.Rows[i].Field<string>("spaPackageWeightUom");
				eRPShipmentPackageInformationDto.spaPackageWidth = dataTable.Rows[i].Field<int>("spaPackageWidth");
				eRPShipmentPackageInformationDto.spaReference1 = dataTable.Rows[i].Field<string>("spaReference1");
				eRPShipmentPackageInformationDto.spaReference2 = dataTable.Rows[i].Field<string>("spaReference2");
				eRPShipmentPackageInformationDto.SPArowVersion = dataTable.Rows[i].Field<byte[]>("SPArowVersion");
				eRPShipmentPackageInformationDto.spaShipmentPackageID = dataTable.Rows[i].Field<int>("spaShipmentPackageID");
				eRPShipmentPackageInformationDto.spaShipmentID = dataTable.Rows[i].Field<string>("spaShipmentID");
				eRPShipmentPackageInformationDto.spaShipmentIDNumber = dataTable.Rows[i].Field<string>("spaShipmentIDNumber");
				eRPShipmentPackageInformationDto.spaShippingMethodID = dataTable.Rows[i].Field<string>("spaShippingMethodID");
				eRPShipmentPackageInformationDto.spaTrackingNo = dataTable.Rows[i].Field<string>("spaTrackingNo");
				eRPShipmentPackageInformationDto.spaUpsPackageTypes = dataTable.Rows[i].Field<string>("spaUpsPackageTypes");
				eRPShipmentPackageInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPShipmentPackageInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPShipmentPackageInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPShipmentPackageInformationDto> GetShipmentPackage(Guid shipmentPackageId)
	{
		ERPShipmentPackageInformationDto eRPShipmentPackageInformationDto = new ERPShipmentPackageInformationDto();
		InitializeParameterLists();
		string[] collection = new string[30]
		{
			"spaCarrier", "spaCreatedBy", "spaCreatedDate", "spaCustomerPackageID", "spaEdi856CustomLabel", "spaUniqueID", "spaFedExPackageTypes", "spaAdditionalHandlingRequired", "spaLargePackage", "spaVerbalConfirmationRequired",
			"spaLabelFilePath", "spaPackageDimensionsUom", "spaPackageHeight", "spaPackageLength", "spaPackageRate", "spaPackageRateForeign", "spaPackageValue", "spaPackageValueForeign", "spaPackageWeight", "spaPackageWeightUom",
			"spaPackageWidth", "spaReference1", "spaReference2", "SPArowVersion", "spaShipmentPackageID", "spaShipmentID", "spaShipmentIDNumber", "spaShippingMethodID", "spaTrackingNo", "spaUpsPackageTypes"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("spaUniqueID|C", shipmentPackageId);
		AddCustomFieldsToSelectList("ShipmentPackages");
		using (DataTable dataTable = GetAsDataTable("ShipmentPackages", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPShipmentPackageInformationDto);
			}
			eRPShipmentPackageInformationDto.spaCarrier = dataTable.Rows[0].Field<string>("spaCarrier");
			eRPShipmentPackageInformationDto.spaCreatedBy = dataTable.Rows[0].Field<string>("spaCreatedBy");
			eRPShipmentPackageInformationDto.spaCreatedDate = dataTable.Rows[0].Field<DateTime?>("spaCreatedDate");
			eRPShipmentPackageInformationDto.spaCustomerPackageID = dataTable.Rows[0].Field<string>("spaCustomerPackageID");
			eRPShipmentPackageInformationDto.spaEdi856CustomLabel = dataTable.Rows[0].Field<string>("spaEdi856CustomLabel");
			eRPShipmentPackageInformationDto.spaUniqueID = dataTable.Rows[0].Field<Guid>("spaUniqueID");
			eRPShipmentPackageInformationDto.spaFedExPackageTypes = dataTable.Rows[0].Field<string>("spaFedExPackageTypes");
			eRPShipmentPackageInformationDto.spaAdditionalHandlingRequired = dataTable.Rows[0].Field<bool>("spaAdditionalHandlingRequired");
			eRPShipmentPackageInformationDto.spaLargePackage = dataTable.Rows[0].Field<bool>("spaLargePackage");
			eRPShipmentPackageInformationDto.spaVerbalConfirmationRequired = dataTable.Rows[0].Field<bool>("spaVerbalConfirmationRequired");
			eRPShipmentPackageInformationDto.spaLabelFilePath = dataTable.Rows[0].Field<string>("spaLabelFilePath");
			eRPShipmentPackageInformationDto.spaPackageDimensionsUom = dataTable.Rows[0].Field<string>("spaPackageDimensionsUom");
			eRPShipmentPackageInformationDto.spaPackageHeight = dataTable.Rows[0].Field<int>("spaPackageHeight");
			eRPShipmentPackageInformationDto.spaPackageLength = dataTable.Rows[0].Field<int>("spaPackageLength");
			eRPShipmentPackageInformationDto.spaPackageRate = dataTable.Rows[0].Field<decimal>("spaPackageRate");
			eRPShipmentPackageInformationDto.spaPackageRateForeign = dataTable.Rows[0].Field<decimal>("spaPackageRateForeign");
			eRPShipmentPackageInformationDto.spaPackageValue = dataTable.Rows[0].Field<decimal>("spaPackageValue");
			eRPShipmentPackageInformationDto.spaPackageValueForeign = dataTable.Rows[0].Field<decimal>("spaPackageValueForeign");
			eRPShipmentPackageInformationDto.spaPackageWeight = dataTable.Rows[0].Field<decimal>("spaPackageWeight");
			eRPShipmentPackageInformationDto.spaPackageWeightUom = dataTable.Rows[0].Field<string>("spaPackageWeightUom");
			eRPShipmentPackageInformationDto.spaPackageWidth = dataTable.Rows[0].Field<int>("spaPackageWidth");
			eRPShipmentPackageInformationDto.spaReference1 = dataTable.Rows[0].Field<string>("spaReference1");
			eRPShipmentPackageInformationDto.spaReference2 = dataTable.Rows[0].Field<string>("spaReference2");
			eRPShipmentPackageInformationDto.SPArowVersion = dataTable.Rows[0].Field<byte[]>("SPArowVersion");
			eRPShipmentPackageInformationDto.spaShipmentPackageID = dataTable.Rows[0].Field<int>("spaShipmentPackageID");
			eRPShipmentPackageInformationDto.spaShipmentID = dataTable.Rows[0].Field<string>("spaShipmentID");
			eRPShipmentPackageInformationDto.spaShipmentIDNumber = dataTable.Rows[0].Field<string>("spaShipmentIDNumber");
			eRPShipmentPackageInformationDto.spaShippingMethodID = dataTable.Rows[0].Field<string>("spaShippingMethodID");
			eRPShipmentPackageInformationDto.spaTrackingNo = dataTable.Rows[0].Field<string>("spaTrackingNo");
			eRPShipmentPackageInformationDto.spaUpsPackageTypes = dataTable.Rows[0].Field<string>("spaUpsPackageTypes");
			eRPShipmentPackageInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPShipmentPackageInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPShipmentPackageInformationDto);
	}

	public Task<APIValidationInfoDto> SaveShipmentPackage(ERPShipmentPackageDto shipmentPackage)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ShipmentPackages WHERE spaUniqueID = " + M1Util.ConvertToLinq(shipmentPackage.spaUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["spaShipmentID"] = shipmentPackage.spaShipmentID.ToUpper();
				dataRow["spaShipmentPackageID"] = shipmentPackage.spaShipmentPackageID;
				shipmentPackage.spaUniqueID = ((shipmentPackage.spaUniqueID == Guid.Empty) ? Guid.NewGuid() : shipmentPackage.spaUniqueID);
				dataRow["spaUniqueID"] = shipmentPackage.spaUniqueID;
				dataRow["spaCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["spaCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ShipmentPackage could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (shipmentPackage.spaRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ShipmentPackage is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["spaRowVersion"], shipmentPackage.spaRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ShipmentPackage has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ShipmentPackage again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["spaCarrier"] = shipmentPackage.spaCarrier;
			dataRow["spaCustomerPackageID"] = shipmentPackage.spaCustomerPackageID;
			dataRow["spaEdi856CustomLabel"] = shipmentPackage.spaEdi856CustomLabel;
			dataRow["spaFedExPackageTypes"] = shipmentPackage.spaFedExPackageTypes;
			dataRow["spaAdditionalHandlingRequired"] = shipmentPackage.spaAdditionalHandlingRequired;
			dataRow["spaLargePackage"] = shipmentPackage.spaLargePackage;
			dataRow["spaVerbalConfirmationRequired"] = shipmentPackage.spaVerbalConfirmationRequired;
			dataRow["spaLabelFilePath"] = shipmentPackage.spaLabelFilePath ?? dataRow["spaLabelFilePath"];
			dataRow["spaPackageDimensionsUom"] = shipmentPackage.spaPackageDimensionsUom;
			dataRow["spaPackageHeight"] = shipmentPackage.spaPackageHeight;
			dataRow["spaPackageLength"] = shipmentPackage.spaPackageLength;
			dataRow["spaPackageRate"] = shipmentPackage.spaPackageRate;
			dataRow["spaPackageRateForeign"] = shipmentPackage.spaPackageRateForeign;
			dataRow["spaPackageValue"] = shipmentPackage.spaPackageValue;
			dataRow["spaPackageValueForeign"] = shipmentPackage.spaPackageValueForeign;
			dataRow["spaPackageWeight"] = shipmentPackage.spaPackageWeight;
			dataRow["spaPackageWeightUom"] = shipmentPackage.spaPackageWeightUom;
			dataRow["spaPackageWidth"] = shipmentPackage.spaPackageWidth;
			dataRow["spaReference1"] = shipmentPackage.spaReference1;
			dataRow["spaReference2"] = shipmentPackage.spaReference2;
			dataRow["SPArowVersion"] = shipmentPackage.spaRowVersion;
			dataRow["spaShipmentIDNumber"] = shipmentPackage.spaShipmentIDNumber;
			dataRow["spaShippingMethodID"] = shipmentPackage.spaShippingMethodID;
			dataRow["spaTrackingNo"] = shipmentPackage.spaTrackingNo;
			dataRow["spaUpsPackageTypes"] = shipmentPackage.spaUpsPackageTypes;
			if (shipmentPackage.CustomFields != null && shipmentPackage.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in shipmentPackage.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ShipmentPackage [{shipmentPackage.spaUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ShipmentPackage [{shipmentPackage.spaUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
