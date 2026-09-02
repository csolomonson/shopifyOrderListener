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

public class ERPDocumentLinkRepository : APIBaseRepository, IERPDocumentLinkRepository, IAPIBaseRepository, IDisposable
{
	public ERPDocumentLinkRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesDocumentLinkExist(Guid documentLinkId)
	{
		InitializeParameterLists();
		base.filterList.Add("xalUniqueID|C", documentLinkId);
		base.selectList.Add("xalUniqueID");
		return Task.FromResult(GetAsObject("DocumentLinks", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPDocumentLinkInformationDto>> GetAllDocumentLinks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPDocumentLinkInformationDto> collection = new List<ERPDocumentLinkInformationDto>();
		InitializeParameterLists();
		string[] array = new string[16]
		{
			"xalAddedByUserID", "xalAddedDate", "xalCloudFileId", "xalCreatedBy", "xalCreatedDate", "xalDescription", "xalUniqueID", "xalFileLastModifiedDate", "xalFileName", "xalFileNameWhenUploaded",
			"xalEmailDefault", "xalPrintDefault", "xalReference", "xalRowVersion", "xalDocumentLinkID", "xalType"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("DocumentLinks");
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
		using (DataTable dataTable = GetAsDataTable("DocumentLinks", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPDocumentLinkInformationDto eRPDocumentLinkInformationDto = new ERPDocumentLinkInformationDto();
				eRPDocumentLinkInformationDto.xalAddedByUserID = dataTable.Rows[i].Field<string>("xalAddedByUserID");
				eRPDocumentLinkInformationDto.xalAddedDate = dataTable.Rows[i].Field<DateTime?>("xalAddedDate");
				eRPDocumentLinkInformationDto.xalCloudFileId = dataTable.Rows[i].Field<string>("xalCloudFileId");
				eRPDocumentLinkInformationDto.xalCreatedBy = dataTable.Rows[i].Field<string>("xalCreatedBy");
				eRPDocumentLinkInformationDto.xalCreatedDate = dataTable.Rows[i].Field<DateTime?>("xalCreatedDate");
				eRPDocumentLinkInformationDto.xalDescription = dataTable.Rows[i].Field<string>("xalDescription");
				eRPDocumentLinkInformationDto.xalUniqueID = dataTable.Rows[i].Field<Guid>("xalUniqueID");
				eRPDocumentLinkInformationDto.xalFileLastModifiedDate = dataTable.Rows[i].Field<DateTime?>("xalFileLastModifiedDate");
				eRPDocumentLinkInformationDto.xalFileName = dataTable.Rows[i].Field<string>("xalFileName");
				eRPDocumentLinkInformationDto.xalFileNameWhenUploaded = dataTable.Rows[i].Field<string>("xalFileNameWhenUploaded");
				eRPDocumentLinkInformationDto.xalEmailDefault = dataTable.Rows[i].Field<bool>("xalEmailDefault");
				eRPDocumentLinkInformationDto.xalPrintDefault = dataTable.Rows[i].Field<bool>("xalPrintDefault");
				eRPDocumentLinkInformationDto.xalReference = dataTable.Rows[i].Field<string>("xalReference");
				eRPDocumentLinkInformationDto.xalRowVersion = dataTable.Rows[i].Field<byte[]>("xalRowVersion");
				eRPDocumentLinkInformationDto.xalDocumentLinkID = dataTable.Rows[i].Field<int>("xalDocumentLinkID");
				eRPDocumentLinkInformationDto.xalType = dataTable.Rows[i].Field<string>("xalType");
				eRPDocumentLinkInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPDocumentLinkInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPDocumentLinkInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPDocumentLinkInformationDto> GetDocumentLink(Guid documentLinkId)
	{
		ERPDocumentLinkInformationDto eRPDocumentLinkInformationDto = new ERPDocumentLinkInformationDto();
		InitializeParameterLists();
		string[] collection = new string[16]
		{
			"xalAddedByUserID", "xalAddedDate", "xalCloudFileId", "xalCreatedBy", "xalCreatedDate", "xalDescription", "xalUniqueID", "xalFileLastModifiedDate", "xalFileName", "xalFileNameWhenUploaded",
			"xalEmailDefault", "xalPrintDefault", "xalReference", "xalRowVersion", "xalDocumentLinkID", "xalType"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xalUniqueID|C", documentLinkId);
		AddCustomFieldsToSelectList("DocumentLinks");
		using (DataTable dataTable = GetAsDataTable("DocumentLinks", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPDocumentLinkInformationDto);
			}
			eRPDocumentLinkInformationDto.xalAddedByUserID = dataTable.Rows[0].Field<string>("xalAddedByUserID");
			eRPDocumentLinkInformationDto.xalAddedDate = dataTable.Rows[0].Field<DateTime?>("xalAddedDate");
			eRPDocumentLinkInformationDto.xalCloudFileId = dataTable.Rows[0].Field<string>("xalCloudFileId");
			eRPDocumentLinkInformationDto.xalCreatedBy = dataTable.Rows[0].Field<string>("xalCreatedBy");
			eRPDocumentLinkInformationDto.xalCreatedDate = dataTable.Rows[0].Field<DateTime?>("xalCreatedDate");
			eRPDocumentLinkInformationDto.xalDescription = dataTable.Rows[0].Field<string>("xalDescription");
			eRPDocumentLinkInformationDto.xalUniqueID = dataTable.Rows[0].Field<Guid>("xalUniqueID");
			eRPDocumentLinkInformationDto.xalFileLastModifiedDate = dataTable.Rows[0].Field<DateTime?>("xalFileLastModifiedDate");
			eRPDocumentLinkInformationDto.xalFileName = dataTable.Rows[0].Field<string>("xalFileName");
			eRPDocumentLinkInformationDto.xalFileNameWhenUploaded = dataTable.Rows[0].Field<string>("xalFileNameWhenUploaded");
			eRPDocumentLinkInformationDto.xalEmailDefault = dataTable.Rows[0].Field<bool>("xalEmailDefault");
			eRPDocumentLinkInformationDto.xalPrintDefault = dataTable.Rows[0].Field<bool>("xalPrintDefault");
			eRPDocumentLinkInformationDto.xalReference = dataTable.Rows[0].Field<string>("xalReference");
			eRPDocumentLinkInformationDto.xalRowVersion = dataTable.Rows[0].Field<byte[]>("xalRowVersion");
			eRPDocumentLinkInformationDto.xalDocumentLinkID = dataTable.Rows[0].Field<int>("xalDocumentLinkID");
			eRPDocumentLinkInformationDto.xalType = dataTable.Rows[0].Field<string>("xalType");
			eRPDocumentLinkInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPDocumentLinkInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPDocumentLinkInformationDto);
	}

	public Task<APIValidationInfoDto> SaveDocumentLink(ERPDocumentLinkDto documentLink)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM DocumentLinks WHERE xalUniqueID = " + M1Util.ConvertToLinq(documentLink.xalUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xalDocumentLinkID"] = documentLink.xalDocumentLinkID;
				documentLink.xalUniqueID = ((documentLink.xalUniqueID == Guid.Empty) ? Guid.NewGuid() : documentLink.xalUniqueID);
				dataRow["xalUniqueID"] = documentLink.xalUniqueID;
				dataRow["xalCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xalCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The DocumentLink could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (documentLink.xalRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the DocumentLink is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xalRowVersion"], documentLink.xalRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the DocumentLink has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the DocumentLink again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xalAddedByUserID"] = documentLink.xalAddedByUserID;
			DataRow dataRow2 = dataRow;
			DateTime? xalAddedDate = documentLink.xalAddedDate;
			dataRow2["xalAddedDate"] = (xalAddedDate.HasValue ? ((object)xalAddedDate.GetValueOrDefault()) : dataRow["xalAddedDate"]);
			dataRow["xalCloudFileId"] = documentLink.xalCloudFileId ?? dataRow["xalCloudFileId"];
			dataRow["xalDescription"] = documentLink.xalDescription;
			DataRow dataRow3 = dataRow;
			xalAddedDate = documentLink.xalFileLastModifiedDate;
			dataRow3["xalFileLastModifiedDate"] = (xalAddedDate.HasValue ? ((object)xalAddedDate.GetValueOrDefault()) : dataRow["xalFileLastModifiedDate"]);
			dataRow["xalFileName"] = documentLink.xalFileName ?? dataRow["xalFileName"];
			dataRow["xalFileNameWhenUploaded"] = documentLink.xalFileNameWhenUploaded ?? dataRow["xalFileNameWhenUploaded"];
			dataRow["xalEmailDefault"] = documentLink.xalEmailDefault;
			dataRow["xalPrintDefault"] = documentLink.xalPrintDefault;
			dataRow["xalReference"] = documentLink.xalReference;
			dataRow["xalType"] = documentLink.xalType;
			if (documentLink.CustomFields != null && documentLink.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in documentLink.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the DocumentLink [{documentLink.xalUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the DocumentLink [{documentLink.xalUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
