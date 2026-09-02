using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPFinancialPropertyRepository : APIBaseRepository, IERPFinancialPropertyRepository, IAPIBaseRepository, IDisposable
{
	public ERPFinancialPropertyRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesFinancialPropertyExist(Guid financialPropertyId)
	{
		InitializeParameterLists();
		base.filterList.Add("xafUniqueID|C", financialPropertyId);
		base.selectList.Add("xafUniqueID");
		return Task.FromResult(GetAsObject("FinancialProperties", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPFinancialPropertyInformationDto>> GetAllFinancialProperties(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPFinancialPropertyInformationDto> collection = new List<ERPFinancialPropertyInformationDto>();
		InitializeParameterLists();
		string[] array = new string[128]
		{
			"xafAccruedCreditorsGlAccountID", "xafAgingMethod", "xafApAgingBucketID", "xafApApCostStartDate", "xafApApGlAccountID", "xafApCashGlAccountID", "xafApDiscountGlAccountID", "xafApFreightGlAccountID", "xafApGroupReceiptsBySupplier", "xafApPaymentMaxLinesPerPage",
			"xafArAgingBucketID", "xafArArGlAccountID", "xafArCashGlAccountID", "xafArDefaultLaborPartGroupID", "xafArDepositGlAccountID", "xafArDepositPartID", "xafArDepositPartRevisionID", "xafArDiscountGlAccountID", "xafArFinanceChargeGlAccountID", "xafArFinanceChargeGraceDays",
			"xafArFinanceChargeLastRunDate", "xafArFinanceChargePercent", "xafArFinanceShowCreditBalance", "xafArFreightGlAccountID", "xafArGroupShipmentsByCustomer", "xafArLaborPartID", "xafArLaborPartRevisionID", "xafArNET1GatewayID", "xafArNET1MerchantKey", "xafArNET1Port",
			"xafArNET1TimeoutSeconds", "xafArShowDeposits", "xafAvalaraAccountID", "xafAvalaraArInvoicePostOption", "xafAvalaraCanadaGstTaxCodeID", "xafAvalaraCanadaHSTTaxCodeID", "xafAvalaraCanadaPSTTaxCodeID", "xafAvalaraCanadaQSTTaxCodeID", "xafAvalaraCompanyCode", "xafAvalaraFilterCountry",
			"xafAvalaraLicenseKey", "xafAvalaraTaxCodeID", "xafAvalaraTimeoutSeconds", "xafAvalaraURL", "xafCAEmployerDentalBenefits", "xafCogsStatusHistory", "xafCogsUseAccounts", "xafCreatedBy", "xafCreatedDate", "xafCreditCardMethod",
			"xafDrawerCashGlAccountID", "xafDrawerCashStartAmount", "xafUniqueID", "xafGlFiscalYearID", "xafGlFiscalYearPeriodID", "xafGlRetainedEarningsAccountID", "xafAgeByDaysInMonth", "xafApAllowParentAccountPost", "xafApAlwaysTakeDiscount", "xafApAssignNumbersToEft",
			"xafApCreditUpdatesReceipt", "xafApDisableTaxFields", "xafApDiscountOnFreight", "xafApDiscountOnTax", "xafApExpressPost", "xafApIncludeTaxInExpAmt", "xafApPaymentFilterPlant", "xafApTaxOnFreight", "xafApUpdateJobCosts", "xafArAllowParentAccountPost",
			"xafArCalculateTaxOnDeposit", "xafArCreateDiscountJournals", "xafArCreditUpdatesShipment", "xafArDisableTaxFields", "xafArDiscountOnFreight", "xafArExpressPost", "xafArIncludeFrgtInDepositCalc", "xafArIncludeTaxInDepositCalc", "xafArPaymentFilterPlant", "xafArTaxOnFreight",
			"xafAvalaraDisableAddrValidate", "xafAvalaraDisableIgnoreLine", "xafAvalaraForceAddressValidate", "xafCreateBankEntries", "xafDisableMultiplePlants", "xafExactDaysInPaymentTerms", "xafFAroundToNearestDollar", "xafGlCreateStockJournals", "xafGlExpressPost", "xafIncludeLLInTermination",
			"xafPAAllowParentAccountPost", "xafPAAssignNumbersToEft", "xafPADeleteZeroPayHeaders", "xafPAExpressPost", "xafPartsMustExist", "xafPAShowHolidaysForSalary", "xafProductionExpressPost", "xafRecalcSalarySacrifice", "xafStpSetGrossPayAsETP", "xafLaborClearingGlAccountID",
			"xafMiscReceiptVarianceAccount", "xafOverheadClearingGlAccountID", "xafPALeaveBalanceCheck", "xafPAUseDate", "xafPurchaseVarianceGlAccountID", "xafRoundingGlAccountID", "xafRowVersion", "xafShipAwaitInvoiceGlAccountID", "xafStockInTransitGlAccountID", "xafStockRevaluationGlAccountID",
			"xafStoreCreditGlAccountID", "xafSuperEmployerID", "xafSuperEndDate", "xafSuperExportDateFormat", "xafSuperExportFilePath", "xafSuperStartDate", "xafSVarLaborGlAccountID", "xafSVarMaterialGlAccountID", "xafSVarOverheadGlAccountID", "xafSVarSubcontractGlAccountID",
			"xafTaxOnReportMethod", "xafTestFileCode", "xafTransmitterControlCode", "xafUS1094FileLocation", "xafWipLaborGlAccountID", "xafWipMaterialGlAccountID", "xafWipoverheadGlAccountID", "xafWipSubcontractGlAccountID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("FinancialProperties");
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
		using (DataTable dataTable = GetAsDataTable("FinancialProperties", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPFinancialPropertyInformationDto eRPFinancialPropertyInformationDto = new ERPFinancialPropertyInformationDto();
				eRPFinancialPropertyInformationDto.xafAccruedCreditorsGlAccountID = dataTable.Rows[i].Field<string>("xafAccruedCreditorsGlAccountID");
				eRPFinancialPropertyInformationDto.xafAgingMethod = dataTable.Rows[i].Field<byte>("xafAgingMethod");
				eRPFinancialPropertyInformationDto.xafApAgingBucketID = dataTable.Rows[i].Field<string>("xafApAgingBucketID");
				eRPFinancialPropertyInformationDto.xafApApCostStartDate = dataTable.Rows[i].Field<DateTime?>("xafApApCostStartDate");
				eRPFinancialPropertyInformationDto.xafApApGlAccountID = dataTable.Rows[i].Field<string>("xafApApGlAccountID");
				eRPFinancialPropertyInformationDto.xafApCashGlAccountID = dataTable.Rows[i].Field<string>("xafApCashGlAccountID");
				eRPFinancialPropertyInformationDto.xafApDiscountGlAccountID = dataTable.Rows[i].Field<string>("xafApDiscountGlAccountID");
				eRPFinancialPropertyInformationDto.xafApFreightGlAccountID = dataTable.Rows[i].Field<string>("xafApFreightGlAccountID");
				eRPFinancialPropertyInformationDto.xafApGroupReceiptsBySupplier = dataTable.Rows[i].Field<byte>("xafApGroupReceiptsBySupplier");
				eRPFinancialPropertyInformationDto.xafApPaymentMaxLinesPerPage = dataTable.Rows[i].Field<byte>("xafApPaymentMaxLinesPerPage");
				eRPFinancialPropertyInformationDto.xafArAgingBucketID = dataTable.Rows[i].Field<string>("xafArAgingBucketID");
				eRPFinancialPropertyInformationDto.xafArArGlAccountID = dataTable.Rows[i].Field<string>("xafArArGlAccountID");
				eRPFinancialPropertyInformationDto.xafArCashGlAccountID = dataTable.Rows[i].Field<string>("xafArCashGlAccountID");
				eRPFinancialPropertyInformationDto.xafArDefaultLaborPartGroupID = dataTable.Rows[i].Field<string>("xafArDefaultLaborPartGroupID");
				eRPFinancialPropertyInformationDto.xafArDepositGlAccountID = dataTable.Rows[i].Field<string>("xafArDepositGlAccountID");
				eRPFinancialPropertyInformationDto.xafArDepositPartID = dataTable.Rows[i].Field<string>("xafArDepositPartID");
				eRPFinancialPropertyInformationDto.xafArDepositPartRevisionID = dataTable.Rows[i].Field<string>("xafArDepositPartRevisionID");
				eRPFinancialPropertyInformationDto.xafArDiscountGlAccountID = dataTable.Rows[i].Field<string>("xafArDiscountGlAccountID");
				eRPFinancialPropertyInformationDto.xafArFinanceChargeGlAccountID = dataTable.Rows[i].Field<string>("xafArFinanceChargeGlAccountID");
				eRPFinancialPropertyInformationDto.xafArFinanceChargeGraceDays = dataTable.Rows[i].Field<short>("xafArFinanceChargeGraceDays");
				eRPFinancialPropertyInformationDto.xafArFinanceChargeLastRunDate = dataTable.Rows[i].Field<DateTime?>("xafArFinanceChargeLastRunDate");
				eRPFinancialPropertyInformationDto.xafArFinanceChargePercent = dataTable.Rows[i].Field<decimal>("xafArFinanceChargePercent");
				eRPFinancialPropertyInformationDto.xafArFinanceShowCreditBalance = dataTable.Rows[i].Field<byte>("xafArFinanceShowCreditBalance");
				eRPFinancialPropertyInformationDto.xafArFreightGlAccountID = dataTable.Rows[i].Field<string>("xafArFreightGlAccountID");
				eRPFinancialPropertyInformationDto.xafArGroupShipmentsByCustomer = dataTable.Rows[i].Field<byte>("xafArGroupShipmentsByCustomer");
				eRPFinancialPropertyInformationDto.xafArLaborPartID = dataTable.Rows[i].Field<string>("xafArLaborPartID");
				eRPFinancialPropertyInformationDto.xafArLaborPartRevisionID = dataTable.Rows[i].Field<string>("xafArLaborPartRevisionID");
				eRPFinancialPropertyInformationDto.xafArNET1GatewayID = dataTable.Rows[i].Field<string>("xafArNET1GatewayID");
				eRPFinancialPropertyInformationDto.xafArNET1MerchantKey = dataTable.Rows[i].Field<string>("xafArNET1MerchantKey");
				eRPFinancialPropertyInformationDto.xafArNET1Port = dataTable.Rows[i].Field<int>("xafArNET1Port");
				eRPFinancialPropertyInformationDto.xafArNET1TimeoutSeconds = dataTable.Rows[i].Field<short>("xafArNET1TimeoutSeconds");
				eRPFinancialPropertyInformationDto.xafArShowDeposits = dataTable.Rows[i].Field<byte>("xafArShowDeposits");
				eRPFinancialPropertyInformationDto.xafAvalaraAccountID = dataTable.Rows[i].Field<string>("xafAvalaraAccountID");
				eRPFinancialPropertyInformationDto.xafAvalaraArInvoicePostOption = dataTable.Rows[i].Field<byte>("xafAvalaraArInvoicePostOption");
				eRPFinancialPropertyInformationDto.xafAvalaraCanadaGstTaxCodeID = dataTable.Rows[i].Field<string>("xafAvalaraCanadaGstTaxCodeID");
				eRPFinancialPropertyInformationDto.xafAvalaraCanadaHSTTaxCodeID = dataTable.Rows[i].Field<string>("xafAvalaraCanadaHSTTaxCodeID");
				eRPFinancialPropertyInformationDto.xafAvalaraCanadaPSTTaxCodeID = dataTable.Rows[i].Field<string>("xafAvalaraCanadaPSTTaxCodeID");
				eRPFinancialPropertyInformationDto.xafAvalaraCanadaQSTTaxCodeID = dataTable.Rows[i].Field<string>("xafAvalaraCanadaQSTTaxCodeID");
				eRPFinancialPropertyInformationDto.xafAvalaraCompanyCode = dataTable.Rows[i].Field<string>("xafAvalaraCompanyCode");
				eRPFinancialPropertyInformationDto.xafAvalaraFilterCountry = dataTable.Rows[i].Field<byte>("xafAvalaraFilterCountry");
				eRPFinancialPropertyInformationDto.xafAvalaraLicenseKey = dataTable.Rows[i].Field<string>("xafAvalaraLicenseKey");
				eRPFinancialPropertyInformationDto.xafAvalaraTaxCodeID = dataTable.Rows[i].Field<string>("xafAvalaraTaxCodeID");
				eRPFinancialPropertyInformationDto.xafAvalaraTimeoutSeconds = dataTable.Rows[i].Field<short>("xafAvalaraTimeoutSeconds");
				eRPFinancialPropertyInformationDto.xafAvalaraURL = dataTable.Rows[i].Field<string>("xafAvalaraURL");
				eRPFinancialPropertyInformationDto.xafCAEmployerDentalBenefits = dataTable.Rows[i].Field<byte>("xafCAEmployerDentalBenefits");
				eRPFinancialPropertyInformationDto.xafCogsStatusHistory = dataTable.Rows[i].Field<string>("xafCogsStatusHistory");
				eRPFinancialPropertyInformationDto.xafCogsUseAccounts = dataTable.Rows[i].Field<byte>("xafCogsUseAccounts");
				eRPFinancialPropertyInformationDto.xafCreatedBy = dataTable.Rows[i].Field<string>("xafCreatedBy");
				eRPFinancialPropertyInformationDto.xafCreatedDate = dataTable.Rows[i].Field<DateTime?>("xafCreatedDate");
				eRPFinancialPropertyInformationDto.xafCreditCardMethod = dataTable.Rows[i].Field<byte>("xafCreditCardMethod");
				eRPFinancialPropertyInformationDto.xafDrawerCashGlAccountID = dataTable.Rows[i].Field<string>("xafDrawerCashGlAccountID");
				eRPFinancialPropertyInformationDto.xafDrawerCashStartAmount = dataTable.Rows[i].Field<decimal>("xafDrawerCashStartAmount");
				eRPFinancialPropertyInformationDto.xafUniqueID = dataTable.Rows[i].Field<Guid>("xafUniqueID");
				eRPFinancialPropertyInformationDto.xafGlFiscalYearID = dataTable.Rows[i].Field<short>("xafGlFiscalYearID");
				eRPFinancialPropertyInformationDto.xafGlFiscalYearPeriodID = dataTable.Rows[i].Field<byte>("xafGlFiscalYearPeriodID");
				eRPFinancialPropertyInformationDto.xafGlRetainedEarningsAccountID = dataTable.Rows[i].Field<string>("xafGlRetainedEarningsAccountID");
				eRPFinancialPropertyInformationDto.xafAgeByDaysInMonth = dataTable.Rows[i].Field<bool>("xafAgeByDaysInMonth");
				eRPFinancialPropertyInformationDto.xafApAllowParentAccountPost = dataTable.Rows[i].Field<bool>("xafApAllowParentAccountPost");
				eRPFinancialPropertyInformationDto.xafApAlwaysTakeDiscount = dataTable.Rows[i].Field<bool>("xafApAlwaysTakeDiscount");
				eRPFinancialPropertyInformationDto.xafApAssignNumbersToEft = dataTable.Rows[i].Field<bool>("xafApAssignNumbersToEft");
				eRPFinancialPropertyInformationDto.xafApCreditUpdatesReceipt = dataTable.Rows[i].Field<bool>("xafApCreditUpdatesReceipt");
				eRPFinancialPropertyInformationDto.xafApDisableTaxFields = dataTable.Rows[i].Field<bool>("xafApDisableTaxFields");
				eRPFinancialPropertyInformationDto.xafApDiscountOnFreight = dataTable.Rows[i].Field<bool>("xafApDiscountOnFreight");
				eRPFinancialPropertyInformationDto.xafApDiscountOnTax = dataTable.Rows[i].Field<bool>("xafApDiscountOnTax");
				eRPFinancialPropertyInformationDto.xafApExpressPost = dataTable.Rows[i].Field<bool>("xafApExpressPost");
				eRPFinancialPropertyInformationDto.xafApIncludeTaxInExpAmt = dataTable.Rows[i].Field<bool>("xafApIncludeTaxInExpAmt");
				eRPFinancialPropertyInformationDto.xafApPaymentFilterPlant = dataTable.Rows[i].Field<bool>("xafApPaymentFilterPlant");
				eRPFinancialPropertyInformationDto.xafApTaxOnFreight = dataTable.Rows[i].Field<bool>("xafApTaxOnFreight");
				eRPFinancialPropertyInformationDto.xafApUpdateJobCosts = dataTable.Rows[i].Field<bool>("xafApUpdateJobCosts");
				eRPFinancialPropertyInformationDto.xafArAllowParentAccountPost = dataTable.Rows[i].Field<bool>("xafArAllowParentAccountPost");
				eRPFinancialPropertyInformationDto.xafArCalculateTaxOnDeposit = dataTable.Rows[i].Field<bool>("xafArCalculateTaxOnDeposit");
				eRPFinancialPropertyInformationDto.xafArCreateDiscountJournals = dataTable.Rows[i].Field<bool>("xafArCreateDiscountJournals");
				eRPFinancialPropertyInformationDto.xafArCreditUpdatesShipment = dataTable.Rows[i].Field<bool>("xafArCreditUpdatesShipment");
				eRPFinancialPropertyInformationDto.xafArDisableTaxFields = dataTable.Rows[i].Field<bool>("xafArDisableTaxFields");
				eRPFinancialPropertyInformationDto.xafArDiscountOnFreight = dataTable.Rows[i].Field<bool>("xafArDiscountOnFreight");
				eRPFinancialPropertyInformationDto.xafArExpressPost = dataTable.Rows[i].Field<bool>("xafArExpressPost");
				eRPFinancialPropertyInformationDto.xafArIncludeFrgtInDepositCalc = dataTable.Rows[i].Field<bool>("xafArIncludeFrgtInDepositCalc");
				eRPFinancialPropertyInformationDto.xafArIncludeTaxInDepositCalc = dataTable.Rows[i].Field<bool>("xafArIncludeTaxInDepositCalc");
				eRPFinancialPropertyInformationDto.xafArPaymentFilterPlant = dataTable.Rows[i].Field<bool>("xafArPaymentFilterPlant");
				eRPFinancialPropertyInformationDto.xafArTaxOnFreight = dataTable.Rows[i].Field<bool>("xafArTaxOnFreight");
				eRPFinancialPropertyInformationDto.xafAvalaraDisableAddrValidate = dataTable.Rows[i].Field<bool>("xafAvalaraDisableAddrValidate");
				eRPFinancialPropertyInformationDto.xafAvalaraDisableIgnoreLine = dataTable.Rows[i].Field<bool>("xafAvalaraDisableIgnoreLine");
				eRPFinancialPropertyInformationDto.xafAvalaraForceAddressValidate = dataTable.Rows[i].Field<bool>("xafAvalaraForceAddressValidate");
				eRPFinancialPropertyInformationDto.xafCreateBankEntries = dataTable.Rows[i].Field<bool>("xafCreateBankEntries");
				eRPFinancialPropertyInformationDto.xafDisableMultiplePlants = dataTable.Rows[i].Field<bool>("xafDisableMultiplePlants");
				eRPFinancialPropertyInformationDto.xafExactDaysInPaymentTerms = dataTable.Rows[i].Field<bool>("xafExactDaysInPaymentTerms");
				eRPFinancialPropertyInformationDto.xafFAroundToNearestDollar = dataTable.Rows[i].Field<bool>("xafFAroundToNearestDollar");
				eRPFinancialPropertyInformationDto.xafGlCreateStockJournals = dataTable.Rows[i].Field<bool>("xafGlCreateStockJournals");
				eRPFinancialPropertyInformationDto.xafGlExpressPost = dataTable.Rows[i].Field<bool>("xafGlExpressPost");
				eRPFinancialPropertyInformationDto.xafIncludeLLInTermination = dataTable.Rows[i].Field<bool>("xafIncludeLLInTermination");
				eRPFinancialPropertyInformationDto.xafPAAllowParentAccountPost = dataTable.Rows[i].Field<bool>("xafPAAllowParentAccountPost");
				eRPFinancialPropertyInformationDto.xafPAAssignNumbersToEft = dataTable.Rows[i].Field<bool>("xafPAAssignNumbersToEft");
				eRPFinancialPropertyInformationDto.xafPADeleteZeroPayHeaders = dataTable.Rows[i].Field<bool>("xafPADeleteZeroPayHeaders");
				eRPFinancialPropertyInformationDto.xafPAExpressPost = dataTable.Rows[i].Field<bool>("xafPAExpressPost");
				eRPFinancialPropertyInformationDto.xafPartsMustExist = dataTable.Rows[i].Field<bool>("xafPartsMustExist");
				eRPFinancialPropertyInformationDto.xafPAShowHolidaysForSalary = dataTable.Rows[i].Field<bool>("xafPAShowHolidaysForSalary");
				eRPFinancialPropertyInformationDto.xafProductionExpressPost = dataTable.Rows[i].Field<bool>("xafProductionExpressPost");
				eRPFinancialPropertyInformationDto.xafRecalcSalarySacrifice = dataTable.Rows[i].Field<bool>("xafRecalcSalarySacrifice");
				eRPFinancialPropertyInformationDto.xafStpSetGrossPayAsETP = dataTable.Rows[i].Field<bool>("xafStpSetGrossPayAsETP");
				eRPFinancialPropertyInformationDto.xafLaborClearingGlAccountID = dataTable.Rows[i].Field<string>("xafLaborClearingGlAccountID");
				eRPFinancialPropertyInformationDto.xafMiscReceiptVarianceAccount = dataTable.Rows[i].Field<byte>("xafMiscReceiptVarianceAccount");
				eRPFinancialPropertyInformationDto.xafOverheadClearingGlAccountID = dataTable.Rows[i].Field<string>("xafOverheadClearingGlAccountID");
				eRPFinancialPropertyInformationDto.xafPALeaveBalanceCheck = dataTable.Rows[i].Field<byte>("xafPALeaveBalanceCheck");
				eRPFinancialPropertyInformationDto.xafPAUseDate = dataTable.Rows[i].Field<byte>("xafPAUseDate");
				eRPFinancialPropertyInformationDto.xafPurchaseVarianceGlAccountID = dataTable.Rows[i].Field<string>("xafPurchaseVarianceGlAccountID");
				eRPFinancialPropertyInformationDto.xafRoundingGlAccountID = dataTable.Rows[i].Field<string>("xafRoundingGlAccountID");
				eRPFinancialPropertyInformationDto.xafRowVersion = dataTable.Rows[i].Field<byte[]>("xafRowVersion");
				eRPFinancialPropertyInformationDto.xafShipAwaitInvoiceGlAccountID = dataTable.Rows[i].Field<string>("xafShipAwaitInvoiceGlAccountID");
				eRPFinancialPropertyInformationDto.xafStockInTransitGlAccountID = dataTable.Rows[i].Field<string>("xafStockInTransitGlAccountID");
				eRPFinancialPropertyInformationDto.xafStockRevaluationGlAccountID = dataTable.Rows[i].Field<string>("xafStockRevaluationGlAccountID");
				eRPFinancialPropertyInformationDto.xafStoreCreditGlAccountID = dataTable.Rows[i].Field<string>("xafStoreCreditGlAccountID");
				eRPFinancialPropertyInformationDto.xafSuperEmployerID = dataTable.Rows[i].Field<string>("xafSuperEmployerID");
				eRPFinancialPropertyInformationDto.xafSuperEndDate = dataTable.Rows[i].Field<DateTime?>("xafSuperEndDate");
				eRPFinancialPropertyInformationDto.xafSuperExportDateFormat = dataTable.Rows[i].Field<string>("xafSuperExportDateFormat");
				eRPFinancialPropertyInformationDto.xafSuperExportFilePath = dataTable.Rows[i].Field<string>("xafSuperExportFilePath");
				eRPFinancialPropertyInformationDto.xafSuperStartDate = dataTable.Rows[i].Field<DateTime?>("xafSuperStartDate");
				eRPFinancialPropertyInformationDto.xafSVarLaborGlAccountID = dataTable.Rows[i].Field<string>("xafSVarLaborGlAccountID");
				eRPFinancialPropertyInformationDto.xafSVarMaterialGlAccountID = dataTable.Rows[i].Field<string>("xafSVarMaterialGlAccountID");
				eRPFinancialPropertyInformationDto.xafSVarOverheadGlAccountID = dataTable.Rows[i].Field<string>("xafSVarOverheadGlAccountID");
				eRPFinancialPropertyInformationDto.xafSVarSubcontractGlAccountID = dataTable.Rows[i].Field<string>("xafSVarSubcontractGlAccountID");
				eRPFinancialPropertyInformationDto.xafTaxOnReportMethod = dataTable.Rows[i].Field<string>("xafTaxOnReportMethod");
				eRPFinancialPropertyInformationDto.xafTestFileCode = dataTable.Rows[i].Field<string>("xafTestFileCode");
				eRPFinancialPropertyInformationDto.xafTransmitterControlCode = dataTable.Rows[i].Field<string>("xafTransmitterControlCode");
				eRPFinancialPropertyInformationDto.xafUS1094FileLocation = dataTable.Rows[i].Field<string>("xafUS1094FileLocation");
				eRPFinancialPropertyInformationDto.xafWipLaborGlAccountID = dataTable.Rows[i].Field<string>("xafWipLaborGlAccountID");
				eRPFinancialPropertyInformationDto.xafWipMaterialGlAccountID = dataTable.Rows[i].Field<string>("xafWipMaterialGlAccountID");
				eRPFinancialPropertyInformationDto.xafWipoverheadGlAccountID = dataTable.Rows[i].Field<string>("xafWipoverheadGlAccountID");
				eRPFinancialPropertyInformationDto.xafWipSubcontractGlAccountID = dataTable.Rows[i].Field<string>("xafWipSubcontractGlAccountID");
				eRPFinancialPropertyInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPFinancialPropertyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPFinancialPropertyInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPFinancialPropertyInformationDto> GetFinancialProperty(Guid financialPropertyId)
	{
		ERPFinancialPropertyInformationDto eRPFinancialPropertyInformationDto = new ERPFinancialPropertyInformationDto();
		InitializeParameterLists();
		string[] collection = new string[128]
		{
			"xafAccruedCreditorsGlAccountID", "xafAgingMethod", "xafApAgingBucketID", "xafApApCostStartDate", "xafApApGlAccountID", "xafApCashGlAccountID", "xafApDiscountGlAccountID", "xafApFreightGlAccountID", "xafApGroupReceiptsBySupplier", "xafApPaymentMaxLinesPerPage",
			"xafArAgingBucketID", "xafArArGlAccountID", "xafArCashGlAccountID", "xafArDefaultLaborPartGroupID", "xafArDepositGlAccountID", "xafArDepositPartID", "xafArDepositPartRevisionID", "xafArDiscountGlAccountID", "xafArFinanceChargeGlAccountID", "xafArFinanceChargeGraceDays",
			"xafArFinanceChargeLastRunDate", "xafArFinanceChargePercent", "xafArFinanceShowCreditBalance", "xafArFreightGlAccountID", "xafArGroupShipmentsByCustomer", "xafArLaborPartID", "xafArLaborPartRevisionID", "xafArNET1GatewayID", "xafArNET1MerchantKey", "xafArNET1Port",
			"xafArNET1TimeoutSeconds", "xafArShowDeposits", "xafAvalaraAccountID", "xafAvalaraArInvoicePostOption", "xafAvalaraCanadaGstTaxCodeID", "xafAvalaraCanadaHSTTaxCodeID", "xafAvalaraCanadaPSTTaxCodeID", "xafAvalaraCanadaQSTTaxCodeID", "xafAvalaraCompanyCode", "xafAvalaraFilterCountry",
			"xafAvalaraLicenseKey", "xafAvalaraTaxCodeID", "xafAvalaraTimeoutSeconds", "xafAvalaraURL", "xafCAEmployerDentalBenefits", "xafCogsStatusHistory", "xafCogsUseAccounts", "xafCreatedBy", "xafCreatedDate", "xafCreditCardMethod",
			"xafDrawerCashGlAccountID", "xafDrawerCashStartAmount", "xafUniqueID", "xafGlFiscalYearID", "xafGlFiscalYearPeriodID", "xafGlRetainedEarningsAccountID", "xafAgeByDaysInMonth", "xafApAllowParentAccountPost", "xafApAlwaysTakeDiscount", "xafApAssignNumbersToEft",
			"xafApCreditUpdatesReceipt", "xafApDisableTaxFields", "xafApDiscountOnFreight", "xafApDiscountOnTax", "xafApExpressPost", "xafApIncludeTaxInExpAmt", "xafApPaymentFilterPlant", "xafApTaxOnFreight", "xafApUpdateJobCosts", "xafArAllowParentAccountPost",
			"xafArCalculateTaxOnDeposit", "xafArCreateDiscountJournals", "xafArCreditUpdatesShipment", "xafArDisableTaxFields", "xafArDiscountOnFreight", "xafArExpressPost", "xafArIncludeFrgtInDepositCalc", "xafArIncludeTaxInDepositCalc", "xafArPaymentFilterPlant", "xafArTaxOnFreight",
			"xafAvalaraDisableAddrValidate", "xafAvalaraDisableIgnoreLine", "xafAvalaraForceAddressValidate", "xafCreateBankEntries", "xafDisableMultiplePlants", "xafExactDaysInPaymentTerms", "xafFAroundToNearestDollar", "xafGlCreateStockJournals", "xafGlExpressPost", "xafIncludeLLInTermination",
			"xafPAAllowParentAccountPost", "xafPAAssignNumbersToEft", "xafPADeleteZeroPayHeaders", "xafPAExpressPost", "xafPartsMustExist", "xafPAShowHolidaysForSalary", "xafProductionExpressPost", "xafRecalcSalarySacrifice", "xafStpSetGrossPayAsETP", "xafLaborClearingGlAccountID",
			"xafMiscReceiptVarianceAccount", "xafOverheadClearingGlAccountID", "xafPALeaveBalanceCheck", "xafPAUseDate", "xafPurchaseVarianceGlAccountID", "xafRoundingGlAccountID", "xafRowVersion", "xafShipAwaitInvoiceGlAccountID", "xafStockInTransitGlAccountID", "xafStockRevaluationGlAccountID",
			"xafStoreCreditGlAccountID", "xafSuperEmployerID", "xafSuperEndDate", "xafSuperExportDateFormat", "xafSuperExportFilePath", "xafSuperStartDate", "xafSVarLaborGlAccountID", "xafSVarMaterialGlAccountID", "xafSVarOverheadGlAccountID", "xafSVarSubcontractGlAccountID",
			"xafTaxOnReportMethod", "xafTestFileCode", "xafTransmitterControlCode", "xafUS1094FileLocation", "xafWipLaborGlAccountID", "xafWipMaterialGlAccountID", "xafWipoverheadGlAccountID", "xafWipSubcontractGlAccountID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xafUniqueID|C", financialPropertyId);
		AddCustomFieldsToSelectList("FinancialProperties");
		using (DataTable dataTable = GetAsDataTable("FinancialProperties", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPFinancialPropertyInformationDto);
			}
			eRPFinancialPropertyInformationDto.xafAccruedCreditorsGlAccountID = dataTable.Rows[0].Field<string>("xafAccruedCreditorsGlAccountID");
			eRPFinancialPropertyInformationDto.xafAgingMethod = dataTable.Rows[0].Field<byte>("xafAgingMethod");
			eRPFinancialPropertyInformationDto.xafApAgingBucketID = dataTable.Rows[0].Field<string>("xafApAgingBucketID");
			eRPFinancialPropertyInformationDto.xafApApCostStartDate = dataTable.Rows[0].Field<DateTime?>("xafApApCostStartDate");
			eRPFinancialPropertyInformationDto.xafApApGlAccountID = dataTable.Rows[0].Field<string>("xafApApGlAccountID");
			eRPFinancialPropertyInformationDto.xafApCashGlAccountID = dataTable.Rows[0].Field<string>("xafApCashGlAccountID");
			eRPFinancialPropertyInformationDto.xafApDiscountGlAccountID = dataTable.Rows[0].Field<string>("xafApDiscountGlAccountID");
			eRPFinancialPropertyInformationDto.xafApFreightGlAccountID = dataTable.Rows[0].Field<string>("xafApFreightGlAccountID");
			eRPFinancialPropertyInformationDto.xafApGroupReceiptsBySupplier = dataTable.Rows[0].Field<byte>("xafApGroupReceiptsBySupplier");
			eRPFinancialPropertyInformationDto.xafApPaymentMaxLinesPerPage = dataTable.Rows[0].Field<byte>("xafApPaymentMaxLinesPerPage");
			eRPFinancialPropertyInformationDto.xafArAgingBucketID = dataTable.Rows[0].Field<string>("xafArAgingBucketID");
			eRPFinancialPropertyInformationDto.xafArArGlAccountID = dataTable.Rows[0].Field<string>("xafArArGlAccountID");
			eRPFinancialPropertyInformationDto.xafArCashGlAccountID = dataTable.Rows[0].Field<string>("xafArCashGlAccountID");
			eRPFinancialPropertyInformationDto.xafArDefaultLaborPartGroupID = dataTable.Rows[0].Field<string>("xafArDefaultLaborPartGroupID");
			eRPFinancialPropertyInformationDto.xafArDepositGlAccountID = dataTable.Rows[0].Field<string>("xafArDepositGlAccountID");
			eRPFinancialPropertyInformationDto.xafArDepositPartID = dataTable.Rows[0].Field<string>("xafArDepositPartID");
			eRPFinancialPropertyInformationDto.xafArDepositPartRevisionID = dataTable.Rows[0].Field<string>("xafArDepositPartRevisionID");
			eRPFinancialPropertyInformationDto.xafArDiscountGlAccountID = dataTable.Rows[0].Field<string>("xafArDiscountGlAccountID");
			eRPFinancialPropertyInformationDto.xafArFinanceChargeGlAccountID = dataTable.Rows[0].Field<string>("xafArFinanceChargeGlAccountID");
			eRPFinancialPropertyInformationDto.xafArFinanceChargeGraceDays = dataTable.Rows[0].Field<short>("xafArFinanceChargeGraceDays");
			eRPFinancialPropertyInformationDto.xafArFinanceChargeLastRunDate = dataTable.Rows[0].Field<DateTime?>("xafArFinanceChargeLastRunDate");
			eRPFinancialPropertyInformationDto.xafArFinanceChargePercent = dataTable.Rows[0].Field<decimal>("xafArFinanceChargePercent");
			eRPFinancialPropertyInformationDto.xafArFinanceShowCreditBalance = dataTable.Rows[0].Field<byte>("xafArFinanceShowCreditBalance");
			eRPFinancialPropertyInformationDto.xafArFreightGlAccountID = dataTable.Rows[0].Field<string>("xafArFreightGlAccountID");
			eRPFinancialPropertyInformationDto.xafArGroupShipmentsByCustomer = dataTable.Rows[0].Field<byte>("xafArGroupShipmentsByCustomer");
			eRPFinancialPropertyInformationDto.xafArLaborPartID = dataTable.Rows[0].Field<string>("xafArLaborPartID");
			eRPFinancialPropertyInformationDto.xafArLaborPartRevisionID = dataTable.Rows[0].Field<string>("xafArLaborPartRevisionID");
			eRPFinancialPropertyInformationDto.xafArNET1GatewayID = dataTable.Rows[0].Field<string>("xafArNET1GatewayID");
			eRPFinancialPropertyInformationDto.xafArNET1MerchantKey = dataTable.Rows[0].Field<string>("xafArNET1MerchantKey");
			eRPFinancialPropertyInformationDto.xafArNET1Port = dataTable.Rows[0].Field<int>("xafArNET1Port");
			eRPFinancialPropertyInformationDto.xafArNET1TimeoutSeconds = dataTable.Rows[0].Field<short>("xafArNET1TimeoutSeconds");
			eRPFinancialPropertyInformationDto.xafArShowDeposits = dataTable.Rows[0].Field<byte>("xafArShowDeposits");
			eRPFinancialPropertyInformationDto.xafAvalaraAccountID = dataTable.Rows[0].Field<string>("xafAvalaraAccountID");
			eRPFinancialPropertyInformationDto.xafAvalaraArInvoicePostOption = dataTable.Rows[0].Field<byte>("xafAvalaraArInvoicePostOption");
			eRPFinancialPropertyInformationDto.xafAvalaraCanadaGstTaxCodeID = dataTable.Rows[0].Field<string>("xafAvalaraCanadaGstTaxCodeID");
			eRPFinancialPropertyInformationDto.xafAvalaraCanadaHSTTaxCodeID = dataTable.Rows[0].Field<string>("xafAvalaraCanadaHSTTaxCodeID");
			eRPFinancialPropertyInformationDto.xafAvalaraCanadaPSTTaxCodeID = dataTable.Rows[0].Field<string>("xafAvalaraCanadaPSTTaxCodeID");
			eRPFinancialPropertyInformationDto.xafAvalaraCanadaQSTTaxCodeID = dataTable.Rows[0].Field<string>("xafAvalaraCanadaQSTTaxCodeID");
			eRPFinancialPropertyInformationDto.xafAvalaraCompanyCode = dataTable.Rows[0].Field<string>("xafAvalaraCompanyCode");
			eRPFinancialPropertyInformationDto.xafAvalaraFilterCountry = dataTable.Rows[0].Field<byte>("xafAvalaraFilterCountry");
			eRPFinancialPropertyInformationDto.xafAvalaraLicenseKey = dataTable.Rows[0].Field<string>("xafAvalaraLicenseKey");
			eRPFinancialPropertyInformationDto.xafAvalaraTaxCodeID = dataTable.Rows[0].Field<string>("xafAvalaraTaxCodeID");
			eRPFinancialPropertyInformationDto.xafAvalaraTimeoutSeconds = dataTable.Rows[0].Field<short>("xafAvalaraTimeoutSeconds");
			eRPFinancialPropertyInformationDto.xafAvalaraURL = dataTable.Rows[0].Field<string>("xafAvalaraURL");
			eRPFinancialPropertyInformationDto.xafCAEmployerDentalBenefits = dataTable.Rows[0].Field<byte>("xafCAEmployerDentalBenefits");
			eRPFinancialPropertyInformationDto.xafCogsStatusHistory = dataTable.Rows[0].Field<string>("xafCogsStatusHistory");
			eRPFinancialPropertyInformationDto.xafCogsUseAccounts = dataTable.Rows[0].Field<byte>("xafCogsUseAccounts");
			eRPFinancialPropertyInformationDto.xafCreatedBy = dataTable.Rows[0].Field<string>("xafCreatedBy");
			eRPFinancialPropertyInformationDto.xafCreatedDate = dataTable.Rows[0].Field<DateTime?>("xafCreatedDate");
			eRPFinancialPropertyInformationDto.xafCreditCardMethod = dataTable.Rows[0].Field<byte>("xafCreditCardMethod");
			eRPFinancialPropertyInformationDto.xafDrawerCashGlAccountID = dataTable.Rows[0].Field<string>("xafDrawerCashGlAccountID");
			eRPFinancialPropertyInformationDto.xafDrawerCashStartAmount = dataTable.Rows[0].Field<decimal>("xafDrawerCashStartAmount");
			eRPFinancialPropertyInformationDto.xafUniqueID = dataTable.Rows[0].Field<Guid>("xafUniqueID");
			eRPFinancialPropertyInformationDto.xafGlFiscalYearID = dataTable.Rows[0].Field<short>("xafGlFiscalYearID");
			eRPFinancialPropertyInformationDto.xafGlFiscalYearPeriodID = dataTable.Rows[0].Field<byte>("xafGlFiscalYearPeriodID");
			eRPFinancialPropertyInformationDto.xafGlRetainedEarningsAccountID = dataTable.Rows[0].Field<string>("xafGlRetainedEarningsAccountID");
			eRPFinancialPropertyInformationDto.xafAgeByDaysInMonth = dataTable.Rows[0].Field<bool>("xafAgeByDaysInMonth");
			eRPFinancialPropertyInformationDto.xafApAllowParentAccountPost = dataTable.Rows[0].Field<bool>("xafApAllowParentAccountPost");
			eRPFinancialPropertyInformationDto.xafApAlwaysTakeDiscount = dataTable.Rows[0].Field<bool>("xafApAlwaysTakeDiscount");
			eRPFinancialPropertyInformationDto.xafApAssignNumbersToEft = dataTable.Rows[0].Field<bool>("xafApAssignNumbersToEft");
			eRPFinancialPropertyInformationDto.xafApCreditUpdatesReceipt = dataTable.Rows[0].Field<bool>("xafApCreditUpdatesReceipt");
			eRPFinancialPropertyInformationDto.xafApDisableTaxFields = dataTable.Rows[0].Field<bool>("xafApDisableTaxFields");
			eRPFinancialPropertyInformationDto.xafApDiscountOnFreight = dataTable.Rows[0].Field<bool>("xafApDiscountOnFreight");
			eRPFinancialPropertyInformationDto.xafApDiscountOnTax = dataTable.Rows[0].Field<bool>("xafApDiscountOnTax");
			eRPFinancialPropertyInformationDto.xafApExpressPost = dataTable.Rows[0].Field<bool>("xafApExpressPost");
			eRPFinancialPropertyInformationDto.xafApIncludeTaxInExpAmt = dataTable.Rows[0].Field<bool>("xafApIncludeTaxInExpAmt");
			eRPFinancialPropertyInformationDto.xafApPaymentFilterPlant = dataTable.Rows[0].Field<bool>("xafApPaymentFilterPlant");
			eRPFinancialPropertyInformationDto.xafApTaxOnFreight = dataTable.Rows[0].Field<bool>("xafApTaxOnFreight");
			eRPFinancialPropertyInformationDto.xafApUpdateJobCosts = dataTable.Rows[0].Field<bool>("xafApUpdateJobCosts");
			eRPFinancialPropertyInformationDto.xafArAllowParentAccountPost = dataTable.Rows[0].Field<bool>("xafArAllowParentAccountPost");
			eRPFinancialPropertyInformationDto.xafArCalculateTaxOnDeposit = dataTable.Rows[0].Field<bool>("xafArCalculateTaxOnDeposit");
			eRPFinancialPropertyInformationDto.xafArCreateDiscountJournals = dataTable.Rows[0].Field<bool>("xafArCreateDiscountJournals");
			eRPFinancialPropertyInformationDto.xafArCreditUpdatesShipment = dataTable.Rows[0].Field<bool>("xafArCreditUpdatesShipment");
			eRPFinancialPropertyInformationDto.xafArDisableTaxFields = dataTable.Rows[0].Field<bool>("xafArDisableTaxFields");
			eRPFinancialPropertyInformationDto.xafArDiscountOnFreight = dataTable.Rows[0].Field<bool>("xafArDiscountOnFreight");
			eRPFinancialPropertyInformationDto.xafArExpressPost = dataTable.Rows[0].Field<bool>("xafArExpressPost");
			eRPFinancialPropertyInformationDto.xafArIncludeFrgtInDepositCalc = dataTable.Rows[0].Field<bool>("xafArIncludeFrgtInDepositCalc");
			eRPFinancialPropertyInformationDto.xafArIncludeTaxInDepositCalc = dataTable.Rows[0].Field<bool>("xafArIncludeTaxInDepositCalc");
			eRPFinancialPropertyInformationDto.xafArPaymentFilterPlant = dataTable.Rows[0].Field<bool>("xafArPaymentFilterPlant");
			eRPFinancialPropertyInformationDto.xafArTaxOnFreight = dataTable.Rows[0].Field<bool>("xafArTaxOnFreight");
			eRPFinancialPropertyInformationDto.xafAvalaraDisableAddrValidate = dataTable.Rows[0].Field<bool>("xafAvalaraDisableAddrValidate");
			eRPFinancialPropertyInformationDto.xafAvalaraDisableIgnoreLine = dataTable.Rows[0].Field<bool>("xafAvalaraDisableIgnoreLine");
			eRPFinancialPropertyInformationDto.xafAvalaraForceAddressValidate = dataTable.Rows[0].Field<bool>("xafAvalaraForceAddressValidate");
			eRPFinancialPropertyInformationDto.xafCreateBankEntries = dataTable.Rows[0].Field<bool>("xafCreateBankEntries");
			eRPFinancialPropertyInformationDto.xafDisableMultiplePlants = dataTable.Rows[0].Field<bool>("xafDisableMultiplePlants");
			eRPFinancialPropertyInformationDto.xafExactDaysInPaymentTerms = dataTable.Rows[0].Field<bool>("xafExactDaysInPaymentTerms");
			eRPFinancialPropertyInformationDto.xafFAroundToNearestDollar = dataTable.Rows[0].Field<bool>("xafFAroundToNearestDollar");
			eRPFinancialPropertyInformationDto.xafGlCreateStockJournals = dataTable.Rows[0].Field<bool>("xafGlCreateStockJournals");
			eRPFinancialPropertyInformationDto.xafGlExpressPost = dataTable.Rows[0].Field<bool>("xafGlExpressPost");
			eRPFinancialPropertyInformationDto.xafIncludeLLInTermination = dataTable.Rows[0].Field<bool>("xafIncludeLLInTermination");
			eRPFinancialPropertyInformationDto.xafPAAllowParentAccountPost = dataTable.Rows[0].Field<bool>("xafPAAllowParentAccountPost");
			eRPFinancialPropertyInformationDto.xafPAAssignNumbersToEft = dataTable.Rows[0].Field<bool>("xafPAAssignNumbersToEft");
			eRPFinancialPropertyInformationDto.xafPADeleteZeroPayHeaders = dataTable.Rows[0].Field<bool>("xafPADeleteZeroPayHeaders");
			eRPFinancialPropertyInformationDto.xafPAExpressPost = dataTable.Rows[0].Field<bool>("xafPAExpressPost");
			eRPFinancialPropertyInformationDto.xafPartsMustExist = dataTable.Rows[0].Field<bool>("xafPartsMustExist");
			eRPFinancialPropertyInformationDto.xafPAShowHolidaysForSalary = dataTable.Rows[0].Field<bool>("xafPAShowHolidaysForSalary");
			eRPFinancialPropertyInformationDto.xafProductionExpressPost = dataTable.Rows[0].Field<bool>("xafProductionExpressPost");
			eRPFinancialPropertyInformationDto.xafRecalcSalarySacrifice = dataTable.Rows[0].Field<bool>("xafRecalcSalarySacrifice");
			eRPFinancialPropertyInformationDto.xafStpSetGrossPayAsETP = dataTable.Rows[0].Field<bool>("xafStpSetGrossPayAsETP");
			eRPFinancialPropertyInformationDto.xafLaborClearingGlAccountID = dataTable.Rows[0].Field<string>("xafLaborClearingGlAccountID");
			eRPFinancialPropertyInformationDto.xafMiscReceiptVarianceAccount = dataTable.Rows[0].Field<byte>("xafMiscReceiptVarianceAccount");
			eRPFinancialPropertyInformationDto.xafOverheadClearingGlAccountID = dataTable.Rows[0].Field<string>("xafOverheadClearingGlAccountID");
			eRPFinancialPropertyInformationDto.xafPALeaveBalanceCheck = dataTable.Rows[0].Field<byte>("xafPALeaveBalanceCheck");
			eRPFinancialPropertyInformationDto.xafPAUseDate = dataTable.Rows[0].Field<byte>("xafPAUseDate");
			eRPFinancialPropertyInformationDto.xafPurchaseVarianceGlAccountID = dataTable.Rows[0].Field<string>("xafPurchaseVarianceGlAccountID");
			eRPFinancialPropertyInformationDto.xafRoundingGlAccountID = dataTable.Rows[0].Field<string>("xafRoundingGlAccountID");
			eRPFinancialPropertyInformationDto.xafRowVersion = dataTable.Rows[0].Field<byte[]>("xafRowVersion");
			eRPFinancialPropertyInformationDto.xafShipAwaitInvoiceGlAccountID = dataTable.Rows[0].Field<string>("xafShipAwaitInvoiceGlAccountID");
			eRPFinancialPropertyInformationDto.xafStockInTransitGlAccountID = dataTable.Rows[0].Field<string>("xafStockInTransitGlAccountID");
			eRPFinancialPropertyInformationDto.xafStockRevaluationGlAccountID = dataTable.Rows[0].Field<string>("xafStockRevaluationGlAccountID");
			eRPFinancialPropertyInformationDto.xafStoreCreditGlAccountID = dataTable.Rows[0].Field<string>("xafStoreCreditGlAccountID");
			eRPFinancialPropertyInformationDto.xafSuperEmployerID = dataTable.Rows[0].Field<string>("xafSuperEmployerID");
			eRPFinancialPropertyInformationDto.xafSuperEndDate = dataTable.Rows[0].Field<DateTime?>("xafSuperEndDate");
			eRPFinancialPropertyInformationDto.xafSuperExportDateFormat = dataTable.Rows[0].Field<string>("xafSuperExportDateFormat");
			eRPFinancialPropertyInformationDto.xafSuperExportFilePath = dataTable.Rows[0].Field<string>("xafSuperExportFilePath");
			eRPFinancialPropertyInformationDto.xafSuperStartDate = dataTable.Rows[0].Field<DateTime?>("xafSuperStartDate");
			eRPFinancialPropertyInformationDto.xafSVarLaborGlAccountID = dataTable.Rows[0].Field<string>("xafSVarLaborGlAccountID");
			eRPFinancialPropertyInformationDto.xafSVarMaterialGlAccountID = dataTable.Rows[0].Field<string>("xafSVarMaterialGlAccountID");
			eRPFinancialPropertyInformationDto.xafSVarOverheadGlAccountID = dataTable.Rows[0].Field<string>("xafSVarOverheadGlAccountID");
			eRPFinancialPropertyInformationDto.xafSVarSubcontractGlAccountID = dataTable.Rows[0].Field<string>("xafSVarSubcontractGlAccountID");
			eRPFinancialPropertyInformationDto.xafTaxOnReportMethod = dataTable.Rows[0].Field<string>("xafTaxOnReportMethod");
			eRPFinancialPropertyInformationDto.xafTestFileCode = dataTable.Rows[0].Field<string>("xafTestFileCode");
			eRPFinancialPropertyInformationDto.xafTransmitterControlCode = dataTable.Rows[0].Field<string>("xafTransmitterControlCode");
			eRPFinancialPropertyInformationDto.xafUS1094FileLocation = dataTable.Rows[0].Field<string>("xafUS1094FileLocation");
			eRPFinancialPropertyInformationDto.xafWipLaborGlAccountID = dataTable.Rows[0].Field<string>("xafWipLaborGlAccountID");
			eRPFinancialPropertyInformationDto.xafWipMaterialGlAccountID = dataTable.Rows[0].Field<string>("xafWipMaterialGlAccountID");
			eRPFinancialPropertyInformationDto.xafWipoverheadGlAccountID = dataTable.Rows[0].Field<string>("xafWipoverheadGlAccountID");
			eRPFinancialPropertyInformationDto.xafWipSubcontractGlAccountID = dataTable.Rows[0].Field<string>("xafWipSubcontractGlAccountID");
			eRPFinancialPropertyInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPFinancialPropertyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPFinancialPropertyInformationDto);
	}
}
