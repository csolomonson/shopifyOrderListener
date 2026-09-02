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

public class ERPSalesOrderJobLinkRepository : APIBaseRepository, IERPSalesOrderJobLinkRepository, IAPIBaseRepository, IDisposable
{
	public ERPSalesOrderJobLinkRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesSalesOrderJobLinkExist(Guid salesOrderJobLinkId)
	{
		InitializeParameterLists();
		base.filterList.Add("omjUniqueID|C", salesOrderJobLinkId);
		base.selectList.Add("omjUniqueID");
		return Task.FromResult(GetAsObject("SalesOrderJobLinks", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPSalesOrderJobLinkInformationDto>> GetAllSalesOrderJobLinks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPSalesOrderJobLinkInformationDto> collection = new List<ERPSalesOrderJobLinkInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"omjCreatedBy", "omjCreatedDate", "omjUniqueID", "omjClosed", "omjJobID", "omjLinkType", "omjRowVersion", "omjSalesOrderDeliveryID", "omjSalesOrderID", "omjSalesOrderLineID",
			"omjSalesOrderJobLinkID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("SalesOrderJobLinks");
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
		using (DataTable dataTable = GetAsDataTable("SalesOrderJobLinks", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPSalesOrderJobLinkInformationDto eRPSalesOrderJobLinkInformationDto = new ERPSalesOrderJobLinkInformationDto();
				eRPSalesOrderJobLinkInformationDto.omjCreatedBy = dataTable.Rows[i].Field<string>("omjCreatedBy");
				eRPSalesOrderJobLinkInformationDto.omjCreatedDate = dataTable.Rows[i].Field<DateTime?>("omjCreatedDate");
				eRPSalesOrderJobLinkInformationDto.omjUniqueID = dataTable.Rows[i].Field<Guid>("omjUniqueID");
				eRPSalesOrderJobLinkInformationDto.omjClosed = dataTable.Rows[i].Field<bool>("omjClosed");
				eRPSalesOrderJobLinkInformationDto.omjJobID = dataTable.Rows[i].Field<string>("omjJobID");
				eRPSalesOrderJobLinkInformationDto.omjLinkType = dataTable.Rows[i].Field<byte>("omjLinkType");
				eRPSalesOrderJobLinkInformationDto.omjRowVersion = dataTable.Rows[i].Field<byte[]>("omjRowVersion");
				eRPSalesOrderJobLinkInformationDto.omjSalesOrderDeliveryID = dataTable.Rows[i].Field<short>("omjSalesOrderDeliveryID");
				eRPSalesOrderJobLinkInformationDto.omjSalesOrderID = dataTable.Rows[i].Field<string>("omjSalesOrderID");
				eRPSalesOrderJobLinkInformationDto.omjSalesOrderLineID = dataTable.Rows[i].Field<short>("omjSalesOrderLineID");
				eRPSalesOrderJobLinkInformationDto.omjSalesOrderJobLinkID = dataTable.Rows[i].Field<int>("omjSalesOrderJobLinkID");
				eRPSalesOrderJobLinkInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPSalesOrderJobLinkInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPSalesOrderJobLinkInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPSalesOrderJobLinkInformationDto> GetSalesOrderJobLink(Guid salesOrderJobLinkId)
	{
		ERPSalesOrderJobLinkInformationDto eRPSalesOrderJobLinkInformationDto = new ERPSalesOrderJobLinkInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"omjCreatedBy", "omjCreatedDate", "omjUniqueID", "omjClosed", "omjJobID", "omjLinkType", "omjRowVersion", "omjSalesOrderDeliveryID", "omjSalesOrderID", "omjSalesOrderLineID",
			"omjSalesOrderJobLinkID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("omjUniqueID|C", salesOrderJobLinkId);
		AddCustomFieldsToSelectList("SalesOrderJobLinks");
		using (DataTable dataTable = GetAsDataTable("SalesOrderJobLinks", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPSalesOrderJobLinkInformationDto);
			}
			eRPSalesOrderJobLinkInformationDto.omjCreatedBy = dataTable.Rows[0].Field<string>("omjCreatedBy");
			eRPSalesOrderJobLinkInformationDto.omjCreatedDate = dataTable.Rows[0].Field<DateTime?>("omjCreatedDate");
			eRPSalesOrderJobLinkInformationDto.omjUniqueID = dataTable.Rows[0].Field<Guid>("omjUniqueID");
			eRPSalesOrderJobLinkInformationDto.omjClosed = dataTable.Rows[0].Field<bool>("omjClosed");
			eRPSalesOrderJobLinkInformationDto.omjJobID = dataTable.Rows[0].Field<string>("omjJobID");
			eRPSalesOrderJobLinkInformationDto.omjLinkType = dataTable.Rows[0].Field<byte>("omjLinkType");
			eRPSalesOrderJobLinkInformationDto.omjRowVersion = dataTable.Rows[0].Field<byte[]>("omjRowVersion");
			eRPSalesOrderJobLinkInformationDto.omjSalesOrderDeliveryID = dataTable.Rows[0].Field<short>("omjSalesOrderDeliveryID");
			eRPSalesOrderJobLinkInformationDto.omjSalesOrderID = dataTable.Rows[0].Field<string>("omjSalesOrderID");
			eRPSalesOrderJobLinkInformationDto.omjSalesOrderLineID = dataTable.Rows[0].Field<short>("omjSalesOrderLineID");
			eRPSalesOrderJobLinkInformationDto.omjSalesOrderJobLinkID = dataTable.Rows[0].Field<int>("omjSalesOrderJobLinkID");
			eRPSalesOrderJobLinkInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPSalesOrderJobLinkInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPSalesOrderJobLinkInformationDto);
	}

	public Task<APIValidationInfoDto> SaveSalesOrderJobLink(ERPSalesOrderJobLinkDto salesOrderJobLink)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM SalesOrderJobLinks WHERE omjUniqueID = " + M1Util.ConvertToLinq(salesOrderJobLink.omjUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["omjSalesOrderID"] = salesOrderJobLink.omjSalesOrderID.ToUpper();
				dataRow["omjSalesOrderLineID"] = salesOrderJobLink.omjSalesOrderLineID;
				dataRow["omjSalesOrderJobLinkID"] = salesOrderJobLink.omjSalesOrderJobLinkID;
				salesOrderJobLink.omjUniqueID = ((salesOrderJobLink.omjUniqueID == Guid.Empty) ? Guid.NewGuid() : salesOrderJobLink.omjUniqueID);
				dataRow["omjUniqueID"] = salesOrderJobLink.omjUniqueID;
				dataRow["omjCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["omjCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The SalesOrderJobLink could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (salesOrderJobLink.omjRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the SalesOrderJobLink is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["omjRowVersion"], salesOrderJobLink.omjRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the SalesOrderJobLink has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the SalesOrderJobLink again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["omjClosed"] = salesOrderJobLink.omjClosed;
			dataRow["omjJobID"] = salesOrderJobLink.omjJobID;
			dataRow["omjLinkType"] = salesOrderJobLink.omjLinkType;
			dataRow["omjSalesOrderDeliveryID"] = salesOrderJobLink.omjSalesOrderDeliveryID;
			if (salesOrderJobLink.CustomFields != null && salesOrderJobLink.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in salesOrderJobLink.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the SalesOrderJobLink [{salesOrderJobLink.omjUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the SalesOrderJobLink [{salesOrderJobLink.omjUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
