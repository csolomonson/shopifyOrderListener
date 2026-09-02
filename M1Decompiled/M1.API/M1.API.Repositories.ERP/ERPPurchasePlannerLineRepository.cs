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

public class ERPPurchasePlannerLineRepository : APIBaseRepository, IERPPurchasePlannerLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPPurchasePlannerLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPurchasePlannerLineExist(Guid purchasePlannerLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("pplUniqueID|C", purchasePlannerLineId);
		base.selectList.Add("pplUniqueID");
		return Task.FromResult(GetAsObject("PurchasePlannerLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPurchasePlannerLineInformationDto>> GetAllPurchasePlannerLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPurchasePlannerLineInformationDto> collection = new List<ERPPurchasePlannerLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[22]
		{
			"pplCreatedBy", "pplCreatedDate", "pplDataMissing", "pplUniqueID", "pplExtendedCostBase", "pplCompleted", "pplNonStockedItem", "pplPhantomOrKitPart", "pplLastRunDate", "pplLineID",
			"pplLotSize", "pplMaximumQuantity", "pplMinimumQuantity", "pplPartID", "pplPartRevisionID", "pplPartShortDescription", "pplPlantID", "pplQuantityOnHand", "pplReorderMethod", "pplRowVersion",
			"pplSessionID", "pplWarehouseID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PurchasePlannerLines");
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
		using (DataTable dataTable = GetAsDataTable("PurchasePlannerLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPurchasePlannerLineInformationDto eRPPurchasePlannerLineInformationDto = new ERPPurchasePlannerLineInformationDto();
				eRPPurchasePlannerLineInformationDto.pplCreatedBy = dataTable.Rows[i].Field<string>("pplCreatedBy");
				eRPPurchasePlannerLineInformationDto.pplCreatedDate = dataTable.Rows[i].Field<DateTime?>("pplCreatedDate");
				eRPPurchasePlannerLineInformationDto.pplDataMissing = dataTable.Rows[i].Field<int>("pplDataMissing");
				eRPPurchasePlannerLineInformationDto.pplUniqueID = dataTable.Rows[i].Field<Guid>("pplUniqueID");
				eRPPurchasePlannerLineInformationDto.pplExtendedCostBase = dataTable.Rows[i].Field<decimal>("pplExtendedCostBase");
				eRPPurchasePlannerLineInformationDto.pplCompleted = dataTable.Rows[i].Field<bool>("pplCompleted");
				eRPPurchasePlannerLineInformationDto.pplNonStockedItem = dataTable.Rows[i].Field<bool>("pplNonStockedItem");
				eRPPurchasePlannerLineInformationDto.pplPhantomOrKitPart = dataTable.Rows[i].Field<bool>("pplPhantomOrKitPart");
				eRPPurchasePlannerLineInformationDto.pplLastRunDate = dataTable.Rows[i].Field<DateTime?>("pplLastRunDate");
				eRPPurchasePlannerLineInformationDto.pplLineID = dataTable.Rows[i].Field<int>("pplLineID");
				eRPPurchasePlannerLineInformationDto.pplLotSize = dataTable.Rows[i].Field<decimal>("pplLotSize");
				eRPPurchasePlannerLineInformationDto.pplMaximumQuantity = dataTable.Rows[i].Field<decimal>("pplMaximumQuantity");
				eRPPurchasePlannerLineInformationDto.pplMinimumQuantity = dataTable.Rows[i].Field<decimal>("pplMinimumQuantity");
				eRPPurchasePlannerLineInformationDto.pplPartID = dataTable.Rows[i].Field<string>("pplPartID");
				eRPPurchasePlannerLineInformationDto.pplPartRevisionID = dataTable.Rows[i].Field<string>("pplPartRevisionID");
				eRPPurchasePlannerLineInformationDto.pplPartShortDescription = dataTable.Rows[i].Field<string>("pplPartShortDescription");
				eRPPurchasePlannerLineInformationDto.pplPlantID = dataTable.Rows[i].Field<string>("pplPlantID");
				eRPPurchasePlannerLineInformationDto.pplQuantityOnHand = dataTable.Rows[i].Field<decimal>("pplQuantityOnHand");
				eRPPurchasePlannerLineInformationDto.pplReorderMethod = dataTable.Rows[i].Field<byte>("pplReorderMethod");
				eRPPurchasePlannerLineInformationDto.pplRowVersion = dataTable.Rows[i].Field<byte[]>("pplRowVersion");
				eRPPurchasePlannerLineInformationDto.pplSessionID = dataTable.Rows[i].Field<string>("pplSessionID");
				eRPPurchasePlannerLineInformationDto.pplWarehouseID = dataTable.Rows[i].Field<string>("pplWarehouseID");
				eRPPurchasePlannerLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPurchasePlannerLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPurchasePlannerLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPurchasePlannerLineInformationDto> GetPurchasePlannerLine(Guid purchasePlannerLineId)
	{
		ERPPurchasePlannerLineInformationDto eRPPurchasePlannerLineInformationDto = new ERPPurchasePlannerLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[22]
		{
			"pplCreatedBy", "pplCreatedDate", "pplDataMissing", "pplUniqueID", "pplExtendedCostBase", "pplCompleted", "pplNonStockedItem", "pplPhantomOrKitPart", "pplLastRunDate", "pplLineID",
			"pplLotSize", "pplMaximumQuantity", "pplMinimumQuantity", "pplPartID", "pplPartRevisionID", "pplPartShortDescription", "pplPlantID", "pplQuantityOnHand", "pplReorderMethod", "pplRowVersion",
			"pplSessionID", "pplWarehouseID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("pplUniqueID|C", purchasePlannerLineId);
		AddCustomFieldsToSelectList("PurchasePlannerLines");
		using (DataTable dataTable = GetAsDataTable("PurchasePlannerLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPurchasePlannerLineInformationDto);
			}
			eRPPurchasePlannerLineInformationDto.pplCreatedBy = dataTable.Rows[0].Field<string>("pplCreatedBy");
			eRPPurchasePlannerLineInformationDto.pplCreatedDate = dataTable.Rows[0].Field<DateTime?>("pplCreatedDate");
			eRPPurchasePlannerLineInformationDto.pplDataMissing = dataTable.Rows[0].Field<int>("pplDataMissing");
			eRPPurchasePlannerLineInformationDto.pplUniqueID = dataTable.Rows[0].Field<Guid>("pplUniqueID");
			eRPPurchasePlannerLineInformationDto.pplExtendedCostBase = dataTable.Rows[0].Field<decimal>("pplExtendedCostBase");
			eRPPurchasePlannerLineInformationDto.pplCompleted = dataTable.Rows[0].Field<bool>("pplCompleted");
			eRPPurchasePlannerLineInformationDto.pplNonStockedItem = dataTable.Rows[0].Field<bool>("pplNonStockedItem");
			eRPPurchasePlannerLineInformationDto.pplPhantomOrKitPart = dataTable.Rows[0].Field<bool>("pplPhantomOrKitPart");
			eRPPurchasePlannerLineInformationDto.pplLastRunDate = dataTable.Rows[0].Field<DateTime?>("pplLastRunDate");
			eRPPurchasePlannerLineInformationDto.pplLineID = dataTable.Rows[0].Field<int>("pplLineID");
			eRPPurchasePlannerLineInformationDto.pplLotSize = dataTable.Rows[0].Field<decimal>("pplLotSize");
			eRPPurchasePlannerLineInformationDto.pplMaximumQuantity = dataTable.Rows[0].Field<decimal>("pplMaximumQuantity");
			eRPPurchasePlannerLineInformationDto.pplMinimumQuantity = dataTable.Rows[0].Field<decimal>("pplMinimumQuantity");
			eRPPurchasePlannerLineInformationDto.pplPartID = dataTable.Rows[0].Field<string>("pplPartID");
			eRPPurchasePlannerLineInformationDto.pplPartRevisionID = dataTable.Rows[0].Field<string>("pplPartRevisionID");
			eRPPurchasePlannerLineInformationDto.pplPartShortDescription = dataTable.Rows[0].Field<string>("pplPartShortDescription");
			eRPPurchasePlannerLineInformationDto.pplPlantID = dataTable.Rows[0].Field<string>("pplPlantID");
			eRPPurchasePlannerLineInformationDto.pplQuantityOnHand = dataTable.Rows[0].Field<decimal>("pplQuantityOnHand");
			eRPPurchasePlannerLineInformationDto.pplReorderMethod = dataTable.Rows[0].Field<byte>("pplReorderMethod");
			eRPPurchasePlannerLineInformationDto.pplRowVersion = dataTable.Rows[0].Field<byte[]>("pplRowVersion");
			eRPPurchasePlannerLineInformationDto.pplSessionID = dataTable.Rows[0].Field<string>("pplSessionID");
			eRPPurchasePlannerLineInformationDto.pplWarehouseID = dataTable.Rows[0].Field<string>("pplWarehouseID");
			eRPPurchasePlannerLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPurchasePlannerLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPurchasePlannerLineInformationDto);
	}

	public Task<APIValidationInfoDto> SavePurchasePlannerLine(ERPPurchasePlannerLineDto purchasePlannerLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PurchasePlannerLines WHERE pplUniqueID = " + M1Util.ConvertToLinq(purchasePlannerLine.pplUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["pplSessionID"] = purchasePlannerLine.pplSessionID.ToUpper();
				dataRow["pplLineID"] = purchasePlannerLine.pplLineID;
				purchasePlannerLine.pplUniqueID = ((purchasePlannerLine.pplUniqueID == Guid.Empty) ? Guid.NewGuid() : purchasePlannerLine.pplUniqueID);
				dataRow["pplUniqueID"] = purchasePlannerLine.pplUniqueID;
				dataRow["pplCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["pplCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PurchasePlannerLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (purchasePlannerLine.pplRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PurchasePlannerLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["pplRowVersion"], purchasePlannerLine.pplRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PurchasePlannerLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PurchasePlannerLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["pplDataMissing"] = purchasePlannerLine.pplDataMissing;
			dataRow["pplExtendedCostBase"] = purchasePlannerLine.pplExtendedCostBase;
			dataRow["pplCompleted"] = purchasePlannerLine.pplCompleted;
			dataRow["pplNonStockedItem"] = purchasePlannerLine.pplNonStockedItem;
			dataRow["pplPhantomOrKitPart"] = purchasePlannerLine.pplPhantomOrKitPart;
			DataRow dataRow2 = dataRow;
			DateTime? pplLastRunDate = purchasePlannerLine.pplLastRunDate;
			dataRow2["pplLastRunDate"] = (pplLastRunDate.HasValue ? ((object)pplLastRunDate.GetValueOrDefault()) : dataRow["pplLastRunDate"]);
			dataRow["pplLotSize"] = purchasePlannerLine.pplLotSize;
			dataRow["pplMaximumQuantity"] = purchasePlannerLine.pplMaximumQuantity;
			dataRow["pplMinimumQuantity"] = purchasePlannerLine.pplMinimumQuantity;
			dataRow["pplPartID"] = purchasePlannerLine.pplPartID;
			dataRow["pplPartRevisionID"] = purchasePlannerLine.pplPartRevisionID;
			dataRow["pplPartShortDescription"] = purchasePlannerLine.pplPartShortDescription;
			dataRow["pplPlantID"] = purchasePlannerLine.pplPlantID;
			dataRow["pplQuantityOnHand"] = purchasePlannerLine.pplQuantityOnHand;
			dataRow["pplReorderMethod"] = purchasePlannerLine.pplReorderMethod;
			dataRow["pplWarehouseID"] = purchasePlannerLine.pplWarehouseID;
			if (purchasePlannerLine.CustomFields != null && purchasePlannerLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in purchasePlannerLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PurchasePlannerLine [{purchasePlannerLine.pplUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PurchasePlannerLine [{purchasePlannerLine.pplUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
