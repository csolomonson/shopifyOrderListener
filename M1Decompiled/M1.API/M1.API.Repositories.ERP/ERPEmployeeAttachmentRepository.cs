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

public class ERPEmployeeAttachmentRepository : APIBaseRepository, IERPEmployeeAttachmentRepository, IAPIBaseRepository, IDisposable
{
	public ERPEmployeeAttachmentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesEmployeeAttachmentExist(Guid employeeAttachmentId)
	{
		InitializeParameterLists();
		base.filterList.Add("lmaUniqueID|C", employeeAttachmentId);
		base.selectList.Add("lmaUniqueID");
		return Task.FromResult(GetAsObject("EmployeeAttachments", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPEmployeeAttachmentInformationDto>> GetAllEmployeeAttachments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPEmployeeAttachmentInformationDto> collection = new List<ERPEmployeeAttachmentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[13]
		{
			"lmaAttachmentTypeID", "lmaEmployeeAttachmentID", "lmaCreatedBy", "lmaCreatedDate", "lmaDate", "lmaEmployeeID", "lmaUniqueID", "lmaFileLocation", "lmaFileName", "lmaLongDescriptionRtf",
			"lmaLongDescriptionText", "lmaRowVersion", "lmaShortDescription"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("EmployeeAttachments");
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
		using (DataTable dataTable = GetAsDataTable("EmployeeAttachments", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPEmployeeAttachmentInformationDto eRPEmployeeAttachmentInformationDto = new ERPEmployeeAttachmentInformationDto();
				eRPEmployeeAttachmentInformationDto.lmaAttachmentTypeID = dataTable.Rows[i].Field<string>("lmaAttachmentTypeID");
				eRPEmployeeAttachmentInformationDto.lmaEmployeeAttachmentID = dataTable.Rows[i].Field<string>("lmaEmployeeAttachmentID");
				eRPEmployeeAttachmentInformationDto.lmaCreatedBy = dataTable.Rows[i].Field<string>("lmaCreatedBy");
				eRPEmployeeAttachmentInformationDto.lmaCreatedDate = dataTable.Rows[i].Field<DateTime?>("lmaCreatedDate");
				eRPEmployeeAttachmentInformationDto.lmaDate = dataTable.Rows[i].Field<DateTime?>("lmaDate");
				eRPEmployeeAttachmentInformationDto.lmaEmployeeID = dataTable.Rows[i].Field<string>("lmaEmployeeID");
				eRPEmployeeAttachmentInformationDto.lmaUniqueID = dataTable.Rows[i].Field<Guid>("lmaUniqueID");
				eRPEmployeeAttachmentInformationDto.lmaFileLocation = dataTable.Rows[i].Field<string>("lmaFileLocation");
				eRPEmployeeAttachmentInformationDto.lmaFileName = dataTable.Rows[i].Field<string>("lmaFileName");
				eRPEmployeeAttachmentInformationDto.lmaLongDescriptionRtf = dataTable.Rows[i].Field<string>("lmaLongDescriptionRtf");
				eRPEmployeeAttachmentInformationDto.lmaLongDescriptionText = dataTable.Rows[i].Field<string>("lmaLongDescriptionText");
				eRPEmployeeAttachmentInformationDto.lmaRowVersion = dataTable.Rows[i].Field<byte[]>("lmaRowVersion");
				eRPEmployeeAttachmentInformationDto.lmaShortDescription = dataTable.Rows[i].Field<string>("lmaShortDescription");
				eRPEmployeeAttachmentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPEmployeeAttachmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPEmployeeAttachmentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPEmployeeAttachmentInformationDto> GetEmployeeAttachment(Guid employeeAttachmentId)
	{
		ERPEmployeeAttachmentInformationDto eRPEmployeeAttachmentInformationDto = new ERPEmployeeAttachmentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[13]
		{
			"lmaAttachmentTypeID", "lmaEmployeeAttachmentID", "lmaCreatedBy", "lmaCreatedDate", "lmaDate", "lmaEmployeeID", "lmaUniqueID", "lmaFileLocation", "lmaFileName", "lmaLongDescriptionRtf",
			"lmaLongDescriptionText", "lmaRowVersion", "lmaShortDescription"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("lmaUniqueID|C", employeeAttachmentId);
		AddCustomFieldsToSelectList("EmployeeAttachments");
		using (DataTable dataTable = GetAsDataTable("EmployeeAttachments", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPEmployeeAttachmentInformationDto);
			}
			eRPEmployeeAttachmentInformationDto.lmaAttachmentTypeID = dataTable.Rows[0].Field<string>("lmaAttachmentTypeID");
			eRPEmployeeAttachmentInformationDto.lmaEmployeeAttachmentID = dataTable.Rows[0].Field<string>("lmaEmployeeAttachmentID");
			eRPEmployeeAttachmentInformationDto.lmaCreatedBy = dataTable.Rows[0].Field<string>("lmaCreatedBy");
			eRPEmployeeAttachmentInformationDto.lmaCreatedDate = dataTable.Rows[0].Field<DateTime?>("lmaCreatedDate");
			eRPEmployeeAttachmentInformationDto.lmaDate = dataTable.Rows[0].Field<DateTime?>("lmaDate");
			eRPEmployeeAttachmentInformationDto.lmaEmployeeID = dataTable.Rows[0].Field<string>("lmaEmployeeID");
			eRPEmployeeAttachmentInformationDto.lmaUniqueID = dataTable.Rows[0].Field<Guid>("lmaUniqueID");
			eRPEmployeeAttachmentInformationDto.lmaFileLocation = dataTable.Rows[0].Field<string>("lmaFileLocation");
			eRPEmployeeAttachmentInformationDto.lmaFileName = dataTable.Rows[0].Field<string>("lmaFileName");
			eRPEmployeeAttachmentInformationDto.lmaLongDescriptionRtf = dataTable.Rows[0].Field<string>("lmaLongDescriptionRtf");
			eRPEmployeeAttachmentInformationDto.lmaLongDescriptionText = dataTable.Rows[0].Field<string>("lmaLongDescriptionText");
			eRPEmployeeAttachmentInformationDto.lmaRowVersion = dataTable.Rows[0].Field<byte[]>("lmaRowVersion");
			eRPEmployeeAttachmentInformationDto.lmaShortDescription = dataTable.Rows[0].Field<string>("lmaShortDescription");
			eRPEmployeeAttachmentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPEmployeeAttachmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPEmployeeAttachmentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveEmployeeAttachment(ERPEmployeeAttachmentDto employeeAttachment)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM EmployeeAttachments WHERE lmaUniqueID = " + M1Util.ConvertToLinq(employeeAttachment.lmaUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["lmaEmployeeAttachmentID"] = employeeAttachment.lmaEmployeeAttachmentID.ToUpper();
				employeeAttachment.lmaUniqueID = ((employeeAttachment.lmaUniqueID == Guid.Empty) ? Guid.NewGuid() : employeeAttachment.lmaUniqueID);
				dataRow["lmaUniqueID"] = employeeAttachment.lmaUniqueID;
				dataRow["lmaCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["lmaCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The EmployeeAttachment could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (employeeAttachment.lmaRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the EmployeeAttachment is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["lmaRowVersion"], employeeAttachment.lmaRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the EmployeeAttachment has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the EmployeeAttachment again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["lmaAttachmentTypeID"] = employeeAttachment.lmaAttachmentTypeID;
			DataRow dataRow2 = dataRow;
			DateTime? lmaDate = employeeAttachment.lmaDate;
			dataRow2["lmaDate"] = (lmaDate.HasValue ? ((object)lmaDate.GetValueOrDefault()) : dataRow["lmaDate"]);
			dataRow["lmaEmployeeID"] = employeeAttachment.lmaEmployeeID;
			dataRow["lmaFileLocation"] = employeeAttachment.lmaFileLocation;
			dataRow["lmaFileName"] = employeeAttachment.lmaFileName;
			dataRow["lmaLongDescriptionRtf"] = employeeAttachment.lmaLongDescriptionRtf ?? dataRow["lmaLongDescriptionRtf"];
			dataRow["lmaLongDescriptionText"] = employeeAttachment.lmaLongDescriptionText ?? dataRow["lmaLongDescriptionText"];
			dataRow["lmaShortDescription"] = employeeAttachment.lmaShortDescription;
			if (employeeAttachment.CustomFields != null && employeeAttachment.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in employeeAttachment.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the EmployeeAttachment [{employeeAttachment.lmaUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the EmployeeAttachment [{employeeAttachment.lmaUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
