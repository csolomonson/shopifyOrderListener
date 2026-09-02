using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAssetPoolTransactionModel : ERPBaseModel, IERPAssetPoolTransactionModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAssetPoolTransactions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAssetPoolTransactionRepository iERPAssetPoolTransactionRepository = (base.ERPAssetPoolTransactionRepository = new ERPAssetPoolTransactionRepository(base.ApiClientContext));
		using (iERPAssetPoolTransactionRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAssetPoolTransactionRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAssetPoolTransactionRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAssetPoolTransactionRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAssetPoolTransactionRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAssetPoolTransaction(Guid assetPoolTransactionId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetPoolTransactionRepository iERPAssetPoolTransactionRepository = (base.ERPAssetPoolTransactionRepository = new ERPAssetPoolTransactionRepository(base.ApiClientContext));
		using (iERPAssetPoolTransactionRepository)
		{
			if (!(await base.ERPAssetPoolTransactionRepository.DoesAssetPoolTransactionExist(assetPoolTransactionId)))
			{
				errorsList.Add($"AssetPoolTransaction [{assetPoolTransactionId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutAssetPoolTransaction(ERPAssetPoolTransactionDto assetPoolTransaction)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetPoolTransactionRepository iERPAssetPoolTransactionRepository = (base.ERPAssetPoolTransactionRepository = new ERPAssetPoolTransactionRepository(base.ApiClientContext));
		using (iERPAssetPoolTransactionRepository)
		{
			if (assetPoolTransaction.fawPoolYearID > 0 && !(await base.ERPAssetPoolTransactionRepository.DoesRecordExistInTableUsingKeys("AssetLowValuePool", new object[1] { "FAVPOOLYEARID" }, new object[1] { assetPoolTransaction.fawPoolYearID })))
			{
				errorsList.Add($"fawPoolYearID [{assetPoolTransaction.fawPoolYearID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(assetPoolTransaction.fawAssetID) && !(await base.ERPAssetPoolTransactionRepository.DoesRecordExistInTableUsingKeys("Assets", new object[1] { "FAPASSETID" }, new object[1] { assetPoolTransaction.fawAssetID })))
			{
				errorsList.Add("fawAssetID [" + assetPoolTransaction.fawAssetID + "] not found.");
			}
			if (assetPoolTransaction.fawAssetAdjustmentID > 0 && !(await base.ERPAssetPoolTransactionRepository.DoesRecordExistInTableUsingKeys("AssetAdjustments", new object[1] { "FAAASSETADJUSTMENTID" }, new object[1] { assetPoolTransaction.fawAssetAdjustmentID })))
			{
				errorsList.Add($"fawAssetAdjustmentID [{assetPoolTransaction.fawAssetAdjustmentID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAssetPoolTransactionDto>>> Process_GetAllAssetPoolTransactions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAssetPoolTransactionDto> allAssetPoolTransactionsDto = new List<ERPAssetPoolTransactionDto>();
		ERPResponseMessageDto<IList<ERPAssetPoolTransactionDto>> result;
		try
		{
			IERPAssetPoolTransactionRepository iERPAssetPoolTransactionRepository = (base.ERPAssetPoolTransactionRepository = new ERPAssetPoolTransactionRepository(base.ApiClientContext));
			using (iERPAssetPoolTransactionRepository)
			{
				foreach (ERPAssetPoolTransactionInformationDto item2 in await base.ERPAssetPoolTransactionRepository.GetAllAssetPoolTransactions(pageSize, pageNumber, filter, orderBy))
				{
					ERPAssetPoolTransactionDto item = new ERPAssetPoolTransactionDto
					{
						fawAmount = item2.fawAmount,
						fawAssetAdjustmentID = item2.fawAssetAdjustmentID,
						fawAssetID = item2.fawAssetID,
						fawCreatedBy = item2.fawCreatedBy,
						fawCreatedDate = item2.fawCreatedDate,
						fawUniqueID = item2.fawUniqueID,
						fawPoolTransactionID = item2.fawPoolTransactionID,
						fawPoolYearID = item2.fawPoolYearID,
						fawRowVersion = item2.fawRowVersion,
						fawTransactionDate = item2.fawTransactionDate,
						fawTransactionType = item2.fawTransactionType,
						CustomFields = item2.CustomFields
					};
					allAssetPoolTransactionsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all AssetPoolTransactions]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAssetPoolTransactionDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAssetPoolTransactionsDto,
				RecordCount = allAssetPoolTransactionsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAssetPoolTransactionDto>> Process_GetAssetPoolTransaction(Guid assetPoolTransactionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAssetPoolTransactionDto assetPoolTransactionDto = null;
		ERPResponseMessageDto<ERPAssetPoolTransactionDto> result;
		try
		{
			IERPAssetPoolTransactionRepository iERPAssetPoolTransactionRepository = (base.ERPAssetPoolTransactionRepository = new ERPAssetPoolTransactionRepository(base.ApiClientContext));
			using (iERPAssetPoolTransactionRepository)
			{
				ERPAssetPoolTransactionInformationDto eRPAssetPoolTransactionInformationDto = await base.ERPAssetPoolTransactionRepository.GetAssetPoolTransaction(assetPoolTransactionId);
				assetPoolTransactionDto = new ERPAssetPoolTransactionDto
				{
					fawAmount = eRPAssetPoolTransactionInformationDto.fawAmount,
					fawAssetAdjustmentID = eRPAssetPoolTransactionInformationDto.fawAssetAdjustmentID,
					fawAssetID = eRPAssetPoolTransactionInformationDto.fawAssetID,
					fawCreatedBy = eRPAssetPoolTransactionInformationDto.fawCreatedBy,
					fawCreatedDate = eRPAssetPoolTransactionInformationDto.fawCreatedDate,
					fawUniqueID = eRPAssetPoolTransactionInformationDto.fawUniqueID,
					fawPoolTransactionID = eRPAssetPoolTransactionInformationDto.fawPoolTransactionID,
					fawPoolYearID = eRPAssetPoolTransactionInformationDto.fawPoolYearID,
					fawRowVersion = eRPAssetPoolTransactionInformationDto.fawRowVersion,
					fawTransactionDate = eRPAssetPoolTransactionInformationDto.fawTransactionDate,
					fawTransactionType = eRPAssetPoolTransactionInformationDto.fawTransactionType,
					CustomFields = eRPAssetPoolTransactionInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the AssetPoolTransactions []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetPoolTransactionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = assetPoolTransactionDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAssetPoolTransactionDto>> Process_PutAssetPoolTransaction(ERPAssetPoolTransactionDto assetPoolTransaction)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPAssetPoolTransactionDto createdObject = null;
		ERPResponseMessageDto<ERPAssetPoolTransactionDto> result;
		try
		{
			IERPAssetPoolTransactionRepository iERPAssetPoolTransactionRepository = (base.ERPAssetPoolTransactionRepository = new ERPAssetPoolTransactionRepository(base.ApiClientContext));
			using (iERPAssetPoolTransactionRepository)
			{
				APIValidationInfoDto postResult = await base.ERPAssetPoolTransactionRepository.SaveAssetPoolTransaction(assetPoolTransaction);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPAssetPoolTransactionInformationDto eRPAssetPoolTransactionInformationDto = await base.ERPAssetPoolTransactionRepository.GetAssetPoolTransaction(assetPoolTransaction.fawUniqueID);
					createdObject = new ERPAssetPoolTransactionDto
					{
						fawAmount = eRPAssetPoolTransactionInformationDto.fawAmount,
						fawAssetAdjustmentID = eRPAssetPoolTransactionInformationDto.fawAssetAdjustmentID,
						fawAssetID = eRPAssetPoolTransactionInformationDto.fawAssetID,
						fawCreatedBy = eRPAssetPoolTransactionInformationDto.fawCreatedBy,
						fawCreatedDate = eRPAssetPoolTransactionInformationDto.fawCreatedDate,
						fawUniqueID = eRPAssetPoolTransactionInformationDto.fawUniqueID,
						fawPoolTransactionID = eRPAssetPoolTransactionInformationDto.fawPoolTransactionID,
						fawPoolYearID = eRPAssetPoolTransactionInformationDto.fawPoolYearID,
						fawRowVersion = eRPAssetPoolTransactionInformationDto.fawRowVersion,
						fawTransactionDate = eRPAssetPoolTransactionInformationDto.fawTransactionDate,
						fawTransactionType = eRPAssetPoolTransactionInformationDto.fawTransactionType,
						CustomFields = eRPAssetPoolTransactionInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing AssetPoolTransaction [{assetPoolTransaction.fawUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetPoolTransactionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteAssetPoolTransaction(Guid assetPoolTransactionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetPoolTransactionRepository iERPAssetPoolTransactionRepository = (base.ERPAssetPoolTransactionRepository = new ERPAssetPoolTransactionRepository(base.ApiClientContext));
		using (iERPAssetPoolTransactionRepository)
		{
			if (!(await base.ERPAssetPoolTransactionRepository.DoesAssetPoolTransactionExist(assetPoolTransactionId)))
			{
				base.ErrorsList.Add($"AssetPoolTransaction [{assetPoolTransactionId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPAssetPoolTransactionInformationDto eRPAssetPoolTransactionInformationDto = await base.ERPAssetPoolTransactionRepository.GetAssetPoolTransaction(assetPoolTransactionId);
				string text = await base.ERPAssetPoolTransactionRepository.WhereUsed("AssetPoolTransactions", new object[1] { eRPAssetPoolTransactionInformationDto.fawPoolTransactionID }, new object[1] { "fawPoolTransactionID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("AssetPoolTransaction cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPAssetPoolTransactionDto>> Process_DeleteAssetPoolTransaction(Guid assetPoolTransactionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPAssetPoolTransactionDto> result;
		try
		{
			IERPAssetPoolTransactionRepository iERPAssetPoolTransactionRepository = (base.ERPAssetPoolTransactionRepository = new ERPAssetPoolTransactionRepository(base.ApiClientContext));
			using (iERPAssetPoolTransactionRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPAssetPoolTransactionRepository.DeleteRowFromTable("AssetPoolTransactions", "faw", assetPoolTransactionId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of AssetPoolTransaction [{assetPoolTransactionId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetPoolTransactionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPAssetPoolTransactionDto()
			};
		}
		return result;
	}
}
