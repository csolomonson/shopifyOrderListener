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

public class ERPFreightPackageLinkRepository : APIBaseRepository, IERPFreightPackageLinkRepository, IAPIBaseRepository, IDisposable
{
	public ERPFreightPackageLinkRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesFreightPackageLinkExist(Guid freightPackageLinkId)
	{
		InitializeParameterLists();
		base.filterList.Add("fplUniqueID|C", freightPackageLinkId);
		base.selectList.Add("fplUniqueID");
		return Task.FromResult(GetAsObject("FreightPackageLinks", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPFreightPackageLinkInformationDto>> GetAllFreightPackageLinks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPFreightPackageLinkInformationDto> collection = new List<ERPFreightPackageLinkInformationDto>();
		InitializeParameterLists();
		string[] array = new string[7] { "fplCreatedBy", "fplCreatedDate", "fplUniqueID", "fplFreightPackageID", "fplFreightPackageLineID", "fplFreightShipmentID", "fplRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("FreightPackageLinks");
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
		using (DataTable dataTable = GetAsDataTable("FreightPackageLinks", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPFreightPackageLinkInformationDto eRPFreightPackageLinkInformationDto = new ERPFreightPackageLinkInformationDto();
				eRPFreightPackageLinkInformationDto.fplCreatedBy = dataTable.Rows[i].Field<string>("fplCreatedBy");
				eRPFreightPackageLinkInformationDto.fplCreatedDate = dataTable.Rows[i].Field<DateTime?>("fplCreatedDate");
				eRPFreightPackageLinkInformationDto.fplUniqueID = dataTable.Rows[i].Field<Guid>("fplUniqueID");
				eRPFreightPackageLinkInformationDto.fplFreightPackageID = dataTable.Rows[i].Field<short>("fplFreightPackageID");
				eRPFreightPackageLinkInformationDto.fplFreightPackageLineID = dataTable.Rows[i].Field<short>("fplFreightPackageLineID");
				eRPFreightPackageLinkInformationDto.fplFreightShipmentID = dataTable.Rows[i].Field<string>("fplFreightShipmentID");
				eRPFreightPackageLinkInformationDto.fplRowVersion = dataTable.Rows[i].Field<byte[]>("fplRowVersion");
				eRPFreightPackageLinkInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPFreightPackageLinkInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPFreightPackageLinkInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPFreightPackageLinkInformationDto> GetFreightPackageLink(Guid freightPackageLinkId)
	{
		ERPFreightPackageLinkInformationDto eRPFreightPackageLinkInformationDto = new ERPFreightPackageLinkInformationDto();
		InitializeParameterLists();
		string[] collection = new string[7] { "fplCreatedBy", "fplCreatedDate", "fplUniqueID", "fplFreightPackageID", "fplFreightPackageLineID", "fplFreightShipmentID", "fplRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("fplUniqueID|C", freightPackageLinkId);
		AddCustomFieldsToSelectList("FreightPackageLinks");
		using (DataTable dataTable = GetAsDataTable("FreightPackageLinks", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPFreightPackageLinkInformationDto);
			}
			eRPFreightPackageLinkInformationDto.fplCreatedBy = dataTable.Rows[0].Field<string>("fplCreatedBy");
			eRPFreightPackageLinkInformationDto.fplCreatedDate = dataTable.Rows[0].Field<DateTime?>("fplCreatedDate");
			eRPFreightPackageLinkInformationDto.fplUniqueID = dataTable.Rows[0].Field<Guid>("fplUniqueID");
			eRPFreightPackageLinkInformationDto.fplFreightPackageID = dataTable.Rows[0].Field<short>("fplFreightPackageID");
			eRPFreightPackageLinkInformationDto.fplFreightPackageLineID = dataTable.Rows[0].Field<short>("fplFreightPackageLineID");
			eRPFreightPackageLinkInformationDto.fplFreightShipmentID = dataTable.Rows[0].Field<string>("fplFreightShipmentID");
			eRPFreightPackageLinkInformationDto.fplRowVersion = dataTable.Rows[0].Field<byte[]>("fplRowVersion");
			eRPFreightPackageLinkInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPFreightPackageLinkInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPFreightPackageLinkInformationDto);
	}

	public Task<APIValidationInfoDto> SaveFreightPackageLink(ERPFreightPackageLinkDto freightPackageLink)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM FreightPackageLinks WHERE fplUniqueID = " + M1Util.ConvertToLinq(freightPackageLink.fplUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["fplFreightShipmentID"] = freightPackageLink.fplFreightShipmentID.ToUpper();
				dataRow["fplFreightPackageID"] = freightPackageLink.fplFreightPackageID;
				dataRow["fplFreightPackageLineID"] = freightPackageLink.fplFreightPackageLineID;
				freightPackageLink.fplUniqueID = ((freightPackageLink.fplUniqueID == Guid.Empty) ? Guid.NewGuid() : freightPackageLink.fplUniqueID);
				dataRow["fplUniqueID"] = freightPackageLink.fplUniqueID;
				dataRow["fplCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["fplCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The FreightPackageLink could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (freightPackageLink.fplRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the FreightPackageLink is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["fplRowVersion"], freightPackageLink.fplRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the FreightPackageLink has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the FreightPackageLink again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			if (freightPackageLink.CustomFields != null && freightPackageLink.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in freightPackageLink.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the FreightPackageLink [{freightPackageLink.fplUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the FreightPackageLink [{freightPackageLink.fplUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
