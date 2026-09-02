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

public class ERPReceiptComponentRepository : APIBaseRepository, IERPReceiptComponentRepository, IAPIBaseRepository, IDisposable
{
	public ERPReceiptComponentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesReceiptComponentExist(Guid receiptComponentId)
	{
		InitializeParameterLists();
		base.filterList.Add("rmoUniqueID|C", receiptComponentId);
		base.selectList.Add("rmoUniqueID");
		return Task.FromResult(GetAsObject("ReceiptComponents", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPReceiptComponentInformationDto>> GetAllReceiptComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPReceiptComponentInformationDto> collection = new List<ERPReceiptComponentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[45]
		{
			"rmoAdditionalQuantity", "rmoConversionFactor", "rmoCreatedBy", "rmoCreatedDate", "rmoDescription", "rmoUniqueID", "rmoExtendedCostBase", "rmoExtendedCostForeign", "rmoInspParentQuantity", "rmoInventoryUnitCost",
			"rmoInventoryUnitCostForeign", "rmoInvParentQuantity", "rmoInvQuantityReceived", "rmoClosed", "rmoInspectionComplete", "rmoJobReceivedComplete", "rmoPostedToGl", "rmoReceivedComplete", "rmoReversed", "rmoJobAssemblyID",
			"rmoJobID", "rmoJobMaterialComponentID", "rmoJobMaterialID", "rmoJobParentQuantity", "rmoJobQuantityReceived", "rmoPartBinID", "rmoPartID", "rmoPartRevisionID", "rmoPartWarehouseLocationID", "rmoPurchaseOrderComponentID",
			"rmoPurchaseOrderID", "rmoPurchaseOrderLineID", "rmoPurchaseUnitCost", "rmoPurchaseUnitCostForeign", "rmoQuantityPerParent", "rmoQuantityToInspect", "rmoReceiptID", "rmoReceiptLineID", "rmoReverseReceiptComponentID", "rmoReverseReceiptID",
			"rmoReverseReceiptLineID", "rmoRowVersion", "rmoReceiptComponentID", "rmoUnitOfMeasure", "rmoWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ReceiptComponents");
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
		using (DataTable dataTable = GetAsDataTable("ReceiptComponents", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPReceiptComponentInformationDto eRPReceiptComponentInformationDto = new ERPReceiptComponentInformationDto();
				eRPReceiptComponentInformationDto.rmoAdditionalQuantity = dataTable.Rows[i].Field<decimal>("rmoAdditionalQuantity");
				eRPReceiptComponentInformationDto.rmoConversionFactor = dataTable.Rows[i].Field<decimal>("rmoConversionFactor");
				eRPReceiptComponentInformationDto.rmoCreatedBy = dataTable.Rows[i].Field<string>("rmoCreatedBy");
				eRPReceiptComponentInformationDto.rmoCreatedDate = dataTable.Rows[i].Field<DateTime?>("rmoCreatedDate");
				eRPReceiptComponentInformationDto.rmoDescription = dataTable.Rows[i].Field<string>("rmoDescription");
				eRPReceiptComponentInformationDto.rmoUniqueID = dataTable.Rows[i].Field<Guid>("rmoUniqueID");
				eRPReceiptComponentInformationDto.rmoExtendedCostBase = dataTable.Rows[i].Field<decimal>("rmoExtendedCostBase");
				eRPReceiptComponentInformationDto.rmoExtendedCostForeign = dataTable.Rows[i].Field<decimal>("rmoExtendedCostForeign");
				eRPReceiptComponentInformationDto.rmoInspParentQuantity = dataTable.Rows[i].Field<decimal>("rmoInspParentQuantity");
				eRPReceiptComponentInformationDto.rmoInventoryUnitCost = dataTable.Rows[i].Field<decimal>("rmoInventoryUnitCost");
				eRPReceiptComponentInformationDto.rmoInventoryUnitCostForeign = dataTable.Rows[i].Field<decimal>("rmoInventoryUnitCostForeign");
				eRPReceiptComponentInformationDto.rmoInvParentQuantity = dataTable.Rows[i].Field<decimal>("rmoInvParentQuantity");
				eRPReceiptComponentInformationDto.rmoInvQuantityReceived = dataTable.Rows[i].Field<decimal>("rmoInvQuantityReceived");
				eRPReceiptComponentInformationDto.rmoClosed = dataTable.Rows[i].Field<bool>("rmoClosed");
				eRPReceiptComponentInformationDto.rmoInspectionComplete = dataTable.Rows[i].Field<bool>("rmoInspectionComplete");
				eRPReceiptComponentInformationDto.rmoJobReceivedComplete = dataTable.Rows[i].Field<bool>("rmoJobReceivedComplete");
				eRPReceiptComponentInformationDto.rmoPostedToGl = dataTable.Rows[i].Field<bool>("rmoPostedToGl");
				eRPReceiptComponentInformationDto.rmoReceivedComplete = dataTable.Rows[i].Field<bool>("rmoReceivedComplete");
				eRPReceiptComponentInformationDto.rmoReversed = dataTable.Rows[i].Field<bool>("rmoReversed");
				eRPReceiptComponentInformationDto.rmoJobAssemblyID = dataTable.Rows[i].Field<int>("rmoJobAssemblyID");
				eRPReceiptComponentInformationDto.rmoJobID = dataTable.Rows[i].Field<string>("rmoJobID");
				eRPReceiptComponentInformationDto.rmoJobMaterialComponentID = dataTable.Rows[i].Field<int>("rmoJobMaterialComponentID");
				eRPReceiptComponentInformationDto.rmoJobMaterialID = dataTable.Rows[i].Field<int>("rmoJobMaterialID");
				eRPReceiptComponentInformationDto.rmoJobParentQuantity = dataTable.Rows[i].Field<decimal>("rmoJobParentQuantity");
				eRPReceiptComponentInformationDto.rmoJobQuantityReceived = dataTable.Rows[i].Field<decimal>("rmoJobQuantityReceived");
				eRPReceiptComponentInformationDto.rmoPartBinID = dataTable.Rows[i].Field<string>("rmoPartBinID");
				eRPReceiptComponentInformationDto.rmoPartID = dataTable.Rows[i].Field<string>("rmoPartID");
				eRPReceiptComponentInformationDto.rmoPartRevisionID = dataTable.Rows[i].Field<string>("rmoPartRevisionID");
				eRPReceiptComponentInformationDto.rmoPartWarehouseLocationID = dataTable.Rows[i].Field<string>("rmoPartWarehouseLocationID");
				eRPReceiptComponentInformationDto.rmoPurchaseOrderComponentID = dataTable.Rows[i].Field<short>("rmoPurchaseOrderComponentID");
				eRPReceiptComponentInformationDto.rmoPurchaseOrderID = dataTable.Rows[i].Field<string>("rmoPurchaseOrderID");
				eRPReceiptComponentInformationDto.rmoPurchaseOrderLineID = dataTable.Rows[i].Field<short>("rmoPurchaseOrderLineID");
				eRPReceiptComponentInformationDto.rmoPurchaseUnitCost = dataTable.Rows[i].Field<decimal>("rmoPurchaseUnitCost");
				eRPReceiptComponentInformationDto.rmoPurchaseUnitCostForeign = dataTable.Rows[i].Field<decimal>("rmoPurchaseUnitCostForeign");
				eRPReceiptComponentInformationDto.rmoQuantityPerParent = dataTable.Rows[i].Field<decimal>("rmoQuantityPerParent");
				eRPReceiptComponentInformationDto.rmoQuantityToInspect = dataTable.Rows[i].Field<decimal>("rmoQuantityToInspect");
				eRPReceiptComponentInformationDto.rmoReceiptID = dataTable.Rows[i].Field<string>("rmoReceiptID");
				eRPReceiptComponentInformationDto.rmoReceiptLineID = dataTable.Rows[i].Field<short>("rmoReceiptLineID");
				eRPReceiptComponentInformationDto.rmoReverseReceiptComponentID = dataTable.Rows[i].Field<short>("rmoReverseReceiptComponentID");
				eRPReceiptComponentInformationDto.rmoReverseReceiptID = dataTable.Rows[i].Field<string>("rmoReverseReceiptID");
				eRPReceiptComponentInformationDto.rmoReverseReceiptLineID = dataTable.Rows[i].Field<short>("rmoReverseReceiptLineID");
				eRPReceiptComponentInformationDto.rmoRowVersion = dataTable.Rows[i].Field<byte[]>("rmoRowVersion");
				eRPReceiptComponentInformationDto.rmoReceiptComponentID = dataTable.Rows[i].Field<short>("rmoReceiptComponentID");
				eRPReceiptComponentInformationDto.rmoUnitOfMeasure = dataTable.Rows[i].Field<string>("rmoUnitOfMeasure");
				eRPReceiptComponentInformationDto.rmoWeight = dataTable.Rows[i].Field<decimal>("rmoWeight");
				eRPReceiptComponentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPReceiptComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPReceiptComponentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPReceiptComponentInformationDto> GetReceiptComponent(Guid receiptComponentId)
	{
		ERPReceiptComponentInformationDto eRPReceiptComponentInformationDto = new ERPReceiptComponentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[45]
		{
			"rmoAdditionalQuantity", "rmoConversionFactor", "rmoCreatedBy", "rmoCreatedDate", "rmoDescription", "rmoUniqueID", "rmoExtendedCostBase", "rmoExtendedCostForeign", "rmoInspParentQuantity", "rmoInventoryUnitCost",
			"rmoInventoryUnitCostForeign", "rmoInvParentQuantity", "rmoInvQuantityReceived", "rmoClosed", "rmoInspectionComplete", "rmoJobReceivedComplete", "rmoPostedToGl", "rmoReceivedComplete", "rmoReversed", "rmoJobAssemblyID",
			"rmoJobID", "rmoJobMaterialComponentID", "rmoJobMaterialID", "rmoJobParentQuantity", "rmoJobQuantityReceived", "rmoPartBinID", "rmoPartID", "rmoPartRevisionID", "rmoPartWarehouseLocationID", "rmoPurchaseOrderComponentID",
			"rmoPurchaseOrderID", "rmoPurchaseOrderLineID", "rmoPurchaseUnitCost", "rmoPurchaseUnitCostForeign", "rmoQuantityPerParent", "rmoQuantityToInspect", "rmoReceiptID", "rmoReceiptLineID", "rmoReverseReceiptComponentID", "rmoReverseReceiptID",
			"rmoReverseReceiptLineID", "rmoRowVersion", "rmoReceiptComponentID", "rmoUnitOfMeasure", "rmoWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rmoUniqueID|C", receiptComponentId);
		AddCustomFieldsToSelectList("ReceiptComponents");
		using (DataTable dataTable = GetAsDataTable("ReceiptComponents", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPReceiptComponentInformationDto);
			}
			eRPReceiptComponentInformationDto.rmoAdditionalQuantity = dataTable.Rows[0].Field<decimal>("rmoAdditionalQuantity");
			eRPReceiptComponentInformationDto.rmoConversionFactor = dataTable.Rows[0].Field<decimal>("rmoConversionFactor");
			eRPReceiptComponentInformationDto.rmoCreatedBy = dataTable.Rows[0].Field<string>("rmoCreatedBy");
			eRPReceiptComponentInformationDto.rmoCreatedDate = dataTable.Rows[0].Field<DateTime?>("rmoCreatedDate");
			eRPReceiptComponentInformationDto.rmoDescription = dataTable.Rows[0].Field<string>("rmoDescription");
			eRPReceiptComponentInformationDto.rmoUniqueID = dataTable.Rows[0].Field<Guid>("rmoUniqueID");
			eRPReceiptComponentInformationDto.rmoExtendedCostBase = dataTable.Rows[0].Field<decimal>("rmoExtendedCostBase");
			eRPReceiptComponentInformationDto.rmoExtendedCostForeign = dataTable.Rows[0].Field<decimal>("rmoExtendedCostForeign");
			eRPReceiptComponentInformationDto.rmoInspParentQuantity = dataTable.Rows[0].Field<decimal>("rmoInspParentQuantity");
			eRPReceiptComponentInformationDto.rmoInventoryUnitCost = dataTable.Rows[0].Field<decimal>("rmoInventoryUnitCost");
			eRPReceiptComponentInformationDto.rmoInventoryUnitCostForeign = dataTable.Rows[0].Field<decimal>("rmoInventoryUnitCostForeign");
			eRPReceiptComponentInformationDto.rmoInvParentQuantity = dataTable.Rows[0].Field<decimal>("rmoInvParentQuantity");
			eRPReceiptComponentInformationDto.rmoInvQuantityReceived = dataTable.Rows[0].Field<decimal>("rmoInvQuantityReceived");
			eRPReceiptComponentInformationDto.rmoClosed = dataTable.Rows[0].Field<bool>("rmoClosed");
			eRPReceiptComponentInformationDto.rmoInspectionComplete = dataTable.Rows[0].Field<bool>("rmoInspectionComplete");
			eRPReceiptComponentInformationDto.rmoJobReceivedComplete = dataTable.Rows[0].Field<bool>("rmoJobReceivedComplete");
			eRPReceiptComponentInformationDto.rmoPostedToGl = dataTable.Rows[0].Field<bool>("rmoPostedToGl");
			eRPReceiptComponentInformationDto.rmoReceivedComplete = dataTable.Rows[0].Field<bool>("rmoReceivedComplete");
			eRPReceiptComponentInformationDto.rmoReversed = dataTable.Rows[0].Field<bool>("rmoReversed");
			eRPReceiptComponentInformationDto.rmoJobAssemblyID = dataTable.Rows[0].Field<int>("rmoJobAssemblyID");
			eRPReceiptComponentInformationDto.rmoJobID = dataTable.Rows[0].Field<string>("rmoJobID");
			eRPReceiptComponentInformationDto.rmoJobMaterialComponentID = dataTable.Rows[0].Field<int>("rmoJobMaterialComponentID");
			eRPReceiptComponentInformationDto.rmoJobMaterialID = dataTable.Rows[0].Field<int>("rmoJobMaterialID");
			eRPReceiptComponentInformationDto.rmoJobParentQuantity = dataTable.Rows[0].Field<decimal>("rmoJobParentQuantity");
			eRPReceiptComponentInformationDto.rmoJobQuantityReceived = dataTable.Rows[0].Field<decimal>("rmoJobQuantityReceived");
			eRPReceiptComponentInformationDto.rmoPartBinID = dataTable.Rows[0].Field<string>("rmoPartBinID");
			eRPReceiptComponentInformationDto.rmoPartID = dataTable.Rows[0].Field<string>("rmoPartID");
			eRPReceiptComponentInformationDto.rmoPartRevisionID = dataTable.Rows[0].Field<string>("rmoPartRevisionID");
			eRPReceiptComponentInformationDto.rmoPartWarehouseLocationID = dataTable.Rows[0].Field<string>("rmoPartWarehouseLocationID");
			eRPReceiptComponentInformationDto.rmoPurchaseOrderComponentID = dataTable.Rows[0].Field<short>("rmoPurchaseOrderComponentID");
			eRPReceiptComponentInformationDto.rmoPurchaseOrderID = dataTable.Rows[0].Field<string>("rmoPurchaseOrderID");
			eRPReceiptComponentInformationDto.rmoPurchaseOrderLineID = dataTable.Rows[0].Field<short>("rmoPurchaseOrderLineID");
			eRPReceiptComponentInformationDto.rmoPurchaseUnitCost = dataTable.Rows[0].Field<decimal>("rmoPurchaseUnitCost");
			eRPReceiptComponentInformationDto.rmoPurchaseUnitCostForeign = dataTable.Rows[0].Field<decimal>("rmoPurchaseUnitCostForeign");
			eRPReceiptComponentInformationDto.rmoQuantityPerParent = dataTable.Rows[0].Field<decimal>("rmoQuantityPerParent");
			eRPReceiptComponentInformationDto.rmoQuantityToInspect = dataTable.Rows[0].Field<decimal>("rmoQuantityToInspect");
			eRPReceiptComponentInformationDto.rmoReceiptID = dataTable.Rows[0].Field<string>("rmoReceiptID");
			eRPReceiptComponentInformationDto.rmoReceiptLineID = dataTable.Rows[0].Field<short>("rmoReceiptLineID");
			eRPReceiptComponentInformationDto.rmoReverseReceiptComponentID = dataTable.Rows[0].Field<short>("rmoReverseReceiptComponentID");
			eRPReceiptComponentInformationDto.rmoReverseReceiptID = dataTable.Rows[0].Field<string>("rmoReverseReceiptID");
			eRPReceiptComponentInformationDto.rmoReverseReceiptLineID = dataTable.Rows[0].Field<short>("rmoReverseReceiptLineID");
			eRPReceiptComponentInformationDto.rmoRowVersion = dataTable.Rows[0].Field<byte[]>("rmoRowVersion");
			eRPReceiptComponentInformationDto.rmoReceiptComponentID = dataTable.Rows[0].Field<short>("rmoReceiptComponentID");
			eRPReceiptComponentInformationDto.rmoUnitOfMeasure = dataTable.Rows[0].Field<string>("rmoUnitOfMeasure");
			eRPReceiptComponentInformationDto.rmoWeight = dataTable.Rows[0].Field<decimal>("rmoWeight");
			eRPReceiptComponentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPReceiptComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPReceiptComponentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveReceiptComponent(ERPReceiptComponentDto receiptComponent)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ReceiptComponents WHERE rmoUniqueID = " + M1Util.ConvertToLinq(receiptComponent.rmoUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["rmoReceiptID"] = receiptComponent.rmoReceiptID.ToUpper();
				dataRow["rmoReceiptLineID"] = receiptComponent.rmoReceiptLineID;
				dataRow["rmoReceiptComponentID"] = receiptComponent.rmoReceiptComponentID;
				receiptComponent.rmoUniqueID = ((receiptComponent.rmoUniqueID == Guid.Empty) ? Guid.NewGuid() : receiptComponent.rmoUniqueID);
				dataRow["rmoUniqueID"] = receiptComponent.rmoUniqueID;
				dataRow["rmoCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["rmoCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ReceiptComponent could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (receiptComponent.rmoRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ReceiptComponent is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["rmoRowVersion"], receiptComponent.rmoRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ReceiptComponent has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ReceiptComponent again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["rmoAdditionalQuantity"] = receiptComponent.rmoAdditionalQuantity;
			dataRow["rmoConversionFactor"] = receiptComponent.rmoConversionFactor;
			dataRow["rmoDescription"] = receiptComponent.rmoDescription;
			dataRow["rmoExtendedCostBase"] = receiptComponent.rmoExtendedCostBase;
			dataRow["rmoExtendedCostForeign"] = receiptComponent.rmoExtendedCostForeign;
			dataRow["rmoInspParentQuantity"] = receiptComponent.rmoInspParentQuantity;
			dataRow["rmoInventoryUnitCost"] = receiptComponent.rmoInventoryUnitCost;
			dataRow["rmoInventoryUnitCostForeign"] = receiptComponent.rmoInventoryUnitCostForeign;
			dataRow["rmoInvParentQuantity"] = receiptComponent.rmoInvParentQuantity;
			dataRow["rmoInvQuantityReceived"] = receiptComponent.rmoInvQuantityReceived;
			dataRow["rmoClosed"] = receiptComponent.rmoClosed;
			dataRow["rmoInspectionComplete"] = receiptComponent.rmoInspectionComplete;
			dataRow["rmoJobReceivedComplete"] = receiptComponent.rmoJobReceivedComplete;
			dataRow["rmoPostedToGl"] = receiptComponent.rmoPostedToGl;
			dataRow["rmoReceivedComplete"] = receiptComponent.rmoReceivedComplete;
			dataRow["rmoReversed"] = receiptComponent.rmoReversed;
			dataRow["rmoJobAssemblyID"] = receiptComponent.rmoJobAssemblyID;
			dataRow["rmoJobID"] = receiptComponent.rmoJobID;
			dataRow["rmoJobMaterialComponentID"] = receiptComponent.rmoJobMaterialComponentID;
			dataRow["rmoJobMaterialID"] = receiptComponent.rmoJobMaterialID;
			dataRow["rmoJobParentQuantity"] = receiptComponent.rmoJobParentQuantity;
			dataRow["rmoJobQuantityReceived"] = receiptComponent.rmoJobQuantityReceived;
			dataRow["rmoPartBinID"] = receiptComponent.rmoPartBinID;
			dataRow["rmoPartID"] = receiptComponent.rmoPartID;
			dataRow["rmoPartRevisionID"] = receiptComponent.rmoPartRevisionID;
			dataRow["rmoPartWarehouseLocationID"] = receiptComponent.rmoPartWarehouseLocationID;
			dataRow["rmoPurchaseOrderComponentID"] = receiptComponent.rmoPurchaseOrderComponentID;
			dataRow["rmoPurchaseOrderID"] = receiptComponent.rmoPurchaseOrderID;
			dataRow["rmoPurchaseOrderLineID"] = receiptComponent.rmoPurchaseOrderLineID;
			dataRow["rmoPurchaseUnitCost"] = receiptComponent.rmoPurchaseUnitCost;
			dataRow["rmoPurchaseUnitCostForeign"] = receiptComponent.rmoPurchaseUnitCostForeign;
			dataRow["rmoQuantityPerParent"] = receiptComponent.rmoQuantityPerParent;
			dataRow["rmoQuantityToInspect"] = receiptComponent.rmoQuantityToInspect;
			dataRow["rmoReverseReceiptComponentID"] = receiptComponent.rmoReverseReceiptComponentID;
			dataRow["rmoReverseReceiptID"] = receiptComponent.rmoReverseReceiptID;
			dataRow["rmoReverseReceiptLineID"] = receiptComponent.rmoReverseReceiptLineID;
			dataRow["rmoUnitOfMeasure"] = receiptComponent.rmoUnitOfMeasure;
			dataRow["rmoWeight"] = receiptComponent.rmoWeight;
			if (receiptComponent.CustomFields != null && receiptComponent.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in receiptComponent.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ReceiptComponent [{receiptComponent.rmoUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ReceiptComponent [{receiptComponent.rmoUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
