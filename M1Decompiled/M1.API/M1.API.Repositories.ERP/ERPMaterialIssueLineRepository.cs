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

public class ERPMaterialIssueLineRepository : APIBaseRepository, IERPMaterialIssueLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPMaterialIssueLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesMaterialIssueLineExist(Guid materialIssueLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("injUniqueID|C", materialIssueLineId);
		base.selectList.Add("injUniqueID");
		return Task.FromResult(GetAsObject("MaterialIssueLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPMaterialIssueLineInformationDto>> GetAllMaterialIssueLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPMaterialIssueLineInformationDto> collection = new List<ERPMaterialIssueLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[42]
		{
			"injCreatedBy", "injCreatedDate", "injUniqueID", "injEstimatedQuantity", "injHeatLot", "injInvIssueQuantity", "injInvScrapQuantity", "injCreateJobSeq", "injIssueComplete", "injKitPart",
			"injPosted", "injReversed", "injIssueType", "injJobAsmIssueQuantity", "injJobAsmScrapQuantity", "injJobAssemblyID", "injJobID", "injJobMaterialID", "injJobMatIssueQuantity", "injJobMatReturnIssueQuantity",
			"injJobMatReturnScrapQuantity", "injJobMatScrapQuantity", "injJobOpenQuantity", "injJobType", "injLongDescriptionRtf", "injLongDescriptionText", "injMaterialIssueID", "injMiscIssueReasonID", "injPartBinID", "injPartID",
			"injPartRevisionID", "injPartWarehouseLocationID", "injPlantID", "injProjectAreaID", "injProjectID", "injQuantityAllocated", "injQuantityOnHand", "injReference", "injReverseMaterialIssueID", "injReverseMaterialIssueLineID",
			"injRowVersion", "injMaterialIssueLineID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("MaterialIssueLines");
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
		using (DataTable dataTable = GetAsDataTable("MaterialIssueLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPMaterialIssueLineInformationDto eRPMaterialIssueLineInformationDto = new ERPMaterialIssueLineInformationDto();
				eRPMaterialIssueLineInformationDto.injCreatedBy = dataTable.Rows[i].Field<string>("injCreatedBy");
				eRPMaterialIssueLineInformationDto.injCreatedDate = dataTable.Rows[i].Field<DateTime?>("injCreatedDate");
				eRPMaterialIssueLineInformationDto.injUniqueID = dataTable.Rows[i].Field<Guid>("injUniqueID");
				eRPMaterialIssueLineInformationDto.injEstimatedQuantity = dataTable.Rows[i].Field<decimal>("injEstimatedQuantity");
				eRPMaterialIssueLineInformationDto.injHeatLot = dataTable.Rows[i].Field<string>("injHeatLot");
				eRPMaterialIssueLineInformationDto.injInvIssueQuantity = dataTable.Rows[i].Field<decimal>("injInvIssueQuantity");
				eRPMaterialIssueLineInformationDto.injInvScrapQuantity = dataTable.Rows[i].Field<decimal>("injInvScrapQuantity");
				eRPMaterialIssueLineInformationDto.injCreateJobSeq = dataTable.Rows[i].Field<bool>("injCreateJobSeq");
				eRPMaterialIssueLineInformationDto.injIssueComplete = dataTable.Rows[i].Field<bool>("injIssueComplete");
				eRPMaterialIssueLineInformationDto.injKitPart = dataTable.Rows[i].Field<bool>("injKitPart");
				eRPMaterialIssueLineInformationDto.injPosted = dataTable.Rows[i].Field<bool>("injPosted");
				eRPMaterialIssueLineInformationDto.injReversed = dataTable.Rows[i].Field<bool>("injReversed");
				eRPMaterialIssueLineInformationDto.injIssueType = dataTable.Rows[i].Field<byte>("injIssueType");
				eRPMaterialIssueLineInformationDto.injJobAsmIssueQuantity = dataTable.Rows[i].Field<decimal>("injJobAsmIssueQuantity");
				eRPMaterialIssueLineInformationDto.injJobAsmScrapQuantity = dataTable.Rows[i].Field<decimal>("injJobAsmScrapQuantity");
				eRPMaterialIssueLineInformationDto.injJobAssemblyID = dataTable.Rows[i].Field<int>("injJobAssemblyID");
				eRPMaterialIssueLineInformationDto.injJobID = dataTable.Rows[i].Field<string>("injJobID");
				eRPMaterialIssueLineInformationDto.injJobMaterialID = dataTable.Rows[i].Field<int>("injJobMaterialID");
				eRPMaterialIssueLineInformationDto.injJobMatIssueQuantity = dataTable.Rows[i].Field<decimal>("injJobMatIssueQuantity");
				eRPMaterialIssueLineInformationDto.injJobMatReturnIssueQuantity = dataTable.Rows[i].Field<decimal>("injJobMatReturnIssueQuantity");
				eRPMaterialIssueLineInformationDto.injJobMatReturnScrapQuantity = dataTable.Rows[i].Field<decimal>("injJobMatReturnScrapQuantity");
				eRPMaterialIssueLineInformationDto.injJobMatScrapQuantity = dataTable.Rows[i].Field<decimal>("injJobMatScrapQuantity");
				eRPMaterialIssueLineInformationDto.injJobOpenQuantity = dataTable.Rows[i].Field<decimal>("injJobOpenQuantity");
				eRPMaterialIssueLineInformationDto.injJobType = dataTable.Rows[i].Field<byte>("injJobType");
				eRPMaterialIssueLineInformationDto.injLongDescriptionRtf = dataTable.Rows[i].Field<string>("injLongDescriptionRtf");
				eRPMaterialIssueLineInformationDto.injLongDescriptionText = dataTable.Rows[i].Field<string>("injLongDescriptionText");
				eRPMaterialIssueLineInformationDto.injMaterialIssueID = dataTable.Rows[i].Field<string>("injMaterialIssueID");
				eRPMaterialIssueLineInformationDto.injMiscIssueReasonID = dataTable.Rows[i].Field<string>("injMiscIssueReasonID");
				eRPMaterialIssueLineInformationDto.injPartBinID = dataTable.Rows[i].Field<string>("injPartBinID");
				eRPMaterialIssueLineInformationDto.injPartID = dataTable.Rows[i].Field<string>("injPartID");
				eRPMaterialIssueLineInformationDto.injPartRevisionID = dataTable.Rows[i].Field<string>("injPartRevisionID");
				eRPMaterialIssueLineInformationDto.injPartWarehouseLocationID = dataTable.Rows[i].Field<string>("injPartWarehouseLocationID");
				eRPMaterialIssueLineInformationDto.injPlantID = dataTable.Rows[i].Field<string>("injPlantID");
				eRPMaterialIssueLineInformationDto.injProjectAreaID = dataTable.Rows[i].Field<string>("injProjectAreaID");
				eRPMaterialIssueLineInformationDto.injProjectID = dataTable.Rows[i].Field<string>("injProjectID");
				eRPMaterialIssueLineInformationDto.injQuantityAllocated = dataTable.Rows[i].Field<decimal>("injQuantityAllocated");
				eRPMaterialIssueLineInformationDto.injQuantityOnHand = dataTable.Rows[i].Field<decimal>("injQuantityOnHand");
				eRPMaterialIssueLineInformationDto.injReference = dataTable.Rows[i].Field<string>("injReference");
				eRPMaterialIssueLineInformationDto.injReverseMaterialIssueID = dataTable.Rows[i].Field<string>("injReverseMaterialIssueID");
				eRPMaterialIssueLineInformationDto.injReverseMaterialIssueLineID = dataTable.Rows[i].Field<short>("injReverseMaterialIssueLineID");
				eRPMaterialIssueLineInformationDto.injRowVersion = dataTable.Rows[i].Field<byte[]>("injRowVersion");
				eRPMaterialIssueLineInformationDto.injMaterialIssueLineID = dataTable.Rows[i].Field<short>("injMaterialIssueLineID");
				eRPMaterialIssueLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPMaterialIssueLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPMaterialIssueLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPMaterialIssueLineInformationDto> GetMaterialIssueLine(Guid materialIssueLineId)
	{
		ERPMaterialIssueLineInformationDto eRPMaterialIssueLineInformationDto = new ERPMaterialIssueLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[42]
		{
			"injCreatedBy", "injCreatedDate", "injUniqueID", "injEstimatedQuantity", "injHeatLot", "injInvIssueQuantity", "injInvScrapQuantity", "injCreateJobSeq", "injIssueComplete", "injKitPart",
			"injPosted", "injReversed", "injIssueType", "injJobAsmIssueQuantity", "injJobAsmScrapQuantity", "injJobAssemblyID", "injJobID", "injJobMaterialID", "injJobMatIssueQuantity", "injJobMatReturnIssueQuantity",
			"injJobMatReturnScrapQuantity", "injJobMatScrapQuantity", "injJobOpenQuantity", "injJobType", "injLongDescriptionRtf", "injLongDescriptionText", "injMaterialIssueID", "injMiscIssueReasonID", "injPartBinID", "injPartID",
			"injPartRevisionID", "injPartWarehouseLocationID", "injPlantID", "injProjectAreaID", "injProjectID", "injQuantityAllocated", "injQuantityOnHand", "injReference", "injReverseMaterialIssueID", "injReverseMaterialIssueLineID",
			"injRowVersion", "injMaterialIssueLineID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("injUniqueID|C", materialIssueLineId);
		AddCustomFieldsToSelectList("MaterialIssueLines");
		using (DataTable dataTable = GetAsDataTable("MaterialIssueLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPMaterialIssueLineInformationDto);
			}
			eRPMaterialIssueLineInformationDto.injCreatedBy = dataTable.Rows[0].Field<string>("injCreatedBy");
			eRPMaterialIssueLineInformationDto.injCreatedDate = dataTable.Rows[0].Field<DateTime?>("injCreatedDate");
			eRPMaterialIssueLineInformationDto.injUniqueID = dataTable.Rows[0].Field<Guid>("injUniqueID");
			eRPMaterialIssueLineInformationDto.injEstimatedQuantity = dataTable.Rows[0].Field<decimal>("injEstimatedQuantity");
			eRPMaterialIssueLineInformationDto.injHeatLot = dataTable.Rows[0].Field<string>("injHeatLot");
			eRPMaterialIssueLineInformationDto.injInvIssueQuantity = dataTable.Rows[0].Field<decimal>("injInvIssueQuantity");
			eRPMaterialIssueLineInformationDto.injInvScrapQuantity = dataTable.Rows[0].Field<decimal>("injInvScrapQuantity");
			eRPMaterialIssueLineInformationDto.injCreateJobSeq = dataTable.Rows[0].Field<bool>("injCreateJobSeq");
			eRPMaterialIssueLineInformationDto.injIssueComplete = dataTable.Rows[0].Field<bool>("injIssueComplete");
			eRPMaterialIssueLineInformationDto.injKitPart = dataTable.Rows[0].Field<bool>("injKitPart");
			eRPMaterialIssueLineInformationDto.injPosted = dataTable.Rows[0].Field<bool>("injPosted");
			eRPMaterialIssueLineInformationDto.injReversed = dataTable.Rows[0].Field<bool>("injReversed");
			eRPMaterialIssueLineInformationDto.injIssueType = dataTable.Rows[0].Field<byte>("injIssueType");
			eRPMaterialIssueLineInformationDto.injJobAsmIssueQuantity = dataTable.Rows[0].Field<decimal>("injJobAsmIssueQuantity");
			eRPMaterialIssueLineInformationDto.injJobAsmScrapQuantity = dataTable.Rows[0].Field<decimal>("injJobAsmScrapQuantity");
			eRPMaterialIssueLineInformationDto.injJobAssemblyID = dataTable.Rows[0].Field<int>("injJobAssemblyID");
			eRPMaterialIssueLineInformationDto.injJobID = dataTable.Rows[0].Field<string>("injJobID");
			eRPMaterialIssueLineInformationDto.injJobMaterialID = dataTable.Rows[0].Field<int>("injJobMaterialID");
			eRPMaterialIssueLineInformationDto.injJobMatIssueQuantity = dataTable.Rows[0].Field<decimal>("injJobMatIssueQuantity");
			eRPMaterialIssueLineInformationDto.injJobMatReturnIssueQuantity = dataTable.Rows[0].Field<decimal>("injJobMatReturnIssueQuantity");
			eRPMaterialIssueLineInformationDto.injJobMatReturnScrapQuantity = dataTable.Rows[0].Field<decimal>("injJobMatReturnScrapQuantity");
			eRPMaterialIssueLineInformationDto.injJobMatScrapQuantity = dataTable.Rows[0].Field<decimal>("injJobMatScrapQuantity");
			eRPMaterialIssueLineInformationDto.injJobOpenQuantity = dataTable.Rows[0].Field<decimal>("injJobOpenQuantity");
			eRPMaterialIssueLineInformationDto.injJobType = dataTable.Rows[0].Field<byte>("injJobType");
			eRPMaterialIssueLineInformationDto.injLongDescriptionRtf = dataTable.Rows[0].Field<string>("injLongDescriptionRtf");
			eRPMaterialIssueLineInformationDto.injLongDescriptionText = dataTable.Rows[0].Field<string>("injLongDescriptionText");
			eRPMaterialIssueLineInformationDto.injMaterialIssueID = dataTable.Rows[0].Field<string>("injMaterialIssueID");
			eRPMaterialIssueLineInformationDto.injMiscIssueReasonID = dataTable.Rows[0].Field<string>("injMiscIssueReasonID");
			eRPMaterialIssueLineInformationDto.injPartBinID = dataTable.Rows[0].Field<string>("injPartBinID");
			eRPMaterialIssueLineInformationDto.injPartID = dataTable.Rows[0].Field<string>("injPartID");
			eRPMaterialIssueLineInformationDto.injPartRevisionID = dataTable.Rows[0].Field<string>("injPartRevisionID");
			eRPMaterialIssueLineInformationDto.injPartWarehouseLocationID = dataTable.Rows[0].Field<string>("injPartWarehouseLocationID");
			eRPMaterialIssueLineInformationDto.injPlantID = dataTable.Rows[0].Field<string>("injPlantID");
			eRPMaterialIssueLineInformationDto.injProjectAreaID = dataTable.Rows[0].Field<string>("injProjectAreaID");
			eRPMaterialIssueLineInformationDto.injProjectID = dataTable.Rows[0].Field<string>("injProjectID");
			eRPMaterialIssueLineInformationDto.injQuantityAllocated = dataTable.Rows[0].Field<decimal>("injQuantityAllocated");
			eRPMaterialIssueLineInformationDto.injQuantityOnHand = dataTable.Rows[0].Field<decimal>("injQuantityOnHand");
			eRPMaterialIssueLineInformationDto.injReference = dataTable.Rows[0].Field<string>("injReference");
			eRPMaterialIssueLineInformationDto.injReverseMaterialIssueID = dataTable.Rows[0].Field<string>("injReverseMaterialIssueID");
			eRPMaterialIssueLineInformationDto.injReverseMaterialIssueLineID = dataTable.Rows[0].Field<short>("injReverseMaterialIssueLineID");
			eRPMaterialIssueLineInformationDto.injRowVersion = dataTable.Rows[0].Field<byte[]>("injRowVersion");
			eRPMaterialIssueLineInformationDto.injMaterialIssueLineID = dataTable.Rows[0].Field<short>("injMaterialIssueLineID");
			eRPMaterialIssueLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPMaterialIssueLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPMaterialIssueLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveMaterialIssueLine(ERPMaterialIssueLineDto materialIssueLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM MaterialIssueLines WHERE injUniqueID = " + M1Util.ConvertToLinq(materialIssueLine.injUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["injMaterialIssueID"] = materialIssueLine.injMaterialIssueID.ToUpper();
				dataRow["injMaterialIssueLineID"] = materialIssueLine.injMaterialIssueLineID;
				materialIssueLine.injUniqueID = ((materialIssueLine.injUniqueID == Guid.Empty) ? Guid.NewGuid() : materialIssueLine.injUniqueID);
				dataRow["injUniqueID"] = materialIssueLine.injUniqueID;
				dataRow["injCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["injCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The MaterialIssueLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (materialIssueLine.injRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the MaterialIssueLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["injRowVersion"], materialIssueLine.injRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the MaterialIssueLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the MaterialIssueLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["injEstimatedQuantity"] = materialIssueLine.injEstimatedQuantity;
			dataRow["injHeatLot"] = materialIssueLine.injHeatLot;
			dataRow["injInvIssueQuantity"] = materialIssueLine.injInvIssueQuantity;
			dataRow["injInvScrapQuantity"] = materialIssueLine.injInvScrapQuantity;
			dataRow["injCreateJobSeq"] = materialIssueLine.injCreateJobSeq;
			dataRow["injIssueComplete"] = materialIssueLine.injIssueComplete;
			dataRow["injKitPart"] = materialIssueLine.injKitPart;
			dataRow["injPosted"] = materialIssueLine.injPosted;
			dataRow["injReversed"] = materialIssueLine.injReversed;
			dataRow["injIssueType"] = materialIssueLine.injIssueType;
			dataRow["injJobAsmIssueQuantity"] = materialIssueLine.injJobAsmIssueQuantity;
			dataRow["injJobAsmScrapQuantity"] = materialIssueLine.injJobAsmScrapQuantity;
			dataRow["injJobAssemblyID"] = materialIssueLine.injJobAssemblyID;
			dataRow["injJobID"] = materialIssueLine.injJobID;
			dataRow["injJobMaterialID"] = materialIssueLine.injJobMaterialID;
			dataRow["injJobMatIssueQuantity"] = materialIssueLine.injJobMatIssueQuantity;
			dataRow["injJobMatReturnIssueQuantity"] = materialIssueLine.injJobMatReturnIssueQuantity;
			dataRow["injJobMatReturnScrapQuantity"] = materialIssueLine.injJobMatReturnScrapQuantity;
			dataRow["injJobMatScrapQuantity"] = materialIssueLine.injJobMatScrapQuantity;
			dataRow["injJobOpenQuantity"] = materialIssueLine.injJobOpenQuantity;
			dataRow["injJobType"] = materialIssueLine.injJobType;
			dataRow["injLongDescriptionRtf"] = materialIssueLine.injLongDescriptionRtf ?? dataRow["injLongDescriptionRtf"];
			dataRow["injLongDescriptionText"] = materialIssueLine.injLongDescriptionText ?? dataRow["injLongDescriptionText"];
			dataRow["injMiscIssueReasonID"] = materialIssueLine.injMiscIssueReasonID;
			dataRow["injPartBinID"] = materialIssueLine.injPartBinID;
			dataRow["injPartID"] = materialIssueLine.injPartID;
			dataRow["injPartRevisionID"] = materialIssueLine.injPartRevisionID;
			dataRow["injPartWarehouseLocationID"] = materialIssueLine.injPartWarehouseLocationID;
			dataRow["injPlantID"] = materialIssueLine.injPlantID;
			dataRow["injProjectAreaID"] = materialIssueLine.injProjectAreaID;
			dataRow["injProjectID"] = materialIssueLine.injProjectID;
			dataRow["injQuantityAllocated"] = materialIssueLine.injQuantityAllocated;
			dataRow["injQuantityOnHand"] = materialIssueLine.injQuantityOnHand;
			dataRow["injReference"] = materialIssueLine.injReference;
			dataRow["injReverseMaterialIssueID"] = materialIssueLine.injReverseMaterialIssueID;
			dataRow["injReverseMaterialIssueLineID"] = materialIssueLine.injReverseMaterialIssueLineID;
			if (materialIssueLine.CustomFields != null && materialIssueLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in materialIssueLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the MaterialIssueLine [{materialIssueLine.injUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the MaterialIssueLine [{materialIssueLine.injUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
