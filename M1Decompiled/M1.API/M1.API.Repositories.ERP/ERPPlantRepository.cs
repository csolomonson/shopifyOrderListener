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

public class ERPPlantRepository : APIBaseRepository, IERPPlantRepository, IAPIBaseRepository, IDisposable
{
	public ERPPlantRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPlantExist(Guid plantId)
	{
		InitializeParameterLists();
		base.filterList.Add("xauUniqueID|C", plantId);
		base.selectList.Add("xauUniqueID");
		return Task.FromResult(GetAsObject("Plants", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPlantInformationDto>> GetAllPlants(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPlantInformationDto> collection = new List<ERPPlantInformationDto>();
		InitializeParameterLists();
		string[] array = new string[64]
		{
			"xauAccruedCreditorsGlAccountID", "xauAddressLine1", "xauAddressLine2", "xauAddressLine3", "xauApApGlAccountID", "xauApBankAccountID", "xauApCashGlAccountID", "xauApDiscountGlAccountID", "xauApFreightGlAccountID", "xauArArGlAccountID",
			"xauArBankAccountID", "xauArCashGlAccountID", "xauArDepositGlAccountID", "xauArDiscountGlAccountID", "xauArFreightGlAccountID", "xauArSalesGlAccountID", "xauCity", "xauPlantID", "xauCountry", "xauCountryCode",
			"xauCreatedBy", "xauCreatedDate", "xauDayStartTimeFri", "xauDayStartTimeMon", "xauDayStartTimeSat", "xauDayStartTimeSun", "xauDayStartTimeThu", "xauDayStartTimeTue", "xauDayStartTimeWed", "xauEmailAddress",
			"xauUniqueID", "xauEstablishedDate", "xauFaxNumber", "xauFederalID", "xauHoursFri", "xauHoursMon", "xauHoursSat", "xauHoursSun", "xauHoursThu", "xauHoursTue",
			"xauHoursWed", "xauInactiveDate", "xauInactive", "xauAvalaraAddressValidated", "xauUseProperties", "xauLaborClearingGlAccountID", "xauName", "xauOverheadClearingGlAccountID", "xauPhoneNumber", "xauPostCode",
			"xauPurchaseVarianceGlAccountID", "xauRowVersion", "xauShipAwaitInvoiceGlAccountID", "xauState", "xauStockInTransitGlAccountID", "xauStockRevaluationGlAccountID", "xauSVarLaborGlAccountID", "xauSVarMaterialGlAccountID", "xauSVarOverheadGlAccountID", "xauSVarSubcontractGlAccountID",
			"xauWipLaborGlAccountID", "xauWipMaterialGlAccountID", "xauWipoverheadGlAccountID", "xauWipSubcontractGlAccountID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Plants");
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
		using (DataTable dataTable = GetAsDataTable("Plants", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPlantInformationDto eRPPlantInformationDto = new ERPPlantInformationDto();
				eRPPlantInformationDto.xauAccruedCreditorsGlAccountID = dataTable.Rows[i].Field<string>("xauAccruedCreditorsGlAccountID");
				eRPPlantInformationDto.xauAddressLine1 = dataTable.Rows[i].Field<string>("xauAddressLine1");
				eRPPlantInformationDto.xauAddressLine2 = dataTable.Rows[i].Field<string>("xauAddressLine2");
				eRPPlantInformationDto.xauAddressLine3 = dataTable.Rows[i].Field<string>("xauAddressLine3");
				eRPPlantInformationDto.xauApApGlAccountID = dataTable.Rows[i].Field<string>("xauApApGlAccountID");
				eRPPlantInformationDto.xauApBankAccountID = dataTable.Rows[i].Field<string>("xauApBankAccountID");
				eRPPlantInformationDto.xauApCashGlAccountID = dataTable.Rows[i].Field<string>("xauApCashGlAccountID");
				eRPPlantInformationDto.xauApDiscountGlAccountID = dataTable.Rows[i].Field<string>("xauApDiscountGlAccountID");
				eRPPlantInformationDto.xauApFreightGlAccountID = dataTable.Rows[i].Field<string>("xauApFreightGlAccountID");
				eRPPlantInformationDto.xauArArGlAccountID = dataTable.Rows[i].Field<string>("xauArArGlAccountID");
				eRPPlantInformationDto.xauArBankAccountID = dataTable.Rows[i].Field<string>("xauArBankAccountID");
				eRPPlantInformationDto.xauArCashGlAccountID = dataTable.Rows[i].Field<string>("xauArCashGlAccountID");
				eRPPlantInformationDto.xauArDepositGlAccountID = dataTable.Rows[i].Field<string>("xauArDepositGlAccountID");
				eRPPlantInformationDto.xauArDiscountGlAccountID = dataTable.Rows[i].Field<string>("xauArDiscountGlAccountID");
				eRPPlantInformationDto.xauArFreightGlAccountID = dataTable.Rows[i].Field<string>("xauArFreightGlAccountID");
				eRPPlantInformationDto.xauArSalesGlAccountID = dataTable.Rows[i].Field<string>("xauArSalesGlAccountID");
				eRPPlantInformationDto.xauCity = dataTable.Rows[i].Field<string>("xauCity");
				eRPPlantInformationDto.xauPlantID = dataTable.Rows[i].Field<string>("xauPlantID");
				eRPPlantInformationDto.xauCountry = dataTable.Rows[i].Field<string>("xauCountry");
				eRPPlantInformationDto.xauCountryCode = dataTable.Rows[i].Field<string>("xauCountryCode");
				eRPPlantInformationDto.xauCreatedBy = dataTable.Rows[i].Field<string>("xauCreatedBy");
				eRPPlantInformationDto.xauCreatedDate = dataTable.Rows[i].Field<DateTime?>("xauCreatedDate");
				eRPPlantInformationDto.xauDayStartTimeFri = dataTable.Rows[i].Field<decimal>("xauDayStartTimeFri");
				eRPPlantInformationDto.xauDayStartTimeMon = dataTable.Rows[i].Field<decimal>("xauDayStartTimeMon");
				eRPPlantInformationDto.xauDayStartTimeSat = dataTable.Rows[i].Field<decimal>("xauDayStartTimeSat");
				eRPPlantInformationDto.xauDayStartTimeSun = dataTable.Rows[i].Field<decimal>("xauDayStartTimeSun");
				eRPPlantInformationDto.xauDayStartTimeThu = dataTable.Rows[i].Field<decimal>("xauDayStartTimeThu");
				eRPPlantInformationDto.xauDayStartTimeTue = dataTable.Rows[i].Field<decimal>("xauDayStartTimeTue");
				eRPPlantInformationDto.xauDayStartTimeWed = dataTable.Rows[i].Field<decimal>("xauDayStartTimeWed");
				eRPPlantInformationDto.xauEmailAddress = dataTable.Rows[i].Field<string>("xauEmailAddress");
				eRPPlantInformationDto.xauUniqueID = dataTable.Rows[i].Field<Guid>("xauUniqueID");
				eRPPlantInformationDto.xauEstablishedDate = dataTable.Rows[i].Field<DateTime?>("xauEstablishedDate");
				eRPPlantInformationDto.xauFaxNumber = dataTable.Rows[i].Field<string>("xauFaxNumber");
				eRPPlantInformationDto.xauFederalID = dataTable.Rows[i].Field<string>("xauFederalID");
				eRPPlantInformationDto.xauHoursFri = dataTable.Rows[i].Field<decimal>("xauHoursFri");
				eRPPlantInformationDto.xauHoursMon = dataTable.Rows[i].Field<decimal>("xauHoursMon");
				eRPPlantInformationDto.xauHoursSat = dataTable.Rows[i].Field<decimal>("xauHoursSat");
				eRPPlantInformationDto.xauHoursSun = dataTable.Rows[i].Field<decimal>("xauHoursSun");
				eRPPlantInformationDto.xauHoursThu = dataTable.Rows[i].Field<decimal>("xauHoursThu");
				eRPPlantInformationDto.xauHoursTue = dataTable.Rows[i].Field<decimal>("xauHoursTue");
				eRPPlantInformationDto.xauHoursWed = dataTable.Rows[i].Field<decimal>("xauHoursWed");
				eRPPlantInformationDto.xauInactiveDate = dataTable.Rows[i].Field<DateTime?>("xauInactiveDate");
				eRPPlantInformationDto.xauInactive = dataTable.Rows[i].Field<bool>("xauInactive");
				eRPPlantInformationDto.xauAvalaraAddressValidated = dataTable.Rows[i].Field<bool>("xauAvalaraAddressValidated");
				eRPPlantInformationDto.xauUseProperties = dataTable.Rows[i].Field<bool>("xauUseProperties");
				eRPPlantInformationDto.xauLaborClearingGlAccountID = dataTable.Rows[i].Field<string>("xauLaborClearingGlAccountID");
				eRPPlantInformationDto.xauName = dataTable.Rows[i].Field<string>("xauName");
				eRPPlantInformationDto.xauOverheadClearingGlAccountID = dataTable.Rows[i].Field<string>("xauOverheadClearingGlAccountID");
				eRPPlantInformationDto.xauPhoneNumber = dataTable.Rows[i].Field<string>("xauPhoneNumber");
				eRPPlantInformationDto.xauPostCode = dataTable.Rows[i].Field<string>("xauPostCode");
				eRPPlantInformationDto.xauPurchaseVarianceGlAccountID = dataTable.Rows[i].Field<string>("xauPurchaseVarianceGlAccountID");
				eRPPlantInformationDto.xauRowVersion = dataTable.Rows[i].Field<byte[]>("xauRowVersion");
				eRPPlantInformationDto.xauShipAwaitInvoiceGlAccountID = dataTable.Rows[i].Field<string>("xauShipAwaitInvoiceGlAccountID");
				eRPPlantInformationDto.xauState = dataTable.Rows[i].Field<string>("xauState");
				eRPPlantInformationDto.xauStockInTransitGlAccountID = dataTable.Rows[i].Field<string>("xauStockInTransitGlAccountID");
				eRPPlantInformationDto.xauStockRevaluationGlAccountID = dataTable.Rows[i].Field<string>("xauStockRevaluationGlAccountID");
				eRPPlantInformationDto.xauSVarLaborGlAccountID = dataTable.Rows[i].Field<string>("xauSVarLaborGlAccountID");
				eRPPlantInformationDto.xauSVarMaterialGlAccountID = dataTable.Rows[i].Field<string>("xauSVarMaterialGlAccountID");
				eRPPlantInformationDto.xauSVarOverheadGlAccountID = dataTable.Rows[i].Field<string>("xauSVarOverheadGlAccountID");
				eRPPlantInformationDto.xauSVarSubcontractGlAccountID = dataTable.Rows[i].Field<string>("xauSVarSubcontractGlAccountID");
				eRPPlantInformationDto.xauWipLaborGlAccountID = dataTable.Rows[i].Field<string>("xauWipLaborGlAccountID");
				eRPPlantInformationDto.xauWipMaterialGlAccountID = dataTable.Rows[i].Field<string>("xauWipMaterialGlAccountID");
				eRPPlantInformationDto.xauWipoverheadGlAccountID = dataTable.Rows[i].Field<string>("xauWipoverheadGlAccountID");
				eRPPlantInformationDto.xauWipSubcontractGlAccountID = dataTable.Rows[i].Field<string>("xauWipSubcontractGlAccountID");
				eRPPlantInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPlantInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPlantInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPlantInformationDto> GetPlant(Guid plantId)
	{
		ERPPlantInformationDto eRPPlantInformationDto = new ERPPlantInformationDto();
		InitializeParameterLists();
		string[] collection = new string[64]
		{
			"xauAccruedCreditorsGlAccountID", "xauAddressLine1", "xauAddressLine2", "xauAddressLine3", "xauApApGlAccountID", "xauApBankAccountID", "xauApCashGlAccountID", "xauApDiscountGlAccountID", "xauApFreightGlAccountID", "xauArArGlAccountID",
			"xauArBankAccountID", "xauArCashGlAccountID", "xauArDepositGlAccountID", "xauArDiscountGlAccountID", "xauArFreightGlAccountID", "xauArSalesGlAccountID", "xauCity", "xauPlantID", "xauCountry", "xauCountryCode",
			"xauCreatedBy", "xauCreatedDate", "xauDayStartTimeFri", "xauDayStartTimeMon", "xauDayStartTimeSat", "xauDayStartTimeSun", "xauDayStartTimeThu", "xauDayStartTimeTue", "xauDayStartTimeWed", "xauEmailAddress",
			"xauUniqueID", "xauEstablishedDate", "xauFaxNumber", "xauFederalID", "xauHoursFri", "xauHoursMon", "xauHoursSat", "xauHoursSun", "xauHoursThu", "xauHoursTue",
			"xauHoursWed", "xauInactiveDate", "xauInactive", "xauAvalaraAddressValidated", "xauUseProperties", "xauLaborClearingGlAccountID", "xauName", "xauOverheadClearingGlAccountID", "xauPhoneNumber", "xauPostCode",
			"xauPurchaseVarianceGlAccountID", "xauRowVersion", "xauShipAwaitInvoiceGlAccountID", "xauState", "xauStockInTransitGlAccountID", "xauStockRevaluationGlAccountID", "xauSVarLaborGlAccountID", "xauSVarMaterialGlAccountID", "xauSVarOverheadGlAccountID", "xauSVarSubcontractGlAccountID",
			"xauWipLaborGlAccountID", "xauWipMaterialGlAccountID", "xauWipoverheadGlAccountID", "xauWipSubcontractGlAccountID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xauUniqueID|C", plantId);
		AddCustomFieldsToSelectList("Plants");
		using (DataTable dataTable = GetAsDataTable("Plants", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPlantInformationDto);
			}
			eRPPlantInformationDto.xauAccruedCreditorsGlAccountID = dataTable.Rows[0].Field<string>("xauAccruedCreditorsGlAccountID");
			eRPPlantInformationDto.xauAddressLine1 = dataTable.Rows[0].Field<string>("xauAddressLine1");
			eRPPlantInformationDto.xauAddressLine2 = dataTable.Rows[0].Field<string>("xauAddressLine2");
			eRPPlantInformationDto.xauAddressLine3 = dataTable.Rows[0].Field<string>("xauAddressLine3");
			eRPPlantInformationDto.xauApApGlAccountID = dataTable.Rows[0].Field<string>("xauApApGlAccountID");
			eRPPlantInformationDto.xauApBankAccountID = dataTable.Rows[0].Field<string>("xauApBankAccountID");
			eRPPlantInformationDto.xauApCashGlAccountID = dataTable.Rows[0].Field<string>("xauApCashGlAccountID");
			eRPPlantInformationDto.xauApDiscountGlAccountID = dataTable.Rows[0].Field<string>("xauApDiscountGlAccountID");
			eRPPlantInformationDto.xauApFreightGlAccountID = dataTable.Rows[0].Field<string>("xauApFreightGlAccountID");
			eRPPlantInformationDto.xauArArGlAccountID = dataTable.Rows[0].Field<string>("xauArArGlAccountID");
			eRPPlantInformationDto.xauArBankAccountID = dataTable.Rows[0].Field<string>("xauArBankAccountID");
			eRPPlantInformationDto.xauArCashGlAccountID = dataTable.Rows[0].Field<string>("xauArCashGlAccountID");
			eRPPlantInformationDto.xauArDepositGlAccountID = dataTable.Rows[0].Field<string>("xauArDepositGlAccountID");
			eRPPlantInformationDto.xauArDiscountGlAccountID = dataTable.Rows[0].Field<string>("xauArDiscountGlAccountID");
			eRPPlantInformationDto.xauArFreightGlAccountID = dataTable.Rows[0].Field<string>("xauArFreightGlAccountID");
			eRPPlantInformationDto.xauArSalesGlAccountID = dataTable.Rows[0].Field<string>("xauArSalesGlAccountID");
			eRPPlantInformationDto.xauCity = dataTable.Rows[0].Field<string>("xauCity");
			eRPPlantInformationDto.xauPlantID = dataTable.Rows[0].Field<string>("xauPlantID");
			eRPPlantInformationDto.xauCountry = dataTable.Rows[0].Field<string>("xauCountry");
			eRPPlantInformationDto.xauCountryCode = dataTable.Rows[0].Field<string>("xauCountryCode");
			eRPPlantInformationDto.xauCreatedBy = dataTable.Rows[0].Field<string>("xauCreatedBy");
			eRPPlantInformationDto.xauCreatedDate = dataTable.Rows[0].Field<DateTime?>("xauCreatedDate");
			eRPPlantInformationDto.xauDayStartTimeFri = dataTable.Rows[0].Field<decimal>("xauDayStartTimeFri");
			eRPPlantInformationDto.xauDayStartTimeMon = dataTable.Rows[0].Field<decimal>("xauDayStartTimeMon");
			eRPPlantInformationDto.xauDayStartTimeSat = dataTable.Rows[0].Field<decimal>("xauDayStartTimeSat");
			eRPPlantInformationDto.xauDayStartTimeSun = dataTable.Rows[0].Field<decimal>("xauDayStartTimeSun");
			eRPPlantInformationDto.xauDayStartTimeThu = dataTable.Rows[0].Field<decimal>("xauDayStartTimeThu");
			eRPPlantInformationDto.xauDayStartTimeTue = dataTable.Rows[0].Field<decimal>("xauDayStartTimeTue");
			eRPPlantInformationDto.xauDayStartTimeWed = dataTable.Rows[0].Field<decimal>("xauDayStartTimeWed");
			eRPPlantInformationDto.xauEmailAddress = dataTable.Rows[0].Field<string>("xauEmailAddress");
			eRPPlantInformationDto.xauUniqueID = dataTable.Rows[0].Field<Guid>("xauUniqueID");
			eRPPlantInformationDto.xauEstablishedDate = dataTable.Rows[0].Field<DateTime?>("xauEstablishedDate");
			eRPPlantInformationDto.xauFaxNumber = dataTable.Rows[0].Field<string>("xauFaxNumber");
			eRPPlantInformationDto.xauFederalID = dataTable.Rows[0].Field<string>("xauFederalID");
			eRPPlantInformationDto.xauHoursFri = dataTable.Rows[0].Field<decimal>("xauHoursFri");
			eRPPlantInformationDto.xauHoursMon = dataTable.Rows[0].Field<decimal>("xauHoursMon");
			eRPPlantInformationDto.xauHoursSat = dataTable.Rows[0].Field<decimal>("xauHoursSat");
			eRPPlantInformationDto.xauHoursSun = dataTable.Rows[0].Field<decimal>("xauHoursSun");
			eRPPlantInformationDto.xauHoursThu = dataTable.Rows[0].Field<decimal>("xauHoursThu");
			eRPPlantInformationDto.xauHoursTue = dataTable.Rows[0].Field<decimal>("xauHoursTue");
			eRPPlantInformationDto.xauHoursWed = dataTable.Rows[0].Field<decimal>("xauHoursWed");
			eRPPlantInformationDto.xauInactiveDate = dataTable.Rows[0].Field<DateTime?>("xauInactiveDate");
			eRPPlantInformationDto.xauInactive = dataTable.Rows[0].Field<bool>("xauInactive");
			eRPPlantInformationDto.xauAvalaraAddressValidated = dataTable.Rows[0].Field<bool>("xauAvalaraAddressValidated");
			eRPPlantInformationDto.xauUseProperties = dataTable.Rows[0].Field<bool>("xauUseProperties");
			eRPPlantInformationDto.xauLaborClearingGlAccountID = dataTable.Rows[0].Field<string>("xauLaborClearingGlAccountID");
			eRPPlantInformationDto.xauName = dataTable.Rows[0].Field<string>("xauName");
			eRPPlantInformationDto.xauOverheadClearingGlAccountID = dataTable.Rows[0].Field<string>("xauOverheadClearingGlAccountID");
			eRPPlantInformationDto.xauPhoneNumber = dataTable.Rows[0].Field<string>("xauPhoneNumber");
			eRPPlantInformationDto.xauPostCode = dataTable.Rows[0].Field<string>("xauPostCode");
			eRPPlantInformationDto.xauPurchaseVarianceGlAccountID = dataTable.Rows[0].Field<string>("xauPurchaseVarianceGlAccountID");
			eRPPlantInformationDto.xauRowVersion = dataTable.Rows[0].Field<byte[]>("xauRowVersion");
			eRPPlantInformationDto.xauShipAwaitInvoiceGlAccountID = dataTable.Rows[0].Field<string>("xauShipAwaitInvoiceGlAccountID");
			eRPPlantInformationDto.xauState = dataTable.Rows[0].Field<string>("xauState");
			eRPPlantInformationDto.xauStockInTransitGlAccountID = dataTable.Rows[0].Field<string>("xauStockInTransitGlAccountID");
			eRPPlantInformationDto.xauStockRevaluationGlAccountID = dataTable.Rows[0].Field<string>("xauStockRevaluationGlAccountID");
			eRPPlantInformationDto.xauSVarLaborGlAccountID = dataTable.Rows[0].Field<string>("xauSVarLaborGlAccountID");
			eRPPlantInformationDto.xauSVarMaterialGlAccountID = dataTable.Rows[0].Field<string>("xauSVarMaterialGlAccountID");
			eRPPlantInformationDto.xauSVarOverheadGlAccountID = dataTable.Rows[0].Field<string>("xauSVarOverheadGlAccountID");
			eRPPlantInformationDto.xauSVarSubcontractGlAccountID = dataTable.Rows[0].Field<string>("xauSVarSubcontractGlAccountID");
			eRPPlantInformationDto.xauWipLaborGlAccountID = dataTable.Rows[0].Field<string>("xauWipLaborGlAccountID");
			eRPPlantInformationDto.xauWipMaterialGlAccountID = dataTable.Rows[0].Field<string>("xauWipMaterialGlAccountID");
			eRPPlantInformationDto.xauWipoverheadGlAccountID = dataTable.Rows[0].Field<string>("xauWipoverheadGlAccountID");
			eRPPlantInformationDto.xauWipSubcontractGlAccountID = dataTable.Rows[0].Field<string>("xauWipSubcontractGlAccountID");
			eRPPlantInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPlantInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPlantInformationDto);
	}

	public Task<APIValidationInfoDto> SavePlant(ERPPlantDto plant)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Plants WHERE xauUniqueID = " + M1Util.ConvertToLinq(plant.xauUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xauPlantID"] = plant.xauPlantID.ToUpper();
				plant.xauUniqueID = ((plant.xauUniqueID == Guid.Empty) ? Guid.NewGuid() : plant.xauUniqueID);
				dataRow["xauUniqueID"] = plant.xauUniqueID;
				dataRow["xauCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xauCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Plant could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (plant.xauRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Plant is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xauRowVersion"], plant.xauRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Plant has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Plant again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xauAccruedCreditorsGlAccountID"] = plant.xauAccruedCreditorsGlAccountID;
			dataRow["xauAddressLine1"] = plant.xauAddressLine1;
			dataRow["xauAddressLine2"] = plant.xauAddressLine2;
			dataRow["xauAddressLine3"] = plant.xauAddressLine3;
			dataRow["xauApApGlAccountID"] = plant.xauApApGlAccountID;
			dataRow["xauApBankAccountID"] = plant.xauApBankAccountID;
			dataRow["xauApCashGlAccountID"] = plant.xauApCashGlAccountID;
			dataRow["xauApDiscountGlAccountID"] = plant.xauApDiscountGlAccountID;
			dataRow["xauApFreightGlAccountID"] = plant.xauApFreightGlAccountID;
			dataRow["xauArArGlAccountID"] = plant.xauArArGlAccountID;
			dataRow["xauArBankAccountID"] = plant.xauArBankAccountID;
			dataRow["xauArCashGlAccountID"] = plant.xauArCashGlAccountID;
			dataRow["xauArDepositGlAccountID"] = plant.xauArDepositGlAccountID;
			dataRow["xauArDiscountGlAccountID"] = plant.xauArDiscountGlAccountID;
			dataRow["xauArFreightGlAccountID"] = plant.xauArFreightGlAccountID;
			dataRow["xauArSalesGlAccountID"] = plant.xauArSalesGlAccountID;
			dataRow["xauCity"] = plant.xauCity;
			dataRow["xauCountry"] = plant.xauCountry;
			dataRow["xauCountryCode"] = plant.xauCountryCode;
			dataRow["xauDayStartTimeFri"] = plant.xauDayStartTimeFri;
			dataRow["xauDayStartTimeMon"] = plant.xauDayStartTimeMon;
			dataRow["xauDayStartTimeSat"] = plant.xauDayStartTimeSat;
			dataRow["xauDayStartTimeSun"] = plant.xauDayStartTimeSun;
			dataRow["xauDayStartTimeThu"] = plant.xauDayStartTimeThu;
			dataRow["xauDayStartTimeTue"] = plant.xauDayStartTimeTue;
			dataRow["xauDayStartTimeWed"] = plant.xauDayStartTimeWed;
			dataRow["xauEmailAddress"] = plant.xauEmailAddress ?? dataRow["xauEmailAddress"];
			DataRow dataRow2 = dataRow;
			DateTime? xauEstablishedDate = plant.xauEstablishedDate;
			dataRow2["xauEstablishedDate"] = (xauEstablishedDate.HasValue ? ((object)xauEstablishedDate.GetValueOrDefault()) : dataRow["xauEstablishedDate"]);
			dataRow["xauFaxNumber"] = plant.xauFaxNumber;
			dataRow["xauFederalID"] = plant.xauFederalID;
			dataRow["xauHoursFri"] = plant.xauHoursFri;
			dataRow["xauHoursMon"] = plant.xauHoursMon;
			dataRow["xauHoursSat"] = plant.xauHoursSat;
			dataRow["xauHoursSun"] = plant.xauHoursSun;
			dataRow["xauHoursThu"] = plant.xauHoursThu;
			dataRow["xauHoursTue"] = plant.xauHoursTue;
			dataRow["xauHoursWed"] = plant.xauHoursWed;
			DataRow dataRow3 = dataRow;
			xauEstablishedDate = plant.xauInactiveDate;
			dataRow3["xauInactiveDate"] = (xauEstablishedDate.HasValue ? ((object)xauEstablishedDate.GetValueOrDefault()) : dataRow["xauInactiveDate"]);
			dataRow["xauInactive"] = plant.xauInactive;
			dataRow["xauAvalaraAddressValidated"] = plant.xauAvalaraAddressValidated;
			dataRow["xauUseProperties"] = plant.xauUseProperties;
			dataRow["xauLaborClearingGlAccountID"] = plant.xauLaborClearingGlAccountID;
			dataRow["xauName"] = plant.xauName;
			dataRow["xauOverheadClearingGlAccountID"] = plant.xauOverheadClearingGlAccountID;
			dataRow["xauPhoneNumber"] = plant.xauPhoneNumber;
			dataRow["xauPostCode"] = plant.xauPostCode;
			dataRow["xauPurchaseVarianceGlAccountID"] = plant.xauPurchaseVarianceGlAccountID;
			dataRow["xauShipAwaitInvoiceGlAccountID"] = plant.xauShipAwaitInvoiceGlAccountID;
			dataRow["xauState"] = plant.xauState;
			dataRow["xauStockInTransitGlAccountID"] = plant.xauStockInTransitGlAccountID;
			dataRow["xauStockRevaluationGlAccountID"] = plant.xauStockRevaluationGlAccountID;
			dataRow["xauSVarLaborGlAccountID"] = plant.xauSVarLaborGlAccountID;
			dataRow["xauSVarMaterialGlAccountID"] = plant.xauSVarMaterialGlAccountID;
			dataRow["xauSVarOverheadGlAccountID"] = plant.xauSVarOverheadGlAccountID;
			dataRow["xauSVarSubcontractGlAccountID"] = plant.xauSVarSubcontractGlAccountID;
			dataRow["xauWipLaborGlAccountID"] = plant.xauWipLaborGlAccountID;
			dataRow["xauWipMaterialGlAccountID"] = plant.xauWipMaterialGlAccountID;
			dataRow["xauWipoverheadGlAccountID"] = plant.xauWipoverheadGlAccountID;
			dataRow["xauWipSubcontractGlAccountID"] = plant.xauWipSubcontractGlAccountID;
			if (plant.CustomFields != null && plant.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in plant.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Plant [{plant.xauUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Plant [{plant.xauUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
