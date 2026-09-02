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

public class ERPOrganizationLocSalesPersonRepository : APIBaseRepository, IERPOrganizationLocSalesPersonRepository, IAPIBaseRepository, IDisposable
{
	public ERPOrganizationLocSalesPersonRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesOrganizationLocSalesPersonExist(Guid organizationLocSalesPersonId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmkUniqueID|C", organizationLocSalesPersonId);
		base.selectList.Add("cmkUniqueID");
		return Task.FromResult(GetAsObject("OrganizationLocSalesPeople", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPOrganizationLocSalesPersonInformationDto>> GetAllOrganizationLocSalesPeople(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPOrganizationLocSalesPersonInformationDto> collection = new List<ERPOrganizationLocSalesPersonInformationDto>();
		InitializeParameterLists();
		string[] array = new string[9] { "cmkCreatedBy", "cmkCreatedDate", "cmkUniqueID", "cmkLocationID", "cmkOrganizationID", "cmkPercent", "cmkRowVersion", "cmkSalesEmployeeID", "cmkSequenceID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("OrganizationLocSalesPeople");
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
		using (DataTable dataTable = GetAsDataTable("OrganizationLocSalesPeople", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPOrganizationLocSalesPersonInformationDto eRPOrganizationLocSalesPersonInformationDto = new ERPOrganizationLocSalesPersonInformationDto();
				eRPOrganizationLocSalesPersonInformationDto.cmkCreatedBy = dataTable.Rows[i].Field<string>("cmkCreatedBy");
				eRPOrganizationLocSalesPersonInformationDto.cmkCreatedDate = dataTable.Rows[i].Field<DateTime?>("cmkCreatedDate");
				eRPOrganizationLocSalesPersonInformationDto.cmkUniqueID = dataTable.Rows[i].Field<Guid>("cmkUniqueID");
				eRPOrganizationLocSalesPersonInformationDto.cmkLocationID = dataTable.Rows[i].Field<string>("cmkLocationID");
				eRPOrganizationLocSalesPersonInformationDto.cmkOrganizationID = dataTable.Rows[i].Field<string>("cmkOrganizationID");
				eRPOrganizationLocSalesPersonInformationDto.cmkPercent = dataTable.Rows[i].Field<decimal>("cmkPercent");
				eRPOrganizationLocSalesPersonInformationDto.cmkRowVersion = dataTable.Rows[i].Field<byte[]>("cmkRowVersion");
				eRPOrganizationLocSalesPersonInformationDto.cmkSalesEmployeeID = dataTable.Rows[i].Field<string>("cmkSalesEmployeeID");
				eRPOrganizationLocSalesPersonInformationDto.cmkSequenceID = dataTable.Rows[i].Field<short>("cmkSequenceID");
				eRPOrganizationLocSalesPersonInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPOrganizationLocSalesPersonInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPOrganizationLocSalesPersonInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPOrganizationLocSalesPersonInformationDto> GetOrganizationLocSalesPerson(Guid organizationLocSalesPersonId)
	{
		ERPOrganizationLocSalesPersonInformationDto eRPOrganizationLocSalesPersonInformationDto = new ERPOrganizationLocSalesPersonInformationDto();
		InitializeParameterLists();
		string[] collection = new string[9] { "cmkCreatedBy", "cmkCreatedDate", "cmkUniqueID", "cmkLocationID", "cmkOrganizationID", "cmkPercent", "cmkRowVersion", "cmkSalesEmployeeID", "cmkSequenceID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("cmkUniqueID|C", organizationLocSalesPersonId);
		AddCustomFieldsToSelectList("OrganizationLocSalesPeople");
		using (DataTable dataTable = GetAsDataTable("OrganizationLocSalesPeople", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPOrganizationLocSalesPersonInformationDto);
			}
			eRPOrganizationLocSalesPersonInformationDto.cmkCreatedBy = dataTable.Rows[0].Field<string>("cmkCreatedBy");
			eRPOrganizationLocSalesPersonInformationDto.cmkCreatedDate = dataTable.Rows[0].Field<DateTime?>("cmkCreatedDate");
			eRPOrganizationLocSalesPersonInformationDto.cmkUniqueID = dataTable.Rows[0].Field<Guid>("cmkUniqueID");
			eRPOrganizationLocSalesPersonInformationDto.cmkLocationID = dataTable.Rows[0].Field<string>("cmkLocationID");
			eRPOrganizationLocSalesPersonInformationDto.cmkOrganizationID = dataTable.Rows[0].Field<string>("cmkOrganizationID");
			eRPOrganizationLocSalesPersonInformationDto.cmkPercent = dataTable.Rows[0].Field<decimal>("cmkPercent");
			eRPOrganizationLocSalesPersonInformationDto.cmkRowVersion = dataTable.Rows[0].Field<byte[]>("cmkRowVersion");
			eRPOrganizationLocSalesPersonInformationDto.cmkSalesEmployeeID = dataTable.Rows[0].Field<string>("cmkSalesEmployeeID");
			eRPOrganizationLocSalesPersonInformationDto.cmkSequenceID = dataTable.Rows[0].Field<short>("cmkSequenceID");
			eRPOrganizationLocSalesPersonInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPOrganizationLocSalesPersonInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPOrganizationLocSalesPersonInformationDto);
	}

	public Task<APIValidationInfoDto> SaveOrganizationLocSalesPerson(ERPOrganizationLocSalesPersonDto organizationLocSalesPerson)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM OrganizationLocSalesPeople WHERE cmkUniqueID = " + M1Util.ConvertToLinq(organizationLocSalesPerson.cmkUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cmkOrganizationID"] = organizationLocSalesPerson.cmkOrganizationID.ToUpper();
				dataRow["cmkLocationID"] = organizationLocSalesPerson.cmkLocationID.ToUpper();
				dataRow["cmkSequenceID"] = organizationLocSalesPerson.cmkSequenceID;
				organizationLocSalesPerson.cmkUniqueID = ((organizationLocSalesPerson.cmkUniqueID == Guid.Empty) ? Guid.NewGuid() : organizationLocSalesPerson.cmkUniqueID);
				dataRow["cmkUniqueID"] = organizationLocSalesPerson.cmkUniqueID;
				dataRow["cmkCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cmkCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The OrganizationLocSalesPerson could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (organizationLocSalesPerson.cmkRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the OrganizationLocSalesPerson is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cmkRowVersion"], organizationLocSalesPerson.cmkRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the OrganizationLocSalesPerson has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the OrganizationLocSalesPerson again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["cmkPercent"] = organizationLocSalesPerson.cmkPercent;
			dataRow["cmkSalesEmployeeID"] = organizationLocSalesPerson.cmkSalesEmployeeID;
			if (organizationLocSalesPerson.CustomFields != null && organizationLocSalesPerson.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in organizationLocSalesPerson.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the OrganizationLocSalesPerson [{organizationLocSalesPerson.cmkUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the OrganizationLocSalesPerson [{organizationLocSalesPerson.cmkUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
