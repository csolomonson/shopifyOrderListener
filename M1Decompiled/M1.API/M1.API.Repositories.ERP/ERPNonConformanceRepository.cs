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

public class ERPNonConformanceRepository : APIBaseRepository, IERPNonConformanceRepository, IAPIBaseRepository, IDisposable
{
	public ERPNonConformanceRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesNonConformanceExist(Guid nonConformanceId)
	{
		InitializeParameterLists();
		base.filterList.Add("qarUniqueID|C", nonConformanceId);
		base.selectList.Add("qarUniqueID");
		return Task.FromResult(GetAsObject("NonConformances", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPNonConformanceInformationDto>> GetAllNonConformances(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPNonConformanceInformationDto> collection = new List<ERPNonConformanceInformationDto>();
		InitializeParameterLists();
		string[] array = new string[39]
		{
			"qarActualHours", "qarNonConformanceID", "qarCorrectiveActionCategoryID", "qarCorrectiveActionCodeID", "qarCorrectiveActionDate", "qarCorrectiveActionRTF", "qarCorrectiveActionText", "qarCorrectiveActionType", "qarCreatedBy", "qarCreatedDate",
			"qarUniqueID", "qarHoursAllowed", "qarHoursRequested", "qarInspectionID", "qarInspectionLineID", "qarCorrectiveActionComplete", "qarJobAssemblyID", "qarJobID", "qarJobMaterialID", "qarJobOperationID",
			"qarNonConformanceCategoryID", "qarNonConformanceCauseID", "qarNonConformanceCodeID", "qarNonConformanceRTF", "qarNonConformanceText", "qarPartBinID", "qarPartID", "qarPartRevisionID", "qarPartShortDescription", "qarPartWareHouseLocationID",
			"qarQuantity", "qarRepairedByOrganizationID", "qarReportedByEmployeeID", "qarRmaClaimID", "qarRmaClaimLineID", "qarRowVersion", "qarSubcontractAmount", "qarSubcontractAmountForeign", "qarUnitOfMeasure"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("NonConformances");
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
		using (DataTable dataTable = GetAsDataTable("NonConformances", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPNonConformanceInformationDto eRPNonConformanceInformationDto = new ERPNonConformanceInformationDto();
				eRPNonConformanceInformationDto.qarActualHours = dataTable.Rows[i].Field<decimal>("qarActualHours");
				eRPNonConformanceInformationDto.qarNonConformanceID = dataTable.Rows[i].Field<string>("qarNonConformanceID");
				eRPNonConformanceInformationDto.qarCorrectiveActionCategoryID = dataTable.Rows[i].Field<string>("qarCorrectiveActionCategoryID");
				eRPNonConformanceInformationDto.qarCorrectiveActionCodeID = dataTable.Rows[i].Field<string>("qarCorrectiveActionCodeID");
				eRPNonConformanceInformationDto.qarCorrectiveActionDate = dataTable.Rows[i].Field<DateTime?>("qarCorrectiveActionDate");
				eRPNonConformanceInformationDto.qarCorrectiveActionRTF = dataTable.Rows[i].Field<string>("qarCorrectiveActionRTF");
				eRPNonConformanceInformationDto.qarCorrectiveActionText = dataTable.Rows[i].Field<string>("qarCorrectiveActionText");
				eRPNonConformanceInformationDto.qarCorrectiveActionType = dataTable.Rows[i].Field<byte>("qarCorrectiveActionType");
				eRPNonConformanceInformationDto.qarCreatedBy = dataTable.Rows[i].Field<string>("qarCreatedBy");
				eRPNonConformanceInformationDto.qarCreatedDate = dataTable.Rows[i].Field<DateTime?>("qarCreatedDate");
				eRPNonConformanceInformationDto.qarUniqueID = dataTable.Rows[i].Field<Guid>("qarUniqueID");
				eRPNonConformanceInformationDto.qarHoursAllowed = dataTable.Rows[i].Field<decimal>("qarHoursAllowed");
				eRPNonConformanceInformationDto.qarHoursRequested = dataTable.Rows[i].Field<decimal>("qarHoursRequested");
				eRPNonConformanceInformationDto.qarInspectionID = dataTable.Rows[i].Field<string>("qarInspectionID");
				eRPNonConformanceInformationDto.qarInspectionLineID = dataTable.Rows[i].Field<short>("qarInspectionLineID");
				eRPNonConformanceInformationDto.qarCorrectiveActionComplete = dataTable.Rows[i].Field<bool>("qarCorrectiveActionComplete");
				eRPNonConformanceInformationDto.qarJobAssemblyID = dataTable.Rows[i].Field<int>("qarJobAssemblyID");
				eRPNonConformanceInformationDto.qarJobID = dataTable.Rows[i].Field<string>("qarJobID");
				eRPNonConformanceInformationDto.qarJobMaterialID = dataTable.Rows[i].Field<int>("qarJobMaterialID");
				eRPNonConformanceInformationDto.qarJobOperationID = dataTable.Rows[i].Field<int>("qarJobOperationID");
				eRPNonConformanceInformationDto.qarNonConformanceCategoryID = dataTable.Rows[i].Field<string>("qarNonConformanceCategoryID");
				eRPNonConformanceInformationDto.qarNonConformanceCauseID = dataTable.Rows[i].Field<string>("qarNonConformanceCauseID");
				eRPNonConformanceInformationDto.qarNonConformanceCodeID = dataTable.Rows[i].Field<string>("qarNonConformanceCodeID");
				eRPNonConformanceInformationDto.qarNonConformanceRTF = dataTable.Rows[i].Field<string>("qarNonConformanceRTF");
				eRPNonConformanceInformationDto.qarNonConformanceText = dataTable.Rows[i].Field<string>("qarNonConformanceText");
				eRPNonConformanceInformationDto.qarPartBinID = dataTable.Rows[i].Field<string>("qarPartBinID");
				eRPNonConformanceInformationDto.qarPartID = dataTable.Rows[i].Field<string>("qarPartID");
				eRPNonConformanceInformationDto.qarPartRevisionID = dataTable.Rows[i].Field<string>("qarPartRevisionID");
				eRPNonConformanceInformationDto.qarPartShortDescription = dataTable.Rows[i].Field<string>("qarPartShortDescription");
				eRPNonConformanceInformationDto.qarPartWareHouseLocationID = dataTable.Rows[i].Field<string>("qarPartWareHouseLocationID");
				eRPNonConformanceInformationDto.qarQuantity = dataTable.Rows[i].Field<decimal>("qarQuantity");
				eRPNonConformanceInformationDto.qarRepairedByOrganizationID = dataTable.Rows[i].Field<string>("qarRepairedByOrganizationID");
				eRPNonConformanceInformationDto.qarReportedByEmployeeID = dataTable.Rows[i].Field<string>("qarReportedByEmployeeID");
				eRPNonConformanceInformationDto.qarRmaClaimID = dataTable.Rows[i].Field<string>("qarRmaClaimID");
				eRPNonConformanceInformationDto.qarRmaClaimLineID = dataTable.Rows[i].Field<short>("qarRmaClaimLineID");
				eRPNonConformanceInformationDto.qarRowVersion = dataTable.Rows[i].Field<byte[]>("qarRowVersion");
				eRPNonConformanceInformationDto.qarSubcontractAmount = dataTable.Rows[i].Field<decimal>("qarSubcontractAmount");
				eRPNonConformanceInformationDto.qarSubcontractAmountForeign = dataTable.Rows[i].Field<decimal>("qarSubcontractAmountForeign");
				eRPNonConformanceInformationDto.qarUnitOfMeasure = dataTable.Rows[i].Field<string>("qarUnitOfMeasure");
				eRPNonConformanceInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPNonConformanceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPNonConformanceInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPNonConformanceInformationDto> GetNonConformance(Guid nonConformanceId)
	{
		ERPNonConformanceInformationDto eRPNonConformanceInformationDto = new ERPNonConformanceInformationDto();
		InitializeParameterLists();
		string[] collection = new string[39]
		{
			"qarActualHours", "qarNonConformanceID", "qarCorrectiveActionCategoryID", "qarCorrectiveActionCodeID", "qarCorrectiveActionDate", "qarCorrectiveActionRTF", "qarCorrectiveActionText", "qarCorrectiveActionType", "qarCreatedBy", "qarCreatedDate",
			"qarUniqueID", "qarHoursAllowed", "qarHoursRequested", "qarInspectionID", "qarInspectionLineID", "qarCorrectiveActionComplete", "qarJobAssemblyID", "qarJobID", "qarJobMaterialID", "qarJobOperationID",
			"qarNonConformanceCategoryID", "qarNonConformanceCauseID", "qarNonConformanceCodeID", "qarNonConformanceRTF", "qarNonConformanceText", "qarPartBinID", "qarPartID", "qarPartRevisionID", "qarPartShortDescription", "qarPartWareHouseLocationID",
			"qarQuantity", "qarRepairedByOrganizationID", "qarReportedByEmployeeID", "qarRmaClaimID", "qarRmaClaimLineID", "qarRowVersion", "qarSubcontractAmount", "qarSubcontractAmountForeign", "qarUnitOfMeasure"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("qarUniqueID|C", nonConformanceId);
		AddCustomFieldsToSelectList("NonConformances");
		using (DataTable dataTable = GetAsDataTable("NonConformances", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPNonConformanceInformationDto);
			}
			eRPNonConformanceInformationDto.qarActualHours = dataTable.Rows[0].Field<decimal>("qarActualHours");
			eRPNonConformanceInformationDto.qarNonConformanceID = dataTable.Rows[0].Field<string>("qarNonConformanceID");
			eRPNonConformanceInformationDto.qarCorrectiveActionCategoryID = dataTable.Rows[0].Field<string>("qarCorrectiveActionCategoryID");
			eRPNonConformanceInformationDto.qarCorrectiveActionCodeID = dataTable.Rows[0].Field<string>("qarCorrectiveActionCodeID");
			eRPNonConformanceInformationDto.qarCorrectiveActionDate = dataTable.Rows[0].Field<DateTime?>("qarCorrectiveActionDate");
			eRPNonConformanceInformationDto.qarCorrectiveActionRTF = dataTable.Rows[0].Field<string>("qarCorrectiveActionRTF");
			eRPNonConformanceInformationDto.qarCorrectiveActionText = dataTable.Rows[0].Field<string>("qarCorrectiveActionText");
			eRPNonConformanceInformationDto.qarCorrectiveActionType = dataTable.Rows[0].Field<byte>("qarCorrectiveActionType");
			eRPNonConformanceInformationDto.qarCreatedBy = dataTable.Rows[0].Field<string>("qarCreatedBy");
			eRPNonConformanceInformationDto.qarCreatedDate = dataTable.Rows[0].Field<DateTime?>("qarCreatedDate");
			eRPNonConformanceInformationDto.qarUniqueID = dataTable.Rows[0].Field<Guid>("qarUniqueID");
			eRPNonConformanceInformationDto.qarHoursAllowed = dataTable.Rows[0].Field<decimal>("qarHoursAllowed");
			eRPNonConformanceInformationDto.qarHoursRequested = dataTable.Rows[0].Field<decimal>("qarHoursRequested");
			eRPNonConformanceInformationDto.qarInspectionID = dataTable.Rows[0].Field<string>("qarInspectionID");
			eRPNonConformanceInformationDto.qarInspectionLineID = dataTable.Rows[0].Field<short>("qarInspectionLineID");
			eRPNonConformanceInformationDto.qarCorrectiveActionComplete = dataTable.Rows[0].Field<bool>("qarCorrectiveActionComplete");
			eRPNonConformanceInformationDto.qarJobAssemblyID = dataTable.Rows[0].Field<int>("qarJobAssemblyID");
			eRPNonConformanceInformationDto.qarJobID = dataTable.Rows[0].Field<string>("qarJobID");
			eRPNonConformanceInformationDto.qarJobMaterialID = dataTable.Rows[0].Field<int>("qarJobMaterialID");
			eRPNonConformanceInformationDto.qarJobOperationID = dataTable.Rows[0].Field<int>("qarJobOperationID");
			eRPNonConformanceInformationDto.qarNonConformanceCategoryID = dataTable.Rows[0].Field<string>("qarNonConformanceCategoryID");
			eRPNonConformanceInformationDto.qarNonConformanceCauseID = dataTable.Rows[0].Field<string>("qarNonConformanceCauseID");
			eRPNonConformanceInformationDto.qarNonConformanceCodeID = dataTable.Rows[0].Field<string>("qarNonConformanceCodeID");
			eRPNonConformanceInformationDto.qarNonConformanceRTF = dataTable.Rows[0].Field<string>("qarNonConformanceRTF");
			eRPNonConformanceInformationDto.qarNonConformanceText = dataTable.Rows[0].Field<string>("qarNonConformanceText");
			eRPNonConformanceInformationDto.qarPartBinID = dataTable.Rows[0].Field<string>("qarPartBinID");
			eRPNonConformanceInformationDto.qarPartID = dataTable.Rows[0].Field<string>("qarPartID");
			eRPNonConformanceInformationDto.qarPartRevisionID = dataTable.Rows[0].Field<string>("qarPartRevisionID");
			eRPNonConformanceInformationDto.qarPartShortDescription = dataTable.Rows[0].Field<string>("qarPartShortDescription");
			eRPNonConformanceInformationDto.qarPartWareHouseLocationID = dataTable.Rows[0].Field<string>("qarPartWareHouseLocationID");
			eRPNonConformanceInformationDto.qarQuantity = dataTable.Rows[0].Field<decimal>("qarQuantity");
			eRPNonConformanceInformationDto.qarRepairedByOrganizationID = dataTable.Rows[0].Field<string>("qarRepairedByOrganizationID");
			eRPNonConformanceInformationDto.qarReportedByEmployeeID = dataTable.Rows[0].Field<string>("qarReportedByEmployeeID");
			eRPNonConformanceInformationDto.qarRmaClaimID = dataTable.Rows[0].Field<string>("qarRmaClaimID");
			eRPNonConformanceInformationDto.qarRmaClaimLineID = dataTable.Rows[0].Field<short>("qarRmaClaimLineID");
			eRPNonConformanceInformationDto.qarRowVersion = dataTable.Rows[0].Field<byte[]>("qarRowVersion");
			eRPNonConformanceInformationDto.qarSubcontractAmount = dataTable.Rows[0].Field<decimal>("qarSubcontractAmount");
			eRPNonConformanceInformationDto.qarSubcontractAmountForeign = dataTable.Rows[0].Field<decimal>("qarSubcontractAmountForeign");
			eRPNonConformanceInformationDto.qarUnitOfMeasure = dataTable.Rows[0].Field<string>("qarUnitOfMeasure");
			eRPNonConformanceInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPNonConformanceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPNonConformanceInformationDto);
	}

	public Task<APIValidationInfoDto> SaveNonConformance(ERPNonConformanceDto nonConformance)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM NonConformances WHERE qarUniqueID = " + M1Util.ConvertToLinq(nonConformance.qarUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qarNonConformanceID"] = nonConformance.qarNonConformanceID.ToUpper();
				nonConformance.qarUniqueID = ((nonConformance.qarUniqueID == Guid.Empty) ? Guid.NewGuid() : nonConformance.qarUniqueID);
				dataRow["qarUniqueID"] = nonConformance.qarUniqueID;
				dataRow["qarCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qarCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The NonConformance could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (nonConformance.qarRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the NonConformance is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["qarRowVersion"], nonConformance.qarRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the NonConformance has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the NonConformance again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qarActualHours"] = nonConformance.qarActualHours;
			dataRow["qarCorrectiveActionCategoryID"] = nonConformance.qarCorrectiveActionCategoryID;
			dataRow["qarCorrectiveActionCodeID"] = nonConformance.qarCorrectiveActionCodeID;
			DataRow dataRow2 = dataRow;
			DateTime? qarCorrectiveActionDate = nonConformance.qarCorrectiveActionDate;
			dataRow2["qarCorrectiveActionDate"] = (qarCorrectiveActionDate.HasValue ? ((object)qarCorrectiveActionDate.GetValueOrDefault()) : dataRow["qarCorrectiveActionDate"]);
			dataRow["qarCorrectiveActionRTF"] = nonConformance.qarCorrectiveActionRTF ?? dataRow["qarCorrectiveActionRTF"];
			dataRow["qarCorrectiveActionText"] = nonConformance.qarCorrectiveActionText ?? dataRow["qarCorrectiveActionText"];
			dataRow["qarCorrectiveActionType"] = nonConformance.qarCorrectiveActionType;
			dataRow["qarHoursAllowed"] = nonConformance.qarHoursAllowed;
			dataRow["qarHoursRequested"] = nonConformance.qarHoursRequested;
			dataRow["qarInspectionID"] = nonConformance.qarInspectionID;
			dataRow["qarInspectionLineID"] = nonConformance.qarInspectionLineID;
			dataRow["qarCorrectiveActionComplete"] = nonConformance.qarCorrectiveActionComplete;
			dataRow["qarJobAssemblyID"] = nonConformance.qarJobAssemblyID;
			dataRow["qarJobID"] = nonConformance.qarJobID;
			dataRow["qarJobMaterialID"] = nonConformance.qarJobMaterialID;
			dataRow["qarJobOperationID"] = nonConformance.qarJobOperationID;
			dataRow["qarNonConformanceCategoryID"] = nonConformance.qarNonConformanceCategoryID;
			dataRow["qarNonConformanceCauseID"] = nonConformance.qarNonConformanceCauseID;
			dataRow["qarNonConformanceCodeID"] = nonConformance.qarNonConformanceCodeID;
			dataRow["qarNonConformanceRTF"] = nonConformance.qarNonConformanceRTF ?? dataRow["qarNonConformanceRTF"];
			dataRow["qarNonConformanceText"] = nonConformance.qarNonConformanceText ?? dataRow["qarNonConformanceText"];
			dataRow["qarPartBinID"] = nonConformance.qarPartBinID;
			dataRow["qarPartID"] = nonConformance.qarPartID;
			dataRow["qarPartRevisionID"] = nonConformance.qarPartRevisionID;
			dataRow["qarPartShortDescription"] = nonConformance.qarPartShortDescription;
			dataRow["qarPartWareHouseLocationID"] = nonConformance.qarPartWareHouseLocationID;
			dataRow["qarQuantity"] = nonConformance.qarQuantity;
			dataRow["qarRepairedByOrganizationID"] = nonConformance.qarRepairedByOrganizationID;
			dataRow["qarReportedByEmployeeID"] = nonConformance.qarReportedByEmployeeID;
			dataRow["qarRmaClaimID"] = nonConformance.qarRmaClaimID;
			dataRow["qarRmaClaimLineID"] = nonConformance.qarRmaClaimLineID;
			dataRow["qarSubcontractAmount"] = nonConformance.qarSubcontractAmount;
			dataRow["qarSubcontractAmountForeign"] = nonConformance.qarSubcontractAmountForeign;
			dataRow["qarUnitOfMeasure"] = nonConformance.qarUnitOfMeasure;
			if (nonConformance.CustomFields != null && nonConformance.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in nonConformance.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the NonConformance [{nonConformance.qarUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the NonConformance [{nonConformance.qarUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
