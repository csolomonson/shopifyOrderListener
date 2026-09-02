using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPWorkCenterMachineModel : ERPBaseModel, IERPWorkCenterMachineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllWorkCenterMachines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPWorkCenterMachineRepository iERPWorkCenterMachineRepository = (base.ERPWorkCenterMachineRepository = new ERPWorkCenterMachineRepository(base.ApiClientContext));
		using (iERPWorkCenterMachineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPWorkCenterMachineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPWorkCenterMachineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPWorkCenterMachineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPWorkCenterMachineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetWorkCenterMachine(Guid workCenterMachineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWorkCenterMachineRepository iERPWorkCenterMachineRepository = (base.ERPWorkCenterMachineRepository = new ERPWorkCenterMachineRepository(base.ApiClientContext));
		using (iERPWorkCenterMachineRepository)
		{
			if (!(await base.ERPWorkCenterMachineRepository.DoesWorkCenterMachineExist(workCenterMachineId)))
			{
				errorsList.Add($"WorkCenterMachine [{workCenterMachineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutWorkCenterMachine(ERPWorkCenterMachineDto workCenterMachine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWorkCenterMachineRepository iERPWorkCenterMachineRepository = (base.ERPWorkCenterMachineRepository = new ERPWorkCenterMachineRepository(base.ApiClientContext));
		using (iERPWorkCenterMachineRepository)
		{
			if (!string.IsNullOrWhiteSpace(workCenterMachine.xaqWorkCenterID) && !(await base.ERPWorkCenterMachineRepository.DoesRecordExistInTableUsingKeys("WorkCenters", new object[1] { "XAWWORKCENTERID" }, new object[1] { workCenterMachine.xaqWorkCenterID })))
			{
				errorsList.Add("xaqWorkCenterID [" + workCenterMachine.xaqWorkCenterID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPWorkCenterMachineDto>>> Process_GetAllWorkCenterMachines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPWorkCenterMachineDto> allWorkCenterMachinesDto = new List<ERPWorkCenterMachineDto>();
		ERPResponseMessageDto<IList<ERPWorkCenterMachineDto>> result;
		try
		{
			IERPWorkCenterMachineRepository iERPWorkCenterMachineRepository = (base.ERPWorkCenterMachineRepository = new ERPWorkCenterMachineRepository(base.ApiClientContext));
			using (iERPWorkCenterMachineRepository)
			{
				foreach (ERPWorkCenterMachineInformationDto item2 in await base.ERPWorkCenterMachineRepository.GetAllWorkCenterMachines(pageSize, pageNumber, filter, orderBy))
				{
					ERPWorkCenterMachineDto item = new ERPWorkCenterMachineDto
					{
						xaqDescription = item2.xaqDescription,
						xaqUniqueID = item2.xaqUniqueID,
						xaqRowVersion = item2.xaqRowVersion,
						xaqWorkCenterMachineID = item2.xaqWorkCenterMachineID,
						xaqWorkCenterID = item2.xaqWorkCenterID,
						CustomFields = item2.CustomFields
					};
					allWorkCenterMachinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all WorkCenterMachines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPWorkCenterMachineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allWorkCenterMachinesDto,
				RecordCount = allWorkCenterMachinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWorkCenterMachineDto>> Process_GetWorkCenterMachine(Guid workCenterMachineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPWorkCenterMachineDto workCenterMachineDto = null;
		ERPResponseMessageDto<ERPWorkCenterMachineDto> result;
		try
		{
			IERPWorkCenterMachineRepository iERPWorkCenterMachineRepository = (base.ERPWorkCenterMachineRepository = new ERPWorkCenterMachineRepository(base.ApiClientContext));
			using (iERPWorkCenterMachineRepository)
			{
				ERPWorkCenterMachineInformationDto eRPWorkCenterMachineInformationDto = await base.ERPWorkCenterMachineRepository.GetWorkCenterMachine(workCenterMachineId);
				workCenterMachineDto = new ERPWorkCenterMachineDto
				{
					xaqDescription = eRPWorkCenterMachineInformationDto.xaqDescription,
					xaqUniqueID = eRPWorkCenterMachineInformationDto.xaqUniqueID,
					xaqRowVersion = eRPWorkCenterMachineInformationDto.xaqRowVersion,
					xaqWorkCenterMachineID = eRPWorkCenterMachineInformationDto.xaqWorkCenterMachineID,
					xaqWorkCenterID = eRPWorkCenterMachineInformationDto.xaqWorkCenterID,
					CustomFields = eRPWorkCenterMachineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the WorkCenterMachines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWorkCenterMachineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = workCenterMachineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWorkCenterMachineDto>> Process_PutWorkCenterMachine(ERPWorkCenterMachineDto workCenterMachine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPWorkCenterMachineDto createdObject = null;
		ERPResponseMessageDto<ERPWorkCenterMachineDto> result;
		try
		{
			IERPWorkCenterMachineRepository iERPWorkCenterMachineRepository = (base.ERPWorkCenterMachineRepository = new ERPWorkCenterMachineRepository(base.ApiClientContext));
			using (iERPWorkCenterMachineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPWorkCenterMachineRepository.SaveWorkCenterMachine(workCenterMachine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPWorkCenterMachineInformationDto eRPWorkCenterMachineInformationDto = await base.ERPWorkCenterMachineRepository.GetWorkCenterMachine(workCenterMachine.xaqUniqueID);
					createdObject = new ERPWorkCenterMachineDto
					{
						xaqDescription = eRPWorkCenterMachineInformationDto.xaqDescription,
						xaqUniqueID = eRPWorkCenterMachineInformationDto.xaqUniqueID,
						xaqRowVersion = eRPWorkCenterMachineInformationDto.xaqRowVersion,
						xaqWorkCenterMachineID = eRPWorkCenterMachineInformationDto.xaqWorkCenterMachineID,
						xaqWorkCenterID = eRPWorkCenterMachineInformationDto.xaqWorkCenterID,
						CustomFields = eRPWorkCenterMachineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing WorkCenterMachine [{workCenterMachine.xaqUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWorkCenterMachineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteWorkCenterMachine(Guid workCenterMachineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWorkCenterMachineRepository iERPWorkCenterMachineRepository = (base.ERPWorkCenterMachineRepository = new ERPWorkCenterMachineRepository(base.ApiClientContext));
		using (iERPWorkCenterMachineRepository)
		{
			if (!(await base.ERPWorkCenterMachineRepository.DoesWorkCenterMachineExist(workCenterMachineId)))
			{
				base.ErrorsList.Add($"WorkCenterMachine [{workCenterMachineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPWorkCenterMachineInformationDto eRPWorkCenterMachineInformationDto = await base.ERPWorkCenterMachineRepository.GetWorkCenterMachine(workCenterMachineId);
				string text = await base.ERPWorkCenterMachineRepository.WhereUsed("WorkCenterMachines", new object[2] { eRPWorkCenterMachineInformationDto.xaqWorkCenterID, eRPWorkCenterMachineInformationDto.xaqWorkCenterMachineID }, new object[2] { "xaqWorkCenterID", "xaqWorkCenterMachineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("WorkCenterMachine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPWorkCenterMachineDto>> Process_DeleteWorkCenterMachine(Guid workCenterMachineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPWorkCenterMachineDto> result;
		try
		{
			IERPWorkCenterMachineRepository iERPWorkCenterMachineRepository = (base.ERPWorkCenterMachineRepository = new ERPWorkCenterMachineRepository(base.ApiClientContext));
			using (iERPWorkCenterMachineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPWorkCenterMachineRepository.DeleteRowFromTable("WorkCenterMachines", "xaq", workCenterMachineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of WorkCenterMachine [{workCenterMachineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWorkCenterMachineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPWorkCenterMachineDto()
			};
		}
		return result;
	}
}
