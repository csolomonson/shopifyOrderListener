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

public class ERPPurchasePlannerSessionRepository : APIBaseRepository, IERPPurchasePlannerSessionRepository, IAPIBaseRepository, IDisposable
{
	public ERPPurchasePlannerSessionRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPurchasePlannerSessionExist(Guid purchasePlannerSessionId)
	{
		InitializeParameterLists();
		base.filterList.Add("ppsUniqueID|C", purchasePlannerSessionId);
		base.selectList.Add("ppsUniqueID");
		return Task.FromResult(GetAsObject("PurchasePlannerSessions", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPurchasePlannerSessionInformationDto>> GetAllPurchasePlannerSessions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPurchasePlannerSessionInformationDto> collection = new List<ERPPurchasePlannerSessionInformationDto>();
		InitializeParameterLists();
		string[] array = new string[22]
		{
			"ppsBuyerEmployeeID", "ppsCompletedDate", "ppsCreatedBy", "ppsCreatedDate", "ppsCutoffDate", "ppsCutoffDatePosupply", "ppsUniqueID", "ppsCalculateForAllParts", "ppsCompleted", "ppsFirmOnly",
			"ppsGenerated", "ppsJobIDs", "ppsPartClassIDs", "ppsPartIDs", "ppsPlantID", "ppsRowVersion", "ppsSalesOrderIDs", "ppsSessionID", "ppsSessionSubtotalBase", "ppsShowAllDemandForPartsOnJobs",
			"ppsSupplierIDs", "ppsWarehouseID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PurchasePlannerSessions");
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
		using (DataTable dataTable = GetAsDataTable("PurchasePlannerSessions", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPurchasePlannerSessionInformationDto eRPPurchasePlannerSessionInformationDto = new ERPPurchasePlannerSessionInformationDto();
				eRPPurchasePlannerSessionInformationDto.ppsBuyerEmployeeID = dataTable.Rows[i].Field<string>("ppsBuyerEmployeeID");
				eRPPurchasePlannerSessionInformationDto.ppsCompletedDate = dataTable.Rows[i].Field<DateTime?>("ppsCompletedDate");
				eRPPurchasePlannerSessionInformationDto.ppsCreatedBy = dataTable.Rows[i].Field<string>("ppsCreatedBy");
				eRPPurchasePlannerSessionInformationDto.ppsCreatedDate = dataTable.Rows[i].Field<DateTime?>("ppsCreatedDate");
				eRPPurchasePlannerSessionInformationDto.ppsCutoffDate = dataTable.Rows[i].Field<DateTime?>("ppsCutoffDate");
				eRPPurchasePlannerSessionInformationDto.ppsCutoffDatePosupply = dataTable.Rows[i].Field<DateTime?>("ppsCutoffDatePosupply");
				eRPPurchasePlannerSessionInformationDto.ppsUniqueID = dataTable.Rows[i].Field<Guid>("ppsUniqueID");
				eRPPurchasePlannerSessionInformationDto.ppsCalculateForAllParts = dataTable.Rows[i].Field<bool>("ppsCalculateForAllParts");
				eRPPurchasePlannerSessionInformationDto.ppsCompleted = dataTable.Rows[i].Field<bool>("ppsCompleted");
				eRPPurchasePlannerSessionInformationDto.ppsFirmOnly = dataTable.Rows[i].Field<bool>("ppsFirmOnly");
				eRPPurchasePlannerSessionInformationDto.ppsGenerated = dataTable.Rows[i].Field<bool>("ppsGenerated");
				eRPPurchasePlannerSessionInformationDto.ppsJobIDs = dataTable.Rows[i].Field<string>("ppsJobIDs");
				eRPPurchasePlannerSessionInformationDto.ppsPartClassIDs = dataTable.Rows[i].Field<string>("ppsPartClassIDs");
				eRPPurchasePlannerSessionInformationDto.ppsPartIDs = dataTable.Rows[i].Field<string>("ppsPartIDs");
				eRPPurchasePlannerSessionInformationDto.ppsPlantID = dataTable.Rows[i].Field<string>("ppsPlantID");
				eRPPurchasePlannerSessionInformationDto.ppsRowVersion = dataTable.Rows[i].Field<byte[]>("ppsRowVersion");
				eRPPurchasePlannerSessionInformationDto.ppsSalesOrderIDs = dataTable.Rows[i].Field<string>("ppsSalesOrderIDs");
				eRPPurchasePlannerSessionInformationDto.ppsSessionID = dataTable.Rows[i].Field<string>("ppsSessionID");
				eRPPurchasePlannerSessionInformationDto.ppsSessionSubtotalBase = dataTable.Rows[i].Field<decimal>("ppsSessionSubtotalBase");
				eRPPurchasePlannerSessionInformationDto.ppsShowAllDemandForPartsOnJobs = dataTable.Rows[i].Field<bool>("ppsShowAllDemandForPartsOnJobs");
				eRPPurchasePlannerSessionInformationDto.ppsSupplierIDs = dataTable.Rows[i].Field<string>("ppsSupplierIDs");
				eRPPurchasePlannerSessionInformationDto.ppsWarehouseID = dataTable.Rows[i].Field<string>("ppsWarehouseID");
				eRPPurchasePlannerSessionInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPurchasePlannerSessionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPurchasePlannerSessionInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPurchasePlannerSessionInformationDto> GetPurchasePlannerSession(Guid purchasePlannerSessionId)
	{
		ERPPurchasePlannerSessionInformationDto eRPPurchasePlannerSessionInformationDto = new ERPPurchasePlannerSessionInformationDto();
		InitializeParameterLists();
		string[] collection = new string[22]
		{
			"ppsBuyerEmployeeID", "ppsCompletedDate", "ppsCreatedBy", "ppsCreatedDate", "ppsCutoffDate", "ppsCutoffDatePosupply", "ppsUniqueID", "ppsCalculateForAllParts", "ppsCompleted", "ppsFirmOnly",
			"ppsGenerated", "ppsJobIDs", "ppsPartClassIDs", "ppsPartIDs", "ppsPlantID", "ppsRowVersion", "ppsSalesOrderIDs", "ppsSessionID", "ppsSessionSubtotalBase", "ppsShowAllDemandForPartsOnJobs",
			"ppsSupplierIDs", "ppsWarehouseID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("ppsUniqueID|C", purchasePlannerSessionId);
		AddCustomFieldsToSelectList("PurchasePlannerSessions");
		using (DataTable dataTable = GetAsDataTable("PurchasePlannerSessions", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPurchasePlannerSessionInformationDto);
			}
			eRPPurchasePlannerSessionInformationDto.ppsBuyerEmployeeID = dataTable.Rows[0].Field<string>("ppsBuyerEmployeeID");
			eRPPurchasePlannerSessionInformationDto.ppsCompletedDate = dataTable.Rows[0].Field<DateTime?>("ppsCompletedDate");
			eRPPurchasePlannerSessionInformationDto.ppsCreatedBy = dataTable.Rows[0].Field<string>("ppsCreatedBy");
			eRPPurchasePlannerSessionInformationDto.ppsCreatedDate = dataTable.Rows[0].Field<DateTime?>("ppsCreatedDate");
			eRPPurchasePlannerSessionInformationDto.ppsCutoffDate = dataTable.Rows[0].Field<DateTime?>("ppsCutoffDate");
			eRPPurchasePlannerSessionInformationDto.ppsCutoffDatePosupply = dataTable.Rows[0].Field<DateTime?>("ppsCutoffDatePosupply");
			eRPPurchasePlannerSessionInformationDto.ppsUniqueID = dataTable.Rows[0].Field<Guid>("ppsUniqueID");
			eRPPurchasePlannerSessionInformationDto.ppsCalculateForAllParts = dataTable.Rows[0].Field<bool>("ppsCalculateForAllParts");
			eRPPurchasePlannerSessionInformationDto.ppsCompleted = dataTable.Rows[0].Field<bool>("ppsCompleted");
			eRPPurchasePlannerSessionInformationDto.ppsFirmOnly = dataTable.Rows[0].Field<bool>("ppsFirmOnly");
			eRPPurchasePlannerSessionInformationDto.ppsGenerated = dataTable.Rows[0].Field<bool>("ppsGenerated");
			eRPPurchasePlannerSessionInformationDto.ppsJobIDs = dataTable.Rows[0].Field<string>("ppsJobIDs");
			eRPPurchasePlannerSessionInformationDto.ppsPartClassIDs = dataTable.Rows[0].Field<string>("ppsPartClassIDs");
			eRPPurchasePlannerSessionInformationDto.ppsPartIDs = dataTable.Rows[0].Field<string>("ppsPartIDs");
			eRPPurchasePlannerSessionInformationDto.ppsPlantID = dataTable.Rows[0].Field<string>("ppsPlantID");
			eRPPurchasePlannerSessionInformationDto.ppsRowVersion = dataTable.Rows[0].Field<byte[]>("ppsRowVersion");
			eRPPurchasePlannerSessionInformationDto.ppsSalesOrderIDs = dataTable.Rows[0].Field<string>("ppsSalesOrderIDs");
			eRPPurchasePlannerSessionInformationDto.ppsSessionID = dataTable.Rows[0].Field<string>("ppsSessionID");
			eRPPurchasePlannerSessionInformationDto.ppsSessionSubtotalBase = dataTable.Rows[0].Field<decimal>("ppsSessionSubtotalBase");
			eRPPurchasePlannerSessionInformationDto.ppsShowAllDemandForPartsOnJobs = dataTable.Rows[0].Field<bool>("ppsShowAllDemandForPartsOnJobs");
			eRPPurchasePlannerSessionInformationDto.ppsSupplierIDs = dataTable.Rows[0].Field<string>("ppsSupplierIDs");
			eRPPurchasePlannerSessionInformationDto.ppsWarehouseID = dataTable.Rows[0].Field<string>("ppsWarehouseID");
			eRPPurchasePlannerSessionInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPurchasePlannerSessionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPurchasePlannerSessionInformationDto);
	}

	public Task<APIValidationInfoDto> SavePurchasePlannerSession(ERPPurchasePlannerSessionDto purchasePlannerSession)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PurchasePlannerSessions WHERE ppsUniqueID = " + M1Util.ConvertToLinq(purchasePlannerSession.ppsUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["ppsSessionID"] = purchasePlannerSession.ppsSessionID.ToUpper();
				purchasePlannerSession.ppsUniqueID = ((purchasePlannerSession.ppsUniqueID == Guid.Empty) ? Guid.NewGuid() : purchasePlannerSession.ppsUniqueID);
				dataRow["ppsUniqueID"] = purchasePlannerSession.ppsUniqueID;
				dataRow["ppsCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["ppsCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PurchasePlannerSession could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (purchasePlannerSession.ppsRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PurchasePlannerSession is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["ppsRowVersion"], purchasePlannerSession.ppsRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PurchasePlannerSession has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PurchasePlannerSession again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["ppsBuyerEmployeeID"] = purchasePlannerSession.ppsBuyerEmployeeID;
			DataRow dataRow2 = dataRow;
			DateTime? ppsCompletedDate = purchasePlannerSession.ppsCompletedDate;
			dataRow2["ppsCompletedDate"] = (ppsCompletedDate.HasValue ? ((object)ppsCompletedDate.GetValueOrDefault()) : dataRow["ppsCompletedDate"]);
			DataRow dataRow3 = dataRow;
			ppsCompletedDate = purchasePlannerSession.ppsCutoffDate;
			dataRow3["ppsCutoffDate"] = (ppsCompletedDate.HasValue ? ((object)ppsCompletedDate.GetValueOrDefault()) : dataRow["ppsCutoffDate"]);
			DataRow dataRow4 = dataRow;
			ppsCompletedDate = purchasePlannerSession.ppsCutoffDatePosupply;
			dataRow4["ppsCutoffDatePosupply"] = (ppsCompletedDate.HasValue ? ((object)ppsCompletedDate.GetValueOrDefault()) : dataRow["ppsCutoffDatePosupply"]);
			dataRow["ppsCalculateForAllParts"] = purchasePlannerSession.ppsCalculateForAllParts;
			dataRow["ppsCompleted"] = purchasePlannerSession.ppsCompleted;
			dataRow["ppsFirmOnly"] = purchasePlannerSession.ppsFirmOnly;
			dataRow["ppsGenerated"] = purchasePlannerSession.ppsGenerated;
			dataRow["ppsJobIDs"] = purchasePlannerSession.ppsJobIDs ?? dataRow["ppsJobIDs"];
			dataRow["ppsPartClassIDs"] = purchasePlannerSession.ppsPartClassIDs ?? dataRow["ppsPartClassIDs"];
			dataRow["ppsPartIDs"] = purchasePlannerSession.ppsPartIDs ?? dataRow["ppsPartIDs"];
			dataRow["ppsPlantID"] = purchasePlannerSession.ppsPlantID;
			dataRow["ppsSalesOrderIDs"] = purchasePlannerSession.ppsSalesOrderIDs ?? dataRow["ppsSalesOrderIDs"];
			dataRow["ppsSessionSubtotalBase"] = purchasePlannerSession.ppsSessionSubtotalBase;
			dataRow["ppsShowAllDemandForPartsOnJobs"] = purchasePlannerSession.ppsShowAllDemandForPartsOnJobs;
			dataRow["ppsSupplierIDs"] = purchasePlannerSession.ppsSupplierIDs ?? dataRow["ppsSupplierIDs"];
			dataRow["ppsWarehouseID"] = purchasePlannerSession.ppsWarehouseID;
			if (purchasePlannerSession.CustomFields != null && purchasePlannerSession.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in purchasePlannerSession.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PurchasePlannerSession [{purchasePlannerSession.ppsUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PurchasePlannerSession [{purchasePlannerSession.ppsUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
