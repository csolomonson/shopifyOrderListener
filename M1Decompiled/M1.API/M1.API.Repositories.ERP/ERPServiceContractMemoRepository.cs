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

public class ERPServiceContractMemoRepository : APIBaseRepository, IERPServiceContractMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPServiceContractMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesServiceContractMemoExist(Guid serviceContractMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("kbmUniqueID|C", serviceContractMemoId);
		base.selectList.Add("kbmUniqueID");
		return Task.FromResult(GetAsObject("ServiceContractMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPServiceContractMemoInformationDto>> GetAllServiceContractMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPServiceContractMemoInformationDto> collection = new List<ERPServiceContractMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "kbmCreatedBy", "kbmCreatedDate", "kbmUniqueID", "kbmLongDescriptionRtf", "kbmLongDescriptionText", "kbmMemoDate", "kbmRowVersion", "kbmServiceContractMemoID", "kbmServiceContractID", "kbmShortDescription" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ServiceContractMemos");
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
		using (DataTable dataTable = GetAsDataTable("ServiceContractMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPServiceContractMemoInformationDto eRPServiceContractMemoInformationDto = new ERPServiceContractMemoInformationDto();
				eRPServiceContractMemoInformationDto.kbmCreatedBy = dataTable.Rows[i].Field<string>("kbmCreatedBy");
				eRPServiceContractMemoInformationDto.kbmCreatedDate = dataTable.Rows[i].Field<DateTime?>("kbmCreatedDate");
				eRPServiceContractMemoInformationDto.kbmUniqueID = dataTable.Rows[i].Field<Guid>("kbmUniqueID");
				eRPServiceContractMemoInformationDto.kbmLongDescriptionRtf = dataTable.Rows[i].Field<string>("kbmLongDescriptionRtf");
				eRPServiceContractMemoInformationDto.kbmLongDescriptionText = dataTable.Rows[i].Field<string>("kbmLongDescriptionText");
				eRPServiceContractMemoInformationDto.kbmMemoDate = dataTable.Rows[i].Field<DateTime?>("kbmMemoDate");
				eRPServiceContractMemoInformationDto.kbmRowVersion = dataTable.Rows[i].Field<byte[]>("kbmRowVersion");
				eRPServiceContractMemoInformationDto.kbmServiceContractMemoID = dataTable.Rows[i].Field<short>("kbmServiceContractMemoID");
				eRPServiceContractMemoInformationDto.kbmServiceContractID = dataTable.Rows[i].Field<string>("kbmServiceContractID");
				eRPServiceContractMemoInformationDto.kbmShortDescription = dataTable.Rows[i].Field<string>("kbmShortDescription");
				eRPServiceContractMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPServiceContractMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPServiceContractMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPServiceContractMemoInformationDto> GetServiceContractMemo(Guid serviceContractMemoId)
	{
		ERPServiceContractMemoInformationDto eRPServiceContractMemoInformationDto = new ERPServiceContractMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "kbmCreatedBy", "kbmCreatedDate", "kbmUniqueID", "kbmLongDescriptionRtf", "kbmLongDescriptionText", "kbmMemoDate", "kbmRowVersion", "kbmServiceContractMemoID", "kbmServiceContractID", "kbmShortDescription" };
		base.selectList.AddRange(collection);
		base.filterList.Add("kbmUniqueID|C", serviceContractMemoId);
		AddCustomFieldsToSelectList("ServiceContractMemos");
		using (DataTable dataTable = GetAsDataTable("ServiceContractMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPServiceContractMemoInformationDto);
			}
			eRPServiceContractMemoInformationDto.kbmCreatedBy = dataTable.Rows[0].Field<string>("kbmCreatedBy");
			eRPServiceContractMemoInformationDto.kbmCreatedDate = dataTable.Rows[0].Field<DateTime?>("kbmCreatedDate");
			eRPServiceContractMemoInformationDto.kbmUniqueID = dataTable.Rows[0].Field<Guid>("kbmUniqueID");
			eRPServiceContractMemoInformationDto.kbmLongDescriptionRtf = dataTable.Rows[0].Field<string>("kbmLongDescriptionRtf");
			eRPServiceContractMemoInformationDto.kbmLongDescriptionText = dataTable.Rows[0].Field<string>("kbmLongDescriptionText");
			eRPServiceContractMemoInformationDto.kbmMemoDate = dataTable.Rows[0].Field<DateTime?>("kbmMemoDate");
			eRPServiceContractMemoInformationDto.kbmRowVersion = dataTable.Rows[0].Field<byte[]>("kbmRowVersion");
			eRPServiceContractMemoInformationDto.kbmServiceContractMemoID = dataTable.Rows[0].Field<short>("kbmServiceContractMemoID");
			eRPServiceContractMemoInformationDto.kbmServiceContractID = dataTable.Rows[0].Field<string>("kbmServiceContractID");
			eRPServiceContractMemoInformationDto.kbmShortDescription = dataTable.Rows[0].Field<string>("kbmShortDescription");
			eRPServiceContractMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPServiceContractMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPServiceContractMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SaveServiceContractMemo(ERPServiceContractMemoDto serviceContractMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ServiceContractMemos WHERE kbmUniqueID = " + M1Util.ConvertToLinq(serviceContractMemo.kbmUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["kbmServiceContractID"] = serviceContractMemo.kbmServiceContractID.ToUpper();
				dataRow["kbmServiceContractMemoID"] = serviceContractMemo.kbmServiceContractMemoID;
				serviceContractMemo.kbmUniqueID = ((serviceContractMemo.kbmUniqueID == Guid.Empty) ? Guid.NewGuid() : serviceContractMemo.kbmUniqueID);
				dataRow["kbmUniqueID"] = serviceContractMemo.kbmUniqueID;
				dataRow["kbmCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["kbmCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ServiceContractMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (serviceContractMemo.kbmRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ServiceContractMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["kbmRowVersion"], serviceContractMemo.kbmRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ServiceContractMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ServiceContractMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["kbmLongDescriptionRtf"] = serviceContractMemo.kbmLongDescriptionRtf ?? dataRow["kbmLongDescriptionRtf"];
			dataRow["kbmLongDescriptionText"] = serviceContractMemo.kbmLongDescriptionText ?? dataRow["kbmLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? kbmMemoDate = serviceContractMemo.kbmMemoDate;
			dataRow2["kbmMemoDate"] = (kbmMemoDate.HasValue ? ((object)kbmMemoDate.GetValueOrDefault()) : dataRow["kbmMemoDate"]);
			dataRow["kbmShortDescription"] = serviceContractMemo.kbmShortDescription;
			if (serviceContractMemo.CustomFields != null && serviceContractMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in serviceContractMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ServiceContractMemo [{serviceContractMemo.kbmUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ServiceContractMemo [{serviceContractMemo.kbmUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
