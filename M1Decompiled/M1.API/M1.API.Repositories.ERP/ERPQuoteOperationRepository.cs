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

public class ERPQuoteOperationRepository : APIBaseRepository, IERPQuoteOperationRepository, IAPIBaseRepository, IDisposable
{
	public ERPQuoteOperationRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesQuoteOperationExist(Guid quoteOperationId)
	{
		InitializeParameterLists();
		base.filterList.Add("qmoUniqueID|C", quoteOperationId);
		base.selectList.Add("qmoUniqueID");
		return Task.FromResult(GetAsObject("QuoteOperations", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPQuoteOperationInformationDto>> GetAllQuoteOperations(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPQuoteOperationInformationDto> collection = new List<ERPQuoteOperationInformationDto>();
		InitializeParameterLists();
		string[] array = new string[67]
		{
			"qmoAdditionalSetupHours", "qmoAdditionalSetupQuantity", "qmoCreatedBy", "qmoCreatedDate", "qmoDocuments", "qmoUniqueID", "qmoEstimatedUnitCost", "qmoInspectionType", "qmoClosed", "qmoMachinesToSchedule",
			"qmoMachineType", "qmoMinimumCharge", "qmoMoveTime", "qmoOperationType", "qmoOverheadRate", "qmoOverlap", "qmoOverlapDestinationLink", "qmoOverlapOffsetTime", "qmoOverlapOperationID", "qmoOverlapSourceLink",
			"qmoPartID", "qmoPartRevisionID", "qmoPlantDepartmentID", "qmoPlantID", "qmoProcessID", "qmoProcessLongDescriptionRtf", "qmoProcessLongDescriptionText", "qmoProcessShortDescription", "qmoProductionRate", "qmoProductionStandard",
			"qmoPurchaseLocationID", "qmoQuantityBreak1", "qmoQuantityBreak2", "qmoQuantityBreak3", "qmoQuantityBreak4", "qmoQuantityBreak5", "qmoQuantityBreak6", "qmoQuantityBreak7", "qmoQuantityBreak8", "qmoQuantityBreak9",
			"qmoQuantityPerAssembly", "qmoQueueTime", "qmoQuoteAssemblyID", "qmoQuoteID", "qmoQuoteLineID", "qmoQuotingRate", "qmoRowVersion", "qmoQuoteOperationID", "qmoSetupCharge", "qmoSetupHours",
			"qmoSetupRate", "qmoSfeMessageRTF", "qmoSfeMessageText", "qmoStandardFactor", "qmoSupplierOrganizationID", "qmoUnitCost1", "qmoUnitCost2", "qmoUnitCost3", "qmoUnitCost4", "qmoUnitCost5",
			"qmoUnitCost6", "qmoUnitCost7", "qmoUnitCost8", "qmoUnitCost9", "qmoUnitOfMeasure", "qmoWorkCenterID", "qmoWorkCenterMachineID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("QuoteOperations");
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
		using (DataTable dataTable = GetAsDataTable("QuoteOperations", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPQuoteOperationInformationDto eRPQuoteOperationInformationDto = new ERPQuoteOperationInformationDto();
				eRPQuoteOperationInformationDto.qmoAdditionalSetupHours = dataTable.Rows[i].Field<decimal>("qmoAdditionalSetupHours");
				eRPQuoteOperationInformationDto.qmoAdditionalSetupQuantity = dataTable.Rows[i].Field<decimal>("qmoAdditionalSetupQuantity");
				eRPQuoteOperationInformationDto.qmoCreatedBy = dataTable.Rows[i].Field<string>("qmoCreatedBy");
				eRPQuoteOperationInformationDto.qmoCreatedDate = dataTable.Rows[i].Field<DateTime?>("qmoCreatedDate");
				eRPQuoteOperationInformationDto.qmoDocuments = dataTable.Rows[i].Field<string>("qmoDocuments");
				eRPQuoteOperationInformationDto.qmoUniqueID = dataTable.Rows[i].Field<Guid>("qmoUniqueID");
				eRPQuoteOperationInformationDto.qmoEstimatedUnitCost = dataTable.Rows[i].Field<decimal>("qmoEstimatedUnitCost");
				eRPQuoteOperationInformationDto.qmoInspectionType = dataTable.Rows[i].Field<byte>("qmoInspectionType");
				eRPQuoteOperationInformationDto.qmoClosed = dataTable.Rows[i].Field<bool>("qmoClosed");
				eRPQuoteOperationInformationDto.qmoMachinesToSchedule = dataTable.Rows[i].Field<short>("qmoMachinesToSchedule");
				eRPQuoteOperationInformationDto.qmoMachineType = dataTable.Rows[i].Field<byte>("qmoMachineType");
				eRPQuoteOperationInformationDto.qmoMinimumCharge = dataTable.Rows[i].Field<decimal>("qmoMinimumCharge");
				eRPQuoteOperationInformationDto.qmoMoveTime = dataTable.Rows[i].Field<decimal>("qmoMoveTime");
				eRPQuoteOperationInformationDto.qmoOperationType = dataTable.Rows[i].Field<byte>("qmoOperationType");
				eRPQuoteOperationInformationDto.qmoOverheadRate = dataTable.Rows[i].Field<decimal>("qmoOverheadRate");
				eRPQuoteOperationInformationDto.qmoOverlap = dataTable.Rows[i].Field<byte>("qmoOverlap");
				eRPQuoteOperationInformationDto.qmoOverlapDestinationLink = dataTable.Rows[i].Field<byte>("qmoOverlapDestinationLink");
				eRPQuoteOperationInformationDto.qmoOverlapOffsetTime = dataTable.Rows[i].Field<decimal>("qmoOverlapOffsetTime");
				eRPQuoteOperationInformationDto.qmoOverlapOperationID = dataTable.Rows[i].Field<int>("qmoOverlapOperationID");
				eRPQuoteOperationInformationDto.qmoOverlapSourceLink = dataTable.Rows[i].Field<byte>("qmoOverlapSourceLink");
				eRPQuoteOperationInformationDto.qmoPartID = dataTable.Rows[i].Field<string>("qmoPartID");
				eRPQuoteOperationInformationDto.qmoPartRevisionID = dataTable.Rows[i].Field<string>("qmoPartRevisionID");
				eRPQuoteOperationInformationDto.qmoPlantDepartmentID = dataTable.Rows[i].Field<string>("qmoPlantDepartmentID");
				eRPQuoteOperationInformationDto.qmoPlantID = dataTable.Rows[i].Field<string>("qmoPlantID");
				eRPQuoteOperationInformationDto.qmoProcessID = dataTable.Rows[i].Field<string>("qmoProcessID");
				eRPQuoteOperationInformationDto.qmoProcessLongDescriptionRtf = dataTable.Rows[i].Field<string>("qmoProcessLongDescriptionRtf");
				eRPQuoteOperationInformationDto.qmoProcessLongDescriptionText = dataTable.Rows[i].Field<string>("qmoProcessLongDescriptionText");
				eRPQuoteOperationInformationDto.qmoProcessShortDescription = dataTable.Rows[i].Field<string>("qmoProcessShortDescription");
				eRPQuoteOperationInformationDto.qmoProductionRate = dataTable.Rows[i].Field<decimal>("qmoProductionRate");
				eRPQuoteOperationInformationDto.qmoProductionStandard = dataTable.Rows[i].Field<decimal>("qmoProductionStandard");
				eRPQuoteOperationInformationDto.qmoPurchaseLocationID = dataTable.Rows[i].Field<string>("qmoPurchaseLocationID");
				eRPQuoteOperationInformationDto.qmoQuantityBreak1 = dataTable.Rows[i].Field<decimal>("qmoQuantityBreak1");
				eRPQuoteOperationInformationDto.qmoQuantityBreak2 = dataTable.Rows[i].Field<decimal>("qmoQuantityBreak2");
				eRPQuoteOperationInformationDto.qmoQuantityBreak3 = dataTable.Rows[i].Field<decimal>("qmoQuantityBreak3");
				eRPQuoteOperationInformationDto.qmoQuantityBreak4 = dataTable.Rows[i].Field<decimal>("qmoQuantityBreak4");
				eRPQuoteOperationInformationDto.qmoQuantityBreak5 = dataTable.Rows[i].Field<decimal>("qmoQuantityBreak5");
				eRPQuoteOperationInformationDto.qmoQuantityBreak6 = dataTable.Rows[i].Field<decimal>("qmoQuantityBreak6");
				eRPQuoteOperationInformationDto.qmoQuantityBreak7 = dataTable.Rows[i].Field<decimal>("qmoQuantityBreak7");
				eRPQuoteOperationInformationDto.qmoQuantityBreak8 = dataTable.Rows[i].Field<decimal>("qmoQuantityBreak8");
				eRPQuoteOperationInformationDto.qmoQuantityBreak9 = dataTable.Rows[i].Field<decimal>("qmoQuantityBreak9");
				eRPQuoteOperationInformationDto.qmoQuantityPerAssembly = dataTable.Rows[i].Field<decimal>("qmoQuantityPerAssembly");
				eRPQuoteOperationInformationDto.qmoQueueTime = dataTable.Rows[i].Field<decimal>("qmoQueueTime");
				eRPQuoteOperationInformationDto.qmoQuoteAssemblyID = dataTable.Rows[i].Field<int>("qmoQuoteAssemblyID");
				eRPQuoteOperationInformationDto.qmoQuoteID = dataTable.Rows[i].Field<string>("qmoQuoteID");
				eRPQuoteOperationInformationDto.qmoQuoteLineID = dataTable.Rows[i].Field<short>("qmoQuoteLineID");
				eRPQuoteOperationInformationDto.qmoQuotingRate = dataTable.Rows[i].Field<decimal>("qmoQuotingRate");
				eRPQuoteOperationInformationDto.qmoRowVersion = dataTable.Rows[i].Field<byte[]>("qmoRowVersion");
				eRPQuoteOperationInformationDto.qmoQuoteOperationID = dataTable.Rows[i].Field<int>("qmoQuoteOperationID");
				eRPQuoteOperationInformationDto.qmoSetupCharge = dataTable.Rows[i].Field<decimal>("qmoSetupCharge");
				eRPQuoteOperationInformationDto.qmoSetupHours = dataTable.Rows[i].Field<decimal>("qmoSetupHours");
				eRPQuoteOperationInformationDto.qmoSetupRate = dataTable.Rows[i].Field<decimal>("qmoSetupRate");
				eRPQuoteOperationInformationDto.qmoSfeMessageRTF = dataTable.Rows[i].Field<string>("qmoSfeMessageRTF");
				eRPQuoteOperationInformationDto.qmoSfeMessageText = dataTable.Rows[i].Field<string>("qmoSfeMessageText");
				eRPQuoteOperationInformationDto.qmoStandardFactor = dataTable.Rows[i].Field<string>("qmoStandardFactor");
				eRPQuoteOperationInformationDto.qmoSupplierOrganizationID = dataTable.Rows[i].Field<string>("qmoSupplierOrganizationID");
				eRPQuoteOperationInformationDto.qmoUnitCost1 = dataTable.Rows[i].Field<decimal>("qmoUnitCost1");
				eRPQuoteOperationInformationDto.qmoUnitCost2 = dataTable.Rows[i].Field<decimal>("qmoUnitCost2");
				eRPQuoteOperationInformationDto.qmoUnitCost3 = dataTable.Rows[i].Field<decimal>("qmoUnitCost3");
				eRPQuoteOperationInformationDto.qmoUnitCost4 = dataTable.Rows[i].Field<decimal>("qmoUnitCost4");
				eRPQuoteOperationInformationDto.qmoUnitCost5 = dataTable.Rows[i].Field<decimal>("qmoUnitCost5");
				eRPQuoteOperationInformationDto.qmoUnitCost6 = dataTable.Rows[i].Field<decimal>("qmoUnitCost6");
				eRPQuoteOperationInformationDto.qmoUnitCost7 = dataTable.Rows[i].Field<decimal>("qmoUnitCost7");
				eRPQuoteOperationInformationDto.qmoUnitCost8 = dataTable.Rows[i].Field<decimal>("qmoUnitCost8");
				eRPQuoteOperationInformationDto.qmoUnitCost9 = dataTable.Rows[i].Field<decimal>("qmoUnitCost9");
				eRPQuoteOperationInformationDto.qmoUnitOfMeasure = dataTable.Rows[i].Field<string>("qmoUnitOfMeasure");
				eRPQuoteOperationInformationDto.qmoWorkCenterID = dataTable.Rows[i].Field<string>("qmoWorkCenterID");
				eRPQuoteOperationInformationDto.qmoWorkCenterMachineID = dataTable.Rows[i].Field<short>("qmoWorkCenterMachineID");
				eRPQuoteOperationInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPQuoteOperationInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPQuoteOperationInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPQuoteOperationInformationDto> GetQuoteOperation(Guid quoteOperationId)
	{
		ERPQuoteOperationInformationDto eRPQuoteOperationInformationDto = new ERPQuoteOperationInformationDto();
		InitializeParameterLists();
		string[] collection = new string[67]
		{
			"qmoAdditionalSetupHours", "qmoAdditionalSetupQuantity", "qmoCreatedBy", "qmoCreatedDate", "qmoDocuments", "qmoUniqueID", "qmoEstimatedUnitCost", "qmoInspectionType", "qmoClosed", "qmoMachinesToSchedule",
			"qmoMachineType", "qmoMinimumCharge", "qmoMoveTime", "qmoOperationType", "qmoOverheadRate", "qmoOverlap", "qmoOverlapDestinationLink", "qmoOverlapOffsetTime", "qmoOverlapOperationID", "qmoOverlapSourceLink",
			"qmoPartID", "qmoPartRevisionID", "qmoPlantDepartmentID", "qmoPlantID", "qmoProcessID", "qmoProcessLongDescriptionRtf", "qmoProcessLongDescriptionText", "qmoProcessShortDescription", "qmoProductionRate", "qmoProductionStandard",
			"qmoPurchaseLocationID", "qmoQuantityBreak1", "qmoQuantityBreak2", "qmoQuantityBreak3", "qmoQuantityBreak4", "qmoQuantityBreak5", "qmoQuantityBreak6", "qmoQuantityBreak7", "qmoQuantityBreak8", "qmoQuantityBreak9",
			"qmoQuantityPerAssembly", "qmoQueueTime", "qmoQuoteAssemblyID", "qmoQuoteID", "qmoQuoteLineID", "qmoQuotingRate", "qmoRowVersion", "qmoQuoteOperationID", "qmoSetupCharge", "qmoSetupHours",
			"qmoSetupRate", "qmoSfeMessageRTF", "qmoSfeMessageText", "qmoStandardFactor", "qmoSupplierOrganizationID", "qmoUnitCost1", "qmoUnitCost2", "qmoUnitCost3", "qmoUnitCost4", "qmoUnitCost5",
			"qmoUnitCost6", "qmoUnitCost7", "qmoUnitCost8", "qmoUnitCost9", "qmoUnitOfMeasure", "qmoWorkCenterID", "qmoWorkCenterMachineID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("qmoUniqueID|C", quoteOperationId);
		AddCustomFieldsToSelectList("QuoteOperations");
		using (DataTable dataTable = GetAsDataTable("QuoteOperations", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPQuoteOperationInformationDto);
			}
			eRPQuoteOperationInformationDto.qmoAdditionalSetupHours = dataTable.Rows[0].Field<decimal>("qmoAdditionalSetupHours");
			eRPQuoteOperationInformationDto.qmoAdditionalSetupQuantity = dataTable.Rows[0].Field<decimal>("qmoAdditionalSetupQuantity");
			eRPQuoteOperationInformationDto.qmoCreatedBy = dataTable.Rows[0].Field<string>("qmoCreatedBy");
			eRPQuoteOperationInformationDto.qmoCreatedDate = dataTable.Rows[0].Field<DateTime?>("qmoCreatedDate");
			eRPQuoteOperationInformationDto.qmoDocuments = dataTable.Rows[0].Field<string>("qmoDocuments");
			eRPQuoteOperationInformationDto.qmoUniqueID = dataTable.Rows[0].Field<Guid>("qmoUniqueID");
			eRPQuoteOperationInformationDto.qmoEstimatedUnitCost = dataTable.Rows[0].Field<decimal>("qmoEstimatedUnitCost");
			eRPQuoteOperationInformationDto.qmoInspectionType = dataTable.Rows[0].Field<byte>("qmoInspectionType");
			eRPQuoteOperationInformationDto.qmoClosed = dataTable.Rows[0].Field<bool>("qmoClosed");
			eRPQuoteOperationInformationDto.qmoMachinesToSchedule = dataTable.Rows[0].Field<short>("qmoMachinesToSchedule");
			eRPQuoteOperationInformationDto.qmoMachineType = dataTable.Rows[0].Field<byte>("qmoMachineType");
			eRPQuoteOperationInformationDto.qmoMinimumCharge = dataTable.Rows[0].Field<decimal>("qmoMinimumCharge");
			eRPQuoteOperationInformationDto.qmoMoveTime = dataTable.Rows[0].Field<decimal>("qmoMoveTime");
			eRPQuoteOperationInformationDto.qmoOperationType = dataTable.Rows[0].Field<byte>("qmoOperationType");
			eRPQuoteOperationInformationDto.qmoOverheadRate = dataTable.Rows[0].Field<decimal>("qmoOverheadRate");
			eRPQuoteOperationInformationDto.qmoOverlap = dataTable.Rows[0].Field<byte>("qmoOverlap");
			eRPQuoteOperationInformationDto.qmoOverlapDestinationLink = dataTable.Rows[0].Field<byte>("qmoOverlapDestinationLink");
			eRPQuoteOperationInformationDto.qmoOverlapOffsetTime = dataTable.Rows[0].Field<decimal>("qmoOverlapOffsetTime");
			eRPQuoteOperationInformationDto.qmoOverlapOperationID = dataTable.Rows[0].Field<int>("qmoOverlapOperationID");
			eRPQuoteOperationInformationDto.qmoOverlapSourceLink = dataTable.Rows[0].Field<byte>("qmoOverlapSourceLink");
			eRPQuoteOperationInformationDto.qmoPartID = dataTable.Rows[0].Field<string>("qmoPartID");
			eRPQuoteOperationInformationDto.qmoPartRevisionID = dataTable.Rows[0].Field<string>("qmoPartRevisionID");
			eRPQuoteOperationInformationDto.qmoPlantDepartmentID = dataTable.Rows[0].Field<string>("qmoPlantDepartmentID");
			eRPQuoteOperationInformationDto.qmoPlantID = dataTable.Rows[0].Field<string>("qmoPlantID");
			eRPQuoteOperationInformationDto.qmoProcessID = dataTable.Rows[0].Field<string>("qmoProcessID");
			eRPQuoteOperationInformationDto.qmoProcessLongDescriptionRtf = dataTable.Rows[0].Field<string>("qmoProcessLongDescriptionRtf");
			eRPQuoteOperationInformationDto.qmoProcessLongDescriptionText = dataTable.Rows[0].Field<string>("qmoProcessLongDescriptionText");
			eRPQuoteOperationInformationDto.qmoProcessShortDescription = dataTable.Rows[0].Field<string>("qmoProcessShortDescription");
			eRPQuoteOperationInformationDto.qmoProductionRate = dataTable.Rows[0].Field<decimal>("qmoProductionRate");
			eRPQuoteOperationInformationDto.qmoProductionStandard = dataTable.Rows[0].Field<decimal>("qmoProductionStandard");
			eRPQuoteOperationInformationDto.qmoPurchaseLocationID = dataTable.Rows[0].Field<string>("qmoPurchaseLocationID");
			eRPQuoteOperationInformationDto.qmoQuantityBreak1 = dataTable.Rows[0].Field<decimal>("qmoQuantityBreak1");
			eRPQuoteOperationInformationDto.qmoQuantityBreak2 = dataTable.Rows[0].Field<decimal>("qmoQuantityBreak2");
			eRPQuoteOperationInformationDto.qmoQuantityBreak3 = dataTable.Rows[0].Field<decimal>("qmoQuantityBreak3");
			eRPQuoteOperationInformationDto.qmoQuantityBreak4 = dataTable.Rows[0].Field<decimal>("qmoQuantityBreak4");
			eRPQuoteOperationInformationDto.qmoQuantityBreak5 = dataTable.Rows[0].Field<decimal>("qmoQuantityBreak5");
			eRPQuoteOperationInformationDto.qmoQuantityBreak6 = dataTable.Rows[0].Field<decimal>("qmoQuantityBreak6");
			eRPQuoteOperationInformationDto.qmoQuantityBreak7 = dataTable.Rows[0].Field<decimal>("qmoQuantityBreak7");
			eRPQuoteOperationInformationDto.qmoQuantityBreak8 = dataTable.Rows[0].Field<decimal>("qmoQuantityBreak8");
			eRPQuoteOperationInformationDto.qmoQuantityBreak9 = dataTable.Rows[0].Field<decimal>("qmoQuantityBreak9");
			eRPQuoteOperationInformationDto.qmoQuantityPerAssembly = dataTable.Rows[0].Field<decimal>("qmoQuantityPerAssembly");
			eRPQuoteOperationInformationDto.qmoQueueTime = dataTable.Rows[0].Field<decimal>("qmoQueueTime");
			eRPQuoteOperationInformationDto.qmoQuoteAssemblyID = dataTable.Rows[0].Field<int>("qmoQuoteAssemblyID");
			eRPQuoteOperationInformationDto.qmoQuoteID = dataTable.Rows[0].Field<string>("qmoQuoteID");
			eRPQuoteOperationInformationDto.qmoQuoteLineID = dataTable.Rows[0].Field<short>("qmoQuoteLineID");
			eRPQuoteOperationInformationDto.qmoQuotingRate = dataTable.Rows[0].Field<decimal>("qmoQuotingRate");
			eRPQuoteOperationInformationDto.qmoRowVersion = dataTable.Rows[0].Field<byte[]>("qmoRowVersion");
			eRPQuoteOperationInformationDto.qmoQuoteOperationID = dataTable.Rows[0].Field<int>("qmoQuoteOperationID");
			eRPQuoteOperationInformationDto.qmoSetupCharge = dataTable.Rows[0].Field<decimal>("qmoSetupCharge");
			eRPQuoteOperationInformationDto.qmoSetupHours = dataTable.Rows[0].Field<decimal>("qmoSetupHours");
			eRPQuoteOperationInformationDto.qmoSetupRate = dataTable.Rows[0].Field<decimal>("qmoSetupRate");
			eRPQuoteOperationInformationDto.qmoSfeMessageRTF = dataTable.Rows[0].Field<string>("qmoSfeMessageRTF");
			eRPQuoteOperationInformationDto.qmoSfeMessageText = dataTable.Rows[0].Field<string>("qmoSfeMessageText");
			eRPQuoteOperationInformationDto.qmoStandardFactor = dataTable.Rows[0].Field<string>("qmoStandardFactor");
			eRPQuoteOperationInformationDto.qmoSupplierOrganizationID = dataTable.Rows[0].Field<string>("qmoSupplierOrganizationID");
			eRPQuoteOperationInformationDto.qmoUnitCost1 = dataTable.Rows[0].Field<decimal>("qmoUnitCost1");
			eRPQuoteOperationInformationDto.qmoUnitCost2 = dataTable.Rows[0].Field<decimal>("qmoUnitCost2");
			eRPQuoteOperationInformationDto.qmoUnitCost3 = dataTable.Rows[0].Field<decimal>("qmoUnitCost3");
			eRPQuoteOperationInformationDto.qmoUnitCost4 = dataTable.Rows[0].Field<decimal>("qmoUnitCost4");
			eRPQuoteOperationInformationDto.qmoUnitCost5 = dataTable.Rows[0].Field<decimal>("qmoUnitCost5");
			eRPQuoteOperationInformationDto.qmoUnitCost6 = dataTable.Rows[0].Field<decimal>("qmoUnitCost6");
			eRPQuoteOperationInformationDto.qmoUnitCost7 = dataTable.Rows[0].Field<decimal>("qmoUnitCost7");
			eRPQuoteOperationInformationDto.qmoUnitCost8 = dataTable.Rows[0].Field<decimal>("qmoUnitCost8");
			eRPQuoteOperationInformationDto.qmoUnitCost9 = dataTable.Rows[0].Field<decimal>("qmoUnitCost9");
			eRPQuoteOperationInformationDto.qmoUnitOfMeasure = dataTable.Rows[0].Field<string>("qmoUnitOfMeasure");
			eRPQuoteOperationInformationDto.qmoWorkCenterID = dataTable.Rows[0].Field<string>("qmoWorkCenterID");
			eRPQuoteOperationInformationDto.qmoWorkCenterMachineID = dataTable.Rows[0].Field<short>("qmoWorkCenterMachineID");
			eRPQuoteOperationInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPQuoteOperationInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPQuoteOperationInformationDto);
	}

	public Task<APIValidationInfoDto> SaveQuoteOperation(ERPQuoteOperationDto quoteOperation)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM QuoteOperations WHERE qmoUniqueID = " + M1Util.ConvertToLinq(quoteOperation.qmoUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qmoQuoteID"] = quoteOperation.qmoQuoteID.ToUpper();
				dataRow["qmoQuoteLineID"] = quoteOperation.qmoQuoteLineID;
				dataRow["qmoQuoteAssemblyID"] = quoteOperation.qmoQuoteAssemblyID;
				dataRow["qmoQuoteOperationID"] = quoteOperation.qmoQuoteOperationID;
				quoteOperation.qmoUniqueID = ((quoteOperation.qmoUniqueID == Guid.Empty) ? Guid.NewGuid() : quoteOperation.qmoUniqueID);
				dataRow["qmoUniqueID"] = quoteOperation.qmoUniqueID;
				dataRow["qmoCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qmoCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The QuoteOperation could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (quoteOperation.qmoRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the QuoteOperation is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["qmoRowVersion"], quoteOperation.qmoRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the QuoteOperation has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the QuoteOperation again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qmoAdditionalSetupHours"] = quoteOperation.qmoAdditionalSetupHours;
			dataRow["qmoAdditionalSetupQuantity"] = quoteOperation.qmoAdditionalSetupQuantity;
			dataRow["qmoDocuments"] = quoteOperation.qmoDocuments ?? dataRow["qmoDocuments"];
			dataRow["qmoEstimatedUnitCost"] = quoteOperation.qmoEstimatedUnitCost;
			dataRow["qmoInspectionType"] = quoteOperation.qmoInspectionType;
			dataRow["qmoClosed"] = quoteOperation.qmoClosed;
			dataRow["qmoMachinesToSchedule"] = quoteOperation.qmoMachinesToSchedule;
			dataRow["qmoMachineType"] = quoteOperation.qmoMachineType;
			dataRow["qmoMinimumCharge"] = quoteOperation.qmoMinimumCharge;
			dataRow["qmoMoveTime"] = quoteOperation.qmoMoveTime;
			dataRow["qmoOperationType"] = quoteOperation.qmoOperationType;
			dataRow["qmoOverheadRate"] = quoteOperation.qmoOverheadRate;
			dataRow["qmoOverlap"] = quoteOperation.qmoOverlap;
			dataRow["qmoOverlapDestinationLink"] = quoteOperation.qmoOverlapDestinationLink;
			dataRow["qmoOverlapOffsetTime"] = quoteOperation.qmoOverlapOffsetTime;
			dataRow["qmoOverlapOperationID"] = quoteOperation.qmoOverlapOperationID;
			dataRow["qmoOverlapSourceLink"] = quoteOperation.qmoOverlapSourceLink;
			dataRow["qmoPartID"] = quoteOperation.qmoPartID;
			dataRow["qmoPartRevisionID"] = quoteOperation.qmoPartRevisionID;
			dataRow["qmoPlantDepartmentID"] = quoteOperation.qmoPlantDepartmentID;
			dataRow["qmoPlantID"] = quoteOperation.qmoPlantID;
			dataRow["qmoProcessID"] = quoteOperation.qmoProcessID;
			dataRow["qmoProcessLongDescriptionRtf"] = quoteOperation.qmoProcessLongDescriptionRtf ?? dataRow["qmoProcessLongDescriptionRtf"];
			dataRow["qmoProcessLongDescriptionText"] = quoteOperation.qmoProcessLongDescriptionText ?? dataRow["qmoProcessLongDescriptionText"];
			dataRow["qmoProcessShortDescription"] = quoteOperation.qmoProcessShortDescription;
			dataRow["qmoProductionRate"] = quoteOperation.qmoProductionRate;
			dataRow["qmoProductionStandard"] = quoteOperation.qmoProductionStandard;
			dataRow["qmoPurchaseLocationID"] = quoteOperation.qmoPurchaseLocationID;
			dataRow["qmoQuantityBreak1"] = quoteOperation.qmoQuantityBreak1;
			dataRow["qmoQuantityBreak2"] = quoteOperation.qmoQuantityBreak2;
			dataRow["qmoQuantityBreak3"] = quoteOperation.qmoQuantityBreak3;
			dataRow["qmoQuantityBreak4"] = quoteOperation.qmoQuantityBreak4;
			dataRow["qmoQuantityBreak5"] = quoteOperation.qmoQuantityBreak5;
			dataRow["qmoQuantityBreak6"] = quoteOperation.qmoQuantityBreak6;
			dataRow["qmoQuantityBreak7"] = quoteOperation.qmoQuantityBreak7;
			dataRow["qmoQuantityBreak8"] = quoteOperation.qmoQuantityBreak8;
			dataRow["qmoQuantityBreak9"] = quoteOperation.qmoQuantityBreak9;
			dataRow["qmoQuantityPerAssembly"] = quoteOperation.qmoQuantityPerAssembly;
			dataRow["qmoQueueTime"] = quoteOperation.qmoQueueTime;
			dataRow["qmoQuotingRate"] = quoteOperation.qmoQuotingRate;
			dataRow["qmoSetupCharge"] = quoteOperation.qmoSetupCharge;
			dataRow["qmoSetupHours"] = quoteOperation.qmoSetupHours;
			dataRow["qmoSetupRate"] = quoteOperation.qmoSetupRate;
			dataRow["qmoSfeMessageRTF"] = quoteOperation.qmoSfeMessageRTF ?? dataRow["qmoSfeMessageRTF"];
			dataRow["qmoSfeMessageText"] = quoteOperation.qmoSfeMessageText ?? dataRow["qmoSfeMessageText"];
			dataRow["qmoStandardFactor"] = quoteOperation.qmoStandardFactor;
			dataRow["qmoSupplierOrganizationID"] = quoteOperation.qmoSupplierOrganizationID;
			dataRow["qmoUnitCost1"] = quoteOperation.qmoUnitCost1;
			dataRow["qmoUnitCost2"] = quoteOperation.qmoUnitCost2;
			dataRow["qmoUnitCost3"] = quoteOperation.qmoUnitCost3;
			dataRow["qmoUnitCost4"] = quoteOperation.qmoUnitCost4;
			dataRow["qmoUnitCost5"] = quoteOperation.qmoUnitCost5;
			dataRow["qmoUnitCost6"] = quoteOperation.qmoUnitCost6;
			dataRow["qmoUnitCost7"] = quoteOperation.qmoUnitCost7;
			dataRow["qmoUnitCost8"] = quoteOperation.qmoUnitCost8;
			dataRow["qmoUnitCost9"] = quoteOperation.qmoUnitCost9;
			dataRow["qmoUnitOfMeasure"] = quoteOperation.qmoUnitOfMeasure;
			dataRow["qmoWorkCenterID"] = quoteOperation.qmoWorkCenterID;
			dataRow["qmoWorkCenterMachineID"] = quoteOperation.qmoWorkCenterMachineID;
			if (quoteOperation.CustomFields != null && quoteOperation.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in quoteOperation.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the QuoteOperation [{quoteOperation.qmoUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the QuoteOperation [{quoteOperation.qmoUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
