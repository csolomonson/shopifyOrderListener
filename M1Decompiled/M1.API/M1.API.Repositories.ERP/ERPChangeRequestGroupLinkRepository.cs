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

public class ERPChangeRequestGroupLinkRepository : APIBaseRepository, IERPChangeRequestGroupLinkRepository, IAPIBaseRepository, IDisposable
{
	public ERPChangeRequestGroupLinkRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesChangeRequestGroupLinkExist(Guid changeRequestGroupLinkId)
	{
		InitializeParameterLists();
		base.filterList.Add("chrUniqueID|C", changeRequestGroupLinkId);
		base.selectList.Add("chrUniqueID");
		return Task.FromResult(GetAsObject("ChangeRequestGroupLinks", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPChangeRequestGroupLinkInformationDto>> GetAllChangeRequestGroupLinks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPChangeRequestGroupLinkInformationDto> collection = new List<ERPChangeRequestGroupLinkInformationDto>();
		InitializeParameterLists();
		string[] array = new string[7] { "chrChangeRequestGroupID", "chrChangeRequestID", "chrCreatedBy", "chrCreatedDate", "chrUniqueID", "chrRowVersion", "chrChangeRequestGroupLinkID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ChangeRequestGroupLinks");
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
		using (DataTable dataTable = GetAsDataTable("ChangeRequestGroupLinks", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPChangeRequestGroupLinkInformationDto eRPChangeRequestGroupLinkInformationDto = new ERPChangeRequestGroupLinkInformationDto();
				eRPChangeRequestGroupLinkInformationDto.chrChangeRequestGroupID = dataTable.Rows[i].Field<string>("chrChangeRequestGroupID");
				eRPChangeRequestGroupLinkInformationDto.chrChangeRequestID = dataTable.Rows[i].Field<string>("chrChangeRequestID");
				eRPChangeRequestGroupLinkInformationDto.chrCreatedBy = dataTable.Rows[i].Field<string>("chrCreatedBy");
				eRPChangeRequestGroupLinkInformationDto.chrCreatedDate = dataTable.Rows[i].Field<DateTime?>("chrCreatedDate");
				eRPChangeRequestGroupLinkInformationDto.chrUniqueID = dataTable.Rows[i].Field<Guid>("chrUniqueID");
				eRPChangeRequestGroupLinkInformationDto.chrRowVersion = dataTable.Rows[i].Field<byte[]>("chrRowVersion");
				eRPChangeRequestGroupLinkInformationDto.chrChangeRequestGroupLinkID = dataTable.Rows[i].Field<short>("chrChangeRequestGroupLinkID");
				eRPChangeRequestGroupLinkInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPChangeRequestGroupLinkInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPChangeRequestGroupLinkInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPChangeRequestGroupLinkInformationDto> GetChangeRequestGroupLink(Guid changeRequestGroupLinkId)
	{
		ERPChangeRequestGroupLinkInformationDto eRPChangeRequestGroupLinkInformationDto = new ERPChangeRequestGroupLinkInformationDto();
		InitializeParameterLists();
		string[] collection = new string[7] { "chrChangeRequestGroupID", "chrChangeRequestID", "chrCreatedBy", "chrCreatedDate", "chrUniqueID", "chrRowVersion", "chrChangeRequestGroupLinkID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("chrUniqueID|C", changeRequestGroupLinkId);
		AddCustomFieldsToSelectList("ChangeRequestGroupLinks");
		using (DataTable dataTable = GetAsDataTable("ChangeRequestGroupLinks", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPChangeRequestGroupLinkInformationDto);
			}
			eRPChangeRequestGroupLinkInformationDto.chrChangeRequestGroupID = dataTable.Rows[0].Field<string>("chrChangeRequestGroupID");
			eRPChangeRequestGroupLinkInformationDto.chrChangeRequestID = dataTable.Rows[0].Field<string>("chrChangeRequestID");
			eRPChangeRequestGroupLinkInformationDto.chrCreatedBy = dataTable.Rows[0].Field<string>("chrCreatedBy");
			eRPChangeRequestGroupLinkInformationDto.chrCreatedDate = dataTable.Rows[0].Field<DateTime?>("chrCreatedDate");
			eRPChangeRequestGroupLinkInformationDto.chrUniqueID = dataTable.Rows[0].Field<Guid>("chrUniqueID");
			eRPChangeRequestGroupLinkInformationDto.chrRowVersion = dataTable.Rows[0].Field<byte[]>("chrRowVersion");
			eRPChangeRequestGroupLinkInformationDto.chrChangeRequestGroupLinkID = dataTable.Rows[0].Field<short>("chrChangeRequestGroupLinkID");
			eRPChangeRequestGroupLinkInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPChangeRequestGroupLinkInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPChangeRequestGroupLinkInformationDto);
	}

	public Task<APIValidationInfoDto> SaveChangeRequestGroupLink(ERPChangeRequestGroupLinkDto changeRequestGroupLink)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ChangeRequestGroupLinks WHERE chrUniqueID = " + M1Util.ConvertToLinq(changeRequestGroupLink.chrUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["chrChangeRequestID"] = changeRequestGroupLink.chrChangeRequestID.ToUpper();
				dataRow["chrChangeRequestGroupLinkID"] = changeRequestGroupLink.chrChangeRequestGroupLinkID;
				changeRequestGroupLink.chrUniqueID = ((changeRequestGroupLink.chrUniqueID == Guid.Empty) ? Guid.NewGuid() : changeRequestGroupLink.chrUniqueID);
				dataRow["chrUniqueID"] = changeRequestGroupLink.chrUniqueID;
				dataRow["chrCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["chrCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ChangeRequestGroupLink could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (changeRequestGroupLink.chrRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ChangeRequestGroupLink is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["chrRowVersion"], changeRequestGroupLink.chrRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ChangeRequestGroupLink has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ChangeRequestGroupLink again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["chrChangeRequestGroupID"] = changeRequestGroupLink.chrChangeRequestGroupID;
			if (changeRequestGroupLink.CustomFields != null && changeRequestGroupLink.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in changeRequestGroupLink.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ChangeRequestGroupLink [{changeRequestGroupLink.chrUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ChangeRequestGroupLink [{changeRequestGroupLink.chrUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
