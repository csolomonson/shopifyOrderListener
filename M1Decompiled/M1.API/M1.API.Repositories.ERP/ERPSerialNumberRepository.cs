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

public class ERPSerialNumberRepository : APIBaseRepository, IERPSerialNumberRepository, IAPIBaseRepository, IDisposable
{
	public ERPSerialNumberRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesSerialNumberExist(Guid serialNumberId)
	{
		InitializeParameterLists();
		base.filterList.Add("imsUniqueID|C", serialNumberId);
		base.selectList.Add("imsUniqueID");
		return Task.FromResult(GetAsObject("SerialNumbers", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPSerialNumberInformationDto>> GetAllSerialNumbers(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPSerialNumberInformationDto> collection = new List<ERPSerialNumberInformationDto>();
		InitializeParameterLists();
		string[] array = new string[12]
		{
			"imsAddedByUserID", "imsAddedDate", "imsSerialNumberID", "imsCreatedBy", "imsCreatedDate", "imsUniqueID", "imsExpirationDate", "imsInactiveDate", "imsInactive", "imsPartID",
			"imsPartRevisionID", "imsRowVersion"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("SerialNumbers");
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
		using (DataTable dataTable = GetAsDataTable("SerialNumbers", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPSerialNumberInformationDto eRPSerialNumberInformationDto = new ERPSerialNumberInformationDto();
				eRPSerialNumberInformationDto.imsAddedByUserID = dataTable.Rows[i].Field<string>("imsAddedByUserID");
				eRPSerialNumberInformationDto.imsAddedDate = dataTable.Rows[i].Field<DateTime?>("imsAddedDate");
				eRPSerialNumberInformationDto.imsSerialNumberID = dataTable.Rows[i].Field<string>("imsSerialNumberID");
				eRPSerialNumberInformationDto.imsCreatedBy = dataTable.Rows[i].Field<string>("imsCreatedBy");
				eRPSerialNumberInformationDto.imsCreatedDate = dataTable.Rows[i].Field<DateTime?>("imsCreatedDate");
				eRPSerialNumberInformationDto.imsUniqueID = dataTable.Rows[i].Field<Guid>("imsUniqueID");
				eRPSerialNumberInformationDto.imsExpirationDate = dataTable.Rows[i].Field<DateTime?>("imsExpirationDate");
				eRPSerialNumberInformationDto.imsInactiveDate = dataTable.Rows[i].Field<DateTime?>("imsInactiveDate");
				eRPSerialNumberInformationDto.imsInactive = dataTable.Rows[i].Field<bool>("imsInactive");
				eRPSerialNumberInformationDto.imsPartID = dataTable.Rows[i].Field<string>("imsPartID");
				eRPSerialNumberInformationDto.imsPartRevisionID = dataTable.Rows[i].Field<string>("imsPartRevisionID");
				eRPSerialNumberInformationDto.imsRowVersion = dataTable.Rows[i].Field<byte[]>("imsRowVersion");
				eRPSerialNumberInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPSerialNumberInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPSerialNumberInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPSerialNumberInformationDto> GetSerialNumber(Guid serialNumberId)
	{
		ERPSerialNumberInformationDto eRPSerialNumberInformationDto = new ERPSerialNumberInformationDto();
		InitializeParameterLists();
		string[] collection = new string[12]
		{
			"imsAddedByUserID", "imsAddedDate", "imsSerialNumberID", "imsCreatedBy", "imsCreatedDate", "imsUniqueID", "imsExpirationDate", "imsInactiveDate", "imsInactive", "imsPartID",
			"imsPartRevisionID", "imsRowVersion"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imsUniqueID|C", serialNumberId);
		AddCustomFieldsToSelectList("SerialNumbers");
		using (DataTable dataTable = GetAsDataTable("SerialNumbers", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPSerialNumberInformationDto);
			}
			eRPSerialNumberInformationDto.imsAddedByUserID = dataTable.Rows[0].Field<string>("imsAddedByUserID");
			eRPSerialNumberInformationDto.imsAddedDate = dataTable.Rows[0].Field<DateTime?>("imsAddedDate");
			eRPSerialNumberInformationDto.imsSerialNumberID = dataTable.Rows[0].Field<string>("imsSerialNumberID");
			eRPSerialNumberInformationDto.imsCreatedBy = dataTable.Rows[0].Field<string>("imsCreatedBy");
			eRPSerialNumberInformationDto.imsCreatedDate = dataTable.Rows[0].Field<DateTime?>("imsCreatedDate");
			eRPSerialNumberInformationDto.imsUniqueID = dataTable.Rows[0].Field<Guid>("imsUniqueID");
			eRPSerialNumberInformationDto.imsExpirationDate = dataTable.Rows[0].Field<DateTime?>("imsExpirationDate");
			eRPSerialNumberInformationDto.imsInactiveDate = dataTable.Rows[0].Field<DateTime?>("imsInactiveDate");
			eRPSerialNumberInformationDto.imsInactive = dataTable.Rows[0].Field<bool>("imsInactive");
			eRPSerialNumberInformationDto.imsPartID = dataTable.Rows[0].Field<string>("imsPartID");
			eRPSerialNumberInformationDto.imsPartRevisionID = dataTable.Rows[0].Field<string>("imsPartRevisionID");
			eRPSerialNumberInformationDto.imsRowVersion = dataTable.Rows[0].Field<byte[]>("imsRowVersion");
			eRPSerialNumberInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPSerialNumberInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPSerialNumberInformationDto);
	}

	public Task<APIValidationInfoDto> SaveSerialNumber(ERPSerialNumberDto serialNumber)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM SerialNumbers WHERE imsUniqueID = " + M1Util.ConvertToLinq(serialNumber.imsUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imsPartID"] = serialNumber.imsPartID.ToUpper();
				dataRow["imsPartRevisionID"] = serialNumber.imsPartRevisionID.ToUpper();
				dataRow["imsSerialNumberID"] = serialNumber.imsSerialNumberID.ToUpper();
				serialNumber.imsUniqueID = ((serialNumber.imsUniqueID == Guid.Empty) ? Guid.NewGuid() : serialNumber.imsUniqueID);
				dataRow["imsUniqueID"] = serialNumber.imsUniqueID;
				dataRow["imsCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imsCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The SerialNumber could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (serialNumber.imsRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the SerialNumber is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imsRowVersion"], serialNumber.imsRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the SerialNumber has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the SerialNumber again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imsAddedByUserID"] = serialNumber.imsAddedByUserID;
			DataRow dataRow2 = dataRow;
			DateTime? imsAddedDate = serialNumber.imsAddedDate;
			dataRow2["imsAddedDate"] = (imsAddedDate.HasValue ? ((object)imsAddedDate.GetValueOrDefault()) : dataRow["imsAddedDate"]);
			DataRow dataRow3 = dataRow;
			imsAddedDate = serialNumber.imsExpirationDate;
			dataRow3["imsExpirationDate"] = (imsAddedDate.HasValue ? ((object)imsAddedDate.GetValueOrDefault()) : dataRow["imsExpirationDate"]);
			DataRow dataRow4 = dataRow;
			imsAddedDate = serialNumber.imsInactiveDate;
			dataRow4["imsInactiveDate"] = (imsAddedDate.HasValue ? ((object)imsAddedDate.GetValueOrDefault()) : dataRow["imsInactiveDate"]);
			dataRow["imsInactive"] = serialNumber.imsInactive;
			if (serialNumber.CustomFields != null && serialNumber.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in serialNumber.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the SerialNumber [{serialNumber.imsUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the SerialNumber [{serialNumber.imsUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
