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

public class ERPOrganizationRepository : APIBaseRepository, IERPOrganizationRepository, IAPIBaseRepository, IDisposable
{
	public ERPOrganizationRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesOrganizationExist(Guid organizationId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmoUniqueID|C", organizationId);
		base.selectList.Add("cmoUniqueID");
		return Task.FromResult(GetAsObject("Organizations", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPOrganizationInformationDto>> GetAllOrganizations(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPOrganizationInformationDto> collection = new List<ERPOrganizationInformationDto>();
		InitializeParameterLists();
		string[] array = new string[135]
		{
			"cmoAccountManagerEmployeeID", "cmoAddressLine1", "cmoAddressLine2", "cmoAddressLine3", "cmoAddressValidationResult", "cmoAlternatePhoneNumber", "cmoApInvoiceContactID", "cmoArInvoiceContactID", "cmoAttachmentFileFolder", "cmoAvalaraUseCodes",
			"cmoBankAccountName", "cmoBankAccountNumber", "cmoBankAccountType", "cmoBankInitials", "cmoBic", "cmoBsbNumber", "cmoCity", "cmoOrganizationID", "cmoCompanyEntryDescription", "cmoCountry",
			"cmoCountryCode", "cmoCounty", "cmoCreatedBy", "cmoCreatedDate", "cmoCurrencyRateID", "cmoCustomerActiveDate", "cmoCustomerCreditLimit", "cmoCustomerGroupID", "cmoCustomerInactiveDate", "cmoCustomerPaymentTermsID",
			"cmoCustomerProspectDate", "cmoCustomerSecondTaxCodeID", "cmoCustomerShipPaymentTypeID", "cmoCustomerShippingCarrier", "cmoCustomerShippingMethodID", "cmoCustomerStatus", "cmoCustomerTaxCodeID", "cmoDefaultApInvoiceLocationID", "cmoDefaultArInvoiceLocationID", "cmoDefaultPurchaseLocationID",
			"cmoDefaultQuoteLocationID", "cmoDefaultShipLocationID", "cmoDropShipLocationID", "cmoDropShipOrganizationID", "cmoEftCode", "cmoEftDescription", "cmoEftParticulars", "cmoEmailAddress", "cmoEmployeeCount", "cmoUniqueID",
			"cmoEstablishedDate", "cmoExpenseSplitPercentTotal", "cmoFaxNumber", "cmoFederalID", "cmoFedEx3rdPartyLocationID", "cmoFedEx3rdPartyOrganizationID", "cmoFedExAccountNumber", "cmoFedExBillingOption", "cmoFirstGivenName", "cmoForm1099Box",
			"cmoFreeOnBoardDescription", "cmoHdAttachmentFilePath", "cmoIban", "cmoIntraCompanyDatasetID", "cmoApIncludeTaxInRetention", "cmoArIncludeTaxInRetention", "cmoArInvoicePerShipmentLine", "cmoAvalaraAddressValidated", "cmoBareCostOfDuty", "cmoBareTransportationCost",
			"cmoCalculateFinanceCharges", "cmoCompetitor", "cmoContractor", "cmoCreatedFromMobile", "cmoCreditHold", "cmoCustomerTaxable", "cmoDirectPayment", "cmoEdiIntegrated", "cmoFinanceCompany", "cmoIgnoreAvalara",
			"cmoIncludeFreightInPrice", "cmoPrintStatement", "cmoRequires1099", "cmoRequiresInspection", "cmoResidentialAddress", "cmoSuperFund", "cmoSupplierAccredited", "cmoSupplierTaxable", "cmoTaxReportable", "cmoUpsValidated",
			"cmoJobPriorityID", "cmoLastName", "cmoLongDescriptionRtf", "cmoLongDescriptionText", "cmoName", "cmoNonTaxReasonID", "cmoOrganizationAccountID", "cmoPhoneNumber", "cmoPostCode", "cmoPurchaseContactID",
			"cmoQuoteContactID", "cmoResellerActiveDate", "cmoResellerCommissionRate", "cmoResellerContactID", "cmoResellerInactiveDate", "cmoResellerLocationID", "cmoResellerOrganizationID", "cmoResellerProspectDate", "cmoResellerStatus", "cmoRowVersion",
			"cmoSecondGivenName", "cmoShipContactID", "cmoSplitPercentTotal", "cmoState", "cmoSuperFundEmployerID", "cmoSuperFundName", "cmoSupplierAccreditedDate", "cmoSupplierActiveDate", "cmoSupplierInactiveDate", "cmoSupplierPaymentTermID",
			"cmoSupplierProspectDate", "cmoSupplierRatingID", "cmoSupplierSecondTaxCodeID", "cmoSupplierShippingMethodID", "cmoSupplierStatus", "cmoSupplierTaxCodeID", "cmoTaxExemptNumber", "cmoTradingName", "cmoUps3rdPartyLocationID", "cmoUps3rdPartyOrganizationID",
			"cmoUpsAcctNumber", "cmoUpsBillingOption", "cmoUpsWsBillingOption", "cmoUsaTransactionTypeCode", "cmoWebAddress"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Organizations");
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
		using (DataTable dataTable = GetAsDataTable("Organizations", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPOrganizationInformationDto eRPOrganizationInformationDto = new ERPOrganizationInformationDto();
				eRPOrganizationInformationDto.cmoAccountManagerEmployeeID = dataTable.Rows[i].Field<string>("cmoAccountManagerEmployeeID");
				eRPOrganizationInformationDto.cmoAddressLine1 = dataTable.Rows[i].Field<string>("cmoAddressLine1");
				eRPOrganizationInformationDto.cmoAddressLine2 = dataTable.Rows[i].Field<string>("cmoAddressLine2");
				eRPOrganizationInformationDto.cmoAddressLine3 = dataTable.Rows[i].Field<string>("cmoAddressLine3");
				eRPOrganizationInformationDto.cmoAddressValidationResult = dataTable.Rows[i].Field<string>("cmoAddressValidationResult");
				eRPOrganizationInformationDto.cmoAlternatePhoneNumber = dataTable.Rows[i].Field<string>("cmoAlternatePhoneNumber");
				eRPOrganizationInformationDto.cmoApInvoiceContactID = dataTable.Rows[i].Field<string>("cmoApInvoiceContactID");
				eRPOrganizationInformationDto.cmoArInvoiceContactID = dataTable.Rows[i].Field<string>("cmoArInvoiceContactID");
				eRPOrganizationInformationDto.cmoAttachmentFileFolder = dataTable.Rows[i].Field<string>("cmoAttachmentFileFolder");
				eRPOrganizationInformationDto.cmoAvalaraUseCodes = dataTable.Rows[i].Field<string>("cmoAvalaraUseCodes");
				eRPOrganizationInformationDto.cmoBankAccountName = dataTable.Rows[i].Field<string>("cmoBankAccountName");
				eRPOrganizationInformationDto.cmoBankAccountNumber = dataTable.Rows[i].Field<string>("cmoBankAccountNumber");
				eRPOrganizationInformationDto.cmoBankAccountType = dataTable.Rows[i].Field<string>("cmoBankAccountType");
				eRPOrganizationInformationDto.cmoBankInitials = dataTable.Rows[i].Field<string>("cmoBankInitials");
				eRPOrganizationInformationDto.cmoBic = dataTable.Rows[i].Field<string>("cmoBic");
				eRPOrganizationInformationDto.cmoBsbNumber = dataTable.Rows[i].Field<string>("cmoBsbNumber");
				eRPOrganizationInformationDto.cmoCity = dataTable.Rows[i].Field<string>("cmoCity");
				eRPOrganizationInformationDto.cmoOrganizationID = dataTable.Rows[i].Field<string>("cmoOrganizationID");
				eRPOrganizationInformationDto.cmoCompanyEntryDescription = dataTable.Rows[i].Field<string>("cmoCompanyEntryDescription");
				eRPOrganizationInformationDto.cmoCountry = dataTable.Rows[i].Field<string>("cmoCountry");
				eRPOrganizationInformationDto.cmoCountryCode = dataTable.Rows[i].Field<string>("cmoCountryCode");
				eRPOrganizationInformationDto.cmoCounty = dataTable.Rows[i].Field<string>("cmoCounty");
				eRPOrganizationInformationDto.cmoCreatedBy = dataTable.Rows[i].Field<string>("cmoCreatedBy");
				eRPOrganizationInformationDto.cmoCreatedDate = dataTable.Rows[i].Field<DateTime?>("cmoCreatedDate");
				eRPOrganizationInformationDto.cmoCurrencyRateID = dataTable.Rows[i].Field<string>("cmoCurrencyRateID");
				eRPOrganizationInformationDto.cmoCustomerActiveDate = dataTable.Rows[i].Field<DateTime?>("cmoCustomerActiveDate");
				eRPOrganizationInformationDto.cmoCustomerCreditLimit = dataTable.Rows[i].Field<decimal>("cmoCustomerCreditLimit");
				eRPOrganizationInformationDto.cmoCustomerGroupID = dataTable.Rows[i].Field<string>("cmoCustomerGroupID");
				eRPOrganizationInformationDto.cmoCustomerInactiveDate = dataTable.Rows[i].Field<DateTime?>("cmoCustomerInactiveDate");
				eRPOrganizationInformationDto.cmoCustomerPaymentTermsID = dataTable.Rows[i].Field<string>("cmoCustomerPaymentTermsID");
				eRPOrganizationInformationDto.cmoCustomerProspectDate = dataTable.Rows[i].Field<DateTime?>("cmoCustomerProspectDate");
				eRPOrganizationInformationDto.cmoCustomerSecondTaxCodeID = dataTable.Rows[i].Field<string>("cmoCustomerSecondTaxCodeID");
				eRPOrganizationInformationDto.cmoCustomerShipPaymentTypeID = dataTable.Rows[i].Field<string>("cmoCustomerShipPaymentTypeID");
				eRPOrganizationInformationDto.cmoCustomerShippingCarrier = dataTable.Rows[i].Field<string>("cmoCustomerShippingCarrier");
				eRPOrganizationInformationDto.cmoCustomerShippingMethodID = dataTable.Rows[i].Field<string>("cmoCustomerShippingMethodID");
				eRPOrganizationInformationDto.cmoCustomerStatus = dataTable.Rows[i].Field<byte>("cmoCustomerStatus");
				eRPOrganizationInformationDto.cmoCustomerTaxCodeID = dataTable.Rows[i].Field<string>("cmoCustomerTaxCodeID");
				eRPOrganizationInformationDto.cmoDefaultApInvoiceLocationID = dataTable.Rows[i].Field<string>("cmoDefaultApInvoiceLocationID");
				eRPOrganizationInformationDto.cmoDefaultArInvoiceLocationID = dataTable.Rows[i].Field<string>("cmoDefaultArInvoiceLocationID");
				eRPOrganizationInformationDto.cmoDefaultPurchaseLocationID = dataTable.Rows[i].Field<string>("cmoDefaultPurchaseLocationID");
				eRPOrganizationInformationDto.cmoDefaultQuoteLocationID = dataTable.Rows[i].Field<string>("cmoDefaultQuoteLocationID");
				eRPOrganizationInformationDto.cmoDefaultShipLocationID = dataTable.Rows[i].Field<string>("cmoDefaultShipLocationID");
				eRPOrganizationInformationDto.cmoDropShipLocationID = dataTable.Rows[i].Field<string>("cmoDropShipLocationID");
				eRPOrganizationInformationDto.cmoDropShipOrganizationID = dataTable.Rows[i].Field<string>("cmoDropShipOrganizationID");
				eRPOrganizationInformationDto.cmoEftCode = dataTable.Rows[i].Field<string>("cmoEftCode");
				eRPOrganizationInformationDto.cmoEftDescription = dataTable.Rows[i].Field<string>("cmoEftDescription");
				eRPOrganizationInformationDto.cmoEftParticulars = dataTable.Rows[i].Field<string>("cmoEftParticulars");
				eRPOrganizationInformationDto.cmoEmailAddress = dataTable.Rows[i].Field<string>("cmoEmailAddress");
				eRPOrganizationInformationDto.cmoEmployeeCount = dataTable.Rows[i].Field<int>("cmoEmployeeCount");
				eRPOrganizationInformationDto.cmoUniqueID = dataTable.Rows[i].Field<Guid>("cmoUniqueID");
				eRPOrganizationInformationDto.cmoEstablishedDate = dataTable.Rows[i].Field<DateTime?>("cmoEstablishedDate");
				eRPOrganizationInformationDto.cmoExpenseSplitPercentTotal = dataTable.Rows[i].Field<decimal>("cmoExpenseSplitPercentTotal");
				eRPOrganizationInformationDto.cmoFaxNumber = dataTable.Rows[i].Field<string>("cmoFaxNumber");
				eRPOrganizationInformationDto.cmoFederalID = dataTable.Rows[i].Field<string>("cmoFederalID");
				eRPOrganizationInformationDto.cmoFedEx3rdPartyLocationID = dataTable.Rows[i].Field<string>("cmoFedEx3rdPartyLocationID");
				eRPOrganizationInformationDto.cmoFedEx3rdPartyOrganizationID = dataTable.Rows[i].Field<string>("cmoFedEx3rdPartyOrganizationID");
				eRPOrganizationInformationDto.cmoFedExAccountNumber = dataTable.Rows[i].Field<string>("cmoFedExAccountNumber");
				eRPOrganizationInformationDto.cmoFedExBillingOption = dataTable.Rows[i].Field<string>("cmoFedExBillingOption");
				eRPOrganizationInformationDto.cmoFirstGivenName = dataTable.Rows[i].Field<string>("cmoFirstGivenName");
				eRPOrganizationInformationDto.cmoForm1099Box = dataTable.Rows[i].Field<byte>("cmoForm1099Box");
				eRPOrganizationInformationDto.cmoFreeOnBoardDescription = dataTable.Rows[i].Field<string>("cmoFreeOnBoardDescription");
				eRPOrganizationInformationDto.cmoHdAttachmentFilePath = dataTable.Rows[i].Field<string>("cmoHdAttachmentFilePath");
				eRPOrganizationInformationDto.cmoIban = dataTable.Rows[i].Field<string>("cmoIban");
				eRPOrganizationInformationDto.cmoIntraCompanyDatasetID = dataTable.Rows[i].Field<string>("cmoIntraCompanyDatasetID");
				eRPOrganizationInformationDto.cmoApIncludeTaxInRetention = dataTable.Rows[i].Field<bool>("cmoApIncludeTaxInRetention");
				eRPOrganizationInformationDto.cmoArIncludeTaxInRetention = dataTable.Rows[i].Field<bool>("cmoArIncludeTaxInRetention");
				eRPOrganizationInformationDto.cmoArInvoicePerShipmentLine = dataTable.Rows[i].Field<bool>("cmoArInvoicePerShipmentLine");
				eRPOrganizationInformationDto.cmoAvalaraAddressValidated = dataTable.Rows[i].Field<bool>("cmoAvalaraAddressValidated");
				eRPOrganizationInformationDto.cmoBareCostOfDuty = dataTable.Rows[i].Field<bool>("cmoBareCostOfDuty");
				eRPOrganizationInformationDto.cmoBareTransportationCost = dataTable.Rows[i].Field<bool>("cmoBareTransportationCost");
				eRPOrganizationInformationDto.cmoCalculateFinanceCharges = dataTable.Rows[i].Field<bool>("cmoCalculateFinanceCharges");
				eRPOrganizationInformationDto.cmoCompetitor = dataTable.Rows[i].Field<bool>("cmoCompetitor");
				eRPOrganizationInformationDto.cmoContractor = dataTable.Rows[i].Field<bool>("cmoContractor");
				eRPOrganizationInformationDto.cmoCreatedFromMobile = dataTable.Rows[i].Field<bool>("cmoCreatedFromMobile");
				eRPOrganizationInformationDto.cmoCreditHold = dataTable.Rows[i].Field<bool>("cmoCreditHold");
				eRPOrganizationInformationDto.cmoCustomerTaxable = dataTable.Rows[i].Field<bool>("cmoCustomerTaxable");
				eRPOrganizationInformationDto.cmoDirectPayment = dataTable.Rows[i].Field<bool>("cmoDirectPayment");
				eRPOrganizationInformationDto.cmoEdiIntegrated = dataTable.Rows[i].Field<bool>("cmoEdiIntegrated");
				eRPOrganizationInformationDto.cmoFinanceCompany = dataTable.Rows[i].Field<bool>("cmoFinanceCompany");
				eRPOrganizationInformationDto.cmoIgnoreAvalara = dataTable.Rows[i].Field<bool>("cmoIgnoreAvalara");
				eRPOrganizationInformationDto.cmoIncludeFreightInPrice = dataTable.Rows[i].Field<bool>("cmoIncludeFreightInPrice");
				eRPOrganizationInformationDto.cmoPrintStatement = dataTable.Rows[i].Field<bool>("cmoPrintStatement");
				eRPOrganizationInformationDto.cmoRequires1099 = dataTable.Rows[i].Field<bool>("cmoRequires1099");
				eRPOrganizationInformationDto.cmoRequiresInspection = dataTable.Rows[i].Field<bool>("cmoRequiresInspection");
				eRPOrganizationInformationDto.cmoResidentialAddress = dataTable.Rows[i].Field<bool>("cmoResidentialAddress");
				eRPOrganizationInformationDto.cmoSuperFund = dataTable.Rows[i].Field<bool>("cmoSuperFund");
				eRPOrganizationInformationDto.cmoSupplierAccredited = dataTable.Rows[i].Field<bool>("cmoSupplierAccredited");
				eRPOrganizationInformationDto.cmoSupplierTaxable = dataTable.Rows[i].Field<bool>("cmoSupplierTaxable");
				eRPOrganizationInformationDto.cmoTaxReportable = dataTable.Rows[i].Field<bool>("cmoTaxReportable");
				eRPOrganizationInformationDto.cmoUpsValidated = dataTable.Rows[i].Field<bool>("cmoUpsValidated");
				eRPOrganizationInformationDto.cmoJobPriorityID = dataTable.Rows[i].Field<short>("cmoJobPriorityID");
				eRPOrganizationInformationDto.cmoLastName = dataTable.Rows[i].Field<string>("cmoLastName");
				eRPOrganizationInformationDto.cmoLongDescriptionRtf = dataTable.Rows[i].Field<string>("cmoLongDescriptionRtf");
				eRPOrganizationInformationDto.cmoLongDescriptionText = dataTable.Rows[i].Field<string>("cmoLongDescriptionText");
				eRPOrganizationInformationDto.cmoName = dataTable.Rows[i].Field<string>("cmoName");
				eRPOrganizationInformationDto.cmoNonTaxReasonID = dataTable.Rows[i].Field<string>("cmoNonTaxReasonID");
				eRPOrganizationInformationDto.cmoOrganizationAccountID = dataTable.Rows[i].Field<string>("cmoOrganizationAccountID");
				eRPOrganizationInformationDto.cmoPhoneNumber = dataTable.Rows[i].Field<string>("cmoPhoneNumber");
				eRPOrganizationInformationDto.cmoPostCode = dataTable.Rows[i].Field<string>("cmoPostCode");
				eRPOrganizationInformationDto.cmoPurchaseContactID = dataTable.Rows[i].Field<string>("cmoPurchaseContactID");
				eRPOrganizationInformationDto.cmoQuoteContactID = dataTable.Rows[i].Field<string>("cmoQuoteContactID");
				eRPOrganizationInformationDto.cmoResellerActiveDate = dataTable.Rows[i].Field<DateTime?>("cmoResellerActiveDate");
				eRPOrganizationInformationDto.cmoResellerCommissionRate = dataTable.Rows[i].Field<decimal>("cmoResellerCommissionRate");
				eRPOrganizationInformationDto.cmoResellerContactID = dataTable.Rows[i].Field<string>("cmoResellerContactID");
				eRPOrganizationInformationDto.cmoResellerInactiveDate = dataTable.Rows[i].Field<DateTime?>("cmoResellerInactiveDate");
				eRPOrganizationInformationDto.cmoResellerLocationID = dataTable.Rows[i].Field<string>("cmoResellerLocationID");
				eRPOrganizationInformationDto.cmoResellerOrganizationID = dataTable.Rows[i].Field<string>("cmoResellerOrganizationID");
				eRPOrganizationInformationDto.cmoResellerProspectDate = dataTable.Rows[i].Field<DateTime?>("cmoResellerProspectDate");
				eRPOrganizationInformationDto.cmoResellerStatus = dataTable.Rows[i].Field<byte>("cmoResellerStatus");
				eRPOrganizationInformationDto.cmoRowVersion = dataTable.Rows[i].Field<byte[]>("cmoRowVersion");
				eRPOrganizationInformationDto.cmoSecondGivenName = dataTable.Rows[i].Field<string>("cmoSecondGivenName");
				eRPOrganizationInformationDto.cmoShipContactID = dataTable.Rows[i].Field<string>("cmoShipContactID");
				eRPOrganizationInformationDto.cmoSplitPercentTotal = dataTable.Rows[i].Field<decimal>("cmoSplitPercentTotal");
				eRPOrganizationInformationDto.cmoState = dataTable.Rows[i].Field<string>("cmoState");
				eRPOrganizationInformationDto.cmoSuperFundEmployerID = dataTable.Rows[i].Field<string>("cmoSuperFundEmployerID");
				eRPOrganizationInformationDto.cmoSuperFundName = dataTable.Rows[i].Field<string>("cmoSuperFundName");
				eRPOrganizationInformationDto.cmoSupplierAccreditedDate = dataTable.Rows[i].Field<DateTime?>("cmoSupplierAccreditedDate");
				eRPOrganizationInformationDto.cmoSupplierActiveDate = dataTable.Rows[i].Field<DateTime?>("cmoSupplierActiveDate");
				eRPOrganizationInformationDto.cmoSupplierInactiveDate = dataTable.Rows[i].Field<DateTime?>("cmoSupplierInactiveDate");
				eRPOrganizationInformationDto.cmoSupplierPaymentTermID = dataTable.Rows[i].Field<string>("cmoSupplierPaymentTermID");
				eRPOrganizationInformationDto.cmoSupplierProspectDate = dataTable.Rows[i].Field<DateTime?>("cmoSupplierProspectDate");
				eRPOrganizationInformationDto.cmoSupplierRatingID = dataTable.Rows[i].Field<string>("cmoSupplierRatingID");
				eRPOrganizationInformationDto.cmoSupplierSecondTaxCodeID = dataTable.Rows[i].Field<string>("cmoSupplierSecondTaxCodeID");
				eRPOrganizationInformationDto.cmoSupplierShippingMethodID = dataTable.Rows[i].Field<string>("cmoSupplierShippingMethodID");
				eRPOrganizationInformationDto.cmoSupplierStatus = dataTable.Rows[i].Field<byte>("cmoSupplierStatus");
				eRPOrganizationInformationDto.cmoSupplierTaxCodeID = dataTable.Rows[i].Field<string>("cmoSupplierTaxCodeID");
				eRPOrganizationInformationDto.cmoTaxExemptNumber = dataTable.Rows[i].Field<string>("cmoTaxExemptNumber");
				eRPOrganizationInformationDto.cmoTradingName = dataTable.Rows[i].Field<string>("cmoTradingName");
				eRPOrganizationInformationDto.cmoUps3rdPartyLocationID = dataTable.Rows[i].Field<string>("cmoUps3rdPartyLocationID");
				eRPOrganizationInformationDto.cmoUps3rdPartyOrganizationID = dataTable.Rows[i].Field<string>("cmoUps3rdPartyOrganizationID");
				eRPOrganizationInformationDto.cmoUpsAcctNumber = dataTable.Rows[i].Field<string>("cmoUpsAcctNumber");
				eRPOrganizationInformationDto.cmoUpsBillingOption = dataTable.Rows[i].Field<string>("cmoUpsBillingOption");
				eRPOrganizationInformationDto.cmoUpsWsBillingOption = dataTable.Rows[i].Field<string>("cmoUpsWsBillingOption");
				eRPOrganizationInformationDto.cmoUsaTransactionTypeCode = dataTable.Rows[i].Field<string>("cmoUsaTransactionTypeCode");
				eRPOrganizationInformationDto.cmoWebAddress = dataTable.Rows[i].Field<string>("cmoWebAddress");
				eRPOrganizationInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPOrganizationInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPOrganizationInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPOrganizationInformationDto> GetOrganization(Guid organizationId)
	{
		ERPOrganizationInformationDto eRPOrganizationInformationDto = new ERPOrganizationInformationDto();
		InitializeParameterLists();
		string[] collection = new string[135]
		{
			"cmoAccountManagerEmployeeID", "cmoAddressLine1", "cmoAddressLine2", "cmoAddressLine3", "cmoAddressValidationResult", "cmoAlternatePhoneNumber", "cmoApInvoiceContactID", "cmoArInvoiceContactID", "cmoAttachmentFileFolder", "cmoAvalaraUseCodes",
			"cmoBankAccountName", "cmoBankAccountNumber", "cmoBankAccountType", "cmoBankInitials", "cmoBic", "cmoBsbNumber", "cmoCity", "cmoOrganizationID", "cmoCompanyEntryDescription", "cmoCountry",
			"cmoCountryCode", "cmoCounty", "cmoCreatedBy", "cmoCreatedDate", "cmoCurrencyRateID", "cmoCustomerActiveDate", "cmoCustomerCreditLimit", "cmoCustomerGroupID", "cmoCustomerInactiveDate", "cmoCustomerPaymentTermsID",
			"cmoCustomerProspectDate", "cmoCustomerSecondTaxCodeID", "cmoCustomerShipPaymentTypeID", "cmoCustomerShippingCarrier", "cmoCustomerShippingMethodID", "cmoCustomerStatus", "cmoCustomerTaxCodeID", "cmoDefaultApInvoiceLocationID", "cmoDefaultArInvoiceLocationID", "cmoDefaultPurchaseLocationID",
			"cmoDefaultQuoteLocationID", "cmoDefaultShipLocationID", "cmoDropShipLocationID", "cmoDropShipOrganizationID", "cmoEftCode", "cmoEftDescription", "cmoEftParticulars", "cmoEmailAddress", "cmoEmployeeCount", "cmoUniqueID",
			"cmoEstablishedDate", "cmoExpenseSplitPercentTotal", "cmoFaxNumber", "cmoFederalID", "cmoFedEx3rdPartyLocationID", "cmoFedEx3rdPartyOrganizationID", "cmoFedExAccountNumber", "cmoFedExBillingOption", "cmoFirstGivenName", "cmoForm1099Box",
			"cmoFreeOnBoardDescription", "cmoHdAttachmentFilePath", "cmoIban", "cmoIntraCompanyDatasetID", "cmoApIncludeTaxInRetention", "cmoArIncludeTaxInRetention", "cmoArInvoicePerShipmentLine", "cmoAvalaraAddressValidated", "cmoBareCostOfDuty", "cmoBareTransportationCost",
			"cmoCalculateFinanceCharges", "cmoCompetitor", "cmoContractor", "cmoCreatedFromMobile", "cmoCreditHold", "cmoCustomerTaxable", "cmoDirectPayment", "cmoEdiIntegrated", "cmoFinanceCompany", "cmoIgnoreAvalara",
			"cmoIncludeFreightInPrice", "cmoPrintStatement", "cmoRequires1099", "cmoRequiresInspection", "cmoResidentialAddress", "cmoSuperFund", "cmoSupplierAccredited", "cmoSupplierTaxable", "cmoTaxReportable", "cmoUpsValidated",
			"cmoJobPriorityID", "cmoLastName", "cmoLongDescriptionRtf", "cmoLongDescriptionText", "cmoName", "cmoNonTaxReasonID", "cmoOrganizationAccountID", "cmoPhoneNumber", "cmoPostCode", "cmoPurchaseContactID",
			"cmoQuoteContactID", "cmoResellerActiveDate", "cmoResellerCommissionRate", "cmoResellerContactID", "cmoResellerInactiveDate", "cmoResellerLocationID", "cmoResellerOrganizationID", "cmoResellerProspectDate", "cmoResellerStatus", "cmoRowVersion",
			"cmoSecondGivenName", "cmoShipContactID", "cmoSplitPercentTotal", "cmoState", "cmoSuperFundEmployerID", "cmoSuperFundName", "cmoSupplierAccreditedDate", "cmoSupplierActiveDate", "cmoSupplierInactiveDate", "cmoSupplierPaymentTermID",
			"cmoSupplierProspectDate", "cmoSupplierRatingID", "cmoSupplierSecondTaxCodeID", "cmoSupplierShippingMethodID", "cmoSupplierStatus", "cmoSupplierTaxCodeID", "cmoTaxExemptNumber", "cmoTradingName", "cmoUps3rdPartyLocationID", "cmoUps3rdPartyOrganizationID",
			"cmoUpsAcctNumber", "cmoUpsBillingOption", "cmoUpsWsBillingOption", "cmoUsaTransactionTypeCode", "cmoWebAddress"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("cmoUniqueID|C", organizationId);
		AddCustomFieldsToSelectList("Organizations");
		using (DataTable dataTable = GetAsDataTable("Organizations", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPOrganizationInformationDto);
			}
			eRPOrganizationInformationDto.cmoAccountManagerEmployeeID = dataTable.Rows[0].Field<string>("cmoAccountManagerEmployeeID");
			eRPOrganizationInformationDto.cmoAddressLine1 = dataTable.Rows[0].Field<string>("cmoAddressLine1");
			eRPOrganizationInformationDto.cmoAddressLine2 = dataTable.Rows[0].Field<string>("cmoAddressLine2");
			eRPOrganizationInformationDto.cmoAddressLine3 = dataTable.Rows[0].Field<string>("cmoAddressLine3");
			eRPOrganizationInformationDto.cmoAddressValidationResult = dataTable.Rows[0].Field<string>("cmoAddressValidationResult");
			eRPOrganizationInformationDto.cmoAlternatePhoneNumber = dataTable.Rows[0].Field<string>("cmoAlternatePhoneNumber");
			eRPOrganizationInformationDto.cmoApInvoiceContactID = dataTable.Rows[0].Field<string>("cmoApInvoiceContactID");
			eRPOrganizationInformationDto.cmoArInvoiceContactID = dataTable.Rows[0].Field<string>("cmoArInvoiceContactID");
			eRPOrganizationInformationDto.cmoAttachmentFileFolder = dataTable.Rows[0].Field<string>("cmoAttachmentFileFolder");
			eRPOrganizationInformationDto.cmoAvalaraUseCodes = dataTable.Rows[0].Field<string>("cmoAvalaraUseCodes");
			eRPOrganizationInformationDto.cmoBankAccountName = dataTable.Rows[0].Field<string>("cmoBankAccountName");
			eRPOrganizationInformationDto.cmoBankAccountNumber = dataTable.Rows[0].Field<string>("cmoBankAccountNumber");
			eRPOrganizationInformationDto.cmoBankAccountType = dataTable.Rows[0].Field<string>("cmoBankAccountType");
			eRPOrganizationInformationDto.cmoBankInitials = dataTable.Rows[0].Field<string>("cmoBankInitials");
			eRPOrganizationInformationDto.cmoBic = dataTable.Rows[0].Field<string>("cmoBic");
			eRPOrganizationInformationDto.cmoBsbNumber = dataTable.Rows[0].Field<string>("cmoBsbNumber");
			eRPOrganizationInformationDto.cmoCity = dataTable.Rows[0].Field<string>("cmoCity");
			eRPOrganizationInformationDto.cmoOrganizationID = dataTable.Rows[0].Field<string>("cmoOrganizationID");
			eRPOrganizationInformationDto.cmoCompanyEntryDescription = dataTable.Rows[0].Field<string>("cmoCompanyEntryDescription");
			eRPOrganizationInformationDto.cmoCountry = dataTable.Rows[0].Field<string>("cmoCountry");
			eRPOrganizationInformationDto.cmoCountryCode = dataTable.Rows[0].Field<string>("cmoCountryCode");
			eRPOrganizationInformationDto.cmoCounty = dataTable.Rows[0].Field<string>("cmoCounty");
			eRPOrganizationInformationDto.cmoCreatedBy = dataTable.Rows[0].Field<string>("cmoCreatedBy");
			eRPOrganizationInformationDto.cmoCreatedDate = dataTable.Rows[0].Field<DateTime?>("cmoCreatedDate");
			eRPOrganizationInformationDto.cmoCurrencyRateID = dataTable.Rows[0].Field<string>("cmoCurrencyRateID");
			eRPOrganizationInformationDto.cmoCustomerActiveDate = dataTable.Rows[0].Field<DateTime?>("cmoCustomerActiveDate");
			eRPOrganizationInformationDto.cmoCustomerCreditLimit = dataTable.Rows[0].Field<decimal>("cmoCustomerCreditLimit");
			eRPOrganizationInformationDto.cmoCustomerGroupID = dataTable.Rows[0].Field<string>("cmoCustomerGroupID");
			eRPOrganizationInformationDto.cmoCustomerInactiveDate = dataTable.Rows[0].Field<DateTime?>("cmoCustomerInactiveDate");
			eRPOrganizationInformationDto.cmoCustomerPaymentTermsID = dataTable.Rows[0].Field<string>("cmoCustomerPaymentTermsID");
			eRPOrganizationInformationDto.cmoCustomerProspectDate = dataTable.Rows[0].Field<DateTime?>("cmoCustomerProspectDate");
			eRPOrganizationInformationDto.cmoCustomerSecondTaxCodeID = dataTable.Rows[0].Field<string>("cmoCustomerSecondTaxCodeID");
			eRPOrganizationInformationDto.cmoCustomerShipPaymentTypeID = dataTable.Rows[0].Field<string>("cmoCustomerShipPaymentTypeID");
			eRPOrganizationInformationDto.cmoCustomerShippingCarrier = dataTable.Rows[0].Field<string>("cmoCustomerShippingCarrier");
			eRPOrganizationInformationDto.cmoCustomerShippingMethodID = dataTable.Rows[0].Field<string>("cmoCustomerShippingMethodID");
			eRPOrganizationInformationDto.cmoCustomerStatus = dataTable.Rows[0].Field<byte>("cmoCustomerStatus");
			eRPOrganizationInformationDto.cmoCustomerTaxCodeID = dataTable.Rows[0].Field<string>("cmoCustomerTaxCodeID");
			eRPOrganizationInformationDto.cmoDefaultApInvoiceLocationID = dataTable.Rows[0].Field<string>("cmoDefaultApInvoiceLocationID");
			eRPOrganizationInformationDto.cmoDefaultArInvoiceLocationID = dataTable.Rows[0].Field<string>("cmoDefaultArInvoiceLocationID");
			eRPOrganizationInformationDto.cmoDefaultPurchaseLocationID = dataTable.Rows[0].Field<string>("cmoDefaultPurchaseLocationID");
			eRPOrganizationInformationDto.cmoDefaultQuoteLocationID = dataTable.Rows[0].Field<string>("cmoDefaultQuoteLocationID");
			eRPOrganizationInformationDto.cmoDefaultShipLocationID = dataTable.Rows[0].Field<string>("cmoDefaultShipLocationID");
			eRPOrganizationInformationDto.cmoDropShipLocationID = dataTable.Rows[0].Field<string>("cmoDropShipLocationID");
			eRPOrganizationInformationDto.cmoDropShipOrganizationID = dataTable.Rows[0].Field<string>("cmoDropShipOrganizationID");
			eRPOrganizationInformationDto.cmoEftCode = dataTable.Rows[0].Field<string>("cmoEftCode");
			eRPOrganizationInformationDto.cmoEftDescription = dataTable.Rows[0].Field<string>("cmoEftDescription");
			eRPOrganizationInformationDto.cmoEftParticulars = dataTable.Rows[0].Field<string>("cmoEftParticulars");
			eRPOrganizationInformationDto.cmoEmailAddress = dataTable.Rows[0].Field<string>("cmoEmailAddress");
			eRPOrganizationInformationDto.cmoEmployeeCount = dataTable.Rows[0].Field<int>("cmoEmployeeCount");
			eRPOrganizationInformationDto.cmoUniqueID = dataTable.Rows[0].Field<Guid>("cmoUniqueID");
			eRPOrganizationInformationDto.cmoEstablishedDate = dataTable.Rows[0].Field<DateTime?>("cmoEstablishedDate");
			eRPOrganizationInformationDto.cmoExpenseSplitPercentTotal = dataTable.Rows[0].Field<decimal>("cmoExpenseSplitPercentTotal");
			eRPOrganizationInformationDto.cmoFaxNumber = dataTable.Rows[0].Field<string>("cmoFaxNumber");
			eRPOrganizationInformationDto.cmoFederalID = dataTable.Rows[0].Field<string>("cmoFederalID");
			eRPOrganizationInformationDto.cmoFedEx3rdPartyLocationID = dataTable.Rows[0].Field<string>("cmoFedEx3rdPartyLocationID");
			eRPOrganizationInformationDto.cmoFedEx3rdPartyOrganizationID = dataTable.Rows[0].Field<string>("cmoFedEx3rdPartyOrganizationID");
			eRPOrganizationInformationDto.cmoFedExAccountNumber = dataTable.Rows[0].Field<string>("cmoFedExAccountNumber");
			eRPOrganizationInformationDto.cmoFedExBillingOption = dataTable.Rows[0].Field<string>("cmoFedExBillingOption");
			eRPOrganizationInformationDto.cmoFirstGivenName = dataTable.Rows[0].Field<string>("cmoFirstGivenName");
			eRPOrganizationInformationDto.cmoForm1099Box = dataTable.Rows[0].Field<byte>("cmoForm1099Box");
			eRPOrganizationInformationDto.cmoFreeOnBoardDescription = dataTable.Rows[0].Field<string>("cmoFreeOnBoardDescription");
			eRPOrganizationInformationDto.cmoHdAttachmentFilePath = dataTable.Rows[0].Field<string>("cmoHdAttachmentFilePath");
			eRPOrganizationInformationDto.cmoIban = dataTable.Rows[0].Field<string>("cmoIban");
			eRPOrganizationInformationDto.cmoIntraCompanyDatasetID = dataTable.Rows[0].Field<string>("cmoIntraCompanyDatasetID");
			eRPOrganizationInformationDto.cmoApIncludeTaxInRetention = dataTable.Rows[0].Field<bool>("cmoApIncludeTaxInRetention");
			eRPOrganizationInformationDto.cmoArIncludeTaxInRetention = dataTable.Rows[0].Field<bool>("cmoArIncludeTaxInRetention");
			eRPOrganizationInformationDto.cmoArInvoicePerShipmentLine = dataTable.Rows[0].Field<bool>("cmoArInvoicePerShipmentLine");
			eRPOrganizationInformationDto.cmoAvalaraAddressValidated = dataTable.Rows[0].Field<bool>("cmoAvalaraAddressValidated");
			eRPOrganizationInformationDto.cmoBareCostOfDuty = dataTable.Rows[0].Field<bool>("cmoBareCostOfDuty");
			eRPOrganizationInformationDto.cmoBareTransportationCost = dataTable.Rows[0].Field<bool>("cmoBareTransportationCost");
			eRPOrganizationInformationDto.cmoCalculateFinanceCharges = dataTable.Rows[0].Field<bool>("cmoCalculateFinanceCharges");
			eRPOrganizationInformationDto.cmoCompetitor = dataTable.Rows[0].Field<bool>("cmoCompetitor");
			eRPOrganizationInformationDto.cmoContractor = dataTable.Rows[0].Field<bool>("cmoContractor");
			eRPOrganizationInformationDto.cmoCreatedFromMobile = dataTable.Rows[0].Field<bool>("cmoCreatedFromMobile");
			eRPOrganizationInformationDto.cmoCreditHold = dataTable.Rows[0].Field<bool>("cmoCreditHold");
			eRPOrganizationInformationDto.cmoCustomerTaxable = dataTable.Rows[0].Field<bool>("cmoCustomerTaxable");
			eRPOrganizationInformationDto.cmoDirectPayment = dataTable.Rows[0].Field<bool>("cmoDirectPayment");
			eRPOrganizationInformationDto.cmoEdiIntegrated = dataTable.Rows[0].Field<bool>("cmoEdiIntegrated");
			eRPOrganizationInformationDto.cmoFinanceCompany = dataTable.Rows[0].Field<bool>("cmoFinanceCompany");
			eRPOrganizationInformationDto.cmoIgnoreAvalara = dataTable.Rows[0].Field<bool>("cmoIgnoreAvalara");
			eRPOrganizationInformationDto.cmoIncludeFreightInPrice = dataTable.Rows[0].Field<bool>("cmoIncludeFreightInPrice");
			eRPOrganizationInformationDto.cmoPrintStatement = dataTable.Rows[0].Field<bool>("cmoPrintStatement");
			eRPOrganizationInformationDto.cmoRequires1099 = dataTable.Rows[0].Field<bool>("cmoRequires1099");
			eRPOrganizationInformationDto.cmoRequiresInspection = dataTable.Rows[0].Field<bool>("cmoRequiresInspection");
			eRPOrganizationInformationDto.cmoResidentialAddress = dataTable.Rows[0].Field<bool>("cmoResidentialAddress");
			eRPOrganizationInformationDto.cmoSuperFund = dataTable.Rows[0].Field<bool>("cmoSuperFund");
			eRPOrganizationInformationDto.cmoSupplierAccredited = dataTable.Rows[0].Field<bool>("cmoSupplierAccredited");
			eRPOrganizationInformationDto.cmoSupplierTaxable = dataTable.Rows[0].Field<bool>("cmoSupplierTaxable");
			eRPOrganizationInformationDto.cmoTaxReportable = dataTable.Rows[0].Field<bool>("cmoTaxReportable");
			eRPOrganizationInformationDto.cmoUpsValidated = dataTable.Rows[0].Field<bool>("cmoUpsValidated");
			eRPOrganizationInformationDto.cmoJobPriorityID = dataTable.Rows[0].Field<short>("cmoJobPriorityID");
			eRPOrganizationInformationDto.cmoLastName = dataTable.Rows[0].Field<string>("cmoLastName");
			eRPOrganizationInformationDto.cmoLongDescriptionRtf = dataTable.Rows[0].Field<string>("cmoLongDescriptionRtf");
			eRPOrganizationInformationDto.cmoLongDescriptionText = dataTable.Rows[0].Field<string>("cmoLongDescriptionText");
			eRPOrganizationInformationDto.cmoName = dataTable.Rows[0].Field<string>("cmoName");
			eRPOrganizationInformationDto.cmoNonTaxReasonID = dataTable.Rows[0].Field<string>("cmoNonTaxReasonID");
			eRPOrganizationInformationDto.cmoOrganizationAccountID = dataTable.Rows[0].Field<string>("cmoOrganizationAccountID");
			eRPOrganizationInformationDto.cmoPhoneNumber = dataTable.Rows[0].Field<string>("cmoPhoneNumber");
			eRPOrganizationInformationDto.cmoPostCode = dataTable.Rows[0].Field<string>("cmoPostCode");
			eRPOrganizationInformationDto.cmoPurchaseContactID = dataTable.Rows[0].Field<string>("cmoPurchaseContactID");
			eRPOrganizationInformationDto.cmoQuoteContactID = dataTable.Rows[0].Field<string>("cmoQuoteContactID");
			eRPOrganizationInformationDto.cmoResellerActiveDate = dataTable.Rows[0].Field<DateTime?>("cmoResellerActiveDate");
			eRPOrganizationInformationDto.cmoResellerCommissionRate = dataTable.Rows[0].Field<decimal>("cmoResellerCommissionRate");
			eRPOrganizationInformationDto.cmoResellerContactID = dataTable.Rows[0].Field<string>("cmoResellerContactID");
			eRPOrganizationInformationDto.cmoResellerInactiveDate = dataTable.Rows[0].Field<DateTime?>("cmoResellerInactiveDate");
			eRPOrganizationInformationDto.cmoResellerLocationID = dataTable.Rows[0].Field<string>("cmoResellerLocationID");
			eRPOrganizationInformationDto.cmoResellerOrganizationID = dataTable.Rows[0].Field<string>("cmoResellerOrganizationID");
			eRPOrganizationInformationDto.cmoResellerProspectDate = dataTable.Rows[0].Field<DateTime?>("cmoResellerProspectDate");
			eRPOrganizationInformationDto.cmoResellerStatus = dataTable.Rows[0].Field<byte>("cmoResellerStatus");
			eRPOrganizationInformationDto.cmoRowVersion = dataTable.Rows[0].Field<byte[]>("cmoRowVersion");
			eRPOrganizationInformationDto.cmoSecondGivenName = dataTable.Rows[0].Field<string>("cmoSecondGivenName");
			eRPOrganizationInformationDto.cmoShipContactID = dataTable.Rows[0].Field<string>("cmoShipContactID");
			eRPOrganizationInformationDto.cmoSplitPercentTotal = dataTable.Rows[0].Field<decimal>("cmoSplitPercentTotal");
			eRPOrganizationInformationDto.cmoState = dataTable.Rows[0].Field<string>("cmoState");
			eRPOrganizationInformationDto.cmoSuperFundEmployerID = dataTable.Rows[0].Field<string>("cmoSuperFundEmployerID");
			eRPOrganizationInformationDto.cmoSuperFundName = dataTable.Rows[0].Field<string>("cmoSuperFundName");
			eRPOrganizationInformationDto.cmoSupplierAccreditedDate = dataTable.Rows[0].Field<DateTime?>("cmoSupplierAccreditedDate");
			eRPOrganizationInformationDto.cmoSupplierActiveDate = dataTable.Rows[0].Field<DateTime?>("cmoSupplierActiveDate");
			eRPOrganizationInformationDto.cmoSupplierInactiveDate = dataTable.Rows[0].Field<DateTime?>("cmoSupplierInactiveDate");
			eRPOrganizationInformationDto.cmoSupplierPaymentTermID = dataTable.Rows[0].Field<string>("cmoSupplierPaymentTermID");
			eRPOrganizationInformationDto.cmoSupplierProspectDate = dataTable.Rows[0].Field<DateTime?>("cmoSupplierProspectDate");
			eRPOrganizationInformationDto.cmoSupplierRatingID = dataTable.Rows[0].Field<string>("cmoSupplierRatingID");
			eRPOrganizationInformationDto.cmoSupplierSecondTaxCodeID = dataTable.Rows[0].Field<string>("cmoSupplierSecondTaxCodeID");
			eRPOrganizationInformationDto.cmoSupplierShippingMethodID = dataTable.Rows[0].Field<string>("cmoSupplierShippingMethodID");
			eRPOrganizationInformationDto.cmoSupplierStatus = dataTable.Rows[0].Field<byte>("cmoSupplierStatus");
			eRPOrganizationInformationDto.cmoSupplierTaxCodeID = dataTable.Rows[0].Field<string>("cmoSupplierTaxCodeID");
			eRPOrganizationInformationDto.cmoTaxExemptNumber = dataTable.Rows[0].Field<string>("cmoTaxExemptNumber");
			eRPOrganizationInformationDto.cmoTradingName = dataTable.Rows[0].Field<string>("cmoTradingName");
			eRPOrganizationInformationDto.cmoUps3rdPartyLocationID = dataTable.Rows[0].Field<string>("cmoUps3rdPartyLocationID");
			eRPOrganizationInformationDto.cmoUps3rdPartyOrganizationID = dataTable.Rows[0].Field<string>("cmoUps3rdPartyOrganizationID");
			eRPOrganizationInformationDto.cmoUpsAcctNumber = dataTable.Rows[0].Field<string>("cmoUpsAcctNumber");
			eRPOrganizationInformationDto.cmoUpsBillingOption = dataTable.Rows[0].Field<string>("cmoUpsBillingOption");
			eRPOrganizationInformationDto.cmoUpsWsBillingOption = dataTable.Rows[0].Field<string>("cmoUpsWsBillingOption");
			eRPOrganizationInformationDto.cmoUsaTransactionTypeCode = dataTable.Rows[0].Field<string>("cmoUsaTransactionTypeCode");
			eRPOrganizationInformationDto.cmoWebAddress = dataTable.Rows[0].Field<string>("cmoWebAddress");
			eRPOrganizationInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPOrganizationInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPOrganizationInformationDto);
	}

	public Task<APIValidationInfoDto> SaveOrganization(ERPOrganizationDto organization)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Organizations WHERE cmoUniqueID = " + M1Util.ConvertToLinq(organization.cmoUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cmoOrganizationID"] = organization.cmoOrganizationID.ToUpper();
				organization.cmoUniqueID = ((organization.cmoUniqueID == Guid.Empty) ? Guid.NewGuid() : organization.cmoUniqueID);
				dataRow["cmoUniqueID"] = organization.cmoUniqueID;
				dataRow["cmoCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cmoCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Organization could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (organization.cmoRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Organization is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cmoRowVersion"], organization.cmoRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Organization has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Organization again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["cmoAccountManagerEmployeeID"] = organization.cmoAccountManagerEmployeeID;
			dataRow["cmoAddressLine1"] = organization.cmoAddressLine1;
			dataRow["cmoAddressLine2"] = organization.cmoAddressLine2;
			dataRow["cmoAddressLine3"] = organization.cmoAddressLine3;
			dataRow["cmoAddressValidationResult"] = organization.cmoAddressValidationResult ?? dataRow["cmoAddressValidationResult"];
			dataRow["cmoAlternatePhoneNumber"] = organization.cmoAlternatePhoneNumber;
			dataRow["cmoApInvoiceContactID"] = organization.cmoApInvoiceContactID;
			dataRow["cmoArInvoiceContactID"] = organization.cmoArInvoiceContactID;
			dataRow["cmoAttachmentFileFolder"] = organization.cmoAttachmentFileFolder ?? dataRow["cmoAttachmentFileFolder"];
			dataRow["cmoAvalaraUseCodes"] = organization.cmoAvalaraUseCodes;
			dataRow["cmoBankAccountName"] = organization.cmoBankAccountName;
			dataRow["cmoBankAccountNumber"] = organization.cmoBankAccountNumber;
			dataRow["cmoBankAccountType"] = organization.cmoBankAccountType;
			dataRow["cmoBankInitials"] = organization.cmoBankInitials;
			dataRow["cmoBic"] = organization.cmoBic;
			dataRow["cmoBsbNumber"] = organization.cmoBsbNumber;
			dataRow["cmoCity"] = organization.cmoCity;
			dataRow["cmoCompanyEntryDescription"] = organization.cmoCompanyEntryDescription;
			dataRow["cmoCountry"] = organization.cmoCountry;
			dataRow["cmoCountryCode"] = organization.cmoCountryCode;
			dataRow["cmoCounty"] = organization.cmoCounty;
			dataRow["cmoCurrencyRateID"] = organization.cmoCurrencyRateID;
			DataRow dataRow2 = dataRow;
			DateTime? cmoCustomerActiveDate = organization.cmoCustomerActiveDate;
			dataRow2["cmoCustomerActiveDate"] = (cmoCustomerActiveDate.HasValue ? ((object)cmoCustomerActiveDate.GetValueOrDefault()) : dataRow["cmoCustomerActiveDate"]);
			dataRow["cmoCustomerCreditLimit"] = organization.cmoCustomerCreditLimit;
			dataRow["cmoCustomerGroupID"] = organization.cmoCustomerGroupID;
			DataRow dataRow3 = dataRow;
			cmoCustomerActiveDate = organization.cmoCustomerInactiveDate;
			dataRow3["cmoCustomerInactiveDate"] = (cmoCustomerActiveDate.HasValue ? ((object)cmoCustomerActiveDate.GetValueOrDefault()) : dataRow["cmoCustomerInactiveDate"]);
			dataRow["cmoCustomerPaymentTermsID"] = organization.cmoCustomerPaymentTermsID;
			DataRow dataRow4 = dataRow;
			cmoCustomerActiveDate = organization.cmoCustomerProspectDate;
			dataRow4["cmoCustomerProspectDate"] = (cmoCustomerActiveDate.HasValue ? ((object)cmoCustomerActiveDate.GetValueOrDefault()) : dataRow["cmoCustomerProspectDate"]);
			dataRow["cmoCustomerSecondTaxCodeID"] = organization.cmoCustomerSecondTaxCodeID;
			dataRow["cmoCustomerShipPaymentTypeID"] = organization.cmoCustomerShipPaymentTypeID;
			dataRow["cmoCustomerShippingCarrier"] = organization.cmoCustomerShippingCarrier;
			dataRow["cmoCustomerShippingMethodID"] = organization.cmoCustomerShippingMethodID;
			dataRow["cmoCustomerStatus"] = organization.cmoCustomerStatus;
			dataRow["cmoCustomerTaxCodeID"] = organization.cmoCustomerTaxCodeID;
			dataRow["cmoDefaultApInvoiceLocationID"] = organization.cmoDefaultApInvoiceLocationID;
			dataRow["cmoDefaultArInvoiceLocationID"] = organization.cmoDefaultArInvoiceLocationID;
			dataRow["cmoDefaultPurchaseLocationID"] = organization.cmoDefaultPurchaseLocationID;
			dataRow["cmoDefaultQuoteLocationID"] = organization.cmoDefaultQuoteLocationID;
			dataRow["cmoDefaultShipLocationID"] = organization.cmoDefaultShipLocationID;
			dataRow["cmoDropShipLocationID"] = organization.cmoDropShipLocationID;
			dataRow["cmoDropShipOrganizationID"] = organization.cmoDropShipOrganizationID;
			dataRow["cmoEftCode"] = organization.cmoEftCode;
			dataRow["cmoEftDescription"] = organization.cmoEftDescription;
			dataRow["cmoEftParticulars"] = organization.cmoEftParticulars;
			dataRow["cmoEmailAddress"] = organization.cmoEmailAddress ?? dataRow["cmoEmailAddress"];
			dataRow["cmoEmployeeCount"] = organization.cmoEmployeeCount;
			DataRow dataRow5 = dataRow;
			cmoCustomerActiveDate = organization.cmoEstablishedDate;
			dataRow5["cmoEstablishedDate"] = (cmoCustomerActiveDate.HasValue ? ((object)cmoCustomerActiveDate.GetValueOrDefault()) : dataRow["cmoEstablishedDate"]);
			dataRow["cmoExpenseSplitPercentTotal"] = organization.cmoExpenseSplitPercentTotal;
			dataRow["cmoFaxNumber"] = organization.cmoFaxNumber;
			dataRow["cmoFederalID"] = organization.cmoFederalID;
			dataRow["cmoFedEx3rdPartyLocationID"] = organization.cmoFedEx3rdPartyLocationID;
			dataRow["cmoFedEx3rdPartyOrganizationID"] = organization.cmoFedEx3rdPartyOrganizationID;
			dataRow["cmoFedExAccountNumber"] = organization.cmoFedExAccountNumber;
			dataRow["cmoFedExBillingOption"] = organization.cmoFedExBillingOption;
			dataRow["cmoFirstGivenName"] = organization.cmoFirstGivenName;
			dataRow["cmoForm1099Box"] = organization.cmoForm1099Box;
			dataRow["cmoFreeOnBoardDescription"] = organization.cmoFreeOnBoardDescription;
			dataRow["cmoHdAttachmentFilePath"] = organization.cmoHdAttachmentFilePath ?? dataRow["cmoHdAttachmentFilePath"];
			dataRow["cmoIban"] = organization.cmoIban;
			dataRow["cmoIntraCompanyDatasetID"] = organization.cmoIntraCompanyDatasetID;
			dataRow["cmoApIncludeTaxInRetention"] = organization.cmoApIncludeTaxInRetention;
			dataRow["cmoArIncludeTaxInRetention"] = organization.cmoArIncludeTaxInRetention;
			dataRow["cmoArInvoicePerShipmentLine"] = organization.cmoArInvoicePerShipmentLine;
			dataRow["cmoAvalaraAddressValidated"] = organization.cmoAvalaraAddressValidated;
			dataRow["cmoBareCostOfDuty"] = organization.cmoBareCostOfDuty;
			dataRow["cmoBareTransportationCost"] = organization.cmoBareTransportationCost;
			dataRow["cmoCalculateFinanceCharges"] = organization.cmoCalculateFinanceCharges;
			dataRow["cmoCompetitor"] = organization.cmoCompetitor;
			dataRow["cmoContractor"] = organization.cmoContractor;
			dataRow["cmoCreatedFromMobile"] = organization.cmoCreatedFromMobile;
			dataRow["cmoCreditHold"] = organization.cmoCreditHold;
			dataRow["cmoCustomerTaxable"] = organization.cmoCustomerTaxable;
			dataRow["cmoDirectPayment"] = organization.cmoDirectPayment;
			dataRow["cmoEdiIntegrated"] = organization.cmoEdiIntegrated;
			dataRow["cmoFinanceCompany"] = organization.cmoFinanceCompany;
			dataRow["cmoIgnoreAvalara"] = organization.cmoIgnoreAvalara;
			dataRow["cmoIncludeFreightInPrice"] = organization.cmoIncludeFreightInPrice;
			dataRow["cmoPrintStatement"] = organization.cmoPrintStatement;
			dataRow["cmoRequires1099"] = organization.cmoRequires1099;
			dataRow["cmoRequiresInspection"] = organization.cmoRequiresInspection;
			dataRow["cmoResidentialAddress"] = organization.cmoResidentialAddress;
			dataRow["cmoSuperFund"] = organization.cmoSuperFund;
			dataRow["cmoSupplierAccredited"] = organization.cmoSupplierAccredited;
			dataRow["cmoSupplierTaxable"] = organization.cmoSupplierTaxable;
			dataRow["cmoTaxReportable"] = organization.cmoTaxReportable;
			dataRow["cmoUpsValidated"] = organization.cmoUpsValidated;
			dataRow["cmoJobPriorityID"] = organization.cmoJobPriorityID;
			dataRow["cmoLastName"] = organization.cmoLastName;
			dataRow["cmoLongDescriptionRtf"] = organization.cmoLongDescriptionRtf ?? dataRow["cmoLongDescriptionRtf"];
			dataRow["cmoLongDescriptionText"] = organization.cmoLongDescriptionText ?? dataRow["cmoLongDescriptionText"];
			dataRow["cmoName"] = organization.cmoName;
			dataRow["cmoNonTaxReasonID"] = organization.cmoNonTaxReasonID;
			dataRow["cmoOrganizationAccountID"] = organization.cmoOrganizationAccountID;
			dataRow["cmoPhoneNumber"] = organization.cmoPhoneNumber;
			dataRow["cmoPostCode"] = organization.cmoPostCode;
			dataRow["cmoPurchaseContactID"] = organization.cmoPurchaseContactID;
			dataRow["cmoQuoteContactID"] = organization.cmoQuoteContactID;
			DataRow dataRow6 = dataRow;
			cmoCustomerActiveDate = organization.cmoResellerActiveDate;
			dataRow6["cmoResellerActiveDate"] = (cmoCustomerActiveDate.HasValue ? ((object)cmoCustomerActiveDate.GetValueOrDefault()) : dataRow["cmoResellerActiveDate"]);
			dataRow["cmoResellerCommissionRate"] = organization.cmoResellerCommissionRate;
			dataRow["cmoResellerContactID"] = organization.cmoResellerContactID;
			DataRow dataRow7 = dataRow;
			cmoCustomerActiveDate = organization.cmoResellerInactiveDate;
			dataRow7["cmoResellerInactiveDate"] = (cmoCustomerActiveDate.HasValue ? ((object)cmoCustomerActiveDate.GetValueOrDefault()) : dataRow["cmoResellerInactiveDate"]);
			dataRow["cmoResellerLocationID"] = organization.cmoResellerLocationID;
			dataRow["cmoResellerOrganizationID"] = organization.cmoResellerOrganizationID;
			DataRow dataRow8 = dataRow;
			cmoCustomerActiveDate = organization.cmoResellerProspectDate;
			dataRow8["cmoResellerProspectDate"] = (cmoCustomerActiveDate.HasValue ? ((object)cmoCustomerActiveDate.GetValueOrDefault()) : dataRow["cmoResellerProspectDate"]);
			dataRow["cmoResellerStatus"] = organization.cmoResellerStatus;
			dataRow["cmoSecondGivenName"] = organization.cmoSecondGivenName;
			dataRow["cmoShipContactID"] = organization.cmoShipContactID;
			dataRow["cmoSplitPercentTotal"] = organization.cmoSplitPercentTotal;
			dataRow["cmoState"] = organization.cmoState;
			dataRow["cmoSuperFundEmployerID"] = organization.cmoSuperFundEmployerID;
			dataRow["cmoSuperFundName"] = organization.cmoSuperFundName;
			DataRow dataRow9 = dataRow;
			cmoCustomerActiveDate = organization.cmoSupplierAccreditedDate;
			dataRow9["cmoSupplierAccreditedDate"] = (cmoCustomerActiveDate.HasValue ? ((object)cmoCustomerActiveDate.GetValueOrDefault()) : dataRow["cmoSupplierAccreditedDate"]);
			DataRow dataRow10 = dataRow;
			cmoCustomerActiveDate = organization.cmoSupplierActiveDate;
			dataRow10["cmoSupplierActiveDate"] = (cmoCustomerActiveDate.HasValue ? ((object)cmoCustomerActiveDate.GetValueOrDefault()) : dataRow["cmoSupplierActiveDate"]);
			DataRow dataRow11 = dataRow;
			cmoCustomerActiveDate = organization.cmoSupplierInactiveDate;
			dataRow11["cmoSupplierInactiveDate"] = (cmoCustomerActiveDate.HasValue ? ((object)cmoCustomerActiveDate.GetValueOrDefault()) : dataRow["cmoSupplierInactiveDate"]);
			dataRow["cmoSupplierPaymentTermID"] = organization.cmoSupplierPaymentTermID;
			DataRow dataRow12 = dataRow;
			cmoCustomerActiveDate = organization.cmoSupplierProspectDate;
			dataRow12["cmoSupplierProspectDate"] = (cmoCustomerActiveDate.HasValue ? ((object)cmoCustomerActiveDate.GetValueOrDefault()) : dataRow["cmoSupplierProspectDate"]);
			dataRow["cmoSupplierRatingID"] = organization.cmoSupplierRatingID;
			dataRow["cmoSupplierSecondTaxCodeID"] = organization.cmoSupplierSecondTaxCodeID;
			dataRow["cmoSupplierShippingMethodID"] = organization.cmoSupplierShippingMethodID;
			dataRow["cmoSupplierStatus"] = organization.cmoSupplierStatus;
			dataRow["cmoSupplierTaxCodeID"] = organization.cmoSupplierTaxCodeID;
			dataRow["cmoTaxExemptNumber"] = organization.cmoTaxExemptNumber;
			dataRow["cmoTradingName"] = organization.cmoTradingName ?? dataRow["cmoTradingName"];
			dataRow["cmoUps3rdPartyLocationID"] = organization.cmoUps3rdPartyLocationID;
			dataRow["cmoUps3rdPartyOrganizationID"] = organization.cmoUps3rdPartyOrganizationID;
			dataRow["cmoUpsAcctNumber"] = organization.cmoUpsAcctNumber;
			dataRow["cmoUpsBillingOption"] = organization.cmoUpsBillingOption;
			dataRow["cmoUpsWsBillingOption"] = organization.cmoUpsWsBillingOption;
			dataRow["cmoUsaTransactionTypeCode"] = organization.cmoUsaTransactionTypeCode;
			dataRow["cmoWebAddress"] = organization.cmoWebAddress;
			if (organization.CustomFields != null && organization.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in organization.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Organization [{organization.cmoUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Organization [{organization.cmoUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
