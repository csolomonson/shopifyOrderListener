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

public class ERPToolRepository : APIBaseRepository, IERPToolRepository, IAPIBaseRepository, IDisposable
{
	public ERPToolRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesToolExist(Guid toolId)
	{
		InitializeParameterLists();
		base.filterList.Add("xttUniqueID|C", toolId);
		base.selectList.Add("xttUniqueID");
		return Task.FromResult(GetAsObject("Tools", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPToolInformationDto>> GetAllTools(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPToolInformationDto> collection = new List<ERPToolInformationDto>();
		InitializeParameterLists();
		string[] array = new string[21]
		{
			"xttAssetID", "xttCheckedOutToEmployeeID", "xttCheckoutReasonID", "xttToolID", "xttCreatedBy", "xttCreatedDate", "xttDescription", "xttDocuments", "xttUniqueID", "xttIdentificationNumber",
			"xttInactiveDate", "xttInactive", "xttLocation", "xttLongDescriptionRtf", "xttLongDescriptionText", "xttMovementDate", "xttMovementType", "xttPlannedReturnDate", "xttRowVersion", "xttToolCategoryID",
			"xttWorkCenterID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Tools");
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
		using (DataTable dataTable = GetAsDataTable("Tools", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPToolInformationDto eRPToolInformationDto = new ERPToolInformationDto();
				eRPToolInformationDto.xttAssetID = dataTable.Rows[i].Field<string>("xttAssetID");
				eRPToolInformationDto.xttCheckedOutToEmployeeID = dataTable.Rows[i].Field<string>("xttCheckedOutToEmployeeID");
				eRPToolInformationDto.xttCheckoutReasonID = dataTable.Rows[i].Field<string>("xttCheckoutReasonID");
				eRPToolInformationDto.xttToolID = dataTable.Rows[i].Field<string>("xttToolID");
				eRPToolInformationDto.xttCreatedBy = dataTable.Rows[i].Field<string>("xttCreatedBy");
				eRPToolInformationDto.xttCreatedDate = dataTable.Rows[i].Field<DateTime?>("xttCreatedDate");
				eRPToolInformationDto.xttDescription = dataTable.Rows[i].Field<string>("xttDescription");
				eRPToolInformationDto.xttDocuments = dataTable.Rows[i].Field<string>("xttDocuments");
				eRPToolInformationDto.xttUniqueID = dataTable.Rows[i].Field<Guid>("xttUniqueID");
				eRPToolInformationDto.xttIdentificationNumber = dataTable.Rows[i].Field<string>("xttIdentificationNumber");
				eRPToolInformationDto.xttInactiveDate = dataTable.Rows[i].Field<DateTime?>("xttInactiveDate");
				eRPToolInformationDto.xttInactive = dataTable.Rows[i].Field<bool>("xttInactive");
				eRPToolInformationDto.xttLocation = dataTable.Rows[i].Field<string>("xttLocation");
				eRPToolInformationDto.xttLongDescriptionRtf = dataTable.Rows[i].Field<string>("xttLongDescriptionRtf");
				eRPToolInformationDto.xttLongDescriptionText = dataTable.Rows[i].Field<string>("xttLongDescriptionText");
				eRPToolInformationDto.xttMovementDate = dataTable.Rows[i].Field<DateTime?>("xttMovementDate");
				eRPToolInformationDto.xttMovementType = dataTable.Rows[i].Field<string>("xttMovementType");
				eRPToolInformationDto.xttPlannedReturnDate = dataTable.Rows[i].Field<DateTime?>("xttPlannedReturnDate");
				eRPToolInformationDto.xttRowVersion = dataTable.Rows[i].Field<byte[]>("xttRowVersion");
				eRPToolInformationDto.xttToolCategoryID = dataTable.Rows[i].Field<string>("xttToolCategoryID");
				eRPToolInformationDto.xttWorkCenterID = dataTable.Rows[i].Field<string>("xttWorkCenterID");
				eRPToolInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPToolInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPToolInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPToolInformationDto> GetTool(Guid toolId)
	{
		ERPToolInformationDto eRPToolInformationDto = new ERPToolInformationDto();
		InitializeParameterLists();
		string[] collection = new string[21]
		{
			"xttAssetID", "xttCheckedOutToEmployeeID", "xttCheckoutReasonID", "xttToolID", "xttCreatedBy", "xttCreatedDate", "xttDescription", "xttDocuments", "xttUniqueID", "xttIdentificationNumber",
			"xttInactiveDate", "xttInactive", "xttLocation", "xttLongDescriptionRtf", "xttLongDescriptionText", "xttMovementDate", "xttMovementType", "xttPlannedReturnDate", "xttRowVersion", "xttToolCategoryID",
			"xttWorkCenterID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xttUniqueID|C", toolId);
		AddCustomFieldsToSelectList("Tools");
		using (DataTable dataTable = GetAsDataTable("Tools", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPToolInformationDto);
			}
			eRPToolInformationDto.xttAssetID = dataTable.Rows[0].Field<string>("xttAssetID");
			eRPToolInformationDto.xttCheckedOutToEmployeeID = dataTable.Rows[0].Field<string>("xttCheckedOutToEmployeeID");
			eRPToolInformationDto.xttCheckoutReasonID = dataTable.Rows[0].Field<string>("xttCheckoutReasonID");
			eRPToolInformationDto.xttToolID = dataTable.Rows[0].Field<string>("xttToolID");
			eRPToolInformationDto.xttCreatedBy = dataTable.Rows[0].Field<string>("xttCreatedBy");
			eRPToolInformationDto.xttCreatedDate = dataTable.Rows[0].Field<DateTime?>("xttCreatedDate");
			eRPToolInformationDto.xttDescription = dataTable.Rows[0].Field<string>("xttDescription");
			eRPToolInformationDto.xttDocuments = dataTable.Rows[0].Field<string>("xttDocuments");
			eRPToolInformationDto.xttUniqueID = dataTable.Rows[0].Field<Guid>("xttUniqueID");
			eRPToolInformationDto.xttIdentificationNumber = dataTable.Rows[0].Field<string>("xttIdentificationNumber");
			eRPToolInformationDto.xttInactiveDate = dataTable.Rows[0].Field<DateTime?>("xttInactiveDate");
			eRPToolInformationDto.xttInactive = dataTable.Rows[0].Field<bool>("xttInactive");
			eRPToolInformationDto.xttLocation = dataTable.Rows[0].Field<string>("xttLocation");
			eRPToolInformationDto.xttLongDescriptionRtf = dataTable.Rows[0].Field<string>("xttLongDescriptionRtf");
			eRPToolInformationDto.xttLongDescriptionText = dataTable.Rows[0].Field<string>("xttLongDescriptionText");
			eRPToolInformationDto.xttMovementDate = dataTable.Rows[0].Field<DateTime?>("xttMovementDate");
			eRPToolInformationDto.xttMovementType = dataTable.Rows[0].Field<string>("xttMovementType");
			eRPToolInformationDto.xttPlannedReturnDate = dataTable.Rows[0].Field<DateTime?>("xttPlannedReturnDate");
			eRPToolInformationDto.xttRowVersion = dataTable.Rows[0].Field<byte[]>("xttRowVersion");
			eRPToolInformationDto.xttToolCategoryID = dataTable.Rows[0].Field<string>("xttToolCategoryID");
			eRPToolInformationDto.xttWorkCenterID = dataTable.Rows[0].Field<string>("xttWorkCenterID");
			eRPToolInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPToolInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPToolInformationDto);
	}

	public Task<APIValidationInfoDto> SaveTool(ERPToolDto tool)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Tools WHERE xttUniqueID = " + M1Util.ConvertToLinq(tool.xttUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xttToolID"] = tool.xttToolID.ToUpper();
				tool.xttUniqueID = ((tool.xttUniqueID == Guid.Empty) ? Guid.NewGuid() : tool.xttUniqueID);
				dataRow["xttUniqueID"] = tool.xttUniqueID;
				dataRow["xttCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xttCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Tool could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (tool.xttRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Tool is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xttRowVersion"], tool.xttRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Tool has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Tool again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xttAssetID"] = tool.xttAssetID;
			dataRow["xttCheckedOutToEmployeeID"] = tool.xttCheckedOutToEmployeeID;
			dataRow["xttCheckoutReasonID"] = tool.xttCheckoutReasonID;
			dataRow["xttDescription"] = tool.xttDescription;
			dataRow["xttDocuments"] = tool.xttDocuments ?? dataRow["xttDocuments"];
			dataRow["xttIdentificationNumber"] = tool.xttIdentificationNumber;
			DataRow dataRow2 = dataRow;
			DateTime? xttInactiveDate = tool.xttInactiveDate;
			dataRow2["xttInactiveDate"] = (xttInactiveDate.HasValue ? ((object)xttInactiveDate.GetValueOrDefault()) : dataRow["xttInactiveDate"]);
			dataRow["xttInactive"] = tool.xttInactive;
			dataRow["xttLocation"] = tool.xttLocation;
			dataRow["xttLongDescriptionRtf"] = tool.xttLongDescriptionRtf ?? dataRow["xttLongDescriptionRtf"];
			dataRow["xttLongDescriptionText"] = tool.xttLongDescriptionText ?? dataRow["xttLongDescriptionText"];
			DataRow dataRow3 = dataRow;
			xttInactiveDate = tool.xttMovementDate;
			dataRow3["xttMovementDate"] = (xttInactiveDate.HasValue ? ((object)xttInactiveDate.GetValueOrDefault()) : dataRow["xttMovementDate"]);
			dataRow["xttMovementType"] = tool.xttMovementType;
			DataRow dataRow4 = dataRow;
			xttInactiveDate = tool.xttPlannedReturnDate;
			dataRow4["xttPlannedReturnDate"] = (xttInactiveDate.HasValue ? ((object)xttInactiveDate.GetValueOrDefault()) : dataRow["xttPlannedReturnDate"]);
			dataRow["xttToolCategoryID"] = tool.xttToolCategoryID;
			dataRow["xttWorkCenterID"] = tool.xttWorkCenterID;
			if (tool.CustomFields != null && tool.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in tool.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Tool [{tool.xttUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Tool [{tool.xttUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
