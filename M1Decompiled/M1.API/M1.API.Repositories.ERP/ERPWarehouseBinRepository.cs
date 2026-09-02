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

public class ERPWarehouseBinRepository : APIBaseRepository, IERPWarehouseBinRepository, IAPIBaseRepository, IDisposable
{
	public ERPWarehouseBinRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesWarehouseBinExist(Guid warehouseBinId)
	{
		InitializeParameterLists();
		base.filterList.Add("inbUniqueID|C", warehouseBinId);
		base.selectList.Add("inbUniqueID");
		return Task.FromResult(GetAsObject("WarehouseBins", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPWarehouseBinInformationDto>> GetAllWarehouseBins(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPWarehouseBinInformationDto> collection = new List<ERPWarehouseBinInformationDto>();
		InitializeParameterLists();
		string[] array = new string[13]
		{
			"inbWarehouseBinID", "inbCreatedBy", "inbCreatedDate", "inbDescription", "inbUniqueID", "inbInactiveDate", "inbInactive", "inbDefaultBin", "inbHasQOHQTI", "inbLongDescriptionRtf",
			"inbLongDescriptionText", "inbRowVersion", "inbWarehouseID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("WarehouseBins");
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
		using (DataTable dataTable = GetAsDataTable("WarehouseBins", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPWarehouseBinInformationDto eRPWarehouseBinInformationDto = new ERPWarehouseBinInformationDto();
				eRPWarehouseBinInformationDto.inbWarehouseBinID = dataTable.Rows[i].Field<string>("inbWarehouseBinID");
				eRPWarehouseBinInformationDto.inbCreatedBy = dataTable.Rows[i].Field<string>("inbCreatedBy");
				eRPWarehouseBinInformationDto.inbCreatedDate = dataTable.Rows[i].Field<DateTime?>("inbCreatedDate");
				eRPWarehouseBinInformationDto.inbDescription = dataTable.Rows[i].Field<string>("inbDescription");
				eRPWarehouseBinInformationDto.inbUniqueID = dataTable.Rows[i].Field<Guid>("inbUniqueID");
				eRPWarehouseBinInformationDto.inbInactiveDate = dataTable.Rows[i].Field<DateTime?>("inbInactiveDate");
				eRPWarehouseBinInformationDto.inbInactive = dataTable.Rows[i].Field<bool>("inbInactive");
				eRPWarehouseBinInformationDto.inbDefaultBin = dataTable.Rows[i].Field<bool>("inbDefaultBin");
				eRPWarehouseBinInformationDto.inbHasQOHQTI = dataTable.Rows[i].Field<bool>("inbHasQOHQTI");
				eRPWarehouseBinInformationDto.inbLongDescriptionRtf = dataTable.Rows[i].Field<string>("inbLongDescriptionRtf");
				eRPWarehouseBinInformationDto.inbLongDescriptionText = dataTable.Rows[i].Field<string>("inbLongDescriptionText");
				eRPWarehouseBinInformationDto.inbRowVersion = dataTable.Rows[i].Field<byte[]>("inbRowVersion");
				eRPWarehouseBinInformationDto.inbWarehouseID = dataTable.Rows[i].Field<string>("inbWarehouseID");
				eRPWarehouseBinInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPWarehouseBinInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPWarehouseBinInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPWarehouseBinInformationDto> GetWarehouseBin(Guid warehouseBinId)
	{
		ERPWarehouseBinInformationDto eRPWarehouseBinInformationDto = new ERPWarehouseBinInformationDto();
		InitializeParameterLists();
		string[] collection = new string[13]
		{
			"inbWarehouseBinID", "inbCreatedBy", "inbCreatedDate", "inbDescription", "inbUniqueID", "inbInactiveDate", "inbInactive", "inbDefaultBin", "inbHasQOHQTI", "inbLongDescriptionRtf",
			"inbLongDescriptionText", "inbRowVersion", "inbWarehouseID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("inbUniqueID|C", warehouseBinId);
		AddCustomFieldsToSelectList("WarehouseBins");
		using (DataTable dataTable = GetAsDataTable("WarehouseBins", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPWarehouseBinInformationDto);
			}
			eRPWarehouseBinInformationDto.inbWarehouseBinID = dataTable.Rows[0].Field<string>("inbWarehouseBinID");
			eRPWarehouseBinInformationDto.inbCreatedBy = dataTable.Rows[0].Field<string>("inbCreatedBy");
			eRPWarehouseBinInformationDto.inbCreatedDate = dataTable.Rows[0].Field<DateTime?>("inbCreatedDate");
			eRPWarehouseBinInformationDto.inbDescription = dataTable.Rows[0].Field<string>("inbDescription");
			eRPWarehouseBinInformationDto.inbUniqueID = dataTable.Rows[0].Field<Guid>("inbUniqueID");
			eRPWarehouseBinInformationDto.inbInactiveDate = dataTable.Rows[0].Field<DateTime?>("inbInactiveDate");
			eRPWarehouseBinInformationDto.inbInactive = dataTable.Rows[0].Field<bool>("inbInactive");
			eRPWarehouseBinInformationDto.inbDefaultBin = dataTable.Rows[0].Field<bool>("inbDefaultBin");
			eRPWarehouseBinInformationDto.inbHasQOHQTI = dataTable.Rows[0].Field<bool>("inbHasQOHQTI");
			eRPWarehouseBinInformationDto.inbLongDescriptionRtf = dataTable.Rows[0].Field<string>("inbLongDescriptionRtf");
			eRPWarehouseBinInformationDto.inbLongDescriptionText = dataTable.Rows[0].Field<string>("inbLongDescriptionText");
			eRPWarehouseBinInformationDto.inbRowVersion = dataTable.Rows[0].Field<byte[]>("inbRowVersion");
			eRPWarehouseBinInformationDto.inbWarehouseID = dataTable.Rows[0].Field<string>("inbWarehouseID");
			eRPWarehouseBinInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPWarehouseBinInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPWarehouseBinInformationDto);
	}

	public Task<APIValidationInfoDto> SaveWarehouseBin(ERPWarehouseBinDto warehouseBin)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM WarehouseBins WHERE inbUniqueID = " + M1Util.ConvertToLinq(warehouseBin.inbUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["inbWarehouseID"] = warehouseBin.inbWarehouseID.ToUpper();
				dataRow["inbWarehouseBinID"] = warehouseBin.inbWarehouseBinID.ToUpper();
				warehouseBin.inbUniqueID = ((warehouseBin.inbUniqueID == Guid.Empty) ? Guid.NewGuid() : warehouseBin.inbUniqueID);
				dataRow["inbUniqueID"] = warehouseBin.inbUniqueID;
				dataRow["inbCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["inbCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The WarehouseBin could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (warehouseBin.inbRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the WarehouseBin is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["inbRowVersion"], warehouseBin.inbRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the WarehouseBin has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the WarehouseBin again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["inbDescription"] = warehouseBin.inbDescription;
			DataRow dataRow2 = dataRow;
			DateTime? inbInactiveDate = warehouseBin.inbInactiveDate;
			dataRow2["inbInactiveDate"] = (inbInactiveDate.HasValue ? ((object)inbInactiveDate.GetValueOrDefault()) : dataRow["inbInactiveDate"]);
			dataRow["inbInactive"] = warehouseBin.inbInactive;
			dataRow["inbDefaultBin"] = warehouseBin.inbDefaultBin;
			dataRow["inbHasQOHQTI"] = warehouseBin.inbHasQOHQTI;
			dataRow["inbLongDescriptionRtf"] = warehouseBin.inbLongDescriptionRtf ?? dataRow["inbLongDescriptionRtf"];
			dataRow["inbLongDescriptionText"] = warehouseBin.inbLongDescriptionText ?? dataRow["inbLongDescriptionText"];
			if (warehouseBin.CustomFields != null && warehouseBin.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in warehouseBin.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the WarehouseBin [{warehouseBin.inbUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the WarehouseBin [{warehouseBin.inbUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
