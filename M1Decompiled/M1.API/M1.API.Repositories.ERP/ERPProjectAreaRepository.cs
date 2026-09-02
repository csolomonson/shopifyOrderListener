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

public class ERPProjectAreaRepository : APIBaseRepository, IERPProjectAreaRepository, IAPIBaseRepository, IDisposable
{
	public ERPProjectAreaRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesProjectAreaExist(Guid projectAreaId)
	{
		InitializeParameterLists();
		base.filterList.Add("praUniqueID|C", projectAreaId);
		base.selectList.Add("praUniqueID");
		return Task.FromResult(GetAsObject("ProjectAreas", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPProjectAreaInformationDto>> GetAllProjectAreas(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPProjectAreaInformationDto> collection = new List<ERPProjectAreaInformationDto>();
		InitializeParameterLists();
		string[] array = new string[7] { "praProjectAreaID", "praCreatedBy", "praCreatedDate", "praDescription", "praUniqueID", "praProjectID", "praRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ProjectAreas");
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
		using (DataTable dataTable = GetAsDataTable("ProjectAreas", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPProjectAreaInformationDto eRPProjectAreaInformationDto = new ERPProjectAreaInformationDto();
				eRPProjectAreaInformationDto.praProjectAreaID = dataTable.Rows[i].Field<string>("praProjectAreaID");
				eRPProjectAreaInformationDto.praCreatedBy = dataTable.Rows[i].Field<string>("praCreatedBy");
				eRPProjectAreaInformationDto.praCreatedDate = dataTable.Rows[i].Field<DateTime?>("praCreatedDate");
				eRPProjectAreaInformationDto.praDescription = dataTable.Rows[i].Field<string>("praDescription");
				eRPProjectAreaInformationDto.praUniqueID = dataTable.Rows[i].Field<Guid>("praUniqueID");
				eRPProjectAreaInformationDto.praProjectID = dataTable.Rows[i].Field<string>("praProjectID");
				eRPProjectAreaInformationDto.praRowVersion = dataTable.Rows[i].Field<byte[]>("praRowVersion");
				eRPProjectAreaInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPProjectAreaInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPProjectAreaInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPProjectAreaInformationDto> GetProjectArea(Guid projectAreaId)
	{
		ERPProjectAreaInformationDto eRPProjectAreaInformationDto = new ERPProjectAreaInformationDto();
		InitializeParameterLists();
		string[] collection = new string[7] { "praProjectAreaID", "praCreatedBy", "praCreatedDate", "praDescription", "praUniqueID", "praProjectID", "praRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("praUniqueID|C", projectAreaId);
		AddCustomFieldsToSelectList("ProjectAreas");
		using (DataTable dataTable = GetAsDataTable("ProjectAreas", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPProjectAreaInformationDto);
			}
			eRPProjectAreaInformationDto.praProjectAreaID = dataTable.Rows[0].Field<string>("praProjectAreaID");
			eRPProjectAreaInformationDto.praCreatedBy = dataTable.Rows[0].Field<string>("praCreatedBy");
			eRPProjectAreaInformationDto.praCreatedDate = dataTable.Rows[0].Field<DateTime?>("praCreatedDate");
			eRPProjectAreaInformationDto.praDescription = dataTable.Rows[0].Field<string>("praDescription");
			eRPProjectAreaInformationDto.praUniqueID = dataTable.Rows[0].Field<Guid>("praUniqueID");
			eRPProjectAreaInformationDto.praProjectID = dataTable.Rows[0].Field<string>("praProjectID");
			eRPProjectAreaInformationDto.praRowVersion = dataTable.Rows[0].Field<byte[]>("praRowVersion");
			eRPProjectAreaInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPProjectAreaInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPProjectAreaInformationDto);
	}

	public Task<APIValidationInfoDto> SaveProjectArea(ERPProjectAreaDto projectArea)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ProjectAreas WHERE praUniqueID = " + M1Util.ConvertToLinq(projectArea.praUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["praProjectID"] = projectArea.praProjectID.ToUpper();
				dataRow["praProjectAreaID"] = projectArea.praProjectAreaID.ToUpper();
				projectArea.praUniqueID = ((projectArea.praUniqueID == Guid.Empty) ? Guid.NewGuid() : projectArea.praUniqueID);
				dataRow["praUniqueID"] = projectArea.praUniqueID;
				dataRow["praCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["praCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ProjectArea could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (projectArea.praRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ProjectArea is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["praRowVersion"], projectArea.praRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ProjectArea has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ProjectArea again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["praDescription"] = projectArea.praDescription;
			if (projectArea.CustomFields != null && projectArea.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in projectArea.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ProjectArea [{projectArea.praUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ProjectArea [{projectArea.praUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
