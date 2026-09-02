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

public class ERPPartMemoRepository : APIBaseRepository, IERPPartMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartMemoExist(Guid partMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("imkUniqueID|C", partMemoId);
		base.selectList.Add("imkUniqueID");
		return Task.FromResult(GetAsObject("PartMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartMemoInformationDto>> GetAllPartMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartMemoInformationDto> collection = new List<ERPPartMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[45]
		{
			"imkCreatedBy", "imkCreatedDate", "imkUniqueID", "imkLongDescriptionRtf", "imkLongDescriptionText", "imkMemoDate", "imkPartID", "imkPartRevisionID", "imkRowVersion", "imkPartMemoID",
			"imkShortDescription", "imkShowInApInvoices", "imkShowInArInvoices", "imkShowInCalls", "imkShowInChangeRequests", "imkShowInDmrClaims", "imkShowInDmrShipments", "imkShowInInspections", "imkShowInJobAssemblies", "imkShowInJobMaterials",
			"imkShowInJobOperations", "imkShowInJobs", "imkShowInKnowledgebasePages", "imkShowInLeads", "imkShowInNonconformances", "imkShowInPartAssemblies", "imkShowInPartMaterials", "imkShowInPartOperations", "imkShowInPartRevisions", "imkShowInPriceAndAvailability",
			"imkShowInPurchaseOrders", "imkShowInQuoteAssemblies", "imkShowInQuoteLines", "imkShowInQuoteMaterials", "imkShowInQuoteOperations", "imkShowInReceipts", "imkShowInRfqs", "imkShowInRmaClaims", "imkShowInRmaReceipts", "imkShowInSalesOrders",
			"imkShowInServiceContracts", "imkShowInShipments", "imkShowInWarehouseReceipts", "imkShowInWarehouseRequisitions", "imkShowInWarehouseTransfers"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartMemos");
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
		using (DataTable dataTable = GetAsDataTable("PartMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartMemoInformationDto eRPPartMemoInformationDto = new ERPPartMemoInformationDto();
				eRPPartMemoInformationDto.imkCreatedBy = dataTable.Rows[i].Field<string>("imkCreatedBy");
				eRPPartMemoInformationDto.imkCreatedDate = dataTable.Rows[i].Field<DateTime?>("imkCreatedDate");
				eRPPartMemoInformationDto.imkUniqueID = dataTable.Rows[i].Field<Guid>("imkUniqueID");
				eRPPartMemoInformationDto.imkLongDescriptionRtf = dataTable.Rows[i].Field<string>("imkLongDescriptionRtf");
				eRPPartMemoInformationDto.imkLongDescriptionText = dataTable.Rows[i].Field<string>("imkLongDescriptionText");
				eRPPartMemoInformationDto.imkMemoDate = dataTable.Rows[i].Field<DateTime?>("imkMemoDate");
				eRPPartMemoInformationDto.imkPartID = dataTable.Rows[i].Field<string>("imkPartID");
				eRPPartMemoInformationDto.imkPartRevisionID = dataTable.Rows[i].Field<string>("imkPartRevisionID");
				eRPPartMemoInformationDto.imkRowVersion = dataTable.Rows[i].Field<byte[]>("imkRowVersion");
				eRPPartMemoInformationDto.imkPartMemoID = dataTable.Rows[i].Field<short>("imkPartMemoID");
				eRPPartMemoInformationDto.imkShortDescription = dataTable.Rows[i].Field<string>("imkShortDescription");
				eRPPartMemoInformationDto.imkShowInApInvoices = dataTable.Rows[i].Field<bool>("imkShowInApInvoices");
				eRPPartMemoInformationDto.imkShowInArInvoices = dataTable.Rows[i].Field<bool>("imkShowInArInvoices");
				eRPPartMemoInformationDto.imkShowInCalls = dataTable.Rows[i].Field<bool>("imkShowInCalls");
				eRPPartMemoInformationDto.imkShowInChangeRequests = dataTable.Rows[i].Field<bool>("imkShowInChangeRequests");
				eRPPartMemoInformationDto.imkShowInDmrClaims = dataTable.Rows[i].Field<bool>("imkShowInDmrClaims");
				eRPPartMemoInformationDto.imkShowInDmrShipments = dataTable.Rows[i].Field<bool>("imkShowInDmrShipments");
				eRPPartMemoInformationDto.imkShowInInspections = dataTable.Rows[i].Field<bool>("imkShowInInspections");
				eRPPartMemoInformationDto.imkShowInJobAssemblies = dataTable.Rows[i].Field<bool>("imkShowInJobAssemblies");
				eRPPartMemoInformationDto.imkShowInJobMaterials = dataTable.Rows[i].Field<bool>("imkShowInJobMaterials");
				eRPPartMemoInformationDto.imkShowInJobOperations = dataTable.Rows[i].Field<bool>("imkShowInJobOperations");
				eRPPartMemoInformationDto.imkShowInJobs = dataTable.Rows[i].Field<bool>("imkShowInJobs");
				eRPPartMemoInformationDto.imkShowInKnowledgebasePages = dataTable.Rows[i].Field<bool>("imkShowInKnowledgebasePages");
				eRPPartMemoInformationDto.imkShowInLeads = dataTable.Rows[i].Field<bool>("imkShowInLeads");
				eRPPartMemoInformationDto.imkShowInNonconformances = dataTable.Rows[i].Field<bool>("imkShowInNonconformances");
				eRPPartMemoInformationDto.imkShowInPartAssemblies = dataTable.Rows[i].Field<bool>("imkShowInPartAssemblies");
				eRPPartMemoInformationDto.imkShowInPartMaterials = dataTable.Rows[i].Field<bool>("imkShowInPartMaterials");
				eRPPartMemoInformationDto.imkShowInPartOperations = dataTable.Rows[i].Field<bool>("imkShowInPartOperations");
				eRPPartMemoInformationDto.imkShowInPartRevisions = dataTable.Rows[i].Field<bool>("imkShowInPartRevisions");
				eRPPartMemoInformationDto.imkShowInPriceAndAvailability = dataTable.Rows[i].Field<bool>("imkShowInPriceAndAvailability");
				eRPPartMemoInformationDto.imkShowInPurchaseOrders = dataTable.Rows[i].Field<bool>("imkShowInPurchaseOrders");
				eRPPartMemoInformationDto.imkShowInQuoteAssemblies = dataTable.Rows[i].Field<bool>("imkShowInQuoteAssemblies");
				eRPPartMemoInformationDto.imkShowInQuoteLines = dataTable.Rows[i].Field<bool>("imkShowInQuoteLines");
				eRPPartMemoInformationDto.imkShowInQuoteMaterials = dataTable.Rows[i].Field<bool>("imkShowInQuoteMaterials");
				eRPPartMemoInformationDto.imkShowInQuoteOperations = dataTable.Rows[i].Field<bool>("imkShowInQuoteOperations");
				eRPPartMemoInformationDto.imkShowInReceipts = dataTable.Rows[i].Field<bool>("imkShowInReceipts");
				eRPPartMemoInformationDto.imkShowInRfqs = dataTable.Rows[i].Field<bool>("imkShowInRfqs");
				eRPPartMemoInformationDto.imkShowInRmaClaims = dataTable.Rows[i].Field<bool>("imkShowInRmaClaims");
				eRPPartMemoInformationDto.imkShowInRmaReceipts = dataTable.Rows[i].Field<bool>("imkShowInRmaReceipts");
				eRPPartMemoInformationDto.imkShowInSalesOrders = dataTable.Rows[i].Field<bool>("imkShowInSalesOrders");
				eRPPartMemoInformationDto.imkShowInServiceContracts = dataTable.Rows[i].Field<bool>("imkShowInServiceContracts");
				eRPPartMemoInformationDto.imkShowInShipments = dataTable.Rows[i].Field<bool>("imkShowInShipments");
				eRPPartMemoInformationDto.imkShowInWarehouseReceipts = dataTable.Rows[i].Field<bool>("imkShowInWarehouseReceipts");
				eRPPartMemoInformationDto.imkShowInWarehouseRequisitions = dataTable.Rows[i].Field<bool>("imkShowInWarehouseRequisitions");
				eRPPartMemoInformationDto.imkShowInWarehouseTransfers = dataTable.Rows[i].Field<bool>("imkShowInWarehouseTransfers");
				eRPPartMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartMemoInformationDto> GetPartMemo(Guid partMemoId)
	{
		ERPPartMemoInformationDto eRPPartMemoInformationDto = new ERPPartMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[45]
		{
			"imkCreatedBy", "imkCreatedDate", "imkUniqueID", "imkLongDescriptionRtf", "imkLongDescriptionText", "imkMemoDate", "imkPartID", "imkPartRevisionID", "imkRowVersion", "imkPartMemoID",
			"imkShortDescription", "imkShowInApInvoices", "imkShowInArInvoices", "imkShowInCalls", "imkShowInChangeRequests", "imkShowInDmrClaims", "imkShowInDmrShipments", "imkShowInInspections", "imkShowInJobAssemblies", "imkShowInJobMaterials",
			"imkShowInJobOperations", "imkShowInJobs", "imkShowInKnowledgebasePages", "imkShowInLeads", "imkShowInNonconformances", "imkShowInPartAssemblies", "imkShowInPartMaterials", "imkShowInPartOperations", "imkShowInPartRevisions", "imkShowInPriceAndAvailability",
			"imkShowInPurchaseOrders", "imkShowInQuoteAssemblies", "imkShowInQuoteLines", "imkShowInQuoteMaterials", "imkShowInQuoteOperations", "imkShowInReceipts", "imkShowInRfqs", "imkShowInRmaClaims", "imkShowInRmaReceipts", "imkShowInSalesOrders",
			"imkShowInServiceContracts", "imkShowInShipments", "imkShowInWarehouseReceipts", "imkShowInWarehouseRequisitions", "imkShowInWarehouseTransfers"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imkUniqueID|C", partMemoId);
		AddCustomFieldsToSelectList("PartMemos");
		using (DataTable dataTable = GetAsDataTable("PartMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartMemoInformationDto);
			}
			eRPPartMemoInformationDto.imkCreatedBy = dataTable.Rows[0].Field<string>("imkCreatedBy");
			eRPPartMemoInformationDto.imkCreatedDate = dataTable.Rows[0].Field<DateTime?>("imkCreatedDate");
			eRPPartMemoInformationDto.imkUniqueID = dataTable.Rows[0].Field<Guid>("imkUniqueID");
			eRPPartMemoInformationDto.imkLongDescriptionRtf = dataTable.Rows[0].Field<string>("imkLongDescriptionRtf");
			eRPPartMemoInformationDto.imkLongDescriptionText = dataTable.Rows[0].Field<string>("imkLongDescriptionText");
			eRPPartMemoInformationDto.imkMemoDate = dataTable.Rows[0].Field<DateTime?>("imkMemoDate");
			eRPPartMemoInformationDto.imkPartID = dataTable.Rows[0].Field<string>("imkPartID");
			eRPPartMemoInformationDto.imkPartRevisionID = dataTable.Rows[0].Field<string>("imkPartRevisionID");
			eRPPartMemoInformationDto.imkRowVersion = dataTable.Rows[0].Field<byte[]>("imkRowVersion");
			eRPPartMemoInformationDto.imkPartMemoID = dataTable.Rows[0].Field<short>("imkPartMemoID");
			eRPPartMemoInformationDto.imkShortDescription = dataTable.Rows[0].Field<string>("imkShortDescription");
			eRPPartMemoInformationDto.imkShowInApInvoices = dataTable.Rows[0].Field<bool>("imkShowInApInvoices");
			eRPPartMemoInformationDto.imkShowInArInvoices = dataTable.Rows[0].Field<bool>("imkShowInArInvoices");
			eRPPartMemoInformationDto.imkShowInCalls = dataTable.Rows[0].Field<bool>("imkShowInCalls");
			eRPPartMemoInformationDto.imkShowInChangeRequests = dataTable.Rows[0].Field<bool>("imkShowInChangeRequests");
			eRPPartMemoInformationDto.imkShowInDmrClaims = dataTable.Rows[0].Field<bool>("imkShowInDmrClaims");
			eRPPartMemoInformationDto.imkShowInDmrShipments = dataTable.Rows[0].Field<bool>("imkShowInDmrShipments");
			eRPPartMemoInformationDto.imkShowInInspections = dataTable.Rows[0].Field<bool>("imkShowInInspections");
			eRPPartMemoInformationDto.imkShowInJobAssemblies = dataTable.Rows[0].Field<bool>("imkShowInJobAssemblies");
			eRPPartMemoInformationDto.imkShowInJobMaterials = dataTable.Rows[0].Field<bool>("imkShowInJobMaterials");
			eRPPartMemoInformationDto.imkShowInJobOperations = dataTable.Rows[0].Field<bool>("imkShowInJobOperations");
			eRPPartMemoInformationDto.imkShowInJobs = dataTable.Rows[0].Field<bool>("imkShowInJobs");
			eRPPartMemoInformationDto.imkShowInKnowledgebasePages = dataTable.Rows[0].Field<bool>("imkShowInKnowledgebasePages");
			eRPPartMemoInformationDto.imkShowInLeads = dataTable.Rows[0].Field<bool>("imkShowInLeads");
			eRPPartMemoInformationDto.imkShowInNonconformances = dataTable.Rows[0].Field<bool>("imkShowInNonconformances");
			eRPPartMemoInformationDto.imkShowInPartAssemblies = dataTable.Rows[0].Field<bool>("imkShowInPartAssemblies");
			eRPPartMemoInformationDto.imkShowInPartMaterials = dataTable.Rows[0].Field<bool>("imkShowInPartMaterials");
			eRPPartMemoInformationDto.imkShowInPartOperations = dataTable.Rows[0].Field<bool>("imkShowInPartOperations");
			eRPPartMemoInformationDto.imkShowInPartRevisions = dataTable.Rows[0].Field<bool>("imkShowInPartRevisions");
			eRPPartMemoInformationDto.imkShowInPriceAndAvailability = dataTable.Rows[0].Field<bool>("imkShowInPriceAndAvailability");
			eRPPartMemoInformationDto.imkShowInPurchaseOrders = dataTable.Rows[0].Field<bool>("imkShowInPurchaseOrders");
			eRPPartMemoInformationDto.imkShowInQuoteAssemblies = dataTable.Rows[0].Field<bool>("imkShowInQuoteAssemblies");
			eRPPartMemoInformationDto.imkShowInQuoteLines = dataTable.Rows[0].Field<bool>("imkShowInQuoteLines");
			eRPPartMemoInformationDto.imkShowInQuoteMaterials = dataTable.Rows[0].Field<bool>("imkShowInQuoteMaterials");
			eRPPartMemoInformationDto.imkShowInQuoteOperations = dataTable.Rows[0].Field<bool>("imkShowInQuoteOperations");
			eRPPartMemoInformationDto.imkShowInReceipts = dataTable.Rows[0].Field<bool>("imkShowInReceipts");
			eRPPartMemoInformationDto.imkShowInRfqs = dataTable.Rows[0].Field<bool>("imkShowInRfqs");
			eRPPartMemoInformationDto.imkShowInRmaClaims = dataTable.Rows[0].Field<bool>("imkShowInRmaClaims");
			eRPPartMemoInformationDto.imkShowInRmaReceipts = dataTable.Rows[0].Field<bool>("imkShowInRmaReceipts");
			eRPPartMemoInformationDto.imkShowInSalesOrders = dataTable.Rows[0].Field<bool>("imkShowInSalesOrders");
			eRPPartMemoInformationDto.imkShowInServiceContracts = dataTable.Rows[0].Field<bool>("imkShowInServiceContracts");
			eRPPartMemoInformationDto.imkShowInShipments = dataTable.Rows[0].Field<bool>("imkShowInShipments");
			eRPPartMemoInformationDto.imkShowInWarehouseReceipts = dataTable.Rows[0].Field<bool>("imkShowInWarehouseReceipts");
			eRPPartMemoInformationDto.imkShowInWarehouseRequisitions = dataTable.Rows[0].Field<bool>("imkShowInWarehouseRequisitions");
			eRPPartMemoInformationDto.imkShowInWarehouseTransfers = dataTable.Rows[0].Field<bool>("imkShowInWarehouseTransfers");
			eRPPartMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartMemo(ERPPartMemoDto partMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartMemos WHERE imkUniqueID = " + M1Util.ConvertToLinq(partMemo.imkUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imkPartID"] = partMemo.imkPartID.ToUpper();
				dataRow["imkPartRevisionID"] = partMemo.imkPartRevisionID.ToUpper();
				dataRow["imkPartMemoID"] = partMemo.imkPartMemoID;
				partMemo.imkUniqueID = ((partMemo.imkUniqueID == Guid.Empty) ? Guid.NewGuid() : partMemo.imkUniqueID);
				dataRow["imkUniqueID"] = partMemo.imkUniqueID;
				dataRow["imkCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imkCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partMemo.imkRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imkRowVersion"], partMemo.imkRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imkLongDescriptionRtf"] = partMemo.imkLongDescriptionRtf ?? dataRow["imkLongDescriptionRtf"];
			dataRow["imkLongDescriptionText"] = partMemo.imkLongDescriptionText ?? dataRow["imkLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? imkMemoDate = partMemo.imkMemoDate;
			dataRow2["imkMemoDate"] = (imkMemoDate.HasValue ? ((object)imkMemoDate.GetValueOrDefault()) : dataRow["imkMemoDate"]);
			dataRow["imkShortDescription"] = partMemo.imkShortDescription;
			dataRow["imkShowInApInvoices"] = partMemo.imkShowInApInvoices;
			dataRow["imkShowInArInvoices"] = partMemo.imkShowInArInvoices;
			dataRow["imkShowInCalls"] = partMemo.imkShowInCalls;
			dataRow["imkShowInChangeRequests"] = partMemo.imkShowInChangeRequests;
			dataRow["imkShowInDmrClaims"] = partMemo.imkShowInDmrClaims;
			dataRow["imkShowInDmrShipments"] = partMemo.imkShowInDmrShipments;
			dataRow["imkShowInInspections"] = partMemo.imkShowInInspections;
			dataRow["imkShowInJobAssemblies"] = partMemo.imkShowInJobAssemblies;
			dataRow["imkShowInJobMaterials"] = partMemo.imkShowInJobMaterials;
			dataRow["imkShowInJobOperations"] = partMemo.imkShowInJobOperations;
			dataRow["imkShowInJobs"] = partMemo.imkShowInJobs;
			dataRow["imkShowInKnowledgebasePages"] = partMemo.imkShowInKnowledgebasePages;
			dataRow["imkShowInLeads"] = partMemo.imkShowInLeads;
			dataRow["imkShowInNonconformances"] = partMemo.imkShowInNonconformances;
			dataRow["imkShowInPartAssemblies"] = partMemo.imkShowInPartAssemblies;
			dataRow["imkShowInPartMaterials"] = partMemo.imkShowInPartMaterials;
			dataRow["imkShowInPartOperations"] = partMemo.imkShowInPartOperations;
			dataRow["imkShowInPartRevisions"] = partMemo.imkShowInPartRevisions;
			dataRow["imkShowInPriceAndAvailability"] = partMemo.imkShowInPriceAndAvailability;
			dataRow["imkShowInPurchaseOrders"] = partMemo.imkShowInPurchaseOrders;
			dataRow["imkShowInQuoteAssemblies"] = partMemo.imkShowInQuoteAssemblies;
			dataRow["imkShowInQuoteLines"] = partMemo.imkShowInQuoteLines;
			dataRow["imkShowInQuoteMaterials"] = partMemo.imkShowInQuoteMaterials;
			dataRow["imkShowInQuoteOperations"] = partMemo.imkShowInQuoteOperations;
			dataRow["imkShowInReceipts"] = partMemo.imkShowInReceipts;
			dataRow["imkShowInRfqs"] = partMemo.imkShowInRfqs;
			dataRow["imkShowInRmaClaims"] = partMemo.imkShowInRmaClaims;
			dataRow["imkShowInRmaReceipts"] = partMemo.imkShowInRmaReceipts;
			dataRow["imkShowInSalesOrders"] = partMemo.imkShowInSalesOrders;
			dataRow["imkShowInServiceContracts"] = partMemo.imkShowInServiceContracts;
			dataRow["imkShowInShipments"] = partMemo.imkShowInShipments;
			dataRow["imkShowInWarehouseReceipts"] = partMemo.imkShowInWarehouseReceipts;
			dataRow["imkShowInWarehouseRequisitions"] = partMemo.imkShowInWarehouseRequisitions;
			dataRow["imkShowInWarehouseTransfers"] = partMemo.imkShowInWarehouseTransfers;
			if (partMemo.CustomFields != null && partMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartMemo [{partMemo.imkUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartMemo [{partMemo.imkUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
