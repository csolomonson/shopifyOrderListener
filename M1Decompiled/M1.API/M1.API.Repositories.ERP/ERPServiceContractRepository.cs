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

public class ERPServiceContractRepository : APIBaseRepository, IERPServiceContractRepository, IAPIBaseRepository, IDisposable
{
	public ERPServiceContractRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesServiceContractExist(Guid serviceContractId)
	{
		InitializeParameterLists();
		base.filterList.Add("kbsUniqueID|C", serviceContractId);
		base.selectList.Add("kbsUniqueID");
		return Task.FromResult(GetAsObject("ServiceContracts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPServiceContractInformationDto>> GetAllServiceContracts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPServiceContractInformationDto> collection = new List<ERPServiceContractInformationDto>();
		InitializeParameterLists();
		string[] array = new string[22]
		{
			"kbsServiceContractID", "kbsContractAmount", "kbsContractLength", "kbsContractLengthType", "kbsCreatedBy", "kbsCreatedDate", "kbsDescription", "kbsEndDate", "kbsUniqueID", "kbsLongDescriptionRtf",
			"kbsLongDescriptionText", "kbsOrganizationID", "kbsPartID", "kbsPartRevisionID", "kbsPartShortDescription", "kbsProjectAreaID", "kbsProjectID", "kbsResellerOrganizationID", "kbsRowVersion", "kbsSerialNumberID",
			"kbsServiceContractTypeID", "kbsStartDate"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ServiceContracts");
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
		using (DataTable dataTable = GetAsDataTable("ServiceContracts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPServiceContractInformationDto eRPServiceContractInformationDto = new ERPServiceContractInformationDto();
				eRPServiceContractInformationDto.kbsServiceContractID = dataTable.Rows[i].Field<string>("kbsServiceContractID");
				eRPServiceContractInformationDto.kbsContractAmount = dataTable.Rows[i].Field<decimal>("kbsContractAmount");
				eRPServiceContractInformationDto.kbsContractLength = dataTable.Rows[i].Field<short>("kbsContractLength");
				eRPServiceContractInformationDto.kbsContractLengthType = dataTable.Rows[i].Field<string>("kbsContractLengthType");
				eRPServiceContractInformationDto.kbsCreatedBy = dataTable.Rows[i].Field<string>("kbsCreatedBy");
				eRPServiceContractInformationDto.kbsCreatedDate = dataTable.Rows[i].Field<DateTime?>("kbsCreatedDate");
				eRPServiceContractInformationDto.kbsDescription = dataTable.Rows[i].Field<string>("kbsDescription");
				eRPServiceContractInformationDto.kbsEndDate = dataTable.Rows[i].Field<DateTime?>("kbsEndDate");
				eRPServiceContractInformationDto.kbsUniqueID = dataTable.Rows[i].Field<Guid>("kbsUniqueID");
				eRPServiceContractInformationDto.kbsLongDescriptionRtf = dataTable.Rows[i].Field<string>("kbsLongDescriptionRtf");
				eRPServiceContractInformationDto.kbsLongDescriptionText = dataTable.Rows[i].Field<string>("kbsLongDescriptionText");
				eRPServiceContractInformationDto.kbsOrganizationID = dataTable.Rows[i].Field<string>("kbsOrganizationID");
				eRPServiceContractInformationDto.kbsPartID = dataTable.Rows[i].Field<string>("kbsPartID");
				eRPServiceContractInformationDto.kbsPartRevisionID = dataTable.Rows[i].Field<string>("kbsPartRevisionID");
				eRPServiceContractInformationDto.kbsPartShortDescription = dataTable.Rows[i].Field<string>("kbsPartShortDescription");
				eRPServiceContractInformationDto.kbsProjectAreaID = dataTable.Rows[i].Field<string>("kbsProjectAreaID");
				eRPServiceContractInformationDto.kbsProjectID = dataTable.Rows[i].Field<string>("kbsProjectID");
				eRPServiceContractInformationDto.kbsResellerOrganizationID = dataTable.Rows[i].Field<string>("kbsResellerOrganizationID");
				eRPServiceContractInformationDto.kbsRowVersion = dataTable.Rows[i].Field<byte[]>("kbsRowVersion");
				eRPServiceContractInformationDto.kbsSerialNumberID = dataTable.Rows[i].Field<string>("kbsSerialNumberID");
				eRPServiceContractInformationDto.kbsServiceContractTypeID = dataTable.Rows[i].Field<string>("kbsServiceContractTypeID");
				eRPServiceContractInformationDto.kbsStartDate = dataTable.Rows[i].Field<DateTime?>("kbsStartDate");
				eRPServiceContractInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPServiceContractInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPServiceContractInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPServiceContractInformationDto> GetServiceContract(Guid serviceContractId)
	{
		ERPServiceContractInformationDto eRPServiceContractInformationDto = new ERPServiceContractInformationDto();
		InitializeParameterLists();
		string[] collection = new string[22]
		{
			"kbsServiceContractID", "kbsContractAmount", "kbsContractLength", "kbsContractLengthType", "kbsCreatedBy", "kbsCreatedDate", "kbsDescription", "kbsEndDate", "kbsUniqueID", "kbsLongDescriptionRtf",
			"kbsLongDescriptionText", "kbsOrganizationID", "kbsPartID", "kbsPartRevisionID", "kbsPartShortDescription", "kbsProjectAreaID", "kbsProjectID", "kbsResellerOrganizationID", "kbsRowVersion", "kbsSerialNumberID",
			"kbsServiceContractTypeID", "kbsStartDate"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("kbsUniqueID|C", serviceContractId);
		AddCustomFieldsToSelectList("ServiceContracts");
		using (DataTable dataTable = GetAsDataTable("ServiceContracts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPServiceContractInformationDto);
			}
			eRPServiceContractInformationDto.kbsServiceContractID = dataTable.Rows[0].Field<string>("kbsServiceContractID");
			eRPServiceContractInformationDto.kbsContractAmount = dataTable.Rows[0].Field<decimal>("kbsContractAmount");
			eRPServiceContractInformationDto.kbsContractLength = dataTable.Rows[0].Field<short>("kbsContractLength");
			eRPServiceContractInformationDto.kbsContractLengthType = dataTable.Rows[0].Field<string>("kbsContractLengthType");
			eRPServiceContractInformationDto.kbsCreatedBy = dataTable.Rows[0].Field<string>("kbsCreatedBy");
			eRPServiceContractInformationDto.kbsCreatedDate = dataTable.Rows[0].Field<DateTime?>("kbsCreatedDate");
			eRPServiceContractInformationDto.kbsDescription = dataTable.Rows[0].Field<string>("kbsDescription");
			eRPServiceContractInformationDto.kbsEndDate = dataTable.Rows[0].Field<DateTime?>("kbsEndDate");
			eRPServiceContractInformationDto.kbsUniqueID = dataTable.Rows[0].Field<Guid>("kbsUniqueID");
			eRPServiceContractInformationDto.kbsLongDescriptionRtf = dataTable.Rows[0].Field<string>("kbsLongDescriptionRtf");
			eRPServiceContractInformationDto.kbsLongDescriptionText = dataTable.Rows[0].Field<string>("kbsLongDescriptionText");
			eRPServiceContractInformationDto.kbsOrganizationID = dataTable.Rows[0].Field<string>("kbsOrganizationID");
			eRPServiceContractInformationDto.kbsPartID = dataTable.Rows[0].Field<string>("kbsPartID");
			eRPServiceContractInformationDto.kbsPartRevisionID = dataTable.Rows[0].Field<string>("kbsPartRevisionID");
			eRPServiceContractInformationDto.kbsPartShortDescription = dataTable.Rows[0].Field<string>("kbsPartShortDescription");
			eRPServiceContractInformationDto.kbsProjectAreaID = dataTable.Rows[0].Field<string>("kbsProjectAreaID");
			eRPServiceContractInformationDto.kbsProjectID = dataTable.Rows[0].Field<string>("kbsProjectID");
			eRPServiceContractInformationDto.kbsResellerOrganizationID = dataTable.Rows[0].Field<string>("kbsResellerOrganizationID");
			eRPServiceContractInformationDto.kbsRowVersion = dataTable.Rows[0].Field<byte[]>("kbsRowVersion");
			eRPServiceContractInformationDto.kbsSerialNumberID = dataTable.Rows[0].Field<string>("kbsSerialNumberID");
			eRPServiceContractInformationDto.kbsServiceContractTypeID = dataTable.Rows[0].Field<string>("kbsServiceContractTypeID");
			eRPServiceContractInformationDto.kbsStartDate = dataTable.Rows[0].Field<DateTime?>("kbsStartDate");
			eRPServiceContractInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPServiceContractInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPServiceContractInformationDto);
	}

	public Task<APIValidationInfoDto> SaveServiceContract(ERPServiceContractDto serviceContract)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ServiceContracts WHERE kbsUniqueID = " + M1Util.ConvertToLinq(serviceContract.kbsUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["kbsServiceContractID"] = serviceContract.kbsServiceContractID.ToUpper();
				serviceContract.kbsUniqueID = ((serviceContract.kbsUniqueID == Guid.Empty) ? Guid.NewGuid() : serviceContract.kbsUniqueID);
				dataRow["kbsUniqueID"] = serviceContract.kbsUniqueID;
				dataRow["kbsCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["kbsCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ServiceContract could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (serviceContract.kbsRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ServiceContract is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["kbsRowVersion"], serviceContract.kbsRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ServiceContract has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ServiceContract again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["kbsContractAmount"] = serviceContract.kbsContractAmount;
			dataRow["kbsContractLength"] = serviceContract.kbsContractLength;
			dataRow["kbsContractLengthType"] = serviceContract.kbsContractLengthType;
			dataRow["kbsDescription"] = serviceContract.kbsDescription;
			DataRow dataRow2 = dataRow;
			DateTime? kbsEndDate = serviceContract.kbsEndDate;
			dataRow2["kbsEndDate"] = (kbsEndDate.HasValue ? ((object)kbsEndDate.GetValueOrDefault()) : dataRow["kbsEndDate"]);
			dataRow["kbsLongDescriptionRtf"] = serviceContract.kbsLongDescriptionRtf ?? dataRow["kbsLongDescriptionRtf"];
			dataRow["kbsLongDescriptionText"] = serviceContract.kbsLongDescriptionText ?? dataRow["kbsLongDescriptionText"];
			dataRow["kbsOrganizationID"] = serviceContract.kbsOrganizationID;
			dataRow["kbsPartID"] = serviceContract.kbsPartID;
			dataRow["kbsPartRevisionID"] = serviceContract.kbsPartRevisionID;
			dataRow["kbsPartShortDescription"] = serviceContract.kbsPartShortDescription;
			dataRow["kbsProjectAreaID"] = serviceContract.kbsProjectAreaID;
			dataRow["kbsProjectID"] = serviceContract.kbsProjectID;
			dataRow["kbsResellerOrganizationID"] = serviceContract.kbsResellerOrganizationID;
			dataRow["kbsSerialNumberID"] = serviceContract.kbsSerialNumberID;
			dataRow["kbsServiceContractTypeID"] = serviceContract.kbsServiceContractTypeID;
			DataRow dataRow3 = dataRow;
			kbsEndDate = serviceContract.kbsStartDate;
			dataRow3["kbsStartDate"] = (kbsEndDate.HasValue ? ((object)kbsEndDate.GetValueOrDefault()) : dataRow["kbsStartDate"]);
			if (serviceContract.CustomFields != null && serviceContract.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in serviceContract.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ServiceContract [{serviceContract.kbsUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ServiceContract [{serviceContract.kbsUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
