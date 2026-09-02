using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPProjectedPaymentModel : ERPBaseModel, IERPProjectedPaymentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllProjectedPayments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPProjectedPaymentRepository iERPProjectedPaymentRepository = (base.ERPProjectedPaymentRepository = new ERPProjectedPaymentRepository(base.ApiClientContext));
		using (iERPProjectedPaymentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPProjectedPaymentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPProjectedPaymentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPProjectedPaymentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPProjectedPaymentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetProjectedPayment(Guid projectedPaymentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProjectedPaymentRepository iERPProjectedPaymentRepository = (base.ERPProjectedPaymentRepository = new ERPProjectedPaymentRepository(base.ApiClientContext));
		using (iERPProjectedPaymentRepository)
		{
			if (!(await base.ERPProjectedPaymentRepository.DoesProjectedPaymentExist(projectedPaymentId)))
			{
				errorsList.Add($"ProjectedPayment [{projectedPaymentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutProjectedPayment(ERPProjectedPaymentDto projectedPayment)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProjectedPaymentRepository iERPProjectedPaymentRepository = (base.ERPProjectedPaymentRepository = new ERPProjectedPaymentRepository(base.ApiClientContext));
		using (iERPProjectedPaymentRepository)
		{
			if (!string.IsNullOrWhiteSpace(projectedPayment.gloPlantDepartmentID) && !(await base.ERPProjectedPaymentRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { projectedPayment.gloPlantID, projectedPayment.gloPlantDepartmentID })))
			{
				errorsList.Add("gloPlantDepartmentID [" + projectedPayment.gloPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(projectedPayment.gloPlantID) && !(await base.ERPProjectedPaymentRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { projectedPayment.gloPlantID })))
			{
				errorsList.Add("gloPlantID [" + projectedPayment.gloPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(projectedPayment.gloOrganizationID) && !(await base.ERPProjectedPaymentRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { projectedPayment.gloOrganizationID })))
			{
				errorsList.Add("gloOrganizationID [" + projectedPayment.gloOrganizationID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPProjectedPaymentDto>>> Process_GetAllProjectedPayments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPProjectedPaymentDto> allProjectedPaymentsDto = new List<ERPProjectedPaymentDto>();
		ERPResponseMessageDto<IList<ERPProjectedPaymentDto>> result;
		try
		{
			IERPProjectedPaymentRepository iERPProjectedPaymentRepository = (base.ERPProjectedPaymentRepository = new ERPProjectedPaymentRepository(base.ApiClientContext));
			using (iERPProjectedPaymentRepository)
			{
				foreach (ERPProjectedPaymentInformationDto item2 in await base.ERPProjectedPaymentRepository.GetAllProjectedPayments(pageSize, pageNumber, filter, orderBy))
				{
					ERPProjectedPaymentDto item = new ERPProjectedPaymentDto
					{
						gloAmount = item2.gloAmount,
						gloClosedDate = item2.gloClosedDate,
						gloCreatedBy = item2.gloCreatedBy,
						gloCreatedDate = item2.gloCreatedDate,
						gloDescription = item2.gloDescription,
						gloUniqueID = item2.gloUniqueID,
						gloIgnoreAfterDate = item2.gloIgnoreAfterDate,
						gloClosed = item2.gloClosed,
						gloOrganizationID = item2.gloOrganizationID,
						gloPaymentDate = item2.gloPaymentDate,
						gloPaymentType = item2.gloPaymentType,
						gloPlantDepartmentID = item2.gloPlantDepartmentID,
						gloPlantID = item2.gloPlantID,
						gloRowVersion = item2.gloRowVersion,
						gloProjectedPaymentID = item2.gloProjectedPaymentID,
						CustomFields = item2.CustomFields
					};
					allProjectedPaymentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ProjectedPayments]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPProjectedPaymentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allProjectedPaymentsDto,
				RecordCount = allProjectedPaymentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProjectedPaymentDto>> Process_GetProjectedPayment(Guid projectedPaymentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPProjectedPaymentDto projectedPaymentDto = null;
		ERPResponseMessageDto<ERPProjectedPaymentDto> result;
		try
		{
			IERPProjectedPaymentRepository iERPProjectedPaymentRepository = (base.ERPProjectedPaymentRepository = new ERPProjectedPaymentRepository(base.ApiClientContext));
			using (iERPProjectedPaymentRepository)
			{
				ERPProjectedPaymentInformationDto eRPProjectedPaymentInformationDto = await base.ERPProjectedPaymentRepository.GetProjectedPayment(projectedPaymentId);
				projectedPaymentDto = new ERPProjectedPaymentDto
				{
					gloAmount = eRPProjectedPaymentInformationDto.gloAmount,
					gloClosedDate = eRPProjectedPaymentInformationDto.gloClosedDate,
					gloCreatedBy = eRPProjectedPaymentInformationDto.gloCreatedBy,
					gloCreatedDate = eRPProjectedPaymentInformationDto.gloCreatedDate,
					gloDescription = eRPProjectedPaymentInformationDto.gloDescription,
					gloUniqueID = eRPProjectedPaymentInformationDto.gloUniqueID,
					gloIgnoreAfterDate = eRPProjectedPaymentInformationDto.gloIgnoreAfterDate,
					gloClosed = eRPProjectedPaymentInformationDto.gloClosed,
					gloOrganizationID = eRPProjectedPaymentInformationDto.gloOrganizationID,
					gloPaymentDate = eRPProjectedPaymentInformationDto.gloPaymentDate,
					gloPaymentType = eRPProjectedPaymentInformationDto.gloPaymentType,
					gloPlantDepartmentID = eRPProjectedPaymentInformationDto.gloPlantDepartmentID,
					gloPlantID = eRPProjectedPaymentInformationDto.gloPlantID,
					gloRowVersion = eRPProjectedPaymentInformationDto.gloRowVersion,
					gloProjectedPaymentID = eRPProjectedPaymentInformationDto.gloProjectedPaymentID,
					CustomFields = eRPProjectedPaymentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ProjectedPayments []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProjectedPaymentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = projectedPaymentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProjectedPaymentDto>> Process_PutProjectedPayment(ERPProjectedPaymentDto projectedPayment)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPProjectedPaymentDto createdObject = null;
		ERPResponseMessageDto<ERPProjectedPaymentDto> result;
		try
		{
			IERPProjectedPaymentRepository iERPProjectedPaymentRepository = (base.ERPProjectedPaymentRepository = new ERPProjectedPaymentRepository(base.ApiClientContext));
			using (iERPProjectedPaymentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPProjectedPaymentRepository.SaveProjectedPayment(projectedPayment);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPProjectedPaymentInformationDto eRPProjectedPaymentInformationDto = await base.ERPProjectedPaymentRepository.GetProjectedPayment(projectedPayment.gloUniqueID);
					createdObject = new ERPProjectedPaymentDto
					{
						gloAmount = eRPProjectedPaymentInformationDto.gloAmount,
						gloClosedDate = eRPProjectedPaymentInformationDto.gloClosedDate,
						gloCreatedBy = eRPProjectedPaymentInformationDto.gloCreatedBy,
						gloCreatedDate = eRPProjectedPaymentInformationDto.gloCreatedDate,
						gloDescription = eRPProjectedPaymentInformationDto.gloDescription,
						gloUniqueID = eRPProjectedPaymentInformationDto.gloUniqueID,
						gloIgnoreAfterDate = eRPProjectedPaymentInformationDto.gloIgnoreAfterDate,
						gloClosed = eRPProjectedPaymentInformationDto.gloClosed,
						gloOrganizationID = eRPProjectedPaymentInformationDto.gloOrganizationID,
						gloPaymentDate = eRPProjectedPaymentInformationDto.gloPaymentDate,
						gloPaymentType = eRPProjectedPaymentInformationDto.gloPaymentType,
						gloPlantDepartmentID = eRPProjectedPaymentInformationDto.gloPlantDepartmentID,
						gloPlantID = eRPProjectedPaymentInformationDto.gloPlantID,
						gloRowVersion = eRPProjectedPaymentInformationDto.gloRowVersion,
						gloProjectedPaymentID = eRPProjectedPaymentInformationDto.gloProjectedPaymentID,
						CustomFields = eRPProjectedPaymentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ProjectedPayment [{projectedPayment.gloUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProjectedPaymentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteProjectedPayment(Guid projectedPaymentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProjectedPaymentRepository iERPProjectedPaymentRepository = (base.ERPProjectedPaymentRepository = new ERPProjectedPaymentRepository(base.ApiClientContext));
		using (iERPProjectedPaymentRepository)
		{
			if (!(await base.ERPProjectedPaymentRepository.DoesProjectedPaymentExist(projectedPaymentId)))
			{
				base.ErrorsList.Add($"ProjectedPayment [{projectedPaymentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPProjectedPaymentInformationDto eRPProjectedPaymentInformationDto = await base.ERPProjectedPaymentRepository.GetProjectedPayment(projectedPaymentId);
				string text = await base.ERPProjectedPaymentRepository.WhereUsed("ProjectedPayments", new object[1] { eRPProjectedPaymentInformationDto.gloProjectedPaymentID }, new object[1] { "gloProjectedPaymentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ProjectedPayment cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPProjectedPaymentDto>> Process_DeleteProjectedPayment(Guid projectedPaymentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPProjectedPaymentDto> result;
		try
		{
			IERPProjectedPaymentRepository iERPProjectedPaymentRepository = (base.ERPProjectedPaymentRepository = new ERPProjectedPaymentRepository(base.ApiClientContext));
			using (iERPProjectedPaymentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPProjectedPaymentRepository.DeleteRowFromTable("ProjectedPayments", "glo", projectedPaymentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ProjectedPayment [{projectedPaymentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProjectedPaymentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPProjectedPaymentDto()
			};
		}
		return result;
	}
}
