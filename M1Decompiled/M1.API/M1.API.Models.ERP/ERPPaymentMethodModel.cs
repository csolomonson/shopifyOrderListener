using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPaymentMethodModel : ERPBaseModel, IERPPaymentMethodModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPaymentMethods(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPaymentMethodRepository iERPPaymentMethodRepository = (base.ERPPaymentMethodRepository = new ERPPaymentMethodRepository(base.ApiClientContext));
		using (iERPPaymentMethodRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPaymentMethodRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPaymentMethodRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPaymentMethodRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPaymentMethodRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPaymentMethod(Guid paymentMethodId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPaymentMethodRepository iERPPaymentMethodRepository = (base.ERPPaymentMethodRepository = new ERPPaymentMethodRepository(base.ApiClientContext));
		using (iERPPaymentMethodRepository)
		{
			if (!(await base.ERPPaymentMethodRepository.DoesPaymentMethodExist(paymentMethodId)))
			{
				errorsList.Add($"PaymentMethod [{paymentMethodId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPaymentMethod(ERPPaymentMethodDto paymentMethod)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPaymentMethodRepository iERPPaymentMethodRepository = (base.ERPPaymentMethodRepository = new ERPPaymentMethodRepository(base.ApiClientContext));
		using (iERPPaymentMethodRepository)
		{
			if (!string.IsNullOrWhiteSpace(paymentMethod.xahBankAccountID) && !(await base.ERPPaymentMethodRepository.DoesRecordExistInTableUsingKeys("BankAccounts", new object[1] { "GLNBANKACCOUNTID" }, new object[1] { paymentMethod.xahBankAccountID })))
			{
				errorsList.Add("xahBankAccountID [" + paymentMethod.xahBankAccountID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPaymentMethodDto>>> Process_GetAllPaymentMethods(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPaymentMethodDto> allPaymentMethodsDto = new List<ERPPaymentMethodDto>();
		ERPResponseMessageDto<IList<ERPPaymentMethodDto>> result;
		try
		{
			IERPPaymentMethodRepository iERPPaymentMethodRepository = (base.ERPPaymentMethodRepository = new ERPPaymentMethodRepository(base.ApiClientContext));
			using (iERPPaymentMethodRepository)
			{
				foreach (ERPPaymentMethodInformationDto item2 in await base.ERPPaymentMethodRepository.GetAllPaymentMethods(pageSize, pageNumber, filter, orderBy))
				{
					ERPPaymentMethodDto item = new ERPPaymentMethodDto
					{
						xahArPaymentSessionRule = item2.xahArPaymentSessionRule,
						xahBankAccountID = item2.xahBankAccountID,
						xahPaymentMethodID = item2.xahPaymentMethodID,
						xahCreatedBy = item2.xahCreatedBy,
						xahCreatedDate = item2.xahCreatedDate,
						xahDescription = item2.xahDescription,
						xahUniqueID = item2.xahUniqueID,
						xahInactiveDate = item2.xahInactiveDate,
						xahInactive = item2.xahInactive,
						xahDoNotOpenCashDrawer = item2.xahDoNotOpenCashDrawer,
						xahPmAmex = item2.xahPmAmex,
						xahPmCash = item2.xahPmCash,
						xahPmCheck = item2.xahPmCheck,
						xahPmDiners = item2.xahPmDiners,
						xahPmDiscover = item2.xahPmDiscover,
						xahPmEnroute = item2.xahPmEnroute,
						xahPmJAL = item2.xahPmJAL,
						xahPmJCB = item2.xahPmJCB,
						xahPmMasterCard = item2.xahPmMasterCard,
						xahPmPurchaseOrder = item2.xahPmPurchaseOrder,
						xahPmStoreCredit = item2.xahPmStoreCredit,
						xahPmVisa = item2.xahPmVisa,
						xahRefundPriority = item2.xahRefundPriority,
						xahRowVersion = item2.xahRowVersion,
						xahSettlementTime = item2.xahSettlementTime,
						CustomFields = item2.CustomFields
					};
					allPaymentMethodsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PaymentMethods]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPaymentMethodDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPaymentMethodsDto,
				RecordCount = allPaymentMethodsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPaymentMethodDto>> Process_GetPaymentMethod(Guid paymentMethodId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPaymentMethodDto paymentMethodDto = null;
		ERPResponseMessageDto<ERPPaymentMethodDto> result;
		try
		{
			IERPPaymentMethodRepository iERPPaymentMethodRepository = (base.ERPPaymentMethodRepository = new ERPPaymentMethodRepository(base.ApiClientContext));
			using (iERPPaymentMethodRepository)
			{
				ERPPaymentMethodInformationDto eRPPaymentMethodInformationDto = await base.ERPPaymentMethodRepository.GetPaymentMethod(paymentMethodId);
				paymentMethodDto = new ERPPaymentMethodDto
				{
					xahArPaymentSessionRule = eRPPaymentMethodInformationDto.xahArPaymentSessionRule,
					xahBankAccountID = eRPPaymentMethodInformationDto.xahBankAccountID,
					xahPaymentMethodID = eRPPaymentMethodInformationDto.xahPaymentMethodID,
					xahCreatedBy = eRPPaymentMethodInformationDto.xahCreatedBy,
					xahCreatedDate = eRPPaymentMethodInformationDto.xahCreatedDate,
					xahDescription = eRPPaymentMethodInformationDto.xahDescription,
					xahUniqueID = eRPPaymentMethodInformationDto.xahUniqueID,
					xahInactiveDate = eRPPaymentMethodInformationDto.xahInactiveDate,
					xahInactive = eRPPaymentMethodInformationDto.xahInactive,
					xahDoNotOpenCashDrawer = eRPPaymentMethodInformationDto.xahDoNotOpenCashDrawer,
					xahPmAmex = eRPPaymentMethodInformationDto.xahPmAmex,
					xahPmCash = eRPPaymentMethodInformationDto.xahPmCash,
					xahPmCheck = eRPPaymentMethodInformationDto.xahPmCheck,
					xahPmDiners = eRPPaymentMethodInformationDto.xahPmDiners,
					xahPmDiscover = eRPPaymentMethodInformationDto.xahPmDiscover,
					xahPmEnroute = eRPPaymentMethodInformationDto.xahPmEnroute,
					xahPmJAL = eRPPaymentMethodInformationDto.xahPmJAL,
					xahPmJCB = eRPPaymentMethodInformationDto.xahPmJCB,
					xahPmMasterCard = eRPPaymentMethodInformationDto.xahPmMasterCard,
					xahPmPurchaseOrder = eRPPaymentMethodInformationDto.xahPmPurchaseOrder,
					xahPmStoreCredit = eRPPaymentMethodInformationDto.xahPmStoreCredit,
					xahPmVisa = eRPPaymentMethodInformationDto.xahPmVisa,
					xahRefundPriority = eRPPaymentMethodInformationDto.xahRefundPriority,
					xahRowVersion = eRPPaymentMethodInformationDto.xahRowVersion,
					xahSettlementTime = eRPPaymentMethodInformationDto.xahSettlementTime,
					CustomFields = eRPPaymentMethodInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PaymentMethods []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPaymentMethodDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = paymentMethodDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPaymentMethodDto>> Process_PutPaymentMethod(ERPPaymentMethodDto paymentMethod)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPaymentMethodDto createdObject = null;
		ERPResponseMessageDto<ERPPaymentMethodDto> result;
		try
		{
			IERPPaymentMethodRepository iERPPaymentMethodRepository = (base.ERPPaymentMethodRepository = new ERPPaymentMethodRepository(base.ApiClientContext));
			using (iERPPaymentMethodRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPaymentMethodRepository.SavePaymentMethod(paymentMethod);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPaymentMethodInformationDto eRPPaymentMethodInformationDto = await base.ERPPaymentMethodRepository.GetPaymentMethod(paymentMethod.xahUniqueID);
					createdObject = new ERPPaymentMethodDto
					{
						xahArPaymentSessionRule = eRPPaymentMethodInformationDto.xahArPaymentSessionRule,
						xahBankAccountID = eRPPaymentMethodInformationDto.xahBankAccountID,
						xahPaymentMethodID = eRPPaymentMethodInformationDto.xahPaymentMethodID,
						xahCreatedBy = eRPPaymentMethodInformationDto.xahCreatedBy,
						xahCreatedDate = eRPPaymentMethodInformationDto.xahCreatedDate,
						xahDescription = eRPPaymentMethodInformationDto.xahDescription,
						xahUniqueID = eRPPaymentMethodInformationDto.xahUniqueID,
						xahInactiveDate = eRPPaymentMethodInformationDto.xahInactiveDate,
						xahInactive = eRPPaymentMethodInformationDto.xahInactive,
						xahDoNotOpenCashDrawer = eRPPaymentMethodInformationDto.xahDoNotOpenCashDrawer,
						xahPmAmex = eRPPaymentMethodInformationDto.xahPmAmex,
						xahPmCash = eRPPaymentMethodInformationDto.xahPmCash,
						xahPmCheck = eRPPaymentMethodInformationDto.xahPmCheck,
						xahPmDiners = eRPPaymentMethodInformationDto.xahPmDiners,
						xahPmDiscover = eRPPaymentMethodInformationDto.xahPmDiscover,
						xahPmEnroute = eRPPaymentMethodInformationDto.xahPmEnroute,
						xahPmJAL = eRPPaymentMethodInformationDto.xahPmJAL,
						xahPmJCB = eRPPaymentMethodInformationDto.xahPmJCB,
						xahPmMasterCard = eRPPaymentMethodInformationDto.xahPmMasterCard,
						xahPmPurchaseOrder = eRPPaymentMethodInformationDto.xahPmPurchaseOrder,
						xahPmStoreCredit = eRPPaymentMethodInformationDto.xahPmStoreCredit,
						xahPmVisa = eRPPaymentMethodInformationDto.xahPmVisa,
						xahRefundPriority = eRPPaymentMethodInformationDto.xahRefundPriority,
						xahRowVersion = eRPPaymentMethodInformationDto.xahRowVersion,
						xahSettlementTime = eRPPaymentMethodInformationDto.xahSettlementTime,
						CustomFields = eRPPaymentMethodInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PaymentMethod [{paymentMethod.xahUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPaymentMethodDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePaymentMethod(Guid paymentMethodId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPaymentMethodRepository iERPPaymentMethodRepository = (base.ERPPaymentMethodRepository = new ERPPaymentMethodRepository(base.ApiClientContext));
		using (iERPPaymentMethodRepository)
		{
			if (!(await base.ERPPaymentMethodRepository.DoesPaymentMethodExist(paymentMethodId)))
			{
				base.ErrorsList.Add($"PaymentMethod [{paymentMethodId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPaymentMethodInformationDto eRPPaymentMethodInformationDto = await base.ERPPaymentMethodRepository.GetPaymentMethod(paymentMethodId);
				string text = await base.ERPPaymentMethodRepository.WhereUsed("PaymentMethods", new object[1] { eRPPaymentMethodInformationDto.xahPaymentMethodID }, new object[1] { "xahPaymentMethodID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PaymentMethod cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPaymentMethodDto>> Process_DeletePaymentMethod(Guid paymentMethodId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPaymentMethodDto> result;
		try
		{
			IERPPaymentMethodRepository iERPPaymentMethodRepository = (base.ERPPaymentMethodRepository = new ERPPaymentMethodRepository(base.ApiClientContext));
			using (iERPPaymentMethodRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPaymentMethodRepository.DeleteRowFromTable("PaymentMethods", "xah", paymentMethodId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PaymentMethod [{paymentMethodId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPaymentMethodDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPaymentMethodDto()
			};
		}
		return result;
	}
}
