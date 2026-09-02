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

public class ERPGLDepartmentRepository : APIBaseRepository, IERPGLDepartmentRepository, IAPIBaseRepository, IDisposable
{
	public ERPGLDepartmentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesGLDepartmentExist(Guid gLDepartmentId)
	{
		InitializeParameterLists();
		base.filterList.Add("gldUniqueID|C", gLDepartmentId);
		base.selectList.Add("gldUniqueID");
		return Task.FromResult(GetAsObject("GLDepartments", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPGLDepartmentInformationDto>> GetAllGLDepartments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPGLDepartmentInformationDto> collection = new List<ERPGLDepartmentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[6] { "gldGlDepartmentID", "gldCreatedBy", "gldCreatedDate", "gldDescription", "gldUniqueID", "gldRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("GLDepartments");
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
		using (DataTable dataTable = GetAsDataTable("GLDepartments", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPGLDepartmentInformationDto eRPGLDepartmentInformationDto = new ERPGLDepartmentInformationDto();
				eRPGLDepartmentInformationDto.gldGlDepartmentID = dataTable.Rows[i].Field<string>("gldGlDepartmentID");
				eRPGLDepartmentInformationDto.gldCreatedBy = dataTable.Rows[i].Field<string>("gldCreatedBy");
				eRPGLDepartmentInformationDto.gldCreatedDate = dataTable.Rows[i].Field<DateTime?>("gldCreatedDate");
				eRPGLDepartmentInformationDto.gldDescription = dataTable.Rows[i].Field<string>("gldDescription");
				eRPGLDepartmentInformationDto.gldUniqueID = dataTable.Rows[i].Field<Guid>("gldUniqueID");
				eRPGLDepartmentInformationDto.gldRowVersion = dataTable.Rows[i].Field<byte[]>("gldRowVersion");
				eRPGLDepartmentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPGLDepartmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPGLDepartmentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPGLDepartmentInformationDto> GetGLDepartment(Guid gLDepartmentId)
	{
		ERPGLDepartmentInformationDto eRPGLDepartmentInformationDto = new ERPGLDepartmentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[6] { "gldGlDepartmentID", "gldCreatedBy", "gldCreatedDate", "gldDescription", "gldUniqueID", "gldRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("gldUniqueID|C", gLDepartmentId);
		AddCustomFieldsToSelectList("GLDepartments");
		using (DataTable dataTable = GetAsDataTable("GLDepartments", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPGLDepartmentInformationDto);
			}
			eRPGLDepartmentInformationDto.gldGlDepartmentID = dataTable.Rows[0].Field<string>("gldGlDepartmentID");
			eRPGLDepartmentInformationDto.gldCreatedBy = dataTable.Rows[0].Field<string>("gldCreatedBy");
			eRPGLDepartmentInformationDto.gldCreatedDate = dataTable.Rows[0].Field<DateTime?>("gldCreatedDate");
			eRPGLDepartmentInformationDto.gldDescription = dataTable.Rows[0].Field<string>("gldDescription");
			eRPGLDepartmentInformationDto.gldUniqueID = dataTable.Rows[0].Field<Guid>("gldUniqueID");
			eRPGLDepartmentInformationDto.gldRowVersion = dataTable.Rows[0].Field<byte[]>("gldRowVersion");
			eRPGLDepartmentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPGLDepartmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPGLDepartmentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveGLDepartment(ERPGLDepartmentDto gLDepartment)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM GLDepartments WHERE gldUniqueID = " + M1Util.ConvertToLinq(gLDepartment.gldUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["gldGlDepartmentID"] = gLDepartment.gldGlDepartmentID.ToUpper();
				gLDepartment.gldUniqueID = ((gLDepartment.gldUniqueID == Guid.Empty) ? Guid.NewGuid() : gLDepartment.gldUniqueID);
				dataRow["gldUniqueID"] = gLDepartment.gldUniqueID;
				dataRow["gldCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["gldCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The GLDepartment could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (gLDepartment.gldRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the GLDepartment is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["gldRowVersion"], gLDepartment.gldRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the GLDepartment has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the GLDepartment again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["gldDescription"] = gLDepartment.gldDescription;
			if (gLDepartment.CustomFields != null && gLDepartment.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in gLDepartment.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the GLDepartment [{gLDepartment.gldUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the GLDepartment [{gLDepartment.gldUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
