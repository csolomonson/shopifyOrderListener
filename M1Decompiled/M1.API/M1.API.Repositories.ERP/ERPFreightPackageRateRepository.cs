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

public class ERPFreightPackageRateRepository : APIBaseRepository, IERPFreightPackageRateRepository, IAPIBaseRepository, IDisposable
{
	public ERPFreightPackageRateRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesFreightPackageRateExist(Guid freightPackageRateId)
	{
		InitializeParameterLists();
		base.filterList.Add("fprUniqueID|C", freightPackageRateId);
		base.selectList.Add("fprUniqueID");
		return Task.FromResult(GetAsObject("FreightPackageRates", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPFreightPackageRateInformationDto>> GetAllFreightPackageRates(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPFreightPackageRateInformationDto> collection = new List<ERPFreightPackageRateInformationDto>();
		InitializeParameterLists();
		string[] array = new string[32]
		{
			"fprCreatedBy", "fprCreatedDate", "fprUniqueID", "fprFdxBaseCharge", "fprFdxCurrency", "fprFdxDeliveryDate", "fprFdxDeliveryDay", "fprFdxDestinationStationID", "fprFdxPackageBaseCharge", "fprFdxPackageBillingWeight",
			"fprFdxPackageDimWeight", "fprFdxPackageFreightDiscount", "fprFdxPackageNetCharge", "fprFdxPackageNetFreight", "fprFdxPackageSurcharges", "fprFdxPackaging", "fprFdxService", "fprFdxTimeInTransit", "fprFdxTotalBillingWeight", "fprFdxTotalCustomerCharge",
			"fprFdxTotalDimWeight", "fprFdxTotalFreightDiscount", "fprFdxTotalNetCharge", "fprFdxTotalNetFreightCharge", "fprFdxTotalSurcharges", "fprFdxUnits", "fprFdxVariableHandlingCharge", "fprFreightPackageID", "fprFreightShipmentID", "fprRCTI",
			"fprRowVersion", "fprFreightPackageRateID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("FreightPackageRates");
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
		using (DataTable dataTable = GetAsDataTable("FreightPackageRates", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPFreightPackageRateInformationDto eRPFreightPackageRateInformationDto = new ERPFreightPackageRateInformationDto();
				eRPFreightPackageRateInformationDto.fprCreatedBy = dataTable.Rows[i].Field<string>("fprCreatedBy");
				eRPFreightPackageRateInformationDto.fprCreatedDate = dataTable.Rows[i].Field<DateTime?>("fprCreatedDate");
				eRPFreightPackageRateInformationDto.fprUniqueID = dataTable.Rows[i].Field<Guid>("fprUniqueID");
				eRPFreightPackageRateInformationDto.fprFdxBaseCharge = dataTable.Rows[i].Field<decimal>("fprFdxBaseCharge");
				eRPFreightPackageRateInformationDto.fprFdxCurrency = dataTable.Rows[i].Field<string>("fprFdxCurrency");
				eRPFreightPackageRateInformationDto.fprFdxDeliveryDate = dataTable.Rows[i].Field<DateTime?>("fprFdxDeliveryDate");
				eRPFreightPackageRateInformationDto.fprFdxDeliveryDay = dataTable.Rows[i].Field<string>("fprFdxDeliveryDay");
				eRPFreightPackageRateInformationDto.fprFdxDestinationStationID = dataTable.Rows[i].Field<string>("fprFdxDestinationStationID");
				eRPFreightPackageRateInformationDto.fprFdxPackageBaseCharge = dataTable.Rows[i].Field<decimal>("fprFdxPackageBaseCharge");
				eRPFreightPackageRateInformationDto.fprFdxPackageBillingWeight = dataTable.Rows[i].Field<decimal>("fprFdxPackageBillingWeight");
				eRPFreightPackageRateInformationDto.fprFdxPackageDimWeight = dataTable.Rows[i].Field<decimal>("fprFdxPackageDimWeight");
				eRPFreightPackageRateInformationDto.fprFdxPackageFreightDiscount = dataTable.Rows[i].Field<decimal>("fprFdxPackageFreightDiscount");
				eRPFreightPackageRateInformationDto.fprFdxPackageNetCharge = dataTable.Rows[i].Field<decimal>("fprFdxPackageNetCharge");
				eRPFreightPackageRateInformationDto.fprFdxPackageNetFreight = dataTable.Rows[i].Field<decimal>("fprFdxPackageNetFreight");
				eRPFreightPackageRateInformationDto.fprFdxPackageSurcharges = dataTable.Rows[i].Field<decimal>("fprFdxPackageSurcharges");
				eRPFreightPackageRateInformationDto.fprFdxPackaging = dataTable.Rows[i].Field<string>("fprFdxPackaging");
				eRPFreightPackageRateInformationDto.fprFdxService = dataTable.Rows[i].Field<string>("fprFdxService");
				eRPFreightPackageRateInformationDto.fprFdxTimeInTransit = dataTable.Rows[i].Field<short>("fprFdxTimeInTransit");
				eRPFreightPackageRateInformationDto.fprFdxTotalBillingWeight = dataTable.Rows[i].Field<decimal>("fprFdxTotalBillingWeight");
				eRPFreightPackageRateInformationDto.fprFdxTotalCustomerCharge = dataTable.Rows[i].Field<decimal>("fprFdxTotalCustomerCharge");
				eRPFreightPackageRateInformationDto.fprFdxTotalDimWeight = dataTable.Rows[i].Field<decimal>("fprFdxTotalDimWeight");
				eRPFreightPackageRateInformationDto.fprFdxTotalFreightDiscount = dataTable.Rows[i].Field<decimal>("fprFdxTotalFreightDiscount");
				eRPFreightPackageRateInformationDto.fprFdxTotalNetCharge = dataTable.Rows[i].Field<decimal>("fprFdxTotalNetCharge");
				eRPFreightPackageRateInformationDto.fprFdxTotalNetFreightCharge = dataTable.Rows[i].Field<decimal>("fprFdxTotalNetFreightCharge");
				eRPFreightPackageRateInformationDto.fprFdxTotalSurcharges = dataTable.Rows[i].Field<decimal>("fprFdxTotalSurcharges");
				eRPFreightPackageRateInformationDto.fprFdxUnits = dataTable.Rows[i].Field<string>("fprFdxUnits");
				eRPFreightPackageRateInformationDto.fprFdxVariableHandlingCharge = dataTable.Rows[i].Field<decimal>("fprFdxVariableHandlingCharge");
				eRPFreightPackageRateInformationDto.fprFreightPackageID = dataTable.Rows[i].Field<short>("fprFreightPackageID");
				eRPFreightPackageRateInformationDto.fprFreightShipmentID = dataTable.Rows[i].Field<string>("fprFreightShipmentID");
				eRPFreightPackageRateInformationDto.fprRCTI = dataTable.Rows[i].Field<string>("fprRCTI");
				eRPFreightPackageRateInformationDto.fprRowVersion = dataTable.Rows[i].Field<byte[]>("fprRowVersion");
				eRPFreightPackageRateInformationDto.fprFreightPackageRateID = dataTable.Rows[i].Field<short>("fprFreightPackageRateID");
				eRPFreightPackageRateInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPFreightPackageRateInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPFreightPackageRateInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPFreightPackageRateInformationDto> GetFreightPackageRate(Guid freightPackageRateId)
	{
		ERPFreightPackageRateInformationDto eRPFreightPackageRateInformationDto = new ERPFreightPackageRateInformationDto();
		InitializeParameterLists();
		string[] collection = new string[32]
		{
			"fprCreatedBy", "fprCreatedDate", "fprUniqueID", "fprFdxBaseCharge", "fprFdxCurrency", "fprFdxDeliveryDate", "fprFdxDeliveryDay", "fprFdxDestinationStationID", "fprFdxPackageBaseCharge", "fprFdxPackageBillingWeight",
			"fprFdxPackageDimWeight", "fprFdxPackageFreightDiscount", "fprFdxPackageNetCharge", "fprFdxPackageNetFreight", "fprFdxPackageSurcharges", "fprFdxPackaging", "fprFdxService", "fprFdxTimeInTransit", "fprFdxTotalBillingWeight", "fprFdxTotalCustomerCharge",
			"fprFdxTotalDimWeight", "fprFdxTotalFreightDiscount", "fprFdxTotalNetCharge", "fprFdxTotalNetFreightCharge", "fprFdxTotalSurcharges", "fprFdxUnits", "fprFdxVariableHandlingCharge", "fprFreightPackageID", "fprFreightShipmentID", "fprRCTI",
			"fprRowVersion", "fprFreightPackageRateID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("fprUniqueID|C", freightPackageRateId);
		AddCustomFieldsToSelectList("FreightPackageRates");
		using (DataTable dataTable = GetAsDataTable("FreightPackageRates", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPFreightPackageRateInformationDto);
			}
			eRPFreightPackageRateInformationDto.fprCreatedBy = dataTable.Rows[0].Field<string>("fprCreatedBy");
			eRPFreightPackageRateInformationDto.fprCreatedDate = dataTable.Rows[0].Field<DateTime?>("fprCreatedDate");
			eRPFreightPackageRateInformationDto.fprUniqueID = dataTable.Rows[0].Field<Guid>("fprUniqueID");
			eRPFreightPackageRateInformationDto.fprFdxBaseCharge = dataTable.Rows[0].Field<decimal>("fprFdxBaseCharge");
			eRPFreightPackageRateInformationDto.fprFdxCurrency = dataTable.Rows[0].Field<string>("fprFdxCurrency");
			eRPFreightPackageRateInformationDto.fprFdxDeliveryDate = dataTable.Rows[0].Field<DateTime?>("fprFdxDeliveryDate");
			eRPFreightPackageRateInformationDto.fprFdxDeliveryDay = dataTable.Rows[0].Field<string>("fprFdxDeliveryDay");
			eRPFreightPackageRateInformationDto.fprFdxDestinationStationID = dataTable.Rows[0].Field<string>("fprFdxDestinationStationID");
			eRPFreightPackageRateInformationDto.fprFdxPackageBaseCharge = dataTable.Rows[0].Field<decimal>("fprFdxPackageBaseCharge");
			eRPFreightPackageRateInformationDto.fprFdxPackageBillingWeight = dataTable.Rows[0].Field<decimal>("fprFdxPackageBillingWeight");
			eRPFreightPackageRateInformationDto.fprFdxPackageDimWeight = dataTable.Rows[0].Field<decimal>("fprFdxPackageDimWeight");
			eRPFreightPackageRateInformationDto.fprFdxPackageFreightDiscount = dataTable.Rows[0].Field<decimal>("fprFdxPackageFreightDiscount");
			eRPFreightPackageRateInformationDto.fprFdxPackageNetCharge = dataTable.Rows[0].Field<decimal>("fprFdxPackageNetCharge");
			eRPFreightPackageRateInformationDto.fprFdxPackageNetFreight = dataTable.Rows[0].Field<decimal>("fprFdxPackageNetFreight");
			eRPFreightPackageRateInformationDto.fprFdxPackageSurcharges = dataTable.Rows[0].Field<decimal>("fprFdxPackageSurcharges");
			eRPFreightPackageRateInformationDto.fprFdxPackaging = dataTable.Rows[0].Field<string>("fprFdxPackaging");
			eRPFreightPackageRateInformationDto.fprFdxService = dataTable.Rows[0].Field<string>("fprFdxService");
			eRPFreightPackageRateInformationDto.fprFdxTimeInTransit = dataTable.Rows[0].Field<short>("fprFdxTimeInTransit");
			eRPFreightPackageRateInformationDto.fprFdxTotalBillingWeight = dataTable.Rows[0].Field<decimal>("fprFdxTotalBillingWeight");
			eRPFreightPackageRateInformationDto.fprFdxTotalCustomerCharge = dataTable.Rows[0].Field<decimal>("fprFdxTotalCustomerCharge");
			eRPFreightPackageRateInformationDto.fprFdxTotalDimWeight = dataTable.Rows[0].Field<decimal>("fprFdxTotalDimWeight");
			eRPFreightPackageRateInformationDto.fprFdxTotalFreightDiscount = dataTable.Rows[0].Field<decimal>("fprFdxTotalFreightDiscount");
			eRPFreightPackageRateInformationDto.fprFdxTotalNetCharge = dataTable.Rows[0].Field<decimal>("fprFdxTotalNetCharge");
			eRPFreightPackageRateInformationDto.fprFdxTotalNetFreightCharge = dataTable.Rows[0].Field<decimal>("fprFdxTotalNetFreightCharge");
			eRPFreightPackageRateInformationDto.fprFdxTotalSurcharges = dataTable.Rows[0].Field<decimal>("fprFdxTotalSurcharges");
			eRPFreightPackageRateInformationDto.fprFdxUnits = dataTable.Rows[0].Field<string>("fprFdxUnits");
			eRPFreightPackageRateInformationDto.fprFdxVariableHandlingCharge = dataTable.Rows[0].Field<decimal>("fprFdxVariableHandlingCharge");
			eRPFreightPackageRateInformationDto.fprFreightPackageID = dataTable.Rows[0].Field<short>("fprFreightPackageID");
			eRPFreightPackageRateInformationDto.fprFreightShipmentID = dataTable.Rows[0].Field<string>("fprFreightShipmentID");
			eRPFreightPackageRateInformationDto.fprRCTI = dataTable.Rows[0].Field<string>("fprRCTI");
			eRPFreightPackageRateInformationDto.fprRowVersion = dataTable.Rows[0].Field<byte[]>("fprRowVersion");
			eRPFreightPackageRateInformationDto.fprFreightPackageRateID = dataTable.Rows[0].Field<short>("fprFreightPackageRateID");
			eRPFreightPackageRateInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPFreightPackageRateInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPFreightPackageRateInformationDto);
	}

	public Task<APIValidationInfoDto> SaveFreightPackageRate(ERPFreightPackageRateDto freightPackageRate)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM FreightPackageRates WHERE fprUniqueID = " + M1Util.ConvertToLinq(freightPackageRate.fprUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["fprFreightShipmentID"] = freightPackageRate.fprFreightShipmentID.ToUpper();
				dataRow["fprFreightPackageID"] = freightPackageRate.fprFreightPackageID;
				dataRow["fprFreightPackageRateID"] = freightPackageRate.fprFreightPackageRateID;
				freightPackageRate.fprUniqueID = ((freightPackageRate.fprUniqueID == Guid.Empty) ? Guid.NewGuid() : freightPackageRate.fprUniqueID);
				dataRow["fprUniqueID"] = freightPackageRate.fprUniqueID;
				dataRow["fprCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["fprCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The FreightPackageRate could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (freightPackageRate.fprRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the FreightPackageRate is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["fprRowVersion"], freightPackageRate.fprRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the FreightPackageRate has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the FreightPackageRate again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["fprFdxBaseCharge"] = freightPackageRate.fprFdxBaseCharge;
			dataRow["fprFdxCurrency"] = freightPackageRate.fprFdxCurrency;
			DataRow dataRow2 = dataRow;
			DateTime? fprFdxDeliveryDate = freightPackageRate.fprFdxDeliveryDate;
			dataRow2["fprFdxDeliveryDate"] = (fprFdxDeliveryDate.HasValue ? ((object)fprFdxDeliveryDate.GetValueOrDefault()) : dataRow["fprFdxDeliveryDate"]);
			dataRow["fprFdxDeliveryDay"] = freightPackageRate.fprFdxDeliveryDay;
			dataRow["fprFdxDestinationStationID"] = freightPackageRate.fprFdxDestinationStationID;
			dataRow["fprFdxPackageBaseCharge"] = freightPackageRate.fprFdxPackageBaseCharge;
			dataRow["fprFdxPackageBillingWeight"] = freightPackageRate.fprFdxPackageBillingWeight;
			dataRow["fprFdxPackageDimWeight"] = freightPackageRate.fprFdxPackageDimWeight;
			dataRow["fprFdxPackageFreightDiscount"] = freightPackageRate.fprFdxPackageFreightDiscount;
			dataRow["fprFdxPackageNetCharge"] = freightPackageRate.fprFdxPackageNetCharge;
			dataRow["fprFdxPackageNetFreight"] = freightPackageRate.fprFdxPackageNetFreight;
			dataRow["fprFdxPackageSurcharges"] = freightPackageRate.fprFdxPackageSurcharges;
			dataRow["fprFdxPackaging"] = freightPackageRate.fprFdxPackaging;
			dataRow["fprFdxService"] = freightPackageRate.fprFdxService;
			dataRow["fprFdxTimeInTransit"] = freightPackageRate.fprFdxTimeInTransit;
			dataRow["fprFdxTotalBillingWeight"] = freightPackageRate.fprFdxTotalBillingWeight;
			dataRow["fprFdxTotalCustomerCharge"] = freightPackageRate.fprFdxTotalCustomerCharge;
			dataRow["fprFdxTotalDimWeight"] = freightPackageRate.fprFdxTotalDimWeight;
			dataRow["fprFdxTotalFreightDiscount"] = freightPackageRate.fprFdxTotalFreightDiscount;
			dataRow["fprFdxTotalNetCharge"] = freightPackageRate.fprFdxTotalNetCharge;
			dataRow["fprFdxTotalNetFreightCharge"] = freightPackageRate.fprFdxTotalNetFreightCharge;
			dataRow["fprFdxTotalSurcharges"] = freightPackageRate.fprFdxTotalSurcharges;
			dataRow["fprFdxUnits"] = freightPackageRate.fprFdxUnits;
			dataRow["fprFdxVariableHandlingCharge"] = freightPackageRate.fprFdxVariableHandlingCharge;
			dataRow["fprRCTI"] = freightPackageRate.fprRCTI;
			if (freightPackageRate.CustomFields != null && freightPackageRate.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in freightPackageRate.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the FreightPackageRate [{freightPackageRate.fprUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the FreightPackageRate [{freightPackageRate.fprUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
