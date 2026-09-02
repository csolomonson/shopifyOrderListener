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

public class ERPContactMethodRepository : APIBaseRepository, IERPContactMethodRepository, IAPIBaseRepository, IDisposable
{
	public ERPContactMethodRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesContactMethodExist(Guid contactMethodId)
	{
		InitializeParameterLists();
		base.filterList.Add("kbcUniqueID|C", contactMethodId);
		base.selectList.Add("kbcUniqueID");
		return Task.FromResult(GetAsObject("ContactMethods", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPContactMethodInformationDto>> GetAllContactMethods(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPContactMethodInformationDto> collection = new List<ERPContactMethodInformationDto>();
		InitializeParameterLists();
		string[] array = new string[6] { "kbcContactMethodID", "kbcCreatedBy", "kbcCreatedDate", "kbcDescription", "kbcUniqueID", "kbcRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ContactMethods");
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
		using (DataTable dataTable = GetAsDataTable("ContactMethods", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPContactMethodInformationDto eRPContactMethodInformationDto = new ERPContactMethodInformationDto();
				eRPContactMethodInformationDto.kbcContactMethodID = dataTable.Rows[i].Field<string>("kbcContactMethodID");
				eRPContactMethodInformationDto.kbcCreatedBy = dataTable.Rows[i].Field<string>("kbcCreatedBy");
				eRPContactMethodInformationDto.kbcCreatedDate = dataTable.Rows[i].Field<DateTime?>("kbcCreatedDate");
				eRPContactMethodInformationDto.kbcDescription = dataTable.Rows[i].Field<string>("kbcDescription");
				eRPContactMethodInformationDto.kbcUniqueID = dataTable.Rows[i].Field<Guid>("kbcUniqueID");
				eRPContactMethodInformationDto.kbcRowVersion = dataTable.Rows[i].Field<byte[]>("kbcRowVersion");
				eRPContactMethodInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPContactMethodInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPContactMethodInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPContactMethodInformationDto> GetContactMethod(Guid contactMethodId)
	{
		ERPContactMethodInformationDto eRPContactMethodInformationDto = new ERPContactMethodInformationDto();
		InitializeParameterLists();
		string[] collection = new string[6] { "kbcContactMethodID", "kbcCreatedBy", "kbcCreatedDate", "kbcDescription", "kbcUniqueID", "kbcRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("kbcUniqueID|C", contactMethodId);
		AddCustomFieldsToSelectList("ContactMethods");
		using (DataTable dataTable = GetAsDataTable("ContactMethods", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPContactMethodInformationDto);
			}
			eRPContactMethodInformationDto.kbcContactMethodID = dataTable.Rows[0].Field<string>("kbcContactMethodID");
			eRPContactMethodInformationDto.kbcCreatedBy = dataTable.Rows[0].Field<string>("kbcCreatedBy");
			eRPContactMethodInformationDto.kbcCreatedDate = dataTable.Rows[0].Field<DateTime?>("kbcCreatedDate");
			eRPContactMethodInformationDto.kbcDescription = dataTable.Rows[0].Field<string>("kbcDescription");
			eRPContactMethodInformationDto.kbcUniqueID = dataTable.Rows[0].Field<Guid>("kbcUniqueID");
			eRPContactMethodInformationDto.kbcRowVersion = dataTable.Rows[0].Field<byte[]>("kbcRowVersion");
			eRPContactMethodInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPContactMethodInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPContactMethodInformationDto);
	}

	public Task<APIValidationInfoDto> SaveContactMethod(ERPContactMethodDto contactMethod)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ContactMethods WHERE kbcUniqueID = " + M1Util.ConvertToLinq(contactMethod.kbcUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["kbcContactMethodID"] = contactMethod.kbcContactMethodID.ToUpper();
				contactMethod.kbcUniqueID = ((contactMethod.kbcUniqueID == Guid.Empty) ? Guid.NewGuid() : contactMethod.kbcUniqueID);
				dataRow["kbcUniqueID"] = contactMethod.kbcUniqueID;
				dataRow["kbcCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["kbcCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ContactMethod could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (contactMethod.kbcRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ContactMethod is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["kbcRowVersion"], contactMethod.kbcRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ContactMethod has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ContactMethod again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["kbcDescription"] = contactMethod.kbcDescription;
			if (contactMethod.CustomFields != null && contactMethod.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in contactMethod.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ContactMethod [{contactMethod.kbcUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ContactMethod [{contactMethod.kbcUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
