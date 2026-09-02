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

public class ERPJobMaterialRepository : APIBaseRepository, IERPJobMaterialRepository, IAPIBaseRepository, IDisposable
{
	public ERPJobMaterialRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesJobMaterialExist(Guid jobMaterialId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmmUniqueID|C", jobMaterialId);
		base.selectList.Add("jmmUniqueID");
		return Task.FromResult(GetAsObject("JobMaterials", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPJobMaterialInformationDto>> GetAllJobMaterials(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPJobMaterialInformationDto> collection = new List<ERPJobMaterialInformationDto>();
		InitializeParameterLists();
		string[] array = new string[73]
		{
			"jmmCalculatedUnitCost", "jmmCreatedBy", "jmmCreatedDate", "jmmDocuments", "jmmDueInDate", "jmmUniqueID", "jmmEstimatedQuantity", "jmmEstimatedUnitCost", "jmmBackflush", "jmmClosed",
			"jmmCostOverride", "jmmFirm", "jmmKitPart", "jmmPullAllFromStock", "jmmReceivedComplete", "jmmJobAssemblyID", "jmmJobID", "jmmLeadTime", "jmmLeadTime1", "jmmLeadTime2",
			"jmmLeadTime3", "jmmLeadTime4", "jmmLeadTime5", "jmmLeadTime6", "jmmLeadTime7", "jmmLeadTime8", "jmmLeadTime9", "jmmMinimumCharge", "jmmOrderByDate", "jmmPartBinID",
			"jmmPartID", "jmmPartLongDescriptionRtf", "jmmPartLongDescriptionText", "jmmPartRevisionID", "jmmPartShortDescription", "jmmPartWarehouseLocationID", "jmmPullFromStockQuantity", "jmmPurchaseLocationID", "jmmPurchaseOrderID", "jmmPurchaseToJobQuantity",
			"jmmQuantityAllocated", "jmmQuantityBreak1", "jmmQuantityBreak2", "jmmQuantityBreak3", "jmmQuantityBreak4", "jmmQuantityBreak5", "jmmQuantityBreak6", "jmmQuantityBreak7", "jmmQuantityBreak8", "jmmQuantityBreak9",
			"jmmQuantityPerAssembly", "jmmQuantityReceived", "jmmQuantityToInspect", "jmmQuantityToReturn", "jmmRelatedJobOperationID", "jmmRequiredDate", "jmmRfqID", "jmmRowVersion", "jmmScrapPercent", "jmmScrapQuantity",
			"jmmScrapQuantityReceived", "jmmJobMaterialID", "jmmSupplierOrganizationID", "jmmUnitCost1", "jmmUnitCost2", "jmmUnitCost3", "jmmUnitCost4", "jmmUnitCost5", "jmmUnitCost6", "jmmUnitCost7",
			"jmmUnitCost8", "jmmUnitCost9", "jmmUnitOfMeasure"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("JobMaterials");
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
		using (DataTable dataTable = GetAsDataTable("JobMaterials", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPJobMaterialInformationDto eRPJobMaterialInformationDto = new ERPJobMaterialInformationDto();
				eRPJobMaterialInformationDto.jmmCalculatedUnitCost = dataTable.Rows[i].Field<decimal>("jmmCalculatedUnitCost");
				eRPJobMaterialInformationDto.jmmCreatedBy = dataTable.Rows[i].Field<string>("jmmCreatedBy");
				eRPJobMaterialInformationDto.jmmCreatedDate = dataTable.Rows[i].Field<DateTime?>("jmmCreatedDate");
				eRPJobMaterialInformationDto.jmmDocuments = dataTable.Rows[i].Field<string>("jmmDocuments");
				eRPJobMaterialInformationDto.jmmDueInDate = dataTable.Rows[i].Field<DateTime?>("jmmDueInDate");
				eRPJobMaterialInformationDto.jmmUniqueID = dataTable.Rows[i].Field<Guid>("jmmUniqueID");
				eRPJobMaterialInformationDto.jmmEstimatedQuantity = dataTable.Rows[i].Field<decimal>("jmmEstimatedQuantity");
				eRPJobMaterialInformationDto.jmmEstimatedUnitCost = dataTable.Rows[i].Field<decimal>("jmmEstimatedUnitCost");
				eRPJobMaterialInformationDto.jmmBackflush = dataTable.Rows[i].Field<bool>("jmmBackflush");
				eRPJobMaterialInformationDto.jmmClosed = dataTable.Rows[i].Field<bool>("jmmClosed");
				eRPJobMaterialInformationDto.jmmCostOverride = dataTable.Rows[i].Field<bool>("jmmCostOverride");
				eRPJobMaterialInformationDto.jmmFirm = dataTable.Rows[i].Field<bool>("jmmFirm");
				eRPJobMaterialInformationDto.jmmKitPart = dataTable.Rows[i].Field<bool>("jmmKitPart");
				eRPJobMaterialInformationDto.jmmPullAllFromStock = dataTable.Rows[i].Field<bool>("jmmPullAllFromStock");
				eRPJobMaterialInformationDto.jmmReceivedComplete = dataTable.Rows[i].Field<bool>("jmmReceivedComplete");
				eRPJobMaterialInformationDto.jmmJobAssemblyID = dataTable.Rows[i].Field<int>("jmmJobAssemblyID");
				eRPJobMaterialInformationDto.jmmJobID = dataTable.Rows[i].Field<string>("jmmJobID");
				eRPJobMaterialInformationDto.jmmLeadTime = dataTable.Rows[i].Field<short>("jmmLeadTime");
				eRPJobMaterialInformationDto.jmmLeadTime1 = dataTable.Rows[i].Field<short>("jmmLeadTime1");
				eRPJobMaterialInformationDto.jmmLeadTime2 = dataTable.Rows[i].Field<short>("jmmLeadTime2");
				eRPJobMaterialInformationDto.jmmLeadTime3 = dataTable.Rows[i].Field<short>("jmmLeadTime3");
				eRPJobMaterialInformationDto.jmmLeadTime4 = dataTable.Rows[i].Field<short>("jmmLeadTime4");
				eRPJobMaterialInformationDto.jmmLeadTime5 = dataTable.Rows[i].Field<short>("jmmLeadTime5");
				eRPJobMaterialInformationDto.jmmLeadTime6 = dataTable.Rows[i].Field<short>("jmmLeadTime6");
				eRPJobMaterialInformationDto.jmmLeadTime7 = dataTable.Rows[i].Field<short>("jmmLeadTime7");
				eRPJobMaterialInformationDto.jmmLeadTime8 = dataTable.Rows[i].Field<short>("jmmLeadTime8");
				eRPJobMaterialInformationDto.jmmLeadTime9 = dataTable.Rows[i].Field<short>("jmmLeadTime9");
				eRPJobMaterialInformationDto.jmmMinimumCharge = dataTable.Rows[i].Field<decimal>("jmmMinimumCharge");
				eRPJobMaterialInformationDto.jmmOrderByDate = dataTable.Rows[i].Field<DateTime?>("jmmOrderByDate");
				eRPJobMaterialInformationDto.jmmPartBinID = dataTable.Rows[i].Field<string>("jmmPartBinID");
				eRPJobMaterialInformationDto.jmmPartID = dataTable.Rows[i].Field<string>("jmmPartID");
				eRPJobMaterialInformationDto.jmmPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("jmmPartLongDescriptionRtf");
				eRPJobMaterialInformationDto.jmmPartLongDescriptionText = dataTable.Rows[i].Field<string>("jmmPartLongDescriptionText");
				eRPJobMaterialInformationDto.jmmPartRevisionID = dataTable.Rows[i].Field<string>("jmmPartRevisionID");
				eRPJobMaterialInformationDto.jmmPartShortDescription = dataTable.Rows[i].Field<string>("jmmPartShortDescription");
				eRPJobMaterialInformationDto.jmmPartWarehouseLocationID = dataTable.Rows[i].Field<string>("jmmPartWarehouseLocationID");
				eRPJobMaterialInformationDto.jmmPullFromStockQuantity = dataTable.Rows[i].Field<decimal>("jmmPullFromStockQuantity");
				eRPJobMaterialInformationDto.jmmPurchaseLocationID = dataTable.Rows[i].Field<string>("jmmPurchaseLocationID");
				eRPJobMaterialInformationDto.jmmPurchaseOrderID = dataTable.Rows[i].Field<string>("jmmPurchaseOrderID");
				eRPJobMaterialInformationDto.jmmPurchaseToJobQuantity = dataTable.Rows[i].Field<decimal>("jmmPurchaseToJobQuantity");
				eRPJobMaterialInformationDto.jmmQuantityAllocated = dataTable.Rows[i].Field<decimal>("jmmQuantityAllocated");
				eRPJobMaterialInformationDto.jmmQuantityBreak1 = dataTable.Rows[i].Field<decimal>("jmmQuantityBreak1");
				eRPJobMaterialInformationDto.jmmQuantityBreak2 = dataTable.Rows[i].Field<decimal>("jmmQuantityBreak2");
				eRPJobMaterialInformationDto.jmmQuantityBreak3 = dataTable.Rows[i].Field<decimal>("jmmQuantityBreak3");
				eRPJobMaterialInformationDto.jmmQuantityBreak4 = dataTable.Rows[i].Field<decimal>("jmmQuantityBreak4");
				eRPJobMaterialInformationDto.jmmQuantityBreak5 = dataTable.Rows[i].Field<decimal>("jmmQuantityBreak5");
				eRPJobMaterialInformationDto.jmmQuantityBreak6 = dataTable.Rows[i].Field<decimal>("jmmQuantityBreak6");
				eRPJobMaterialInformationDto.jmmQuantityBreak7 = dataTable.Rows[i].Field<decimal>("jmmQuantityBreak7");
				eRPJobMaterialInformationDto.jmmQuantityBreak8 = dataTable.Rows[i].Field<decimal>("jmmQuantityBreak8");
				eRPJobMaterialInformationDto.jmmQuantityBreak9 = dataTable.Rows[i].Field<decimal>("jmmQuantityBreak9");
				eRPJobMaterialInformationDto.jmmQuantityPerAssembly = dataTable.Rows[i].Field<decimal>("jmmQuantityPerAssembly");
				eRPJobMaterialInformationDto.jmmQuantityReceived = dataTable.Rows[i].Field<decimal>("jmmQuantityReceived");
				eRPJobMaterialInformationDto.jmmQuantityToInspect = dataTable.Rows[i].Field<decimal>("jmmQuantityToInspect");
				eRPJobMaterialInformationDto.jmmQuantityToReturn = dataTable.Rows[i].Field<decimal>("jmmQuantityToReturn");
				eRPJobMaterialInformationDto.jmmRelatedJobOperationID = dataTable.Rows[i].Field<int>("jmmRelatedJobOperationID");
				eRPJobMaterialInformationDto.jmmRequiredDate = dataTable.Rows[i].Field<DateTime?>("jmmRequiredDate");
				eRPJobMaterialInformationDto.jmmRfqID = dataTable.Rows[i].Field<string>("jmmRfqID");
				eRPJobMaterialInformationDto.jmmRowVersion = dataTable.Rows[i].Field<byte[]>("jmmRowVersion");
				eRPJobMaterialInformationDto.jmmScrapPercent = dataTable.Rows[i].Field<decimal>("jmmScrapPercent");
				eRPJobMaterialInformationDto.jmmScrapQuantity = dataTable.Rows[i].Field<decimal>("jmmScrapQuantity");
				eRPJobMaterialInformationDto.jmmScrapQuantityReceived = dataTable.Rows[i].Field<decimal>("jmmScrapQuantityReceived");
				eRPJobMaterialInformationDto.jmmJobMaterialID = dataTable.Rows[i].Field<int>("jmmJobMaterialID");
				eRPJobMaterialInformationDto.jmmSupplierOrganizationID = dataTable.Rows[i].Field<string>("jmmSupplierOrganizationID");
				eRPJobMaterialInformationDto.jmmUnitCost1 = dataTable.Rows[i].Field<decimal>("jmmUnitCost1");
				eRPJobMaterialInformationDto.jmmUnitCost2 = dataTable.Rows[i].Field<decimal>("jmmUnitCost2");
				eRPJobMaterialInformationDto.jmmUnitCost3 = dataTable.Rows[i].Field<decimal>("jmmUnitCost3");
				eRPJobMaterialInformationDto.jmmUnitCost4 = dataTable.Rows[i].Field<decimal>("jmmUnitCost4");
				eRPJobMaterialInformationDto.jmmUnitCost5 = dataTable.Rows[i].Field<decimal>("jmmUnitCost5");
				eRPJobMaterialInformationDto.jmmUnitCost6 = dataTable.Rows[i].Field<decimal>("jmmUnitCost6");
				eRPJobMaterialInformationDto.jmmUnitCost7 = dataTable.Rows[i].Field<decimal>("jmmUnitCost7");
				eRPJobMaterialInformationDto.jmmUnitCost8 = dataTable.Rows[i].Field<decimal>("jmmUnitCost8");
				eRPJobMaterialInformationDto.jmmUnitCost9 = dataTable.Rows[i].Field<decimal>("jmmUnitCost9");
				eRPJobMaterialInformationDto.jmmUnitOfMeasure = dataTable.Rows[i].Field<string>("jmmUnitOfMeasure");
				eRPJobMaterialInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPJobMaterialInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPJobMaterialInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPJobMaterialInformationDto> GetJobMaterial(Guid jobMaterialId)
	{
		ERPJobMaterialInformationDto eRPJobMaterialInformationDto = new ERPJobMaterialInformationDto();
		InitializeParameterLists();
		string[] collection = new string[73]
		{
			"jmmCalculatedUnitCost", "jmmCreatedBy", "jmmCreatedDate", "jmmDocuments", "jmmDueInDate", "jmmUniqueID", "jmmEstimatedQuantity", "jmmEstimatedUnitCost", "jmmBackflush", "jmmClosed",
			"jmmCostOverride", "jmmFirm", "jmmKitPart", "jmmPullAllFromStock", "jmmReceivedComplete", "jmmJobAssemblyID", "jmmJobID", "jmmLeadTime", "jmmLeadTime1", "jmmLeadTime2",
			"jmmLeadTime3", "jmmLeadTime4", "jmmLeadTime5", "jmmLeadTime6", "jmmLeadTime7", "jmmLeadTime8", "jmmLeadTime9", "jmmMinimumCharge", "jmmOrderByDate", "jmmPartBinID",
			"jmmPartID", "jmmPartLongDescriptionRtf", "jmmPartLongDescriptionText", "jmmPartRevisionID", "jmmPartShortDescription", "jmmPartWarehouseLocationID", "jmmPullFromStockQuantity", "jmmPurchaseLocationID", "jmmPurchaseOrderID", "jmmPurchaseToJobQuantity",
			"jmmQuantityAllocated", "jmmQuantityBreak1", "jmmQuantityBreak2", "jmmQuantityBreak3", "jmmQuantityBreak4", "jmmQuantityBreak5", "jmmQuantityBreak6", "jmmQuantityBreak7", "jmmQuantityBreak8", "jmmQuantityBreak9",
			"jmmQuantityPerAssembly", "jmmQuantityReceived", "jmmQuantityToInspect", "jmmQuantityToReturn", "jmmRelatedJobOperationID", "jmmRequiredDate", "jmmRfqID", "jmmRowVersion", "jmmScrapPercent", "jmmScrapQuantity",
			"jmmScrapQuantityReceived", "jmmJobMaterialID", "jmmSupplierOrganizationID", "jmmUnitCost1", "jmmUnitCost2", "jmmUnitCost3", "jmmUnitCost4", "jmmUnitCost5", "jmmUnitCost6", "jmmUnitCost7",
			"jmmUnitCost8", "jmmUnitCost9", "jmmUnitOfMeasure"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("jmmUniqueID|C", jobMaterialId);
		AddCustomFieldsToSelectList("JobMaterials");
		using (DataTable dataTable = GetAsDataTable("JobMaterials", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPJobMaterialInformationDto);
			}
			eRPJobMaterialInformationDto.jmmCalculatedUnitCost = dataTable.Rows[0].Field<decimal>("jmmCalculatedUnitCost");
			eRPJobMaterialInformationDto.jmmCreatedBy = dataTable.Rows[0].Field<string>("jmmCreatedBy");
			eRPJobMaterialInformationDto.jmmCreatedDate = dataTable.Rows[0].Field<DateTime?>("jmmCreatedDate");
			eRPJobMaterialInformationDto.jmmDocuments = dataTable.Rows[0].Field<string>("jmmDocuments");
			eRPJobMaterialInformationDto.jmmDueInDate = dataTable.Rows[0].Field<DateTime?>("jmmDueInDate");
			eRPJobMaterialInformationDto.jmmUniqueID = dataTable.Rows[0].Field<Guid>("jmmUniqueID");
			eRPJobMaterialInformationDto.jmmEstimatedQuantity = dataTable.Rows[0].Field<decimal>("jmmEstimatedQuantity");
			eRPJobMaterialInformationDto.jmmEstimatedUnitCost = dataTable.Rows[0].Field<decimal>("jmmEstimatedUnitCost");
			eRPJobMaterialInformationDto.jmmBackflush = dataTable.Rows[0].Field<bool>("jmmBackflush");
			eRPJobMaterialInformationDto.jmmClosed = dataTable.Rows[0].Field<bool>("jmmClosed");
			eRPJobMaterialInformationDto.jmmCostOverride = dataTable.Rows[0].Field<bool>("jmmCostOverride");
			eRPJobMaterialInformationDto.jmmFirm = dataTable.Rows[0].Field<bool>("jmmFirm");
			eRPJobMaterialInformationDto.jmmKitPart = dataTable.Rows[0].Field<bool>("jmmKitPart");
			eRPJobMaterialInformationDto.jmmPullAllFromStock = dataTable.Rows[0].Field<bool>("jmmPullAllFromStock");
			eRPJobMaterialInformationDto.jmmReceivedComplete = dataTable.Rows[0].Field<bool>("jmmReceivedComplete");
			eRPJobMaterialInformationDto.jmmJobAssemblyID = dataTable.Rows[0].Field<int>("jmmJobAssemblyID");
			eRPJobMaterialInformationDto.jmmJobID = dataTable.Rows[0].Field<string>("jmmJobID");
			eRPJobMaterialInformationDto.jmmLeadTime = dataTable.Rows[0].Field<short>("jmmLeadTime");
			eRPJobMaterialInformationDto.jmmLeadTime1 = dataTable.Rows[0].Field<short>("jmmLeadTime1");
			eRPJobMaterialInformationDto.jmmLeadTime2 = dataTable.Rows[0].Field<short>("jmmLeadTime2");
			eRPJobMaterialInformationDto.jmmLeadTime3 = dataTable.Rows[0].Field<short>("jmmLeadTime3");
			eRPJobMaterialInformationDto.jmmLeadTime4 = dataTable.Rows[0].Field<short>("jmmLeadTime4");
			eRPJobMaterialInformationDto.jmmLeadTime5 = dataTable.Rows[0].Field<short>("jmmLeadTime5");
			eRPJobMaterialInformationDto.jmmLeadTime6 = dataTable.Rows[0].Field<short>("jmmLeadTime6");
			eRPJobMaterialInformationDto.jmmLeadTime7 = dataTable.Rows[0].Field<short>("jmmLeadTime7");
			eRPJobMaterialInformationDto.jmmLeadTime8 = dataTable.Rows[0].Field<short>("jmmLeadTime8");
			eRPJobMaterialInformationDto.jmmLeadTime9 = dataTable.Rows[0].Field<short>("jmmLeadTime9");
			eRPJobMaterialInformationDto.jmmMinimumCharge = dataTable.Rows[0].Field<decimal>("jmmMinimumCharge");
			eRPJobMaterialInformationDto.jmmOrderByDate = dataTable.Rows[0].Field<DateTime?>("jmmOrderByDate");
			eRPJobMaterialInformationDto.jmmPartBinID = dataTable.Rows[0].Field<string>("jmmPartBinID");
			eRPJobMaterialInformationDto.jmmPartID = dataTable.Rows[0].Field<string>("jmmPartID");
			eRPJobMaterialInformationDto.jmmPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("jmmPartLongDescriptionRtf");
			eRPJobMaterialInformationDto.jmmPartLongDescriptionText = dataTable.Rows[0].Field<string>("jmmPartLongDescriptionText");
			eRPJobMaterialInformationDto.jmmPartRevisionID = dataTable.Rows[0].Field<string>("jmmPartRevisionID");
			eRPJobMaterialInformationDto.jmmPartShortDescription = dataTable.Rows[0].Field<string>("jmmPartShortDescription");
			eRPJobMaterialInformationDto.jmmPartWarehouseLocationID = dataTable.Rows[0].Field<string>("jmmPartWarehouseLocationID");
			eRPJobMaterialInformationDto.jmmPullFromStockQuantity = dataTable.Rows[0].Field<decimal>("jmmPullFromStockQuantity");
			eRPJobMaterialInformationDto.jmmPurchaseLocationID = dataTable.Rows[0].Field<string>("jmmPurchaseLocationID");
			eRPJobMaterialInformationDto.jmmPurchaseOrderID = dataTable.Rows[0].Field<string>("jmmPurchaseOrderID");
			eRPJobMaterialInformationDto.jmmPurchaseToJobQuantity = dataTable.Rows[0].Field<decimal>("jmmPurchaseToJobQuantity");
			eRPJobMaterialInformationDto.jmmQuantityAllocated = dataTable.Rows[0].Field<decimal>("jmmQuantityAllocated");
			eRPJobMaterialInformationDto.jmmQuantityBreak1 = dataTable.Rows[0].Field<decimal>("jmmQuantityBreak1");
			eRPJobMaterialInformationDto.jmmQuantityBreak2 = dataTable.Rows[0].Field<decimal>("jmmQuantityBreak2");
			eRPJobMaterialInformationDto.jmmQuantityBreak3 = dataTable.Rows[0].Field<decimal>("jmmQuantityBreak3");
			eRPJobMaterialInformationDto.jmmQuantityBreak4 = dataTable.Rows[0].Field<decimal>("jmmQuantityBreak4");
			eRPJobMaterialInformationDto.jmmQuantityBreak5 = dataTable.Rows[0].Field<decimal>("jmmQuantityBreak5");
			eRPJobMaterialInformationDto.jmmQuantityBreak6 = dataTable.Rows[0].Field<decimal>("jmmQuantityBreak6");
			eRPJobMaterialInformationDto.jmmQuantityBreak7 = dataTable.Rows[0].Field<decimal>("jmmQuantityBreak7");
			eRPJobMaterialInformationDto.jmmQuantityBreak8 = dataTable.Rows[0].Field<decimal>("jmmQuantityBreak8");
			eRPJobMaterialInformationDto.jmmQuantityBreak9 = dataTable.Rows[0].Field<decimal>("jmmQuantityBreak9");
			eRPJobMaterialInformationDto.jmmQuantityPerAssembly = dataTable.Rows[0].Field<decimal>("jmmQuantityPerAssembly");
			eRPJobMaterialInformationDto.jmmQuantityReceived = dataTable.Rows[0].Field<decimal>("jmmQuantityReceived");
			eRPJobMaterialInformationDto.jmmQuantityToInspect = dataTable.Rows[0].Field<decimal>("jmmQuantityToInspect");
			eRPJobMaterialInformationDto.jmmQuantityToReturn = dataTable.Rows[0].Field<decimal>("jmmQuantityToReturn");
			eRPJobMaterialInformationDto.jmmRelatedJobOperationID = dataTable.Rows[0].Field<int>("jmmRelatedJobOperationID");
			eRPJobMaterialInformationDto.jmmRequiredDate = dataTable.Rows[0].Field<DateTime?>("jmmRequiredDate");
			eRPJobMaterialInformationDto.jmmRfqID = dataTable.Rows[0].Field<string>("jmmRfqID");
			eRPJobMaterialInformationDto.jmmRowVersion = dataTable.Rows[0].Field<byte[]>("jmmRowVersion");
			eRPJobMaterialInformationDto.jmmScrapPercent = dataTable.Rows[0].Field<decimal>("jmmScrapPercent");
			eRPJobMaterialInformationDto.jmmScrapQuantity = dataTable.Rows[0].Field<decimal>("jmmScrapQuantity");
			eRPJobMaterialInformationDto.jmmScrapQuantityReceived = dataTable.Rows[0].Field<decimal>("jmmScrapQuantityReceived");
			eRPJobMaterialInformationDto.jmmJobMaterialID = dataTable.Rows[0].Field<int>("jmmJobMaterialID");
			eRPJobMaterialInformationDto.jmmSupplierOrganizationID = dataTable.Rows[0].Field<string>("jmmSupplierOrganizationID");
			eRPJobMaterialInformationDto.jmmUnitCost1 = dataTable.Rows[0].Field<decimal>("jmmUnitCost1");
			eRPJobMaterialInformationDto.jmmUnitCost2 = dataTable.Rows[0].Field<decimal>("jmmUnitCost2");
			eRPJobMaterialInformationDto.jmmUnitCost3 = dataTable.Rows[0].Field<decimal>("jmmUnitCost3");
			eRPJobMaterialInformationDto.jmmUnitCost4 = dataTable.Rows[0].Field<decimal>("jmmUnitCost4");
			eRPJobMaterialInformationDto.jmmUnitCost5 = dataTable.Rows[0].Field<decimal>("jmmUnitCost5");
			eRPJobMaterialInformationDto.jmmUnitCost6 = dataTable.Rows[0].Field<decimal>("jmmUnitCost6");
			eRPJobMaterialInformationDto.jmmUnitCost7 = dataTable.Rows[0].Field<decimal>("jmmUnitCost7");
			eRPJobMaterialInformationDto.jmmUnitCost8 = dataTable.Rows[0].Field<decimal>("jmmUnitCost8");
			eRPJobMaterialInformationDto.jmmUnitCost9 = dataTable.Rows[0].Field<decimal>("jmmUnitCost9");
			eRPJobMaterialInformationDto.jmmUnitOfMeasure = dataTable.Rows[0].Field<string>("jmmUnitOfMeasure");
			eRPJobMaterialInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPJobMaterialInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPJobMaterialInformationDto);
	}

	public Task<APIValidationInfoDto> SaveJobMaterial(ERPJobMaterialDto jobMaterial)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM JobMaterials WHERE jmmUniqueID = " + M1Util.ConvertToLinq(jobMaterial.jmmUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["jmmJobID"] = jobMaterial.jmmJobID.ToUpper();
				dataRow["jmmJobAssemblyID"] = jobMaterial.jmmJobAssemblyID;
				dataRow["jmmJobMaterialID"] = jobMaterial.jmmJobMaterialID;
				jobMaterial.jmmUniqueID = ((jobMaterial.jmmUniqueID == Guid.Empty) ? Guid.NewGuid() : jobMaterial.jmmUniqueID);
				dataRow["jmmUniqueID"] = jobMaterial.jmmUniqueID;
				dataRow["jmmCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["jmmCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The JobMaterial could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (jobMaterial.jmmRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the JobMaterial is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["jmmRowVersion"], jobMaterial.jmmRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the JobMaterial has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the JobMaterial again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["jmmCalculatedUnitCost"] = jobMaterial.jmmCalculatedUnitCost;
			dataRow["jmmDocuments"] = jobMaterial.jmmDocuments ?? dataRow["jmmDocuments"];
			DataRow dataRow2 = dataRow;
			DateTime? jmmDueInDate = jobMaterial.jmmDueInDate;
			dataRow2["jmmDueInDate"] = (jmmDueInDate.HasValue ? ((object)jmmDueInDate.GetValueOrDefault()) : dataRow["jmmDueInDate"]);
			dataRow["jmmEstimatedQuantity"] = jobMaterial.jmmEstimatedQuantity;
			dataRow["jmmEstimatedUnitCost"] = jobMaterial.jmmEstimatedUnitCost;
			dataRow["jmmBackflush"] = jobMaterial.jmmBackflush;
			dataRow["jmmClosed"] = jobMaterial.jmmClosed;
			dataRow["jmmCostOverride"] = jobMaterial.jmmCostOverride;
			dataRow["jmmFirm"] = jobMaterial.jmmFirm;
			dataRow["jmmKitPart"] = jobMaterial.jmmKitPart;
			dataRow["jmmPullAllFromStock"] = jobMaterial.jmmPullAllFromStock;
			dataRow["jmmReceivedComplete"] = jobMaterial.jmmReceivedComplete;
			dataRow["jmmLeadTime"] = jobMaterial.jmmLeadTime;
			dataRow["jmmLeadTime1"] = jobMaterial.jmmLeadTime1;
			dataRow["jmmLeadTime2"] = jobMaterial.jmmLeadTime2;
			dataRow["jmmLeadTime3"] = jobMaterial.jmmLeadTime3;
			dataRow["jmmLeadTime4"] = jobMaterial.jmmLeadTime4;
			dataRow["jmmLeadTime5"] = jobMaterial.jmmLeadTime5;
			dataRow["jmmLeadTime6"] = jobMaterial.jmmLeadTime6;
			dataRow["jmmLeadTime7"] = jobMaterial.jmmLeadTime7;
			dataRow["jmmLeadTime8"] = jobMaterial.jmmLeadTime8;
			dataRow["jmmLeadTime9"] = jobMaterial.jmmLeadTime9;
			dataRow["jmmMinimumCharge"] = jobMaterial.jmmMinimumCharge;
			DataRow dataRow3 = dataRow;
			jmmDueInDate = jobMaterial.jmmOrderByDate;
			dataRow3["jmmOrderByDate"] = (jmmDueInDate.HasValue ? ((object)jmmDueInDate.GetValueOrDefault()) : dataRow["jmmOrderByDate"]);
			dataRow["jmmPartBinID"] = jobMaterial.jmmPartBinID;
			dataRow["jmmPartID"] = jobMaterial.jmmPartID;
			dataRow["jmmPartLongDescriptionRtf"] = jobMaterial.jmmPartLongDescriptionRtf ?? dataRow["jmmPartLongDescriptionRtf"];
			dataRow["jmmPartLongDescriptionText"] = jobMaterial.jmmPartLongDescriptionText ?? dataRow["jmmPartLongDescriptionText"];
			dataRow["jmmPartRevisionID"] = jobMaterial.jmmPartRevisionID;
			dataRow["jmmPartShortDescription"] = jobMaterial.jmmPartShortDescription;
			dataRow["jmmPartWarehouseLocationID"] = jobMaterial.jmmPartWarehouseLocationID;
			dataRow["jmmPullFromStockQuantity"] = jobMaterial.jmmPullFromStockQuantity;
			dataRow["jmmPurchaseLocationID"] = jobMaterial.jmmPurchaseLocationID;
			dataRow["jmmPurchaseOrderID"] = jobMaterial.jmmPurchaseOrderID;
			dataRow["jmmPurchaseToJobQuantity"] = jobMaterial.jmmPurchaseToJobQuantity;
			dataRow["jmmQuantityAllocated"] = jobMaterial.jmmQuantityAllocated;
			dataRow["jmmQuantityBreak1"] = jobMaterial.jmmQuantityBreak1;
			dataRow["jmmQuantityBreak2"] = jobMaterial.jmmQuantityBreak2;
			dataRow["jmmQuantityBreak3"] = jobMaterial.jmmQuantityBreak3;
			dataRow["jmmQuantityBreak4"] = jobMaterial.jmmQuantityBreak4;
			dataRow["jmmQuantityBreak5"] = jobMaterial.jmmQuantityBreak5;
			dataRow["jmmQuantityBreak6"] = jobMaterial.jmmQuantityBreak6;
			dataRow["jmmQuantityBreak7"] = jobMaterial.jmmQuantityBreak7;
			dataRow["jmmQuantityBreak8"] = jobMaterial.jmmQuantityBreak8;
			dataRow["jmmQuantityBreak9"] = jobMaterial.jmmQuantityBreak9;
			dataRow["jmmQuantityPerAssembly"] = jobMaterial.jmmQuantityPerAssembly;
			dataRow["jmmQuantityReceived"] = jobMaterial.jmmQuantityReceived;
			dataRow["jmmQuantityToInspect"] = jobMaterial.jmmQuantityToInspect;
			dataRow["jmmQuantityToReturn"] = jobMaterial.jmmQuantityToReturn;
			dataRow["jmmRelatedJobOperationID"] = jobMaterial.jmmRelatedJobOperationID;
			DataRow dataRow4 = dataRow;
			jmmDueInDate = jobMaterial.jmmRequiredDate;
			dataRow4["jmmRequiredDate"] = (jmmDueInDate.HasValue ? ((object)jmmDueInDate.GetValueOrDefault()) : dataRow["jmmRequiredDate"]);
			dataRow["jmmRfqID"] = jobMaterial.jmmRfqID;
			dataRow["jmmScrapPercent"] = jobMaterial.jmmScrapPercent;
			dataRow["jmmScrapQuantity"] = jobMaterial.jmmScrapQuantity;
			dataRow["jmmScrapQuantityReceived"] = jobMaterial.jmmScrapQuantityReceived;
			dataRow["jmmSupplierOrganizationID"] = jobMaterial.jmmSupplierOrganizationID;
			dataRow["jmmUnitCost1"] = jobMaterial.jmmUnitCost1;
			dataRow["jmmUnitCost2"] = jobMaterial.jmmUnitCost2;
			dataRow["jmmUnitCost3"] = jobMaterial.jmmUnitCost3;
			dataRow["jmmUnitCost4"] = jobMaterial.jmmUnitCost4;
			dataRow["jmmUnitCost5"] = jobMaterial.jmmUnitCost5;
			dataRow["jmmUnitCost6"] = jobMaterial.jmmUnitCost6;
			dataRow["jmmUnitCost7"] = jobMaterial.jmmUnitCost7;
			dataRow["jmmUnitCost8"] = jobMaterial.jmmUnitCost8;
			dataRow["jmmUnitCost9"] = jobMaterial.jmmUnitCost9;
			dataRow["jmmUnitOfMeasure"] = jobMaterial.jmmUnitOfMeasure;
			if (jobMaterial.CustomFields != null && jobMaterial.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in jobMaterial.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the JobMaterial [{jobMaterial.jmmUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the JobMaterial [{jobMaterial.jmmUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
