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

public class ERPLotNumberStatusRepository : APIBaseRepository, IERPLotNumberStatusRepository, IAPIBaseRepository, IDisposable
{
	public ERPLotNumberStatusRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesLotNumberStatusExist(Guid lotNumberStatusId)
	{
		InitializeParameterLists();
		base.filterList.Add("absUniqueID|C", lotNumberStatusId);
		base.selectList.Add("absUniqueID");
		return Task.FromResult(GetAsObject("LotNumberStatuses", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPLotNumberStatusInformationDto>> GetAllLotNumberStatuses(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPLotNumberStatusInformationDto> collection = new List<ERPLotNumberStatusInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"absCreatedBy", "absCreatedDate", "absUniqueID", "absLotNumberID", "absPartBinID", "absPartID", "absPartRevisionID", "absPartWarehouseLocationID", "absQuantity", "absRowVersion",
			"absStatus"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("LotNumberStatuses");
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
		using (DataTable dataTable = GetAsDataTable("LotNumberStatuses", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPLotNumberStatusInformationDto eRPLotNumberStatusInformationDto = new ERPLotNumberStatusInformationDto();
				eRPLotNumberStatusInformationDto.absCreatedBy = dataTable.Rows[i].Field<string>("absCreatedBy");
				eRPLotNumberStatusInformationDto.absCreatedDate = dataTable.Rows[i].Field<DateTime?>("absCreatedDate");
				eRPLotNumberStatusInformationDto.absUniqueID = dataTable.Rows[i].Field<Guid>("absUniqueID");
				eRPLotNumberStatusInformationDto.absLotNumberID = dataTable.Rows[i].Field<string>("absLotNumberID");
				eRPLotNumberStatusInformationDto.absPartBinID = dataTable.Rows[i].Field<string>("absPartBinID");
				eRPLotNumberStatusInformationDto.absPartID = dataTable.Rows[i].Field<string>("absPartID");
				eRPLotNumberStatusInformationDto.absPartRevisionID = dataTable.Rows[i].Field<string>("absPartRevisionID");
				eRPLotNumberStatusInformationDto.absPartWarehouseLocationID = dataTable.Rows[i].Field<string>("absPartWarehouseLocationID");
				eRPLotNumberStatusInformationDto.absQuantity = dataTable.Rows[i].Field<decimal>("absQuantity");
				eRPLotNumberStatusInformationDto.absRowVersion = dataTable.Rows[i].Field<byte[]>("absRowVersion");
				eRPLotNumberStatusInformationDto.absStatus = dataTable.Rows[i].Field<byte>("absStatus");
				eRPLotNumberStatusInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPLotNumberStatusInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPLotNumberStatusInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPLotNumberStatusInformationDto> GetLotNumberStatus(Guid lotNumberStatusId)
	{
		ERPLotNumberStatusInformationDto eRPLotNumberStatusInformationDto = new ERPLotNumberStatusInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"absCreatedBy", "absCreatedDate", "absUniqueID", "absLotNumberID", "absPartBinID", "absPartID", "absPartRevisionID", "absPartWarehouseLocationID", "absQuantity", "absRowVersion",
			"absStatus"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("absUniqueID|C", lotNumberStatusId);
		AddCustomFieldsToSelectList("LotNumberStatuses");
		using (DataTable dataTable = GetAsDataTable("LotNumberStatuses", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPLotNumberStatusInformationDto);
			}
			eRPLotNumberStatusInformationDto.absCreatedBy = dataTable.Rows[0].Field<string>("absCreatedBy");
			eRPLotNumberStatusInformationDto.absCreatedDate = dataTable.Rows[0].Field<DateTime?>("absCreatedDate");
			eRPLotNumberStatusInformationDto.absUniqueID = dataTable.Rows[0].Field<Guid>("absUniqueID");
			eRPLotNumberStatusInformationDto.absLotNumberID = dataTable.Rows[0].Field<string>("absLotNumberID");
			eRPLotNumberStatusInformationDto.absPartBinID = dataTable.Rows[0].Field<string>("absPartBinID");
			eRPLotNumberStatusInformationDto.absPartID = dataTable.Rows[0].Field<string>("absPartID");
			eRPLotNumberStatusInformationDto.absPartRevisionID = dataTable.Rows[0].Field<string>("absPartRevisionID");
			eRPLotNumberStatusInformationDto.absPartWarehouseLocationID = dataTable.Rows[0].Field<string>("absPartWarehouseLocationID");
			eRPLotNumberStatusInformationDto.absQuantity = dataTable.Rows[0].Field<decimal>("absQuantity");
			eRPLotNumberStatusInformationDto.absRowVersion = dataTable.Rows[0].Field<byte[]>("absRowVersion");
			eRPLotNumberStatusInformationDto.absStatus = dataTable.Rows[0].Field<byte>("absStatus");
			eRPLotNumberStatusInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPLotNumberStatusInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPLotNumberStatusInformationDto);
	}

	public Task<APIValidationInfoDto> SaveLotNumberStatus(ERPLotNumberStatusDto lotNumberStatus)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM LotNumberStatuses WHERE absUniqueID = " + M1Util.ConvertToLinq(lotNumberStatus.absUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["absPartID"] = lotNumberStatus.absPartID.ToUpper();
				dataRow["absPartRevisionID"] = lotNumberStatus.absPartRevisionID.ToUpper();
				dataRow["absLotNumberID"] = lotNumberStatus.absLotNumberID.ToUpper();
				dataRow["absPartWarehouseLocationID"] = lotNumberStatus.absPartWarehouseLocationID.ToUpper();
				dataRow["absPartBinID"] = lotNumberStatus.absPartBinID.ToUpper();
				dataRow["absStatus"] = lotNumberStatus.absStatus;
				lotNumberStatus.absUniqueID = ((lotNumberStatus.absUniqueID == Guid.Empty) ? Guid.NewGuid() : lotNumberStatus.absUniqueID);
				dataRow["absUniqueID"] = lotNumberStatus.absUniqueID;
				dataRow["absCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["absCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The LotNumberStatus could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (lotNumberStatus.absRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the LotNumberStatus is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["absRowVersion"], lotNumberStatus.absRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the LotNumberStatus has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the LotNumberStatus again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["absQuantity"] = lotNumberStatus.absQuantity;
			if (lotNumberStatus.CustomFields != null && lotNumberStatus.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in lotNumberStatus.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the LotNumberStatus [{lotNumberStatus.absUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the LotNumberStatus [{lotNumberStatus.absUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
