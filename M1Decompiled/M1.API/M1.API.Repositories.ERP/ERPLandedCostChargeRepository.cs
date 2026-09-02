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

public class ERPLandedCostChargeRepository : APIBaseRepository, IERPLandedCostChargeRepository, IAPIBaseRepository, IDisposable
{
	public ERPLandedCostChargeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesLandedCostChargeExist(Guid landedCostChargeId)
	{
		InitializeParameterLists();
		base.filterList.Add("rmhUniqueID|C", landedCostChargeId);
		base.selectList.Add("rmhUniqueID");
		return Task.FromResult(GetAsObject("LandedCostCharges", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPLandedCostChargeInformationDto>> GetAllLandedCostCharges(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPLandedCostChargeInformationDto> collection = new List<ERPLandedCostChargeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[27]
		{
			"rmhApInvoiceID", "rmhApInvoiceLineID", "rmhCreatedBy", "rmhCreatedDate", "rmhCurrencyRateID", "rmhDescription", "rmhUniqueID", "rmhEstExchangeRate", "rmhEstTotalCost", "rmhEstTotalCostForeign",
			"rmhExchangeRate", "rmhCustomRate", "rmhInTransitJournalsCreated", "rmhInvoicedComplete", "rmhReversed", "rmhLandedCostCategoryID", "rmhLandedCostID", "rmhLandedCostMethod", "rmhReverseLandedCostChargeID", "rmhReverseLandedCostID",
			"rmhRowVersion", "rmhLandedCostChargeID", "rmhSupplierContactID", "rmhSupplierLocationID", "rmhSupplierOrganizationID", "rmhTotalCost", "rmhTotalCostForeign"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("LandedCostCharges");
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
		using (DataTable dataTable = GetAsDataTable("LandedCostCharges", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPLandedCostChargeInformationDto eRPLandedCostChargeInformationDto = new ERPLandedCostChargeInformationDto();
				eRPLandedCostChargeInformationDto.rmhApInvoiceID = dataTable.Rows[i].Field<string>("rmhApInvoiceID");
				eRPLandedCostChargeInformationDto.rmhApInvoiceLineID = dataTable.Rows[i].Field<short>("rmhApInvoiceLineID");
				eRPLandedCostChargeInformationDto.rmhCreatedBy = dataTable.Rows[i].Field<string>("rmhCreatedBy");
				eRPLandedCostChargeInformationDto.rmhCreatedDate = dataTable.Rows[i].Field<DateTime?>("rmhCreatedDate");
				eRPLandedCostChargeInformationDto.rmhCurrencyRateID = dataTable.Rows[i].Field<string>("rmhCurrencyRateID");
				eRPLandedCostChargeInformationDto.rmhDescription = dataTable.Rows[i].Field<string>("rmhDescription");
				eRPLandedCostChargeInformationDto.rmhUniqueID = dataTable.Rows[i].Field<Guid>("rmhUniqueID");
				eRPLandedCostChargeInformationDto.rmhEstExchangeRate = dataTable.Rows[i].Field<decimal>("rmhEstExchangeRate");
				eRPLandedCostChargeInformationDto.rmhEstTotalCost = dataTable.Rows[i].Field<decimal>("rmhEstTotalCost");
				eRPLandedCostChargeInformationDto.rmhEstTotalCostForeign = dataTable.Rows[i].Field<decimal>("rmhEstTotalCostForeign");
				eRPLandedCostChargeInformationDto.rmhExchangeRate = dataTable.Rows[i].Field<decimal>("rmhExchangeRate");
				eRPLandedCostChargeInformationDto.rmhCustomRate = dataTable.Rows[i].Field<bool>("rmhCustomRate");
				eRPLandedCostChargeInformationDto.rmhInTransitJournalsCreated = dataTable.Rows[i].Field<bool>("rmhInTransitJournalsCreated");
				eRPLandedCostChargeInformationDto.rmhInvoicedComplete = dataTable.Rows[i].Field<bool>("rmhInvoicedComplete");
				eRPLandedCostChargeInformationDto.rmhReversed = dataTable.Rows[i].Field<bool>("rmhReversed");
				eRPLandedCostChargeInformationDto.rmhLandedCostCategoryID = dataTable.Rows[i].Field<string>("rmhLandedCostCategoryID");
				eRPLandedCostChargeInformationDto.rmhLandedCostID = dataTable.Rows[i].Field<string>("rmhLandedCostID");
				eRPLandedCostChargeInformationDto.rmhLandedCostMethod = dataTable.Rows[i].Field<byte>("rmhLandedCostMethod");
				eRPLandedCostChargeInformationDto.rmhReverseLandedCostChargeID = dataTable.Rows[i].Field<short>("rmhReverseLandedCostChargeID");
				eRPLandedCostChargeInformationDto.rmhReverseLandedCostID = dataTable.Rows[i].Field<string>("rmhReverseLandedCostID");
				eRPLandedCostChargeInformationDto.rmhRowVersion = dataTable.Rows[i].Field<byte[]>("rmhRowVersion");
				eRPLandedCostChargeInformationDto.rmhLandedCostChargeID = dataTable.Rows[i].Field<short>("rmhLandedCostChargeID");
				eRPLandedCostChargeInformationDto.rmhSupplierContactID = dataTable.Rows[i].Field<string>("rmhSupplierContactID");
				eRPLandedCostChargeInformationDto.rmhSupplierLocationID = dataTable.Rows[i].Field<string>("rmhSupplierLocationID");
				eRPLandedCostChargeInformationDto.rmhSupplierOrganizationID = dataTable.Rows[i].Field<string>("rmhSupplierOrganizationID");
				eRPLandedCostChargeInformationDto.rmhTotalCost = dataTable.Rows[i].Field<decimal>("rmhTotalCost");
				eRPLandedCostChargeInformationDto.rmhTotalCostForeign = dataTable.Rows[i].Field<decimal>("rmhTotalCostForeign");
				eRPLandedCostChargeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPLandedCostChargeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPLandedCostChargeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPLandedCostChargeInformationDto> GetLandedCostCharge(Guid landedCostChargeId)
	{
		ERPLandedCostChargeInformationDto eRPLandedCostChargeInformationDto = new ERPLandedCostChargeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[27]
		{
			"rmhApInvoiceID", "rmhApInvoiceLineID", "rmhCreatedBy", "rmhCreatedDate", "rmhCurrencyRateID", "rmhDescription", "rmhUniqueID", "rmhEstExchangeRate", "rmhEstTotalCost", "rmhEstTotalCostForeign",
			"rmhExchangeRate", "rmhCustomRate", "rmhInTransitJournalsCreated", "rmhInvoicedComplete", "rmhReversed", "rmhLandedCostCategoryID", "rmhLandedCostID", "rmhLandedCostMethod", "rmhReverseLandedCostChargeID", "rmhReverseLandedCostID",
			"rmhRowVersion", "rmhLandedCostChargeID", "rmhSupplierContactID", "rmhSupplierLocationID", "rmhSupplierOrganizationID", "rmhTotalCost", "rmhTotalCostForeign"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rmhUniqueID|C", landedCostChargeId);
		AddCustomFieldsToSelectList("LandedCostCharges");
		using (DataTable dataTable = GetAsDataTable("LandedCostCharges", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPLandedCostChargeInformationDto);
			}
			eRPLandedCostChargeInformationDto.rmhApInvoiceID = dataTable.Rows[0].Field<string>("rmhApInvoiceID");
			eRPLandedCostChargeInformationDto.rmhApInvoiceLineID = dataTable.Rows[0].Field<short>("rmhApInvoiceLineID");
			eRPLandedCostChargeInformationDto.rmhCreatedBy = dataTable.Rows[0].Field<string>("rmhCreatedBy");
			eRPLandedCostChargeInformationDto.rmhCreatedDate = dataTable.Rows[0].Field<DateTime?>("rmhCreatedDate");
			eRPLandedCostChargeInformationDto.rmhCurrencyRateID = dataTable.Rows[0].Field<string>("rmhCurrencyRateID");
			eRPLandedCostChargeInformationDto.rmhDescription = dataTable.Rows[0].Field<string>("rmhDescription");
			eRPLandedCostChargeInformationDto.rmhUniqueID = dataTable.Rows[0].Field<Guid>("rmhUniqueID");
			eRPLandedCostChargeInformationDto.rmhEstExchangeRate = dataTable.Rows[0].Field<decimal>("rmhEstExchangeRate");
			eRPLandedCostChargeInformationDto.rmhEstTotalCost = dataTable.Rows[0].Field<decimal>("rmhEstTotalCost");
			eRPLandedCostChargeInformationDto.rmhEstTotalCostForeign = dataTable.Rows[0].Field<decimal>("rmhEstTotalCostForeign");
			eRPLandedCostChargeInformationDto.rmhExchangeRate = dataTable.Rows[0].Field<decimal>("rmhExchangeRate");
			eRPLandedCostChargeInformationDto.rmhCustomRate = dataTable.Rows[0].Field<bool>("rmhCustomRate");
			eRPLandedCostChargeInformationDto.rmhInTransitJournalsCreated = dataTable.Rows[0].Field<bool>("rmhInTransitJournalsCreated");
			eRPLandedCostChargeInformationDto.rmhInvoicedComplete = dataTable.Rows[0].Field<bool>("rmhInvoicedComplete");
			eRPLandedCostChargeInformationDto.rmhReversed = dataTable.Rows[0].Field<bool>("rmhReversed");
			eRPLandedCostChargeInformationDto.rmhLandedCostCategoryID = dataTable.Rows[0].Field<string>("rmhLandedCostCategoryID");
			eRPLandedCostChargeInformationDto.rmhLandedCostID = dataTable.Rows[0].Field<string>("rmhLandedCostID");
			eRPLandedCostChargeInformationDto.rmhLandedCostMethod = dataTable.Rows[0].Field<byte>("rmhLandedCostMethod");
			eRPLandedCostChargeInformationDto.rmhReverseLandedCostChargeID = dataTable.Rows[0].Field<short>("rmhReverseLandedCostChargeID");
			eRPLandedCostChargeInformationDto.rmhReverseLandedCostID = dataTable.Rows[0].Field<string>("rmhReverseLandedCostID");
			eRPLandedCostChargeInformationDto.rmhRowVersion = dataTable.Rows[0].Field<byte[]>("rmhRowVersion");
			eRPLandedCostChargeInformationDto.rmhLandedCostChargeID = dataTable.Rows[0].Field<short>("rmhLandedCostChargeID");
			eRPLandedCostChargeInformationDto.rmhSupplierContactID = dataTable.Rows[0].Field<string>("rmhSupplierContactID");
			eRPLandedCostChargeInformationDto.rmhSupplierLocationID = dataTable.Rows[0].Field<string>("rmhSupplierLocationID");
			eRPLandedCostChargeInformationDto.rmhSupplierOrganizationID = dataTable.Rows[0].Field<string>("rmhSupplierOrganizationID");
			eRPLandedCostChargeInformationDto.rmhTotalCost = dataTable.Rows[0].Field<decimal>("rmhTotalCost");
			eRPLandedCostChargeInformationDto.rmhTotalCostForeign = dataTable.Rows[0].Field<decimal>("rmhTotalCostForeign");
			eRPLandedCostChargeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPLandedCostChargeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPLandedCostChargeInformationDto);
	}

	public Task<APIValidationInfoDto> SaveLandedCostCharge(ERPLandedCostChargeDto landedCostCharge)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM LandedCostCharges WHERE rmhUniqueID = " + M1Util.ConvertToLinq(landedCostCharge.rmhUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["rmhLandedCostID"] = landedCostCharge.rmhLandedCostID.ToUpper();
				dataRow["rmhLandedCostChargeID"] = landedCostCharge.rmhLandedCostChargeID;
				landedCostCharge.rmhUniqueID = ((landedCostCharge.rmhUniqueID == Guid.Empty) ? Guid.NewGuid() : landedCostCharge.rmhUniqueID);
				dataRow["rmhUniqueID"] = landedCostCharge.rmhUniqueID;
				dataRow["rmhCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["rmhCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The LandedCostCharge could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (landedCostCharge.rmhRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the LandedCostCharge is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["rmhRowVersion"], landedCostCharge.rmhRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the LandedCostCharge has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the LandedCostCharge again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["rmhApInvoiceID"] = landedCostCharge.rmhApInvoiceID;
			dataRow["rmhApInvoiceLineID"] = landedCostCharge.rmhApInvoiceLineID;
			dataRow["rmhCurrencyRateID"] = landedCostCharge.rmhCurrencyRateID;
			dataRow["rmhDescription"] = landedCostCharge.rmhDescription;
			dataRow["rmhEstExchangeRate"] = landedCostCharge.rmhEstExchangeRate;
			dataRow["rmhEstTotalCost"] = landedCostCharge.rmhEstTotalCost;
			dataRow["rmhEstTotalCostForeign"] = landedCostCharge.rmhEstTotalCostForeign;
			dataRow["rmhExchangeRate"] = landedCostCharge.rmhExchangeRate;
			dataRow["rmhCustomRate"] = landedCostCharge.rmhCustomRate;
			dataRow["rmhInTransitJournalsCreated"] = landedCostCharge.rmhInTransitJournalsCreated;
			dataRow["rmhInvoicedComplete"] = landedCostCharge.rmhInvoicedComplete;
			dataRow["rmhReversed"] = landedCostCharge.rmhReversed;
			dataRow["rmhLandedCostCategoryID"] = landedCostCharge.rmhLandedCostCategoryID;
			dataRow["rmhLandedCostMethod"] = landedCostCharge.rmhLandedCostMethod;
			dataRow["rmhReverseLandedCostChargeID"] = landedCostCharge.rmhReverseLandedCostChargeID;
			dataRow["rmhReverseLandedCostID"] = landedCostCharge.rmhReverseLandedCostID;
			dataRow["rmhSupplierContactID"] = landedCostCharge.rmhSupplierContactID;
			dataRow["rmhSupplierLocationID"] = landedCostCharge.rmhSupplierLocationID;
			dataRow["rmhSupplierOrganizationID"] = landedCostCharge.rmhSupplierOrganizationID;
			dataRow["rmhTotalCost"] = landedCostCharge.rmhTotalCost;
			dataRow["rmhTotalCostForeign"] = landedCostCharge.rmhTotalCostForeign;
			if (landedCostCharge.CustomFields != null && landedCostCharge.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in landedCostCharge.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the LandedCostCharge [{landedCostCharge.rmhUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the LandedCostCharge [{landedCostCharge.rmhUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
