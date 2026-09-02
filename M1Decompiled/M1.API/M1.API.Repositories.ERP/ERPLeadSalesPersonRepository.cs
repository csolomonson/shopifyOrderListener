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

public class ERPLeadSalesPersonRepository : APIBaseRepository, IERPLeadSalesPersonRepository, IAPIBaseRepository, IDisposable
{
	public ERPLeadSalesPersonRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesLeadSalesPersonExist(Guid leadSalesPersonId)
	{
		InitializeParameterLists();
		base.filterList.Add("lojUniqueID|C", leadSalesPersonId);
		base.selectList.Add("lojUniqueID");
		return Task.FromResult(GetAsObject("LeadSalesPeople", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPLeadSalesPersonInformationDto>> GetAllLeadSalesPeople(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPLeadSalesPersonInformationDto> collection = new List<ERPLeadSalesPersonInformationDto>();
		InitializeParameterLists();
		string[] array = new string[8] { "lojCreatedBy", "lojCreatedDate", "lojUniqueID", "lojLeadID", "lojPercent", "lojRowVersion", "lojSalesEmployeeID", "lojSequenceID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("LeadSalesPeople");
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
		using (DataTable dataTable = GetAsDataTable("LeadSalesPeople", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPLeadSalesPersonInformationDto eRPLeadSalesPersonInformationDto = new ERPLeadSalesPersonInformationDto();
				eRPLeadSalesPersonInformationDto.lojCreatedBy = dataTable.Rows[i].Field<string>("lojCreatedBy");
				eRPLeadSalesPersonInformationDto.lojCreatedDate = dataTable.Rows[i].Field<DateTime?>("lojCreatedDate");
				eRPLeadSalesPersonInformationDto.lojUniqueID = dataTable.Rows[i].Field<Guid>("lojUniqueID");
				eRPLeadSalesPersonInformationDto.lojLeadID = dataTable.Rows[i].Field<string>("lojLeadID");
				eRPLeadSalesPersonInformationDto.lojPercent = dataTable.Rows[i].Field<decimal>("lojPercent");
				eRPLeadSalesPersonInformationDto.lojRowVersion = dataTable.Rows[i].Field<byte[]>("lojRowVersion");
				eRPLeadSalesPersonInformationDto.lojSalesEmployeeID = dataTable.Rows[i].Field<string>("lojSalesEmployeeID");
				eRPLeadSalesPersonInformationDto.lojSequenceID = dataTable.Rows[i].Field<short>("lojSequenceID");
				eRPLeadSalesPersonInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPLeadSalesPersonInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPLeadSalesPersonInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPLeadSalesPersonInformationDto> GetLeadSalesPerson(Guid leadSalesPersonId)
	{
		ERPLeadSalesPersonInformationDto eRPLeadSalesPersonInformationDto = new ERPLeadSalesPersonInformationDto();
		InitializeParameterLists();
		string[] collection = new string[8] { "lojCreatedBy", "lojCreatedDate", "lojUniqueID", "lojLeadID", "lojPercent", "lojRowVersion", "lojSalesEmployeeID", "lojSequenceID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("lojUniqueID|C", leadSalesPersonId);
		AddCustomFieldsToSelectList("LeadSalesPeople");
		using (DataTable dataTable = GetAsDataTable("LeadSalesPeople", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPLeadSalesPersonInformationDto);
			}
			eRPLeadSalesPersonInformationDto.lojCreatedBy = dataTable.Rows[0].Field<string>("lojCreatedBy");
			eRPLeadSalesPersonInformationDto.lojCreatedDate = dataTable.Rows[0].Field<DateTime?>("lojCreatedDate");
			eRPLeadSalesPersonInformationDto.lojUniqueID = dataTable.Rows[0].Field<Guid>("lojUniqueID");
			eRPLeadSalesPersonInformationDto.lojLeadID = dataTable.Rows[0].Field<string>("lojLeadID");
			eRPLeadSalesPersonInformationDto.lojPercent = dataTable.Rows[0].Field<decimal>("lojPercent");
			eRPLeadSalesPersonInformationDto.lojRowVersion = dataTable.Rows[0].Field<byte[]>("lojRowVersion");
			eRPLeadSalesPersonInformationDto.lojSalesEmployeeID = dataTable.Rows[0].Field<string>("lojSalesEmployeeID");
			eRPLeadSalesPersonInformationDto.lojSequenceID = dataTable.Rows[0].Field<short>("lojSequenceID");
			eRPLeadSalesPersonInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPLeadSalesPersonInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPLeadSalesPersonInformationDto);
	}

	public Task<APIValidationInfoDto> SaveLeadSalesPerson(ERPLeadSalesPersonDto leadSalesPerson)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM LeadSalesPeople WHERE lojUniqueID = " + M1Util.ConvertToLinq(leadSalesPerson.lojUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["lojLeadID"] = leadSalesPerson.lojLeadID.ToUpper();
				dataRow["lojSequenceID"] = leadSalesPerson.lojSequenceID;
				leadSalesPerson.lojUniqueID = ((leadSalesPerson.lojUniqueID == Guid.Empty) ? Guid.NewGuid() : leadSalesPerson.lojUniqueID);
				dataRow["lojUniqueID"] = leadSalesPerson.lojUniqueID;
				dataRow["lojCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["lojCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The LeadSalesPerson could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (leadSalesPerson.lojRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the LeadSalesPerson is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["lojRowVersion"], leadSalesPerson.lojRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the LeadSalesPerson has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the LeadSalesPerson again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["lojPercent"] = leadSalesPerson.lojPercent;
			dataRow["lojSalesEmployeeID"] = leadSalesPerson.lojSalesEmployeeID;
			if (leadSalesPerson.CustomFields != null && leadSalesPerson.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in leadSalesPerson.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the LeadSalesPerson [{leadSalesPerson.lojUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the LeadSalesPerson [{leadSalesPerson.lojUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
