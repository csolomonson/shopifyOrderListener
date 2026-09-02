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

public class ERPProjectRepository : APIBaseRepository, IERPProjectRepository, IAPIBaseRepository, IDisposable
{
	public ERPProjectRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesProjectExist(Guid projectId)
	{
		InitializeParameterLists();
		base.filterList.Add("prpUniqueID|C", projectId);
		base.selectList.Add("prpUniqueID");
		return Task.FromResult(GetAsObject("Projects", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPProjectInformationDto>> GetAllProjects(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPProjectInformationDto> collection = new List<ERPProjectInformationDto>();
		InitializeParameterLists();
		string[] array = new string[18]
		{
			"prpClosedDate", "prpProjectID", "prpContactID", "prpCreatedBy", "prpCreatedDate", "prpDueDate", "prpUniqueID", "prpClosed", "prpLocationID", "prpLongDescriptionRtf",
			"prpLongDescriptionText", "prpOrganizationID", "prpProjectDate", "prpProjectManagerEmployeeID", "prpProjectTypeID", "prpRowVersion", "prpShortDescription", "prpStatus"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Projects");
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
		using (DataTable dataTable = GetAsDataTable("Projects", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPProjectInformationDto eRPProjectInformationDto = new ERPProjectInformationDto();
				eRPProjectInformationDto.prpClosedDate = dataTable.Rows[i].Field<DateTime?>("prpClosedDate");
				eRPProjectInformationDto.prpProjectID = dataTable.Rows[i].Field<string>("prpProjectID");
				eRPProjectInformationDto.prpContactID = dataTable.Rows[i].Field<string>("prpContactID");
				eRPProjectInformationDto.prpCreatedBy = dataTable.Rows[i].Field<string>("prpCreatedBy");
				eRPProjectInformationDto.prpCreatedDate = dataTable.Rows[i].Field<DateTime?>("prpCreatedDate");
				eRPProjectInformationDto.prpDueDate = dataTable.Rows[i].Field<DateTime?>("prpDueDate");
				eRPProjectInformationDto.prpUniqueID = dataTable.Rows[i].Field<Guid>("prpUniqueID");
				eRPProjectInformationDto.prpClosed = dataTable.Rows[i].Field<bool>("prpClosed");
				eRPProjectInformationDto.prpLocationID = dataTable.Rows[i].Field<string>("prpLocationID");
				eRPProjectInformationDto.prpLongDescriptionRtf = dataTable.Rows[i].Field<string>("prpLongDescriptionRtf");
				eRPProjectInformationDto.prpLongDescriptionText = dataTable.Rows[i].Field<string>("prpLongDescriptionText");
				eRPProjectInformationDto.prpOrganizationID = dataTable.Rows[i].Field<string>("prpOrganizationID");
				eRPProjectInformationDto.prpProjectDate = dataTable.Rows[i].Field<DateTime?>("prpProjectDate");
				eRPProjectInformationDto.prpProjectManagerEmployeeID = dataTable.Rows[i].Field<string>("prpProjectManagerEmployeeID");
				eRPProjectInformationDto.prpProjectTypeID = dataTable.Rows[i].Field<string>("prpProjectTypeID");
				eRPProjectInformationDto.prpRowVersion = dataTable.Rows[i].Field<byte[]>("prpRowVersion");
				eRPProjectInformationDto.prpShortDescription = dataTable.Rows[i].Field<string>("prpShortDescription");
				eRPProjectInformationDto.prpStatus = dataTable.Rows[i].Field<string>("prpStatus");
				eRPProjectInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPProjectInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPProjectInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPProjectInformationDto> GetProject(Guid projectId)
	{
		ERPProjectInformationDto eRPProjectInformationDto = new ERPProjectInformationDto();
		InitializeParameterLists();
		string[] collection = new string[18]
		{
			"prpClosedDate", "prpProjectID", "prpContactID", "prpCreatedBy", "prpCreatedDate", "prpDueDate", "prpUniqueID", "prpClosed", "prpLocationID", "prpLongDescriptionRtf",
			"prpLongDescriptionText", "prpOrganizationID", "prpProjectDate", "prpProjectManagerEmployeeID", "prpProjectTypeID", "prpRowVersion", "prpShortDescription", "prpStatus"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("prpUniqueID|C", projectId);
		AddCustomFieldsToSelectList("Projects");
		using (DataTable dataTable = GetAsDataTable("Projects", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPProjectInformationDto);
			}
			eRPProjectInformationDto.prpClosedDate = dataTable.Rows[0].Field<DateTime?>("prpClosedDate");
			eRPProjectInformationDto.prpProjectID = dataTable.Rows[0].Field<string>("prpProjectID");
			eRPProjectInformationDto.prpContactID = dataTable.Rows[0].Field<string>("prpContactID");
			eRPProjectInformationDto.prpCreatedBy = dataTable.Rows[0].Field<string>("prpCreatedBy");
			eRPProjectInformationDto.prpCreatedDate = dataTable.Rows[0].Field<DateTime?>("prpCreatedDate");
			eRPProjectInformationDto.prpDueDate = dataTable.Rows[0].Field<DateTime?>("prpDueDate");
			eRPProjectInformationDto.prpUniqueID = dataTable.Rows[0].Field<Guid>("prpUniqueID");
			eRPProjectInformationDto.prpClosed = dataTable.Rows[0].Field<bool>("prpClosed");
			eRPProjectInformationDto.prpLocationID = dataTable.Rows[0].Field<string>("prpLocationID");
			eRPProjectInformationDto.prpLongDescriptionRtf = dataTable.Rows[0].Field<string>("prpLongDescriptionRtf");
			eRPProjectInformationDto.prpLongDescriptionText = dataTable.Rows[0].Field<string>("prpLongDescriptionText");
			eRPProjectInformationDto.prpOrganizationID = dataTable.Rows[0].Field<string>("prpOrganizationID");
			eRPProjectInformationDto.prpProjectDate = dataTable.Rows[0].Field<DateTime?>("prpProjectDate");
			eRPProjectInformationDto.prpProjectManagerEmployeeID = dataTable.Rows[0].Field<string>("prpProjectManagerEmployeeID");
			eRPProjectInformationDto.prpProjectTypeID = dataTable.Rows[0].Field<string>("prpProjectTypeID");
			eRPProjectInformationDto.prpRowVersion = dataTable.Rows[0].Field<byte[]>("prpRowVersion");
			eRPProjectInformationDto.prpShortDescription = dataTable.Rows[0].Field<string>("prpShortDescription");
			eRPProjectInformationDto.prpStatus = dataTable.Rows[0].Field<string>("prpStatus");
			eRPProjectInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPProjectInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPProjectInformationDto);
	}

	public Task<APIValidationInfoDto> SaveProject(ERPProjectDto project)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Projects WHERE prpUniqueID = " + M1Util.ConvertToLinq(project.prpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["prpProjectID"] = project.prpProjectID.ToUpper();
				project.prpUniqueID = ((project.prpUniqueID == Guid.Empty) ? Guid.NewGuid() : project.prpUniqueID);
				dataRow["prpUniqueID"] = project.prpUniqueID;
				dataRow["prpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["prpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Project could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (project.prpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Project is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["prpRowVersion"], project.prpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Project has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Project again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? prpClosedDate = project.prpClosedDate;
			dataRow2["prpClosedDate"] = (prpClosedDate.HasValue ? ((object)prpClosedDate.GetValueOrDefault()) : dataRow["prpClosedDate"]);
			dataRow["prpContactID"] = project.prpContactID;
			DataRow dataRow3 = dataRow;
			prpClosedDate = project.prpDueDate;
			dataRow3["prpDueDate"] = (prpClosedDate.HasValue ? ((object)prpClosedDate.GetValueOrDefault()) : dataRow["prpDueDate"]);
			dataRow["prpClosed"] = project.prpClosed;
			dataRow["prpLocationID"] = project.prpLocationID;
			dataRow["prpLongDescriptionRtf"] = project.prpLongDescriptionRtf ?? dataRow["prpLongDescriptionRtf"];
			dataRow["prpLongDescriptionText"] = project.prpLongDescriptionText ?? dataRow["prpLongDescriptionText"];
			dataRow["prpOrganizationID"] = project.prpOrganizationID;
			DataRow dataRow4 = dataRow;
			prpClosedDate = project.prpProjectDate;
			dataRow4["prpProjectDate"] = (prpClosedDate.HasValue ? ((object)prpClosedDate.GetValueOrDefault()) : dataRow["prpProjectDate"]);
			dataRow["prpProjectManagerEmployeeID"] = project.prpProjectManagerEmployeeID;
			dataRow["prpProjectTypeID"] = project.prpProjectTypeID;
			dataRow["prpShortDescription"] = project.prpShortDescription;
			dataRow["prpStatus"] = project.prpStatus;
			if (project.CustomFields != null && project.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in project.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Project [{project.prpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Project [{project.prpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
