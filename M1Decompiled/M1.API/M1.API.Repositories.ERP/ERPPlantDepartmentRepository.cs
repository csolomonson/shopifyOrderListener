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

public class ERPPlantDepartmentRepository : APIBaseRepository, IERPPlantDepartmentRepository, IAPIBaseRepository, IDisposable
{
	public ERPPlantDepartmentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPlantDepartmentExist(Guid plantDepartmentId)
	{
		InitializeParameterLists();
		base.filterList.Add("xavUniqueID|C", plantDepartmentId);
		base.selectList.Add("xavUniqueID");
		return Task.FromResult(GetAsObject("PlantDepartments", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPlantDepartmentInformationDto>> GetAllPlantDepartments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPlantDepartmentInformationDto> collection = new List<ERPPlantDepartmentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[23]
		{
			"xavApApGlAccountID", "xavApBankAccountID", "xavApCashGlAccountID", "xavApDiscountGlAccountID", "xavApFreightGlAccountID", "xavArArGlAccountID", "xavArBankAccountID", "xavArCashGlAccountID", "xavArDepositGlAccountID", "xavArDiscountGlAccountID",
			"xavArFreightGlAccountID", "xavArSalesGlAccountID", "xavPlantDepartmentID", "xavCreatedBy", "xavCreatedDate", "xavUniqueID", "xavEstablishedDate", "xavInactiveDate", "xavInactive", "xavUseProperties",
			"xavName", "xavPlantID", "xavRowVersion"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PlantDepartments");
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
		using (DataTable dataTable = GetAsDataTable("PlantDepartments", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPlantDepartmentInformationDto eRPPlantDepartmentInformationDto = new ERPPlantDepartmentInformationDto();
				eRPPlantDepartmentInformationDto.xavApApGlAccountID = dataTable.Rows[i].Field<string>("xavApApGlAccountID");
				eRPPlantDepartmentInformationDto.xavApBankAccountID = dataTable.Rows[i].Field<string>("xavApBankAccountID");
				eRPPlantDepartmentInformationDto.xavApCashGlAccountID = dataTable.Rows[i].Field<string>("xavApCashGlAccountID");
				eRPPlantDepartmentInformationDto.xavApDiscountGlAccountID = dataTable.Rows[i].Field<string>("xavApDiscountGlAccountID");
				eRPPlantDepartmentInformationDto.xavApFreightGlAccountID = dataTable.Rows[i].Field<string>("xavApFreightGlAccountID");
				eRPPlantDepartmentInformationDto.xavArArGlAccountID = dataTable.Rows[i].Field<string>("xavArArGlAccountID");
				eRPPlantDepartmentInformationDto.xavArBankAccountID = dataTable.Rows[i].Field<string>("xavArBankAccountID");
				eRPPlantDepartmentInformationDto.xavArCashGlAccountID = dataTable.Rows[i].Field<string>("xavArCashGlAccountID");
				eRPPlantDepartmentInformationDto.xavArDepositGlAccountID = dataTable.Rows[i].Field<string>("xavArDepositGlAccountID");
				eRPPlantDepartmentInformationDto.xavArDiscountGlAccountID = dataTable.Rows[i].Field<string>("xavArDiscountGlAccountID");
				eRPPlantDepartmentInformationDto.xavArFreightGlAccountID = dataTable.Rows[i].Field<string>("xavArFreightGlAccountID");
				eRPPlantDepartmentInformationDto.xavArSalesGlAccountID = dataTable.Rows[i].Field<string>("xavArSalesGlAccountID");
				eRPPlantDepartmentInformationDto.xavPlantDepartmentID = dataTable.Rows[i].Field<string>("xavPlantDepartmentID");
				eRPPlantDepartmentInformationDto.xavCreatedBy = dataTable.Rows[i].Field<string>("xavCreatedBy");
				eRPPlantDepartmentInformationDto.xavCreatedDate = dataTable.Rows[i].Field<DateTime?>("xavCreatedDate");
				eRPPlantDepartmentInformationDto.xavUniqueID = dataTable.Rows[i].Field<Guid>("xavUniqueID");
				eRPPlantDepartmentInformationDto.xavEstablishedDate = dataTable.Rows[i].Field<DateTime?>("xavEstablishedDate");
				eRPPlantDepartmentInformationDto.xavInactiveDate = dataTable.Rows[i].Field<DateTime?>("xavInactiveDate");
				eRPPlantDepartmentInformationDto.xavInactive = dataTable.Rows[i].Field<bool>("xavInactive");
				eRPPlantDepartmentInformationDto.xavUseProperties = dataTable.Rows[i].Field<bool>("xavUseProperties");
				eRPPlantDepartmentInformationDto.xavName = dataTable.Rows[i].Field<string>("xavName");
				eRPPlantDepartmentInformationDto.xavPlantID = dataTable.Rows[i].Field<string>("xavPlantID");
				eRPPlantDepartmentInformationDto.xavRowVersion = dataTable.Rows[i].Field<byte[]>("xavRowVersion");
				eRPPlantDepartmentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPlantDepartmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPlantDepartmentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPlantDepartmentInformationDto> GetPlantDepartment(Guid plantDepartmentId)
	{
		ERPPlantDepartmentInformationDto eRPPlantDepartmentInformationDto = new ERPPlantDepartmentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[23]
		{
			"xavApApGlAccountID", "xavApBankAccountID", "xavApCashGlAccountID", "xavApDiscountGlAccountID", "xavApFreightGlAccountID", "xavArArGlAccountID", "xavArBankAccountID", "xavArCashGlAccountID", "xavArDepositGlAccountID", "xavArDiscountGlAccountID",
			"xavArFreightGlAccountID", "xavArSalesGlAccountID", "xavPlantDepartmentID", "xavCreatedBy", "xavCreatedDate", "xavUniqueID", "xavEstablishedDate", "xavInactiveDate", "xavInactive", "xavUseProperties",
			"xavName", "xavPlantID", "xavRowVersion"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xavUniqueID|C", plantDepartmentId);
		AddCustomFieldsToSelectList("PlantDepartments");
		using (DataTable dataTable = GetAsDataTable("PlantDepartments", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPlantDepartmentInformationDto);
			}
			eRPPlantDepartmentInformationDto.xavApApGlAccountID = dataTable.Rows[0].Field<string>("xavApApGlAccountID");
			eRPPlantDepartmentInformationDto.xavApBankAccountID = dataTable.Rows[0].Field<string>("xavApBankAccountID");
			eRPPlantDepartmentInformationDto.xavApCashGlAccountID = dataTable.Rows[0].Field<string>("xavApCashGlAccountID");
			eRPPlantDepartmentInformationDto.xavApDiscountGlAccountID = dataTable.Rows[0].Field<string>("xavApDiscountGlAccountID");
			eRPPlantDepartmentInformationDto.xavApFreightGlAccountID = dataTable.Rows[0].Field<string>("xavApFreightGlAccountID");
			eRPPlantDepartmentInformationDto.xavArArGlAccountID = dataTable.Rows[0].Field<string>("xavArArGlAccountID");
			eRPPlantDepartmentInformationDto.xavArBankAccountID = dataTable.Rows[0].Field<string>("xavArBankAccountID");
			eRPPlantDepartmentInformationDto.xavArCashGlAccountID = dataTable.Rows[0].Field<string>("xavArCashGlAccountID");
			eRPPlantDepartmentInformationDto.xavArDepositGlAccountID = dataTable.Rows[0].Field<string>("xavArDepositGlAccountID");
			eRPPlantDepartmentInformationDto.xavArDiscountGlAccountID = dataTable.Rows[0].Field<string>("xavArDiscountGlAccountID");
			eRPPlantDepartmentInformationDto.xavArFreightGlAccountID = dataTable.Rows[0].Field<string>("xavArFreightGlAccountID");
			eRPPlantDepartmentInformationDto.xavArSalesGlAccountID = dataTable.Rows[0].Field<string>("xavArSalesGlAccountID");
			eRPPlantDepartmentInformationDto.xavPlantDepartmentID = dataTable.Rows[0].Field<string>("xavPlantDepartmentID");
			eRPPlantDepartmentInformationDto.xavCreatedBy = dataTable.Rows[0].Field<string>("xavCreatedBy");
			eRPPlantDepartmentInformationDto.xavCreatedDate = dataTable.Rows[0].Field<DateTime?>("xavCreatedDate");
			eRPPlantDepartmentInformationDto.xavUniqueID = dataTable.Rows[0].Field<Guid>("xavUniqueID");
			eRPPlantDepartmentInformationDto.xavEstablishedDate = dataTable.Rows[0].Field<DateTime?>("xavEstablishedDate");
			eRPPlantDepartmentInformationDto.xavInactiveDate = dataTable.Rows[0].Field<DateTime?>("xavInactiveDate");
			eRPPlantDepartmentInformationDto.xavInactive = dataTable.Rows[0].Field<bool>("xavInactive");
			eRPPlantDepartmentInformationDto.xavUseProperties = dataTable.Rows[0].Field<bool>("xavUseProperties");
			eRPPlantDepartmentInformationDto.xavName = dataTable.Rows[0].Field<string>("xavName");
			eRPPlantDepartmentInformationDto.xavPlantID = dataTable.Rows[0].Field<string>("xavPlantID");
			eRPPlantDepartmentInformationDto.xavRowVersion = dataTable.Rows[0].Field<byte[]>("xavRowVersion");
			eRPPlantDepartmentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPlantDepartmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPlantDepartmentInformationDto);
	}

	public Task<APIValidationInfoDto> SavePlantDepartment(ERPPlantDepartmentDto plantDepartment)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PlantDepartments WHERE xavUniqueID = " + M1Util.ConvertToLinq(plantDepartment.xavUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xavPlantID"] = plantDepartment.xavPlantID.ToUpper();
				dataRow["xavPlantDepartmentID"] = plantDepartment.xavPlantDepartmentID.ToUpper();
				plantDepartment.xavUniqueID = ((plantDepartment.xavUniqueID == Guid.Empty) ? Guid.NewGuid() : plantDepartment.xavUniqueID);
				dataRow["xavUniqueID"] = plantDepartment.xavUniqueID;
				dataRow["xavCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xavCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PlantDepartment could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (plantDepartment.xavRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PlantDepartment is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xavRowVersion"], plantDepartment.xavRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PlantDepartment has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PlantDepartment again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xavApApGlAccountID"] = plantDepartment.xavApApGlAccountID;
			dataRow["xavApBankAccountID"] = plantDepartment.xavApBankAccountID;
			dataRow["xavApCashGlAccountID"] = plantDepartment.xavApCashGlAccountID;
			dataRow["xavApDiscountGlAccountID"] = plantDepartment.xavApDiscountGlAccountID;
			dataRow["xavApFreightGlAccountID"] = plantDepartment.xavApFreightGlAccountID;
			dataRow["xavArArGlAccountID"] = plantDepartment.xavArArGlAccountID;
			dataRow["xavArBankAccountID"] = plantDepartment.xavArBankAccountID;
			dataRow["xavArCashGlAccountID"] = plantDepartment.xavArCashGlAccountID;
			dataRow["xavArDepositGlAccountID"] = plantDepartment.xavArDepositGlAccountID;
			dataRow["xavArDiscountGlAccountID"] = plantDepartment.xavArDiscountGlAccountID;
			dataRow["xavArFreightGlAccountID"] = plantDepartment.xavArFreightGlAccountID;
			dataRow["xavArSalesGlAccountID"] = plantDepartment.xavArSalesGlAccountID;
			DataRow dataRow2 = dataRow;
			DateTime? xavEstablishedDate = plantDepartment.xavEstablishedDate;
			dataRow2["xavEstablishedDate"] = (xavEstablishedDate.HasValue ? ((object)xavEstablishedDate.GetValueOrDefault()) : dataRow["xavEstablishedDate"]);
			DataRow dataRow3 = dataRow;
			xavEstablishedDate = plantDepartment.xavInactiveDate;
			dataRow3["xavInactiveDate"] = (xavEstablishedDate.HasValue ? ((object)xavEstablishedDate.GetValueOrDefault()) : dataRow["xavInactiveDate"]);
			dataRow["xavInactive"] = plantDepartment.xavInactive;
			dataRow["xavUseProperties"] = plantDepartment.xavUseProperties;
			dataRow["xavName"] = plantDepartment.xavName;
			if (plantDepartment.CustomFields != null && plantDepartment.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in plantDepartment.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PlantDepartment [{plantDepartment.xavUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PlantDepartment [{plantDepartment.xavUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
