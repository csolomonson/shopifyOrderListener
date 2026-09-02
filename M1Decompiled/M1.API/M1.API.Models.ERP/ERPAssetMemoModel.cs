using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAssetMemoModel : ERPBaseModel, IERPAssetMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAssetMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAssetMemoRepository iERPAssetMemoRepository = (base.ERPAssetMemoRepository = new ERPAssetMemoRepository(base.ApiClientContext));
		using (iERPAssetMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAssetMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAssetMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAssetMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAssetMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAssetMemo(Guid assetMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetMemoRepository iERPAssetMemoRepository = (base.ERPAssetMemoRepository = new ERPAssetMemoRepository(base.ApiClientContext));
		using (iERPAssetMemoRepository)
		{
			if (!(await base.ERPAssetMemoRepository.DoesAssetMemoExist(assetMemoId)))
			{
				errorsList.Add($"AssetMemo [{assetMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutAssetMemo(ERPAssetMemoDto assetMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetMemoRepository iERPAssetMemoRepository = (base.ERPAssetMemoRepository = new ERPAssetMemoRepository(base.ApiClientContext));
		using (iERPAssetMemoRepository)
		{
			if (!string.IsNullOrWhiteSpace(assetMemo.fakAssetID) && !(await base.ERPAssetMemoRepository.DoesRecordExistInTableUsingKeys("Assets", new object[1] { "FAPASSETID" }, new object[1] { assetMemo.fakAssetID })))
			{
				errorsList.Add("fakAssetID [" + assetMemo.fakAssetID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAssetMemoDto>>> Process_GetAllAssetMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAssetMemoDto> allAssetMemosDto = new List<ERPAssetMemoDto>();
		ERPResponseMessageDto<IList<ERPAssetMemoDto>> result;
		try
		{
			IERPAssetMemoRepository iERPAssetMemoRepository = (base.ERPAssetMemoRepository = new ERPAssetMemoRepository(base.ApiClientContext));
			using (iERPAssetMemoRepository)
			{
				foreach (ERPAssetMemoInformationDto item2 in await base.ERPAssetMemoRepository.GetAllAssetMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPAssetMemoDto item = new ERPAssetMemoDto
					{
						fakAssetID = item2.fakAssetID,
						fakCreatedBy = item2.fakCreatedBy,
						fakCreatedDate = item2.fakCreatedDate,
						fakUniqueID = item2.fakUniqueID,
						fakLongDescriptionRtf = item2.fakLongDescriptionRtf,
						fakLongDescriptionText = item2.fakLongDescriptionText,
						fakMemoDate = item2.fakMemoDate,
						fakRowVersion = item2.fakRowVersion,
						fakAssetMemoID = item2.fakAssetMemoID,
						fakShortDescription = item2.fakShortDescription,
						CustomFields = item2.CustomFields
					};
					allAssetMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all AssetMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAssetMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAssetMemosDto,
				RecordCount = allAssetMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAssetMemoDto>> Process_GetAssetMemo(Guid assetMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAssetMemoDto assetMemoDto = null;
		ERPResponseMessageDto<ERPAssetMemoDto> result;
		try
		{
			IERPAssetMemoRepository iERPAssetMemoRepository = (base.ERPAssetMemoRepository = new ERPAssetMemoRepository(base.ApiClientContext));
			using (iERPAssetMemoRepository)
			{
				ERPAssetMemoInformationDto eRPAssetMemoInformationDto = await base.ERPAssetMemoRepository.GetAssetMemo(assetMemoId);
				assetMemoDto = new ERPAssetMemoDto
				{
					fakAssetID = eRPAssetMemoInformationDto.fakAssetID,
					fakCreatedBy = eRPAssetMemoInformationDto.fakCreatedBy,
					fakCreatedDate = eRPAssetMemoInformationDto.fakCreatedDate,
					fakUniqueID = eRPAssetMemoInformationDto.fakUniqueID,
					fakLongDescriptionRtf = eRPAssetMemoInformationDto.fakLongDescriptionRtf,
					fakLongDescriptionText = eRPAssetMemoInformationDto.fakLongDescriptionText,
					fakMemoDate = eRPAssetMemoInformationDto.fakMemoDate,
					fakRowVersion = eRPAssetMemoInformationDto.fakRowVersion,
					fakAssetMemoID = eRPAssetMemoInformationDto.fakAssetMemoID,
					fakShortDescription = eRPAssetMemoInformationDto.fakShortDescription,
					CustomFields = eRPAssetMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the AssetMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = assetMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAssetMemoDto>> Process_PutAssetMemo(ERPAssetMemoDto assetMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPAssetMemoDto createdObject = null;
		ERPResponseMessageDto<ERPAssetMemoDto> result;
		try
		{
			IERPAssetMemoRepository iERPAssetMemoRepository = (base.ERPAssetMemoRepository = new ERPAssetMemoRepository(base.ApiClientContext));
			using (iERPAssetMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPAssetMemoRepository.SaveAssetMemo(assetMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPAssetMemoInformationDto eRPAssetMemoInformationDto = await base.ERPAssetMemoRepository.GetAssetMemo(assetMemo.fakUniqueID);
					createdObject = new ERPAssetMemoDto
					{
						fakAssetID = eRPAssetMemoInformationDto.fakAssetID,
						fakCreatedBy = eRPAssetMemoInformationDto.fakCreatedBy,
						fakCreatedDate = eRPAssetMemoInformationDto.fakCreatedDate,
						fakUniqueID = eRPAssetMemoInformationDto.fakUniqueID,
						fakLongDescriptionRtf = eRPAssetMemoInformationDto.fakLongDescriptionRtf,
						fakLongDescriptionText = eRPAssetMemoInformationDto.fakLongDescriptionText,
						fakMemoDate = eRPAssetMemoInformationDto.fakMemoDate,
						fakRowVersion = eRPAssetMemoInformationDto.fakRowVersion,
						fakAssetMemoID = eRPAssetMemoInformationDto.fakAssetMemoID,
						fakShortDescription = eRPAssetMemoInformationDto.fakShortDescription,
						CustomFields = eRPAssetMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing AssetMemo [{assetMemo.fakUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteAssetMemo(Guid assetMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetMemoRepository iERPAssetMemoRepository = (base.ERPAssetMemoRepository = new ERPAssetMemoRepository(base.ApiClientContext));
		using (iERPAssetMemoRepository)
		{
			if (!(await base.ERPAssetMemoRepository.DoesAssetMemoExist(assetMemoId)))
			{
				base.ErrorsList.Add($"AssetMemo [{assetMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPAssetMemoInformationDto eRPAssetMemoInformationDto = await base.ERPAssetMemoRepository.GetAssetMemo(assetMemoId);
				string text = await base.ERPAssetMemoRepository.WhereUsed("AssetMemos", new object[2] { eRPAssetMemoInformationDto.fakAssetID, eRPAssetMemoInformationDto.fakAssetMemoID }, new object[2] { "fakAssetID", "fakAssetMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("AssetMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPAssetMemoDto>> Process_DeleteAssetMemo(Guid assetMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPAssetMemoDto> result;
		try
		{
			IERPAssetMemoRepository iERPAssetMemoRepository = (base.ERPAssetMemoRepository = new ERPAssetMemoRepository(base.ApiClientContext));
			using (iERPAssetMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPAssetMemoRepository.DeleteRowFromTable("AssetMemos", "fak", assetMemoId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of AssetMemo [{assetMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPAssetMemoDto()
			};
		}
		return result;
	}
}
