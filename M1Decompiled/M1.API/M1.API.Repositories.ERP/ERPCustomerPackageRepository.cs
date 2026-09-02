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

public class ERPCustomerPackageRepository : APIBaseRepository, IERPCustomerPackageRepository, IAPIBaseRepository, IDisposable
{
	public ERPCustomerPackageRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesCustomerPackageExist(Guid customerPackageId)
	{
		InitializeParameterLists();
		base.filterList.Add("cpaUniqueID|C", customerPackageId);
		base.selectList.Add("cpaUniqueID");
		return Task.FromResult(GetAsObject("CustomerPackages", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPCustomerPackageInformationDto>> GetAllCustomerPackages(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPCustomerPackageInformationDto> collection = new List<ERPCustomerPackageInformationDto>();
		InitializeParameterLists();
		string[] array = new string[12]
		{
			"cpaCustomerPackageID", "cpaCreatedBy", "cpaCreatedDate", "cpaUniqueID", "cpaInactiveDate", "cpaInactive", "cpaPackageDescription", "cpaPackageDimensionsUom", "cpaPackageHeight", "cpaPackageLength",
			"cpaPackageWidth", "cpaRowVersion"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("CustomerPackages");
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
		using (DataTable dataTable = GetAsDataTable("CustomerPackages", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPCustomerPackageInformationDto eRPCustomerPackageInformationDto = new ERPCustomerPackageInformationDto();
				eRPCustomerPackageInformationDto.cpaCustomerPackageID = dataTable.Rows[i].Field<string>("cpaCustomerPackageID");
				eRPCustomerPackageInformationDto.cpaCreatedBy = dataTable.Rows[i].Field<string>("cpaCreatedBy");
				eRPCustomerPackageInformationDto.cpaCreatedDate = dataTable.Rows[i].Field<DateTime?>("cpaCreatedDate");
				eRPCustomerPackageInformationDto.cpaUniqueID = dataTable.Rows[i].Field<Guid>("cpaUniqueID");
				eRPCustomerPackageInformationDto.cpaInactiveDate = dataTable.Rows[i].Field<DateTime?>("cpaInactiveDate");
				eRPCustomerPackageInformationDto.cpaInactive = dataTable.Rows[i].Field<bool>("cpaInactive");
				eRPCustomerPackageInformationDto.cpaPackageDescription = dataTable.Rows[i].Field<string>("cpaPackageDescription");
				eRPCustomerPackageInformationDto.cpaPackageDimensionsUom = dataTable.Rows[i].Field<string>("cpaPackageDimensionsUom");
				eRPCustomerPackageInformationDto.cpaPackageHeight = dataTable.Rows[i].Field<int>("cpaPackageHeight");
				eRPCustomerPackageInformationDto.cpaPackageLength = dataTable.Rows[i].Field<int>("cpaPackageLength");
				eRPCustomerPackageInformationDto.cpaPackageWidth = dataTable.Rows[i].Field<int>("cpaPackageWidth");
				eRPCustomerPackageInformationDto.cpaRowVersion = dataTable.Rows[i].Field<byte[]>("cpaRowVersion");
				eRPCustomerPackageInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPCustomerPackageInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPCustomerPackageInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPCustomerPackageInformationDto> GetCustomerPackage(Guid customerPackageId)
	{
		ERPCustomerPackageInformationDto eRPCustomerPackageInformationDto = new ERPCustomerPackageInformationDto();
		InitializeParameterLists();
		string[] collection = new string[12]
		{
			"cpaCustomerPackageID", "cpaCreatedBy", "cpaCreatedDate", "cpaUniqueID", "cpaInactiveDate", "cpaInactive", "cpaPackageDescription", "cpaPackageDimensionsUom", "cpaPackageHeight", "cpaPackageLength",
			"cpaPackageWidth", "cpaRowVersion"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("cpaUniqueID|C", customerPackageId);
		AddCustomFieldsToSelectList("CustomerPackages");
		using (DataTable dataTable = GetAsDataTable("CustomerPackages", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPCustomerPackageInformationDto);
			}
			eRPCustomerPackageInformationDto.cpaCustomerPackageID = dataTable.Rows[0].Field<string>("cpaCustomerPackageID");
			eRPCustomerPackageInformationDto.cpaCreatedBy = dataTable.Rows[0].Field<string>("cpaCreatedBy");
			eRPCustomerPackageInformationDto.cpaCreatedDate = dataTable.Rows[0].Field<DateTime?>("cpaCreatedDate");
			eRPCustomerPackageInformationDto.cpaUniqueID = dataTable.Rows[0].Field<Guid>("cpaUniqueID");
			eRPCustomerPackageInformationDto.cpaInactiveDate = dataTable.Rows[0].Field<DateTime?>("cpaInactiveDate");
			eRPCustomerPackageInformationDto.cpaInactive = dataTable.Rows[0].Field<bool>("cpaInactive");
			eRPCustomerPackageInformationDto.cpaPackageDescription = dataTable.Rows[0].Field<string>("cpaPackageDescription");
			eRPCustomerPackageInformationDto.cpaPackageDimensionsUom = dataTable.Rows[0].Field<string>("cpaPackageDimensionsUom");
			eRPCustomerPackageInformationDto.cpaPackageHeight = dataTable.Rows[0].Field<int>("cpaPackageHeight");
			eRPCustomerPackageInformationDto.cpaPackageLength = dataTable.Rows[0].Field<int>("cpaPackageLength");
			eRPCustomerPackageInformationDto.cpaPackageWidth = dataTable.Rows[0].Field<int>("cpaPackageWidth");
			eRPCustomerPackageInformationDto.cpaRowVersion = dataTable.Rows[0].Field<byte[]>("cpaRowVersion");
			eRPCustomerPackageInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPCustomerPackageInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPCustomerPackageInformationDto);
	}

	public Task<APIValidationInfoDto> SaveCustomerPackage(ERPCustomerPackageDto customerPackage)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM CustomerPackages WHERE cpaUniqueID = " + M1Util.ConvertToLinq(customerPackage.cpaUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cpaCustomerPackageID"] = customerPackage.cpaCustomerPackageID.ToUpper();
				customerPackage.cpaUniqueID = ((customerPackage.cpaUniqueID == Guid.Empty) ? Guid.NewGuid() : customerPackage.cpaUniqueID);
				dataRow["cpaUniqueID"] = customerPackage.cpaUniqueID;
				dataRow["cpaCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cpaCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The CustomerPackage could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (customerPackage.cpaRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the CustomerPackage is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cpaRowVersion"], customerPackage.cpaRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the CustomerPackage has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the CustomerPackage again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? cpaInactiveDate = customerPackage.cpaInactiveDate;
			dataRow2["cpaInactiveDate"] = (cpaInactiveDate.HasValue ? ((object)cpaInactiveDate.GetValueOrDefault()) : dataRow["cpaInactiveDate"]);
			dataRow["cpaInactive"] = customerPackage.cpaInactive;
			dataRow["cpaPackageDescription"] = customerPackage.cpaPackageDescription;
			dataRow["cpaPackageDimensionsUom"] = customerPackage.cpaPackageDimensionsUom;
			dataRow["cpaPackageHeight"] = customerPackage.cpaPackageHeight;
			dataRow["cpaPackageLength"] = customerPackage.cpaPackageLength;
			dataRow["cpaPackageWidth"] = customerPackage.cpaPackageWidth;
			if (customerPackage.CustomFields != null && customerPackage.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in customerPackage.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the CustomerPackage [{customerPackage.cpaUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the CustomerPackage [{customerPackage.cpaUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
