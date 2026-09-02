using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.Repositories.Core;

namespace M1.API.Models.BOM;

public class BOMOrganizationLocationModel : BOMBaseModel, IBOMOrganizationLocationModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetOrganizationLocation(string organizationId, string organizationLocationId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		using (OrganizationLocationRepository organizationLocationRepository = new OrganizationLocationRepository(base.ApiClientContext))
		{
			if (!(await organizationLocationRepository.DoesOrganizationLocationExists(organizationId, organizationLocationId)))
			{
				errorsList.Add("Organization [" + organizationId + "], containing OrganizationLocation [" + organizationLocationId + "] is invalid");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PostOrganizationLocation(BOMOrganizationLocationDto organizationLocation)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		APIValidationInfoDto result;
		try
		{
			using (OrganizationRepository organizationRepository = new OrganizationRepository(base.ApiClientContext))
			{
				if (!string.IsNullOrWhiteSpace(organizationLocation.OrganizationID) && !(await organizationRepository.DoesOrganizationExists(organizationLocation.OrganizationID)))
				{
					base.ErrorsList.Add("Organization [" + organizationLocation.OrganizationID + "] is not valid.");
				}
			}
			using (TaxCodeRepository taxCodeRepository = new TaxCodeRepository(base.ApiClientContext))
			{
				if (!string.IsNullOrWhiteSpace(organizationLocation.CustomerTaxCodeID) && !(await taxCodeRepository.DoesTaxCodeExistAsync(organizationLocation.CustomerTaxCodeID)))
				{
					base.ErrorsList.Add("Customer tax code [" + organizationLocation.CustomerTaxCodeID + "] is invalid.");
				}
				if (!string.IsNullOrWhiteSpace(organizationLocation.CustomerSecondTaxCodeID) && !(await taxCodeRepository.DoesTaxCodeExistAsync(organizationLocation.CustomerSecondTaxCodeID)))
				{
					base.ErrorsList.Add("Customer second tax code [" + organizationLocation.CustomerSecondTaxCodeID + "] is invalid.");
				}
			}
			using (ShippingMethodRepository shippingMethodRepository = new ShippingMethodRepository(base.ApiClientContext))
			{
				if (!string.IsNullOrWhiteSpace(organizationLocation.CustomerShippingMethodID) && !(await shippingMethodRepository.DoesShippingMethodExistsAsync(organizationLocation.CustomerShippingMethodID)))
				{
					base.ErrorsList.Add("Customer Shipping Method code [" + organizationLocation.CustomerShippingMethodID + "] is invalid.");
				}
			}
			using ShippingPaymentTypeRepository shipPaymentTypeRepository = new ShippingPaymentTypeRepository(base.ApiClientContext);
			if (!string.IsNullOrWhiteSpace(organizationLocation.CustomerShipPaymentTypeID) && !(await shipPaymentTypeRepository.DoesShippingPaymentTypeExistsAsync(organizationLocation.CustomerShipPaymentTypeID)))
			{
				base.ErrorsList.Add("Customer Ship Payment Type code [" + organizationLocation.CustomerShipPaymentTypeID + "] is invalid.");
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the organization location [" + organizationLocation.LocationID + "]");
		}
		finally
		{
			result = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return await Task.FromResult(result);
	}

	public async Task<BOMResponseMessageDto<IList<CTMOrganizationLocationDto>>> Process_GetAllOrganizationLocations(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<CTMOrganizationLocationDto> allOrganizationLocationsDto = new List<CTMOrganizationLocationDto>();
		BOMResponseMessageDto<IList<CTMOrganizationLocationDto>> result;
		try
		{
			using OrganizationLocationRepository organizationLocationRepository = new OrganizationLocationRepository(base.ApiClientContext);
			foreach (OrganizationLocationInformationDto item2 in await organizationLocationRepository.GetAllOrganizationLocations(pageSize, pageNumber))
			{
				CTMOrganizationLocationDto item = new CTMOrganizationLocationDto
				{
					OrganizationID = item2.OrganizationID,
					LocationID = item2.LocationID,
					Name = item2.Name,
					AddressLine1 = item2.AddressLine1,
					AddressLine2 = item2.AddressLine2,
					AddressLine3 = item2.AddressLine3,
					City = item2.City,
					County = item2.County,
					State = item2.State,
					PostCode = item2.PostCode,
					Country = item2.Country,
					PhoneNumber = item2.PhoneNumber,
					EmailAddress = item2.EmailAddress,
					QuoteLocation = item2.QuoteLocation,
					QuoteContactID = item2.QuoteContactID,
					ShipLocation = item2.ShipLocation,
					ShipContactID = item2.ShipContactID,
					ArInvoiceLocation = item2.ArInvoiceLocation,
					ArInvoiceContactID = item2.ArInvoiceContactID,
					PurchaseLocation = item2.PurchaseLocation,
					PurchaseContactID = item2.PurchaseContactID,
					ApInvoiceLocation = item2.ApInvoiceLocation,
					ApInvoiceContactID = item2.ApInvoiceContactID,
					CustomerTaxable = item2.CustomerTaxable,
					CustomerTaxCodeID = item2.CustomerTaxCodeID,
					CustomerSecondTaxCodeID = item2.CustomerSecondTaxCodeID,
					CustomerShippingMethodID = item2.CustomerShippingMethodID,
					CustomerShipPaymentTypeID = item2.CustomerShipPaymentTypeID,
					TaxExemptNumber = item2.TaxExemptNumber,
					NonTaxReasonID = item2.NonTaxReasonID,
					CustomerPaymentTermID = item2.CustomerPaymentTermID,
					CurrencyRateID = item2.CurrencyRateID,
					SupplierPaymentTermID = item2.SupplierPaymentTermID,
					SupplierShippingMethodID = item2.SupplierShippingMethodID,
					Inactive = item2.Inactive,
					InactiveDate = item2.InactiveDate,
					CustomerShippingCarrier = item2.CustomerShippingCarrier,
					UpsAcctNumber = item2.UpsAcctNumber,
					UpsWsBillingOption = item2.UpsWsBillingOption,
					Ups3rdPartyLocationID = item2.Ups3rdPartyOrganizationID,
					ResidentialAddress = item2.ResidentialAddress,
					FedExAccountNumber = item2.FedExAccountNumber,
					FedEx3rdPartyOrganizationID = item2.FedEx3rdPartyOrganizationID,
					FedExBillingOption = item2.FedExBillingOption,
					CreditCheckForLocation = item2.CreditCheckForLocation,
					CustomerCreditLimit = item2.CustomerCreditLimit,
					CreditHold = item2.CreditHold,
					CountryCode = item2.CountryCode,
					CreatedDate = item2.CreatedDate,
					CreatedBy = item2.CreatedBy,
					AvalaraUseCodes = item2.AvalaraUseCodes,
					UniqueID = item2.UniqueID,
					RowVersion = item2.RowVersion
				};
				allOrganizationLocationsDto.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all OrganizationLocations]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<CTMOrganizationLocationDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allOrganizationLocationsDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<CTMOrganizationLocationDto>> Process_GetOrganizationLocation(string organizationId, string organizationLocationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		CTMOrganizationLocationDto organizationLocationDto = null;
		BOMResponseMessageDto<CTMOrganizationLocationDto> result;
		try
		{
			using OrganizationLocationRepository organizationLocationRepository = new OrganizationLocationRepository(base.ApiClientContext);
			OrganizationLocationInformationDto organizationLocationInformationDto = await organizationLocationRepository.GetOrganizationLocation(organizationId, organizationLocationId);
			organizationLocationDto = new CTMOrganizationLocationDto
			{
				OrganizationID = organizationLocationInformationDto.OrganizationID,
				LocationID = organizationLocationInformationDto.LocationID,
				Name = organizationLocationInformationDto.Name,
				AddressLine1 = organizationLocationInformationDto.AddressLine1,
				AddressLine2 = organizationLocationInformationDto.AddressLine2,
				AddressLine3 = organizationLocationInformationDto.AddressLine3,
				City = organizationLocationInformationDto.City,
				County = organizationLocationInformationDto.County,
				State = organizationLocationInformationDto.State,
				PostCode = organizationLocationInformationDto.PostCode,
				Country = organizationLocationInformationDto.Country,
				PhoneNumber = organizationLocationInformationDto.PhoneNumber,
				EmailAddress = organizationLocationInformationDto.EmailAddress,
				QuoteLocation = organizationLocationInformationDto.QuoteLocation,
				QuoteContactID = organizationLocationInformationDto.QuoteContactID,
				ShipLocation = organizationLocationInformationDto.ShipLocation,
				ShipContactID = organizationLocationInformationDto.ShipContactID,
				ArInvoiceLocation = organizationLocationInformationDto.ArInvoiceLocation,
				ArInvoiceContactID = organizationLocationInformationDto.ArInvoiceContactID,
				PurchaseLocation = organizationLocationInformationDto.PurchaseLocation,
				PurchaseContactID = organizationLocationInformationDto.PurchaseContactID,
				ApInvoiceLocation = organizationLocationInformationDto.ApInvoiceLocation,
				ApInvoiceContactID = organizationLocationInformationDto.ApInvoiceContactID,
				CustomerTaxable = organizationLocationInformationDto.CustomerTaxable,
				CustomerTaxCodeID = organizationLocationInformationDto.CustomerTaxCodeID,
				CustomerSecondTaxCodeID = organizationLocationInformationDto.CustomerSecondTaxCodeID,
				CustomerShippingMethodID = organizationLocationInformationDto.CustomerShippingMethodID,
				CustomerShipPaymentTypeID = organizationLocationInformationDto.CustomerShipPaymentTypeID,
				TaxExemptNumber = organizationLocationInformationDto.TaxExemptNumber,
				NonTaxReasonID = organizationLocationInformationDto.NonTaxReasonID,
				CustomerPaymentTermID = organizationLocationInformationDto.CustomerPaymentTermID,
				CurrencyRateID = organizationLocationInformationDto.CurrencyRateID,
				SupplierPaymentTermID = organizationLocationInformationDto.SupplierPaymentTermID,
				SupplierShippingMethodID = organizationLocationInformationDto.SupplierShippingMethodID,
				Inactive = organizationLocationInformationDto.Inactive,
				InactiveDate = organizationLocationInformationDto.InactiveDate,
				CustomerShippingCarrier = organizationLocationInformationDto.CustomerShippingCarrier,
				UpsAcctNumber = organizationLocationInformationDto.UpsAcctNumber,
				UpsWsBillingOption = organizationLocationInformationDto.UpsWsBillingOption,
				Ups3rdPartyLocationID = organizationLocationInformationDto.Ups3rdPartyOrganizationID,
				ResidentialAddress = organizationLocationInformationDto.ResidentialAddress,
				FedExAccountNumber = organizationLocationInformationDto.FedExAccountNumber,
				FedEx3rdPartyOrganizationID = organizationLocationInformationDto.FedEx3rdPartyOrganizationID,
				FedExBillingOption = organizationLocationInformationDto.FedExBillingOption,
				CreditCheckForLocation = organizationLocationInformationDto.CreditCheckForLocation,
				CustomerCreditLimit = organizationLocationInformationDto.CustomerCreditLimit,
				CreditHold = organizationLocationInformationDto.CreditHold,
				CountryCode = organizationLocationInformationDto.CountryCode,
				CreatedDate = organizationLocationInformationDto.CreatedDate,
				CreatedBy = organizationLocationInformationDto.CreatedBy,
				AvalaraUseCodes = organizationLocationInformationDto.AvalaraUseCodes,
				UniqueID = organizationLocationInformationDto.UniqueID,
				RowVersion = organizationLocationInformationDto.RowVersion
			};
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the OrganizationLocations []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<CTMOrganizationLocationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = organizationLocationDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMOrganizationLocationDto>> Process_PostOrganizationLocation(BOMOrganizationLocationDto organizationLocation)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		BOMResponseMessageDto<BOMOrganizationLocationDto> result;
		try
		{
			using OrganizationLocationRepository organizationLocationRepository = new OrganizationLocationRepository(base.ApiClientContext);
			APIValidationInfoDto aPIValidationInfoDto = await organizationLocationRepository.SaveOrganizationLocation(organizationLocation);
			((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
			((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing OrganizationLocation [" + organizationLocation.LocationID + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMOrganizationLocationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = organizationLocation
			};
		}
		return result;
	}
}
