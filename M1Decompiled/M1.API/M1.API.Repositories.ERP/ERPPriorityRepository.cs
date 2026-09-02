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

public class ERPPriorityRepository : APIBaseRepository, IERPPriorityRepository, IAPIBaseRepository, IDisposable
{
	public ERPPriorityRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPriorityExist(Guid priorityId)
	{
		InitializeParameterLists();
		base.filterList.Add("kbrUniqueID|C", priorityId);
		base.selectList.Add("kbrUniqueID");
		return Task.FromResult(GetAsObject("Priorities", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPriorityInformationDto>> GetAllPriorities(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPriorityInformationDto> collection = new List<ERPPriorityInformationDto>();
		InitializeParameterLists();
		string[] array = new string[6] { "kbrCreatedBy", "kbrCreatedDate", "kbrDescription", "kbrUniqueID", "kbrRowVersion", "kbrPriorityID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Priorities");
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
		using (DataTable dataTable = GetAsDataTable("Priorities", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPriorityInformationDto eRPPriorityInformationDto = new ERPPriorityInformationDto();
				eRPPriorityInformationDto.kbrCreatedBy = dataTable.Rows[i].Field<string>("kbrCreatedBy");
				eRPPriorityInformationDto.kbrCreatedDate = dataTable.Rows[i].Field<DateTime?>("kbrCreatedDate");
				eRPPriorityInformationDto.kbrDescription = dataTable.Rows[i].Field<string>("kbrDescription");
				eRPPriorityInformationDto.kbrUniqueID = dataTable.Rows[i].Field<Guid>("kbrUniqueID");
				eRPPriorityInformationDto.kbrRowVersion = dataTable.Rows[i].Field<byte[]>("kbrRowVersion");
				eRPPriorityInformationDto.kbrPriorityID = dataTable.Rows[i].Field<byte>("kbrPriorityID");
				eRPPriorityInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPriorityInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPriorityInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPriorityInformationDto> GetPriority(Guid priorityId)
	{
		ERPPriorityInformationDto eRPPriorityInformationDto = new ERPPriorityInformationDto();
		InitializeParameterLists();
		string[] collection = new string[6] { "kbrCreatedBy", "kbrCreatedDate", "kbrDescription", "kbrUniqueID", "kbrRowVersion", "kbrPriorityID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("kbrUniqueID|C", priorityId);
		AddCustomFieldsToSelectList("Priorities");
		using (DataTable dataTable = GetAsDataTable("Priorities", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPriorityInformationDto);
			}
			eRPPriorityInformationDto.kbrCreatedBy = dataTable.Rows[0].Field<string>("kbrCreatedBy");
			eRPPriorityInformationDto.kbrCreatedDate = dataTable.Rows[0].Field<DateTime?>("kbrCreatedDate");
			eRPPriorityInformationDto.kbrDescription = dataTable.Rows[0].Field<string>("kbrDescription");
			eRPPriorityInformationDto.kbrUniqueID = dataTable.Rows[0].Field<Guid>("kbrUniqueID");
			eRPPriorityInformationDto.kbrRowVersion = dataTable.Rows[0].Field<byte[]>("kbrRowVersion");
			eRPPriorityInformationDto.kbrPriorityID = dataTable.Rows[0].Field<byte>("kbrPriorityID");
			eRPPriorityInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPriorityInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPriorityInformationDto);
	}

	public Task<APIValidationInfoDto> SavePriority(ERPPriorityDto priority)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Priorities WHERE kbrUniqueID = " + M1Util.ConvertToLinq(priority.kbrUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["kbrPriorityID"] = priority.kbrPriorityID;
				priority.kbrUniqueID = ((priority.kbrUniqueID == Guid.Empty) ? Guid.NewGuid() : priority.kbrUniqueID);
				dataRow["kbrUniqueID"] = priority.kbrUniqueID;
				dataRow["kbrCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["kbrCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Priority could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (priority.kbrRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Priority is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["kbrRowVersion"], priority.kbrRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Priority has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Priority again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["kbrDescription"] = priority.kbrDescription;
			if (priority.CustomFields != null && priority.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in priority.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Priority [{priority.kbrUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Priority [{priority.kbrUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
