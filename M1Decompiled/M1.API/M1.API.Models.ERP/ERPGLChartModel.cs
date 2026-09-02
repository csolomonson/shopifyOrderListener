using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPGLChartModel : ERPBaseModel, IERPGLChartModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllGLCharts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLChartRepository iERPGLChartRepository = (base.ERPGLChartRepository = new ERPGLChartRepository(base.ApiClientContext));
		using (iERPGLChartRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPGLChartRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPGLChartRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPGLChartRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPGLChartRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetGLChart(Guid gLChartId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLChartRepository iERPGLChartRepository = (base.ERPGLChartRepository = new ERPGLChartRepository(base.ApiClientContext));
		using (iERPGLChartRepository)
		{
			if (!(await base.ERPGLChartRepository.DoesGLChartExist(gLChartId)))
			{
				errorsList.Add($"GLChart [{gLChartId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutGLChart(ERPGLChartDto gLChart)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLChartRepository iERPGLChartRepository = (base.ERPGLChartRepository = new ERPGLChartRepository(base.ApiClientContext));
		using (iERPGLChartRepository)
		{
			if (!string.IsNullOrWhiteSpace(gLChart.glcParentGlChartID) && !(await base.ERPGLChartRepository.DoesRecordExistInTableUsingKeys("GLCharts", new object[1] { "GLCGLCHARTID" }, new object[1] { gLChart.glcParentGlChartID })))
			{
				errorsList.Add("glcParentGlChartID [" + gLChart.glcParentGlChartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLChart.glcGlCategoryID) && !(await base.ERPGLChartRepository.DoesRecordExistInTableUsingKeys("GLCategories", new object[1] { "GLTGLCATEGORYID" }, new object[1] { gLChart.glcGlCategoryID })))
			{
				errorsList.Add("glcGlCategoryID [" + gLChart.glcGlCategoryID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPGLChartDto>>> Process_GetAllGLCharts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPGLChartDto> allGLChartsDto = new List<ERPGLChartDto>();
		ERPResponseMessageDto<IList<ERPGLChartDto>> result;
		try
		{
			IERPGLChartRepository iERPGLChartRepository = (base.ERPGLChartRepository = new ERPGLChartRepository(base.ApiClientContext));
			using (iERPGLChartRepository)
			{
				foreach (ERPGLChartInformationDto item2 in await base.ERPGLChartRepository.GetAllGLCharts(pageSize, pageNumber, filter, orderBy))
				{
					ERPGLChartDto item = new ERPGLChartDto
					{
						glcAccountType = item2.glcAccountType,
						glcCashFlowCategory = item2.glcCashFlowCategory,
						glcGlChartID = item2.glcGlChartID,
						glcCogsAccountType = item2.glcCogsAccountType,
						glcCreatedBy = item2.glcCreatedBy,
						glcCreatedDate = item2.glcCreatedDate,
						glcDescription = item2.glcDescription,
						glcUniqueID = item2.glcUniqueID,
						glcGlCategoryID = item2.glcGlCategoryID,
						glcCashEquivalents = item2.glcCashEquivalents,
						glcParentAccount = item2.glcParentAccount,
						glcNormalBalance = item2.glcNormalBalance,
						glcParentDescription = item2.glcParentDescription,
						glcParentGlChartID = item2.glcParentGlChartID,
						glcRowVersion = item2.glcRowVersion,
						CustomFields = item2.CustomFields
					};
					allGLChartsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all GLCharts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPGLChartDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allGLChartsDto,
				RecordCount = allGLChartsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLChartDto>> Process_GetGLChart(Guid gLChartId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPGLChartDto gLChartDto = null;
		ERPResponseMessageDto<ERPGLChartDto> result;
		try
		{
			IERPGLChartRepository iERPGLChartRepository = (base.ERPGLChartRepository = new ERPGLChartRepository(base.ApiClientContext));
			using (iERPGLChartRepository)
			{
				ERPGLChartInformationDto eRPGLChartInformationDto = await base.ERPGLChartRepository.GetGLChart(gLChartId);
				gLChartDto = new ERPGLChartDto
				{
					glcAccountType = eRPGLChartInformationDto.glcAccountType,
					glcCashFlowCategory = eRPGLChartInformationDto.glcCashFlowCategory,
					glcGlChartID = eRPGLChartInformationDto.glcGlChartID,
					glcCogsAccountType = eRPGLChartInformationDto.glcCogsAccountType,
					glcCreatedBy = eRPGLChartInformationDto.glcCreatedBy,
					glcCreatedDate = eRPGLChartInformationDto.glcCreatedDate,
					glcDescription = eRPGLChartInformationDto.glcDescription,
					glcUniqueID = eRPGLChartInformationDto.glcUniqueID,
					glcGlCategoryID = eRPGLChartInformationDto.glcGlCategoryID,
					glcCashEquivalents = eRPGLChartInformationDto.glcCashEquivalents,
					glcParentAccount = eRPGLChartInformationDto.glcParentAccount,
					glcNormalBalance = eRPGLChartInformationDto.glcNormalBalance,
					glcParentDescription = eRPGLChartInformationDto.glcParentDescription,
					glcParentGlChartID = eRPGLChartInformationDto.glcParentGlChartID,
					glcRowVersion = eRPGLChartInformationDto.glcRowVersion,
					CustomFields = eRPGLChartInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the GLCharts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLChartDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = gLChartDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLChartDto>> Process_PutGLChart(ERPGLChartDto gLChart)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPGLChartDto createdObject = null;
		ERPResponseMessageDto<ERPGLChartDto> result;
		try
		{
			IERPGLChartRepository iERPGLChartRepository = (base.ERPGLChartRepository = new ERPGLChartRepository(base.ApiClientContext));
			using (iERPGLChartRepository)
			{
				APIValidationInfoDto postResult = await base.ERPGLChartRepository.SaveGLChart(gLChart);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPGLChartInformationDto eRPGLChartInformationDto = await base.ERPGLChartRepository.GetGLChart(gLChart.glcUniqueID);
					createdObject = new ERPGLChartDto
					{
						glcAccountType = eRPGLChartInformationDto.glcAccountType,
						glcCashFlowCategory = eRPGLChartInformationDto.glcCashFlowCategory,
						glcGlChartID = eRPGLChartInformationDto.glcGlChartID,
						glcCogsAccountType = eRPGLChartInformationDto.glcCogsAccountType,
						glcCreatedBy = eRPGLChartInformationDto.glcCreatedBy,
						glcCreatedDate = eRPGLChartInformationDto.glcCreatedDate,
						glcDescription = eRPGLChartInformationDto.glcDescription,
						glcUniqueID = eRPGLChartInformationDto.glcUniqueID,
						glcGlCategoryID = eRPGLChartInformationDto.glcGlCategoryID,
						glcCashEquivalents = eRPGLChartInformationDto.glcCashEquivalents,
						glcParentAccount = eRPGLChartInformationDto.glcParentAccount,
						glcNormalBalance = eRPGLChartInformationDto.glcNormalBalance,
						glcParentDescription = eRPGLChartInformationDto.glcParentDescription,
						glcParentGlChartID = eRPGLChartInformationDto.glcParentGlChartID,
						glcRowVersion = eRPGLChartInformationDto.glcRowVersion,
						CustomFields = eRPGLChartInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing GLChart [{gLChart.glcUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLChartDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteGLChart(Guid gLChartId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLChartRepository iERPGLChartRepository = (base.ERPGLChartRepository = new ERPGLChartRepository(base.ApiClientContext));
		using (iERPGLChartRepository)
		{
			if (!(await base.ERPGLChartRepository.DoesGLChartExist(gLChartId)))
			{
				base.ErrorsList.Add($"GLChart [{gLChartId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPGLChartInformationDto eRPGLChartInformationDto = await base.ERPGLChartRepository.GetGLChart(gLChartId);
				string text = await base.ERPGLChartRepository.WhereUsed("GLCharts", new object[1] { eRPGLChartInformationDto.glcGlChartID }, new object[1] { "glcGlChartID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("GLChart cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPGLChartDto>> Process_DeleteGLChart(Guid gLChartId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPGLChartDto> result;
		try
		{
			IERPGLChartRepository iERPGLChartRepository = (base.ERPGLChartRepository = new ERPGLChartRepository(base.ApiClientContext));
			using (iERPGLChartRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPGLChartRepository.DeleteRowFromTable("GLCharts", "glc", gLChartId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of GLChart [{gLChartId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLChartDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPGLChartDto()
			};
		}
		return result;
	}
}
