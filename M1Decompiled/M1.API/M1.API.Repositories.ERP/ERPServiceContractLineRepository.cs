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

public class ERPServiceContractLineRepository : APIBaseRepository, IERPServiceContractLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPServiceContractLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesServiceContractLineExist(Guid serviceContractLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("kbnUniqueID|C", serviceContractLineId);
		base.selectList.Add("kbnUniqueID");
		return Task.FromResult(GetAsObject("ServiceContractLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPServiceContractLineInformationDto>> GetAllServiceContractLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPServiceContractLineInformationDto> collection = new List<ERPServiceContractLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[14]
		{
			"kbnContractLength", "kbnContractLengthType", "kbnCreatedBy", "kbnCreatedDate", "kbnEndDate", "kbnUniqueID", "kbnPartID", "kbnPartRevisionID", "kbnPartShortDescription", "kbnRowVersion",
			"kbnServiceContractLineID", "kbnSerialNumberID", "kbnServiceContractID", "kbnStartDate"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ServiceContractLines");
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
		using (DataTable dataTable = GetAsDataTable("ServiceContractLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPServiceContractLineInformationDto eRPServiceContractLineInformationDto = new ERPServiceContractLineInformationDto();
				eRPServiceContractLineInformationDto.kbnContractLength = dataTable.Rows[i].Field<short>("kbnContractLength");
				eRPServiceContractLineInformationDto.kbnContractLengthType = dataTable.Rows[i].Field<string>("kbnContractLengthType");
				eRPServiceContractLineInformationDto.kbnCreatedBy = dataTable.Rows[i].Field<string>("kbnCreatedBy");
				eRPServiceContractLineInformationDto.kbnCreatedDate = dataTable.Rows[i].Field<DateTime?>("kbnCreatedDate");
				eRPServiceContractLineInformationDto.kbnEndDate = dataTable.Rows[i].Field<DateTime?>("kbnEndDate");
				eRPServiceContractLineInformationDto.kbnUniqueID = dataTable.Rows[i].Field<Guid>("kbnUniqueID");
				eRPServiceContractLineInformationDto.kbnPartID = dataTable.Rows[i].Field<string>("kbnPartID");
				eRPServiceContractLineInformationDto.kbnPartRevisionID = dataTable.Rows[i].Field<string>("kbnPartRevisionID");
				eRPServiceContractLineInformationDto.kbnPartShortDescription = dataTable.Rows[i].Field<string>("kbnPartShortDescription");
				eRPServiceContractLineInformationDto.kbnRowVersion = dataTable.Rows[i].Field<byte[]>("kbnRowVersion");
				eRPServiceContractLineInformationDto.kbnServiceContractLineID = dataTable.Rows[i].Field<short>("kbnServiceContractLineID");
				eRPServiceContractLineInformationDto.kbnSerialNumberID = dataTable.Rows[i].Field<string>("kbnSerialNumberID");
				eRPServiceContractLineInformationDto.kbnServiceContractID = dataTable.Rows[i].Field<string>("kbnServiceContractID");
				eRPServiceContractLineInformationDto.kbnStartDate = dataTable.Rows[i].Field<DateTime?>("kbnStartDate");
				eRPServiceContractLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPServiceContractLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPServiceContractLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPServiceContractLineInformationDto> GetServiceContractLine(Guid serviceContractLineId)
	{
		ERPServiceContractLineInformationDto eRPServiceContractLineInformationDto = new ERPServiceContractLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[14]
		{
			"kbnContractLength", "kbnContractLengthType", "kbnCreatedBy", "kbnCreatedDate", "kbnEndDate", "kbnUniqueID", "kbnPartID", "kbnPartRevisionID", "kbnPartShortDescription", "kbnRowVersion",
			"kbnServiceContractLineID", "kbnSerialNumberID", "kbnServiceContractID", "kbnStartDate"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("kbnUniqueID|C", serviceContractLineId);
		AddCustomFieldsToSelectList("ServiceContractLines");
		using (DataTable dataTable = GetAsDataTable("ServiceContractLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPServiceContractLineInformationDto);
			}
			eRPServiceContractLineInformationDto.kbnContractLength = dataTable.Rows[0].Field<short>("kbnContractLength");
			eRPServiceContractLineInformationDto.kbnContractLengthType = dataTable.Rows[0].Field<string>("kbnContractLengthType");
			eRPServiceContractLineInformationDto.kbnCreatedBy = dataTable.Rows[0].Field<string>("kbnCreatedBy");
			eRPServiceContractLineInformationDto.kbnCreatedDate = dataTable.Rows[0].Field<DateTime?>("kbnCreatedDate");
			eRPServiceContractLineInformationDto.kbnEndDate = dataTable.Rows[0].Field<DateTime?>("kbnEndDate");
			eRPServiceContractLineInformationDto.kbnUniqueID = dataTable.Rows[0].Field<Guid>("kbnUniqueID");
			eRPServiceContractLineInformationDto.kbnPartID = dataTable.Rows[0].Field<string>("kbnPartID");
			eRPServiceContractLineInformationDto.kbnPartRevisionID = dataTable.Rows[0].Field<string>("kbnPartRevisionID");
			eRPServiceContractLineInformationDto.kbnPartShortDescription = dataTable.Rows[0].Field<string>("kbnPartShortDescription");
			eRPServiceContractLineInformationDto.kbnRowVersion = dataTable.Rows[0].Field<byte[]>("kbnRowVersion");
			eRPServiceContractLineInformationDto.kbnServiceContractLineID = dataTable.Rows[0].Field<short>("kbnServiceContractLineID");
			eRPServiceContractLineInformationDto.kbnSerialNumberID = dataTable.Rows[0].Field<string>("kbnSerialNumberID");
			eRPServiceContractLineInformationDto.kbnServiceContractID = dataTable.Rows[0].Field<string>("kbnServiceContractID");
			eRPServiceContractLineInformationDto.kbnStartDate = dataTable.Rows[0].Field<DateTime?>("kbnStartDate");
			eRPServiceContractLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPServiceContractLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPServiceContractLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveServiceContractLine(ERPServiceContractLineDto serviceContractLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ServiceContractLines WHERE kbnUniqueID = " + M1Util.ConvertToLinq(serviceContractLine.kbnUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["kbnServiceContractID"] = serviceContractLine.kbnServiceContractID.ToUpper();
				dataRow["kbnServiceContractLineID"] = serviceContractLine.kbnServiceContractLineID;
				serviceContractLine.kbnUniqueID = ((serviceContractLine.kbnUniqueID == Guid.Empty) ? Guid.NewGuid() : serviceContractLine.kbnUniqueID);
				dataRow["kbnUniqueID"] = serviceContractLine.kbnUniqueID;
				dataRow["kbnCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["kbnCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ServiceContractLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (serviceContractLine.kbnRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ServiceContractLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["kbnRowVersion"], serviceContractLine.kbnRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ServiceContractLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ServiceContractLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["kbnContractLength"] = serviceContractLine.kbnContractLength;
			dataRow["kbnContractLengthType"] = serviceContractLine.kbnContractLengthType;
			DataRow dataRow2 = dataRow;
			DateTime? kbnEndDate = serviceContractLine.kbnEndDate;
			dataRow2["kbnEndDate"] = (kbnEndDate.HasValue ? ((object)kbnEndDate.GetValueOrDefault()) : dataRow["kbnEndDate"]);
			dataRow["kbnPartID"] = serviceContractLine.kbnPartID;
			dataRow["kbnPartRevisionID"] = serviceContractLine.kbnPartRevisionID;
			dataRow["kbnPartShortDescription"] = serviceContractLine.kbnPartShortDescription;
			dataRow["kbnSerialNumberID"] = serviceContractLine.kbnSerialNumberID;
			DataRow dataRow3 = dataRow;
			kbnEndDate = serviceContractLine.kbnStartDate;
			dataRow3["kbnStartDate"] = (kbnEndDate.HasValue ? ((object)kbnEndDate.GetValueOrDefault()) : dataRow["kbnStartDate"]);
			if (serviceContractLine.CustomFields != null && serviceContractLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in serviceContractLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ServiceContractLine [{serviceContractLine.kbnUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ServiceContractLine [{serviceContractLine.kbnUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
