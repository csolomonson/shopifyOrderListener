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

public class ERPLotNumberRepository : APIBaseRepository, IERPLotNumberRepository, IAPIBaseRepository, IDisposable
{
	public ERPLotNumberRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesLotNumberExist(Guid lotNumberId)
	{
		InitializeParameterLists();
		base.filterList.Add("ablUniqueID|C", lotNumberId);
		base.selectList.Add("ablUniqueID");
		return Task.FromResult(GetAsObject("LotNumbers", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPLotNumberInformationDto>> GetAllLotNumbers(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPLotNumberInformationDto> collection = new List<ERPLotNumberInformationDto>();
		InitializeParameterLists();
		string[] array = new string[12]
		{
			"ablAddedByUserID", "ablAddedDate", "ablLotNumberID", "ablCreatedBy", "ablCreatedDate", "ablUniqueID", "ablExpirationDate", "ablInactiveDate", "ablInactive", "ablPartID",
			"ablPartRevisionID", "ablRowVersion"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("LotNumbers");
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
		using (DataTable dataTable = GetAsDataTable("LotNumbers", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPLotNumberInformationDto eRPLotNumberInformationDto = new ERPLotNumberInformationDto();
				eRPLotNumberInformationDto.ablAddedByUserID = dataTable.Rows[i].Field<string>("ablAddedByUserID");
				eRPLotNumberInformationDto.ablAddedDate = dataTable.Rows[i].Field<DateTime?>("ablAddedDate");
				eRPLotNumberInformationDto.ablLotNumberID = dataTable.Rows[i].Field<string>("ablLotNumberID");
				eRPLotNumberInformationDto.ablCreatedBy = dataTable.Rows[i].Field<string>("ablCreatedBy");
				eRPLotNumberInformationDto.ablCreatedDate = dataTable.Rows[i].Field<DateTime?>("ablCreatedDate");
				eRPLotNumberInformationDto.ablUniqueID = dataTable.Rows[i].Field<Guid>("ablUniqueID");
				eRPLotNumberInformationDto.ablExpirationDate = dataTable.Rows[i].Field<DateTime?>("ablExpirationDate");
				eRPLotNumberInformationDto.ablInactiveDate = dataTable.Rows[i].Field<DateTime?>("ablInactiveDate");
				eRPLotNumberInformationDto.ablInactive = dataTable.Rows[i].Field<bool>("ablInactive");
				eRPLotNumberInformationDto.ablPartID = dataTable.Rows[i].Field<string>("ablPartID");
				eRPLotNumberInformationDto.ablPartRevisionID = dataTable.Rows[i].Field<string>("ablPartRevisionID");
				eRPLotNumberInformationDto.ablRowVersion = dataTable.Rows[i].Field<byte[]>("ablRowVersion");
				eRPLotNumberInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPLotNumberInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPLotNumberInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPLotNumberInformationDto> GetLotNumber(Guid lotNumberId)
	{
		ERPLotNumberInformationDto eRPLotNumberInformationDto = new ERPLotNumberInformationDto();
		InitializeParameterLists();
		string[] collection = new string[12]
		{
			"ablAddedByUserID", "ablAddedDate", "ablLotNumberID", "ablCreatedBy", "ablCreatedDate", "ablUniqueID", "ablExpirationDate", "ablInactiveDate", "ablInactive", "ablPartID",
			"ablPartRevisionID", "ablRowVersion"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("ablUniqueID|C", lotNumberId);
		AddCustomFieldsToSelectList("LotNumbers");
		using (DataTable dataTable = GetAsDataTable("LotNumbers", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPLotNumberInformationDto);
			}
			eRPLotNumberInformationDto.ablAddedByUserID = dataTable.Rows[0].Field<string>("ablAddedByUserID");
			eRPLotNumberInformationDto.ablAddedDate = dataTable.Rows[0].Field<DateTime?>("ablAddedDate");
			eRPLotNumberInformationDto.ablLotNumberID = dataTable.Rows[0].Field<string>("ablLotNumberID");
			eRPLotNumberInformationDto.ablCreatedBy = dataTable.Rows[0].Field<string>("ablCreatedBy");
			eRPLotNumberInformationDto.ablCreatedDate = dataTable.Rows[0].Field<DateTime?>("ablCreatedDate");
			eRPLotNumberInformationDto.ablUniqueID = dataTable.Rows[0].Field<Guid>("ablUniqueID");
			eRPLotNumberInformationDto.ablExpirationDate = dataTable.Rows[0].Field<DateTime?>("ablExpirationDate");
			eRPLotNumberInformationDto.ablInactiveDate = dataTable.Rows[0].Field<DateTime?>("ablInactiveDate");
			eRPLotNumberInformationDto.ablInactive = dataTable.Rows[0].Field<bool>("ablInactive");
			eRPLotNumberInformationDto.ablPartID = dataTable.Rows[0].Field<string>("ablPartID");
			eRPLotNumberInformationDto.ablPartRevisionID = dataTable.Rows[0].Field<string>("ablPartRevisionID");
			eRPLotNumberInformationDto.ablRowVersion = dataTable.Rows[0].Field<byte[]>("ablRowVersion");
			eRPLotNumberInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPLotNumberInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPLotNumberInformationDto);
	}

	public Task<APIValidationInfoDto> SaveLotNumber(ERPLotNumberDto lotNumber)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM LotNumbers WHERE ablUniqueID = " + M1Util.ConvertToLinq(lotNumber.ablUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["ablPartID"] = lotNumber.ablPartID.ToUpper();
				dataRow["ablPartRevisionID"] = lotNumber.ablPartRevisionID.ToUpper();
				dataRow["ablLotNumberID"] = lotNumber.ablLotNumberID.ToUpper();
				lotNumber.ablUniqueID = ((lotNumber.ablUniqueID == Guid.Empty) ? Guid.NewGuid() : lotNumber.ablUniqueID);
				dataRow["ablUniqueID"] = lotNumber.ablUniqueID;
				dataRow["ablCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["ablCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The LotNumber could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (lotNumber.ablRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the LotNumber is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["ablRowVersion"], lotNumber.ablRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the LotNumber has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the LotNumber again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["ablAddedByUserID"] = lotNumber.ablAddedByUserID;
			DataRow dataRow2 = dataRow;
			DateTime? ablAddedDate = lotNumber.ablAddedDate;
			dataRow2["ablAddedDate"] = (ablAddedDate.HasValue ? ((object)ablAddedDate.GetValueOrDefault()) : dataRow["ablAddedDate"]);
			DataRow dataRow3 = dataRow;
			ablAddedDate = lotNumber.ablExpirationDate;
			dataRow3["ablExpirationDate"] = (ablAddedDate.HasValue ? ((object)ablAddedDate.GetValueOrDefault()) : dataRow["ablExpirationDate"]);
			DataRow dataRow4 = dataRow;
			ablAddedDate = lotNumber.ablInactiveDate;
			dataRow4["ablInactiveDate"] = (ablAddedDate.HasValue ? ((object)ablAddedDate.GetValueOrDefault()) : dataRow["ablInactiveDate"]);
			dataRow["ablInactive"] = lotNumber.ablInactive;
			if (lotNumber.CustomFields != null && lotNumber.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in lotNumber.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the LotNumber [{lotNumber.ablUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the LotNumber [{lotNumber.ablUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
