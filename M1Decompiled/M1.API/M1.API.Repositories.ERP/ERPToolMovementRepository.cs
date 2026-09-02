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

public class ERPToolMovementRepository : APIBaseRepository, IERPToolMovementRepository, IAPIBaseRepository, IDisposable
{
	public ERPToolMovementRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesToolMovementExist(Guid toolMovementId)
	{
		InitializeParameterLists();
		base.filterList.Add("xtaUniqueID|C", toolMovementId);
		base.selectList.Add("xtaUniqueID");
		return Task.FromResult(GetAsObject("ToolMovements", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPToolMovementInformationDto>> GetAllToolMovements(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPToolMovementInformationDto> collection = new List<ERPToolMovementInformationDto>();
		InitializeParameterLists();
		string[] array = new string[19]
		{
			"xtaCheckedOutToEmployeeID", "xtaCheckoutReasonID", "xtaCreatedBy", "xtaCreatedDate", "xtaUniqueID", "xtaJobID", "xtaLocation", "xtaMovementDate", "xtaMovementType", "xtaNotesRTF",
			"xtaNotesText", "xtaPlannedReturnDate", "xtaPlantDepartmentID", "xtaPlantID", "xtaProductionDepartmentID", "xtaRowVersion", "xtaToolMovementID", "xtaToolID", "xtaWorkCenterID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ToolMovements");
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
		using (DataTable dataTable = GetAsDataTable("ToolMovements", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPToolMovementInformationDto eRPToolMovementInformationDto = new ERPToolMovementInformationDto();
				eRPToolMovementInformationDto.xtaCheckedOutToEmployeeID = dataTable.Rows[i].Field<string>("xtaCheckedOutToEmployeeID");
				eRPToolMovementInformationDto.xtaCheckoutReasonID = dataTable.Rows[i].Field<string>("xtaCheckoutReasonID");
				eRPToolMovementInformationDto.xtaCreatedBy = dataTable.Rows[i].Field<string>("xtaCreatedBy");
				eRPToolMovementInformationDto.xtaCreatedDate = dataTable.Rows[i].Field<DateTime?>("xtaCreatedDate");
				eRPToolMovementInformationDto.xtaUniqueID = dataTable.Rows[i].Field<Guid>("xtaUniqueID");
				eRPToolMovementInformationDto.xtaJobID = dataTable.Rows[i].Field<string>("xtaJobID");
				eRPToolMovementInformationDto.xtaLocation = dataTable.Rows[i].Field<string>("xtaLocation");
				eRPToolMovementInformationDto.xtaMovementDate = dataTable.Rows[i].Field<DateTime?>("xtaMovementDate");
				eRPToolMovementInformationDto.xtaMovementType = dataTable.Rows[i].Field<string>("xtaMovementType");
				eRPToolMovementInformationDto.xtaNotesRTF = dataTable.Rows[i].Field<string>("xtaNotesRTF");
				eRPToolMovementInformationDto.xtaNotesText = dataTable.Rows[i].Field<string>("xtaNotesText");
				eRPToolMovementInformationDto.xtaPlannedReturnDate = dataTable.Rows[i].Field<DateTime?>("xtaPlannedReturnDate");
				eRPToolMovementInformationDto.xtaPlantDepartmentID = dataTable.Rows[i].Field<string>("xtaPlantDepartmentID");
				eRPToolMovementInformationDto.xtaPlantID = dataTable.Rows[i].Field<string>("xtaPlantID");
				eRPToolMovementInformationDto.xtaProductionDepartmentID = dataTable.Rows[i].Field<string>("xtaProductionDepartmentID");
				eRPToolMovementInformationDto.xtaRowVersion = dataTable.Rows[i].Field<byte[]>("xtaRowVersion");
				eRPToolMovementInformationDto.xtaToolMovementID = dataTable.Rows[i].Field<int>("xtaToolMovementID");
				eRPToolMovementInformationDto.xtaToolID = dataTable.Rows[i].Field<string>("xtaToolID");
				eRPToolMovementInformationDto.xtaWorkCenterID = dataTable.Rows[i].Field<string>("xtaWorkCenterID");
				eRPToolMovementInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPToolMovementInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPToolMovementInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPToolMovementInformationDto> GetToolMovement(Guid toolMovementId)
	{
		ERPToolMovementInformationDto eRPToolMovementInformationDto = new ERPToolMovementInformationDto();
		InitializeParameterLists();
		string[] collection = new string[19]
		{
			"xtaCheckedOutToEmployeeID", "xtaCheckoutReasonID", "xtaCreatedBy", "xtaCreatedDate", "xtaUniqueID", "xtaJobID", "xtaLocation", "xtaMovementDate", "xtaMovementType", "xtaNotesRTF",
			"xtaNotesText", "xtaPlannedReturnDate", "xtaPlantDepartmentID", "xtaPlantID", "xtaProductionDepartmentID", "xtaRowVersion", "xtaToolMovementID", "xtaToolID", "xtaWorkCenterID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xtaUniqueID|C", toolMovementId);
		AddCustomFieldsToSelectList("ToolMovements");
		using (DataTable dataTable = GetAsDataTable("ToolMovements", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPToolMovementInformationDto);
			}
			eRPToolMovementInformationDto.xtaCheckedOutToEmployeeID = dataTable.Rows[0].Field<string>("xtaCheckedOutToEmployeeID");
			eRPToolMovementInformationDto.xtaCheckoutReasonID = dataTable.Rows[0].Field<string>("xtaCheckoutReasonID");
			eRPToolMovementInformationDto.xtaCreatedBy = dataTable.Rows[0].Field<string>("xtaCreatedBy");
			eRPToolMovementInformationDto.xtaCreatedDate = dataTable.Rows[0].Field<DateTime?>("xtaCreatedDate");
			eRPToolMovementInformationDto.xtaUniqueID = dataTable.Rows[0].Field<Guid>("xtaUniqueID");
			eRPToolMovementInformationDto.xtaJobID = dataTable.Rows[0].Field<string>("xtaJobID");
			eRPToolMovementInformationDto.xtaLocation = dataTable.Rows[0].Field<string>("xtaLocation");
			eRPToolMovementInformationDto.xtaMovementDate = dataTable.Rows[0].Field<DateTime?>("xtaMovementDate");
			eRPToolMovementInformationDto.xtaMovementType = dataTable.Rows[0].Field<string>("xtaMovementType");
			eRPToolMovementInformationDto.xtaNotesRTF = dataTable.Rows[0].Field<string>("xtaNotesRTF");
			eRPToolMovementInformationDto.xtaNotesText = dataTable.Rows[0].Field<string>("xtaNotesText");
			eRPToolMovementInformationDto.xtaPlannedReturnDate = dataTable.Rows[0].Field<DateTime?>("xtaPlannedReturnDate");
			eRPToolMovementInformationDto.xtaPlantDepartmentID = dataTable.Rows[0].Field<string>("xtaPlantDepartmentID");
			eRPToolMovementInformationDto.xtaPlantID = dataTable.Rows[0].Field<string>("xtaPlantID");
			eRPToolMovementInformationDto.xtaProductionDepartmentID = dataTable.Rows[0].Field<string>("xtaProductionDepartmentID");
			eRPToolMovementInformationDto.xtaRowVersion = dataTable.Rows[0].Field<byte[]>("xtaRowVersion");
			eRPToolMovementInformationDto.xtaToolMovementID = dataTable.Rows[0].Field<int>("xtaToolMovementID");
			eRPToolMovementInformationDto.xtaToolID = dataTable.Rows[0].Field<string>("xtaToolID");
			eRPToolMovementInformationDto.xtaWorkCenterID = dataTable.Rows[0].Field<string>("xtaWorkCenterID");
			eRPToolMovementInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPToolMovementInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPToolMovementInformationDto);
	}

	public Task<APIValidationInfoDto> SaveToolMovement(ERPToolMovementDto toolMovement)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ToolMovements WHERE xtaUniqueID = " + M1Util.ConvertToLinq(toolMovement.xtaUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xtaToolMovementID"] = toolMovement.xtaToolMovementID;
				toolMovement.xtaUniqueID = ((toolMovement.xtaUniqueID == Guid.Empty) ? Guid.NewGuid() : toolMovement.xtaUniqueID);
				dataRow["xtaUniqueID"] = toolMovement.xtaUniqueID;
				dataRow["xtaCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xtaCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ToolMovement could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (toolMovement.xtaRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ToolMovement is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xtaRowVersion"], toolMovement.xtaRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ToolMovement has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ToolMovement again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xtaCheckedOutToEmployeeID"] = toolMovement.xtaCheckedOutToEmployeeID;
			dataRow["xtaCheckoutReasonID"] = toolMovement.xtaCheckoutReasonID;
			dataRow["xtaJobID"] = toolMovement.xtaJobID;
			dataRow["xtaLocation"] = toolMovement.xtaLocation;
			DataRow dataRow2 = dataRow;
			DateTime? xtaMovementDate = toolMovement.xtaMovementDate;
			dataRow2["xtaMovementDate"] = (xtaMovementDate.HasValue ? ((object)xtaMovementDate.GetValueOrDefault()) : dataRow["xtaMovementDate"]);
			dataRow["xtaMovementType"] = toolMovement.xtaMovementType;
			dataRow["xtaNotesRTF"] = toolMovement.xtaNotesRTF ?? dataRow["xtaNotesRTF"];
			dataRow["xtaNotesText"] = toolMovement.xtaNotesText ?? dataRow["xtaNotesText"];
			DataRow dataRow3 = dataRow;
			xtaMovementDate = toolMovement.xtaPlannedReturnDate;
			dataRow3["xtaPlannedReturnDate"] = (xtaMovementDate.HasValue ? ((object)xtaMovementDate.GetValueOrDefault()) : dataRow["xtaPlannedReturnDate"]);
			dataRow["xtaPlantDepartmentID"] = toolMovement.xtaPlantDepartmentID;
			dataRow["xtaPlantID"] = toolMovement.xtaPlantID;
			dataRow["xtaProductionDepartmentID"] = toolMovement.xtaProductionDepartmentID;
			dataRow["xtaToolID"] = toolMovement.xtaToolID;
			dataRow["xtaWorkCenterID"] = toolMovement.xtaWorkCenterID;
			if (toolMovement.CustomFields != null && toolMovement.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in toolMovement.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ToolMovement [{toolMovement.xtaUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ToolMovement [{toolMovement.xtaUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
