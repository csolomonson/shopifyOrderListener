using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartTransactionCostModel : ERPBaseModel, IERPPartTransactionCostModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartTransactionCosts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartTransactionCostRepository iERPPartTransactionCostRepository = (base.ERPPartTransactionCostRepository = new ERPPartTransactionCostRepository(base.ApiClientContext));
		using (iERPPartTransactionCostRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartTransactionCostRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartTransactionCostRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartTransactionCostRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartTransactionCostRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartTransactionCost(Guid partTransactionCostId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartTransactionCostRepository iERPPartTransactionCostRepository = (base.ERPPartTransactionCostRepository = new ERPPartTransactionCostRepository(base.ApiClientContext));
		using (iERPPartTransactionCostRepository)
		{
			if (!(await base.ERPPartTransactionCostRepository.DoesPartTransactionCostExist(partTransactionCostId)))
			{
				errorsList.Add($"PartTransactionCost [{partTransactionCostId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartTransactionCost(ERPPartTransactionCostDto partTransactionCost)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartTransactionCostRepository iERPPartTransactionCostRepository = (base.ERPPartTransactionCostRepository = new ERPPartTransactionCostRepository(base.ApiClientContext));
		using (iERPPartTransactionCostRepository)
		{
			if (partTransactionCost.intPartTransactionID > 0 && !(await base.ERPPartTransactionCostRepository.DoesRecordExistInTableUsingKeys("PartTransactions", new object[1] { "IMTPARTTRANSACTIONID" }, new object[1] { partTransactionCost.intPartTransactionID })))
			{
				errorsList.Add($"intPartTransactionID [{partTransactionCost.intPartTransactionID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartTransactionCostDto>>> Process_GetAllPartTransactionCosts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartTransactionCostDto> allPartTransactionCostsDto = new List<ERPPartTransactionCostDto>();
		ERPResponseMessageDto<IList<ERPPartTransactionCostDto>> result;
		try
		{
			IERPPartTransactionCostRepository iERPPartTransactionCostRepository = (base.ERPPartTransactionCostRepository = new ERPPartTransactionCostRepository(base.ApiClientContext));
			using (iERPPartTransactionCostRepository)
			{
				foreach (ERPPartTransactionCostInformationDto item2 in await base.ERPPartTransactionCostRepository.GetAllPartTransactionCosts(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartTransactionCostDto item = new ERPPartTransactionCostDto
					{
						intActualUnitDutyCost = item2.intActualUnitDutyCost,
						intActualUnitFreightCost = item2.intActualUnitFreightCost,
						intActualUnitLaborCost = item2.intActualUnitLaborCost,
						intActualUnitMaterialCost = item2.intActualUnitMaterialCost,
						intActualUnitMiscCost = item2.intActualUnitMiscCost,
						intActualUnitOverheadCost = item2.intActualUnitOverheadCost,
						intActualUnitSubcontractCost = item2.intActualUnitSubcontractCost,
						intCostType = item2.intCostType,
						intCreatedBy = item2.intCreatedBy,
						intCreatedDate = item2.intCreatedDate,
						intUniqueID = item2.intUniqueID,
						intPartTransactionID = item2.intPartTransactionID,
						intPrevUnitDutyCost = item2.intPrevUnitDutyCost,
						intPrevUnitFreightCost = item2.intPrevUnitFreightCost,
						intPrevUnitLaborCost = item2.intPrevUnitLaborCost,
						intPrevUnitMaterialCost = item2.intPrevUnitMaterialCost,
						intPrevUnitMiscCost = item2.intPrevUnitMiscCost,
						intPrevUnitOverheadCost = item2.intPrevUnitOverheadCost,
						intPrevUnitSubcontractCost = item2.intPrevUnitSubcontractCost,
						intQuantity = item2.intQuantity,
						intRowVersion = item2.intRowVersion,
						intPartTransactionCostID = item2.intPartTransactionCostID,
						intSourceTableName = item2.intSourceTableName,
						intSourceTableUniqueID = item2.intSourceTableUniqueID,
						intUnitDutyCost = item2.intUnitDutyCost,
						intUnitFreightCost = item2.intUnitFreightCost,
						intUnitLaborCost = item2.intUnitLaborCost,
						intUnitMaterialCost = item2.intUnitMaterialCost,
						intUnitMiscCost = item2.intUnitMiscCost,
						intUnitOverheadCost = item2.intUnitOverheadCost,
						intUnitSubcontractCost = item2.intUnitSubcontractCost,
						CustomFields = item2.CustomFields
					};
					allPartTransactionCostsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartTransactionCosts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartTransactionCostDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartTransactionCostsDto,
				RecordCount = allPartTransactionCostsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartTransactionCostDto>> Process_GetPartTransactionCost(Guid partTransactionCostId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartTransactionCostDto partTransactionCostDto = null;
		ERPResponseMessageDto<ERPPartTransactionCostDto> result;
		try
		{
			IERPPartTransactionCostRepository iERPPartTransactionCostRepository = (base.ERPPartTransactionCostRepository = new ERPPartTransactionCostRepository(base.ApiClientContext));
			using (iERPPartTransactionCostRepository)
			{
				ERPPartTransactionCostInformationDto eRPPartTransactionCostInformationDto = await base.ERPPartTransactionCostRepository.GetPartTransactionCost(partTransactionCostId);
				partTransactionCostDto = new ERPPartTransactionCostDto
				{
					intActualUnitDutyCost = eRPPartTransactionCostInformationDto.intActualUnitDutyCost,
					intActualUnitFreightCost = eRPPartTransactionCostInformationDto.intActualUnitFreightCost,
					intActualUnitLaborCost = eRPPartTransactionCostInformationDto.intActualUnitLaborCost,
					intActualUnitMaterialCost = eRPPartTransactionCostInformationDto.intActualUnitMaterialCost,
					intActualUnitMiscCost = eRPPartTransactionCostInformationDto.intActualUnitMiscCost,
					intActualUnitOverheadCost = eRPPartTransactionCostInformationDto.intActualUnitOverheadCost,
					intActualUnitSubcontractCost = eRPPartTransactionCostInformationDto.intActualUnitSubcontractCost,
					intCostType = eRPPartTransactionCostInformationDto.intCostType,
					intCreatedBy = eRPPartTransactionCostInformationDto.intCreatedBy,
					intCreatedDate = eRPPartTransactionCostInformationDto.intCreatedDate,
					intUniqueID = eRPPartTransactionCostInformationDto.intUniqueID,
					intPartTransactionID = eRPPartTransactionCostInformationDto.intPartTransactionID,
					intPrevUnitDutyCost = eRPPartTransactionCostInformationDto.intPrevUnitDutyCost,
					intPrevUnitFreightCost = eRPPartTransactionCostInformationDto.intPrevUnitFreightCost,
					intPrevUnitLaborCost = eRPPartTransactionCostInformationDto.intPrevUnitLaborCost,
					intPrevUnitMaterialCost = eRPPartTransactionCostInformationDto.intPrevUnitMaterialCost,
					intPrevUnitMiscCost = eRPPartTransactionCostInformationDto.intPrevUnitMiscCost,
					intPrevUnitOverheadCost = eRPPartTransactionCostInformationDto.intPrevUnitOverheadCost,
					intPrevUnitSubcontractCost = eRPPartTransactionCostInformationDto.intPrevUnitSubcontractCost,
					intQuantity = eRPPartTransactionCostInformationDto.intQuantity,
					intRowVersion = eRPPartTransactionCostInformationDto.intRowVersion,
					intPartTransactionCostID = eRPPartTransactionCostInformationDto.intPartTransactionCostID,
					intSourceTableName = eRPPartTransactionCostInformationDto.intSourceTableName,
					intSourceTableUniqueID = eRPPartTransactionCostInformationDto.intSourceTableUniqueID,
					intUnitDutyCost = eRPPartTransactionCostInformationDto.intUnitDutyCost,
					intUnitFreightCost = eRPPartTransactionCostInformationDto.intUnitFreightCost,
					intUnitLaborCost = eRPPartTransactionCostInformationDto.intUnitLaborCost,
					intUnitMaterialCost = eRPPartTransactionCostInformationDto.intUnitMaterialCost,
					intUnitMiscCost = eRPPartTransactionCostInformationDto.intUnitMiscCost,
					intUnitOverheadCost = eRPPartTransactionCostInformationDto.intUnitOverheadCost,
					intUnitSubcontractCost = eRPPartTransactionCostInformationDto.intUnitSubcontractCost,
					CustomFields = eRPPartTransactionCostInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartTransactionCosts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartTransactionCostDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partTransactionCostDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartTransactionCostDto>> Process_PutPartTransactionCost(ERPPartTransactionCostDto partTransactionCost)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartTransactionCostDto createdObject = null;
		ERPResponseMessageDto<ERPPartTransactionCostDto> result;
		try
		{
			IERPPartTransactionCostRepository iERPPartTransactionCostRepository = (base.ERPPartTransactionCostRepository = new ERPPartTransactionCostRepository(base.ApiClientContext));
			using (iERPPartTransactionCostRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartTransactionCostRepository.SavePartTransactionCost(partTransactionCost);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartTransactionCostInformationDto eRPPartTransactionCostInformationDto = await base.ERPPartTransactionCostRepository.GetPartTransactionCost(partTransactionCost.intUniqueID);
					createdObject = new ERPPartTransactionCostDto
					{
						intActualUnitDutyCost = eRPPartTransactionCostInformationDto.intActualUnitDutyCost,
						intActualUnitFreightCost = eRPPartTransactionCostInformationDto.intActualUnitFreightCost,
						intActualUnitLaborCost = eRPPartTransactionCostInformationDto.intActualUnitLaborCost,
						intActualUnitMaterialCost = eRPPartTransactionCostInformationDto.intActualUnitMaterialCost,
						intActualUnitMiscCost = eRPPartTransactionCostInformationDto.intActualUnitMiscCost,
						intActualUnitOverheadCost = eRPPartTransactionCostInformationDto.intActualUnitOverheadCost,
						intActualUnitSubcontractCost = eRPPartTransactionCostInformationDto.intActualUnitSubcontractCost,
						intCostType = eRPPartTransactionCostInformationDto.intCostType,
						intCreatedBy = eRPPartTransactionCostInformationDto.intCreatedBy,
						intCreatedDate = eRPPartTransactionCostInformationDto.intCreatedDate,
						intUniqueID = eRPPartTransactionCostInformationDto.intUniqueID,
						intPartTransactionID = eRPPartTransactionCostInformationDto.intPartTransactionID,
						intPrevUnitDutyCost = eRPPartTransactionCostInformationDto.intPrevUnitDutyCost,
						intPrevUnitFreightCost = eRPPartTransactionCostInformationDto.intPrevUnitFreightCost,
						intPrevUnitLaborCost = eRPPartTransactionCostInformationDto.intPrevUnitLaborCost,
						intPrevUnitMaterialCost = eRPPartTransactionCostInformationDto.intPrevUnitMaterialCost,
						intPrevUnitMiscCost = eRPPartTransactionCostInformationDto.intPrevUnitMiscCost,
						intPrevUnitOverheadCost = eRPPartTransactionCostInformationDto.intPrevUnitOverheadCost,
						intPrevUnitSubcontractCost = eRPPartTransactionCostInformationDto.intPrevUnitSubcontractCost,
						intQuantity = eRPPartTransactionCostInformationDto.intQuantity,
						intRowVersion = eRPPartTransactionCostInformationDto.intRowVersion,
						intPartTransactionCostID = eRPPartTransactionCostInformationDto.intPartTransactionCostID,
						intSourceTableName = eRPPartTransactionCostInformationDto.intSourceTableName,
						intSourceTableUniqueID = eRPPartTransactionCostInformationDto.intSourceTableUniqueID,
						intUnitDutyCost = eRPPartTransactionCostInformationDto.intUnitDutyCost,
						intUnitFreightCost = eRPPartTransactionCostInformationDto.intUnitFreightCost,
						intUnitLaborCost = eRPPartTransactionCostInformationDto.intUnitLaborCost,
						intUnitMaterialCost = eRPPartTransactionCostInformationDto.intUnitMaterialCost,
						intUnitMiscCost = eRPPartTransactionCostInformationDto.intUnitMiscCost,
						intUnitOverheadCost = eRPPartTransactionCostInformationDto.intUnitOverheadCost,
						intUnitSubcontractCost = eRPPartTransactionCostInformationDto.intUnitSubcontractCost,
						CustomFields = eRPPartTransactionCostInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartTransactionCost [{partTransactionCost.intUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartTransactionCostDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartTransactionCost(Guid partTransactionCostId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartTransactionCostRepository iERPPartTransactionCostRepository = (base.ERPPartTransactionCostRepository = new ERPPartTransactionCostRepository(base.ApiClientContext));
		using (iERPPartTransactionCostRepository)
		{
			if (!(await base.ERPPartTransactionCostRepository.DoesPartTransactionCostExist(partTransactionCostId)))
			{
				base.ErrorsList.Add($"PartTransactionCost [{partTransactionCostId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartTransactionCostInformationDto eRPPartTransactionCostInformationDto = await base.ERPPartTransactionCostRepository.GetPartTransactionCost(partTransactionCostId);
				string text = await base.ERPPartTransactionCostRepository.WhereUsed("PartTransactionCosts", new object[2] { eRPPartTransactionCostInformationDto.intPartTransactionID, eRPPartTransactionCostInformationDto.intPartTransactionCostID }, new object[2] { "intPartTransactionID", "intPartTransactionCostID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartTransactionCost cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartTransactionCostDto>> Process_DeletePartTransactionCost(Guid partTransactionCostId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartTransactionCostDto> result;
		try
		{
			IERPPartTransactionCostRepository iERPPartTransactionCostRepository = (base.ERPPartTransactionCostRepository = new ERPPartTransactionCostRepository(base.ApiClientContext));
			using (iERPPartTransactionCostRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartTransactionCostRepository.DeleteRowFromTable("PartTransactionCosts", "int", partTransactionCostId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartTransactionCost [{partTransactionCostId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartTransactionCostDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartTransactionCostDto()
			};
		}
		return result;
	}
}
