using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom.Sales;
using M1.API.Repositories.Core;
using M1.API.Repositories.Core.Sales;

namespace M1.API.Models.BOM.Sales;

public class BOMQuoteModel : BOMBaseModel, IBOMQuoteModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public IDictionary<string, object> QuoteKeyDictionary { get; set; }

	public BOMQuoteModel()
	{
		QuoteKeyDictionary = new Dictionary<string, object>();
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetQuoteAsync(string quoteId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		APIValidationInfoDto result;
		try
		{
			using (QuoteRepository quoteRepository = new QuoteRepository(base.ApiClientContext))
			{
				string text = await GetM1QuoteIdFromGuid(quoteId);
				if (string.IsNullOrWhiteSpace(text))
				{
					errorsList.Add("Quote [" + quoteId + "] is invalid");
				}
				else
				{
					QuoteKeyDictionary.Add("qmpQuoteID", text);
					if (!(await quoteRepository.DoesQuoteExistsAsync(text)))
					{
						errorsList.Add("Quote [" + quoteId + "] is invalid");
					}
				}
			}
			if (errorsList != null && errorsList.Count > 0)
			{
				httpStatus = HttpStatusCode.BadRequest;
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			errorsList.Add("Error ocurred [" + ex.Message + "] while validating the quote [" + quoteId + "]");
		}
		finally
		{
			result = new APIValidationInfoDto(errorsList, warningsList, httpStatus);
			errorsList.Clear();
			warningsList.Clear();
		}
		return await Task.FromResult(result);
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PostQuoteAsync(BOMCreateQuoteDto quote)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		APIValidationInfoDto result;
		try
		{
			using (OrganizationRepository organizationRepository = new OrganizationRepository(base.ApiClientContext))
			{
				if (!string.IsNullOrWhiteSpace(quote.CustomerOrganizationID) && !(await organizationRepository.DoesOrganizationExists(quote.CustomerOrganizationID)))
				{
					base.ErrorsList.Add("Customer Organization [" + quote.CustomerOrganizationID + "] is not valid.");
				}
				if (!string.IsNullOrWhiteSpace(quote.ShipOrganizationID) && !(await organizationRepository.DoesOrganizationExists(quote.CustomerOrganizationID)))
				{
					base.ErrorsList.Add("Ship Organization [" + quote.ShipOrganizationID + "] is not valid.");
				}
			}
			using EmployeeRepository employeeRepository = new EmployeeRepository(base.ApiClientContext);
			if (!string.IsNullOrWhiteSpace(quote.QuoterEmployeeID) && !(await employeeRepository.DoesEmployeeExistsAsync(quote.QuoterEmployeeID)))
			{
				base.ErrorsList.Add("Quoter Employee with ID [" + quote.QuoterEmployeeID + "] is either invalid or not registered as a quoter employee.");
			}
			bool flag = !string.IsNullOrEmpty(quote.CurrencyRateID);
			if (flag)
			{
				flag = await employeeRepository.IsMultiCurrencyEnabled();
			}
			if (flag && !(await employeeRepository.DoesCurrencyCodeExists(quote.CurrencyRateID)))
			{
				base.ErrorsList.Add("Currency Rate with ID [" + quote.CurrencyRateID + "] is invalid.");
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating Quote [" + quote.QuoteID + "]");
		}
		finally
		{
			result = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return await Task.FromResult(result);
	}

	public async Task<BOMResponseMessageDto<IList<BOMQuoteDto>>> Process_GetAllQuotesAsync(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMQuoteDto> allQuotesDto = new List<BOMQuoteDto>();
		BOMResponseMessageDto<IList<BOMQuoteDto>> result;
		try
		{
			using QuoteRepository quoteRepository = new QuoteRepository(base.ApiClientContext);
			foreach (BOMQuoteDto item2 in await quoteRepository.GetAllQuotesAsync(pageSize, pageNumber))
			{
				BOMQuoteDto item = new BOMQuoteDto
				{
					QuoteID = item2.QuoteID,
					CustomerOrganizationID = item2.CustomerOrganizationID,
					PlantID = item2.PlantID,
					QuoterEmployeeID = item2.QuoterEmployeeID,
					QuoteDate = item2.QuoteDate,
					DueDate = item2.DueDate,
					ExpirationDate = item2.ExpirationDate,
					ProjectID = item2.ProjectID,
					Closed = item2.Closed,
					ClosedDate = item2.ClosedDate,
					PaymentTermID = item2.PaymentTermID,
					ShippingMethodID = item2.ShippingMethodID,
					CreatedBy = item2.CreatedBy,
					CreatedDate = item2.CreatedDate,
					UniqueID = item2.UniqueID,
					RowVersion = item2.RowVersion
				};
				allQuotesDto.Add(item);
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
			result = new BOMResponseMessageDto<IList<BOMQuoteDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allQuotesDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<CTMBOMQuoteLineDto>> Process_GetQuoteLinesAsync(string quoteId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		CTMBOMQuoteLineDto quoteLineDto = new CTMBOMQuoteLineDto();
		BOMResponseMessageDto<CTMBOMQuoteLineDto> result;
		try
		{
			using (QuoteRepository quoteRepository = new QuoteRepository(base.ApiClientContext))
			{
				BOMQuoteDto quoteInfo = await quoteRepository.GetQuoteAsync(quoteId);
				IList<BOMQuoteLineDto> obj = await quoteRepository.GetQuoteLinesInfoAsync(quoteId);
				quoteLineDto.Quote = new BOMQuoteDto
				{
					QuoteID = quoteInfo.QuoteID,
					CustomerOrganizationID = quoteInfo.CustomerOrganizationID,
					PlantID = quoteInfo.PlantID,
					QuoterEmployeeID = quoteInfo.QuoterEmployeeID,
					QuoteDate = quoteInfo.QuoteDate,
					DueDate = quoteInfo.DueDate,
					ExpirationDate = quoteInfo.ExpirationDate,
					ProjectID = quoteInfo.ProjectID,
					Closed = quoteInfo.Closed,
					ClosedDate = quoteInfo.ClosedDate,
					PaymentTermID = quoteInfo.PaymentTermID,
					ShippingMethodID = quoteInfo.ShippingMethodID,
					CreatedBy = quoteInfo.CreatedBy,
					CreatedDate = quoteInfo.CreatedDate,
					UniqueID = quoteInfo.UniqueID,
					RowVersion = quoteInfo.RowVersion
				};
				foreach (BOMQuoteLineDto item in obj)
				{
					quoteLineDto.QuoteLines.Add(new BOMQuoteLineDto
					{
						QuoteID = item.QuoteID,
						QuoteLineID = item.QuoteLineID,
						PartID = item.PartID,
						PartRevisionID = item.PartRevisionID,
						UnitOfMeasure = item.UnitOfMeasure,
						PartGroupID = item.PartGroupID,
						PartShortDescription = item.PartShortDescription,
						OrgPartShortDescription = item.OrgPartShortDescription,
						ResolutionReasonID = item.ResolutionReasonID,
						QuoteMarkupType = item.QuoteMarkupType,
						PurchaseToOrder = item.PurchaseToOrder,
						PurchaseUnitCostForeign = item.PurchaseUnitCostForeign,
						SupplierOrganizationID = item.SupplierOrganizationID,
						PurchaseLocationID = item.PurchaseLocationID,
						Firm = item.Firm,
						ProjectID = item.ProjectID,
						ProjectAreaID = item.ProjectAreaID,
						Closed = item.Closed,
						CreatedBy = item.CreatedBy,
						CreatedDate = item.CreatedDate,
						UniqueID = item.UniqueID,
						RowVersion = item.RowVersion
					});
				}
			}
			IList<string> errorsList = base.ErrorsList;
			if (errorsList != null && errorsList.Count > 0)
			{
				httpStatus = HttpStatusCode.BadRequest;
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Quote [" + quoteId + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<CTMBOMQuoteLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteLineDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMQuoteDto>> Process_GetQuoteAsync(string quoteId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		BOMQuoteDto quoteDto = null;
		BOMResponseMessageDto<BOMQuoteDto> result;
		try
		{
			using QuoteRepository quoteRepository = new QuoteRepository(base.ApiClientContext);
			BOMQuoteDto bOMQuoteDto = await quoteRepository.GetQuoteAsync(quoteId);
			quoteDto = new BOMQuoteDto
			{
				QuoteID = bOMQuoteDto.QuoteID,
				CustomerOrganizationID = bOMQuoteDto.CustomerOrganizationID,
				PlantID = bOMQuoteDto.PlantID,
				QuoterEmployeeID = bOMQuoteDto.QuoterEmployeeID,
				QuoteDate = bOMQuoteDto.QuoteDate,
				DueDate = bOMQuoteDto.DueDate,
				ExpirationDate = bOMQuoteDto.ExpirationDate,
				ProjectID = bOMQuoteDto.ProjectID,
				Closed = bOMQuoteDto.Closed,
				ClosedDate = bOMQuoteDto.ClosedDate,
				PaymentTermID = bOMQuoteDto.PaymentTermID,
				ShippingMethodID = bOMQuoteDto.ShippingMethodID,
				CreatedBy = bOMQuoteDto.CreatedBy,
				CreatedDate = bOMQuoteDto.CreatedDate,
				UniqueID = bOMQuoteDto.UniqueID,
				RowVersion = bOMQuoteDto.RowVersion
			};
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Quotes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMQuoteDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMCreateQuoteDto>> Process_PostQuoteAsync(BOMCreateQuoteDto quote)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		BOMResponseMessageDto<BOMCreateQuoteDto> result;
		try
		{
			using QuoteRepository quoteRepository = new QuoteRepository(base.ApiClientContext);
			APIValidationInfoDto aPIValidationInfoDto = await quoteRepository.SaveQuoteAsync(quote);
			((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
			((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing Quote [" + quote.QuoteID + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMCreateQuoteDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quote
			};
		}
		return result;
	}

	private async Task<string> GetM1QuoteIdFromGuid(string quoteIdString)
	{
		if (Guid.TryParse(quoteIdString, out var result))
		{
			using (QuoteRepository quoteRepository = new QuoteRepository(base.ApiClientContext))
			{
				return await quoteRepository.GetQuoteIdFromGuidAsync(result);
			}
		}
		return quoteIdString;
	}
}
