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

public class ERPCustomerGroupRepository : APIBaseRepository, IERPCustomerGroupRepository, IAPIBaseRepository, IDisposable
{
	public ERPCustomerGroupRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesCustomerGroupExist(Guid customerGroupId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmuUniqueID|C", customerGroupId);
		base.selectList.Add("cmuUniqueID");
		return Task.FromResult(GetAsObject("CustomerGroups", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPCustomerGroupInformationDto>> GetAllCustomerGroups(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPCustomerGroupInformationDto> collection = new List<ERPCustomerGroupInformationDto>();
		InitializeParameterLists();
		string[] array = new string[6] { "cmuCustomerGroupID", "cmuCreatedBy", "cmuCreatedDate", "cmuDescription", "cmuUniqueID", "cmuRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("CustomerGroups");
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
		using (DataTable dataTable = GetAsDataTable("CustomerGroups", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPCustomerGroupInformationDto eRPCustomerGroupInformationDto = new ERPCustomerGroupInformationDto();
				eRPCustomerGroupInformationDto.cmuCustomerGroupID = dataTable.Rows[i].Field<string>("cmuCustomerGroupID");
				eRPCustomerGroupInformationDto.cmuCreatedBy = dataTable.Rows[i].Field<string>("cmuCreatedBy");
				eRPCustomerGroupInformationDto.cmuCreatedDate = dataTable.Rows[i].Field<DateTime?>("cmuCreatedDate");
				eRPCustomerGroupInformationDto.cmuDescription = dataTable.Rows[i].Field<string>("cmuDescription");
				eRPCustomerGroupInformationDto.cmuUniqueID = dataTable.Rows[i].Field<Guid>("cmuUniqueID");
				eRPCustomerGroupInformationDto.cmuRowVersion = dataTable.Rows[i].Field<byte[]>("cmuRowVersion");
				eRPCustomerGroupInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPCustomerGroupInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPCustomerGroupInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPCustomerGroupInformationDto> GetCustomerGroup(Guid customerGroupId)
	{
		ERPCustomerGroupInformationDto eRPCustomerGroupInformationDto = new ERPCustomerGroupInformationDto();
		InitializeParameterLists();
		string[] collection = new string[6] { "cmuCustomerGroupID", "cmuCreatedBy", "cmuCreatedDate", "cmuDescription", "cmuUniqueID", "cmuRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("cmuUniqueID|C", customerGroupId);
		AddCustomFieldsToSelectList("CustomerGroups");
		using (DataTable dataTable = GetAsDataTable("CustomerGroups", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPCustomerGroupInformationDto);
			}
			eRPCustomerGroupInformationDto.cmuCustomerGroupID = dataTable.Rows[0].Field<string>("cmuCustomerGroupID");
			eRPCustomerGroupInformationDto.cmuCreatedBy = dataTable.Rows[0].Field<string>("cmuCreatedBy");
			eRPCustomerGroupInformationDto.cmuCreatedDate = dataTable.Rows[0].Field<DateTime?>("cmuCreatedDate");
			eRPCustomerGroupInformationDto.cmuDescription = dataTable.Rows[0].Field<string>("cmuDescription");
			eRPCustomerGroupInformationDto.cmuUniqueID = dataTable.Rows[0].Field<Guid>("cmuUniqueID");
			eRPCustomerGroupInformationDto.cmuRowVersion = dataTable.Rows[0].Field<byte[]>("cmuRowVersion");
			eRPCustomerGroupInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPCustomerGroupInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPCustomerGroupInformationDto);
	}

	public Task<APIValidationInfoDto> SaveCustomerGroup(ERPCustomerGroupDto customerGroup)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM CustomerGroups WHERE cmuUniqueID = " + M1Util.ConvertToLinq(customerGroup.cmuUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cmuCustomerGroupID"] = customerGroup.cmuCustomerGroupID.ToUpper();
				customerGroup.cmuUniqueID = ((customerGroup.cmuUniqueID == Guid.Empty) ? Guid.NewGuid() : customerGroup.cmuUniqueID);
				dataRow["cmuUniqueID"] = customerGroup.cmuUniqueID;
				dataRow["cmuCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cmuCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The CustomerGroup could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (customerGroup.cmuRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the CustomerGroup is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cmuRowVersion"], customerGroup.cmuRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the CustomerGroup has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the CustomerGroup again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["cmuDescription"] = customerGroup.cmuDescription;
			if (customerGroup.CustomFields != null && customerGroup.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in customerGroup.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the CustomerGroup [{customerGroup.cmuUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the CustomerGroup [{customerGroup.cmuUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
