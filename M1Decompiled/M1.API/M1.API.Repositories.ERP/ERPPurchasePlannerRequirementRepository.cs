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

public class ERPPurchasePlannerRequirementRepository : APIBaseRepository, IERPPurchasePlannerRequirementRepository, IAPIBaseRepository, IDisposable
{
	public ERPPurchasePlannerRequirementRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPurchasePlannerRequirementExist(Guid purchasePlannerRequirementId)
	{
		InitializeParameterLists();
		base.filterList.Add("pprUniqueID|C", purchasePlannerRequirementId);
		base.selectList.Add("pprUniqueID");
		return Task.FromResult(GetAsObject("PurchasePlannerRequirements", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPurchasePlannerRequirementInformationDto>> GetAllPurchasePlannerRequirements(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPurchasePlannerRequirementInformationDto> collection = new List<ERPPurchasePlannerRequirementInformationDto>();
		InitializeParameterLists();
		string[] array = new string[23]
		{
			"pprCreatedBy", "pprCreatedDate", "pprDueDate", "pprUniqueID", "pprJobAssemblyID", "pprJobID", "pprJobMaterialID", "pprLineID", "pprPlannedReceiptQuantity", "pprPlannedRequirementQuantity",
			"pprProjectedBalance", "pprPullFromStockQuantity", "pprPurchaseOrderDate", "pprPurchaseOrderID", "pprPurchaseToJobQuantity", "pprPurchaseType", "pprRequirementID", "pprRowVersion", "pprSalesOrderDeliveryID", "pprSalesOrderID",
			"pprSalesOrderLineID", "pprSessionID", "pprSource"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PurchasePlannerRequirements");
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
		using (DataTable dataTable = GetAsDataTable("PurchasePlannerRequirements", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPurchasePlannerRequirementInformationDto eRPPurchasePlannerRequirementInformationDto = new ERPPurchasePlannerRequirementInformationDto();
				eRPPurchasePlannerRequirementInformationDto.pprCreatedBy = dataTable.Rows[i].Field<string>("pprCreatedBy");
				eRPPurchasePlannerRequirementInformationDto.pprCreatedDate = dataTable.Rows[i].Field<DateTime?>("pprCreatedDate");
				eRPPurchasePlannerRequirementInformationDto.pprDueDate = dataTable.Rows[i].Field<DateTime?>("pprDueDate");
				eRPPurchasePlannerRequirementInformationDto.pprUniqueID = dataTable.Rows[i].Field<Guid>("pprUniqueID");
				eRPPurchasePlannerRequirementInformationDto.pprJobAssemblyID = dataTable.Rows[i].Field<int>("pprJobAssemblyID");
				eRPPurchasePlannerRequirementInformationDto.pprJobID = dataTable.Rows[i].Field<string>("pprJobID");
				eRPPurchasePlannerRequirementInformationDto.pprJobMaterialID = dataTable.Rows[i].Field<int>("pprJobMaterialID");
				eRPPurchasePlannerRequirementInformationDto.pprLineID = dataTable.Rows[i].Field<int>("pprLineID");
				eRPPurchasePlannerRequirementInformationDto.pprPlannedReceiptQuantity = dataTable.Rows[i].Field<decimal>("pprPlannedReceiptQuantity");
				eRPPurchasePlannerRequirementInformationDto.pprPlannedRequirementQuantity = dataTable.Rows[i].Field<decimal>("pprPlannedRequirementQuantity");
				eRPPurchasePlannerRequirementInformationDto.pprProjectedBalance = dataTable.Rows[i].Field<decimal>("pprProjectedBalance");
				eRPPurchasePlannerRequirementInformationDto.pprPullFromStockQuantity = dataTable.Rows[i].Field<decimal>("pprPullFromStockQuantity");
				eRPPurchasePlannerRequirementInformationDto.pprPurchaseOrderDate = dataTable.Rows[i].Field<DateTime?>("pprPurchaseOrderDate");
				eRPPurchasePlannerRequirementInformationDto.pprPurchaseOrderID = dataTable.Rows[i].Field<string>("pprPurchaseOrderID");
				eRPPurchasePlannerRequirementInformationDto.pprPurchaseToJobQuantity = dataTable.Rows[i].Field<decimal>("pprPurchaseToJobQuantity");
				eRPPurchasePlannerRequirementInformationDto.pprPurchaseType = dataTable.Rows[i].Field<byte>("pprPurchaseType");
				eRPPurchasePlannerRequirementInformationDto.pprRequirementID = dataTable.Rows[i].Field<int>("pprRequirementID");
				eRPPurchasePlannerRequirementInformationDto.pprRowVersion = dataTable.Rows[i].Field<byte[]>("pprRowVersion");
				eRPPurchasePlannerRequirementInformationDto.pprSalesOrderDeliveryID = dataTable.Rows[i].Field<short>("pprSalesOrderDeliveryID");
				eRPPurchasePlannerRequirementInformationDto.pprSalesOrderID = dataTable.Rows[i].Field<string>("pprSalesOrderID");
				eRPPurchasePlannerRequirementInformationDto.pprSalesOrderLineID = dataTable.Rows[i].Field<short>("pprSalesOrderLineID");
				eRPPurchasePlannerRequirementInformationDto.pprSessionID = dataTable.Rows[i].Field<string>("pprSessionID");
				eRPPurchasePlannerRequirementInformationDto.pprSource = dataTable.Rows[i].Field<string>("pprSource");
				eRPPurchasePlannerRequirementInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPurchasePlannerRequirementInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPurchasePlannerRequirementInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPurchasePlannerRequirementInformationDto> GetPurchasePlannerRequirement(Guid purchasePlannerRequirementId)
	{
		ERPPurchasePlannerRequirementInformationDto eRPPurchasePlannerRequirementInformationDto = new ERPPurchasePlannerRequirementInformationDto();
		InitializeParameterLists();
		string[] collection = new string[23]
		{
			"pprCreatedBy", "pprCreatedDate", "pprDueDate", "pprUniqueID", "pprJobAssemblyID", "pprJobID", "pprJobMaterialID", "pprLineID", "pprPlannedReceiptQuantity", "pprPlannedRequirementQuantity",
			"pprProjectedBalance", "pprPullFromStockQuantity", "pprPurchaseOrderDate", "pprPurchaseOrderID", "pprPurchaseToJobQuantity", "pprPurchaseType", "pprRequirementID", "pprRowVersion", "pprSalesOrderDeliveryID", "pprSalesOrderID",
			"pprSalesOrderLineID", "pprSessionID", "pprSource"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("pprUniqueID|C", purchasePlannerRequirementId);
		AddCustomFieldsToSelectList("PurchasePlannerRequirements");
		using (DataTable dataTable = GetAsDataTable("PurchasePlannerRequirements", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPurchasePlannerRequirementInformationDto);
			}
			eRPPurchasePlannerRequirementInformationDto.pprCreatedBy = dataTable.Rows[0].Field<string>("pprCreatedBy");
			eRPPurchasePlannerRequirementInformationDto.pprCreatedDate = dataTable.Rows[0].Field<DateTime?>("pprCreatedDate");
			eRPPurchasePlannerRequirementInformationDto.pprDueDate = dataTable.Rows[0].Field<DateTime?>("pprDueDate");
			eRPPurchasePlannerRequirementInformationDto.pprUniqueID = dataTable.Rows[0].Field<Guid>("pprUniqueID");
			eRPPurchasePlannerRequirementInformationDto.pprJobAssemblyID = dataTable.Rows[0].Field<int>("pprJobAssemblyID");
			eRPPurchasePlannerRequirementInformationDto.pprJobID = dataTable.Rows[0].Field<string>("pprJobID");
			eRPPurchasePlannerRequirementInformationDto.pprJobMaterialID = dataTable.Rows[0].Field<int>("pprJobMaterialID");
			eRPPurchasePlannerRequirementInformationDto.pprLineID = dataTable.Rows[0].Field<int>("pprLineID");
			eRPPurchasePlannerRequirementInformationDto.pprPlannedReceiptQuantity = dataTable.Rows[0].Field<decimal>("pprPlannedReceiptQuantity");
			eRPPurchasePlannerRequirementInformationDto.pprPlannedRequirementQuantity = dataTable.Rows[0].Field<decimal>("pprPlannedRequirementQuantity");
			eRPPurchasePlannerRequirementInformationDto.pprProjectedBalance = dataTable.Rows[0].Field<decimal>("pprProjectedBalance");
			eRPPurchasePlannerRequirementInformationDto.pprPullFromStockQuantity = dataTable.Rows[0].Field<decimal>("pprPullFromStockQuantity");
			eRPPurchasePlannerRequirementInformationDto.pprPurchaseOrderDate = dataTable.Rows[0].Field<DateTime?>("pprPurchaseOrderDate");
			eRPPurchasePlannerRequirementInformationDto.pprPurchaseOrderID = dataTable.Rows[0].Field<string>("pprPurchaseOrderID");
			eRPPurchasePlannerRequirementInformationDto.pprPurchaseToJobQuantity = dataTable.Rows[0].Field<decimal>("pprPurchaseToJobQuantity");
			eRPPurchasePlannerRequirementInformationDto.pprPurchaseType = dataTable.Rows[0].Field<byte>("pprPurchaseType");
			eRPPurchasePlannerRequirementInformationDto.pprRequirementID = dataTable.Rows[0].Field<int>("pprRequirementID");
			eRPPurchasePlannerRequirementInformationDto.pprRowVersion = dataTable.Rows[0].Field<byte[]>("pprRowVersion");
			eRPPurchasePlannerRequirementInformationDto.pprSalesOrderDeliveryID = dataTable.Rows[0].Field<short>("pprSalesOrderDeliveryID");
			eRPPurchasePlannerRequirementInformationDto.pprSalesOrderID = dataTable.Rows[0].Field<string>("pprSalesOrderID");
			eRPPurchasePlannerRequirementInformationDto.pprSalesOrderLineID = dataTable.Rows[0].Field<short>("pprSalesOrderLineID");
			eRPPurchasePlannerRequirementInformationDto.pprSessionID = dataTable.Rows[0].Field<string>("pprSessionID");
			eRPPurchasePlannerRequirementInformationDto.pprSource = dataTable.Rows[0].Field<string>("pprSource");
			eRPPurchasePlannerRequirementInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPurchasePlannerRequirementInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPurchasePlannerRequirementInformationDto);
	}

	public Task<APIValidationInfoDto> SavePurchasePlannerRequirement(ERPPurchasePlannerRequirementDto purchasePlannerRequirement)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PurchasePlannerRequirements WHERE pprUniqueID = " + M1Util.ConvertToLinq(purchasePlannerRequirement.pprUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["pprSessionID"] = purchasePlannerRequirement.pprSessionID.ToUpper();
				dataRow["pprLineID"] = purchasePlannerRequirement.pprLineID;
				dataRow["pprRequirementID"] = purchasePlannerRequirement.pprRequirementID;
				purchasePlannerRequirement.pprUniqueID = ((purchasePlannerRequirement.pprUniqueID == Guid.Empty) ? Guid.NewGuid() : purchasePlannerRequirement.pprUniqueID);
				dataRow["pprUniqueID"] = purchasePlannerRequirement.pprUniqueID;
				dataRow["pprCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["pprCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PurchasePlannerRequirement could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (purchasePlannerRequirement.pprRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PurchasePlannerRequirement is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["pprRowVersion"], purchasePlannerRequirement.pprRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PurchasePlannerRequirement has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PurchasePlannerRequirement again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? pprDueDate = purchasePlannerRequirement.pprDueDate;
			dataRow2["pprDueDate"] = (pprDueDate.HasValue ? ((object)pprDueDate.GetValueOrDefault()) : dataRow["pprDueDate"]);
			dataRow["pprJobAssemblyID"] = purchasePlannerRequirement.pprJobAssemblyID;
			dataRow["pprJobID"] = purchasePlannerRequirement.pprJobID;
			dataRow["pprJobMaterialID"] = purchasePlannerRequirement.pprJobMaterialID;
			dataRow["pprPlannedReceiptQuantity"] = purchasePlannerRequirement.pprPlannedReceiptQuantity;
			dataRow["pprPlannedRequirementQuantity"] = purchasePlannerRequirement.pprPlannedRequirementQuantity;
			dataRow["pprProjectedBalance"] = purchasePlannerRequirement.pprProjectedBalance;
			dataRow["pprPullFromStockQuantity"] = purchasePlannerRequirement.pprPullFromStockQuantity;
			DataRow dataRow3 = dataRow;
			pprDueDate = purchasePlannerRequirement.pprPurchaseOrderDate;
			dataRow3["pprPurchaseOrderDate"] = (pprDueDate.HasValue ? ((object)pprDueDate.GetValueOrDefault()) : dataRow["pprPurchaseOrderDate"]);
			dataRow["pprPurchaseOrderID"] = purchasePlannerRequirement.pprPurchaseOrderID;
			dataRow["pprPurchaseToJobQuantity"] = purchasePlannerRequirement.pprPurchaseToJobQuantity;
			dataRow["pprPurchaseType"] = purchasePlannerRequirement.pprPurchaseType;
			dataRow["pprSalesOrderDeliveryID"] = purchasePlannerRequirement.pprSalesOrderDeliveryID;
			dataRow["pprSalesOrderID"] = purchasePlannerRequirement.pprSalesOrderID;
			dataRow["pprSalesOrderLineID"] = purchasePlannerRequirement.pprSalesOrderLineID;
			dataRow["pprSource"] = purchasePlannerRequirement.pprSource;
			if (purchasePlannerRequirement.CustomFields != null && purchasePlannerRequirement.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in purchasePlannerRequirement.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PurchasePlannerRequirement [{purchasePlannerRequirement.pprUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PurchasePlannerRequirement [{purchasePlannerRequirement.pprUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
