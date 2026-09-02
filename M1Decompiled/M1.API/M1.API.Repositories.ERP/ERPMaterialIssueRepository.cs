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

public class ERPMaterialIssueRepository : APIBaseRepository, IERPMaterialIssueRepository, IAPIBaseRepository, IDisposable
{
	public ERPMaterialIssueRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesMaterialIssueExist(Guid materialIssueId)
	{
		InitializeParameterLists();
		base.filterList.Add("iniUniqueID|C", materialIssueId);
		base.selectList.Add("iniUniqueID");
		return Task.FromResult(GetAsObject("MaterialIssues", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPMaterialIssueInformationDto>> GetAllMaterialIssues(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPMaterialIssueInformationDto> collection = new List<ERPMaterialIssueInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"iniMaterialIssueID", "iniCreatedBy", "iniCreatedDate", "iniUniqueID", "iniPosted", "iniReversalEntry", "iniReversed", "iniMaterialIssueDate", "iniPostedDate", "iniRowVersion",
			"iniSourceTableUniqueID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("MaterialIssues");
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
		using (DataTable dataTable = GetAsDataTable("MaterialIssues", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPMaterialIssueInformationDto eRPMaterialIssueInformationDto = new ERPMaterialIssueInformationDto();
				eRPMaterialIssueInformationDto.iniMaterialIssueID = dataTable.Rows[i].Field<string>("iniMaterialIssueID");
				eRPMaterialIssueInformationDto.iniCreatedBy = dataTable.Rows[i].Field<string>("iniCreatedBy");
				eRPMaterialIssueInformationDto.iniCreatedDate = dataTable.Rows[i].Field<DateTime?>("iniCreatedDate");
				eRPMaterialIssueInformationDto.iniUniqueID = dataTable.Rows[i].Field<Guid>("iniUniqueID");
				eRPMaterialIssueInformationDto.iniPosted = dataTable.Rows[i].Field<bool>("iniPosted");
				eRPMaterialIssueInformationDto.iniReversalEntry = dataTable.Rows[i].Field<bool>("iniReversalEntry");
				eRPMaterialIssueInformationDto.iniReversed = dataTable.Rows[i].Field<bool>("iniReversed");
				eRPMaterialIssueInformationDto.iniMaterialIssueDate = dataTable.Rows[i].Field<DateTime?>("iniMaterialIssueDate");
				eRPMaterialIssueInformationDto.iniPostedDate = dataTable.Rows[i].Field<DateTime?>("iniPostedDate");
				eRPMaterialIssueInformationDto.iniRowVersion = dataTable.Rows[i].Field<byte[]>("iniRowVersion");
				eRPMaterialIssueInformationDto.iniSourceTableUniqueID = dataTable.Rows[i].Field<Guid>("iniSourceTableUniqueID");
				eRPMaterialIssueInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPMaterialIssueInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPMaterialIssueInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPMaterialIssueInformationDto> GetMaterialIssue(Guid materialIssueId)
	{
		ERPMaterialIssueInformationDto eRPMaterialIssueInformationDto = new ERPMaterialIssueInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"iniMaterialIssueID", "iniCreatedBy", "iniCreatedDate", "iniUniqueID", "iniPosted", "iniReversalEntry", "iniReversed", "iniMaterialIssueDate", "iniPostedDate", "iniRowVersion",
			"iniSourceTableUniqueID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("iniUniqueID|C", materialIssueId);
		AddCustomFieldsToSelectList("MaterialIssues");
		using (DataTable dataTable = GetAsDataTable("MaterialIssues", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPMaterialIssueInformationDto);
			}
			eRPMaterialIssueInformationDto.iniMaterialIssueID = dataTable.Rows[0].Field<string>("iniMaterialIssueID");
			eRPMaterialIssueInformationDto.iniCreatedBy = dataTable.Rows[0].Field<string>("iniCreatedBy");
			eRPMaterialIssueInformationDto.iniCreatedDate = dataTable.Rows[0].Field<DateTime?>("iniCreatedDate");
			eRPMaterialIssueInformationDto.iniUniqueID = dataTable.Rows[0].Field<Guid>("iniUniqueID");
			eRPMaterialIssueInformationDto.iniPosted = dataTable.Rows[0].Field<bool>("iniPosted");
			eRPMaterialIssueInformationDto.iniReversalEntry = dataTable.Rows[0].Field<bool>("iniReversalEntry");
			eRPMaterialIssueInformationDto.iniReversed = dataTable.Rows[0].Field<bool>("iniReversed");
			eRPMaterialIssueInformationDto.iniMaterialIssueDate = dataTable.Rows[0].Field<DateTime?>("iniMaterialIssueDate");
			eRPMaterialIssueInformationDto.iniPostedDate = dataTable.Rows[0].Field<DateTime?>("iniPostedDate");
			eRPMaterialIssueInformationDto.iniRowVersion = dataTable.Rows[0].Field<byte[]>("iniRowVersion");
			eRPMaterialIssueInformationDto.iniSourceTableUniqueID = dataTable.Rows[0].Field<Guid>("iniSourceTableUniqueID");
			eRPMaterialIssueInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPMaterialIssueInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPMaterialIssueInformationDto);
	}

	public Task<APIValidationInfoDto> SaveMaterialIssue(ERPMaterialIssueDto materialIssue)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM MaterialIssues WHERE iniUniqueID = " + M1Util.ConvertToLinq(materialIssue.iniUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["iniMaterialIssueID"] = materialIssue.iniMaterialIssueID.ToUpper();
				materialIssue.iniUniqueID = ((materialIssue.iniUniqueID == Guid.Empty) ? Guid.NewGuid() : materialIssue.iniUniqueID);
				dataRow["iniUniqueID"] = materialIssue.iniUniqueID;
				dataRow["iniCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["iniCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The MaterialIssue could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (materialIssue.iniRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the MaterialIssue is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["iniRowVersion"], materialIssue.iniRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the MaterialIssue has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the MaterialIssue again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["iniPosted"] = materialIssue.iniPosted;
			dataRow["iniReversalEntry"] = materialIssue.iniReversalEntry;
			dataRow["iniReversed"] = materialIssue.iniReversed;
			DataRow dataRow2 = dataRow;
			DateTime? iniMaterialIssueDate = materialIssue.iniMaterialIssueDate;
			dataRow2["iniMaterialIssueDate"] = (iniMaterialIssueDate.HasValue ? ((object)iniMaterialIssueDate.GetValueOrDefault()) : dataRow["iniMaterialIssueDate"]);
			DataRow dataRow3 = dataRow;
			iniMaterialIssueDate = materialIssue.iniPostedDate;
			dataRow3["iniPostedDate"] = (iniMaterialIssueDate.HasValue ? ((object)iniMaterialIssueDate.GetValueOrDefault()) : dataRow["iniPostedDate"]);
			dataRow["iniSourceTableUniqueID"] = materialIssue.iniSourceTableUniqueID;
			if (materialIssue.CustomFields != null && materialIssue.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in materialIssue.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the MaterialIssue [{materialIssue.iniUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the MaterialIssue [{materialIssue.iniUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
