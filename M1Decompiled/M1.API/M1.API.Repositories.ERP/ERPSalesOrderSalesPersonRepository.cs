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

public class ERPSalesOrderSalesPersonRepository : APIBaseRepository, IERPSalesOrderSalesPersonRepository, IAPIBaseRepository, IDisposable
{
	public ERPSalesOrderSalesPersonRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesSalesOrderSalesPersonExist(Guid salesOrderSalesPersonId)
	{
		InitializeParameterLists();
		base.filterList.Add("omiUniqueID|C", salesOrderSalesPersonId);
		base.selectList.Add("omiUniqueID");
		return Task.FromResult(GetAsObject("SalesOrderSalesPeople", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPSalesOrderSalesPersonInformationDto>> GetAllSalesOrderSalesPeople(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPSalesOrderSalesPersonInformationDto> collection = new List<ERPSalesOrderSalesPersonInformationDto>();
		InitializeParameterLists();
		string[] array = new string[9] { "omiCreatedBy", "omiCreatedDate", "omiUniqueID", "omiClosed", "omiPercent", "omiRowVersion", "omiSalesEmployeeID", "omiSalesOrderID", "omiSequenceID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("SalesOrderSalesPeople");
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
		using (DataTable dataTable = GetAsDataTable("SalesOrderSalesPeople", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPSalesOrderSalesPersonInformationDto eRPSalesOrderSalesPersonInformationDto = new ERPSalesOrderSalesPersonInformationDto();
				eRPSalesOrderSalesPersonInformationDto.omiCreatedBy = dataTable.Rows[i].Field<string>("omiCreatedBy");
				eRPSalesOrderSalesPersonInformationDto.omiCreatedDate = dataTable.Rows[i].Field<DateTime?>("omiCreatedDate");
				eRPSalesOrderSalesPersonInformationDto.omiUniqueID = dataTable.Rows[i].Field<Guid>("omiUniqueID");
				eRPSalesOrderSalesPersonInformationDto.omiClosed = dataTable.Rows[i].Field<bool>("omiClosed");
				eRPSalesOrderSalesPersonInformationDto.omiPercent = dataTable.Rows[i].Field<decimal>("omiPercent");
				eRPSalesOrderSalesPersonInformationDto.omiRowVersion = dataTable.Rows[i].Field<byte[]>("omiRowVersion");
				eRPSalesOrderSalesPersonInformationDto.omiSalesEmployeeID = dataTable.Rows[i].Field<string>("omiSalesEmployeeID");
				eRPSalesOrderSalesPersonInformationDto.omiSalesOrderID = dataTable.Rows[i].Field<string>("omiSalesOrderID");
				eRPSalesOrderSalesPersonInformationDto.omiSequenceID = dataTable.Rows[i].Field<short>("omiSequenceID");
				eRPSalesOrderSalesPersonInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPSalesOrderSalesPersonInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPSalesOrderSalesPersonInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPSalesOrderSalesPersonInformationDto> GetSalesOrderSalesPerson(Guid salesOrderSalesPersonId)
	{
		ERPSalesOrderSalesPersonInformationDto eRPSalesOrderSalesPersonInformationDto = new ERPSalesOrderSalesPersonInformationDto();
		InitializeParameterLists();
		string[] collection = new string[9] { "omiCreatedBy", "omiCreatedDate", "omiUniqueID", "omiClosed", "omiPercent", "omiRowVersion", "omiSalesEmployeeID", "omiSalesOrderID", "omiSequenceID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("omiUniqueID|C", salesOrderSalesPersonId);
		AddCustomFieldsToSelectList("SalesOrderSalesPeople");
		using (DataTable dataTable = GetAsDataTable("SalesOrderSalesPeople", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPSalesOrderSalesPersonInformationDto);
			}
			eRPSalesOrderSalesPersonInformationDto.omiCreatedBy = dataTable.Rows[0].Field<string>("omiCreatedBy");
			eRPSalesOrderSalesPersonInformationDto.omiCreatedDate = dataTable.Rows[0].Field<DateTime?>("omiCreatedDate");
			eRPSalesOrderSalesPersonInformationDto.omiUniqueID = dataTable.Rows[0].Field<Guid>("omiUniqueID");
			eRPSalesOrderSalesPersonInformationDto.omiClosed = dataTable.Rows[0].Field<bool>("omiClosed");
			eRPSalesOrderSalesPersonInformationDto.omiPercent = dataTable.Rows[0].Field<decimal>("omiPercent");
			eRPSalesOrderSalesPersonInformationDto.omiRowVersion = dataTable.Rows[0].Field<byte[]>("omiRowVersion");
			eRPSalesOrderSalesPersonInformationDto.omiSalesEmployeeID = dataTable.Rows[0].Field<string>("omiSalesEmployeeID");
			eRPSalesOrderSalesPersonInformationDto.omiSalesOrderID = dataTable.Rows[0].Field<string>("omiSalesOrderID");
			eRPSalesOrderSalesPersonInformationDto.omiSequenceID = dataTable.Rows[0].Field<short>("omiSequenceID");
			eRPSalesOrderSalesPersonInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPSalesOrderSalesPersonInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPSalesOrderSalesPersonInformationDto);
	}

	public Task<APIValidationInfoDto> SaveSalesOrderSalesPerson(ERPSalesOrderSalesPersonDto salesOrderSalesPerson)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM SalesOrderSalesPeople WHERE omiUniqueID = " + M1Util.ConvertToLinq(salesOrderSalesPerson.omiUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["omiSalesOrderID"] = salesOrderSalesPerson.omiSalesOrderID.ToUpper();
				dataRow["omiSequenceID"] = salesOrderSalesPerson.omiSequenceID;
				salesOrderSalesPerson.omiUniqueID = ((salesOrderSalesPerson.omiUniqueID == Guid.Empty) ? Guid.NewGuid() : salesOrderSalesPerson.omiUniqueID);
				dataRow["omiUniqueID"] = salesOrderSalesPerson.omiUniqueID;
				dataRow["omiCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["omiCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The SalesOrderSalesPerson could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (salesOrderSalesPerson.omiRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the SalesOrderSalesPerson is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["omiRowVersion"], salesOrderSalesPerson.omiRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the SalesOrderSalesPerson has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the SalesOrderSalesPerson again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["omiClosed"] = salesOrderSalesPerson.omiClosed;
			dataRow["omiPercent"] = salesOrderSalesPerson.omiPercent;
			dataRow["omiSalesEmployeeID"] = salesOrderSalesPerson.omiSalesEmployeeID;
			if (salesOrderSalesPerson.CustomFields != null && salesOrderSalesPerson.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in salesOrderSalesPerson.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the SalesOrderSalesPerson [{salesOrderSalesPerson.omiUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the SalesOrderSalesPerson [{salesOrderSalesPerson.omiUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
