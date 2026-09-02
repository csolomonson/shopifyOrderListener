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

public class ERPProjectTypeRepository : APIBaseRepository, IERPProjectTypeRepository, IAPIBaseRepository, IDisposable
{
	public ERPProjectTypeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesProjectTypeExist(Guid projectTypeId)
	{
		InitializeParameterLists();
		base.filterList.Add("prtUniqueID|C", projectTypeId);
		base.selectList.Add("prtUniqueID");
		return Task.FromResult(GetAsObject("ProjectTypes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPProjectTypeInformationDto>> GetAllProjectTypes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPProjectTypeInformationDto> collection = new List<ERPProjectTypeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[8] { "prtProjectTypeID", "prtCreatedBy", "prtCreatedDate", "prtDescription", "prtUniqueID", "prtInactiveDate", "prtInactive", "prtRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ProjectTypes");
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
		using (DataTable dataTable = GetAsDataTable("ProjectTypes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPProjectTypeInformationDto eRPProjectTypeInformationDto = new ERPProjectTypeInformationDto();
				eRPProjectTypeInformationDto.prtProjectTypeID = dataTable.Rows[i].Field<string>("prtProjectTypeID");
				eRPProjectTypeInformationDto.prtCreatedBy = dataTable.Rows[i].Field<string>("prtCreatedBy");
				eRPProjectTypeInformationDto.prtCreatedDate = dataTable.Rows[i].Field<DateTime?>("prtCreatedDate");
				eRPProjectTypeInformationDto.prtDescription = dataTable.Rows[i].Field<string>("prtDescription");
				eRPProjectTypeInformationDto.prtUniqueID = dataTable.Rows[i].Field<Guid>("prtUniqueID");
				eRPProjectTypeInformationDto.prtInactiveDate = dataTable.Rows[i].Field<DateTime?>("prtInactiveDate");
				eRPProjectTypeInformationDto.prtInactive = dataTable.Rows[i].Field<bool>("prtInactive");
				eRPProjectTypeInformationDto.prtRowVersion = dataTable.Rows[i].Field<byte[]>("prtRowVersion");
				eRPProjectTypeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPProjectTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPProjectTypeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPProjectTypeInformationDto> GetProjectType(Guid projectTypeId)
	{
		ERPProjectTypeInformationDto eRPProjectTypeInformationDto = new ERPProjectTypeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[8] { "prtProjectTypeID", "prtCreatedBy", "prtCreatedDate", "prtDescription", "prtUniqueID", "prtInactiveDate", "prtInactive", "prtRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("prtUniqueID|C", projectTypeId);
		AddCustomFieldsToSelectList("ProjectTypes");
		using (DataTable dataTable = GetAsDataTable("ProjectTypes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPProjectTypeInformationDto);
			}
			eRPProjectTypeInformationDto.prtProjectTypeID = dataTable.Rows[0].Field<string>("prtProjectTypeID");
			eRPProjectTypeInformationDto.prtCreatedBy = dataTable.Rows[0].Field<string>("prtCreatedBy");
			eRPProjectTypeInformationDto.prtCreatedDate = dataTable.Rows[0].Field<DateTime?>("prtCreatedDate");
			eRPProjectTypeInformationDto.prtDescription = dataTable.Rows[0].Field<string>("prtDescription");
			eRPProjectTypeInformationDto.prtUniqueID = dataTable.Rows[0].Field<Guid>("prtUniqueID");
			eRPProjectTypeInformationDto.prtInactiveDate = dataTable.Rows[0].Field<DateTime?>("prtInactiveDate");
			eRPProjectTypeInformationDto.prtInactive = dataTable.Rows[0].Field<bool>("prtInactive");
			eRPProjectTypeInformationDto.prtRowVersion = dataTable.Rows[0].Field<byte[]>("prtRowVersion");
			eRPProjectTypeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPProjectTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPProjectTypeInformationDto);
	}

	public Task<APIValidationInfoDto> SaveProjectType(ERPProjectTypeDto projectType)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ProjectTypes WHERE prtUniqueID = " + M1Util.ConvertToLinq(projectType.prtUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["prtProjectTypeID"] = projectType.prtProjectTypeID.ToUpper();
				projectType.prtUniqueID = ((projectType.prtUniqueID == Guid.Empty) ? Guid.NewGuid() : projectType.prtUniqueID);
				dataRow["prtUniqueID"] = projectType.prtUniqueID;
				dataRow["prtCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["prtCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ProjectType could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (projectType.prtRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ProjectType is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["prtRowVersion"], projectType.prtRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ProjectType has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ProjectType again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["prtDescription"] = projectType.prtDescription;
			DataRow dataRow2 = dataRow;
			DateTime? prtInactiveDate = projectType.prtInactiveDate;
			dataRow2["prtInactiveDate"] = (prtInactiveDate.HasValue ? ((object)prtInactiveDate.GetValueOrDefault()) : dataRow["prtInactiveDate"]);
			dataRow["prtInactive"] = projectType.prtInactive;
			if (projectType.CustomFields != null && projectType.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in projectType.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ProjectType [{projectType.prtUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ProjectType [{projectType.prtUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
