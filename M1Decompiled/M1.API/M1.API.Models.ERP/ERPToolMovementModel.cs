using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPToolMovementModel : ERPBaseModel, IERPToolMovementModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllToolMovements(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPToolMovementRepository iERPToolMovementRepository = (base.ERPToolMovementRepository = new ERPToolMovementRepository(base.ApiClientContext));
		using (iERPToolMovementRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPToolMovementRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPToolMovementRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPToolMovementRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPToolMovementRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetToolMovement(Guid toolMovementId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPToolMovementRepository iERPToolMovementRepository = (base.ERPToolMovementRepository = new ERPToolMovementRepository(base.ApiClientContext));
		using (iERPToolMovementRepository)
		{
			if (!(await base.ERPToolMovementRepository.DoesToolMovementExist(toolMovementId)))
			{
				errorsList.Add($"ToolMovement [{toolMovementId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutToolMovement(ERPToolMovementDto toolMovement)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPToolMovementRepository iERPToolMovementRepository = (base.ERPToolMovementRepository = new ERPToolMovementRepository(base.ApiClientContext));
		using (iERPToolMovementRepository)
		{
			if (!string.IsNullOrWhiteSpace(toolMovement.xtaToolID) && !(await base.ERPToolMovementRepository.DoesRecordExistInTableUsingKeys("Tools", new object[1] { "xttToolID" }, new object[1] { toolMovement.xtaToolID })))
			{
				errorsList.Add("xtaToolID [" + toolMovement.xtaToolID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(toolMovement.xtaCheckedOutToEmployeeID) && !(await base.ERPToolMovementRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { toolMovement.xtaCheckedOutToEmployeeID })))
			{
				errorsList.Add("xtaCheckedOutToEmployeeID [" + toolMovement.xtaCheckedOutToEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(toolMovement.xtaCheckoutReasonID) && !(await base.ERPToolMovementRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { toolMovement.xtaCheckoutReasonID })))
			{
				errorsList.Add("xtaCheckoutReasonID [" + toolMovement.xtaCheckoutReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(toolMovement.xtaJobID) && !(await base.ERPToolMovementRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { toolMovement.xtaJobID })))
			{
				errorsList.Add("xtaJobID [" + toolMovement.xtaJobID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(toolMovement.xtaWorkCenterID) && !(await base.ERPToolMovementRepository.DoesRecordExistInTableUsingKeys("WorkCenters", new object[1] { "XAWWORKCENTERID" }, new object[1] { toolMovement.xtaWorkCenterID })))
			{
				errorsList.Add("xtaWorkCenterID [" + toolMovement.xtaWorkCenterID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(toolMovement.xtaProductionDepartmentID) && !(await base.ERPToolMovementRepository.DoesRecordExistInTableUsingKeys("ProductionDepartments", new object[1] { "XAEPRODUCTIONDEPARTMENTID" }, new object[1] { toolMovement.xtaProductionDepartmentID })))
			{
				errorsList.Add("xtaProductionDepartmentID [" + toolMovement.xtaProductionDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(toolMovement.xtaPlantID) && !(await base.ERPToolMovementRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { toolMovement.xtaPlantID })))
			{
				errorsList.Add("xtaPlantID [" + toolMovement.xtaPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(toolMovement.xtaPlantDepartmentID) && !(await base.ERPToolMovementRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { toolMovement.xtaPlantID, toolMovement.xtaPlantDepartmentID })))
			{
				errorsList.Add("xtaPlantDepartmentID [" + toolMovement.xtaPlantDepartmentID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPToolMovementDto>>> Process_GetAllToolMovements(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPToolMovementDto> allToolMovementsDto = new List<ERPToolMovementDto>();
		ERPResponseMessageDto<IList<ERPToolMovementDto>> result;
		try
		{
			IERPToolMovementRepository iERPToolMovementRepository = (base.ERPToolMovementRepository = new ERPToolMovementRepository(base.ApiClientContext));
			using (iERPToolMovementRepository)
			{
				foreach (ERPToolMovementInformationDto item2 in await base.ERPToolMovementRepository.GetAllToolMovements(pageSize, pageNumber, filter, orderBy))
				{
					ERPToolMovementDto item = new ERPToolMovementDto
					{
						xtaCheckedOutToEmployeeID = item2.xtaCheckedOutToEmployeeID,
						xtaCheckoutReasonID = item2.xtaCheckoutReasonID,
						xtaCreatedBy = item2.xtaCreatedBy,
						xtaCreatedDate = item2.xtaCreatedDate,
						xtaUniqueID = item2.xtaUniqueID,
						xtaJobID = item2.xtaJobID,
						xtaLocation = item2.xtaLocation,
						xtaMovementDate = item2.xtaMovementDate,
						xtaMovementType = item2.xtaMovementType,
						xtaNotesRTF = item2.xtaNotesRTF,
						xtaNotesText = item2.xtaNotesText,
						xtaPlannedReturnDate = item2.xtaPlannedReturnDate,
						xtaPlantDepartmentID = item2.xtaPlantDepartmentID,
						xtaPlantID = item2.xtaPlantID,
						xtaProductionDepartmentID = item2.xtaProductionDepartmentID,
						xtaRowVersion = item2.xtaRowVersion,
						xtaToolMovementID = item2.xtaToolMovementID,
						xtaToolID = item2.xtaToolID,
						xtaWorkCenterID = item2.xtaWorkCenterID,
						CustomFields = item2.CustomFields
					};
					allToolMovementsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ToolMovements]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPToolMovementDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allToolMovementsDto,
				RecordCount = allToolMovementsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPToolMovementDto>> Process_GetToolMovement(Guid toolMovementId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPToolMovementDto toolMovementDto = null;
		ERPResponseMessageDto<ERPToolMovementDto> result;
		try
		{
			IERPToolMovementRepository iERPToolMovementRepository = (base.ERPToolMovementRepository = new ERPToolMovementRepository(base.ApiClientContext));
			using (iERPToolMovementRepository)
			{
				ERPToolMovementInformationDto eRPToolMovementInformationDto = await base.ERPToolMovementRepository.GetToolMovement(toolMovementId);
				toolMovementDto = new ERPToolMovementDto
				{
					xtaCheckedOutToEmployeeID = eRPToolMovementInformationDto.xtaCheckedOutToEmployeeID,
					xtaCheckoutReasonID = eRPToolMovementInformationDto.xtaCheckoutReasonID,
					xtaCreatedBy = eRPToolMovementInformationDto.xtaCreatedBy,
					xtaCreatedDate = eRPToolMovementInformationDto.xtaCreatedDate,
					xtaUniqueID = eRPToolMovementInformationDto.xtaUniqueID,
					xtaJobID = eRPToolMovementInformationDto.xtaJobID,
					xtaLocation = eRPToolMovementInformationDto.xtaLocation,
					xtaMovementDate = eRPToolMovementInformationDto.xtaMovementDate,
					xtaMovementType = eRPToolMovementInformationDto.xtaMovementType,
					xtaNotesRTF = eRPToolMovementInformationDto.xtaNotesRTF,
					xtaNotesText = eRPToolMovementInformationDto.xtaNotesText,
					xtaPlannedReturnDate = eRPToolMovementInformationDto.xtaPlannedReturnDate,
					xtaPlantDepartmentID = eRPToolMovementInformationDto.xtaPlantDepartmentID,
					xtaPlantID = eRPToolMovementInformationDto.xtaPlantID,
					xtaProductionDepartmentID = eRPToolMovementInformationDto.xtaProductionDepartmentID,
					xtaRowVersion = eRPToolMovementInformationDto.xtaRowVersion,
					xtaToolMovementID = eRPToolMovementInformationDto.xtaToolMovementID,
					xtaToolID = eRPToolMovementInformationDto.xtaToolID,
					xtaWorkCenterID = eRPToolMovementInformationDto.xtaWorkCenterID,
					CustomFields = eRPToolMovementInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ToolMovements []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPToolMovementDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = toolMovementDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPToolMovementDto>> Process_PutToolMovement(ERPToolMovementDto toolMovement)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPToolMovementDto createdObject = null;
		ERPResponseMessageDto<ERPToolMovementDto> result;
		try
		{
			IERPToolMovementRepository iERPToolMovementRepository = (base.ERPToolMovementRepository = new ERPToolMovementRepository(base.ApiClientContext));
			using (iERPToolMovementRepository)
			{
				APIValidationInfoDto postResult = await base.ERPToolMovementRepository.SaveToolMovement(toolMovement);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPToolMovementInformationDto eRPToolMovementInformationDto = await base.ERPToolMovementRepository.GetToolMovement(toolMovement.xtaUniqueID);
					createdObject = new ERPToolMovementDto
					{
						xtaCheckedOutToEmployeeID = eRPToolMovementInformationDto.xtaCheckedOutToEmployeeID,
						xtaCheckoutReasonID = eRPToolMovementInformationDto.xtaCheckoutReasonID,
						xtaCreatedBy = eRPToolMovementInformationDto.xtaCreatedBy,
						xtaCreatedDate = eRPToolMovementInformationDto.xtaCreatedDate,
						xtaUniqueID = eRPToolMovementInformationDto.xtaUniqueID,
						xtaJobID = eRPToolMovementInformationDto.xtaJobID,
						xtaLocation = eRPToolMovementInformationDto.xtaLocation,
						xtaMovementDate = eRPToolMovementInformationDto.xtaMovementDate,
						xtaMovementType = eRPToolMovementInformationDto.xtaMovementType,
						xtaNotesRTF = eRPToolMovementInformationDto.xtaNotesRTF,
						xtaNotesText = eRPToolMovementInformationDto.xtaNotesText,
						xtaPlannedReturnDate = eRPToolMovementInformationDto.xtaPlannedReturnDate,
						xtaPlantDepartmentID = eRPToolMovementInformationDto.xtaPlantDepartmentID,
						xtaPlantID = eRPToolMovementInformationDto.xtaPlantID,
						xtaProductionDepartmentID = eRPToolMovementInformationDto.xtaProductionDepartmentID,
						xtaRowVersion = eRPToolMovementInformationDto.xtaRowVersion,
						xtaToolMovementID = eRPToolMovementInformationDto.xtaToolMovementID,
						xtaToolID = eRPToolMovementInformationDto.xtaToolID,
						xtaWorkCenterID = eRPToolMovementInformationDto.xtaWorkCenterID,
						CustomFields = eRPToolMovementInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ToolMovement [{toolMovement.xtaUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPToolMovementDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteToolMovement(Guid toolMovementId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPToolMovementRepository iERPToolMovementRepository = (base.ERPToolMovementRepository = new ERPToolMovementRepository(base.ApiClientContext));
		using (iERPToolMovementRepository)
		{
			if (!(await base.ERPToolMovementRepository.DoesToolMovementExist(toolMovementId)))
			{
				base.ErrorsList.Add($"ToolMovement [{toolMovementId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPToolMovementInformationDto eRPToolMovementInformationDto = await base.ERPToolMovementRepository.GetToolMovement(toolMovementId);
				string text = await base.ERPToolMovementRepository.WhereUsed("ToolMovements", new object[1] { eRPToolMovementInformationDto.xtaToolMovementID }, new object[1] { "xtaToolMovementID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ToolMovement cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPToolMovementDto>> Process_DeleteToolMovement(Guid toolMovementId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPToolMovementDto> result;
		try
		{
			IERPToolMovementRepository iERPToolMovementRepository = (base.ERPToolMovementRepository = new ERPToolMovementRepository(base.ApiClientContext));
			using (iERPToolMovementRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPToolMovementRepository.DeleteRowFromTable("ToolMovements", "xta", toolMovementId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ToolMovement [{toolMovementId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPToolMovementDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPToolMovementDto()
			};
		}
		return result;
	}
}
