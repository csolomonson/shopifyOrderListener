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

public class ERPInspectionLineRepository : APIBaseRepository, IERPInspectionLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPInspectionLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesInspectionLineExist(Guid inspectionLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("qalUniqueID|C", inspectionLineId);
		base.selectList.Add("qalUniqueID");
		return Task.FromResult(GetAsObject("InspectionLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPInspectionLineInformationDto>> GetAllInspectionLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPInspectionLineInformationDto> collection = new List<ERPInspectionLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[65]
		{
			"qalActionType", "qalApprovalDecisionDate", "qalApprovalRequestDate", "qalApprovalStatus", "qalClosedDate", "qalCreatedBy", "qalCreatedDate", "qalUniqueID", "qalInspectionDate", "qalInspectionID",
			"qalInspectionNotesRTF", "qalInspectionNotesText", "qalInspectionType", "qalInspectorEmployeeID", "qalInvQuantityAccepted", "qalInvQuantityToReturn", "qalInvQuantityToScrap", "qalFirstOffInspection", "qalInspectionComplete", "qalKitPart",
			"qalManualInspectionFinalized", "qalPosted", "qalReturnToSupplier", "qalReversed", "qalTransferredToDmr", "qalJobAssemblyID", "qalJobID", "qalJobMaterialID", "qalJobMatQuantityAccepted", "qalJobMatQuantityRejected",
			"qalJobMatQuantityToReturn", "qalJobMatQuantityToScrap", "qalJobOperationID", "qalJobOprQuantityAccepted", "qalJobOprQuantityRejected", "qalJobOprQuantityToReturn", "qalJobOprQuantityToScrap", "qalJobType", "qalMfgReceiptQuantityAccepted", "qalMfgReceiptQuantityToReturn",
			"qalMfgReceiptQuantityToScrap", "qalNextApprovalEmployeeID", "qalPartBinID", "qalPartID", "qalPartLongDescriptionRtf", "qalPartLongDescriptionText", "qalPartRevisionID", "qalPartShortDescription", "qalPartTransactionID", "qalPartWarehouseLocationID",
			"qalProjectAreaID", "qalProjectID", "qalPurchaseLocationID", "qalQuantityRejected", "qalQuantityToInspect", "qalReverseInspectionID", "qalReverseInspectionLineID", "qalScrapReasonID", "qalInspectionLineID", "qalSourceTableName",
			"qalSourceTableUniqueID", "qalStatus", "qalSupplierOrganizationID", "qalUnitCost", "qalUnitOfMeasure"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("InspectionLines");
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
		using (DataTable dataTable = GetAsDataTable("InspectionLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPInspectionLineInformationDto eRPInspectionLineInformationDto = new ERPInspectionLineInformationDto();
				eRPInspectionLineInformationDto.qalActionType = dataTable.Rows[i].Field<byte>("qalActionType");
				eRPInspectionLineInformationDto.qalApprovalDecisionDate = dataTable.Rows[i].Field<DateTime?>("qalApprovalDecisionDate");
				eRPInspectionLineInformationDto.qalApprovalRequestDate = dataTable.Rows[i].Field<DateTime?>("qalApprovalRequestDate");
				eRPInspectionLineInformationDto.qalApprovalStatus = dataTable.Rows[i].Field<byte>("qalApprovalStatus");
				eRPInspectionLineInformationDto.qalClosedDate = dataTable.Rows[i].Field<DateTime?>("qalClosedDate");
				eRPInspectionLineInformationDto.qalCreatedBy = dataTable.Rows[i].Field<string>("qalCreatedBy");
				eRPInspectionLineInformationDto.qalCreatedDate = dataTable.Rows[i].Field<DateTime?>("qalCreatedDate");
				eRPInspectionLineInformationDto.qalUniqueID = dataTable.Rows[i].Field<Guid>("qalUniqueID");
				eRPInspectionLineInformationDto.qalInspectionDate = dataTable.Rows[i].Field<DateTime?>("qalInspectionDate");
				eRPInspectionLineInformationDto.qalInspectionID = dataTable.Rows[i].Field<string>("qalInspectionID");
				eRPInspectionLineInformationDto.qalInspectionNotesRTF = dataTable.Rows[i].Field<string>("qalInspectionNotesRTF");
				eRPInspectionLineInformationDto.qalInspectionNotesText = dataTable.Rows[i].Field<string>("qalInspectionNotesText");
				eRPInspectionLineInformationDto.qalInspectionType = dataTable.Rows[i].Field<byte>("qalInspectionType");
				eRPInspectionLineInformationDto.qalInspectorEmployeeID = dataTable.Rows[i].Field<string>("qalInspectorEmployeeID");
				eRPInspectionLineInformationDto.qalInvQuantityAccepted = dataTable.Rows[i].Field<decimal>("qalInvQuantityAccepted");
				eRPInspectionLineInformationDto.qalInvQuantityToReturn = dataTable.Rows[i].Field<decimal>("qalInvQuantityToReturn");
				eRPInspectionLineInformationDto.qalInvQuantityToScrap = dataTable.Rows[i].Field<decimal>("qalInvQuantityToScrap");
				eRPInspectionLineInformationDto.qalFirstOffInspection = dataTable.Rows[i].Field<bool>("qalFirstOffInspection");
				eRPInspectionLineInformationDto.qalInspectionComplete = dataTable.Rows[i].Field<bool>("qalInspectionComplete");
				eRPInspectionLineInformationDto.qalKitPart = dataTable.Rows[i].Field<bool>("qalKitPart");
				eRPInspectionLineInformationDto.qalManualInspectionFinalized = dataTable.Rows[i].Field<bool>("qalManualInspectionFinalized");
				eRPInspectionLineInformationDto.qalPosted = dataTable.Rows[i].Field<bool>("qalPosted");
				eRPInspectionLineInformationDto.qalReturnToSupplier = dataTable.Rows[i].Field<bool>("qalReturnToSupplier");
				eRPInspectionLineInformationDto.qalReversed = dataTable.Rows[i].Field<bool>("qalReversed");
				eRPInspectionLineInformationDto.qalTransferredToDmr = dataTable.Rows[i].Field<bool>("qalTransferredToDmr");
				eRPInspectionLineInformationDto.qalJobAssemblyID = dataTable.Rows[i].Field<int>("qalJobAssemblyID");
				eRPInspectionLineInformationDto.qalJobID = dataTable.Rows[i].Field<string>("qalJobID");
				eRPInspectionLineInformationDto.qalJobMaterialID = dataTable.Rows[i].Field<int>("qalJobMaterialID");
				eRPInspectionLineInformationDto.qalJobMatQuantityAccepted = dataTable.Rows[i].Field<decimal>("qalJobMatQuantityAccepted");
				eRPInspectionLineInformationDto.qalJobMatQuantityRejected = dataTable.Rows[i].Field<decimal>("qalJobMatQuantityRejected");
				eRPInspectionLineInformationDto.qalJobMatQuantityToReturn = dataTable.Rows[i].Field<decimal>("qalJobMatQuantityToReturn");
				eRPInspectionLineInformationDto.qalJobMatQuantityToScrap = dataTable.Rows[i].Field<decimal>("qalJobMatQuantityToScrap");
				eRPInspectionLineInformationDto.qalJobOperationID = dataTable.Rows[i].Field<int>("qalJobOperationID");
				eRPInspectionLineInformationDto.qalJobOprQuantityAccepted = dataTable.Rows[i].Field<decimal>("qalJobOprQuantityAccepted");
				eRPInspectionLineInformationDto.qalJobOprQuantityRejected = dataTable.Rows[i].Field<decimal>("qalJobOprQuantityRejected");
				eRPInspectionLineInformationDto.qalJobOprQuantityToReturn = dataTable.Rows[i].Field<decimal>("qalJobOprQuantityToReturn");
				eRPInspectionLineInformationDto.qalJobOprQuantityToScrap = dataTable.Rows[i].Field<decimal>("qalJobOprQuantityToScrap");
				eRPInspectionLineInformationDto.qalJobType = dataTable.Rows[i].Field<byte>("qalJobType");
				eRPInspectionLineInformationDto.qalMfgReceiptQuantityAccepted = dataTable.Rows[i].Field<decimal>("qalMfgReceiptQuantityAccepted");
				eRPInspectionLineInformationDto.qalMfgReceiptQuantityToReturn = dataTable.Rows[i].Field<decimal>("qalMfgReceiptQuantityToReturn");
				eRPInspectionLineInformationDto.qalMfgReceiptQuantityToScrap = dataTable.Rows[i].Field<decimal>("qalMfgReceiptQuantityToScrap");
				eRPInspectionLineInformationDto.qalNextApprovalEmployeeID = dataTable.Rows[i].Field<string>("qalNextApprovalEmployeeID");
				eRPInspectionLineInformationDto.qalPartBinID = dataTable.Rows[i].Field<string>("qalPartBinID");
				eRPInspectionLineInformationDto.qalPartID = dataTable.Rows[i].Field<string>("qalPartID");
				eRPInspectionLineInformationDto.qalPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("qalPartLongDescriptionRtf");
				eRPInspectionLineInformationDto.qalPartLongDescriptionText = dataTable.Rows[i].Field<string>("qalPartLongDescriptionText");
				eRPInspectionLineInformationDto.qalPartRevisionID = dataTable.Rows[i].Field<string>("qalPartRevisionID");
				eRPInspectionLineInformationDto.qalPartShortDescription = dataTable.Rows[i].Field<string>("qalPartShortDescription");
				eRPInspectionLineInformationDto.qalPartTransactionID = dataTable.Rows[i].Field<int>("qalPartTransactionID");
				eRPInspectionLineInformationDto.qalPartWarehouseLocationID = dataTable.Rows[i].Field<string>("qalPartWarehouseLocationID");
				eRPInspectionLineInformationDto.qalProjectAreaID = dataTable.Rows[i].Field<string>("qalProjectAreaID");
				eRPInspectionLineInformationDto.qalProjectID = dataTable.Rows[i].Field<string>("qalProjectID");
				eRPInspectionLineInformationDto.qalPurchaseLocationID = dataTable.Rows[i].Field<string>("qalPurchaseLocationID");
				eRPInspectionLineInformationDto.qalQuantityRejected = dataTable.Rows[i].Field<decimal>("qalQuantityRejected");
				eRPInspectionLineInformationDto.qalQuantityToInspect = dataTable.Rows[i].Field<decimal>("qalQuantityToInspect");
				eRPInspectionLineInformationDto.qalReverseInspectionID = dataTable.Rows[i].Field<string>("qalReverseInspectionID");
				eRPInspectionLineInformationDto.qalReverseInspectionLineID = dataTable.Rows[i].Field<short>("qalReverseInspectionLineID");
				eRPInspectionLineInformationDto.qalScrapReasonID = dataTable.Rows[i].Field<string>("qalScrapReasonID");
				eRPInspectionLineInformationDto.qalInspectionLineID = dataTable.Rows[i].Field<short>("qalInspectionLineID");
				eRPInspectionLineInformationDto.qalSourceTableName = dataTable.Rows[i].Field<string>("qalSourceTableName");
				eRPInspectionLineInformationDto.qalSourceTableUniqueID = dataTable.Rows[i].Field<Guid>("qalSourceTableUniqueID");
				eRPInspectionLineInformationDto.qalStatus = dataTable.Rows[i].Field<string>("qalStatus");
				eRPInspectionLineInformationDto.qalSupplierOrganizationID = dataTable.Rows[i].Field<string>("qalSupplierOrganizationID");
				eRPInspectionLineInformationDto.qalUnitCost = dataTable.Rows[i].Field<decimal>("qalUnitCost");
				eRPInspectionLineInformationDto.qalUnitOfMeasure = dataTable.Rows[i].Field<string>("qalUnitOfMeasure");
				eRPInspectionLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPInspectionLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPInspectionLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPInspectionLineInformationDto> GetInspectionLine(Guid inspectionLineId)
	{
		ERPInspectionLineInformationDto eRPInspectionLineInformationDto = new ERPInspectionLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[65]
		{
			"qalActionType", "qalApprovalDecisionDate", "qalApprovalRequestDate", "qalApprovalStatus", "qalClosedDate", "qalCreatedBy", "qalCreatedDate", "qalUniqueID", "qalInspectionDate", "qalInspectionID",
			"qalInspectionNotesRTF", "qalInspectionNotesText", "qalInspectionType", "qalInspectorEmployeeID", "qalInvQuantityAccepted", "qalInvQuantityToReturn", "qalInvQuantityToScrap", "qalFirstOffInspection", "qalInspectionComplete", "qalKitPart",
			"qalManualInspectionFinalized", "qalPosted", "qalReturnToSupplier", "qalReversed", "qalTransferredToDmr", "qalJobAssemblyID", "qalJobID", "qalJobMaterialID", "qalJobMatQuantityAccepted", "qalJobMatQuantityRejected",
			"qalJobMatQuantityToReturn", "qalJobMatQuantityToScrap", "qalJobOperationID", "qalJobOprQuantityAccepted", "qalJobOprQuantityRejected", "qalJobOprQuantityToReturn", "qalJobOprQuantityToScrap", "qalJobType", "qalMfgReceiptQuantityAccepted", "qalMfgReceiptQuantityToReturn",
			"qalMfgReceiptQuantityToScrap", "qalNextApprovalEmployeeID", "qalPartBinID", "qalPartID", "qalPartLongDescriptionRtf", "qalPartLongDescriptionText", "qalPartRevisionID", "qalPartShortDescription", "qalPartTransactionID", "qalPartWarehouseLocationID",
			"qalProjectAreaID", "qalProjectID", "qalPurchaseLocationID", "qalQuantityRejected", "qalQuantityToInspect", "qalReverseInspectionID", "qalReverseInspectionLineID", "qalScrapReasonID", "qalInspectionLineID", "qalSourceTableName",
			"qalSourceTableUniqueID", "qalStatus", "qalSupplierOrganizationID", "qalUnitCost", "qalUnitOfMeasure"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("qalUniqueID|C", inspectionLineId);
		AddCustomFieldsToSelectList("InspectionLines");
		using (DataTable dataTable = GetAsDataTable("InspectionLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPInspectionLineInformationDto);
			}
			eRPInspectionLineInformationDto.qalActionType = dataTable.Rows[0].Field<byte>("qalActionType");
			eRPInspectionLineInformationDto.qalApprovalDecisionDate = dataTable.Rows[0].Field<DateTime?>("qalApprovalDecisionDate");
			eRPInspectionLineInformationDto.qalApprovalRequestDate = dataTable.Rows[0].Field<DateTime?>("qalApprovalRequestDate");
			eRPInspectionLineInformationDto.qalApprovalStatus = dataTable.Rows[0].Field<byte>("qalApprovalStatus");
			eRPInspectionLineInformationDto.qalClosedDate = dataTable.Rows[0].Field<DateTime?>("qalClosedDate");
			eRPInspectionLineInformationDto.qalCreatedBy = dataTable.Rows[0].Field<string>("qalCreatedBy");
			eRPInspectionLineInformationDto.qalCreatedDate = dataTable.Rows[0].Field<DateTime?>("qalCreatedDate");
			eRPInspectionLineInformationDto.qalUniqueID = dataTable.Rows[0].Field<Guid>("qalUniqueID");
			eRPInspectionLineInformationDto.qalInspectionDate = dataTable.Rows[0].Field<DateTime?>("qalInspectionDate");
			eRPInspectionLineInformationDto.qalInspectionID = dataTable.Rows[0].Field<string>("qalInspectionID");
			eRPInspectionLineInformationDto.qalInspectionNotesRTF = dataTable.Rows[0].Field<string>("qalInspectionNotesRTF");
			eRPInspectionLineInformationDto.qalInspectionNotesText = dataTable.Rows[0].Field<string>("qalInspectionNotesText");
			eRPInspectionLineInformationDto.qalInspectionType = dataTable.Rows[0].Field<byte>("qalInspectionType");
			eRPInspectionLineInformationDto.qalInspectorEmployeeID = dataTable.Rows[0].Field<string>("qalInspectorEmployeeID");
			eRPInspectionLineInformationDto.qalInvQuantityAccepted = dataTable.Rows[0].Field<decimal>("qalInvQuantityAccepted");
			eRPInspectionLineInformationDto.qalInvQuantityToReturn = dataTable.Rows[0].Field<decimal>("qalInvQuantityToReturn");
			eRPInspectionLineInformationDto.qalInvQuantityToScrap = dataTable.Rows[0].Field<decimal>("qalInvQuantityToScrap");
			eRPInspectionLineInformationDto.qalFirstOffInspection = dataTable.Rows[0].Field<bool>("qalFirstOffInspection");
			eRPInspectionLineInformationDto.qalInspectionComplete = dataTable.Rows[0].Field<bool>("qalInspectionComplete");
			eRPInspectionLineInformationDto.qalKitPart = dataTable.Rows[0].Field<bool>("qalKitPart");
			eRPInspectionLineInformationDto.qalManualInspectionFinalized = dataTable.Rows[0].Field<bool>("qalManualInspectionFinalized");
			eRPInspectionLineInformationDto.qalPosted = dataTable.Rows[0].Field<bool>("qalPosted");
			eRPInspectionLineInformationDto.qalReturnToSupplier = dataTable.Rows[0].Field<bool>("qalReturnToSupplier");
			eRPInspectionLineInformationDto.qalReversed = dataTable.Rows[0].Field<bool>("qalReversed");
			eRPInspectionLineInformationDto.qalTransferredToDmr = dataTable.Rows[0].Field<bool>("qalTransferredToDmr");
			eRPInspectionLineInformationDto.qalJobAssemblyID = dataTable.Rows[0].Field<int>("qalJobAssemblyID");
			eRPInspectionLineInformationDto.qalJobID = dataTable.Rows[0].Field<string>("qalJobID");
			eRPInspectionLineInformationDto.qalJobMaterialID = dataTable.Rows[0].Field<int>("qalJobMaterialID");
			eRPInspectionLineInformationDto.qalJobMatQuantityAccepted = dataTable.Rows[0].Field<decimal>("qalJobMatQuantityAccepted");
			eRPInspectionLineInformationDto.qalJobMatQuantityRejected = dataTable.Rows[0].Field<decimal>("qalJobMatQuantityRejected");
			eRPInspectionLineInformationDto.qalJobMatQuantityToReturn = dataTable.Rows[0].Field<decimal>("qalJobMatQuantityToReturn");
			eRPInspectionLineInformationDto.qalJobMatQuantityToScrap = dataTable.Rows[0].Field<decimal>("qalJobMatQuantityToScrap");
			eRPInspectionLineInformationDto.qalJobOperationID = dataTable.Rows[0].Field<int>("qalJobOperationID");
			eRPInspectionLineInformationDto.qalJobOprQuantityAccepted = dataTable.Rows[0].Field<decimal>("qalJobOprQuantityAccepted");
			eRPInspectionLineInformationDto.qalJobOprQuantityRejected = dataTable.Rows[0].Field<decimal>("qalJobOprQuantityRejected");
			eRPInspectionLineInformationDto.qalJobOprQuantityToReturn = dataTable.Rows[0].Field<decimal>("qalJobOprQuantityToReturn");
			eRPInspectionLineInformationDto.qalJobOprQuantityToScrap = dataTable.Rows[0].Field<decimal>("qalJobOprQuantityToScrap");
			eRPInspectionLineInformationDto.qalJobType = dataTable.Rows[0].Field<byte>("qalJobType");
			eRPInspectionLineInformationDto.qalMfgReceiptQuantityAccepted = dataTable.Rows[0].Field<decimal>("qalMfgReceiptQuantityAccepted");
			eRPInspectionLineInformationDto.qalMfgReceiptQuantityToReturn = dataTable.Rows[0].Field<decimal>("qalMfgReceiptQuantityToReturn");
			eRPInspectionLineInformationDto.qalMfgReceiptQuantityToScrap = dataTable.Rows[0].Field<decimal>("qalMfgReceiptQuantityToScrap");
			eRPInspectionLineInformationDto.qalNextApprovalEmployeeID = dataTable.Rows[0].Field<string>("qalNextApprovalEmployeeID");
			eRPInspectionLineInformationDto.qalPartBinID = dataTable.Rows[0].Field<string>("qalPartBinID");
			eRPInspectionLineInformationDto.qalPartID = dataTable.Rows[0].Field<string>("qalPartID");
			eRPInspectionLineInformationDto.qalPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("qalPartLongDescriptionRtf");
			eRPInspectionLineInformationDto.qalPartLongDescriptionText = dataTable.Rows[0].Field<string>("qalPartLongDescriptionText");
			eRPInspectionLineInformationDto.qalPartRevisionID = dataTable.Rows[0].Field<string>("qalPartRevisionID");
			eRPInspectionLineInformationDto.qalPartShortDescription = dataTable.Rows[0].Field<string>("qalPartShortDescription");
			eRPInspectionLineInformationDto.qalPartTransactionID = dataTable.Rows[0].Field<int>("qalPartTransactionID");
			eRPInspectionLineInformationDto.qalPartWarehouseLocationID = dataTable.Rows[0].Field<string>("qalPartWarehouseLocationID");
			eRPInspectionLineInformationDto.qalProjectAreaID = dataTable.Rows[0].Field<string>("qalProjectAreaID");
			eRPInspectionLineInformationDto.qalProjectID = dataTable.Rows[0].Field<string>("qalProjectID");
			eRPInspectionLineInformationDto.qalPurchaseLocationID = dataTable.Rows[0].Field<string>("qalPurchaseLocationID");
			eRPInspectionLineInformationDto.qalQuantityRejected = dataTable.Rows[0].Field<decimal>("qalQuantityRejected");
			eRPInspectionLineInformationDto.qalQuantityToInspect = dataTable.Rows[0].Field<decimal>("qalQuantityToInspect");
			eRPInspectionLineInformationDto.qalReverseInspectionID = dataTable.Rows[0].Field<string>("qalReverseInspectionID");
			eRPInspectionLineInformationDto.qalReverseInspectionLineID = dataTable.Rows[0].Field<short>("qalReverseInspectionLineID");
			eRPInspectionLineInformationDto.qalScrapReasonID = dataTable.Rows[0].Field<string>("qalScrapReasonID");
			eRPInspectionLineInformationDto.qalInspectionLineID = dataTable.Rows[0].Field<short>("qalInspectionLineID");
			eRPInspectionLineInformationDto.qalSourceTableName = dataTable.Rows[0].Field<string>("qalSourceTableName");
			eRPInspectionLineInformationDto.qalSourceTableUniqueID = dataTable.Rows[0].Field<Guid>("qalSourceTableUniqueID");
			eRPInspectionLineInformationDto.qalStatus = dataTable.Rows[0].Field<string>("qalStatus");
			eRPInspectionLineInformationDto.qalSupplierOrganizationID = dataTable.Rows[0].Field<string>("qalSupplierOrganizationID");
			eRPInspectionLineInformationDto.qalUnitCost = dataTable.Rows[0].Field<decimal>("qalUnitCost");
			eRPInspectionLineInformationDto.qalUnitOfMeasure = dataTable.Rows[0].Field<string>("qalUnitOfMeasure");
			eRPInspectionLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPInspectionLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPInspectionLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveInspectionLine(ERPInspectionLineDto inspectionLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM InspectionLines WHERE qalUniqueID = " + M1Util.ConvertToLinq(inspectionLine.qalUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qalInspectionID"] = inspectionLine.qalInspectionID.ToUpper();
				dataRow["qalInspectionLineID"] = inspectionLine.qalInspectionLineID;
				inspectionLine.qalUniqueID = ((inspectionLine.qalUniqueID == Guid.Empty) ? Guid.NewGuid() : inspectionLine.qalUniqueID);
				dataRow["qalUniqueID"] = inspectionLine.qalUniqueID;
				dataRow["qalCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qalCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The InspectionLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qalActionType"] = inspectionLine.qalActionType;
			DataRow dataRow2 = dataRow;
			DateTime? qalApprovalDecisionDate = inspectionLine.qalApprovalDecisionDate;
			dataRow2["qalApprovalDecisionDate"] = (qalApprovalDecisionDate.HasValue ? ((object)qalApprovalDecisionDate.GetValueOrDefault()) : dataRow["qalApprovalDecisionDate"]);
			DataRow dataRow3 = dataRow;
			qalApprovalDecisionDate = inspectionLine.qalApprovalRequestDate;
			dataRow3["qalApprovalRequestDate"] = (qalApprovalDecisionDate.HasValue ? ((object)qalApprovalDecisionDate.GetValueOrDefault()) : dataRow["qalApprovalRequestDate"]);
			dataRow["qalApprovalStatus"] = inspectionLine.qalApprovalStatus;
			DataRow dataRow4 = dataRow;
			qalApprovalDecisionDate = inspectionLine.qalClosedDate;
			dataRow4["qalClosedDate"] = (qalApprovalDecisionDate.HasValue ? ((object)qalApprovalDecisionDate.GetValueOrDefault()) : dataRow["qalClosedDate"]);
			DataRow dataRow5 = dataRow;
			qalApprovalDecisionDate = inspectionLine.qalInspectionDate;
			dataRow5["qalInspectionDate"] = (qalApprovalDecisionDate.HasValue ? ((object)qalApprovalDecisionDate.GetValueOrDefault()) : dataRow["qalInspectionDate"]);
			dataRow["qalInspectionNotesRTF"] = inspectionLine.qalInspectionNotesRTF ?? dataRow["qalInspectionNotesRTF"];
			dataRow["qalInspectionNotesText"] = inspectionLine.qalInspectionNotesText ?? dataRow["qalInspectionNotesText"];
			dataRow["qalInspectionType"] = inspectionLine.qalInspectionType;
			dataRow["qalInspectorEmployeeID"] = inspectionLine.qalInspectorEmployeeID;
			dataRow["qalInvQuantityAccepted"] = inspectionLine.qalInvQuantityAccepted;
			dataRow["qalInvQuantityToReturn"] = inspectionLine.qalInvQuantityToReturn;
			dataRow["qalInvQuantityToScrap"] = inspectionLine.qalInvQuantityToScrap;
			dataRow["qalFirstOffInspection"] = inspectionLine.qalFirstOffInspection;
			dataRow["qalInspectionComplete"] = inspectionLine.qalInspectionComplete;
			dataRow["qalKitPart"] = inspectionLine.qalKitPart;
			dataRow["qalManualInspectionFinalized"] = inspectionLine.qalManualInspectionFinalized;
			dataRow["qalPosted"] = inspectionLine.qalPosted;
			dataRow["qalReturnToSupplier"] = inspectionLine.qalReturnToSupplier;
			dataRow["qalReversed"] = inspectionLine.qalReversed;
			dataRow["qalTransferredToDmr"] = inspectionLine.qalTransferredToDmr;
			dataRow["qalJobAssemblyID"] = inspectionLine.qalJobAssemblyID;
			dataRow["qalJobID"] = inspectionLine.qalJobID;
			dataRow["qalJobMaterialID"] = inspectionLine.qalJobMaterialID;
			dataRow["qalJobMatQuantityAccepted"] = inspectionLine.qalJobMatQuantityAccepted;
			dataRow["qalJobMatQuantityRejected"] = inspectionLine.qalJobMatQuantityRejected;
			dataRow["qalJobMatQuantityToReturn"] = inspectionLine.qalJobMatQuantityToReturn;
			dataRow["qalJobMatQuantityToScrap"] = inspectionLine.qalJobMatQuantityToScrap;
			dataRow["qalJobOperationID"] = inspectionLine.qalJobOperationID;
			dataRow["qalJobOprQuantityAccepted"] = inspectionLine.qalJobOprQuantityAccepted;
			dataRow["qalJobOprQuantityRejected"] = inspectionLine.qalJobOprQuantityRejected;
			dataRow["qalJobOprQuantityToReturn"] = inspectionLine.qalJobOprQuantityToReturn;
			dataRow["qalJobOprQuantityToScrap"] = inspectionLine.qalJobOprQuantityToScrap;
			dataRow["qalJobType"] = inspectionLine.qalJobType;
			dataRow["qalMfgReceiptQuantityAccepted"] = inspectionLine.qalMfgReceiptQuantityAccepted;
			dataRow["qalMfgReceiptQuantityToReturn"] = inspectionLine.qalMfgReceiptQuantityToReturn;
			dataRow["qalMfgReceiptQuantityToScrap"] = inspectionLine.qalMfgReceiptQuantityToScrap;
			dataRow["qalNextApprovalEmployeeID"] = inspectionLine.qalNextApprovalEmployeeID;
			dataRow["qalPartBinID"] = inspectionLine.qalPartBinID;
			dataRow["qalPartID"] = inspectionLine.qalPartID;
			dataRow["qalPartLongDescriptionRtf"] = inspectionLine.qalPartLongDescriptionRtf ?? dataRow["qalPartLongDescriptionRtf"];
			dataRow["qalPartLongDescriptionText"] = inspectionLine.qalPartLongDescriptionText ?? dataRow["qalPartLongDescriptionText"];
			dataRow["qalPartRevisionID"] = inspectionLine.qalPartRevisionID;
			dataRow["qalPartShortDescription"] = inspectionLine.qalPartShortDescription;
			dataRow["qalPartTransactionID"] = inspectionLine.qalPartTransactionID;
			dataRow["qalPartWarehouseLocationID"] = inspectionLine.qalPartWarehouseLocationID;
			dataRow["qalProjectAreaID"] = inspectionLine.qalProjectAreaID;
			dataRow["qalProjectID"] = inspectionLine.qalProjectID;
			dataRow["qalPurchaseLocationID"] = inspectionLine.qalPurchaseLocationID;
			dataRow["qalQuantityRejected"] = inspectionLine.qalQuantityRejected;
			dataRow["qalQuantityToInspect"] = inspectionLine.qalQuantityToInspect;
			dataRow["qalReverseInspectionID"] = inspectionLine.qalReverseInspectionID;
			dataRow["qalReverseInspectionLineID"] = inspectionLine.qalReverseInspectionLineID;
			dataRow["qalScrapReasonID"] = inspectionLine.qalScrapReasonID;
			dataRow["qalSourceTableName"] = inspectionLine.qalSourceTableName;
			dataRow["qalSourceTableUniqueID"] = inspectionLine.qalSourceTableUniqueID;
			dataRow["qalStatus"] = inspectionLine.qalStatus;
			dataRow["qalSupplierOrganizationID"] = inspectionLine.qalSupplierOrganizationID;
			dataRow["qalUnitCost"] = inspectionLine.qalUnitCost;
			dataRow["qalUnitOfMeasure"] = inspectionLine.qalUnitOfMeasure;
			if (inspectionLine.CustomFields != null && inspectionLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in inspectionLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the InspectionLine [{inspectionLine.qalUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the InspectionLine [{inspectionLine.qalUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
