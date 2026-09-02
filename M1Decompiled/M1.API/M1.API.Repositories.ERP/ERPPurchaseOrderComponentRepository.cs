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

public class ERPPurchaseOrderComponentRepository : APIBaseRepository, IERPPurchaseOrderComponentRepository, IAPIBaseRepository, IDisposable
{
	public ERPPurchaseOrderComponentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPurchaseOrderComponentExist(Guid purchaseOrderComponentId)
	{
		InitializeParameterLists();
		base.filterList.Add("pmoUniqueID|C", purchaseOrderComponentId);
		base.selectList.Add("pmoUniqueID");
		return Task.FromResult(GetAsObject("PurchaseOrderComponents", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPurchaseOrderComponentInformationDto>> GetAllPurchaseOrderComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPurchaseOrderComponentInformationDto> collection = new List<ERPPurchaseOrderComponentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[30]
		{
			"pmoAdditionalQuantity", "pmoCreatedBy", "pmoCreatedDate", "pmoDeliveryQuantity", "pmoDescription", "pmoUniqueID", "pmoExtendedCostBase", "pmoExtendedCostForeign", "pmoClosed", "pmoIntraCompanyPosted",
			"pmoReceivedComplete", "pmoJobAssemblyID", "pmoJobID", "pmoJobMaterialComponentID", "pmoJobMaterialID", "pmoParentQuantity", "pmoPartBinID", "pmoPartID", "pmoPartRevisionID", "pmoPartWarehouseLocationID",
			"pmoPurchaseOrderID", "pmoPurchaseOrderLineID", "pmoPurchaseUnitCost", "pmoPurchaseUnitCostForeign", "pmoQuantityPerParent", "pmoQuantityReceived", "pmoRowVersion", "pmoPurchaseOrderComponentID", "pmoUnitOfMeasure", "pmoWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PurchaseOrderComponents");
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
		using (DataTable dataTable = GetAsDataTable("PurchaseOrderComponents", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPurchaseOrderComponentInformationDto eRPPurchaseOrderComponentInformationDto = new ERPPurchaseOrderComponentInformationDto();
				eRPPurchaseOrderComponentInformationDto.pmoAdditionalQuantity = dataTable.Rows[i].Field<decimal>("pmoAdditionalQuantity");
				eRPPurchaseOrderComponentInformationDto.pmoCreatedBy = dataTable.Rows[i].Field<string>("pmoCreatedBy");
				eRPPurchaseOrderComponentInformationDto.pmoCreatedDate = dataTable.Rows[i].Field<DateTime?>("pmoCreatedDate");
				eRPPurchaseOrderComponentInformationDto.pmoDeliveryQuantity = dataTable.Rows[i].Field<decimal>("pmoDeliveryQuantity");
				eRPPurchaseOrderComponentInformationDto.pmoDescription = dataTable.Rows[i].Field<string>("pmoDescription");
				eRPPurchaseOrderComponentInformationDto.pmoUniqueID = dataTable.Rows[i].Field<Guid>("pmoUniqueID");
				eRPPurchaseOrderComponentInformationDto.pmoExtendedCostBase = dataTable.Rows[i].Field<decimal>("pmoExtendedCostBase");
				eRPPurchaseOrderComponentInformationDto.pmoExtendedCostForeign = dataTable.Rows[i].Field<decimal>("pmoExtendedCostForeign");
				eRPPurchaseOrderComponentInformationDto.pmoClosed = dataTable.Rows[i].Field<bool>("pmoClosed");
				eRPPurchaseOrderComponentInformationDto.pmoIntraCompanyPosted = dataTable.Rows[i].Field<bool>("pmoIntraCompanyPosted");
				eRPPurchaseOrderComponentInformationDto.pmoReceivedComplete = dataTable.Rows[i].Field<bool>("pmoReceivedComplete");
				eRPPurchaseOrderComponentInformationDto.pmoJobAssemblyID = dataTable.Rows[i].Field<int>("pmoJobAssemblyID");
				eRPPurchaseOrderComponentInformationDto.pmoJobID = dataTable.Rows[i].Field<string>("pmoJobID");
				eRPPurchaseOrderComponentInformationDto.pmoJobMaterialComponentID = dataTable.Rows[i].Field<int>("pmoJobMaterialComponentID");
				eRPPurchaseOrderComponentInformationDto.pmoJobMaterialID = dataTable.Rows[i].Field<int>("pmoJobMaterialID");
				eRPPurchaseOrderComponentInformationDto.pmoParentQuantity = dataTable.Rows[i].Field<decimal>("pmoParentQuantity");
				eRPPurchaseOrderComponentInformationDto.pmoPartBinID = dataTable.Rows[i].Field<string>("pmoPartBinID");
				eRPPurchaseOrderComponentInformationDto.pmoPartID = dataTable.Rows[i].Field<string>("pmoPartID");
				eRPPurchaseOrderComponentInformationDto.pmoPartRevisionID = dataTable.Rows[i].Field<string>("pmoPartRevisionID");
				eRPPurchaseOrderComponentInformationDto.pmoPartWarehouseLocationID = dataTable.Rows[i].Field<string>("pmoPartWarehouseLocationID");
				eRPPurchaseOrderComponentInformationDto.pmoPurchaseOrderID = dataTable.Rows[i].Field<string>("pmoPurchaseOrderID");
				eRPPurchaseOrderComponentInformationDto.pmoPurchaseOrderLineID = dataTable.Rows[i].Field<short>("pmoPurchaseOrderLineID");
				eRPPurchaseOrderComponentInformationDto.pmoPurchaseUnitCost = dataTable.Rows[i].Field<decimal>("pmoPurchaseUnitCost");
				eRPPurchaseOrderComponentInformationDto.pmoPurchaseUnitCostForeign = dataTable.Rows[i].Field<decimal>("pmoPurchaseUnitCostForeign");
				eRPPurchaseOrderComponentInformationDto.pmoQuantityPerParent = dataTable.Rows[i].Field<decimal>("pmoQuantityPerParent");
				eRPPurchaseOrderComponentInformationDto.pmoQuantityReceived = dataTable.Rows[i].Field<decimal>("pmoQuantityReceived");
				eRPPurchaseOrderComponentInformationDto.pmoRowVersion = dataTable.Rows[i].Field<byte[]>("pmoRowVersion");
				eRPPurchaseOrderComponentInformationDto.pmoPurchaseOrderComponentID = dataTable.Rows[i].Field<short>("pmoPurchaseOrderComponentID");
				eRPPurchaseOrderComponentInformationDto.pmoUnitOfMeasure = dataTable.Rows[i].Field<string>("pmoUnitOfMeasure");
				eRPPurchaseOrderComponentInformationDto.pmoWeight = dataTable.Rows[i].Field<decimal>("pmoWeight");
				eRPPurchaseOrderComponentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPurchaseOrderComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPurchaseOrderComponentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPurchaseOrderComponentInformationDto> GetPurchaseOrderComponent(Guid purchaseOrderComponentId)
	{
		ERPPurchaseOrderComponentInformationDto eRPPurchaseOrderComponentInformationDto = new ERPPurchaseOrderComponentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[30]
		{
			"pmoAdditionalQuantity", "pmoCreatedBy", "pmoCreatedDate", "pmoDeliveryQuantity", "pmoDescription", "pmoUniqueID", "pmoExtendedCostBase", "pmoExtendedCostForeign", "pmoClosed", "pmoIntraCompanyPosted",
			"pmoReceivedComplete", "pmoJobAssemblyID", "pmoJobID", "pmoJobMaterialComponentID", "pmoJobMaterialID", "pmoParentQuantity", "pmoPartBinID", "pmoPartID", "pmoPartRevisionID", "pmoPartWarehouseLocationID",
			"pmoPurchaseOrderID", "pmoPurchaseOrderLineID", "pmoPurchaseUnitCost", "pmoPurchaseUnitCostForeign", "pmoQuantityPerParent", "pmoQuantityReceived", "pmoRowVersion", "pmoPurchaseOrderComponentID", "pmoUnitOfMeasure", "pmoWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("pmoUniqueID|C", purchaseOrderComponentId);
		AddCustomFieldsToSelectList("PurchaseOrderComponents");
		using (DataTable dataTable = GetAsDataTable("PurchaseOrderComponents", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPurchaseOrderComponentInformationDto);
			}
			eRPPurchaseOrderComponentInformationDto.pmoAdditionalQuantity = dataTable.Rows[0].Field<decimal>("pmoAdditionalQuantity");
			eRPPurchaseOrderComponentInformationDto.pmoCreatedBy = dataTable.Rows[0].Field<string>("pmoCreatedBy");
			eRPPurchaseOrderComponentInformationDto.pmoCreatedDate = dataTable.Rows[0].Field<DateTime?>("pmoCreatedDate");
			eRPPurchaseOrderComponentInformationDto.pmoDeliveryQuantity = dataTable.Rows[0].Field<decimal>("pmoDeliveryQuantity");
			eRPPurchaseOrderComponentInformationDto.pmoDescription = dataTable.Rows[0].Field<string>("pmoDescription");
			eRPPurchaseOrderComponentInformationDto.pmoUniqueID = dataTable.Rows[0].Field<Guid>("pmoUniqueID");
			eRPPurchaseOrderComponentInformationDto.pmoExtendedCostBase = dataTable.Rows[0].Field<decimal>("pmoExtendedCostBase");
			eRPPurchaseOrderComponentInformationDto.pmoExtendedCostForeign = dataTable.Rows[0].Field<decimal>("pmoExtendedCostForeign");
			eRPPurchaseOrderComponentInformationDto.pmoClosed = dataTable.Rows[0].Field<bool>("pmoClosed");
			eRPPurchaseOrderComponentInformationDto.pmoIntraCompanyPosted = dataTable.Rows[0].Field<bool>("pmoIntraCompanyPosted");
			eRPPurchaseOrderComponentInformationDto.pmoReceivedComplete = dataTable.Rows[0].Field<bool>("pmoReceivedComplete");
			eRPPurchaseOrderComponentInformationDto.pmoJobAssemblyID = dataTable.Rows[0].Field<int>("pmoJobAssemblyID");
			eRPPurchaseOrderComponentInformationDto.pmoJobID = dataTable.Rows[0].Field<string>("pmoJobID");
			eRPPurchaseOrderComponentInformationDto.pmoJobMaterialComponentID = dataTable.Rows[0].Field<int>("pmoJobMaterialComponentID");
			eRPPurchaseOrderComponentInformationDto.pmoJobMaterialID = dataTable.Rows[0].Field<int>("pmoJobMaterialID");
			eRPPurchaseOrderComponentInformationDto.pmoParentQuantity = dataTable.Rows[0].Field<decimal>("pmoParentQuantity");
			eRPPurchaseOrderComponentInformationDto.pmoPartBinID = dataTable.Rows[0].Field<string>("pmoPartBinID");
			eRPPurchaseOrderComponentInformationDto.pmoPartID = dataTable.Rows[0].Field<string>("pmoPartID");
			eRPPurchaseOrderComponentInformationDto.pmoPartRevisionID = dataTable.Rows[0].Field<string>("pmoPartRevisionID");
			eRPPurchaseOrderComponentInformationDto.pmoPartWarehouseLocationID = dataTable.Rows[0].Field<string>("pmoPartWarehouseLocationID");
			eRPPurchaseOrderComponentInformationDto.pmoPurchaseOrderID = dataTable.Rows[0].Field<string>("pmoPurchaseOrderID");
			eRPPurchaseOrderComponentInformationDto.pmoPurchaseOrderLineID = dataTable.Rows[0].Field<short>("pmoPurchaseOrderLineID");
			eRPPurchaseOrderComponentInformationDto.pmoPurchaseUnitCost = dataTable.Rows[0].Field<decimal>("pmoPurchaseUnitCost");
			eRPPurchaseOrderComponentInformationDto.pmoPurchaseUnitCostForeign = dataTable.Rows[0].Field<decimal>("pmoPurchaseUnitCostForeign");
			eRPPurchaseOrderComponentInformationDto.pmoQuantityPerParent = dataTable.Rows[0].Field<decimal>("pmoQuantityPerParent");
			eRPPurchaseOrderComponentInformationDto.pmoQuantityReceived = dataTable.Rows[0].Field<decimal>("pmoQuantityReceived");
			eRPPurchaseOrderComponentInformationDto.pmoRowVersion = dataTable.Rows[0].Field<byte[]>("pmoRowVersion");
			eRPPurchaseOrderComponentInformationDto.pmoPurchaseOrderComponentID = dataTable.Rows[0].Field<short>("pmoPurchaseOrderComponentID");
			eRPPurchaseOrderComponentInformationDto.pmoUnitOfMeasure = dataTable.Rows[0].Field<string>("pmoUnitOfMeasure");
			eRPPurchaseOrderComponentInformationDto.pmoWeight = dataTable.Rows[0].Field<decimal>("pmoWeight");
			eRPPurchaseOrderComponentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPurchaseOrderComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPurchaseOrderComponentInformationDto);
	}

	public Task<APIValidationInfoDto> SavePurchaseOrderComponent(ERPPurchaseOrderComponentDto purchaseOrderComponent)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PurchaseOrderComponents WHERE pmoUniqueID = " + M1Util.ConvertToLinq(purchaseOrderComponent.pmoUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["pmoPurchaseOrderID"] = purchaseOrderComponent.pmoPurchaseOrderID.ToUpper();
				dataRow["pmoPurchaseOrderLineID"] = purchaseOrderComponent.pmoPurchaseOrderLineID;
				dataRow["pmoPurchaseOrderComponentID"] = purchaseOrderComponent.pmoPurchaseOrderComponentID;
				purchaseOrderComponent.pmoUniqueID = ((purchaseOrderComponent.pmoUniqueID == Guid.Empty) ? Guid.NewGuid() : purchaseOrderComponent.pmoUniqueID);
				dataRow["pmoUniqueID"] = purchaseOrderComponent.pmoUniqueID;
				dataRow["pmoCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["pmoCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PurchaseOrderComponent could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (purchaseOrderComponent.pmoRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PurchaseOrderComponent is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["pmoRowVersion"], purchaseOrderComponent.pmoRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PurchaseOrderComponent has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PurchaseOrderComponent again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["pmoAdditionalQuantity"] = purchaseOrderComponent.pmoAdditionalQuantity;
			dataRow["pmoDeliveryQuantity"] = purchaseOrderComponent.pmoDeliveryQuantity;
			dataRow["pmoDescription"] = purchaseOrderComponent.pmoDescription;
			dataRow["pmoExtendedCostBase"] = purchaseOrderComponent.pmoExtendedCostBase;
			dataRow["pmoExtendedCostForeign"] = purchaseOrderComponent.pmoExtendedCostForeign;
			dataRow["pmoClosed"] = purchaseOrderComponent.pmoClosed;
			dataRow["pmoIntraCompanyPosted"] = purchaseOrderComponent.pmoIntraCompanyPosted;
			dataRow["pmoReceivedComplete"] = purchaseOrderComponent.pmoReceivedComplete;
			dataRow["pmoJobAssemblyID"] = purchaseOrderComponent.pmoJobAssemblyID;
			dataRow["pmoJobID"] = purchaseOrderComponent.pmoJobID;
			dataRow["pmoJobMaterialComponentID"] = purchaseOrderComponent.pmoJobMaterialComponentID;
			dataRow["pmoJobMaterialID"] = purchaseOrderComponent.pmoJobMaterialID;
			dataRow["pmoParentQuantity"] = purchaseOrderComponent.pmoParentQuantity;
			dataRow["pmoPartBinID"] = purchaseOrderComponent.pmoPartBinID;
			dataRow["pmoPartID"] = purchaseOrderComponent.pmoPartID;
			dataRow["pmoPartRevisionID"] = purchaseOrderComponent.pmoPartRevisionID;
			dataRow["pmoPartWarehouseLocationID"] = purchaseOrderComponent.pmoPartWarehouseLocationID;
			dataRow["pmoPurchaseUnitCost"] = purchaseOrderComponent.pmoPurchaseUnitCost;
			dataRow["pmoPurchaseUnitCostForeign"] = purchaseOrderComponent.pmoPurchaseUnitCostForeign;
			dataRow["pmoQuantityPerParent"] = purchaseOrderComponent.pmoQuantityPerParent;
			dataRow["pmoQuantityReceived"] = purchaseOrderComponent.pmoQuantityReceived;
			dataRow["pmoUnitOfMeasure"] = purchaseOrderComponent.pmoUnitOfMeasure;
			dataRow["pmoWeight"] = purchaseOrderComponent.pmoWeight;
			if (purchaseOrderComponent.CustomFields != null && purchaseOrderComponent.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in purchaseOrderComponent.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PurchaseOrderComponent [{purchaseOrderComponent.pmoUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PurchaseOrderComponent [{purchaseOrderComponent.pmoUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
