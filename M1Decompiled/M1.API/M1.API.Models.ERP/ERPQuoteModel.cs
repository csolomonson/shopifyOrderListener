using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPQuoteModel : ERPBaseModel, IERPQuoteModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllQuotes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPQuoteRepository iERPQuoteRepository = (base.ERPQuoteRepository = new ERPQuoteRepository(base.ApiClientContext));
		using (iERPQuoteRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPQuoteRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPQuoteRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPQuoteRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPQuoteRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetQuote(Guid quoteId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteRepository iERPQuoteRepository = (base.ERPQuoteRepository = new ERPQuoteRepository(base.ApiClientContext));
		using (iERPQuoteRepository)
		{
			if (!(await base.ERPQuoteRepository.DoesQuoteExist(quoteId)))
			{
				errorsList.Add($"Quote [{quoteId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutQuote(ERPQuoteDto quote)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteRepository iERPQuoteRepository = (base.ERPQuoteRepository = new ERPQuoteRepository(base.ApiClientContext));
		using (iERPQuoteRepository)
		{
			if (!string.IsNullOrWhiteSpace(quote.qmpCustomerOrganizationID) && !(await base.ERPQuoteRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { quote.qmpCustomerOrganizationID })))
			{
				errorsList.Add("qmpCustomerOrganizationID [" + quote.qmpCustomerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quote.qmpPlantDepartmentID) && !(await base.ERPQuoteRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { quote.qmpPlantID, quote.qmpPlantDepartmentID })))
			{
				errorsList.Add("qmpPlantDepartmentID [" + quote.qmpPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quote.qmpPlantID) && !(await base.ERPQuoteRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { quote.qmpPlantID })))
			{
				errorsList.Add("qmpPlantID [" + quote.qmpPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quote.qmpArInvoiceLocationID) && !(await base.ERPQuoteRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { quote.qmpCustomerOrganizationID, quote.qmpArInvoiceLocationID })))
			{
				errorsList.Add("qmpArInvoiceLocationID [" + quote.qmpArInvoiceLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quote.qmpArInvoiceContactID) && !(await base.ERPQuoteRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { quote.qmpCustomerOrganizationID, quote.qmpArInvoiceLocationID, quote.qmpArInvoiceContactID })))
			{
				errorsList.Add("qmpArInvoiceContactID [" + quote.qmpArInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quote.qmpQuoteLocationID) && !(await base.ERPQuoteRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { quote.qmpCustomerOrganizationID, quote.qmpQuoteLocationID })))
			{
				errorsList.Add("qmpQuoteLocationID [" + quote.qmpQuoteLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quote.qmpQuoteContactID) && !(await base.ERPQuoteRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { quote.qmpCustomerOrganizationID, quote.qmpQuoteLocationID, quote.qmpQuoteContactID })))
			{
				errorsList.Add("qmpQuoteContactID [" + quote.qmpQuoteContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quote.qmpShipOrganizationID) && !(await base.ERPQuoteRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { quote.qmpShipOrganizationID })))
			{
				errorsList.Add("qmpShipOrganizationID [" + quote.qmpShipOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quote.qmpShipLocationID) && !(await base.ERPQuoteRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { quote.qmpShipOrganizationID, quote.qmpShipLocationID })))
			{
				errorsList.Add("qmpShipLocationID [" + quote.qmpShipLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quote.qmpShipContactID) && !(await base.ERPQuoteRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { quote.qmpShipOrganizationID, quote.qmpShipLocationID, quote.qmpShipContactID })))
			{
				errorsList.Add("qmpShipContactID [" + quote.qmpShipContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quote.qmpStandardMessageID) && !(await base.ERPQuoteRepository.DoesRecordExistInTableUsingKeys("StandardMessages", new object[1] { "XAMSTANDARDMESSAGEID" }, new object[1] { quote.qmpStandardMessageID })))
			{
				errorsList.Add("qmpStandardMessageID [" + quote.qmpStandardMessageID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quote.qmpShippingPaymentTypeID) && !(await base.ERPQuoteRepository.DoesRecordExistInTableUsingKeys("ShippingPaymentTypes", new object[1] { "XAYSHIPPINGPAYMENTTYPEID" }, new object[1] { quote.qmpShippingPaymentTypeID })))
			{
				errorsList.Add("qmpShippingPaymentTypeID [" + quote.qmpShippingPaymentTypeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quote.qmpQuoterEmployeeID) && !(await base.ERPQuoteRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { quote.qmpQuoterEmployeeID })))
			{
				errorsList.Add("qmpQuoterEmployeeID [" + quote.qmpQuoterEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quote.qmpCurrencyRateID) && !(await base.ERPQuoteRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { quote.qmpCurrencyRateID })))
			{
				errorsList.Add("qmpCurrencyRateID [" + quote.qmpCurrencyRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quote.qmpProjectID) && !(await base.ERPQuoteRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { quote.qmpProjectID })))
			{
				errorsList.Add("qmpProjectID [" + quote.qmpProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quote.qmpPaymentTermID) && !(await base.ERPQuoteRepository.DoesRecordExistInTableUsingKeys("PaymentTerms", new object[1] { "XATPAYMENTTERMID" }, new object[1] { quote.qmpPaymentTermID })))
			{
				errorsList.Add("qmpPaymentTermID [" + quote.qmpPaymentTermID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quote.qmpShippingMethodID) && !(await base.ERPQuoteRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { quote.qmpShippingMethodID })))
			{
				errorsList.Add("qmpShippingMethodID [" + quote.qmpShippingMethodID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPQuoteDto>>> Process_GetAllQuotes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPQuoteDto> allQuotesDto = new List<ERPQuoteDto>();
		ERPResponseMessageDto<IList<ERPQuoteDto>> result;
		try
		{
			IERPQuoteRepository iERPQuoteRepository = (base.ERPQuoteRepository = new ERPQuoteRepository(base.ApiClientContext));
			using (iERPQuoteRepository)
			{
				foreach (ERPQuoteInformationDto item2 in await base.ERPQuoteRepository.GetAllQuotes(pageSize, pageNumber, filter, orderBy))
				{
					ERPQuoteDto item = new ERPQuoteDto
					{
						qmpArInvoiceContactID = item2.qmpArInvoiceContactID,
						qmpArInvoiceLocationID = item2.qmpArInvoiceLocationID,
						qmpClosedDate = item2.qmpClosedDate,
						qmpQuoteID = item2.qmpQuoteID,
						qmpCreatedBy = item2.qmpCreatedBy,
						qmpCreatedDate = item2.qmpCreatedDate,
						qmpCurrencyRateID = item2.qmpCurrencyRateID,
						qmpCustomerOrganizationID = item2.qmpCustomerOrganizationID,
						qmpDueDate = item2.qmpDueDate,
						qmpUniqueID = item2.qmpUniqueID,
						qmpExchangeRate = item2.qmpExchangeRate,
						qmpExpirationDate = item2.qmpExpirationDate,
						qmpFreeOnBoardDescription = item2.qmpFreeOnBoardDescription,
						qmpAvalaraTaxCalculated = item2.qmpAvalaraTaxCalculated,
						qmpClosed = item2.qmpClosed,
						qmpCreatedFromMobile = item2.qmpCreatedFromMobile,
						qmpCustomRate = item2.qmpCustomRate,
						qmpPaymentTermID = item2.qmpPaymentTermID,
						qmpPlantDepartmentID = item2.qmpPlantDepartmentID,
						qmpPlantID = item2.qmpPlantID,
						qmpProjectID = item2.qmpProjectID,
						qmpQuoteContactID = item2.qmpQuoteContactID,
						qmpQuoteDate = item2.qmpQuoteDate,
						qmpQuoteFooterMessageRTF = item2.qmpQuoteFooterMessageRTF,
						qmpQuoteFooterMessageText = item2.qmpQuoteFooterMessageText,
						qmpQuoteHeaderMessageRTF = item2.qmpQuoteHeaderMessageRTF,
						qmpQuoteHeaderMessageText = item2.qmpQuoteHeaderMessageText,
						qmpQuoteLocationID = item2.qmpQuoteLocationID,
						qmpQuoterEmployeeID = item2.qmpQuoterEmployeeID,
						qmpRowVersion = item2.qmpRowVersion,
						qmpShipContactID = item2.qmpShipContactID,
						qmpShipLocationID = item2.qmpShipLocationID,
						qmpShipOrganizationID = item2.qmpShipOrganizationID,
						qmpShippingMethodID = item2.qmpShippingMethodID,
						qmpShippingPaymentTypeID = item2.qmpShippingPaymentTypeID,
						qmpSplitPercentTotal = item2.qmpSplitPercentTotal,
						qmpStandardMessageID = item2.qmpStandardMessageID,
						qmpTaxDate = item2.qmpTaxDate,
						CustomFields = item2.CustomFields
					};
					allQuotesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Quotes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPQuoteDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allQuotesDto,
				RecordCount = allQuotesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuoteDto>> Process_GetQuote(Guid quoteId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPQuoteDto quoteDto = null;
		ERPResponseMessageDto<ERPQuoteDto> result;
		try
		{
			IERPQuoteRepository iERPQuoteRepository = (base.ERPQuoteRepository = new ERPQuoteRepository(base.ApiClientContext));
			using (iERPQuoteRepository)
			{
				ERPQuoteInformationDto eRPQuoteInformationDto = await base.ERPQuoteRepository.GetQuote(quoteId);
				quoteDto = new ERPQuoteDto
				{
					qmpArInvoiceContactID = eRPQuoteInformationDto.qmpArInvoiceContactID,
					qmpArInvoiceLocationID = eRPQuoteInformationDto.qmpArInvoiceLocationID,
					qmpClosedDate = eRPQuoteInformationDto.qmpClosedDate,
					qmpQuoteID = eRPQuoteInformationDto.qmpQuoteID,
					qmpCreatedBy = eRPQuoteInformationDto.qmpCreatedBy,
					qmpCreatedDate = eRPQuoteInformationDto.qmpCreatedDate,
					qmpCurrencyRateID = eRPQuoteInformationDto.qmpCurrencyRateID,
					qmpCustomerOrganizationID = eRPQuoteInformationDto.qmpCustomerOrganizationID,
					qmpDueDate = eRPQuoteInformationDto.qmpDueDate,
					qmpUniqueID = eRPQuoteInformationDto.qmpUniqueID,
					qmpExchangeRate = eRPQuoteInformationDto.qmpExchangeRate,
					qmpExpirationDate = eRPQuoteInformationDto.qmpExpirationDate,
					qmpFreeOnBoardDescription = eRPQuoteInformationDto.qmpFreeOnBoardDescription,
					qmpAvalaraTaxCalculated = eRPQuoteInformationDto.qmpAvalaraTaxCalculated,
					qmpClosed = eRPQuoteInformationDto.qmpClosed,
					qmpCreatedFromMobile = eRPQuoteInformationDto.qmpCreatedFromMobile,
					qmpCustomRate = eRPQuoteInformationDto.qmpCustomRate,
					qmpPaymentTermID = eRPQuoteInformationDto.qmpPaymentTermID,
					qmpPlantDepartmentID = eRPQuoteInformationDto.qmpPlantDepartmentID,
					qmpPlantID = eRPQuoteInformationDto.qmpPlantID,
					qmpProjectID = eRPQuoteInformationDto.qmpProjectID,
					qmpQuoteContactID = eRPQuoteInformationDto.qmpQuoteContactID,
					qmpQuoteDate = eRPQuoteInformationDto.qmpQuoteDate,
					qmpQuoteFooterMessageRTF = eRPQuoteInformationDto.qmpQuoteFooterMessageRTF,
					qmpQuoteFooterMessageText = eRPQuoteInformationDto.qmpQuoteFooterMessageText,
					qmpQuoteHeaderMessageRTF = eRPQuoteInformationDto.qmpQuoteHeaderMessageRTF,
					qmpQuoteHeaderMessageText = eRPQuoteInformationDto.qmpQuoteHeaderMessageText,
					qmpQuoteLocationID = eRPQuoteInformationDto.qmpQuoteLocationID,
					qmpQuoterEmployeeID = eRPQuoteInformationDto.qmpQuoterEmployeeID,
					qmpRowVersion = eRPQuoteInformationDto.qmpRowVersion,
					qmpShipContactID = eRPQuoteInformationDto.qmpShipContactID,
					qmpShipLocationID = eRPQuoteInformationDto.qmpShipLocationID,
					qmpShipOrganizationID = eRPQuoteInformationDto.qmpShipOrganizationID,
					qmpShippingMethodID = eRPQuoteInformationDto.qmpShippingMethodID,
					qmpShippingPaymentTypeID = eRPQuoteInformationDto.qmpShippingPaymentTypeID,
					qmpSplitPercentTotal = eRPQuoteInformationDto.qmpSplitPercentTotal,
					qmpStandardMessageID = eRPQuoteInformationDto.qmpStandardMessageID,
					qmpTaxDate = eRPQuoteInformationDto.qmpTaxDate,
					CustomFields = eRPQuoteInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Quotes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuoteDto>> Process_PutQuote(ERPQuoteDto quote)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPQuoteDto createdObject = null;
		ERPResponseMessageDto<ERPQuoteDto> result;
		try
		{
			IERPQuoteRepository iERPQuoteRepository = (base.ERPQuoteRepository = new ERPQuoteRepository(base.ApiClientContext));
			using (iERPQuoteRepository)
			{
				APIValidationInfoDto postResult = await base.ERPQuoteRepository.SaveQuote(quote);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPQuoteInformationDto eRPQuoteInformationDto = await base.ERPQuoteRepository.GetQuote(quote.qmpUniqueID);
					createdObject = new ERPQuoteDto
					{
						qmpArInvoiceContactID = eRPQuoteInformationDto.qmpArInvoiceContactID,
						qmpArInvoiceLocationID = eRPQuoteInformationDto.qmpArInvoiceLocationID,
						qmpClosedDate = eRPQuoteInformationDto.qmpClosedDate,
						qmpQuoteID = eRPQuoteInformationDto.qmpQuoteID,
						qmpCreatedBy = eRPQuoteInformationDto.qmpCreatedBy,
						qmpCreatedDate = eRPQuoteInformationDto.qmpCreatedDate,
						qmpCurrencyRateID = eRPQuoteInformationDto.qmpCurrencyRateID,
						qmpCustomerOrganizationID = eRPQuoteInformationDto.qmpCustomerOrganizationID,
						qmpDueDate = eRPQuoteInformationDto.qmpDueDate,
						qmpUniqueID = eRPQuoteInformationDto.qmpUniqueID,
						qmpExchangeRate = eRPQuoteInformationDto.qmpExchangeRate,
						qmpExpirationDate = eRPQuoteInformationDto.qmpExpirationDate,
						qmpFreeOnBoardDescription = eRPQuoteInformationDto.qmpFreeOnBoardDescription,
						qmpAvalaraTaxCalculated = eRPQuoteInformationDto.qmpAvalaraTaxCalculated,
						qmpClosed = eRPQuoteInformationDto.qmpClosed,
						qmpCreatedFromMobile = eRPQuoteInformationDto.qmpCreatedFromMobile,
						qmpCustomRate = eRPQuoteInformationDto.qmpCustomRate,
						qmpPaymentTermID = eRPQuoteInformationDto.qmpPaymentTermID,
						qmpPlantDepartmentID = eRPQuoteInformationDto.qmpPlantDepartmentID,
						qmpPlantID = eRPQuoteInformationDto.qmpPlantID,
						qmpProjectID = eRPQuoteInformationDto.qmpProjectID,
						qmpQuoteContactID = eRPQuoteInformationDto.qmpQuoteContactID,
						qmpQuoteDate = eRPQuoteInformationDto.qmpQuoteDate,
						qmpQuoteFooterMessageRTF = eRPQuoteInformationDto.qmpQuoteFooterMessageRTF,
						qmpQuoteFooterMessageText = eRPQuoteInformationDto.qmpQuoteFooterMessageText,
						qmpQuoteHeaderMessageRTF = eRPQuoteInformationDto.qmpQuoteHeaderMessageRTF,
						qmpQuoteHeaderMessageText = eRPQuoteInformationDto.qmpQuoteHeaderMessageText,
						qmpQuoteLocationID = eRPQuoteInformationDto.qmpQuoteLocationID,
						qmpQuoterEmployeeID = eRPQuoteInformationDto.qmpQuoterEmployeeID,
						qmpRowVersion = eRPQuoteInformationDto.qmpRowVersion,
						qmpShipContactID = eRPQuoteInformationDto.qmpShipContactID,
						qmpShipLocationID = eRPQuoteInformationDto.qmpShipLocationID,
						qmpShipOrganizationID = eRPQuoteInformationDto.qmpShipOrganizationID,
						qmpShippingMethodID = eRPQuoteInformationDto.qmpShippingMethodID,
						qmpShippingPaymentTypeID = eRPQuoteInformationDto.qmpShippingPaymentTypeID,
						qmpSplitPercentTotal = eRPQuoteInformationDto.qmpSplitPercentTotal,
						qmpStandardMessageID = eRPQuoteInformationDto.qmpStandardMessageID,
						qmpTaxDate = eRPQuoteInformationDto.qmpTaxDate,
						CustomFields = eRPQuoteInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Quote [{quote.qmpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteQuote(Guid quoteId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteRepository iERPQuoteRepository = (base.ERPQuoteRepository = new ERPQuoteRepository(base.ApiClientContext));
		using (iERPQuoteRepository)
		{
			if (!(await base.ERPQuoteRepository.DoesQuoteExist(quoteId)))
			{
				base.ErrorsList.Add($"Quote [{quoteId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPQuoteInformationDto eRPQuoteInformationDto = await base.ERPQuoteRepository.GetQuote(quoteId);
				string text = await base.ERPQuoteRepository.WhereUsed("Quotes", new object[1] { eRPQuoteInformationDto.qmpQuoteID }, new object[1] { "qmpQuoteID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Quote cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPQuoteDto>> Process_DeleteQuote(Guid quoteId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPQuoteDto> result;
		try
		{
			IERPQuoteRepository iERPQuoteRepository = (base.ERPQuoteRepository = new ERPQuoteRepository(base.ApiClientContext));
			using (iERPQuoteRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPQuoteRepository.DeleteRowFromTable("Quotes", "qmp", quoteId);
				((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
				IList<string> errorsList = base.ErrorsList;
				if (errorsList != null && errorsList.Count > 0)
				{
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Quote [{quoteId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPQuoteDto()
			};
		}
		return result;
	}
}
