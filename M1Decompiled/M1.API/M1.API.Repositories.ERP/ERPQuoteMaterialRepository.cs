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

public class ERPQuoteMaterialRepository : APIBaseRepository, IERPQuoteMaterialRepository, IAPIBaseRepository, IDisposable
{
	public ERPQuoteMaterialRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesQuoteMaterialExist(Guid quoteMaterialId)
	{
		InitializeParameterLists();
		base.filterList.Add("qmmUniqueID|C", quoteMaterialId);
		base.selectList.Add("qmmUniqueID");
		return Task.FromResult(GetAsObject("QuoteMaterials", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPQuoteMaterialInformationDto>> GetAllQuoteMaterials(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPQuoteMaterialInformationDto> collection = new List<ERPQuoteMaterialInformationDto>();
		InitializeParameterLists();
		string[] array = new string[58]
		{
			"qmmCreatedBy", "qmmCreatedDate", "qmmDocuments", "qmmUniqueID", "qmmEstimatedUnitCost", "qmmBackflush", "qmmClosed", "qmmCostOverride", "qmmLeadTime", "qmmLeadTime1",
			"qmmLeadTime2", "qmmLeadTime3", "qmmLeadTime4", "qmmLeadTime5", "qmmLeadTime6", "qmmLeadTime7", "qmmLeadTime8", "qmmLeadTime9", "qmmMinimumCharge", "qmmPartBinID",
			"qmmPartID", "qmmPartLongDescriptionRtf", "qmmPartLongDescriptionText", "qmmPartRevisionID", "qmmPartShortDescription", "qmmPartWarehouseLocationID", "qmmPurchaseLocationID", "qmmQuantityBreak1", "qmmQuantityBreak2", "qmmQuantityBreak3",
			"qmmQuantityBreak4", "qmmQuantityBreak5", "qmmQuantityBreak6", "qmmQuantityBreak7", "qmmQuantityBreak8", "qmmQuantityBreak9", "qmmQuantityPerAssembly", "qmmQuoteAssemblyID", "qmmQuoteID", "qmmQuoteLineID",
			"qmmRelatedQuoteOperationID", "qmmRowVersion", "qmmScrapPercent", "qmmScrapQuantity", "qmmQuoteMaterialID", "qmmSourcePriceID", "qmmSourceRfqID", "qmmSupplierOrganizationID", "qmmUnitCost1", "qmmUnitCost2",
			"qmmUnitCost3", "qmmUnitCost4", "qmmUnitCost5", "qmmUnitCost6", "qmmUnitCost7", "qmmUnitCost8", "qmmUnitCost9", "qmmUnitOfMeasure"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("QuoteMaterials");
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
		using (DataTable dataTable = GetAsDataTable("QuoteMaterials", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPQuoteMaterialInformationDto eRPQuoteMaterialInformationDto = new ERPQuoteMaterialInformationDto();
				eRPQuoteMaterialInformationDto.qmmCreatedBy = dataTable.Rows[i].Field<string>("qmmCreatedBy");
				eRPQuoteMaterialInformationDto.qmmCreatedDate = dataTable.Rows[i].Field<DateTime?>("qmmCreatedDate");
				eRPQuoteMaterialInformationDto.qmmDocuments = dataTable.Rows[i].Field<string>("qmmDocuments");
				eRPQuoteMaterialInformationDto.qmmUniqueID = dataTable.Rows[i].Field<Guid>("qmmUniqueID");
				eRPQuoteMaterialInformationDto.qmmEstimatedUnitCost = dataTable.Rows[i].Field<decimal>("qmmEstimatedUnitCost");
				eRPQuoteMaterialInformationDto.qmmBackflush = dataTable.Rows[i].Field<bool>("qmmBackflush");
				eRPQuoteMaterialInformationDto.qmmClosed = dataTable.Rows[i].Field<bool>("qmmClosed");
				eRPQuoteMaterialInformationDto.qmmCostOverride = dataTable.Rows[i].Field<bool>("qmmCostOverride");
				eRPQuoteMaterialInformationDto.qmmLeadTime = dataTable.Rows[i].Field<short>("qmmLeadTime");
				eRPQuoteMaterialInformationDto.qmmLeadTime1 = dataTable.Rows[i].Field<short>("qmmLeadTime1");
				eRPQuoteMaterialInformationDto.qmmLeadTime2 = dataTable.Rows[i].Field<short>("qmmLeadTime2");
				eRPQuoteMaterialInformationDto.qmmLeadTime3 = dataTable.Rows[i].Field<short>("qmmLeadTime3");
				eRPQuoteMaterialInformationDto.qmmLeadTime4 = dataTable.Rows[i].Field<short>("qmmLeadTime4");
				eRPQuoteMaterialInformationDto.qmmLeadTime5 = dataTable.Rows[i].Field<short>("qmmLeadTime5");
				eRPQuoteMaterialInformationDto.qmmLeadTime6 = dataTable.Rows[i].Field<short>("qmmLeadTime6");
				eRPQuoteMaterialInformationDto.qmmLeadTime7 = dataTable.Rows[i].Field<short>("qmmLeadTime7");
				eRPQuoteMaterialInformationDto.qmmLeadTime8 = dataTable.Rows[i].Field<short>("qmmLeadTime8");
				eRPQuoteMaterialInformationDto.qmmLeadTime9 = dataTable.Rows[i].Field<short>("qmmLeadTime9");
				eRPQuoteMaterialInformationDto.qmmMinimumCharge = dataTable.Rows[i].Field<decimal>("qmmMinimumCharge");
				eRPQuoteMaterialInformationDto.qmmPartBinID = dataTable.Rows[i].Field<string>("qmmPartBinID");
				eRPQuoteMaterialInformationDto.qmmPartID = dataTable.Rows[i].Field<string>("qmmPartID");
				eRPQuoteMaterialInformationDto.qmmPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("qmmPartLongDescriptionRtf");
				eRPQuoteMaterialInformationDto.qmmPartLongDescriptionText = dataTable.Rows[i].Field<string>("qmmPartLongDescriptionText");
				eRPQuoteMaterialInformationDto.qmmPartRevisionID = dataTable.Rows[i].Field<string>("qmmPartRevisionID");
				eRPQuoteMaterialInformationDto.qmmPartShortDescription = dataTable.Rows[i].Field<string>("qmmPartShortDescription");
				eRPQuoteMaterialInformationDto.qmmPartWarehouseLocationID = dataTable.Rows[i].Field<string>("qmmPartWarehouseLocationID");
				eRPQuoteMaterialInformationDto.qmmPurchaseLocationID = dataTable.Rows[i].Field<string>("qmmPurchaseLocationID");
				eRPQuoteMaterialInformationDto.qmmQuantityBreak1 = dataTable.Rows[i].Field<decimal>("qmmQuantityBreak1");
				eRPQuoteMaterialInformationDto.qmmQuantityBreak2 = dataTable.Rows[i].Field<decimal>("qmmQuantityBreak2");
				eRPQuoteMaterialInformationDto.qmmQuantityBreak3 = dataTable.Rows[i].Field<decimal>("qmmQuantityBreak3");
				eRPQuoteMaterialInformationDto.qmmQuantityBreak4 = dataTable.Rows[i].Field<decimal>("qmmQuantityBreak4");
				eRPQuoteMaterialInformationDto.qmmQuantityBreak5 = dataTable.Rows[i].Field<decimal>("qmmQuantityBreak5");
				eRPQuoteMaterialInformationDto.qmmQuantityBreak6 = dataTable.Rows[i].Field<decimal>("qmmQuantityBreak6");
				eRPQuoteMaterialInformationDto.qmmQuantityBreak7 = dataTable.Rows[i].Field<decimal>("qmmQuantityBreak7");
				eRPQuoteMaterialInformationDto.qmmQuantityBreak8 = dataTable.Rows[i].Field<decimal>("qmmQuantityBreak8");
				eRPQuoteMaterialInformationDto.qmmQuantityBreak9 = dataTable.Rows[i].Field<decimal>("qmmQuantityBreak9");
				eRPQuoteMaterialInformationDto.qmmQuantityPerAssembly = dataTable.Rows[i].Field<decimal>("qmmQuantityPerAssembly");
				eRPQuoteMaterialInformationDto.qmmQuoteAssemblyID = dataTable.Rows[i].Field<int>("qmmQuoteAssemblyID");
				eRPQuoteMaterialInformationDto.qmmQuoteID = dataTable.Rows[i].Field<string>("qmmQuoteID");
				eRPQuoteMaterialInformationDto.qmmQuoteLineID = dataTable.Rows[i].Field<short>("qmmQuoteLineID");
				eRPQuoteMaterialInformationDto.qmmRelatedQuoteOperationID = dataTable.Rows[i].Field<int>("qmmRelatedQuoteOperationID");
				eRPQuoteMaterialInformationDto.qmmRowVersion = dataTable.Rows[i].Field<byte[]>("qmmRowVersion");
				eRPQuoteMaterialInformationDto.qmmScrapPercent = dataTable.Rows[i].Field<decimal>("qmmScrapPercent");
				eRPQuoteMaterialInformationDto.qmmScrapQuantity = dataTable.Rows[i].Field<decimal>("qmmScrapQuantity");
				eRPQuoteMaterialInformationDto.qmmQuoteMaterialID = dataTable.Rows[i].Field<int>("qmmQuoteMaterialID");
				eRPQuoteMaterialInformationDto.qmmSourcePriceID = dataTable.Rows[i].Field<int>("qmmSourcePriceID");
				eRPQuoteMaterialInformationDto.qmmSourceRfqID = dataTable.Rows[i].Field<string>("qmmSourceRfqID");
				eRPQuoteMaterialInformationDto.qmmSupplierOrganizationID = dataTable.Rows[i].Field<string>("qmmSupplierOrganizationID");
				eRPQuoteMaterialInformationDto.qmmUnitCost1 = dataTable.Rows[i].Field<decimal>("qmmUnitCost1");
				eRPQuoteMaterialInformationDto.qmmUnitCost2 = dataTable.Rows[i].Field<decimal>("qmmUnitCost2");
				eRPQuoteMaterialInformationDto.qmmUnitCost3 = dataTable.Rows[i].Field<decimal>("qmmUnitCost3");
				eRPQuoteMaterialInformationDto.qmmUnitCost4 = dataTable.Rows[i].Field<decimal>("qmmUnitCost4");
				eRPQuoteMaterialInformationDto.qmmUnitCost5 = dataTable.Rows[i].Field<decimal>("qmmUnitCost5");
				eRPQuoteMaterialInformationDto.qmmUnitCost6 = dataTable.Rows[i].Field<decimal>("qmmUnitCost6");
				eRPQuoteMaterialInformationDto.qmmUnitCost7 = dataTable.Rows[i].Field<decimal>("qmmUnitCost7");
				eRPQuoteMaterialInformationDto.qmmUnitCost8 = dataTable.Rows[i].Field<decimal>("qmmUnitCost8");
				eRPQuoteMaterialInformationDto.qmmUnitCost9 = dataTable.Rows[i].Field<decimal>("qmmUnitCost9");
				eRPQuoteMaterialInformationDto.qmmUnitOfMeasure = dataTable.Rows[i].Field<string>("qmmUnitOfMeasure");
				eRPQuoteMaterialInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPQuoteMaterialInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPQuoteMaterialInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPQuoteMaterialInformationDto> GetQuoteMaterial(Guid quoteMaterialId)
	{
		ERPQuoteMaterialInformationDto eRPQuoteMaterialInformationDto = new ERPQuoteMaterialInformationDto();
		InitializeParameterLists();
		string[] collection = new string[58]
		{
			"qmmCreatedBy", "qmmCreatedDate", "qmmDocuments", "qmmUniqueID", "qmmEstimatedUnitCost", "qmmBackflush", "qmmClosed", "qmmCostOverride", "qmmLeadTime", "qmmLeadTime1",
			"qmmLeadTime2", "qmmLeadTime3", "qmmLeadTime4", "qmmLeadTime5", "qmmLeadTime6", "qmmLeadTime7", "qmmLeadTime8", "qmmLeadTime9", "qmmMinimumCharge", "qmmPartBinID",
			"qmmPartID", "qmmPartLongDescriptionRtf", "qmmPartLongDescriptionText", "qmmPartRevisionID", "qmmPartShortDescription", "qmmPartWarehouseLocationID", "qmmPurchaseLocationID", "qmmQuantityBreak1", "qmmQuantityBreak2", "qmmQuantityBreak3",
			"qmmQuantityBreak4", "qmmQuantityBreak5", "qmmQuantityBreak6", "qmmQuantityBreak7", "qmmQuantityBreak8", "qmmQuantityBreak9", "qmmQuantityPerAssembly", "qmmQuoteAssemblyID", "qmmQuoteID", "qmmQuoteLineID",
			"qmmRelatedQuoteOperationID", "qmmRowVersion", "qmmScrapPercent", "qmmScrapQuantity", "qmmQuoteMaterialID", "qmmSourcePriceID", "qmmSourceRfqID", "qmmSupplierOrganizationID", "qmmUnitCost1", "qmmUnitCost2",
			"qmmUnitCost3", "qmmUnitCost4", "qmmUnitCost5", "qmmUnitCost6", "qmmUnitCost7", "qmmUnitCost8", "qmmUnitCost9", "qmmUnitOfMeasure"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("qmmUniqueID|C", quoteMaterialId);
		AddCustomFieldsToSelectList("QuoteMaterials");
		using (DataTable dataTable = GetAsDataTable("QuoteMaterials", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPQuoteMaterialInformationDto);
			}
			eRPQuoteMaterialInformationDto.qmmCreatedBy = dataTable.Rows[0].Field<string>("qmmCreatedBy");
			eRPQuoteMaterialInformationDto.qmmCreatedDate = dataTable.Rows[0].Field<DateTime?>("qmmCreatedDate");
			eRPQuoteMaterialInformationDto.qmmDocuments = dataTable.Rows[0].Field<string>("qmmDocuments");
			eRPQuoteMaterialInformationDto.qmmUniqueID = dataTable.Rows[0].Field<Guid>("qmmUniqueID");
			eRPQuoteMaterialInformationDto.qmmEstimatedUnitCost = dataTable.Rows[0].Field<decimal>("qmmEstimatedUnitCost");
			eRPQuoteMaterialInformationDto.qmmBackflush = dataTable.Rows[0].Field<bool>("qmmBackflush");
			eRPQuoteMaterialInformationDto.qmmClosed = dataTable.Rows[0].Field<bool>("qmmClosed");
			eRPQuoteMaterialInformationDto.qmmCostOverride = dataTable.Rows[0].Field<bool>("qmmCostOverride");
			eRPQuoteMaterialInformationDto.qmmLeadTime = dataTable.Rows[0].Field<short>("qmmLeadTime");
			eRPQuoteMaterialInformationDto.qmmLeadTime1 = dataTable.Rows[0].Field<short>("qmmLeadTime1");
			eRPQuoteMaterialInformationDto.qmmLeadTime2 = dataTable.Rows[0].Field<short>("qmmLeadTime2");
			eRPQuoteMaterialInformationDto.qmmLeadTime3 = dataTable.Rows[0].Field<short>("qmmLeadTime3");
			eRPQuoteMaterialInformationDto.qmmLeadTime4 = dataTable.Rows[0].Field<short>("qmmLeadTime4");
			eRPQuoteMaterialInformationDto.qmmLeadTime5 = dataTable.Rows[0].Field<short>("qmmLeadTime5");
			eRPQuoteMaterialInformationDto.qmmLeadTime6 = dataTable.Rows[0].Field<short>("qmmLeadTime6");
			eRPQuoteMaterialInformationDto.qmmLeadTime7 = dataTable.Rows[0].Field<short>("qmmLeadTime7");
			eRPQuoteMaterialInformationDto.qmmLeadTime8 = dataTable.Rows[0].Field<short>("qmmLeadTime8");
			eRPQuoteMaterialInformationDto.qmmLeadTime9 = dataTable.Rows[0].Field<short>("qmmLeadTime9");
			eRPQuoteMaterialInformationDto.qmmMinimumCharge = dataTable.Rows[0].Field<decimal>("qmmMinimumCharge");
			eRPQuoteMaterialInformationDto.qmmPartBinID = dataTable.Rows[0].Field<string>("qmmPartBinID");
			eRPQuoteMaterialInformationDto.qmmPartID = dataTable.Rows[0].Field<string>("qmmPartID");
			eRPQuoteMaterialInformationDto.qmmPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("qmmPartLongDescriptionRtf");
			eRPQuoteMaterialInformationDto.qmmPartLongDescriptionText = dataTable.Rows[0].Field<string>("qmmPartLongDescriptionText");
			eRPQuoteMaterialInformationDto.qmmPartRevisionID = dataTable.Rows[0].Field<string>("qmmPartRevisionID");
			eRPQuoteMaterialInformationDto.qmmPartShortDescription = dataTable.Rows[0].Field<string>("qmmPartShortDescription");
			eRPQuoteMaterialInformationDto.qmmPartWarehouseLocationID = dataTable.Rows[0].Field<string>("qmmPartWarehouseLocationID");
			eRPQuoteMaterialInformationDto.qmmPurchaseLocationID = dataTable.Rows[0].Field<string>("qmmPurchaseLocationID");
			eRPQuoteMaterialInformationDto.qmmQuantityBreak1 = dataTable.Rows[0].Field<decimal>("qmmQuantityBreak1");
			eRPQuoteMaterialInformationDto.qmmQuantityBreak2 = dataTable.Rows[0].Field<decimal>("qmmQuantityBreak2");
			eRPQuoteMaterialInformationDto.qmmQuantityBreak3 = dataTable.Rows[0].Field<decimal>("qmmQuantityBreak3");
			eRPQuoteMaterialInformationDto.qmmQuantityBreak4 = dataTable.Rows[0].Field<decimal>("qmmQuantityBreak4");
			eRPQuoteMaterialInformationDto.qmmQuantityBreak5 = dataTable.Rows[0].Field<decimal>("qmmQuantityBreak5");
			eRPQuoteMaterialInformationDto.qmmQuantityBreak6 = dataTable.Rows[0].Field<decimal>("qmmQuantityBreak6");
			eRPQuoteMaterialInformationDto.qmmQuantityBreak7 = dataTable.Rows[0].Field<decimal>("qmmQuantityBreak7");
			eRPQuoteMaterialInformationDto.qmmQuantityBreak8 = dataTable.Rows[0].Field<decimal>("qmmQuantityBreak8");
			eRPQuoteMaterialInformationDto.qmmQuantityBreak9 = dataTable.Rows[0].Field<decimal>("qmmQuantityBreak9");
			eRPQuoteMaterialInformationDto.qmmQuantityPerAssembly = dataTable.Rows[0].Field<decimal>("qmmQuantityPerAssembly");
			eRPQuoteMaterialInformationDto.qmmQuoteAssemblyID = dataTable.Rows[0].Field<int>("qmmQuoteAssemblyID");
			eRPQuoteMaterialInformationDto.qmmQuoteID = dataTable.Rows[0].Field<string>("qmmQuoteID");
			eRPQuoteMaterialInformationDto.qmmQuoteLineID = dataTable.Rows[0].Field<short>("qmmQuoteLineID");
			eRPQuoteMaterialInformationDto.qmmRelatedQuoteOperationID = dataTable.Rows[0].Field<int>("qmmRelatedQuoteOperationID");
			eRPQuoteMaterialInformationDto.qmmRowVersion = dataTable.Rows[0].Field<byte[]>("qmmRowVersion");
			eRPQuoteMaterialInformationDto.qmmScrapPercent = dataTable.Rows[0].Field<decimal>("qmmScrapPercent");
			eRPQuoteMaterialInformationDto.qmmScrapQuantity = dataTable.Rows[0].Field<decimal>("qmmScrapQuantity");
			eRPQuoteMaterialInformationDto.qmmQuoteMaterialID = dataTable.Rows[0].Field<int>("qmmQuoteMaterialID");
			eRPQuoteMaterialInformationDto.qmmSourcePriceID = dataTable.Rows[0].Field<int>("qmmSourcePriceID");
			eRPQuoteMaterialInformationDto.qmmSourceRfqID = dataTable.Rows[0].Field<string>("qmmSourceRfqID");
			eRPQuoteMaterialInformationDto.qmmSupplierOrganizationID = dataTable.Rows[0].Field<string>("qmmSupplierOrganizationID");
			eRPQuoteMaterialInformationDto.qmmUnitCost1 = dataTable.Rows[0].Field<decimal>("qmmUnitCost1");
			eRPQuoteMaterialInformationDto.qmmUnitCost2 = dataTable.Rows[0].Field<decimal>("qmmUnitCost2");
			eRPQuoteMaterialInformationDto.qmmUnitCost3 = dataTable.Rows[0].Field<decimal>("qmmUnitCost3");
			eRPQuoteMaterialInformationDto.qmmUnitCost4 = dataTable.Rows[0].Field<decimal>("qmmUnitCost4");
			eRPQuoteMaterialInformationDto.qmmUnitCost5 = dataTable.Rows[0].Field<decimal>("qmmUnitCost5");
			eRPQuoteMaterialInformationDto.qmmUnitCost6 = dataTable.Rows[0].Field<decimal>("qmmUnitCost6");
			eRPQuoteMaterialInformationDto.qmmUnitCost7 = dataTable.Rows[0].Field<decimal>("qmmUnitCost7");
			eRPQuoteMaterialInformationDto.qmmUnitCost8 = dataTable.Rows[0].Field<decimal>("qmmUnitCost8");
			eRPQuoteMaterialInformationDto.qmmUnitCost9 = dataTable.Rows[0].Field<decimal>("qmmUnitCost9");
			eRPQuoteMaterialInformationDto.qmmUnitOfMeasure = dataTable.Rows[0].Field<string>("qmmUnitOfMeasure");
			eRPQuoteMaterialInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPQuoteMaterialInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPQuoteMaterialInformationDto);
	}

	public Task<APIValidationInfoDto> SaveQuoteMaterial(ERPQuoteMaterialDto quoteMaterial)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM QuoteMaterials WHERE qmmUniqueID = " + M1Util.ConvertToLinq(quoteMaterial.qmmUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qmmQuoteID"] = quoteMaterial.qmmQuoteID.ToUpper();
				dataRow["qmmQuoteLineID"] = quoteMaterial.qmmQuoteLineID;
				dataRow["qmmQuoteAssemblyID"] = quoteMaterial.qmmQuoteAssemblyID;
				dataRow["qmmQuoteMaterialID"] = quoteMaterial.qmmQuoteMaterialID;
				quoteMaterial.qmmUniqueID = ((quoteMaterial.qmmUniqueID == Guid.Empty) ? Guid.NewGuid() : quoteMaterial.qmmUniqueID);
				dataRow["qmmUniqueID"] = quoteMaterial.qmmUniqueID;
				dataRow["qmmCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qmmCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The QuoteMaterial could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (quoteMaterial.qmmRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the QuoteMaterial is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["qmmRowVersion"], quoteMaterial.qmmRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the QuoteMaterial has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the QuoteMaterial again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qmmDocuments"] = quoteMaterial.qmmDocuments ?? dataRow["qmmDocuments"];
			dataRow["qmmEstimatedUnitCost"] = quoteMaterial.qmmEstimatedUnitCost;
			dataRow["qmmBackflush"] = quoteMaterial.qmmBackflush;
			dataRow["qmmClosed"] = quoteMaterial.qmmClosed;
			dataRow["qmmCostOverride"] = quoteMaterial.qmmCostOverride;
			dataRow["qmmLeadTime"] = quoteMaterial.qmmLeadTime;
			dataRow["qmmLeadTime1"] = quoteMaterial.qmmLeadTime1;
			dataRow["qmmLeadTime2"] = quoteMaterial.qmmLeadTime2;
			dataRow["qmmLeadTime3"] = quoteMaterial.qmmLeadTime3;
			dataRow["qmmLeadTime4"] = quoteMaterial.qmmLeadTime4;
			dataRow["qmmLeadTime5"] = quoteMaterial.qmmLeadTime5;
			dataRow["qmmLeadTime6"] = quoteMaterial.qmmLeadTime6;
			dataRow["qmmLeadTime7"] = quoteMaterial.qmmLeadTime7;
			dataRow["qmmLeadTime8"] = quoteMaterial.qmmLeadTime8;
			dataRow["qmmLeadTime9"] = quoteMaterial.qmmLeadTime9;
			dataRow["qmmMinimumCharge"] = quoteMaterial.qmmMinimumCharge;
			dataRow["qmmPartBinID"] = quoteMaterial.qmmPartBinID;
			dataRow["qmmPartID"] = quoteMaterial.qmmPartID;
			dataRow["qmmPartLongDescriptionRtf"] = quoteMaterial.qmmPartLongDescriptionRtf ?? dataRow["qmmPartLongDescriptionRtf"];
			dataRow["qmmPartLongDescriptionText"] = quoteMaterial.qmmPartLongDescriptionText ?? dataRow["qmmPartLongDescriptionText"];
			dataRow["qmmPartRevisionID"] = quoteMaterial.qmmPartRevisionID;
			dataRow["qmmPartShortDescription"] = quoteMaterial.qmmPartShortDescription;
			dataRow["qmmPartWarehouseLocationID"] = quoteMaterial.qmmPartWarehouseLocationID;
			dataRow["qmmPurchaseLocationID"] = quoteMaterial.qmmPurchaseLocationID;
			dataRow["qmmQuantityBreak1"] = quoteMaterial.qmmQuantityBreak1;
			dataRow["qmmQuantityBreak2"] = quoteMaterial.qmmQuantityBreak2;
			dataRow["qmmQuantityBreak3"] = quoteMaterial.qmmQuantityBreak3;
			dataRow["qmmQuantityBreak4"] = quoteMaterial.qmmQuantityBreak4;
			dataRow["qmmQuantityBreak5"] = quoteMaterial.qmmQuantityBreak5;
			dataRow["qmmQuantityBreak6"] = quoteMaterial.qmmQuantityBreak6;
			dataRow["qmmQuantityBreak7"] = quoteMaterial.qmmQuantityBreak7;
			dataRow["qmmQuantityBreak8"] = quoteMaterial.qmmQuantityBreak8;
			dataRow["qmmQuantityBreak9"] = quoteMaterial.qmmQuantityBreak9;
			dataRow["qmmQuantityPerAssembly"] = quoteMaterial.qmmQuantityPerAssembly;
			dataRow["qmmRelatedQuoteOperationID"] = quoteMaterial.qmmRelatedQuoteOperationID;
			dataRow["qmmScrapPercent"] = quoteMaterial.qmmScrapPercent;
			dataRow["qmmScrapQuantity"] = quoteMaterial.qmmScrapQuantity;
			dataRow["qmmSourcePriceID"] = quoteMaterial.qmmSourcePriceID;
			dataRow["qmmSourceRfqID"] = quoteMaterial.qmmSourceRfqID;
			dataRow["qmmSupplierOrganizationID"] = quoteMaterial.qmmSupplierOrganizationID;
			dataRow["qmmUnitCost1"] = quoteMaterial.qmmUnitCost1;
			dataRow["qmmUnitCost2"] = quoteMaterial.qmmUnitCost2;
			dataRow["qmmUnitCost3"] = quoteMaterial.qmmUnitCost3;
			dataRow["qmmUnitCost4"] = quoteMaterial.qmmUnitCost4;
			dataRow["qmmUnitCost5"] = quoteMaterial.qmmUnitCost5;
			dataRow["qmmUnitCost6"] = quoteMaterial.qmmUnitCost6;
			dataRow["qmmUnitCost7"] = quoteMaterial.qmmUnitCost7;
			dataRow["qmmUnitCost8"] = quoteMaterial.qmmUnitCost8;
			dataRow["qmmUnitCost9"] = quoteMaterial.qmmUnitCost9;
			dataRow["qmmUnitOfMeasure"] = quoteMaterial.qmmUnitOfMeasure;
			if (quoteMaterial.CustomFields != null && quoteMaterial.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in quoteMaterial.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the QuoteMaterial [{quoteMaterial.qmmUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the QuoteMaterial [{quoteMaterial.qmmUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
