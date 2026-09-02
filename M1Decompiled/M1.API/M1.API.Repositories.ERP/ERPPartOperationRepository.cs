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

public class ERPPartOperationRepository : APIBaseRepository, IERPPartOperationRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartOperationRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartOperationExist(Guid partOperationId)
	{
		InitializeParameterLists();
		base.filterList.Add("imoUniqueID|C", partOperationId);
		base.selectList.Add("imoUniqueID");
		return Task.FromResult(GetAsObject("PartOperations", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartOperationInformationDto>> GetAllPartOperations(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartOperationInformationDto> collection = new List<ERPPartOperationInformationDto>();
		InitializeParameterLists();
		string[] array = new string[60]
		{
			"imoCreatedBy", "imoCreatedDate", "imoDocuments", "imoUniqueID", "imoEstimatedUnitCost", "imoInspectionType", "imoMachinesToSchedule", "imoMachineType", "imoMethodAssemblyID", "imoMethodID",
			"imoMethodOperationID", "imoMethodRevisionID", "imoMinimumCharge", "imoMoveTime", "imoOperationType", "imoOverlap", "imoOverlapDestinationLink", "imoOverlapOffsetTime", "imoOverlapOperationID", "imoOverlapSourceLink",
			"imoPartID", "imoPartRevisionID", "imoPlantDepartmentID", "imoPlantID", "imoProcessID", "imoProcessLongDescriptionRtf", "imoProcessLongDescriptionText", "imoProcessShortDescription", "imoProductionStandard", "imoPurchaseLocationID",
			"imoQuantityBreak1", "imoQuantityBreak2", "imoQuantityBreak3", "imoQuantityBreak4", "imoQuantityBreak5", "imoQuantityBreak6", "imoQuantityBreak7", "imoQuantityBreak8", "imoQuantityBreak9", "imoQuantityPerAssembly",
			"imoQueueTime", "imoRowVersion", "imoSetupCharge", "imoSetupHours", "imoSfeMessageRTF", "imoSfeMessageText", "imoStandardFactor", "imoSupplierOrganizationID", "imoUnitCost1", "imoUnitCost2",
			"imoUnitCost3", "imoUnitCost4", "imoUnitCost5", "imoUnitCost6", "imoUnitCost7", "imoUnitCost8", "imoUnitCost9", "imoUnitOfMeasure", "imoWorkCenterID", "imoWorkCenterMachineID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartOperations");
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
		using (DataTable dataTable = GetAsDataTable("PartOperations", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartOperationInformationDto eRPPartOperationInformationDto = new ERPPartOperationInformationDto();
				eRPPartOperationInformationDto.imoCreatedBy = dataTable.Rows[i].Field<string>("imoCreatedBy");
				eRPPartOperationInformationDto.imoCreatedDate = dataTable.Rows[i].Field<DateTime?>("imoCreatedDate");
				eRPPartOperationInformationDto.imoDocuments = dataTable.Rows[i].Field<string>("imoDocuments");
				eRPPartOperationInformationDto.imoUniqueID = dataTable.Rows[i].Field<Guid>("imoUniqueID");
				eRPPartOperationInformationDto.imoEstimatedUnitCost = dataTable.Rows[i].Field<decimal>("imoEstimatedUnitCost");
				eRPPartOperationInformationDto.imoInspectionType = dataTable.Rows[i].Field<byte>("imoInspectionType");
				eRPPartOperationInformationDto.imoMachinesToSchedule = dataTable.Rows[i].Field<short>("imoMachinesToSchedule");
				eRPPartOperationInformationDto.imoMachineType = dataTable.Rows[i].Field<byte>("imoMachineType");
				eRPPartOperationInformationDto.imoMethodAssemblyID = dataTable.Rows[i].Field<int>("imoMethodAssemblyID");
				eRPPartOperationInformationDto.imoMethodID = dataTable.Rows[i].Field<string>("imoMethodID");
				eRPPartOperationInformationDto.imoMethodOperationID = dataTable.Rows[i].Field<int>("imoMethodOperationID");
				eRPPartOperationInformationDto.imoMethodRevisionID = dataTable.Rows[i].Field<string>("imoMethodRevisionID");
				eRPPartOperationInformationDto.imoMinimumCharge = dataTable.Rows[i].Field<decimal>("imoMinimumCharge");
				eRPPartOperationInformationDto.imoMoveTime = dataTable.Rows[i].Field<decimal>("imoMoveTime");
				eRPPartOperationInformationDto.imoOperationType = dataTable.Rows[i].Field<byte>("imoOperationType");
				eRPPartOperationInformationDto.imoOverlap = dataTable.Rows[i].Field<byte>("imoOverlap");
				eRPPartOperationInformationDto.imoOverlapDestinationLink = dataTable.Rows[i].Field<byte>("imoOverlapDestinationLink");
				eRPPartOperationInformationDto.imoOverlapOffsetTime = dataTable.Rows[i].Field<decimal>("imoOverlapOffsetTime");
				eRPPartOperationInformationDto.imoOverlapOperationID = dataTable.Rows[i].Field<int>("imoOverlapOperationID");
				eRPPartOperationInformationDto.imoOverlapSourceLink = dataTable.Rows[i].Field<byte>("imoOverlapSourceLink");
				eRPPartOperationInformationDto.imoPartID = dataTable.Rows[i].Field<string>("imoPartID");
				eRPPartOperationInformationDto.imoPartRevisionID = dataTable.Rows[i].Field<string>("imoPartRevisionID");
				eRPPartOperationInformationDto.imoPlantDepartmentID = dataTable.Rows[i].Field<string>("imoPlantDepartmentID");
				eRPPartOperationInformationDto.imoPlantID = dataTable.Rows[i].Field<string>("imoPlantID");
				eRPPartOperationInformationDto.imoProcessID = dataTable.Rows[i].Field<string>("imoProcessID");
				eRPPartOperationInformationDto.imoProcessLongDescriptionRtf = dataTable.Rows[i].Field<string>("imoProcessLongDescriptionRtf");
				eRPPartOperationInformationDto.imoProcessLongDescriptionText = dataTable.Rows[i].Field<string>("imoProcessLongDescriptionText");
				eRPPartOperationInformationDto.imoProcessShortDescription = dataTable.Rows[i].Field<string>("imoProcessShortDescription");
				eRPPartOperationInformationDto.imoProductionStandard = dataTable.Rows[i].Field<decimal>("imoProductionStandard");
				eRPPartOperationInformationDto.imoPurchaseLocationID = dataTable.Rows[i].Field<string>("imoPurchaseLocationID");
				eRPPartOperationInformationDto.imoQuantityBreak1 = dataTable.Rows[i].Field<decimal>("imoQuantityBreak1");
				eRPPartOperationInformationDto.imoQuantityBreak2 = dataTable.Rows[i].Field<decimal>("imoQuantityBreak2");
				eRPPartOperationInformationDto.imoQuantityBreak3 = dataTable.Rows[i].Field<decimal>("imoQuantityBreak3");
				eRPPartOperationInformationDto.imoQuantityBreak4 = dataTable.Rows[i].Field<decimal>("imoQuantityBreak4");
				eRPPartOperationInformationDto.imoQuantityBreak5 = dataTable.Rows[i].Field<decimal>("imoQuantityBreak5");
				eRPPartOperationInformationDto.imoQuantityBreak6 = dataTable.Rows[i].Field<decimal>("imoQuantityBreak6");
				eRPPartOperationInformationDto.imoQuantityBreak7 = dataTable.Rows[i].Field<decimal>("imoQuantityBreak7");
				eRPPartOperationInformationDto.imoQuantityBreak8 = dataTable.Rows[i].Field<decimal>("imoQuantityBreak8");
				eRPPartOperationInformationDto.imoQuantityBreak9 = dataTable.Rows[i].Field<decimal>("imoQuantityBreak9");
				eRPPartOperationInformationDto.imoQuantityPerAssembly = dataTable.Rows[i].Field<decimal>("imoQuantityPerAssembly");
				eRPPartOperationInformationDto.imoQueueTime = dataTable.Rows[i].Field<decimal>("imoQueueTime");
				eRPPartOperationInformationDto.imoRowVersion = dataTable.Rows[i].Field<byte[]>("imoRowVersion");
				eRPPartOperationInformationDto.imoSetupCharge = dataTable.Rows[i].Field<decimal>("imoSetupCharge");
				eRPPartOperationInformationDto.imoSetupHours = dataTable.Rows[i].Field<decimal>("imoSetupHours");
				eRPPartOperationInformationDto.imoSfeMessageRTF = dataTable.Rows[i].Field<string>("imoSfeMessageRTF");
				eRPPartOperationInformationDto.imoSfeMessageText = dataTable.Rows[i].Field<string>("imoSfeMessageText");
				eRPPartOperationInformationDto.imoStandardFactor = dataTable.Rows[i].Field<string>("imoStandardFactor");
				eRPPartOperationInformationDto.imoSupplierOrganizationID = dataTable.Rows[i].Field<string>("imoSupplierOrganizationID");
				eRPPartOperationInformationDto.imoUnitCost1 = dataTable.Rows[i].Field<decimal>("imoUnitCost1");
				eRPPartOperationInformationDto.imoUnitCost2 = dataTable.Rows[i].Field<decimal>("imoUnitCost2");
				eRPPartOperationInformationDto.imoUnitCost3 = dataTable.Rows[i].Field<decimal>("imoUnitCost3");
				eRPPartOperationInformationDto.imoUnitCost4 = dataTable.Rows[i].Field<decimal>("imoUnitCost4");
				eRPPartOperationInformationDto.imoUnitCost5 = dataTable.Rows[i].Field<decimal>("imoUnitCost5");
				eRPPartOperationInformationDto.imoUnitCost6 = dataTable.Rows[i].Field<decimal>("imoUnitCost6");
				eRPPartOperationInformationDto.imoUnitCost7 = dataTable.Rows[i].Field<decimal>("imoUnitCost7");
				eRPPartOperationInformationDto.imoUnitCost8 = dataTable.Rows[i].Field<decimal>("imoUnitCost8");
				eRPPartOperationInformationDto.imoUnitCost9 = dataTable.Rows[i].Field<decimal>("imoUnitCost9");
				eRPPartOperationInformationDto.imoUnitOfMeasure = dataTable.Rows[i].Field<string>("imoUnitOfMeasure");
				eRPPartOperationInformationDto.imoWorkCenterID = dataTable.Rows[i].Field<string>("imoWorkCenterID");
				eRPPartOperationInformationDto.imoWorkCenterMachineID = dataTable.Rows[i].Field<short>("imoWorkCenterMachineID");
				eRPPartOperationInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartOperationInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartOperationInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartOperationInformationDto> GetPartOperation(Guid partOperationId)
	{
		ERPPartOperationInformationDto eRPPartOperationInformationDto = new ERPPartOperationInformationDto();
		InitializeParameterLists();
		string[] collection = new string[60]
		{
			"imoCreatedBy", "imoCreatedDate", "imoDocuments", "imoUniqueID", "imoEstimatedUnitCost", "imoInspectionType", "imoMachinesToSchedule", "imoMachineType", "imoMethodAssemblyID", "imoMethodID",
			"imoMethodOperationID", "imoMethodRevisionID", "imoMinimumCharge", "imoMoveTime", "imoOperationType", "imoOverlap", "imoOverlapDestinationLink", "imoOverlapOffsetTime", "imoOverlapOperationID", "imoOverlapSourceLink",
			"imoPartID", "imoPartRevisionID", "imoPlantDepartmentID", "imoPlantID", "imoProcessID", "imoProcessLongDescriptionRtf", "imoProcessLongDescriptionText", "imoProcessShortDescription", "imoProductionStandard", "imoPurchaseLocationID",
			"imoQuantityBreak1", "imoQuantityBreak2", "imoQuantityBreak3", "imoQuantityBreak4", "imoQuantityBreak5", "imoQuantityBreak6", "imoQuantityBreak7", "imoQuantityBreak8", "imoQuantityBreak9", "imoQuantityPerAssembly",
			"imoQueueTime", "imoRowVersion", "imoSetupCharge", "imoSetupHours", "imoSfeMessageRTF", "imoSfeMessageText", "imoStandardFactor", "imoSupplierOrganizationID", "imoUnitCost1", "imoUnitCost2",
			"imoUnitCost3", "imoUnitCost4", "imoUnitCost5", "imoUnitCost6", "imoUnitCost7", "imoUnitCost8", "imoUnitCost9", "imoUnitOfMeasure", "imoWorkCenterID", "imoWorkCenterMachineID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imoUniqueID|C", partOperationId);
		AddCustomFieldsToSelectList("PartOperations");
		using (DataTable dataTable = GetAsDataTable("PartOperations", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartOperationInformationDto);
			}
			eRPPartOperationInformationDto.imoCreatedBy = dataTable.Rows[0].Field<string>("imoCreatedBy");
			eRPPartOperationInformationDto.imoCreatedDate = dataTable.Rows[0].Field<DateTime?>("imoCreatedDate");
			eRPPartOperationInformationDto.imoDocuments = dataTable.Rows[0].Field<string>("imoDocuments");
			eRPPartOperationInformationDto.imoUniqueID = dataTable.Rows[0].Field<Guid>("imoUniqueID");
			eRPPartOperationInformationDto.imoEstimatedUnitCost = dataTable.Rows[0].Field<decimal>("imoEstimatedUnitCost");
			eRPPartOperationInformationDto.imoInspectionType = dataTable.Rows[0].Field<byte>("imoInspectionType");
			eRPPartOperationInformationDto.imoMachinesToSchedule = dataTable.Rows[0].Field<short>("imoMachinesToSchedule");
			eRPPartOperationInformationDto.imoMachineType = dataTable.Rows[0].Field<byte>("imoMachineType");
			eRPPartOperationInformationDto.imoMethodAssemblyID = dataTable.Rows[0].Field<int>("imoMethodAssemblyID");
			eRPPartOperationInformationDto.imoMethodID = dataTable.Rows[0].Field<string>("imoMethodID");
			eRPPartOperationInformationDto.imoMethodOperationID = dataTable.Rows[0].Field<int>("imoMethodOperationID");
			eRPPartOperationInformationDto.imoMethodRevisionID = dataTable.Rows[0].Field<string>("imoMethodRevisionID");
			eRPPartOperationInformationDto.imoMinimumCharge = dataTable.Rows[0].Field<decimal>("imoMinimumCharge");
			eRPPartOperationInformationDto.imoMoveTime = dataTable.Rows[0].Field<decimal>("imoMoveTime");
			eRPPartOperationInformationDto.imoOperationType = dataTable.Rows[0].Field<byte>("imoOperationType");
			eRPPartOperationInformationDto.imoOverlap = dataTable.Rows[0].Field<byte>("imoOverlap");
			eRPPartOperationInformationDto.imoOverlapDestinationLink = dataTable.Rows[0].Field<byte>("imoOverlapDestinationLink");
			eRPPartOperationInformationDto.imoOverlapOffsetTime = dataTable.Rows[0].Field<decimal>("imoOverlapOffsetTime");
			eRPPartOperationInformationDto.imoOverlapOperationID = dataTable.Rows[0].Field<int>("imoOverlapOperationID");
			eRPPartOperationInformationDto.imoOverlapSourceLink = dataTable.Rows[0].Field<byte>("imoOverlapSourceLink");
			eRPPartOperationInformationDto.imoPartID = dataTable.Rows[0].Field<string>("imoPartID");
			eRPPartOperationInformationDto.imoPartRevisionID = dataTable.Rows[0].Field<string>("imoPartRevisionID");
			eRPPartOperationInformationDto.imoPlantDepartmentID = dataTable.Rows[0].Field<string>("imoPlantDepartmentID");
			eRPPartOperationInformationDto.imoPlantID = dataTable.Rows[0].Field<string>("imoPlantID");
			eRPPartOperationInformationDto.imoProcessID = dataTable.Rows[0].Field<string>("imoProcessID");
			eRPPartOperationInformationDto.imoProcessLongDescriptionRtf = dataTable.Rows[0].Field<string>("imoProcessLongDescriptionRtf");
			eRPPartOperationInformationDto.imoProcessLongDescriptionText = dataTable.Rows[0].Field<string>("imoProcessLongDescriptionText");
			eRPPartOperationInformationDto.imoProcessShortDescription = dataTable.Rows[0].Field<string>("imoProcessShortDescription");
			eRPPartOperationInformationDto.imoProductionStandard = dataTable.Rows[0].Field<decimal>("imoProductionStandard");
			eRPPartOperationInformationDto.imoPurchaseLocationID = dataTable.Rows[0].Field<string>("imoPurchaseLocationID");
			eRPPartOperationInformationDto.imoQuantityBreak1 = dataTable.Rows[0].Field<decimal>("imoQuantityBreak1");
			eRPPartOperationInformationDto.imoQuantityBreak2 = dataTable.Rows[0].Field<decimal>("imoQuantityBreak2");
			eRPPartOperationInformationDto.imoQuantityBreak3 = dataTable.Rows[0].Field<decimal>("imoQuantityBreak3");
			eRPPartOperationInformationDto.imoQuantityBreak4 = dataTable.Rows[0].Field<decimal>("imoQuantityBreak4");
			eRPPartOperationInformationDto.imoQuantityBreak5 = dataTable.Rows[0].Field<decimal>("imoQuantityBreak5");
			eRPPartOperationInformationDto.imoQuantityBreak6 = dataTable.Rows[0].Field<decimal>("imoQuantityBreak6");
			eRPPartOperationInformationDto.imoQuantityBreak7 = dataTable.Rows[0].Field<decimal>("imoQuantityBreak7");
			eRPPartOperationInformationDto.imoQuantityBreak8 = dataTable.Rows[0].Field<decimal>("imoQuantityBreak8");
			eRPPartOperationInformationDto.imoQuantityBreak9 = dataTable.Rows[0].Field<decimal>("imoQuantityBreak9");
			eRPPartOperationInformationDto.imoQuantityPerAssembly = dataTable.Rows[0].Field<decimal>("imoQuantityPerAssembly");
			eRPPartOperationInformationDto.imoQueueTime = dataTable.Rows[0].Field<decimal>("imoQueueTime");
			eRPPartOperationInformationDto.imoRowVersion = dataTable.Rows[0].Field<byte[]>("imoRowVersion");
			eRPPartOperationInformationDto.imoSetupCharge = dataTable.Rows[0].Field<decimal>("imoSetupCharge");
			eRPPartOperationInformationDto.imoSetupHours = dataTable.Rows[0].Field<decimal>("imoSetupHours");
			eRPPartOperationInformationDto.imoSfeMessageRTF = dataTable.Rows[0].Field<string>("imoSfeMessageRTF");
			eRPPartOperationInformationDto.imoSfeMessageText = dataTable.Rows[0].Field<string>("imoSfeMessageText");
			eRPPartOperationInformationDto.imoStandardFactor = dataTable.Rows[0].Field<string>("imoStandardFactor");
			eRPPartOperationInformationDto.imoSupplierOrganizationID = dataTable.Rows[0].Field<string>("imoSupplierOrganizationID");
			eRPPartOperationInformationDto.imoUnitCost1 = dataTable.Rows[0].Field<decimal>("imoUnitCost1");
			eRPPartOperationInformationDto.imoUnitCost2 = dataTable.Rows[0].Field<decimal>("imoUnitCost2");
			eRPPartOperationInformationDto.imoUnitCost3 = dataTable.Rows[0].Field<decimal>("imoUnitCost3");
			eRPPartOperationInformationDto.imoUnitCost4 = dataTable.Rows[0].Field<decimal>("imoUnitCost4");
			eRPPartOperationInformationDto.imoUnitCost5 = dataTable.Rows[0].Field<decimal>("imoUnitCost5");
			eRPPartOperationInformationDto.imoUnitCost6 = dataTable.Rows[0].Field<decimal>("imoUnitCost6");
			eRPPartOperationInformationDto.imoUnitCost7 = dataTable.Rows[0].Field<decimal>("imoUnitCost7");
			eRPPartOperationInformationDto.imoUnitCost8 = dataTable.Rows[0].Field<decimal>("imoUnitCost8");
			eRPPartOperationInformationDto.imoUnitCost9 = dataTable.Rows[0].Field<decimal>("imoUnitCost9");
			eRPPartOperationInformationDto.imoUnitOfMeasure = dataTable.Rows[0].Field<string>("imoUnitOfMeasure");
			eRPPartOperationInformationDto.imoWorkCenterID = dataTable.Rows[0].Field<string>("imoWorkCenterID");
			eRPPartOperationInformationDto.imoWorkCenterMachineID = dataTable.Rows[0].Field<short>("imoWorkCenterMachineID");
			eRPPartOperationInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartOperationInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartOperationInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartOperation(ERPPartOperationDto partOperation)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartOperations WHERE imoUniqueID = " + M1Util.ConvertToLinq(partOperation.imoUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imoMethodID"] = partOperation.imoMethodID.ToUpper();
				dataRow["imoMethodRevisionID"] = partOperation.imoMethodRevisionID.ToUpper();
				dataRow["imoMethodAssemblyID"] = partOperation.imoMethodAssemblyID;
				dataRow["imoMethodOperationID"] = partOperation.imoMethodOperationID;
				partOperation.imoUniqueID = ((partOperation.imoUniqueID == Guid.Empty) ? Guid.NewGuid() : partOperation.imoUniqueID);
				dataRow["imoUniqueID"] = partOperation.imoUniqueID;
				dataRow["imoCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imoCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartOperation could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partOperation.imoRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartOperation is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imoRowVersion"], partOperation.imoRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartOperation has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartOperation again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imoDocuments"] = partOperation.imoDocuments ?? dataRow["imoDocuments"];
			dataRow["imoEstimatedUnitCost"] = partOperation.imoEstimatedUnitCost;
			dataRow["imoInspectionType"] = partOperation.imoInspectionType;
			dataRow["imoMachinesToSchedule"] = partOperation.imoMachinesToSchedule;
			dataRow["imoMachineType"] = partOperation.imoMachineType;
			dataRow["imoMinimumCharge"] = partOperation.imoMinimumCharge;
			dataRow["imoMoveTime"] = partOperation.imoMoveTime;
			dataRow["imoOperationType"] = partOperation.imoOperationType;
			dataRow["imoOverlap"] = partOperation.imoOverlap;
			dataRow["imoOverlapDestinationLink"] = partOperation.imoOverlapDestinationLink;
			dataRow["imoOverlapOffsetTime"] = partOperation.imoOverlapOffsetTime;
			dataRow["imoOverlapOperationID"] = partOperation.imoOverlapOperationID;
			dataRow["imoOverlapSourceLink"] = partOperation.imoOverlapSourceLink;
			dataRow["imoPartID"] = partOperation.imoPartID;
			dataRow["imoPartRevisionID"] = partOperation.imoPartRevisionID;
			dataRow["imoPlantDepartmentID"] = partOperation.imoPlantDepartmentID;
			dataRow["imoPlantID"] = partOperation.imoPlantID;
			dataRow["imoProcessID"] = partOperation.imoProcessID;
			dataRow["imoProcessLongDescriptionRtf"] = partOperation.imoProcessLongDescriptionRtf ?? dataRow["imoProcessLongDescriptionRtf"];
			dataRow["imoProcessLongDescriptionText"] = partOperation.imoProcessLongDescriptionText ?? dataRow["imoProcessLongDescriptionText"];
			dataRow["imoProcessShortDescription"] = partOperation.imoProcessShortDescription;
			dataRow["imoProductionStandard"] = partOperation.imoProductionStandard;
			dataRow["imoPurchaseLocationID"] = partOperation.imoPurchaseLocationID;
			dataRow["imoQuantityBreak1"] = partOperation.imoQuantityBreak1;
			dataRow["imoQuantityBreak2"] = partOperation.imoQuantityBreak2;
			dataRow["imoQuantityBreak3"] = partOperation.imoQuantityBreak3;
			dataRow["imoQuantityBreak4"] = partOperation.imoQuantityBreak4;
			dataRow["imoQuantityBreak5"] = partOperation.imoQuantityBreak5;
			dataRow["imoQuantityBreak6"] = partOperation.imoQuantityBreak6;
			dataRow["imoQuantityBreak7"] = partOperation.imoQuantityBreak7;
			dataRow["imoQuantityBreak8"] = partOperation.imoQuantityBreak8;
			dataRow["imoQuantityBreak9"] = partOperation.imoQuantityBreak9;
			dataRow["imoQuantityPerAssembly"] = partOperation.imoQuantityPerAssembly;
			dataRow["imoQueueTime"] = partOperation.imoQueueTime;
			dataRow["imoSetupCharge"] = partOperation.imoSetupCharge;
			dataRow["imoSetupHours"] = partOperation.imoSetupHours;
			dataRow["imoSfeMessageRTF"] = partOperation.imoSfeMessageRTF ?? dataRow["imoSfeMessageRTF"];
			dataRow["imoSfeMessageText"] = partOperation.imoSfeMessageText ?? dataRow["imoSfeMessageText"];
			dataRow["imoStandardFactor"] = partOperation.imoStandardFactor;
			dataRow["imoSupplierOrganizationID"] = partOperation.imoSupplierOrganizationID;
			dataRow["imoUnitCost1"] = partOperation.imoUnitCost1;
			dataRow["imoUnitCost2"] = partOperation.imoUnitCost2;
			dataRow["imoUnitCost3"] = partOperation.imoUnitCost3;
			dataRow["imoUnitCost4"] = partOperation.imoUnitCost4;
			dataRow["imoUnitCost5"] = partOperation.imoUnitCost5;
			dataRow["imoUnitCost6"] = partOperation.imoUnitCost6;
			dataRow["imoUnitCost7"] = partOperation.imoUnitCost7;
			dataRow["imoUnitCost8"] = partOperation.imoUnitCost8;
			dataRow["imoUnitCost9"] = partOperation.imoUnitCost9;
			dataRow["imoUnitOfMeasure"] = partOperation.imoUnitOfMeasure;
			dataRow["imoWorkCenterID"] = partOperation.imoWorkCenterID;
			dataRow["imoWorkCenterMachineID"] = partOperation.imoWorkCenterMachineID;
			if (partOperation.CustomFields != null && partOperation.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partOperation.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartOperation [{partOperation.imoUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartOperation [{partOperation.imoUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
