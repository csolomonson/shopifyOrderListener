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

public class ERPJobOperationRepository : APIBaseRepository, IERPJobOperationRepository, IAPIBaseRepository, IDisposable
{
	public ERPJobOperationRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesJobOperationExist(Guid jobOperationId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmoUniqueID|C", jobOperationId);
		base.selectList.Add("jmoUniqueID");
		return Task.FromResult(GetAsObject("JobOperations", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPJobOperationInformationDto>> GetAllJobOperations(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPJobOperationInformationDto> collection = new List<ERPJobOperationInformationDto>();
		InitializeParameterLists();
		string[] array = new string[90]
		{
			"jmoActualProductionHours", "jmoActualSetupHours", "jmoCalculatedUnitCost", "jmoCompletedProductionHours", "jmoCompletedSetupHours", "jmoCreatedBy", "jmoCreatedDate", "jmoDocuments", "jmoDueDate", "jmoDueHour",
			"jmoUniqueID", "jmoEstimatedProductionHours", "jmoEstimatedUnitCost", "jmoInspectionStatus", "jmoInspectionType", "jmoAddedOperation", "jmoClosed", "jmoFirm", "jmoInspectionComplete", "jmoProductionComplete",
			"jmoPrototypeOperation", "jmoSetupComplete", "jmoJobAssemblyID", "jmoJobID", "jmoMachinesToSchedule", "jmoMachineType", "jmoMinimumCharge", "jmoMoveTime", "jmoOperationQuantity", "jmoOperationType",
			"jmoOverheadRate", "jmoOverlap", "jmoOverlapDestinationLink", "jmoOverlapOffsetTime", "jmoOverlapOperationID", "jmoOverlapSourceLink", "jmoPartBinID", "jmoPartID", "jmoPartRevisionID", "jmoPartWarehouseLocationID",
			"jmoPlantDepartmentID", "jmoPlantID", "jmoProcessID", "jmoProcessLongDescriptionRtf", "jmoProcessLongDescriptionText", "jmoProcessShortDescription", "jmoProductionRate", "jmoProductionStandard", "jmoPurchaseLocationID", "jmoPurchaseOrderID",
			"jmoQuantityBreak1", "jmoQuantityBreak2", "jmoQuantityBreak3", "jmoQuantityBreak4", "jmoQuantityBreak5", "jmoQuantityBreak6", "jmoQuantityBreak7", "jmoQuantityBreak8", "jmoQuantityBreak9", "jmoQuantityComplete",
			"jmoQuantityPerAssembly", "jmoQuantityToInspect", "jmoQuantityToReturn", "jmoQueueTime", "jmoRfqID", "jmoRowVersion", "jmoScrapQuantityReceived", "jmoJobOperationID", "jmoSetupCharge", "jmoSetupHours",
			"jmoSetupPercentComplete", "jmoSetupRate", "jmoSfeMessageRTF", "jmoSfeMessageText", "jmoStandardFactor", "jmoStartDate", "jmoStartHour", "jmoSupplierOrganizationID", "jmoUnitCost1", "jmoUnitCost2",
			"jmoUnitCost3", "jmoUnitCost4", "jmoUnitCost5", "jmoUnitCost6", "jmoUnitCost7", "jmoUnitCost8", "jmoUnitCost9", "jmoUnitOfMeasure", "jmoWorkCenterID", "jmoWorkCenterMachineID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("JobOperations");
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
		using (DataTable dataTable = GetAsDataTable("JobOperations", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPJobOperationInformationDto eRPJobOperationInformationDto = new ERPJobOperationInformationDto();
				eRPJobOperationInformationDto.jmoActualProductionHours = dataTable.Rows[i].Field<decimal>("jmoActualProductionHours");
				eRPJobOperationInformationDto.jmoActualSetupHours = dataTable.Rows[i].Field<decimal>("jmoActualSetupHours");
				eRPJobOperationInformationDto.jmoCalculatedUnitCost = dataTable.Rows[i].Field<decimal>("jmoCalculatedUnitCost");
				eRPJobOperationInformationDto.jmoCompletedProductionHours = dataTable.Rows[i].Field<decimal>("jmoCompletedProductionHours");
				eRPJobOperationInformationDto.jmoCompletedSetupHours = dataTable.Rows[i].Field<decimal>("jmoCompletedSetupHours");
				eRPJobOperationInformationDto.jmoCreatedBy = dataTable.Rows[i].Field<string>("jmoCreatedBy");
				eRPJobOperationInformationDto.jmoCreatedDate = dataTable.Rows[i].Field<DateTime?>("jmoCreatedDate");
				eRPJobOperationInformationDto.jmoDocuments = dataTable.Rows[i].Field<string>("jmoDocuments");
				eRPJobOperationInformationDto.jmoDueDate = dataTable.Rows[i].Field<DateTime?>("jmoDueDate");
				eRPJobOperationInformationDto.jmoDueHour = dataTable.Rows[i].Field<decimal>("jmoDueHour");
				eRPJobOperationInformationDto.jmoUniqueID = dataTable.Rows[i].Field<Guid>("jmoUniqueID");
				eRPJobOperationInformationDto.jmoEstimatedProductionHours = dataTable.Rows[i].Field<decimal>("jmoEstimatedProductionHours");
				eRPJobOperationInformationDto.jmoEstimatedUnitCost = dataTable.Rows[i].Field<decimal>("jmoEstimatedUnitCost");
				eRPJobOperationInformationDto.jmoInspectionStatus = dataTable.Rows[i].Field<byte>("jmoInspectionStatus");
				eRPJobOperationInformationDto.jmoInspectionType = dataTable.Rows[i].Field<byte>("jmoInspectionType");
				eRPJobOperationInformationDto.jmoAddedOperation = dataTable.Rows[i].Field<bool>("jmoAddedOperation");
				eRPJobOperationInformationDto.jmoClosed = dataTable.Rows[i].Field<bool>("jmoClosed");
				eRPJobOperationInformationDto.jmoFirm = dataTable.Rows[i].Field<bool>("jmoFirm");
				eRPJobOperationInformationDto.jmoInspectionComplete = dataTable.Rows[i].Field<bool>("jmoInspectionComplete");
				eRPJobOperationInformationDto.jmoProductionComplete = dataTable.Rows[i].Field<bool>("jmoProductionComplete");
				eRPJobOperationInformationDto.jmoPrototypeOperation = dataTable.Rows[i].Field<bool>("jmoPrototypeOperation");
				eRPJobOperationInformationDto.jmoSetupComplete = dataTable.Rows[i].Field<bool>("jmoSetupComplete");
				eRPJobOperationInformationDto.jmoJobAssemblyID = dataTable.Rows[i].Field<int>("jmoJobAssemblyID");
				eRPJobOperationInformationDto.jmoJobID = dataTable.Rows[i].Field<string>("jmoJobID");
				eRPJobOperationInformationDto.jmoMachinesToSchedule = dataTable.Rows[i].Field<short>("jmoMachinesToSchedule");
				eRPJobOperationInformationDto.jmoMachineType = dataTable.Rows[i].Field<byte>("jmoMachineType");
				eRPJobOperationInformationDto.jmoMinimumCharge = dataTable.Rows[i].Field<decimal>("jmoMinimumCharge");
				eRPJobOperationInformationDto.jmoMoveTime = dataTable.Rows[i].Field<decimal>("jmoMoveTime");
				eRPJobOperationInformationDto.jmoOperationQuantity = dataTable.Rows[i].Field<decimal>("jmoOperationQuantity");
				eRPJobOperationInformationDto.jmoOperationType = dataTable.Rows[i].Field<byte>("jmoOperationType");
				eRPJobOperationInformationDto.jmoOverheadRate = dataTable.Rows[i].Field<decimal>("jmoOverheadRate");
				eRPJobOperationInformationDto.jmoOverlap = dataTable.Rows[i].Field<byte>("jmoOverlap");
				eRPJobOperationInformationDto.jmoOverlapDestinationLink = dataTable.Rows[i].Field<byte>("jmoOverlapDestinationLink");
				eRPJobOperationInformationDto.jmoOverlapOffsetTime = dataTable.Rows[i].Field<decimal>("jmoOverlapOffsetTime");
				eRPJobOperationInformationDto.jmoOverlapOperationID = dataTable.Rows[i].Field<int>("jmoOverlapOperationID");
				eRPJobOperationInformationDto.jmoOverlapSourceLink = dataTable.Rows[i].Field<byte>("jmoOverlapSourceLink");
				eRPJobOperationInformationDto.jmoPartBinID = dataTable.Rows[i].Field<string>("jmoPartBinID");
				eRPJobOperationInformationDto.jmoPartID = dataTable.Rows[i].Field<string>("jmoPartID");
				eRPJobOperationInformationDto.jmoPartRevisionID = dataTable.Rows[i].Field<string>("jmoPartRevisionID");
				eRPJobOperationInformationDto.jmoPartWarehouseLocationID = dataTable.Rows[i].Field<string>("jmoPartWarehouseLocationID");
				eRPJobOperationInformationDto.jmoPlantDepartmentID = dataTable.Rows[i].Field<string>("jmoPlantDepartmentID");
				eRPJobOperationInformationDto.jmoPlantID = dataTable.Rows[i].Field<string>("jmoPlantID");
				eRPJobOperationInformationDto.jmoProcessID = dataTable.Rows[i].Field<string>("jmoProcessID");
				eRPJobOperationInformationDto.jmoProcessLongDescriptionRtf = dataTable.Rows[i].Field<string>("jmoProcessLongDescriptionRtf");
				eRPJobOperationInformationDto.jmoProcessLongDescriptionText = dataTable.Rows[i].Field<string>("jmoProcessLongDescriptionText");
				eRPJobOperationInformationDto.jmoProcessShortDescription = dataTable.Rows[i].Field<string>("jmoProcessShortDescription");
				eRPJobOperationInformationDto.jmoProductionRate = dataTable.Rows[i].Field<decimal>("jmoProductionRate");
				eRPJobOperationInformationDto.jmoProductionStandard = dataTable.Rows[i].Field<decimal>("jmoProductionStandard");
				eRPJobOperationInformationDto.jmoPurchaseLocationID = dataTable.Rows[i].Field<string>("jmoPurchaseLocationID");
				eRPJobOperationInformationDto.jmoPurchaseOrderID = dataTable.Rows[i].Field<string>("jmoPurchaseOrderID");
				eRPJobOperationInformationDto.jmoQuantityBreak1 = dataTable.Rows[i].Field<decimal>("jmoQuantityBreak1");
				eRPJobOperationInformationDto.jmoQuantityBreak2 = dataTable.Rows[i].Field<decimal>("jmoQuantityBreak2");
				eRPJobOperationInformationDto.jmoQuantityBreak3 = dataTable.Rows[i].Field<decimal>("jmoQuantityBreak3");
				eRPJobOperationInformationDto.jmoQuantityBreak4 = dataTable.Rows[i].Field<decimal>("jmoQuantityBreak4");
				eRPJobOperationInformationDto.jmoQuantityBreak5 = dataTable.Rows[i].Field<decimal>("jmoQuantityBreak5");
				eRPJobOperationInformationDto.jmoQuantityBreak6 = dataTable.Rows[i].Field<decimal>("jmoQuantityBreak6");
				eRPJobOperationInformationDto.jmoQuantityBreak7 = dataTable.Rows[i].Field<decimal>("jmoQuantityBreak7");
				eRPJobOperationInformationDto.jmoQuantityBreak8 = dataTable.Rows[i].Field<decimal>("jmoQuantityBreak8");
				eRPJobOperationInformationDto.jmoQuantityBreak9 = dataTable.Rows[i].Field<decimal>("jmoQuantityBreak9");
				eRPJobOperationInformationDto.jmoQuantityComplete = dataTable.Rows[i].Field<decimal>("jmoQuantityComplete");
				eRPJobOperationInformationDto.jmoQuantityPerAssembly = dataTable.Rows[i].Field<decimal>("jmoQuantityPerAssembly");
				eRPJobOperationInformationDto.jmoQuantityToInspect = dataTable.Rows[i].Field<decimal>("jmoQuantityToInspect");
				eRPJobOperationInformationDto.jmoQuantityToReturn = dataTable.Rows[i].Field<decimal>("jmoQuantityToReturn");
				eRPJobOperationInformationDto.jmoQueueTime = dataTable.Rows[i].Field<decimal>("jmoQueueTime");
				eRPJobOperationInformationDto.jmoRfqID = dataTable.Rows[i].Field<string>("jmoRfqID");
				eRPJobOperationInformationDto.jmoRowVersion = dataTable.Rows[i].Field<byte[]>("jmoRowVersion");
				eRPJobOperationInformationDto.jmoScrapQuantityReceived = dataTable.Rows[i].Field<decimal>("jmoScrapQuantityReceived");
				eRPJobOperationInformationDto.jmoJobOperationID = dataTable.Rows[i].Field<int>("jmoJobOperationID");
				eRPJobOperationInformationDto.jmoSetupCharge = dataTable.Rows[i].Field<decimal>("jmoSetupCharge");
				eRPJobOperationInformationDto.jmoSetupHours = dataTable.Rows[i].Field<decimal>("jmoSetupHours");
				eRPJobOperationInformationDto.jmoSetupPercentComplete = dataTable.Rows[i].Field<short>("jmoSetupPercentComplete");
				eRPJobOperationInformationDto.jmoSetupRate = dataTable.Rows[i].Field<decimal>("jmoSetupRate");
				eRPJobOperationInformationDto.jmoSfeMessageRTF = dataTable.Rows[i].Field<string>("jmoSfeMessageRTF");
				eRPJobOperationInformationDto.jmoSfeMessageText = dataTable.Rows[i].Field<string>("jmoSfeMessageText");
				eRPJobOperationInformationDto.jmoStandardFactor = dataTable.Rows[i].Field<string>("jmoStandardFactor");
				eRPJobOperationInformationDto.jmoStartDate = dataTable.Rows[i].Field<DateTime?>("jmoStartDate");
				eRPJobOperationInformationDto.jmoStartHour = dataTable.Rows[i].Field<decimal>("jmoStartHour");
				eRPJobOperationInformationDto.jmoSupplierOrganizationID = dataTable.Rows[i].Field<string>("jmoSupplierOrganizationID");
				eRPJobOperationInformationDto.jmoUnitCost1 = dataTable.Rows[i].Field<decimal>("jmoUnitCost1");
				eRPJobOperationInformationDto.jmoUnitCost2 = dataTable.Rows[i].Field<decimal>("jmoUnitCost2");
				eRPJobOperationInformationDto.jmoUnitCost3 = dataTable.Rows[i].Field<decimal>("jmoUnitCost3");
				eRPJobOperationInformationDto.jmoUnitCost4 = dataTable.Rows[i].Field<decimal>("jmoUnitCost4");
				eRPJobOperationInformationDto.jmoUnitCost5 = dataTable.Rows[i].Field<decimal>("jmoUnitCost5");
				eRPJobOperationInformationDto.jmoUnitCost6 = dataTable.Rows[i].Field<decimal>("jmoUnitCost6");
				eRPJobOperationInformationDto.jmoUnitCost7 = dataTable.Rows[i].Field<decimal>("jmoUnitCost7");
				eRPJobOperationInformationDto.jmoUnitCost8 = dataTable.Rows[i].Field<decimal>("jmoUnitCost8");
				eRPJobOperationInformationDto.jmoUnitCost9 = dataTable.Rows[i].Field<decimal>("jmoUnitCost9");
				eRPJobOperationInformationDto.jmoUnitOfMeasure = dataTable.Rows[i].Field<string>("jmoUnitOfMeasure");
				eRPJobOperationInformationDto.jmoWorkCenterID = dataTable.Rows[i].Field<string>("jmoWorkCenterID");
				eRPJobOperationInformationDto.jmoWorkCenterMachineID = dataTable.Rows[i].Field<short>("jmoWorkCenterMachineID");
				eRPJobOperationInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPJobOperationInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPJobOperationInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPJobOperationInformationDto> GetJobOperation(Guid jobOperationId)
	{
		ERPJobOperationInformationDto eRPJobOperationInformationDto = new ERPJobOperationInformationDto();
		InitializeParameterLists();
		string[] collection = new string[90]
		{
			"jmoActualProductionHours", "jmoActualSetupHours", "jmoCalculatedUnitCost", "jmoCompletedProductionHours", "jmoCompletedSetupHours", "jmoCreatedBy", "jmoCreatedDate", "jmoDocuments", "jmoDueDate", "jmoDueHour",
			"jmoUniqueID", "jmoEstimatedProductionHours", "jmoEstimatedUnitCost", "jmoInspectionStatus", "jmoInspectionType", "jmoAddedOperation", "jmoClosed", "jmoFirm", "jmoInspectionComplete", "jmoProductionComplete",
			"jmoPrototypeOperation", "jmoSetupComplete", "jmoJobAssemblyID", "jmoJobID", "jmoMachinesToSchedule", "jmoMachineType", "jmoMinimumCharge", "jmoMoveTime", "jmoOperationQuantity", "jmoOperationType",
			"jmoOverheadRate", "jmoOverlap", "jmoOverlapDestinationLink", "jmoOverlapOffsetTime", "jmoOverlapOperationID", "jmoOverlapSourceLink", "jmoPartBinID", "jmoPartID", "jmoPartRevisionID", "jmoPartWarehouseLocationID",
			"jmoPlantDepartmentID", "jmoPlantID", "jmoProcessID", "jmoProcessLongDescriptionRtf", "jmoProcessLongDescriptionText", "jmoProcessShortDescription", "jmoProductionRate", "jmoProductionStandard", "jmoPurchaseLocationID", "jmoPurchaseOrderID",
			"jmoQuantityBreak1", "jmoQuantityBreak2", "jmoQuantityBreak3", "jmoQuantityBreak4", "jmoQuantityBreak5", "jmoQuantityBreak6", "jmoQuantityBreak7", "jmoQuantityBreak8", "jmoQuantityBreak9", "jmoQuantityComplete",
			"jmoQuantityPerAssembly", "jmoQuantityToInspect", "jmoQuantityToReturn", "jmoQueueTime", "jmoRfqID", "jmoRowVersion", "jmoScrapQuantityReceived", "jmoJobOperationID", "jmoSetupCharge", "jmoSetupHours",
			"jmoSetupPercentComplete", "jmoSetupRate", "jmoSfeMessageRTF", "jmoSfeMessageText", "jmoStandardFactor", "jmoStartDate", "jmoStartHour", "jmoSupplierOrganizationID", "jmoUnitCost1", "jmoUnitCost2",
			"jmoUnitCost3", "jmoUnitCost4", "jmoUnitCost5", "jmoUnitCost6", "jmoUnitCost7", "jmoUnitCost8", "jmoUnitCost9", "jmoUnitOfMeasure", "jmoWorkCenterID", "jmoWorkCenterMachineID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("jmoUniqueID|C", jobOperationId);
		AddCustomFieldsToSelectList("JobOperations");
		using (DataTable dataTable = GetAsDataTable("JobOperations", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPJobOperationInformationDto);
			}
			eRPJobOperationInformationDto.jmoActualProductionHours = dataTable.Rows[0].Field<decimal>("jmoActualProductionHours");
			eRPJobOperationInformationDto.jmoActualSetupHours = dataTable.Rows[0].Field<decimal>("jmoActualSetupHours");
			eRPJobOperationInformationDto.jmoCalculatedUnitCost = dataTable.Rows[0].Field<decimal>("jmoCalculatedUnitCost");
			eRPJobOperationInformationDto.jmoCompletedProductionHours = dataTable.Rows[0].Field<decimal>("jmoCompletedProductionHours");
			eRPJobOperationInformationDto.jmoCompletedSetupHours = dataTable.Rows[0].Field<decimal>("jmoCompletedSetupHours");
			eRPJobOperationInformationDto.jmoCreatedBy = dataTable.Rows[0].Field<string>("jmoCreatedBy");
			eRPJobOperationInformationDto.jmoCreatedDate = dataTable.Rows[0].Field<DateTime?>("jmoCreatedDate");
			eRPJobOperationInformationDto.jmoDocuments = dataTable.Rows[0].Field<string>("jmoDocuments");
			eRPJobOperationInformationDto.jmoDueDate = dataTable.Rows[0].Field<DateTime?>("jmoDueDate");
			eRPJobOperationInformationDto.jmoDueHour = dataTable.Rows[0].Field<decimal>("jmoDueHour");
			eRPJobOperationInformationDto.jmoUniqueID = dataTable.Rows[0].Field<Guid>("jmoUniqueID");
			eRPJobOperationInformationDto.jmoEstimatedProductionHours = dataTable.Rows[0].Field<decimal>("jmoEstimatedProductionHours");
			eRPJobOperationInformationDto.jmoEstimatedUnitCost = dataTable.Rows[0].Field<decimal>("jmoEstimatedUnitCost");
			eRPJobOperationInformationDto.jmoInspectionStatus = dataTable.Rows[0].Field<byte>("jmoInspectionStatus");
			eRPJobOperationInformationDto.jmoInspectionType = dataTable.Rows[0].Field<byte>("jmoInspectionType");
			eRPJobOperationInformationDto.jmoAddedOperation = dataTable.Rows[0].Field<bool>("jmoAddedOperation");
			eRPJobOperationInformationDto.jmoClosed = dataTable.Rows[0].Field<bool>("jmoClosed");
			eRPJobOperationInformationDto.jmoFirm = dataTable.Rows[0].Field<bool>("jmoFirm");
			eRPJobOperationInformationDto.jmoInspectionComplete = dataTable.Rows[0].Field<bool>("jmoInspectionComplete");
			eRPJobOperationInformationDto.jmoProductionComplete = dataTable.Rows[0].Field<bool>("jmoProductionComplete");
			eRPJobOperationInformationDto.jmoPrototypeOperation = dataTable.Rows[0].Field<bool>("jmoPrototypeOperation");
			eRPJobOperationInformationDto.jmoSetupComplete = dataTable.Rows[0].Field<bool>("jmoSetupComplete");
			eRPJobOperationInformationDto.jmoJobAssemblyID = dataTable.Rows[0].Field<int>("jmoJobAssemblyID");
			eRPJobOperationInformationDto.jmoJobID = dataTable.Rows[0].Field<string>("jmoJobID");
			eRPJobOperationInformationDto.jmoMachinesToSchedule = dataTable.Rows[0].Field<short>("jmoMachinesToSchedule");
			eRPJobOperationInformationDto.jmoMachineType = dataTable.Rows[0].Field<byte>("jmoMachineType");
			eRPJobOperationInformationDto.jmoMinimumCharge = dataTable.Rows[0].Field<decimal>("jmoMinimumCharge");
			eRPJobOperationInformationDto.jmoMoveTime = dataTable.Rows[0].Field<decimal>("jmoMoveTime");
			eRPJobOperationInformationDto.jmoOperationQuantity = dataTable.Rows[0].Field<decimal>("jmoOperationQuantity");
			eRPJobOperationInformationDto.jmoOperationType = dataTable.Rows[0].Field<byte>("jmoOperationType");
			eRPJobOperationInformationDto.jmoOverheadRate = dataTable.Rows[0].Field<decimal>("jmoOverheadRate");
			eRPJobOperationInformationDto.jmoOverlap = dataTable.Rows[0].Field<byte>("jmoOverlap");
			eRPJobOperationInformationDto.jmoOverlapDestinationLink = dataTable.Rows[0].Field<byte>("jmoOverlapDestinationLink");
			eRPJobOperationInformationDto.jmoOverlapOffsetTime = dataTable.Rows[0].Field<decimal>("jmoOverlapOffsetTime");
			eRPJobOperationInformationDto.jmoOverlapOperationID = dataTable.Rows[0].Field<int>("jmoOverlapOperationID");
			eRPJobOperationInformationDto.jmoOverlapSourceLink = dataTable.Rows[0].Field<byte>("jmoOverlapSourceLink");
			eRPJobOperationInformationDto.jmoPartBinID = dataTable.Rows[0].Field<string>("jmoPartBinID");
			eRPJobOperationInformationDto.jmoPartID = dataTable.Rows[0].Field<string>("jmoPartID");
			eRPJobOperationInformationDto.jmoPartRevisionID = dataTable.Rows[0].Field<string>("jmoPartRevisionID");
			eRPJobOperationInformationDto.jmoPartWarehouseLocationID = dataTable.Rows[0].Field<string>("jmoPartWarehouseLocationID");
			eRPJobOperationInformationDto.jmoPlantDepartmentID = dataTable.Rows[0].Field<string>("jmoPlantDepartmentID");
			eRPJobOperationInformationDto.jmoPlantID = dataTable.Rows[0].Field<string>("jmoPlantID");
			eRPJobOperationInformationDto.jmoProcessID = dataTable.Rows[0].Field<string>("jmoProcessID");
			eRPJobOperationInformationDto.jmoProcessLongDescriptionRtf = dataTable.Rows[0].Field<string>("jmoProcessLongDescriptionRtf");
			eRPJobOperationInformationDto.jmoProcessLongDescriptionText = dataTable.Rows[0].Field<string>("jmoProcessLongDescriptionText");
			eRPJobOperationInformationDto.jmoProcessShortDescription = dataTable.Rows[0].Field<string>("jmoProcessShortDescription");
			eRPJobOperationInformationDto.jmoProductionRate = dataTable.Rows[0].Field<decimal>("jmoProductionRate");
			eRPJobOperationInformationDto.jmoProductionStandard = dataTable.Rows[0].Field<decimal>("jmoProductionStandard");
			eRPJobOperationInformationDto.jmoPurchaseLocationID = dataTable.Rows[0].Field<string>("jmoPurchaseLocationID");
			eRPJobOperationInformationDto.jmoPurchaseOrderID = dataTable.Rows[0].Field<string>("jmoPurchaseOrderID");
			eRPJobOperationInformationDto.jmoQuantityBreak1 = dataTable.Rows[0].Field<decimal>("jmoQuantityBreak1");
			eRPJobOperationInformationDto.jmoQuantityBreak2 = dataTable.Rows[0].Field<decimal>("jmoQuantityBreak2");
			eRPJobOperationInformationDto.jmoQuantityBreak3 = dataTable.Rows[0].Field<decimal>("jmoQuantityBreak3");
			eRPJobOperationInformationDto.jmoQuantityBreak4 = dataTable.Rows[0].Field<decimal>("jmoQuantityBreak4");
			eRPJobOperationInformationDto.jmoQuantityBreak5 = dataTable.Rows[0].Field<decimal>("jmoQuantityBreak5");
			eRPJobOperationInformationDto.jmoQuantityBreak6 = dataTable.Rows[0].Field<decimal>("jmoQuantityBreak6");
			eRPJobOperationInformationDto.jmoQuantityBreak7 = dataTable.Rows[0].Field<decimal>("jmoQuantityBreak7");
			eRPJobOperationInformationDto.jmoQuantityBreak8 = dataTable.Rows[0].Field<decimal>("jmoQuantityBreak8");
			eRPJobOperationInformationDto.jmoQuantityBreak9 = dataTable.Rows[0].Field<decimal>("jmoQuantityBreak9");
			eRPJobOperationInformationDto.jmoQuantityComplete = dataTable.Rows[0].Field<decimal>("jmoQuantityComplete");
			eRPJobOperationInformationDto.jmoQuantityPerAssembly = dataTable.Rows[0].Field<decimal>("jmoQuantityPerAssembly");
			eRPJobOperationInformationDto.jmoQuantityToInspect = dataTable.Rows[0].Field<decimal>("jmoQuantityToInspect");
			eRPJobOperationInformationDto.jmoQuantityToReturn = dataTable.Rows[0].Field<decimal>("jmoQuantityToReturn");
			eRPJobOperationInformationDto.jmoQueueTime = dataTable.Rows[0].Field<decimal>("jmoQueueTime");
			eRPJobOperationInformationDto.jmoRfqID = dataTable.Rows[0].Field<string>("jmoRfqID");
			eRPJobOperationInformationDto.jmoRowVersion = dataTable.Rows[0].Field<byte[]>("jmoRowVersion");
			eRPJobOperationInformationDto.jmoScrapQuantityReceived = dataTable.Rows[0].Field<decimal>("jmoScrapQuantityReceived");
			eRPJobOperationInformationDto.jmoJobOperationID = dataTable.Rows[0].Field<int>("jmoJobOperationID");
			eRPJobOperationInformationDto.jmoSetupCharge = dataTable.Rows[0].Field<decimal>("jmoSetupCharge");
			eRPJobOperationInformationDto.jmoSetupHours = dataTable.Rows[0].Field<decimal>("jmoSetupHours");
			eRPJobOperationInformationDto.jmoSetupPercentComplete = dataTable.Rows[0].Field<short>("jmoSetupPercentComplete");
			eRPJobOperationInformationDto.jmoSetupRate = dataTable.Rows[0].Field<decimal>("jmoSetupRate");
			eRPJobOperationInformationDto.jmoSfeMessageRTF = dataTable.Rows[0].Field<string>("jmoSfeMessageRTF");
			eRPJobOperationInformationDto.jmoSfeMessageText = dataTable.Rows[0].Field<string>("jmoSfeMessageText");
			eRPJobOperationInformationDto.jmoStandardFactor = dataTable.Rows[0].Field<string>("jmoStandardFactor");
			eRPJobOperationInformationDto.jmoStartDate = dataTable.Rows[0].Field<DateTime?>("jmoStartDate");
			eRPJobOperationInformationDto.jmoStartHour = dataTable.Rows[0].Field<decimal>("jmoStartHour");
			eRPJobOperationInformationDto.jmoSupplierOrganizationID = dataTable.Rows[0].Field<string>("jmoSupplierOrganizationID");
			eRPJobOperationInformationDto.jmoUnitCost1 = dataTable.Rows[0].Field<decimal>("jmoUnitCost1");
			eRPJobOperationInformationDto.jmoUnitCost2 = dataTable.Rows[0].Field<decimal>("jmoUnitCost2");
			eRPJobOperationInformationDto.jmoUnitCost3 = dataTable.Rows[0].Field<decimal>("jmoUnitCost3");
			eRPJobOperationInformationDto.jmoUnitCost4 = dataTable.Rows[0].Field<decimal>("jmoUnitCost4");
			eRPJobOperationInformationDto.jmoUnitCost5 = dataTable.Rows[0].Field<decimal>("jmoUnitCost5");
			eRPJobOperationInformationDto.jmoUnitCost6 = dataTable.Rows[0].Field<decimal>("jmoUnitCost6");
			eRPJobOperationInformationDto.jmoUnitCost7 = dataTable.Rows[0].Field<decimal>("jmoUnitCost7");
			eRPJobOperationInformationDto.jmoUnitCost8 = dataTable.Rows[0].Field<decimal>("jmoUnitCost8");
			eRPJobOperationInformationDto.jmoUnitCost9 = dataTable.Rows[0].Field<decimal>("jmoUnitCost9");
			eRPJobOperationInformationDto.jmoUnitOfMeasure = dataTable.Rows[0].Field<string>("jmoUnitOfMeasure");
			eRPJobOperationInformationDto.jmoWorkCenterID = dataTable.Rows[0].Field<string>("jmoWorkCenterID");
			eRPJobOperationInformationDto.jmoWorkCenterMachineID = dataTable.Rows[0].Field<short>("jmoWorkCenterMachineID");
			eRPJobOperationInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPJobOperationInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPJobOperationInformationDto);
	}

	public Task<APIValidationInfoDto> SaveJobOperation(ERPJobOperationDto jobOperation)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM JobOperations WHERE jmoUniqueID = " + M1Util.ConvertToLinq(jobOperation.jmoUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["jmoJobID"] = jobOperation.jmoJobID.ToUpper();
				dataRow["jmoJobAssemblyID"] = jobOperation.jmoJobAssemblyID;
				dataRow["jmoJobOperationID"] = jobOperation.jmoJobOperationID;
				jobOperation.jmoUniqueID = ((jobOperation.jmoUniqueID == Guid.Empty) ? Guid.NewGuid() : jobOperation.jmoUniqueID);
				dataRow["jmoUniqueID"] = jobOperation.jmoUniqueID;
				dataRow["jmoCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["jmoCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The JobOperation could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (jobOperation.jmoRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the JobOperation is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["jmoRowVersion"], jobOperation.jmoRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the JobOperation has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the JobOperation again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["jmoActualProductionHours"] = jobOperation.jmoActualProductionHours;
			dataRow["jmoActualSetupHours"] = jobOperation.jmoActualSetupHours;
			dataRow["jmoCalculatedUnitCost"] = jobOperation.jmoCalculatedUnitCost;
			dataRow["jmoCompletedProductionHours"] = jobOperation.jmoCompletedProductionHours;
			dataRow["jmoCompletedSetupHours"] = jobOperation.jmoCompletedSetupHours;
			dataRow["jmoDocuments"] = jobOperation.jmoDocuments ?? dataRow["jmoDocuments"];
			DataRow dataRow2 = dataRow;
			DateTime? jmoDueDate = jobOperation.jmoDueDate;
			dataRow2["jmoDueDate"] = (jmoDueDate.HasValue ? ((object)jmoDueDate.GetValueOrDefault()) : dataRow["jmoDueDate"]);
			dataRow["jmoDueHour"] = jobOperation.jmoDueHour;
			dataRow["jmoEstimatedProductionHours"] = jobOperation.jmoEstimatedProductionHours;
			dataRow["jmoEstimatedUnitCost"] = jobOperation.jmoEstimatedUnitCost;
			dataRow["jmoInspectionStatus"] = jobOperation.jmoInspectionStatus;
			dataRow["jmoInspectionType"] = jobOperation.jmoInspectionType;
			dataRow["jmoAddedOperation"] = jobOperation.jmoAddedOperation;
			dataRow["jmoClosed"] = jobOperation.jmoClosed;
			dataRow["jmoFirm"] = jobOperation.jmoFirm;
			dataRow["jmoInspectionComplete"] = jobOperation.jmoInspectionComplete;
			dataRow["jmoProductionComplete"] = jobOperation.jmoProductionComplete;
			dataRow["jmoPrototypeOperation"] = jobOperation.jmoPrototypeOperation;
			dataRow["jmoSetupComplete"] = jobOperation.jmoSetupComplete;
			dataRow["jmoMachinesToSchedule"] = jobOperation.jmoMachinesToSchedule;
			dataRow["jmoMachineType"] = jobOperation.jmoMachineType;
			dataRow["jmoMinimumCharge"] = jobOperation.jmoMinimumCharge;
			dataRow["jmoMoveTime"] = jobOperation.jmoMoveTime;
			dataRow["jmoOperationQuantity"] = jobOperation.jmoOperationQuantity;
			dataRow["jmoOperationType"] = jobOperation.jmoOperationType;
			dataRow["jmoOverheadRate"] = jobOperation.jmoOverheadRate;
			dataRow["jmoOverlap"] = jobOperation.jmoOverlap;
			dataRow["jmoOverlapDestinationLink"] = jobOperation.jmoOverlapDestinationLink;
			dataRow["jmoOverlapOffsetTime"] = jobOperation.jmoOverlapOffsetTime;
			dataRow["jmoOverlapOperationID"] = jobOperation.jmoOverlapOperationID;
			dataRow["jmoOverlapSourceLink"] = jobOperation.jmoOverlapSourceLink;
			dataRow["jmoPartBinID"] = jobOperation.jmoPartBinID;
			dataRow["jmoPartID"] = jobOperation.jmoPartID;
			dataRow["jmoPartRevisionID"] = jobOperation.jmoPartRevisionID;
			dataRow["jmoPartWarehouseLocationID"] = jobOperation.jmoPartWarehouseLocationID;
			dataRow["jmoPlantDepartmentID"] = jobOperation.jmoPlantDepartmentID;
			dataRow["jmoPlantID"] = jobOperation.jmoPlantID;
			dataRow["jmoProcessID"] = jobOperation.jmoProcessID;
			dataRow["jmoProcessLongDescriptionRtf"] = jobOperation.jmoProcessLongDescriptionRtf ?? dataRow["jmoProcessLongDescriptionRtf"];
			dataRow["jmoProcessLongDescriptionText"] = jobOperation.jmoProcessLongDescriptionText ?? dataRow["jmoProcessLongDescriptionText"];
			dataRow["jmoProcessShortDescription"] = jobOperation.jmoProcessShortDescription;
			dataRow["jmoProductionRate"] = jobOperation.jmoProductionRate;
			dataRow["jmoProductionStandard"] = jobOperation.jmoProductionStandard;
			dataRow["jmoPurchaseLocationID"] = jobOperation.jmoPurchaseLocationID;
			dataRow["jmoPurchaseOrderID"] = jobOperation.jmoPurchaseOrderID;
			dataRow["jmoQuantityBreak1"] = jobOperation.jmoQuantityBreak1;
			dataRow["jmoQuantityBreak2"] = jobOperation.jmoQuantityBreak2;
			dataRow["jmoQuantityBreak3"] = jobOperation.jmoQuantityBreak3;
			dataRow["jmoQuantityBreak4"] = jobOperation.jmoQuantityBreak4;
			dataRow["jmoQuantityBreak5"] = jobOperation.jmoQuantityBreak5;
			dataRow["jmoQuantityBreak6"] = jobOperation.jmoQuantityBreak6;
			dataRow["jmoQuantityBreak7"] = jobOperation.jmoQuantityBreak7;
			dataRow["jmoQuantityBreak8"] = jobOperation.jmoQuantityBreak8;
			dataRow["jmoQuantityBreak9"] = jobOperation.jmoQuantityBreak9;
			dataRow["jmoQuantityComplete"] = jobOperation.jmoQuantityComplete;
			dataRow["jmoQuantityPerAssembly"] = jobOperation.jmoQuantityPerAssembly;
			dataRow["jmoQuantityToInspect"] = jobOperation.jmoQuantityToInspect;
			dataRow["jmoQuantityToReturn"] = jobOperation.jmoQuantityToReturn;
			dataRow["jmoQueueTime"] = jobOperation.jmoQueueTime;
			dataRow["jmoRfqID"] = jobOperation.jmoRfqID;
			dataRow["jmoScrapQuantityReceived"] = jobOperation.jmoScrapQuantityReceived;
			dataRow["jmoSetupCharge"] = jobOperation.jmoSetupCharge;
			dataRow["jmoSetupHours"] = jobOperation.jmoSetupHours;
			dataRow["jmoSetupPercentComplete"] = jobOperation.jmoSetupPercentComplete;
			dataRow["jmoSetupRate"] = jobOperation.jmoSetupRate;
			dataRow["jmoSfeMessageRTF"] = jobOperation.jmoSfeMessageRTF ?? dataRow["jmoSfeMessageRTF"];
			dataRow["jmoSfeMessageText"] = jobOperation.jmoSfeMessageText ?? dataRow["jmoSfeMessageText"];
			dataRow["jmoStandardFactor"] = jobOperation.jmoStandardFactor;
			DataRow dataRow3 = dataRow;
			jmoDueDate = jobOperation.jmoStartDate;
			dataRow3["jmoStartDate"] = (jmoDueDate.HasValue ? ((object)jmoDueDate.GetValueOrDefault()) : dataRow["jmoStartDate"]);
			dataRow["jmoStartHour"] = jobOperation.jmoStartHour;
			dataRow["jmoSupplierOrganizationID"] = jobOperation.jmoSupplierOrganizationID;
			dataRow["jmoUnitCost1"] = jobOperation.jmoUnitCost1;
			dataRow["jmoUnitCost2"] = jobOperation.jmoUnitCost2;
			dataRow["jmoUnitCost3"] = jobOperation.jmoUnitCost3;
			dataRow["jmoUnitCost4"] = jobOperation.jmoUnitCost4;
			dataRow["jmoUnitCost5"] = jobOperation.jmoUnitCost5;
			dataRow["jmoUnitCost6"] = jobOperation.jmoUnitCost6;
			dataRow["jmoUnitCost7"] = jobOperation.jmoUnitCost7;
			dataRow["jmoUnitCost8"] = jobOperation.jmoUnitCost8;
			dataRow["jmoUnitCost9"] = jobOperation.jmoUnitCost9;
			dataRow["jmoUnitOfMeasure"] = jobOperation.jmoUnitOfMeasure;
			dataRow["jmoWorkCenterID"] = jobOperation.jmoWorkCenterID;
			dataRow["jmoWorkCenterMachineID"] = jobOperation.jmoWorkCenterMachineID;
			if (jobOperation.CustomFields != null && jobOperation.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in jobOperation.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the JobOperation [{jobOperation.jmoUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the JobOperation [{jobOperation.jmoUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
