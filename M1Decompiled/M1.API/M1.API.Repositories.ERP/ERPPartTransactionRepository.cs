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

public class ERPPartTransactionRepository : APIBaseRepository, IERPPartTransactionRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartTransactionRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartTransactionExist(Guid partTransactionId)
	{
		InitializeParameterLists();
		base.filterList.Add("imtUniqueID|C", partTransactionId);
		base.selectList.Add("imtUniqueID");
		return Task.FromResult(GetAsObject("PartTransactions", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartTransactionInformationDto>> GetAllPartTransactions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartTransactionInformationDto> collection = new List<ERPPartTransactionInformationDto>();
		InitializeParameterLists();
		string[] array = new string[43]
		{
			"imtCogsCalculatedDate", "imtCreatedBy", "imtCreatedDate", "imtUniqueID", "imtHeatLot", "imtInspectionStatus", "imtInventoryQuantityReceived", "imtInventoryUnitOfMeasure", "imtCogsPostedToGl", "imtJobCompleteStatus",
			"imtNonInventoryTransaction", "imtNonNettable", "imtPoLineReceivedComplete", "imtRequiresInspection", "imtIssueType", "imtJobAssemblyID", "imtJobID", "imtJobMaterialComponentID", "imtJobMaterialID", "imtJobOperationID",
			"imtJobType", "imtPartBinID", "imtPartID", "imtPartRevisionID", "imtPartWarehouseLocationID", "imtPlantID", "imtPreviousQuantityOnHand", "imtProjectAreaID", "imtProjectID", "imtQuantityToInspect",
			"imtQuantityToReturn", "imtReceiptType", "imtReference", "imtRowVersion", "imtScrapQuantity", "imtPartTransactionID", "imtSetupCharge", "imtSource", "imtTableName", "imtTableUniqueID",
			"imtTransactionDate", "imtTransactionType", "imtUserID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartTransactions");
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
		using (DataTable dataTable = GetAsDataTable("PartTransactions", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartTransactionInformationDto eRPPartTransactionInformationDto = new ERPPartTransactionInformationDto();
				eRPPartTransactionInformationDto.imtCogsCalculatedDate = dataTable.Rows[i].Field<DateTime?>("imtCogsCalculatedDate");
				eRPPartTransactionInformationDto.imtCreatedBy = dataTable.Rows[i].Field<string>("imtCreatedBy");
				eRPPartTransactionInformationDto.imtCreatedDate = dataTable.Rows[i].Field<DateTime?>("imtCreatedDate");
				eRPPartTransactionInformationDto.imtUniqueID = dataTable.Rows[i].Field<Guid>("imtUniqueID");
				eRPPartTransactionInformationDto.imtHeatLot = dataTable.Rows[i].Field<string>("imtHeatLot");
				eRPPartTransactionInformationDto.imtInspectionStatus = dataTable.Rows[i].Field<string>("imtInspectionStatus");
				eRPPartTransactionInformationDto.imtInventoryQuantityReceived = dataTable.Rows[i].Field<decimal>("imtInventoryQuantityReceived");
				eRPPartTransactionInformationDto.imtInventoryUnitOfMeasure = dataTable.Rows[i].Field<string>("imtInventoryUnitOfMeasure");
				eRPPartTransactionInformationDto.imtCogsPostedToGl = dataTable.Rows[i].Field<bool>("imtCogsPostedToGl");
				eRPPartTransactionInformationDto.imtJobCompleteStatus = dataTable.Rows[i].Field<bool>("imtJobCompleteStatus");
				eRPPartTransactionInformationDto.imtNonInventoryTransaction = dataTable.Rows[i].Field<bool>("imtNonInventoryTransaction");
				eRPPartTransactionInformationDto.imtNonNettable = dataTable.Rows[i].Field<bool>("imtNonNettable");
				eRPPartTransactionInformationDto.imtPoLineReceivedComplete = dataTable.Rows[i].Field<bool>("imtPoLineReceivedComplete");
				eRPPartTransactionInformationDto.imtRequiresInspection = dataTable.Rows[i].Field<bool>("imtRequiresInspection");
				eRPPartTransactionInformationDto.imtIssueType = dataTable.Rows[i].Field<byte>("imtIssueType");
				eRPPartTransactionInformationDto.imtJobAssemblyID = dataTable.Rows[i].Field<int>("imtJobAssemblyID");
				eRPPartTransactionInformationDto.imtJobID = dataTable.Rows[i].Field<string>("imtJobID");
				eRPPartTransactionInformationDto.imtJobMaterialComponentID = dataTable.Rows[i].Field<int>("imtJobMaterialComponentID");
				eRPPartTransactionInformationDto.imtJobMaterialID = dataTable.Rows[i].Field<int>("imtJobMaterialID");
				eRPPartTransactionInformationDto.imtJobOperationID = dataTable.Rows[i].Field<int>("imtJobOperationID");
				eRPPartTransactionInformationDto.imtJobType = dataTable.Rows[i].Field<byte>("imtJobType");
				eRPPartTransactionInformationDto.imtPartBinID = dataTable.Rows[i].Field<string>("imtPartBinID");
				eRPPartTransactionInformationDto.imtPartID = dataTable.Rows[i].Field<string>("imtPartID");
				eRPPartTransactionInformationDto.imtPartRevisionID = dataTable.Rows[i].Field<string>("imtPartRevisionID");
				eRPPartTransactionInformationDto.imtPartWarehouseLocationID = dataTable.Rows[i].Field<string>("imtPartWarehouseLocationID");
				eRPPartTransactionInformationDto.imtPlantID = dataTable.Rows[i].Field<string>("imtPlantID");
				eRPPartTransactionInformationDto.imtPreviousQuantityOnHand = dataTable.Rows[i].Field<decimal>("imtPreviousQuantityOnHand");
				eRPPartTransactionInformationDto.imtProjectAreaID = dataTable.Rows[i].Field<string>("imtProjectAreaID");
				eRPPartTransactionInformationDto.imtProjectID = dataTable.Rows[i].Field<string>("imtProjectID");
				eRPPartTransactionInformationDto.imtQuantityToInspect = dataTable.Rows[i].Field<decimal>("imtQuantityToInspect");
				eRPPartTransactionInformationDto.imtQuantityToReturn = dataTable.Rows[i].Field<decimal>("imtQuantityToReturn");
				eRPPartTransactionInformationDto.imtReceiptType = dataTable.Rows[i].Field<byte>("imtReceiptType");
				eRPPartTransactionInformationDto.imtReference = dataTable.Rows[i].Field<string>("imtReference");
				eRPPartTransactionInformationDto.imtRowVersion = dataTable.Rows[i].Field<byte[]>("imtRowVersion");
				eRPPartTransactionInformationDto.imtScrapQuantity = dataTable.Rows[i].Field<decimal>("imtScrapQuantity");
				eRPPartTransactionInformationDto.imtPartTransactionID = dataTable.Rows[i].Field<int>("imtPartTransactionID");
				eRPPartTransactionInformationDto.imtSetupCharge = dataTable.Rows[i].Field<decimal>("imtSetupCharge");
				eRPPartTransactionInformationDto.imtSource = dataTable.Rows[i].Field<byte>("imtSource");
				eRPPartTransactionInformationDto.imtTableName = dataTable.Rows[i].Field<string>("imtTableName");
				eRPPartTransactionInformationDto.imtTableUniqueID = dataTable.Rows[i].Field<Guid>("imtTableUniqueID");
				eRPPartTransactionInformationDto.imtTransactionDate = dataTable.Rows[i].Field<DateTime?>("imtTransactionDate");
				eRPPartTransactionInformationDto.imtTransactionType = dataTable.Rows[i].Field<byte>("imtTransactionType");
				eRPPartTransactionInformationDto.imtUserID = dataTable.Rows[i].Field<string>("imtUserID");
				eRPPartTransactionInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartTransactionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartTransactionInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartTransactionInformationDto> GetPartTransaction(Guid partTransactionId)
	{
		ERPPartTransactionInformationDto eRPPartTransactionInformationDto = new ERPPartTransactionInformationDto();
		InitializeParameterLists();
		string[] collection = new string[43]
		{
			"imtCogsCalculatedDate", "imtCreatedBy", "imtCreatedDate", "imtUniqueID", "imtHeatLot", "imtInspectionStatus", "imtInventoryQuantityReceived", "imtInventoryUnitOfMeasure", "imtCogsPostedToGl", "imtJobCompleteStatus",
			"imtNonInventoryTransaction", "imtNonNettable", "imtPoLineReceivedComplete", "imtRequiresInspection", "imtIssueType", "imtJobAssemblyID", "imtJobID", "imtJobMaterialComponentID", "imtJobMaterialID", "imtJobOperationID",
			"imtJobType", "imtPartBinID", "imtPartID", "imtPartRevisionID", "imtPartWarehouseLocationID", "imtPlantID", "imtPreviousQuantityOnHand", "imtProjectAreaID", "imtProjectID", "imtQuantityToInspect",
			"imtQuantityToReturn", "imtReceiptType", "imtReference", "imtRowVersion", "imtScrapQuantity", "imtPartTransactionID", "imtSetupCharge", "imtSource", "imtTableName", "imtTableUniqueID",
			"imtTransactionDate", "imtTransactionType", "imtUserID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imtUniqueID|C", partTransactionId);
		AddCustomFieldsToSelectList("PartTransactions");
		using (DataTable dataTable = GetAsDataTable("PartTransactions", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartTransactionInformationDto);
			}
			eRPPartTransactionInformationDto.imtCogsCalculatedDate = dataTable.Rows[0].Field<DateTime?>("imtCogsCalculatedDate");
			eRPPartTransactionInformationDto.imtCreatedBy = dataTable.Rows[0].Field<string>("imtCreatedBy");
			eRPPartTransactionInformationDto.imtCreatedDate = dataTable.Rows[0].Field<DateTime?>("imtCreatedDate");
			eRPPartTransactionInformationDto.imtUniqueID = dataTable.Rows[0].Field<Guid>("imtUniqueID");
			eRPPartTransactionInformationDto.imtHeatLot = dataTable.Rows[0].Field<string>("imtHeatLot");
			eRPPartTransactionInformationDto.imtInspectionStatus = dataTable.Rows[0].Field<string>("imtInspectionStatus");
			eRPPartTransactionInformationDto.imtInventoryQuantityReceived = dataTable.Rows[0].Field<decimal>("imtInventoryQuantityReceived");
			eRPPartTransactionInformationDto.imtInventoryUnitOfMeasure = dataTable.Rows[0].Field<string>("imtInventoryUnitOfMeasure");
			eRPPartTransactionInformationDto.imtCogsPostedToGl = dataTable.Rows[0].Field<bool>("imtCogsPostedToGl");
			eRPPartTransactionInformationDto.imtJobCompleteStatus = dataTable.Rows[0].Field<bool>("imtJobCompleteStatus");
			eRPPartTransactionInformationDto.imtNonInventoryTransaction = dataTable.Rows[0].Field<bool>("imtNonInventoryTransaction");
			eRPPartTransactionInformationDto.imtNonNettable = dataTable.Rows[0].Field<bool>("imtNonNettable");
			eRPPartTransactionInformationDto.imtPoLineReceivedComplete = dataTable.Rows[0].Field<bool>("imtPoLineReceivedComplete");
			eRPPartTransactionInformationDto.imtRequiresInspection = dataTable.Rows[0].Field<bool>("imtRequiresInspection");
			eRPPartTransactionInformationDto.imtIssueType = dataTable.Rows[0].Field<byte>("imtIssueType");
			eRPPartTransactionInformationDto.imtJobAssemblyID = dataTable.Rows[0].Field<int>("imtJobAssemblyID");
			eRPPartTransactionInformationDto.imtJobID = dataTable.Rows[0].Field<string>("imtJobID");
			eRPPartTransactionInformationDto.imtJobMaterialComponentID = dataTable.Rows[0].Field<int>("imtJobMaterialComponentID");
			eRPPartTransactionInformationDto.imtJobMaterialID = dataTable.Rows[0].Field<int>("imtJobMaterialID");
			eRPPartTransactionInformationDto.imtJobOperationID = dataTable.Rows[0].Field<int>("imtJobOperationID");
			eRPPartTransactionInformationDto.imtJobType = dataTable.Rows[0].Field<byte>("imtJobType");
			eRPPartTransactionInformationDto.imtPartBinID = dataTable.Rows[0].Field<string>("imtPartBinID");
			eRPPartTransactionInformationDto.imtPartID = dataTable.Rows[0].Field<string>("imtPartID");
			eRPPartTransactionInformationDto.imtPartRevisionID = dataTable.Rows[0].Field<string>("imtPartRevisionID");
			eRPPartTransactionInformationDto.imtPartWarehouseLocationID = dataTable.Rows[0].Field<string>("imtPartWarehouseLocationID");
			eRPPartTransactionInformationDto.imtPlantID = dataTable.Rows[0].Field<string>("imtPlantID");
			eRPPartTransactionInformationDto.imtPreviousQuantityOnHand = dataTable.Rows[0].Field<decimal>("imtPreviousQuantityOnHand");
			eRPPartTransactionInformationDto.imtProjectAreaID = dataTable.Rows[0].Field<string>("imtProjectAreaID");
			eRPPartTransactionInformationDto.imtProjectID = dataTable.Rows[0].Field<string>("imtProjectID");
			eRPPartTransactionInformationDto.imtQuantityToInspect = dataTable.Rows[0].Field<decimal>("imtQuantityToInspect");
			eRPPartTransactionInformationDto.imtQuantityToReturn = dataTable.Rows[0].Field<decimal>("imtQuantityToReturn");
			eRPPartTransactionInformationDto.imtReceiptType = dataTable.Rows[0].Field<byte>("imtReceiptType");
			eRPPartTransactionInformationDto.imtReference = dataTable.Rows[0].Field<string>("imtReference");
			eRPPartTransactionInformationDto.imtRowVersion = dataTable.Rows[0].Field<byte[]>("imtRowVersion");
			eRPPartTransactionInformationDto.imtScrapQuantity = dataTable.Rows[0].Field<decimal>("imtScrapQuantity");
			eRPPartTransactionInformationDto.imtPartTransactionID = dataTable.Rows[0].Field<int>("imtPartTransactionID");
			eRPPartTransactionInformationDto.imtSetupCharge = dataTable.Rows[0].Field<decimal>("imtSetupCharge");
			eRPPartTransactionInformationDto.imtSource = dataTable.Rows[0].Field<byte>("imtSource");
			eRPPartTransactionInformationDto.imtTableName = dataTable.Rows[0].Field<string>("imtTableName");
			eRPPartTransactionInformationDto.imtTableUniqueID = dataTable.Rows[0].Field<Guid>("imtTableUniqueID");
			eRPPartTransactionInformationDto.imtTransactionDate = dataTable.Rows[0].Field<DateTime?>("imtTransactionDate");
			eRPPartTransactionInformationDto.imtTransactionType = dataTable.Rows[0].Field<byte>("imtTransactionType");
			eRPPartTransactionInformationDto.imtUserID = dataTable.Rows[0].Field<string>("imtUserID");
			eRPPartTransactionInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartTransactionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartTransactionInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartTransaction(ERPPartTransactionDto partTransaction)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartTransactions WHERE imtUniqueID = " + M1Util.ConvertToLinq(partTransaction.imtUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imtPartTransactionID"] = partTransaction.imtPartTransactionID;
				partTransaction.imtUniqueID = ((partTransaction.imtUniqueID == Guid.Empty) ? Guid.NewGuid() : partTransaction.imtUniqueID);
				dataRow["imtUniqueID"] = partTransaction.imtUniqueID;
				dataRow["imtCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imtCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartTransaction could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partTransaction.imtRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartTransaction is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imtRowVersion"], partTransaction.imtRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartTransaction has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartTransaction again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? imtCogsCalculatedDate = partTransaction.imtCogsCalculatedDate;
			dataRow2["imtCogsCalculatedDate"] = (imtCogsCalculatedDate.HasValue ? ((object)imtCogsCalculatedDate.GetValueOrDefault()) : dataRow["imtCogsCalculatedDate"]);
			dataRow["imtHeatLot"] = partTransaction.imtHeatLot;
			dataRow["imtInspectionStatus"] = partTransaction.imtInspectionStatus;
			dataRow["imtInventoryQuantityReceived"] = partTransaction.imtInventoryQuantityReceived;
			dataRow["imtInventoryUnitOfMeasure"] = partTransaction.imtInventoryUnitOfMeasure;
			dataRow["imtCogsPostedToGl"] = partTransaction.imtCogsPostedToGl;
			dataRow["imtJobCompleteStatus"] = partTransaction.imtJobCompleteStatus;
			dataRow["imtNonInventoryTransaction"] = partTransaction.imtNonInventoryTransaction;
			dataRow["imtNonNettable"] = partTransaction.imtNonNettable;
			dataRow["imtPoLineReceivedComplete"] = partTransaction.imtPoLineReceivedComplete;
			dataRow["imtRequiresInspection"] = partTransaction.imtRequiresInspection;
			dataRow["imtIssueType"] = partTransaction.imtIssueType;
			dataRow["imtJobAssemblyID"] = partTransaction.imtJobAssemblyID;
			dataRow["imtJobID"] = partTransaction.imtJobID;
			dataRow["imtJobMaterialComponentID"] = partTransaction.imtJobMaterialComponentID;
			dataRow["imtJobMaterialID"] = partTransaction.imtJobMaterialID;
			dataRow["imtJobOperationID"] = partTransaction.imtJobOperationID;
			dataRow["imtJobType"] = partTransaction.imtJobType;
			dataRow["imtPartBinID"] = partTransaction.imtPartBinID;
			dataRow["imtPartID"] = partTransaction.imtPartID;
			dataRow["imtPartRevisionID"] = partTransaction.imtPartRevisionID;
			dataRow["imtPartWarehouseLocationID"] = partTransaction.imtPartWarehouseLocationID;
			dataRow["imtPlantID"] = partTransaction.imtPlantID;
			dataRow["imtPreviousQuantityOnHand"] = partTransaction.imtPreviousQuantityOnHand;
			dataRow["imtProjectAreaID"] = partTransaction.imtProjectAreaID;
			dataRow["imtProjectID"] = partTransaction.imtProjectID;
			dataRow["imtQuantityToInspect"] = partTransaction.imtQuantityToInspect;
			dataRow["imtQuantityToReturn"] = partTransaction.imtQuantityToReturn;
			dataRow["imtReceiptType"] = partTransaction.imtReceiptType;
			dataRow["imtReference"] = partTransaction.imtReference;
			dataRow["imtScrapQuantity"] = partTransaction.imtScrapQuantity;
			dataRow["imtSetupCharge"] = partTransaction.imtSetupCharge;
			dataRow["imtSource"] = partTransaction.imtSource;
			dataRow["imtTableName"] = partTransaction.imtTableName;
			dataRow["imtTableUniqueID"] = partTransaction.imtTableUniqueID;
			DataRow dataRow3 = dataRow;
			imtCogsCalculatedDate = partTransaction.imtTransactionDate;
			dataRow3["imtTransactionDate"] = (imtCogsCalculatedDate.HasValue ? ((object)imtCogsCalculatedDate.GetValueOrDefault()) : dataRow["imtTransactionDate"]);
			dataRow["imtTransactionType"] = partTransaction.imtTransactionType;
			dataRow["imtUserID"] = partTransaction.imtUserID;
			if (partTransaction.CustomFields != null && partTransaction.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partTransaction.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartTransaction [{partTransaction.imtUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartTransaction [{partTransaction.imtUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
