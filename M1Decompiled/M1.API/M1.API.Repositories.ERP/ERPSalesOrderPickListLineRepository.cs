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

public class ERPSalesOrderPickListLineRepository : APIBaseRepository, IERPSalesOrderPickListLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPSalesOrderPickListLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesSalesOrderPickListLineExist(Guid salesOrderPickListLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("omyUniqueID|C", salesOrderPickListLineId);
		base.selectList.Add("omyUniqueID");
		return Task.FromResult(GetAsObject("SalesOrderPickListLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPSalesOrderPickListLineInformationDto>> GetAllSalesOrderPickListLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPSalesOrderPickListLineInformationDto> collection = new List<ERPSalesOrderPickListLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[18]
		{
			"omyCreatedBy", "omyCreatedDate", "omyDeliveryDate", "omyUniqueID", "omyOpenQuantity", "omyPartBinID", "omyPartID", "omyPartRevisionID", "omyPartWareHouseLocationID", "omyPickDate",
			"omyPickListLineID", "omyPickListSessionID", "omyPickQuantity", "omyRowVersion", "omySalesOrderDeliveryID", "omySalesOrderID", "omySalesOrderLineID", "omyStatus"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("SalesOrderPickListLines");
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
		using (DataTable dataTable = GetAsDataTable("SalesOrderPickListLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPSalesOrderPickListLineInformationDto eRPSalesOrderPickListLineInformationDto = new ERPSalesOrderPickListLineInformationDto();
				eRPSalesOrderPickListLineInformationDto.omyCreatedBy = dataTable.Rows[i].Field<string>("omyCreatedBy");
				eRPSalesOrderPickListLineInformationDto.omyCreatedDate = dataTable.Rows[i].Field<DateTime?>("omyCreatedDate");
				eRPSalesOrderPickListLineInformationDto.omyDeliveryDate = dataTable.Rows[i].Field<DateTime?>("omyDeliveryDate");
				eRPSalesOrderPickListLineInformationDto.omyUniqueID = dataTable.Rows[i].Field<Guid>("omyUniqueID");
				eRPSalesOrderPickListLineInformationDto.omyOpenQuantity = dataTable.Rows[i].Field<decimal>("omyOpenQuantity");
				eRPSalesOrderPickListLineInformationDto.omyPartBinID = dataTable.Rows[i].Field<string>("omyPartBinID");
				eRPSalesOrderPickListLineInformationDto.omyPartID = dataTable.Rows[i].Field<string>("omyPartID");
				eRPSalesOrderPickListLineInformationDto.omyPartRevisionID = dataTable.Rows[i].Field<string>("omyPartRevisionID");
				eRPSalesOrderPickListLineInformationDto.omyPartWareHouseLocationID = dataTable.Rows[i].Field<string>("omyPartWareHouseLocationID");
				eRPSalesOrderPickListLineInformationDto.omyPickDate = dataTable.Rows[i].Field<DateTime?>("omyPickDate");
				eRPSalesOrderPickListLineInformationDto.omyPickListLineID = dataTable.Rows[i].Field<short>("omyPickListLineID");
				eRPSalesOrderPickListLineInformationDto.omyPickListSessionID = dataTable.Rows[i].Field<int>("omyPickListSessionID");
				eRPSalesOrderPickListLineInformationDto.omyPickQuantity = dataTable.Rows[i].Field<decimal>("omyPickQuantity");
				eRPSalesOrderPickListLineInformationDto.omyRowVersion = dataTable.Rows[i].Field<byte[]>("omyRowVersion");
				eRPSalesOrderPickListLineInformationDto.omySalesOrderDeliveryID = dataTable.Rows[i].Field<short>("omySalesOrderDeliveryID");
				eRPSalesOrderPickListLineInformationDto.omySalesOrderID = dataTable.Rows[i].Field<string>("omySalesOrderID");
				eRPSalesOrderPickListLineInformationDto.omySalesOrderLineID = dataTable.Rows[i].Field<short>("omySalesOrderLineID");
				eRPSalesOrderPickListLineInformationDto.omyStatus = dataTable.Rows[i].Field<byte>("omyStatus");
				eRPSalesOrderPickListLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPSalesOrderPickListLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPSalesOrderPickListLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPSalesOrderPickListLineInformationDto> GetSalesOrderPickListLine(Guid salesOrderPickListLineId)
	{
		ERPSalesOrderPickListLineInformationDto eRPSalesOrderPickListLineInformationDto = new ERPSalesOrderPickListLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[18]
		{
			"omyCreatedBy", "omyCreatedDate", "omyDeliveryDate", "omyUniqueID", "omyOpenQuantity", "omyPartBinID", "omyPartID", "omyPartRevisionID", "omyPartWareHouseLocationID", "omyPickDate",
			"omyPickListLineID", "omyPickListSessionID", "omyPickQuantity", "omyRowVersion", "omySalesOrderDeliveryID", "omySalesOrderID", "omySalesOrderLineID", "omyStatus"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("omyUniqueID|C", salesOrderPickListLineId);
		AddCustomFieldsToSelectList("SalesOrderPickListLines");
		using (DataTable dataTable = GetAsDataTable("SalesOrderPickListLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPSalesOrderPickListLineInformationDto);
			}
			eRPSalesOrderPickListLineInformationDto.omyCreatedBy = dataTable.Rows[0].Field<string>("omyCreatedBy");
			eRPSalesOrderPickListLineInformationDto.omyCreatedDate = dataTable.Rows[0].Field<DateTime?>("omyCreatedDate");
			eRPSalesOrderPickListLineInformationDto.omyDeliveryDate = dataTable.Rows[0].Field<DateTime?>("omyDeliveryDate");
			eRPSalesOrderPickListLineInformationDto.omyUniqueID = dataTable.Rows[0].Field<Guid>("omyUniqueID");
			eRPSalesOrderPickListLineInformationDto.omyOpenQuantity = dataTable.Rows[0].Field<decimal>("omyOpenQuantity");
			eRPSalesOrderPickListLineInformationDto.omyPartBinID = dataTable.Rows[0].Field<string>("omyPartBinID");
			eRPSalesOrderPickListLineInformationDto.omyPartID = dataTable.Rows[0].Field<string>("omyPartID");
			eRPSalesOrderPickListLineInformationDto.omyPartRevisionID = dataTable.Rows[0].Field<string>("omyPartRevisionID");
			eRPSalesOrderPickListLineInformationDto.omyPartWareHouseLocationID = dataTable.Rows[0].Field<string>("omyPartWareHouseLocationID");
			eRPSalesOrderPickListLineInformationDto.omyPickDate = dataTable.Rows[0].Field<DateTime?>("omyPickDate");
			eRPSalesOrderPickListLineInformationDto.omyPickListLineID = dataTable.Rows[0].Field<short>("omyPickListLineID");
			eRPSalesOrderPickListLineInformationDto.omyPickListSessionID = dataTable.Rows[0].Field<int>("omyPickListSessionID");
			eRPSalesOrderPickListLineInformationDto.omyPickQuantity = dataTable.Rows[0].Field<decimal>("omyPickQuantity");
			eRPSalesOrderPickListLineInformationDto.omyRowVersion = dataTable.Rows[0].Field<byte[]>("omyRowVersion");
			eRPSalesOrderPickListLineInformationDto.omySalesOrderDeliveryID = dataTable.Rows[0].Field<short>("omySalesOrderDeliveryID");
			eRPSalesOrderPickListLineInformationDto.omySalesOrderID = dataTable.Rows[0].Field<string>("omySalesOrderID");
			eRPSalesOrderPickListLineInformationDto.omySalesOrderLineID = dataTable.Rows[0].Field<short>("omySalesOrderLineID");
			eRPSalesOrderPickListLineInformationDto.omyStatus = dataTable.Rows[0].Field<byte>("omyStatus");
			eRPSalesOrderPickListLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPSalesOrderPickListLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPSalesOrderPickListLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveSalesOrderPickListLine(ERPSalesOrderPickListLineDto salesOrderPickListLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM SalesOrderPickListLines WHERE omyUniqueID = " + M1Util.ConvertToLinq(salesOrderPickListLine.omyUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["omyPickListSessionID"] = salesOrderPickListLine.omyPickListSessionID;
				dataRow["omyPickListLineID"] = salesOrderPickListLine.omyPickListLineID;
				salesOrderPickListLine.omyUniqueID = ((salesOrderPickListLine.omyUniqueID == Guid.Empty) ? Guid.NewGuid() : salesOrderPickListLine.omyUniqueID);
				dataRow["omyUniqueID"] = salesOrderPickListLine.omyUniqueID;
				dataRow["omyCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["omyCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The SalesOrderPickListLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (salesOrderPickListLine.omyRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the SalesOrderPickListLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["omyRowVersion"], salesOrderPickListLine.omyRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the SalesOrderPickListLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the SalesOrderPickListLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? omyDeliveryDate = salesOrderPickListLine.omyDeliveryDate;
			dataRow2["omyDeliveryDate"] = (omyDeliveryDate.HasValue ? ((object)omyDeliveryDate.GetValueOrDefault()) : dataRow["omyDeliveryDate"]);
			dataRow["omyOpenQuantity"] = salesOrderPickListLine.omyOpenQuantity;
			dataRow["omyPartBinID"] = salesOrderPickListLine.omyPartBinID;
			dataRow["omyPartID"] = salesOrderPickListLine.omyPartID;
			dataRow["omyPartRevisionID"] = salesOrderPickListLine.omyPartRevisionID;
			dataRow["omyPartWareHouseLocationID"] = salesOrderPickListLine.omyPartWareHouseLocationID;
			DataRow dataRow3 = dataRow;
			omyDeliveryDate = salesOrderPickListLine.omyPickDate;
			dataRow3["omyPickDate"] = (omyDeliveryDate.HasValue ? ((object)omyDeliveryDate.GetValueOrDefault()) : dataRow["omyPickDate"]);
			dataRow["omyPickQuantity"] = salesOrderPickListLine.omyPickQuantity;
			dataRow["omySalesOrderDeliveryID"] = salesOrderPickListLine.omySalesOrderDeliveryID;
			dataRow["omySalesOrderID"] = salesOrderPickListLine.omySalesOrderID;
			dataRow["omySalesOrderLineID"] = salesOrderPickListLine.omySalesOrderLineID;
			dataRow["omyStatus"] = salesOrderPickListLine.omyStatus;
			if (salesOrderPickListLine.CustomFields != null && salesOrderPickListLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in salesOrderPickListLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the SalesOrderPickListLine [{salesOrderPickListLine.omyUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the SalesOrderPickListLine [{salesOrderPickListLine.omyUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
