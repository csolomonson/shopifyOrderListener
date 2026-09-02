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

public class ERPSerialNumberStatusRepository : APIBaseRepository, IERPSerialNumberStatusRepository, IAPIBaseRepository, IDisposable
{
	public ERPSerialNumberStatusRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesSerialNumberStatusExist(Guid serialNumberStatusId)
	{
		InitializeParameterLists();
		base.filterList.Add("snsUniqueID|C", serialNumberStatusId);
		base.selectList.Add("snsUniqueID");
		return Task.FromResult(GetAsObject("SerialNumberStatuses", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPSerialNumberStatusInformationDto>> GetAllSerialNumberStatuses(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPSerialNumberStatusInformationDto> collection = new List<ERPSerialNumberStatusInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"snsCreatedBy", "snsCreatedDate", "snsUniqueID", "snsPartBinID", "snsPartID", "snsPartRevisionID", "snsPartWarehouseLocationID", "snsQuantity", "snsRowVersion", "snsSerialNumberID",
			"snsStatus"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("SerialNumberStatuses");
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
		using (DataTable dataTable = GetAsDataTable("SerialNumberStatuses", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPSerialNumberStatusInformationDto eRPSerialNumberStatusInformationDto = new ERPSerialNumberStatusInformationDto();
				eRPSerialNumberStatusInformationDto.snsCreatedBy = dataTable.Rows[i].Field<string>("snsCreatedBy");
				eRPSerialNumberStatusInformationDto.snsCreatedDate = dataTable.Rows[i].Field<DateTime?>("snsCreatedDate");
				eRPSerialNumberStatusInformationDto.snsUniqueID = dataTable.Rows[i].Field<Guid>("snsUniqueID");
				eRPSerialNumberStatusInformationDto.snsPartBinID = dataTable.Rows[i].Field<string>("snsPartBinID");
				eRPSerialNumberStatusInformationDto.snsPartID = dataTable.Rows[i].Field<string>("snsPartID");
				eRPSerialNumberStatusInformationDto.snsPartRevisionID = dataTable.Rows[i].Field<string>("snsPartRevisionID");
				eRPSerialNumberStatusInformationDto.snsPartWarehouseLocationID = dataTable.Rows[i].Field<string>("snsPartWarehouseLocationID");
				eRPSerialNumberStatusInformationDto.snsQuantity = dataTable.Rows[i].Field<decimal>("snsQuantity");
				eRPSerialNumberStatusInformationDto.snsRowVersion = dataTable.Rows[i].Field<byte[]>("snsRowVersion");
				eRPSerialNumberStatusInformationDto.snsSerialNumberID = dataTable.Rows[i].Field<string>("snsSerialNumberID");
				eRPSerialNumberStatusInformationDto.snsStatus = dataTable.Rows[i].Field<byte>("snsStatus");
				eRPSerialNumberStatusInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPSerialNumberStatusInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPSerialNumberStatusInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPSerialNumberStatusInformationDto> GetSerialNumberStatus(Guid serialNumberStatusId)
	{
		ERPSerialNumberStatusInformationDto eRPSerialNumberStatusInformationDto = new ERPSerialNumberStatusInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"snsCreatedBy", "snsCreatedDate", "snsUniqueID", "snsPartBinID", "snsPartID", "snsPartRevisionID", "snsPartWarehouseLocationID", "snsQuantity", "snsRowVersion", "snsSerialNumberID",
			"snsStatus"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("snsUniqueID|C", serialNumberStatusId);
		AddCustomFieldsToSelectList("SerialNumberStatuses");
		using (DataTable dataTable = GetAsDataTable("SerialNumberStatuses", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPSerialNumberStatusInformationDto);
			}
			eRPSerialNumberStatusInformationDto.snsCreatedBy = dataTable.Rows[0].Field<string>("snsCreatedBy");
			eRPSerialNumberStatusInformationDto.snsCreatedDate = dataTable.Rows[0].Field<DateTime?>("snsCreatedDate");
			eRPSerialNumberStatusInformationDto.snsUniqueID = dataTable.Rows[0].Field<Guid>("snsUniqueID");
			eRPSerialNumberStatusInformationDto.snsPartBinID = dataTable.Rows[0].Field<string>("snsPartBinID");
			eRPSerialNumberStatusInformationDto.snsPartID = dataTable.Rows[0].Field<string>("snsPartID");
			eRPSerialNumberStatusInformationDto.snsPartRevisionID = dataTable.Rows[0].Field<string>("snsPartRevisionID");
			eRPSerialNumberStatusInformationDto.snsPartWarehouseLocationID = dataTable.Rows[0].Field<string>("snsPartWarehouseLocationID");
			eRPSerialNumberStatusInformationDto.snsQuantity = dataTable.Rows[0].Field<decimal>("snsQuantity");
			eRPSerialNumberStatusInformationDto.snsRowVersion = dataTable.Rows[0].Field<byte[]>("snsRowVersion");
			eRPSerialNumberStatusInformationDto.snsSerialNumberID = dataTable.Rows[0].Field<string>("snsSerialNumberID");
			eRPSerialNumberStatusInformationDto.snsStatus = dataTable.Rows[0].Field<byte>("snsStatus");
			eRPSerialNumberStatusInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPSerialNumberStatusInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPSerialNumberStatusInformationDto);
	}

	public Task<APIValidationInfoDto> SaveSerialNumberStatus(ERPSerialNumberStatusDto serialNumberStatus)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM SerialNumberStatuses WHERE snsUniqueID = " + M1Util.ConvertToLinq(serialNumberStatus.snsUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["snsPartID"] = serialNumberStatus.snsPartID.ToUpper();
				dataRow["snsPartRevisionID"] = serialNumberStatus.snsPartRevisionID.ToUpper();
				dataRow["snsSerialNumberID"] = serialNumberStatus.snsSerialNumberID.ToUpper();
				dataRow["snsPartWarehouseLocationID"] = serialNumberStatus.snsPartWarehouseLocationID.ToUpper();
				dataRow["snsPartBinID"] = serialNumberStatus.snsPartBinID.ToUpper();
				dataRow["snsStatus"] = serialNumberStatus.snsStatus;
				serialNumberStatus.snsUniqueID = ((serialNumberStatus.snsUniqueID == Guid.Empty) ? Guid.NewGuid() : serialNumberStatus.snsUniqueID);
				dataRow["snsUniqueID"] = serialNumberStatus.snsUniqueID;
				dataRow["snsCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["snsCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The SerialNumberStatus could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (serialNumberStatus.snsRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the SerialNumberStatus is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["snsRowVersion"], serialNumberStatus.snsRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the SerialNumberStatus has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the SerialNumberStatus again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["snsQuantity"] = serialNumberStatus.snsQuantity;
			if (serialNumberStatus.CustomFields != null && serialNumberStatus.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in serialNumberStatus.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the SerialNumberStatus [{serialNumberStatus.snsUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the SerialNumberStatus [{serialNumberStatus.snsUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
