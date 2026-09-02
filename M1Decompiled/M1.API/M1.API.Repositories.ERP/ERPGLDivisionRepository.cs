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

public class ERPGLDivisionRepository : APIBaseRepository, IERPGLDivisionRepository, IAPIBaseRepository, IDisposable
{
	public ERPGLDivisionRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesGLDivisionExist(Guid gLDivisionId)
	{
		InitializeParameterLists();
		base.filterList.Add("glvUniqueID|C", gLDivisionId);
		base.selectList.Add("glvUniqueID");
		return Task.FromResult(GetAsObject("GLDivisions", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPGLDivisionInformationDto>> GetAllGLDivisions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPGLDivisionInformationDto> collection = new List<ERPGLDivisionInformationDto>();
		InitializeParameterLists();
		string[] array = new string[7] { "glvGlDivisionID", "glvCreatedBy", "glvCreatedDate", "glvDescription", "glvUniqueID", "glvRetainedEarningsAccountID", "glvRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("GLDivisions");
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
		using (DataTable dataTable = GetAsDataTable("GLDivisions", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPGLDivisionInformationDto eRPGLDivisionInformationDto = new ERPGLDivisionInformationDto();
				eRPGLDivisionInformationDto.glvGlDivisionID = dataTable.Rows[i].Field<string>("glvGlDivisionID");
				eRPGLDivisionInformationDto.glvCreatedBy = dataTable.Rows[i].Field<string>("glvCreatedBy");
				eRPGLDivisionInformationDto.glvCreatedDate = dataTable.Rows[i].Field<DateTime?>("glvCreatedDate");
				eRPGLDivisionInformationDto.glvDescription = dataTable.Rows[i].Field<string>("glvDescription");
				eRPGLDivisionInformationDto.glvUniqueID = dataTable.Rows[i].Field<Guid>("glvUniqueID");
				eRPGLDivisionInformationDto.glvRetainedEarningsAccountID = dataTable.Rows[i].Field<string>("glvRetainedEarningsAccountID");
				eRPGLDivisionInformationDto.glvRowVersion = dataTable.Rows[i].Field<byte[]>("glvRowVersion");
				eRPGLDivisionInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPGLDivisionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPGLDivisionInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPGLDivisionInformationDto> GetGLDivision(Guid gLDivisionId)
	{
		ERPGLDivisionInformationDto eRPGLDivisionInformationDto = new ERPGLDivisionInformationDto();
		InitializeParameterLists();
		string[] collection = new string[7] { "glvGlDivisionID", "glvCreatedBy", "glvCreatedDate", "glvDescription", "glvUniqueID", "glvRetainedEarningsAccountID", "glvRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("glvUniqueID|C", gLDivisionId);
		AddCustomFieldsToSelectList("GLDivisions");
		using (DataTable dataTable = GetAsDataTable("GLDivisions", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPGLDivisionInformationDto);
			}
			eRPGLDivisionInformationDto.glvGlDivisionID = dataTable.Rows[0].Field<string>("glvGlDivisionID");
			eRPGLDivisionInformationDto.glvCreatedBy = dataTable.Rows[0].Field<string>("glvCreatedBy");
			eRPGLDivisionInformationDto.glvCreatedDate = dataTable.Rows[0].Field<DateTime?>("glvCreatedDate");
			eRPGLDivisionInformationDto.glvDescription = dataTable.Rows[0].Field<string>("glvDescription");
			eRPGLDivisionInformationDto.glvUniqueID = dataTable.Rows[0].Field<Guid>("glvUniqueID");
			eRPGLDivisionInformationDto.glvRetainedEarningsAccountID = dataTable.Rows[0].Field<string>("glvRetainedEarningsAccountID");
			eRPGLDivisionInformationDto.glvRowVersion = dataTable.Rows[0].Field<byte[]>("glvRowVersion");
			eRPGLDivisionInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPGLDivisionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPGLDivisionInformationDto);
	}

	public Task<APIValidationInfoDto> SaveGLDivision(ERPGLDivisionDto gLDivision)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM GLDivisions WHERE glvUniqueID = " + M1Util.ConvertToLinq(gLDivision.glvUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["glvGlDivisionID"] = gLDivision.glvGlDivisionID.ToUpper();
				gLDivision.glvUniqueID = ((gLDivision.glvUniqueID == Guid.Empty) ? Guid.NewGuid() : gLDivision.glvUniqueID);
				dataRow["glvUniqueID"] = gLDivision.glvUniqueID;
				dataRow["glvCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["glvCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The GLDivision could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (gLDivision.glvRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the GLDivision is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["glvRowVersion"], gLDivision.glvRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the GLDivision has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the GLDivision again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["glvDescription"] = gLDivision.glvDescription;
			dataRow["glvRetainedEarningsAccountID"] = gLDivision.glvRetainedEarningsAccountID;
			if (gLDivision.CustomFields != null && gLDivision.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in gLDivision.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the GLDivision [{gLDivision.glvUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the GLDivision [{gLDivision.glvUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
