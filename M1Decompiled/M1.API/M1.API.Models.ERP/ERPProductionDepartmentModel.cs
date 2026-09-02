using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPProductionDepartmentModel : ERPBaseModel, IERPProductionDepartmentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllProductionDepartments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPProductionDepartmentRepository iERPProductionDepartmentRepository = (base.ERPProductionDepartmentRepository = new ERPProductionDepartmentRepository(base.ApiClientContext));
		using (iERPProductionDepartmentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPProductionDepartmentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPProductionDepartmentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPProductionDepartmentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPProductionDepartmentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetProductionDepartment(Guid productionDepartmentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProductionDepartmentRepository iERPProductionDepartmentRepository = (base.ERPProductionDepartmentRepository = new ERPProductionDepartmentRepository(base.ApiClientContext));
		using (iERPProductionDepartmentRepository)
		{
			if (!(await base.ERPProductionDepartmentRepository.DoesProductionDepartmentExist(productionDepartmentId)))
			{
				errorsList.Add($"ProductionDepartment [{productionDepartmentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutProductionDepartment(ERPProductionDepartmentDto productionDepartment)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPProductionDepartmentRepository iERPProductionDepartmentRepository = (base.ERPProductionDepartmentRepository = new ERPProductionDepartmentRepository(base.ApiClientContext));
		using (iERPProductionDepartmentRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPProductionDepartmentDto>>> Process_GetAllProductionDepartments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPProductionDepartmentDto> allProductionDepartmentsDto = new List<ERPProductionDepartmentDto>();
		ERPResponseMessageDto<IList<ERPProductionDepartmentDto>> result;
		try
		{
			IERPProductionDepartmentRepository iERPProductionDepartmentRepository = (base.ERPProductionDepartmentRepository = new ERPProductionDepartmentRepository(base.ApiClientContext));
			using (iERPProductionDepartmentRepository)
			{
				foreach (ERPProductionDepartmentInformationDto item2 in await base.ERPProductionDepartmentRepository.GetAllProductionDepartments(pageSize, pageNumber, filter, orderBy))
				{
					ERPProductionDepartmentDto item = new ERPProductionDepartmentDto
					{
						xaeProductionDepartmentID = item2.xaeProductionDepartmentID,
						xaeCreatedBy = item2.xaeCreatedBy,
						xaeCreatedDate = item2.xaeCreatedDate,
						xaeDescription = item2.xaeDescription,
						xaeUniqueID = item2.xaeUniqueID,
						xaeRowVersion = item2.xaeRowVersion,
						CustomFields = item2.CustomFields
					};
					allProductionDepartmentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ProductionDepartments]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPProductionDepartmentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allProductionDepartmentsDto,
				RecordCount = allProductionDepartmentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProductionDepartmentDto>> Process_GetProductionDepartment(Guid productionDepartmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPProductionDepartmentDto productionDepartmentDto = null;
		ERPResponseMessageDto<ERPProductionDepartmentDto> result;
		try
		{
			IERPProductionDepartmentRepository iERPProductionDepartmentRepository = (base.ERPProductionDepartmentRepository = new ERPProductionDepartmentRepository(base.ApiClientContext));
			using (iERPProductionDepartmentRepository)
			{
				ERPProductionDepartmentInformationDto eRPProductionDepartmentInformationDto = await base.ERPProductionDepartmentRepository.GetProductionDepartment(productionDepartmentId);
				productionDepartmentDto = new ERPProductionDepartmentDto
				{
					xaeProductionDepartmentID = eRPProductionDepartmentInformationDto.xaeProductionDepartmentID,
					xaeCreatedBy = eRPProductionDepartmentInformationDto.xaeCreatedBy,
					xaeCreatedDate = eRPProductionDepartmentInformationDto.xaeCreatedDate,
					xaeDescription = eRPProductionDepartmentInformationDto.xaeDescription,
					xaeUniqueID = eRPProductionDepartmentInformationDto.xaeUniqueID,
					xaeRowVersion = eRPProductionDepartmentInformationDto.xaeRowVersion,
					CustomFields = eRPProductionDepartmentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ProductionDepartments []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProductionDepartmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = productionDepartmentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProductionDepartmentDto>> Process_PutProductionDepartment(ERPProductionDepartmentDto productionDepartment)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPProductionDepartmentDto createdObject = null;
		ERPResponseMessageDto<ERPProductionDepartmentDto> result;
		try
		{
			IERPProductionDepartmentRepository iERPProductionDepartmentRepository = (base.ERPProductionDepartmentRepository = new ERPProductionDepartmentRepository(base.ApiClientContext));
			using (iERPProductionDepartmentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPProductionDepartmentRepository.SaveProductionDepartment(productionDepartment);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPProductionDepartmentInformationDto eRPProductionDepartmentInformationDto = await base.ERPProductionDepartmentRepository.GetProductionDepartment(productionDepartment.xaeUniqueID);
					createdObject = new ERPProductionDepartmentDto
					{
						xaeProductionDepartmentID = eRPProductionDepartmentInformationDto.xaeProductionDepartmentID,
						xaeCreatedBy = eRPProductionDepartmentInformationDto.xaeCreatedBy,
						xaeCreatedDate = eRPProductionDepartmentInformationDto.xaeCreatedDate,
						xaeDescription = eRPProductionDepartmentInformationDto.xaeDescription,
						xaeUniqueID = eRPProductionDepartmentInformationDto.xaeUniqueID,
						xaeRowVersion = eRPProductionDepartmentInformationDto.xaeRowVersion,
						CustomFields = eRPProductionDepartmentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ProductionDepartment [{productionDepartment.xaeUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProductionDepartmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteProductionDepartment(Guid productionDepartmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProductionDepartmentRepository iERPProductionDepartmentRepository = (base.ERPProductionDepartmentRepository = new ERPProductionDepartmentRepository(base.ApiClientContext));
		using (iERPProductionDepartmentRepository)
		{
			if (!(await base.ERPProductionDepartmentRepository.DoesProductionDepartmentExist(productionDepartmentId)))
			{
				base.ErrorsList.Add($"ProductionDepartment [{productionDepartmentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPProductionDepartmentInformationDto eRPProductionDepartmentInformationDto = await base.ERPProductionDepartmentRepository.GetProductionDepartment(productionDepartmentId);
				string text = await base.ERPProductionDepartmentRepository.WhereUsed("ProductionDepartments", new object[1] { eRPProductionDepartmentInformationDto.xaeProductionDepartmentID }, new object[1] { "xaeProductionDepartmentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ProductionDepartment cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPProductionDepartmentDto>> Process_DeleteProductionDepartment(Guid productionDepartmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPProductionDepartmentDto> result;
		try
		{
			IERPProductionDepartmentRepository iERPProductionDepartmentRepository = (base.ERPProductionDepartmentRepository = new ERPProductionDepartmentRepository(base.ApiClientContext));
			using (iERPProductionDepartmentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPProductionDepartmentRepository.DeleteRowFromTable("ProductionDepartments", "xae", productionDepartmentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ProductionDepartment [{productionDepartmentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProductionDepartmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPProductionDepartmentDto()
			};
		}
		return result;
	}
}
