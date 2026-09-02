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

public class ERPPartMaterialRepository : APIBaseRepository, IERPPartMaterialRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartMaterialRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartMaterialExist(Guid partMaterialId)
	{
		InitializeParameterLists();
		base.filterList.Add("immUniqueID|C", partMaterialId);
		base.selectList.Add("immUniqueID");
		return Task.FromResult(GetAsObject("PartMaterials", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartMaterialInformationDto>> GetAllPartMaterials(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartMaterialInformationDto> collection = new List<ERPPartMaterialInformationDto>();
		InitializeParameterLists();
		string[] array = new string[29]
		{
			"immCreatedBy", "immCreatedDate", "immDocuments", "immUniqueID", "immEstimatedUnitCost", "immBackflush", "immManualPart", "immUseDefaultWarehouseAndBin", "immLeadTime", "immMethodAssemblyID",
			"immMethodID", "immMethodMaterialID", "immMethodRevisionID", "immMinimumCharge", "immPartBinID", "immPartID", "immPartLongDescriptionRtf", "immPartLongDescriptionText", "immPartRevisionID", "immPartShortDescription",
			"immPartWarehouseLocationID", "immPurchaseLocationID", "immQuantityPerAssembly", "immRelatedPartOperationID", "immRowVersion", "immScrapPercent", "immScrapQuantity", "immSupplierOrganizationID", "immUnitOfMeasure"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartMaterials");
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
		using (DataTable dataTable = GetAsDataTable("PartMaterials", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartMaterialInformationDto eRPPartMaterialInformationDto = new ERPPartMaterialInformationDto();
				eRPPartMaterialInformationDto.immCreatedBy = dataTable.Rows[i].Field<string>("immCreatedBy");
				eRPPartMaterialInformationDto.immCreatedDate = dataTable.Rows[i].Field<DateTime?>("immCreatedDate");
				eRPPartMaterialInformationDto.immDocuments = dataTable.Rows[i].Field<string>("immDocuments");
				eRPPartMaterialInformationDto.immUniqueID = dataTable.Rows[i].Field<Guid>("immUniqueID");
				eRPPartMaterialInformationDto.immEstimatedUnitCost = dataTable.Rows[i].Field<decimal>("immEstimatedUnitCost");
				eRPPartMaterialInformationDto.immBackflush = dataTable.Rows[i].Field<bool>("immBackflush");
				eRPPartMaterialInformationDto.immManualPart = dataTable.Rows[i].Field<bool>("immManualPart");
				eRPPartMaterialInformationDto.immUseDefaultWarehouseAndBin = dataTable.Rows[i].Field<bool>("immUseDefaultWarehouseAndBin");
				eRPPartMaterialInformationDto.immLeadTime = dataTable.Rows[i].Field<short>("immLeadTime");
				eRPPartMaterialInformationDto.immMethodAssemblyID = dataTable.Rows[i].Field<int>("immMethodAssemblyID");
				eRPPartMaterialInformationDto.immMethodID = dataTable.Rows[i].Field<string>("immMethodID");
				eRPPartMaterialInformationDto.immMethodMaterialID = dataTable.Rows[i].Field<int>("immMethodMaterialID");
				eRPPartMaterialInformationDto.immMethodRevisionID = dataTable.Rows[i].Field<string>("immMethodRevisionID");
				eRPPartMaterialInformationDto.immMinimumCharge = dataTable.Rows[i].Field<decimal>("immMinimumCharge");
				eRPPartMaterialInformationDto.immPartBinID = dataTable.Rows[i].Field<string>("immPartBinID");
				eRPPartMaterialInformationDto.immPartID = dataTable.Rows[i].Field<string>("immPartID");
				eRPPartMaterialInformationDto.immPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("immPartLongDescriptionRtf");
				eRPPartMaterialInformationDto.immPartLongDescriptionText = dataTable.Rows[i].Field<string>("immPartLongDescriptionText");
				eRPPartMaterialInformationDto.immPartRevisionID = dataTable.Rows[i].Field<string>("immPartRevisionID");
				eRPPartMaterialInformationDto.immPartShortDescription = dataTable.Rows[i].Field<string>("immPartShortDescription");
				eRPPartMaterialInformationDto.immPartWarehouseLocationID = dataTable.Rows[i].Field<string>("immPartWarehouseLocationID");
				eRPPartMaterialInformationDto.immPurchaseLocationID = dataTable.Rows[i].Field<string>("immPurchaseLocationID");
				eRPPartMaterialInformationDto.immQuantityPerAssembly = dataTable.Rows[i].Field<decimal>("immQuantityPerAssembly");
				eRPPartMaterialInformationDto.immRelatedPartOperationID = dataTable.Rows[i].Field<int>("immRelatedPartOperationID");
				eRPPartMaterialInformationDto.immRowVersion = dataTable.Rows[i].Field<byte[]>("immRowVersion");
				eRPPartMaterialInformationDto.immScrapPercent = dataTable.Rows[i].Field<decimal>("immScrapPercent");
				eRPPartMaterialInformationDto.immScrapQuantity = dataTable.Rows[i].Field<decimal>("immScrapQuantity");
				eRPPartMaterialInformationDto.immSupplierOrganizationID = dataTable.Rows[i].Field<string>("immSupplierOrganizationID");
				eRPPartMaterialInformationDto.immUnitOfMeasure = dataTable.Rows[i].Field<string>("immUnitOfMeasure");
				eRPPartMaterialInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartMaterialInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartMaterialInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartMaterialInformationDto> GetPartMaterial(Guid partMaterialId)
	{
		ERPPartMaterialInformationDto eRPPartMaterialInformationDto = new ERPPartMaterialInformationDto();
		InitializeParameterLists();
		string[] collection = new string[29]
		{
			"immCreatedBy", "immCreatedDate", "immDocuments", "immUniqueID", "immEstimatedUnitCost", "immBackflush", "immManualPart", "immUseDefaultWarehouseAndBin", "immLeadTime", "immMethodAssemblyID",
			"immMethodID", "immMethodMaterialID", "immMethodRevisionID", "immMinimumCharge", "immPartBinID", "immPartID", "immPartLongDescriptionRtf", "immPartLongDescriptionText", "immPartRevisionID", "immPartShortDescription",
			"immPartWarehouseLocationID", "immPurchaseLocationID", "immQuantityPerAssembly", "immRelatedPartOperationID", "immRowVersion", "immScrapPercent", "immScrapQuantity", "immSupplierOrganizationID", "immUnitOfMeasure"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("immUniqueID|C", partMaterialId);
		AddCustomFieldsToSelectList("PartMaterials");
		using (DataTable dataTable = GetAsDataTable("PartMaterials", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartMaterialInformationDto);
			}
			eRPPartMaterialInformationDto.immCreatedBy = dataTable.Rows[0].Field<string>("immCreatedBy");
			eRPPartMaterialInformationDto.immCreatedDate = dataTable.Rows[0].Field<DateTime?>("immCreatedDate");
			eRPPartMaterialInformationDto.immDocuments = dataTable.Rows[0].Field<string>("immDocuments");
			eRPPartMaterialInformationDto.immUniqueID = dataTable.Rows[0].Field<Guid>("immUniqueID");
			eRPPartMaterialInformationDto.immEstimatedUnitCost = dataTable.Rows[0].Field<decimal>("immEstimatedUnitCost");
			eRPPartMaterialInformationDto.immBackflush = dataTable.Rows[0].Field<bool>("immBackflush");
			eRPPartMaterialInformationDto.immManualPart = dataTable.Rows[0].Field<bool>("immManualPart");
			eRPPartMaterialInformationDto.immUseDefaultWarehouseAndBin = dataTable.Rows[0].Field<bool>("immUseDefaultWarehouseAndBin");
			eRPPartMaterialInformationDto.immLeadTime = dataTable.Rows[0].Field<short>("immLeadTime");
			eRPPartMaterialInformationDto.immMethodAssemblyID = dataTable.Rows[0].Field<int>("immMethodAssemblyID");
			eRPPartMaterialInformationDto.immMethodID = dataTable.Rows[0].Field<string>("immMethodID");
			eRPPartMaterialInformationDto.immMethodMaterialID = dataTable.Rows[0].Field<int>("immMethodMaterialID");
			eRPPartMaterialInformationDto.immMethodRevisionID = dataTable.Rows[0].Field<string>("immMethodRevisionID");
			eRPPartMaterialInformationDto.immMinimumCharge = dataTable.Rows[0].Field<decimal>("immMinimumCharge");
			eRPPartMaterialInformationDto.immPartBinID = dataTable.Rows[0].Field<string>("immPartBinID");
			eRPPartMaterialInformationDto.immPartID = dataTable.Rows[0].Field<string>("immPartID");
			eRPPartMaterialInformationDto.immPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("immPartLongDescriptionRtf");
			eRPPartMaterialInformationDto.immPartLongDescriptionText = dataTable.Rows[0].Field<string>("immPartLongDescriptionText");
			eRPPartMaterialInformationDto.immPartRevisionID = dataTable.Rows[0].Field<string>("immPartRevisionID");
			eRPPartMaterialInformationDto.immPartShortDescription = dataTable.Rows[0].Field<string>("immPartShortDescription");
			eRPPartMaterialInformationDto.immPartWarehouseLocationID = dataTable.Rows[0].Field<string>("immPartWarehouseLocationID");
			eRPPartMaterialInformationDto.immPurchaseLocationID = dataTable.Rows[0].Field<string>("immPurchaseLocationID");
			eRPPartMaterialInformationDto.immQuantityPerAssembly = dataTable.Rows[0].Field<decimal>("immQuantityPerAssembly");
			eRPPartMaterialInformationDto.immRelatedPartOperationID = dataTable.Rows[0].Field<int>("immRelatedPartOperationID");
			eRPPartMaterialInformationDto.immRowVersion = dataTable.Rows[0].Field<byte[]>("immRowVersion");
			eRPPartMaterialInformationDto.immScrapPercent = dataTable.Rows[0].Field<decimal>("immScrapPercent");
			eRPPartMaterialInformationDto.immScrapQuantity = dataTable.Rows[0].Field<decimal>("immScrapQuantity");
			eRPPartMaterialInformationDto.immSupplierOrganizationID = dataTable.Rows[0].Field<string>("immSupplierOrganizationID");
			eRPPartMaterialInformationDto.immUnitOfMeasure = dataTable.Rows[0].Field<string>("immUnitOfMeasure");
			eRPPartMaterialInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartMaterialInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartMaterialInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartMaterial(ERPPartMaterialDto partMaterial)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartMaterials WHERE immUniqueID = " + M1Util.ConvertToLinq(partMaterial.immUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["immMethodID"] = partMaterial.immMethodID.ToUpper();
				dataRow["immMethodRevisionID"] = partMaterial.immMethodRevisionID.ToUpper();
				dataRow["immMethodAssemblyID"] = partMaterial.immMethodAssemblyID;
				dataRow["immMethodMaterialID"] = partMaterial.immMethodMaterialID;
				partMaterial.immUniqueID = ((partMaterial.immUniqueID == Guid.Empty) ? Guid.NewGuid() : partMaterial.immUniqueID);
				dataRow["immUniqueID"] = partMaterial.immUniqueID;
				dataRow["immCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["immCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartMaterial could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partMaterial.immRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartMaterial is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["immRowVersion"], partMaterial.immRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartMaterial has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartMaterial again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["immDocuments"] = partMaterial.immDocuments ?? dataRow["immDocuments"];
			dataRow["immEstimatedUnitCost"] = partMaterial.immEstimatedUnitCost;
			dataRow["immBackflush"] = partMaterial.immBackflush;
			dataRow["immManualPart"] = partMaterial.immManualPart;
			dataRow["immUseDefaultWarehouseAndBin"] = partMaterial.immUseDefaultWarehouseAndBin;
			dataRow["immLeadTime"] = partMaterial.immLeadTime;
			dataRow["immMinimumCharge"] = partMaterial.immMinimumCharge;
			dataRow["immPartBinID"] = partMaterial.immPartBinID;
			dataRow["immPartID"] = partMaterial.immPartID;
			dataRow["immPartLongDescriptionRtf"] = partMaterial.immPartLongDescriptionRtf ?? dataRow["immPartLongDescriptionRtf"];
			dataRow["immPartLongDescriptionText"] = partMaterial.immPartLongDescriptionText ?? dataRow["immPartLongDescriptionText"];
			dataRow["immPartRevisionID"] = partMaterial.immPartRevisionID;
			dataRow["immPartShortDescription"] = partMaterial.immPartShortDescription;
			dataRow["immPartWarehouseLocationID"] = partMaterial.immPartWarehouseLocationID;
			dataRow["immPurchaseLocationID"] = partMaterial.immPurchaseLocationID;
			dataRow["immQuantityPerAssembly"] = partMaterial.immQuantityPerAssembly;
			dataRow["immRelatedPartOperationID"] = partMaterial.immRelatedPartOperationID;
			dataRow["immScrapPercent"] = partMaterial.immScrapPercent;
			dataRow["immScrapQuantity"] = partMaterial.immScrapQuantity;
			dataRow["immSupplierOrganizationID"] = partMaterial.immSupplierOrganizationID;
			dataRow["immUnitOfMeasure"] = partMaterial.immUnitOfMeasure;
			if (partMaterial.CustomFields != null && partMaterial.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partMaterial.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartMaterial [{partMaterial.immUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartMaterial [{partMaterial.immUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
