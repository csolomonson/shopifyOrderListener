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

public class ERPServiceContractOwnerRepository : APIBaseRepository, IERPServiceContractOwnerRepository, IAPIBaseRepository, IDisposable
{
	public ERPServiceContractOwnerRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesServiceContractOwnerExist(Guid serviceContractOwnerId)
	{
		InitializeParameterLists();
		base.filterList.Add("kboUniqueID|C", serviceContractOwnerId);
		base.selectList.Add("kboUniqueID");
		return Task.FromResult(GetAsObject("ServiceContractOwners", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPServiceContractOwnerInformationDto>> GetAllServiceContractOwners(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPServiceContractOwnerInformationDto> collection = new List<ERPServiceContractOwnerInformationDto>();
		InitializeParameterLists();
		string[] array = new string[37]
		{
			"kboAddressLine1", "kboAddressLine2", "kboAddressLine3", "kboCity", "kboCountry", "kboCreatedBy", "kboCreatedDate", "kboDeliveryDate", "kboEmailAddress", "kboUniqueID",
			"kboFaxNumber", "kboFirstName", "kboHomePhoneNumber", "kboCurrentOwner", "kboSameAsAbove", "kboTermsAccepted", "kboLastName", "kboMiddleName", "kboMobileNumber", "kboOrganizationID",
			"kboPhysicalAddressLine1", "kboPhysicalAddressLine2", "kboPhysicalAddressLine3", "kboPhysicalCity", "kboPhysicalCountry", "kboPhysicalLocationCity", "kboPhysicalLocationState", "kboPhysicalPostCode", "kboPhysicalState", "kboPostCode",
			"kboRegisteredDate", "kboRowVersion", "kboServiceContractOwnerID", "kboServiceContractID", "kboStartDate", "kboState", "kboWorkPhoneNumber"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ServiceContractOwners");
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
		using (DataTable dataTable = GetAsDataTable("ServiceContractOwners", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPServiceContractOwnerInformationDto eRPServiceContractOwnerInformationDto = new ERPServiceContractOwnerInformationDto();
				eRPServiceContractOwnerInformationDto.kboAddressLine1 = dataTable.Rows[i].Field<string>("kboAddressLine1");
				eRPServiceContractOwnerInformationDto.kboAddressLine2 = dataTable.Rows[i].Field<string>("kboAddressLine2");
				eRPServiceContractOwnerInformationDto.kboAddressLine3 = dataTable.Rows[i].Field<string>("kboAddressLine3");
				eRPServiceContractOwnerInformationDto.kboCity = dataTable.Rows[i].Field<string>("kboCity");
				eRPServiceContractOwnerInformationDto.kboCountry = dataTable.Rows[i].Field<string>("kboCountry");
				eRPServiceContractOwnerInformationDto.kboCreatedBy = dataTable.Rows[i].Field<string>("kboCreatedBy");
				eRPServiceContractOwnerInformationDto.kboCreatedDate = dataTable.Rows[i].Field<DateTime?>("kboCreatedDate");
				eRPServiceContractOwnerInformationDto.kboDeliveryDate = dataTable.Rows[i].Field<DateTime?>("kboDeliveryDate");
				eRPServiceContractOwnerInformationDto.kboEmailAddress = dataTable.Rows[i].Field<string>("kboEmailAddress");
				eRPServiceContractOwnerInformationDto.kboUniqueID = dataTable.Rows[i].Field<Guid>("kboUniqueID");
				eRPServiceContractOwnerInformationDto.kboFaxNumber = dataTable.Rows[i].Field<string>("kboFaxNumber");
				eRPServiceContractOwnerInformationDto.kboFirstName = dataTable.Rows[i].Field<string>("kboFirstName");
				eRPServiceContractOwnerInformationDto.kboHomePhoneNumber = dataTable.Rows[i].Field<string>("kboHomePhoneNumber");
				eRPServiceContractOwnerInformationDto.kboCurrentOwner = dataTable.Rows[i].Field<bool>("kboCurrentOwner");
				eRPServiceContractOwnerInformationDto.kboSameAsAbove = dataTable.Rows[i].Field<bool>("kboSameAsAbove");
				eRPServiceContractOwnerInformationDto.kboTermsAccepted = dataTable.Rows[i].Field<bool>("kboTermsAccepted");
				eRPServiceContractOwnerInformationDto.kboLastName = dataTable.Rows[i].Field<string>("kboLastName");
				eRPServiceContractOwnerInformationDto.kboMiddleName = dataTable.Rows[i].Field<string>("kboMiddleName");
				eRPServiceContractOwnerInformationDto.kboMobileNumber = dataTable.Rows[i].Field<string>("kboMobileNumber");
				eRPServiceContractOwnerInformationDto.kboOrganizationID = dataTable.Rows[i].Field<string>("kboOrganizationID");
				eRPServiceContractOwnerInformationDto.kboPhysicalAddressLine1 = dataTable.Rows[i].Field<string>("kboPhysicalAddressLine1");
				eRPServiceContractOwnerInformationDto.kboPhysicalAddressLine2 = dataTable.Rows[i].Field<string>("kboPhysicalAddressLine2");
				eRPServiceContractOwnerInformationDto.kboPhysicalAddressLine3 = dataTable.Rows[i].Field<string>("kboPhysicalAddressLine3");
				eRPServiceContractOwnerInformationDto.kboPhysicalCity = dataTable.Rows[i].Field<string>("kboPhysicalCity");
				eRPServiceContractOwnerInformationDto.kboPhysicalCountry = dataTable.Rows[i].Field<string>("kboPhysicalCountry");
				eRPServiceContractOwnerInformationDto.kboPhysicalLocationCity = dataTable.Rows[i].Field<string>("kboPhysicalLocationCity");
				eRPServiceContractOwnerInformationDto.kboPhysicalLocationState = dataTable.Rows[i].Field<string>("kboPhysicalLocationState");
				eRPServiceContractOwnerInformationDto.kboPhysicalPostCode = dataTable.Rows[i].Field<string>("kboPhysicalPostCode");
				eRPServiceContractOwnerInformationDto.kboPhysicalState = dataTable.Rows[i].Field<string>("kboPhysicalState");
				eRPServiceContractOwnerInformationDto.kboPostCode = dataTable.Rows[i].Field<string>("kboPostCode");
				eRPServiceContractOwnerInformationDto.kboRegisteredDate = dataTable.Rows[i].Field<DateTime?>("kboRegisteredDate");
				eRPServiceContractOwnerInformationDto.kboRowVersion = dataTable.Rows[i].Field<byte[]>("kboRowVersion");
				eRPServiceContractOwnerInformationDto.kboServiceContractOwnerID = dataTable.Rows[i].Field<short>("kboServiceContractOwnerID");
				eRPServiceContractOwnerInformationDto.kboServiceContractID = dataTable.Rows[i].Field<string>("kboServiceContractID");
				eRPServiceContractOwnerInformationDto.kboStartDate = dataTable.Rows[i].Field<DateTime?>("kboStartDate");
				eRPServiceContractOwnerInformationDto.kboState = dataTable.Rows[i].Field<string>("kboState");
				eRPServiceContractOwnerInformationDto.kboWorkPhoneNumber = dataTable.Rows[i].Field<string>("kboWorkPhoneNumber");
				eRPServiceContractOwnerInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPServiceContractOwnerInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPServiceContractOwnerInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPServiceContractOwnerInformationDto> GetServiceContractOwner(Guid serviceContractOwnerId)
	{
		ERPServiceContractOwnerInformationDto eRPServiceContractOwnerInformationDto = new ERPServiceContractOwnerInformationDto();
		InitializeParameterLists();
		string[] collection = new string[37]
		{
			"kboAddressLine1", "kboAddressLine2", "kboAddressLine3", "kboCity", "kboCountry", "kboCreatedBy", "kboCreatedDate", "kboDeliveryDate", "kboEmailAddress", "kboUniqueID",
			"kboFaxNumber", "kboFirstName", "kboHomePhoneNumber", "kboCurrentOwner", "kboSameAsAbove", "kboTermsAccepted", "kboLastName", "kboMiddleName", "kboMobileNumber", "kboOrganizationID",
			"kboPhysicalAddressLine1", "kboPhysicalAddressLine2", "kboPhysicalAddressLine3", "kboPhysicalCity", "kboPhysicalCountry", "kboPhysicalLocationCity", "kboPhysicalLocationState", "kboPhysicalPostCode", "kboPhysicalState", "kboPostCode",
			"kboRegisteredDate", "kboRowVersion", "kboServiceContractOwnerID", "kboServiceContractID", "kboStartDate", "kboState", "kboWorkPhoneNumber"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("kboUniqueID|C", serviceContractOwnerId);
		AddCustomFieldsToSelectList("ServiceContractOwners");
		using (DataTable dataTable = GetAsDataTable("ServiceContractOwners", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPServiceContractOwnerInformationDto);
			}
			eRPServiceContractOwnerInformationDto.kboAddressLine1 = dataTable.Rows[0].Field<string>("kboAddressLine1");
			eRPServiceContractOwnerInformationDto.kboAddressLine2 = dataTable.Rows[0].Field<string>("kboAddressLine2");
			eRPServiceContractOwnerInformationDto.kboAddressLine3 = dataTable.Rows[0].Field<string>("kboAddressLine3");
			eRPServiceContractOwnerInformationDto.kboCity = dataTable.Rows[0].Field<string>("kboCity");
			eRPServiceContractOwnerInformationDto.kboCountry = dataTable.Rows[0].Field<string>("kboCountry");
			eRPServiceContractOwnerInformationDto.kboCreatedBy = dataTable.Rows[0].Field<string>("kboCreatedBy");
			eRPServiceContractOwnerInformationDto.kboCreatedDate = dataTable.Rows[0].Field<DateTime?>("kboCreatedDate");
			eRPServiceContractOwnerInformationDto.kboDeliveryDate = dataTable.Rows[0].Field<DateTime?>("kboDeliveryDate");
			eRPServiceContractOwnerInformationDto.kboEmailAddress = dataTable.Rows[0].Field<string>("kboEmailAddress");
			eRPServiceContractOwnerInformationDto.kboUniqueID = dataTable.Rows[0].Field<Guid>("kboUniqueID");
			eRPServiceContractOwnerInformationDto.kboFaxNumber = dataTable.Rows[0].Field<string>("kboFaxNumber");
			eRPServiceContractOwnerInformationDto.kboFirstName = dataTable.Rows[0].Field<string>("kboFirstName");
			eRPServiceContractOwnerInformationDto.kboHomePhoneNumber = dataTable.Rows[0].Field<string>("kboHomePhoneNumber");
			eRPServiceContractOwnerInformationDto.kboCurrentOwner = dataTable.Rows[0].Field<bool>("kboCurrentOwner");
			eRPServiceContractOwnerInformationDto.kboSameAsAbove = dataTable.Rows[0].Field<bool>("kboSameAsAbove");
			eRPServiceContractOwnerInformationDto.kboTermsAccepted = dataTable.Rows[0].Field<bool>("kboTermsAccepted");
			eRPServiceContractOwnerInformationDto.kboLastName = dataTable.Rows[0].Field<string>("kboLastName");
			eRPServiceContractOwnerInformationDto.kboMiddleName = dataTable.Rows[0].Field<string>("kboMiddleName");
			eRPServiceContractOwnerInformationDto.kboMobileNumber = dataTable.Rows[0].Field<string>("kboMobileNumber");
			eRPServiceContractOwnerInformationDto.kboOrganizationID = dataTable.Rows[0].Field<string>("kboOrganizationID");
			eRPServiceContractOwnerInformationDto.kboPhysicalAddressLine1 = dataTable.Rows[0].Field<string>("kboPhysicalAddressLine1");
			eRPServiceContractOwnerInformationDto.kboPhysicalAddressLine2 = dataTable.Rows[0].Field<string>("kboPhysicalAddressLine2");
			eRPServiceContractOwnerInformationDto.kboPhysicalAddressLine3 = dataTable.Rows[0].Field<string>("kboPhysicalAddressLine3");
			eRPServiceContractOwnerInformationDto.kboPhysicalCity = dataTable.Rows[0].Field<string>("kboPhysicalCity");
			eRPServiceContractOwnerInformationDto.kboPhysicalCountry = dataTable.Rows[0].Field<string>("kboPhysicalCountry");
			eRPServiceContractOwnerInformationDto.kboPhysicalLocationCity = dataTable.Rows[0].Field<string>("kboPhysicalLocationCity");
			eRPServiceContractOwnerInformationDto.kboPhysicalLocationState = dataTable.Rows[0].Field<string>("kboPhysicalLocationState");
			eRPServiceContractOwnerInformationDto.kboPhysicalPostCode = dataTable.Rows[0].Field<string>("kboPhysicalPostCode");
			eRPServiceContractOwnerInformationDto.kboPhysicalState = dataTable.Rows[0].Field<string>("kboPhysicalState");
			eRPServiceContractOwnerInformationDto.kboPostCode = dataTable.Rows[0].Field<string>("kboPostCode");
			eRPServiceContractOwnerInformationDto.kboRegisteredDate = dataTable.Rows[0].Field<DateTime?>("kboRegisteredDate");
			eRPServiceContractOwnerInformationDto.kboRowVersion = dataTable.Rows[0].Field<byte[]>("kboRowVersion");
			eRPServiceContractOwnerInformationDto.kboServiceContractOwnerID = dataTable.Rows[0].Field<short>("kboServiceContractOwnerID");
			eRPServiceContractOwnerInformationDto.kboServiceContractID = dataTable.Rows[0].Field<string>("kboServiceContractID");
			eRPServiceContractOwnerInformationDto.kboStartDate = dataTable.Rows[0].Field<DateTime?>("kboStartDate");
			eRPServiceContractOwnerInformationDto.kboState = dataTable.Rows[0].Field<string>("kboState");
			eRPServiceContractOwnerInformationDto.kboWorkPhoneNumber = dataTable.Rows[0].Field<string>("kboWorkPhoneNumber");
			eRPServiceContractOwnerInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPServiceContractOwnerInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPServiceContractOwnerInformationDto);
	}

	public Task<APIValidationInfoDto> SaveServiceContractOwner(ERPServiceContractOwnerDto serviceContractOwner)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ServiceContractOwners WHERE kboUniqueID = " + M1Util.ConvertToLinq(serviceContractOwner.kboUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["kboServiceContractID"] = serviceContractOwner.kboServiceContractID.ToUpper();
				dataRow["kboServiceContractOwnerID"] = serviceContractOwner.kboServiceContractOwnerID;
				serviceContractOwner.kboUniqueID = ((serviceContractOwner.kboUniqueID == Guid.Empty) ? Guid.NewGuid() : serviceContractOwner.kboUniqueID);
				dataRow["kboUniqueID"] = serviceContractOwner.kboUniqueID;
				dataRow["kboCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["kboCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ServiceContractOwner could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (serviceContractOwner.kboRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ServiceContractOwner is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["kboRowVersion"], serviceContractOwner.kboRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ServiceContractOwner has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ServiceContractOwner again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["kboAddressLine1"] = serviceContractOwner.kboAddressLine1;
			dataRow["kboAddressLine2"] = serviceContractOwner.kboAddressLine2;
			dataRow["kboAddressLine3"] = serviceContractOwner.kboAddressLine3;
			dataRow["kboCity"] = serviceContractOwner.kboCity;
			dataRow["kboCountry"] = serviceContractOwner.kboCountry;
			DataRow dataRow2 = dataRow;
			DateTime? kboDeliveryDate = serviceContractOwner.kboDeliveryDate;
			dataRow2["kboDeliveryDate"] = (kboDeliveryDate.HasValue ? ((object)kboDeliveryDate.GetValueOrDefault()) : dataRow["kboDeliveryDate"]);
			dataRow["kboEmailAddress"] = serviceContractOwner.kboEmailAddress ?? dataRow["kboEmailAddress"];
			dataRow["kboFaxNumber"] = serviceContractOwner.kboFaxNumber;
			dataRow["kboFirstName"] = serviceContractOwner.kboFirstName;
			dataRow["kboHomePhoneNumber"] = serviceContractOwner.kboHomePhoneNumber;
			dataRow["kboCurrentOwner"] = serviceContractOwner.kboCurrentOwner;
			dataRow["kboSameAsAbove"] = serviceContractOwner.kboSameAsAbove;
			dataRow["kboTermsAccepted"] = serviceContractOwner.kboTermsAccepted;
			dataRow["kboLastName"] = serviceContractOwner.kboLastName;
			dataRow["kboMiddleName"] = serviceContractOwner.kboMiddleName;
			dataRow["kboMobileNumber"] = serviceContractOwner.kboMobileNumber;
			dataRow["kboOrganizationID"] = serviceContractOwner.kboOrganizationID;
			dataRow["kboPhysicalAddressLine1"] = serviceContractOwner.kboPhysicalAddressLine1;
			dataRow["kboPhysicalAddressLine2"] = serviceContractOwner.kboPhysicalAddressLine2;
			dataRow["kboPhysicalAddressLine3"] = serviceContractOwner.kboPhysicalAddressLine3;
			dataRow["kboPhysicalCity"] = serviceContractOwner.kboPhysicalCity;
			dataRow["kboPhysicalCountry"] = serviceContractOwner.kboPhysicalCountry;
			dataRow["kboPhysicalLocationCity"] = serviceContractOwner.kboPhysicalLocationCity;
			dataRow["kboPhysicalLocationState"] = serviceContractOwner.kboPhysicalLocationState;
			dataRow["kboPhysicalPostCode"] = serviceContractOwner.kboPhysicalPostCode;
			dataRow["kboPhysicalState"] = serviceContractOwner.kboPhysicalState;
			dataRow["kboPostCode"] = serviceContractOwner.kboPostCode;
			DataRow dataRow3 = dataRow;
			kboDeliveryDate = serviceContractOwner.kboRegisteredDate;
			dataRow3["kboRegisteredDate"] = (kboDeliveryDate.HasValue ? ((object)kboDeliveryDate.GetValueOrDefault()) : dataRow["kboRegisteredDate"]);
			DataRow dataRow4 = dataRow;
			kboDeliveryDate = serviceContractOwner.kboStartDate;
			dataRow4["kboStartDate"] = (kboDeliveryDate.HasValue ? ((object)kboDeliveryDate.GetValueOrDefault()) : dataRow["kboStartDate"]);
			dataRow["kboState"] = serviceContractOwner.kboState;
			dataRow["kboWorkPhoneNumber"] = serviceContractOwner.kboWorkPhoneNumber;
			if (serviceContractOwner.CustomFields != null && serviceContractOwner.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in serviceContractOwner.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ServiceContractOwner [{serviceContractOwner.kboUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ServiceContractOwner [{serviceContractOwner.kboUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
