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

public class ERPPurchasePlannerOrderDetailRepository : APIBaseRepository, IERPPurchasePlannerOrderDetailRepository, IAPIBaseRepository, IDisposable
{
	public ERPPurchasePlannerOrderDetailRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPurchasePlannerOrderDetailExist(Guid purchasePlannerOrderDetailId)
	{
		InitializeParameterLists();
		base.filterList.Add("ppoUniqueID|C", purchasePlannerOrderDetailId);
		base.selectList.Add("ppoUniqueID");
		return Task.FromResult(GetAsObject("PurchasePlannerOrderDetails", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPurchasePlannerOrderDetailInformationDto>> GetAllPurchasePlannerOrderDetails(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPurchasePlannerOrderDetailInformationDto> collection = new List<ERPPurchasePlannerOrderDetailInformationDto>();
		InitializeParameterLists();
		string[] array = new string[36]
		{
			"ppoConversionFactor", "ppoCreatedBy", "ppoCreatedDate", "ppoCurrencyRateID", "ppoDataMissing", "ppoDueDate", "ppoUniqueID", "ppoExtendedCostBase", "ppoInventoryQuantity", "ppoInventoryUnitOfMeasure",
			"ppoCompleted", "ppoSupplierRequirement", "ppoJobAssemblyID", "ppoJobID", "ppoJobMaterialID", "ppoLeadTime", "ppoLineID", "ppoOrderDetailID", "ppoPartBinID", "ppoPartID",
			"ppoPartRevisionID", "ppoPartWarehouseLocationID", "ppoProjectAreaID", "ppoProjectID", "ppoPurchaseLocationID", "ppoPurchaseQuantity", "ppoPurchaseType", "ppoPurchaseUnitOfMeasure", "ppoRowVersion", "ppoSalesOrderDeliveryID",
			"ppoSalesOrderID", "ppoSalesOrderLineID", "ppoSessionID", "ppoSupplierOrganizationID", "ppoUnitCostBase", "ppoUnitCostForeign"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PurchasePlannerOrderDetails");
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
		using (DataTable dataTable = GetAsDataTable("PurchasePlannerOrderDetails", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPurchasePlannerOrderDetailInformationDto eRPPurchasePlannerOrderDetailInformationDto = new ERPPurchasePlannerOrderDetailInformationDto();
				eRPPurchasePlannerOrderDetailInformationDto.ppoConversionFactor = dataTable.Rows[i].Field<decimal>("ppoConversionFactor");
				eRPPurchasePlannerOrderDetailInformationDto.ppoCreatedBy = dataTable.Rows[i].Field<string>("ppoCreatedBy");
				eRPPurchasePlannerOrderDetailInformationDto.ppoCreatedDate = dataTable.Rows[i].Field<DateTime?>("ppoCreatedDate");
				eRPPurchasePlannerOrderDetailInformationDto.ppoCurrencyRateID = dataTable.Rows[i].Field<string>("ppoCurrencyRateID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoDataMissing = dataTable.Rows[i].Field<int>("ppoDataMissing");
				eRPPurchasePlannerOrderDetailInformationDto.ppoDueDate = dataTable.Rows[i].Field<DateTime?>("ppoDueDate");
				eRPPurchasePlannerOrderDetailInformationDto.ppoUniqueID = dataTable.Rows[i].Field<Guid>("ppoUniqueID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoExtendedCostBase = dataTable.Rows[i].Field<decimal>("ppoExtendedCostBase");
				eRPPurchasePlannerOrderDetailInformationDto.ppoInventoryQuantity = dataTable.Rows[i].Field<decimal>("ppoInventoryQuantity");
				eRPPurchasePlannerOrderDetailInformationDto.ppoInventoryUnitOfMeasure = dataTable.Rows[i].Field<string>("ppoInventoryUnitOfMeasure");
				eRPPurchasePlannerOrderDetailInformationDto.ppoCompleted = dataTable.Rows[i].Field<bool>("ppoCompleted");
				eRPPurchasePlannerOrderDetailInformationDto.ppoSupplierRequirement = dataTable.Rows[i].Field<bool>("ppoSupplierRequirement");
				eRPPurchasePlannerOrderDetailInformationDto.ppoJobAssemblyID = dataTable.Rows[i].Field<int>("ppoJobAssemblyID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoJobID = dataTable.Rows[i].Field<string>("ppoJobID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoJobMaterialID = dataTable.Rows[i].Field<int>("ppoJobMaterialID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoLeadTime = dataTable.Rows[i].Field<short>("ppoLeadTime");
				eRPPurchasePlannerOrderDetailInformationDto.ppoLineID = dataTable.Rows[i].Field<int>("ppoLineID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoOrderDetailID = dataTable.Rows[i].Field<int>("ppoOrderDetailID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoPartBinID = dataTable.Rows[i].Field<string>("ppoPartBinID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoPartID = dataTable.Rows[i].Field<string>("ppoPartID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoPartRevisionID = dataTable.Rows[i].Field<string>("ppoPartRevisionID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoPartWarehouseLocationID = dataTable.Rows[i].Field<string>("ppoPartWarehouseLocationID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoProjectAreaID = dataTable.Rows[i].Field<string>("ppoProjectAreaID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoProjectID = dataTable.Rows[i].Field<string>("ppoProjectID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoPurchaseLocationID = dataTable.Rows[i].Field<string>("ppoPurchaseLocationID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoPurchaseQuantity = dataTable.Rows[i].Field<decimal>("ppoPurchaseQuantity");
				eRPPurchasePlannerOrderDetailInformationDto.ppoPurchaseType = dataTable.Rows[i].Field<byte>("ppoPurchaseType");
				eRPPurchasePlannerOrderDetailInformationDto.ppoPurchaseUnitOfMeasure = dataTable.Rows[i].Field<string>("ppoPurchaseUnitOfMeasure");
				eRPPurchasePlannerOrderDetailInformationDto.ppoRowVersion = dataTable.Rows[i].Field<byte[]>("ppoRowVersion");
				eRPPurchasePlannerOrderDetailInformationDto.ppoSalesOrderDeliveryID = dataTable.Rows[i].Field<short>("ppoSalesOrderDeliveryID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoSalesOrderID = dataTable.Rows[i].Field<string>("ppoSalesOrderID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoSalesOrderLineID = dataTable.Rows[i].Field<short>("ppoSalesOrderLineID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoSessionID = dataTable.Rows[i].Field<string>("ppoSessionID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoSupplierOrganizationID = dataTable.Rows[i].Field<string>("ppoSupplierOrganizationID");
				eRPPurchasePlannerOrderDetailInformationDto.ppoUnitCostBase = dataTable.Rows[i].Field<decimal>("ppoUnitCostBase");
				eRPPurchasePlannerOrderDetailInformationDto.ppoUnitCostForeign = dataTable.Rows[i].Field<decimal>("ppoUnitCostForeign");
				eRPPurchasePlannerOrderDetailInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPurchasePlannerOrderDetailInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPurchasePlannerOrderDetailInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPurchasePlannerOrderDetailInformationDto> GetPurchasePlannerOrderDetail(Guid purchasePlannerOrderDetailId)
	{
		ERPPurchasePlannerOrderDetailInformationDto eRPPurchasePlannerOrderDetailInformationDto = new ERPPurchasePlannerOrderDetailInformationDto();
		InitializeParameterLists();
		string[] collection = new string[36]
		{
			"ppoConversionFactor", "ppoCreatedBy", "ppoCreatedDate", "ppoCurrencyRateID", "ppoDataMissing", "ppoDueDate", "ppoUniqueID", "ppoExtendedCostBase", "ppoInventoryQuantity", "ppoInventoryUnitOfMeasure",
			"ppoCompleted", "ppoSupplierRequirement", "ppoJobAssemblyID", "ppoJobID", "ppoJobMaterialID", "ppoLeadTime", "ppoLineID", "ppoOrderDetailID", "ppoPartBinID", "ppoPartID",
			"ppoPartRevisionID", "ppoPartWarehouseLocationID", "ppoProjectAreaID", "ppoProjectID", "ppoPurchaseLocationID", "ppoPurchaseQuantity", "ppoPurchaseType", "ppoPurchaseUnitOfMeasure", "ppoRowVersion", "ppoSalesOrderDeliveryID",
			"ppoSalesOrderID", "ppoSalesOrderLineID", "ppoSessionID", "ppoSupplierOrganizationID", "ppoUnitCostBase", "ppoUnitCostForeign"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("ppoUniqueID|C", purchasePlannerOrderDetailId);
		AddCustomFieldsToSelectList("PurchasePlannerOrderDetails");
		using (DataTable dataTable = GetAsDataTable("PurchasePlannerOrderDetails", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPurchasePlannerOrderDetailInformationDto);
			}
			eRPPurchasePlannerOrderDetailInformationDto.ppoConversionFactor = dataTable.Rows[0].Field<decimal>("ppoConversionFactor");
			eRPPurchasePlannerOrderDetailInformationDto.ppoCreatedBy = dataTable.Rows[0].Field<string>("ppoCreatedBy");
			eRPPurchasePlannerOrderDetailInformationDto.ppoCreatedDate = dataTable.Rows[0].Field<DateTime?>("ppoCreatedDate");
			eRPPurchasePlannerOrderDetailInformationDto.ppoCurrencyRateID = dataTable.Rows[0].Field<string>("ppoCurrencyRateID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoDataMissing = dataTable.Rows[0].Field<int>("ppoDataMissing");
			eRPPurchasePlannerOrderDetailInformationDto.ppoDueDate = dataTable.Rows[0].Field<DateTime?>("ppoDueDate");
			eRPPurchasePlannerOrderDetailInformationDto.ppoUniqueID = dataTable.Rows[0].Field<Guid>("ppoUniqueID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoExtendedCostBase = dataTable.Rows[0].Field<decimal>("ppoExtendedCostBase");
			eRPPurchasePlannerOrderDetailInformationDto.ppoInventoryQuantity = dataTable.Rows[0].Field<decimal>("ppoInventoryQuantity");
			eRPPurchasePlannerOrderDetailInformationDto.ppoInventoryUnitOfMeasure = dataTable.Rows[0].Field<string>("ppoInventoryUnitOfMeasure");
			eRPPurchasePlannerOrderDetailInformationDto.ppoCompleted = dataTable.Rows[0].Field<bool>("ppoCompleted");
			eRPPurchasePlannerOrderDetailInformationDto.ppoSupplierRequirement = dataTable.Rows[0].Field<bool>("ppoSupplierRequirement");
			eRPPurchasePlannerOrderDetailInformationDto.ppoJobAssemblyID = dataTable.Rows[0].Field<int>("ppoJobAssemblyID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoJobID = dataTable.Rows[0].Field<string>("ppoJobID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoJobMaterialID = dataTable.Rows[0].Field<int>("ppoJobMaterialID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoLeadTime = dataTable.Rows[0].Field<short>("ppoLeadTime");
			eRPPurchasePlannerOrderDetailInformationDto.ppoLineID = dataTable.Rows[0].Field<int>("ppoLineID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoOrderDetailID = dataTable.Rows[0].Field<int>("ppoOrderDetailID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoPartBinID = dataTable.Rows[0].Field<string>("ppoPartBinID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoPartID = dataTable.Rows[0].Field<string>("ppoPartID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoPartRevisionID = dataTable.Rows[0].Field<string>("ppoPartRevisionID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoPartWarehouseLocationID = dataTable.Rows[0].Field<string>("ppoPartWarehouseLocationID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoProjectAreaID = dataTable.Rows[0].Field<string>("ppoProjectAreaID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoProjectID = dataTable.Rows[0].Field<string>("ppoProjectID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoPurchaseLocationID = dataTable.Rows[0].Field<string>("ppoPurchaseLocationID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoPurchaseQuantity = dataTable.Rows[0].Field<decimal>("ppoPurchaseQuantity");
			eRPPurchasePlannerOrderDetailInformationDto.ppoPurchaseType = dataTable.Rows[0].Field<byte>("ppoPurchaseType");
			eRPPurchasePlannerOrderDetailInformationDto.ppoPurchaseUnitOfMeasure = dataTable.Rows[0].Field<string>("ppoPurchaseUnitOfMeasure");
			eRPPurchasePlannerOrderDetailInformationDto.ppoRowVersion = dataTable.Rows[0].Field<byte[]>("ppoRowVersion");
			eRPPurchasePlannerOrderDetailInformationDto.ppoSalesOrderDeliveryID = dataTable.Rows[0].Field<short>("ppoSalesOrderDeliveryID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoSalesOrderID = dataTable.Rows[0].Field<string>("ppoSalesOrderID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoSalesOrderLineID = dataTable.Rows[0].Field<short>("ppoSalesOrderLineID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoSessionID = dataTable.Rows[0].Field<string>("ppoSessionID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoSupplierOrganizationID = dataTable.Rows[0].Field<string>("ppoSupplierOrganizationID");
			eRPPurchasePlannerOrderDetailInformationDto.ppoUnitCostBase = dataTable.Rows[0].Field<decimal>("ppoUnitCostBase");
			eRPPurchasePlannerOrderDetailInformationDto.ppoUnitCostForeign = dataTable.Rows[0].Field<decimal>("ppoUnitCostForeign");
			eRPPurchasePlannerOrderDetailInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPurchasePlannerOrderDetailInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPurchasePlannerOrderDetailInformationDto);
	}

	public Task<APIValidationInfoDto> SavePurchasePlannerOrderDetail(ERPPurchasePlannerOrderDetailDto purchasePlannerOrderDetail)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PurchasePlannerOrderDetails WHERE ppoUniqueID = " + M1Util.ConvertToLinq(purchasePlannerOrderDetail.ppoUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["ppoSessionID"] = purchasePlannerOrderDetail.ppoSessionID.ToUpper();
				dataRow["ppoLineID"] = purchasePlannerOrderDetail.ppoLineID;
				dataRow["ppoOrderDetailID"] = purchasePlannerOrderDetail.ppoOrderDetailID;
				purchasePlannerOrderDetail.ppoUniqueID = ((purchasePlannerOrderDetail.ppoUniqueID == Guid.Empty) ? Guid.NewGuid() : purchasePlannerOrderDetail.ppoUniqueID);
				dataRow["ppoUniqueID"] = purchasePlannerOrderDetail.ppoUniqueID;
				dataRow["ppoCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["ppoCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PurchasePlannerOrderDetail could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (purchasePlannerOrderDetail.ppoRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PurchasePlannerOrderDetail is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["ppoRowVersion"], purchasePlannerOrderDetail.ppoRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PurchasePlannerOrderDetail has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PurchasePlannerOrderDetail again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["ppoConversionFactor"] = purchasePlannerOrderDetail.ppoConversionFactor;
			dataRow["ppoCurrencyRateID"] = purchasePlannerOrderDetail.ppoCurrencyRateID;
			dataRow["ppoDataMissing"] = purchasePlannerOrderDetail.ppoDataMissing;
			DataRow dataRow2 = dataRow;
			DateTime? ppoDueDate = purchasePlannerOrderDetail.ppoDueDate;
			dataRow2["ppoDueDate"] = (ppoDueDate.HasValue ? ((object)ppoDueDate.GetValueOrDefault()) : dataRow["ppoDueDate"]);
			dataRow["ppoExtendedCostBase"] = purchasePlannerOrderDetail.ppoExtendedCostBase;
			dataRow["ppoInventoryQuantity"] = purchasePlannerOrderDetail.ppoInventoryQuantity;
			dataRow["ppoInventoryUnitOfMeasure"] = purchasePlannerOrderDetail.ppoInventoryUnitOfMeasure;
			dataRow["ppoCompleted"] = purchasePlannerOrderDetail.ppoCompleted;
			dataRow["ppoSupplierRequirement"] = purchasePlannerOrderDetail.ppoSupplierRequirement;
			dataRow["ppoJobAssemblyID"] = purchasePlannerOrderDetail.ppoJobAssemblyID;
			dataRow["ppoJobID"] = purchasePlannerOrderDetail.ppoJobID;
			dataRow["ppoJobMaterialID"] = purchasePlannerOrderDetail.ppoJobMaterialID;
			dataRow["ppoLeadTime"] = purchasePlannerOrderDetail.ppoLeadTime;
			dataRow["ppoPartBinID"] = purchasePlannerOrderDetail.ppoPartBinID;
			dataRow["ppoPartID"] = purchasePlannerOrderDetail.ppoPartID;
			dataRow["ppoPartRevisionID"] = purchasePlannerOrderDetail.ppoPartRevisionID;
			dataRow["ppoPartWarehouseLocationID"] = purchasePlannerOrderDetail.ppoPartWarehouseLocationID;
			dataRow["ppoProjectAreaID"] = purchasePlannerOrderDetail.ppoProjectAreaID;
			dataRow["ppoProjectID"] = purchasePlannerOrderDetail.ppoProjectID;
			dataRow["ppoPurchaseLocationID"] = purchasePlannerOrderDetail.ppoPurchaseLocationID;
			dataRow["ppoPurchaseQuantity"] = purchasePlannerOrderDetail.ppoPurchaseQuantity;
			dataRow["ppoPurchaseType"] = purchasePlannerOrderDetail.ppoPurchaseType;
			dataRow["ppoPurchaseUnitOfMeasure"] = purchasePlannerOrderDetail.ppoPurchaseUnitOfMeasure;
			dataRow["ppoSalesOrderDeliveryID"] = purchasePlannerOrderDetail.ppoSalesOrderDeliveryID;
			dataRow["ppoSalesOrderID"] = purchasePlannerOrderDetail.ppoSalesOrderID;
			dataRow["ppoSalesOrderLineID"] = purchasePlannerOrderDetail.ppoSalesOrderLineID;
			dataRow["ppoSupplierOrganizationID"] = purchasePlannerOrderDetail.ppoSupplierOrganizationID;
			dataRow["ppoUnitCostBase"] = purchasePlannerOrderDetail.ppoUnitCostBase;
			dataRow["ppoUnitCostForeign"] = purchasePlannerOrderDetail.ppoUnitCostForeign;
			if (purchasePlannerOrderDetail.CustomFields != null && purchasePlannerOrderDetail.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in purchasePlannerOrderDetail.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PurchasePlannerOrderDetail [{purchasePlannerOrderDetail.ppoUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PurchasePlannerOrderDetail [{purchasePlannerOrderDetail.ppoUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
