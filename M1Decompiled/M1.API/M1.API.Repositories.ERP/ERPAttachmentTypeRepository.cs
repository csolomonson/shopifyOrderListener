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

public class ERPAttachmentTypeRepository : APIBaseRepository, IERPAttachmentTypeRepository, IAPIBaseRepository, IDisposable
{
	public ERPAttachmentTypeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAttachmentTypeExist(Guid attachmentTypeId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmtUniqueID|C", attachmentTypeId);
		base.selectList.Add("cmtUniqueID");
		return Task.FromResult(GetAsObject("AttachmentTypes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAttachmentTypeInformationDto>> GetAllAttachmentTypes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAttachmentTypeInformationDto> collection = new List<ERPAttachmentTypeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[8] { "cmtAttachmentTypeID", "cmtCreatedBy", "cmtCreatedDate", "cmtDescription", "cmtUniqueID", "cmtRequiresLogin", "cmtRequiresServiceContract", "cmtRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("AttachmentTypes");
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
		using (DataTable dataTable = GetAsDataTable("AttachmentTypes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAttachmentTypeInformationDto eRPAttachmentTypeInformationDto = new ERPAttachmentTypeInformationDto();
				eRPAttachmentTypeInformationDto.cmtAttachmentTypeID = dataTable.Rows[i].Field<string>("cmtAttachmentTypeID");
				eRPAttachmentTypeInformationDto.cmtCreatedBy = dataTable.Rows[i].Field<string>("cmtCreatedBy");
				eRPAttachmentTypeInformationDto.cmtCreatedDate = dataTable.Rows[i].Field<DateTime?>("cmtCreatedDate");
				eRPAttachmentTypeInformationDto.cmtDescription = dataTable.Rows[i].Field<string>("cmtDescription");
				eRPAttachmentTypeInformationDto.cmtUniqueID = dataTable.Rows[i].Field<Guid>("cmtUniqueID");
				eRPAttachmentTypeInformationDto.cmtRequiresLogin = dataTable.Rows[i].Field<bool>("cmtRequiresLogin");
				eRPAttachmentTypeInformationDto.cmtRequiresServiceContract = dataTable.Rows[i].Field<bool>("cmtRequiresServiceContract");
				eRPAttachmentTypeInformationDto.cmtRowVersion = dataTable.Rows[i].Field<byte[]>("cmtRowVersion");
				eRPAttachmentTypeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAttachmentTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAttachmentTypeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAttachmentTypeInformationDto> GetAttachmentType(Guid attachmentTypeId)
	{
		ERPAttachmentTypeInformationDto eRPAttachmentTypeInformationDto = new ERPAttachmentTypeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[8] { "cmtAttachmentTypeID", "cmtCreatedBy", "cmtCreatedDate", "cmtDescription", "cmtUniqueID", "cmtRequiresLogin", "cmtRequiresServiceContract", "cmtRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("cmtUniqueID|C", attachmentTypeId);
		AddCustomFieldsToSelectList("AttachmentTypes");
		using (DataTable dataTable = GetAsDataTable("AttachmentTypes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAttachmentTypeInformationDto);
			}
			eRPAttachmentTypeInformationDto.cmtAttachmentTypeID = dataTable.Rows[0].Field<string>("cmtAttachmentTypeID");
			eRPAttachmentTypeInformationDto.cmtCreatedBy = dataTable.Rows[0].Field<string>("cmtCreatedBy");
			eRPAttachmentTypeInformationDto.cmtCreatedDate = dataTable.Rows[0].Field<DateTime?>("cmtCreatedDate");
			eRPAttachmentTypeInformationDto.cmtDescription = dataTable.Rows[0].Field<string>("cmtDescription");
			eRPAttachmentTypeInformationDto.cmtUniqueID = dataTable.Rows[0].Field<Guid>("cmtUniqueID");
			eRPAttachmentTypeInformationDto.cmtRequiresLogin = dataTable.Rows[0].Field<bool>("cmtRequiresLogin");
			eRPAttachmentTypeInformationDto.cmtRequiresServiceContract = dataTable.Rows[0].Field<bool>("cmtRequiresServiceContract");
			eRPAttachmentTypeInformationDto.cmtRowVersion = dataTable.Rows[0].Field<byte[]>("cmtRowVersion");
			eRPAttachmentTypeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAttachmentTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAttachmentTypeInformationDto);
	}

	public Task<APIValidationInfoDto> SaveAttachmentType(ERPAttachmentTypeDto attachmentType)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM AttachmentTypes WHERE cmtUniqueID = " + M1Util.ConvertToLinq(attachmentType.cmtUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cmtAttachmentTypeID"] = attachmentType.cmtAttachmentTypeID.ToUpper();
				attachmentType.cmtUniqueID = ((attachmentType.cmtUniqueID == Guid.Empty) ? Guid.NewGuid() : attachmentType.cmtUniqueID);
				dataRow["cmtUniqueID"] = attachmentType.cmtUniqueID;
				dataRow["cmtCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cmtCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The AttachmentType could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (attachmentType.cmtRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the AttachmentType is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cmtRowVersion"], attachmentType.cmtRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the AttachmentType has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the AttachmentType again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["cmtDescription"] = attachmentType.cmtDescription;
			dataRow["cmtRequiresLogin"] = attachmentType.cmtRequiresLogin;
			dataRow["cmtRequiresServiceContract"] = attachmentType.cmtRequiresServiceContract;
			if (attachmentType.CustomFields != null && attachmentType.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in attachmentType.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the AttachmentType [{attachmentType.cmtUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the AttachmentType [{attachmentType.cmtUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
