using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPSalesOrderJobLinkModel : ERPBaseModel, IERPSalesOrderJobLinkModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderJobLinks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSalesOrderJobLinkRepository iERPSalesOrderJobLinkRepository = (base.ERPSalesOrderJobLinkRepository = new ERPSalesOrderJobLinkRepository(base.ApiClientContext));
		using (iERPSalesOrderJobLinkRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPSalesOrderJobLinkRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPSalesOrderJobLinkRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPSalesOrderJobLinkRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPSalesOrderJobLinkRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderJobLink(Guid salesOrderJobLinkId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderJobLinkRepository iERPSalesOrderJobLinkRepository = (base.ERPSalesOrderJobLinkRepository = new ERPSalesOrderJobLinkRepository(base.ApiClientContext));
		using (iERPSalesOrderJobLinkRepository)
		{
			if (!(await base.ERPSalesOrderJobLinkRepository.DoesSalesOrderJobLinkExist(salesOrderJobLinkId)))
			{
				errorsList.Add($"SalesOrderJobLink [{salesOrderJobLinkId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderJobLink(ERPSalesOrderJobLinkDto salesOrderJobLink)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderJobLinkRepository iERPSalesOrderJobLinkRepository = (base.ERPSalesOrderJobLinkRepository = new ERPSalesOrderJobLinkRepository(base.ApiClientContext));
		using (iERPSalesOrderJobLinkRepository)
		{
			if (!string.IsNullOrWhiteSpace(salesOrderJobLink.omjSalesOrderID) && !(await base.ERPSalesOrderJobLinkRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { salesOrderJobLink.omjSalesOrderID })))
			{
				errorsList.Add("omjSalesOrderID [" + salesOrderJobLink.omjSalesOrderID + "] not found.");
			}
			if (salesOrderJobLink.omjSalesOrderLineID > 0 && !(await base.ERPSalesOrderJobLinkRepository.DoesRecordExistInTableUsingKeys("SalesOrderLines", new object[2] { "OMLSALESORDERID", "OMLSALESORDERLINEID" }, new object[2] { salesOrderJobLink.omjSalesOrderID, salesOrderJobLink.omjSalesOrderLineID })))
			{
				errorsList.Add($"omjSalesOrderLineID [{salesOrderJobLink.omjSalesOrderLineID}] not found.");
			}
			if (salesOrderJobLink.omjSalesOrderDeliveryID > 0 && !(await base.ERPSalesOrderJobLinkRepository.DoesRecordExistInTableUsingKeys("SalesOrderDeliveries", new object[3] { "OMDSALESORDERID", "OMDSALESORDERLINEID", "OMDSALESORDERDELIVERYID" }, new object[3] { salesOrderJobLink.omjSalesOrderID, salesOrderJobLink.omjSalesOrderLineID, salesOrderJobLink.omjSalesOrderDeliveryID })))
			{
				errorsList.Add($"omjSalesOrderDeliveryID [{salesOrderJobLink.omjSalesOrderDeliveryID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderJobLink.omjJobID) && !(await base.ERPSalesOrderJobLinkRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { salesOrderJobLink.omjJobID })))
			{
				errorsList.Add("omjJobID [" + salesOrderJobLink.omjJobID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPSalesOrderJobLinkDto>>> Process_GetAllSalesOrderJobLinks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPSalesOrderJobLinkDto> allSalesOrderJobLinksDto = new List<ERPSalesOrderJobLinkDto>();
		ERPResponseMessageDto<IList<ERPSalesOrderJobLinkDto>> result;
		try
		{
			IERPSalesOrderJobLinkRepository iERPSalesOrderJobLinkRepository = (base.ERPSalesOrderJobLinkRepository = new ERPSalesOrderJobLinkRepository(base.ApiClientContext));
			using (iERPSalesOrderJobLinkRepository)
			{
				foreach (ERPSalesOrderJobLinkInformationDto item2 in await base.ERPSalesOrderJobLinkRepository.GetAllSalesOrderJobLinks(pageSize, pageNumber, filter, orderBy))
				{
					ERPSalesOrderJobLinkDto item = new ERPSalesOrderJobLinkDto
					{
						omjCreatedBy = item2.omjCreatedBy,
						omjCreatedDate = item2.omjCreatedDate,
						omjUniqueID = item2.omjUniqueID,
						omjClosed = item2.omjClosed,
						omjJobID = item2.omjJobID,
						omjLinkType = item2.omjLinkType,
						omjRowVersion = item2.omjRowVersion,
						omjSalesOrderDeliveryID = item2.omjSalesOrderDeliveryID,
						omjSalesOrderID = item2.omjSalesOrderID,
						omjSalesOrderLineID = item2.omjSalesOrderLineID,
						omjSalesOrderJobLinkID = item2.omjSalesOrderJobLinkID,
						CustomFields = item2.CustomFields
					};
					allSalesOrderJobLinksDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all SalesOrderJobLinks]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPSalesOrderJobLinkDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSalesOrderJobLinksDto,
				RecordCount = allSalesOrderJobLinksDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderJobLinkDto>> Process_GetSalesOrderJobLink(Guid salesOrderJobLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPSalesOrderJobLinkDto salesOrderJobLinkDto = null;
		ERPResponseMessageDto<ERPSalesOrderJobLinkDto> result;
		try
		{
			IERPSalesOrderJobLinkRepository iERPSalesOrderJobLinkRepository = (base.ERPSalesOrderJobLinkRepository = new ERPSalesOrderJobLinkRepository(base.ApiClientContext));
			using (iERPSalesOrderJobLinkRepository)
			{
				ERPSalesOrderJobLinkInformationDto eRPSalesOrderJobLinkInformationDto = await base.ERPSalesOrderJobLinkRepository.GetSalesOrderJobLink(salesOrderJobLinkId);
				salesOrderJobLinkDto = new ERPSalesOrderJobLinkDto
				{
					omjCreatedBy = eRPSalesOrderJobLinkInformationDto.omjCreatedBy,
					omjCreatedDate = eRPSalesOrderJobLinkInformationDto.omjCreatedDate,
					omjUniqueID = eRPSalesOrderJobLinkInformationDto.omjUniqueID,
					omjClosed = eRPSalesOrderJobLinkInformationDto.omjClosed,
					omjJobID = eRPSalesOrderJobLinkInformationDto.omjJobID,
					omjLinkType = eRPSalesOrderJobLinkInformationDto.omjLinkType,
					omjRowVersion = eRPSalesOrderJobLinkInformationDto.omjRowVersion,
					omjSalesOrderDeliveryID = eRPSalesOrderJobLinkInformationDto.omjSalesOrderDeliveryID,
					omjSalesOrderID = eRPSalesOrderJobLinkInformationDto.omjSalesOrderID,
					omjSalesOrderLineID = eRPSalesOrderJobLinkInformationDto.omjSalesOrderLineID,
					omjSalesOrderJobLinkID = eRPSalesOrderJobLinkInformationDto.omjSalesOrderJobLinkID,
					CustomFields = eRPSalesOrderJobLinkInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the SalesOrderJobLinks []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderJobLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = salesOrderJobLinkDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderJobLinkDto>> Process_PutSalesOrderJobLink(ERPSalesOrderJobLinkDto salesOrderJobLink)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPSalesOrderJobLinkDto createdObject = null;
		ERPResponseMessageDto<ERPSalesOrderJobLinkDto> result;
		try
		{
			IERPSalesOrderJobLinkRepository iERPSalesOrderJobLinkRepository = (base.ERPSalesOrderJobLinkRepository = new ERPSalesOrderJobLinkRepository(base.ApiClientContext));
			using (iERPSalesOrderJobLinkRepository)
			{
				APIValidationInfoDto postResult = await base.ERPSalesOrderJobLinkRepository.SaveSalesOrderJobLink(salesOrderJobLink);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPSalesOrderJobLinkInformationDto eRPSalesOrderJobLinkInformationDto = await base.ERPSalesOrderJobLinkRepository.GetSalesOrderJobLink(salesOrderJobLink.omjUniqueID);
					createdObject = new ERPSalesOrderJobLinkDto
					{
						omjCreatedBy = eRPSalesOrderJobLinkInformationDto.omjCreatedBy,
						omjCreatedDate = eRPSalesOrderJobLinkInformationDto.omjCreatedDate,
						omjUniqueID = eRPSalesOrderJobLinkInformationDto.omjUniqueID,
						omjClosed = eRPSalesOrderJobLinkInformationDto.omjClosed,
						omjJobID = eRPSalesOrderJobLinkInformationDto.omjJobID,
						omjLinkType = eRPSalesOrderJobLinkInformationDto.omjLinkType,
						omjRowVersion = eRPSalesOrderJobLinkInformationDto.omjRowVersion,
						omjSalesOrderDeliveryID = eRPSalesOrderJobLinkInformationDto.omjSalesOrderDeliveryID,
						omjSalesOrderID = eRPSalesOrderJobLinkInformationDto.omjSalesOrderID,
						omjSalesOrderLineID = eRPSalesOrderJobLinkInformationDto.omjSalesOrderLineID,
						omjSalesOrderJobLinkID = eRPSalesOrderJobLinkInformationDto.omjSalesOrderJobLinkID,
						CustomFields = eRPSalesOrderJobLinkInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing SalesOrderJobLink [{salesOrderJobLink.omjUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderJobLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderJobLink(Guid salesOrderJobLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderJobLinkRepository iERPSalesOrderJobLinkRepository = (base.ERPSalesOrderJobLinkRepository = new ERPSalesOrderJobLinkRepository(base.ApiClientContext));
		using (iERPSalesOrderJobLinkRepository)
		{
			if (!(await base.ERPSalesOrderJobLinkRepository.DoesSalesOrderJobLinkExist(salesOrderJobLinkId)))
			{
				base.ErrorsList.Add($"SalesOrderJobLink [{salesOrderJobLinkId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPSalesOrderJobLinkInformationDto eRPSalesOrderJobLinkInformationDto = await base.ERPSalesOrderJobLinkRepository.GetSalesOrderJobLink(salesOrderJobLinkId);
				string text = await base.ERPSalesOrderJobLinkRepository.WhereUsed("SalesOrderJobLinks", new object[3] { eRPSalesOrderJobLinkInformationDto.omjSalesOrderID, eRPSalesOrderJobLinkInformationDto.omjSalesOrderLineID, eRPSalesOrderJobLinkInformationDto.omjSalesOrderJobLinkID }, new object[3] { "omjSalesOrderID", "omjSalesOrderLineID", "omjSalesOrderJobLinkID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("SalesOrderJobLink cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderJobLinkDto>> Process_DeleteSalesOrderJobLink(Guid salesOrderJobLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPSalesOrderJobLinkDto> result;
		try
		{
			IERPSalesOrderJobLinkRepository iERPSalesOrderJobLinkRepository = (base.ERPSalesOrderJobLinkRepository = new ERPSalesOrderJobLinkRepository(base.ApiClientContext));
			using (iERPSalesOrderJobLinkRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPSalesOrderJobLinkRepository.DeleteRowFromTable("SalesOrderJobLinks", "omj", salesOrderJobLinkId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of SalesOrderJobLink [{salesOrderJobLinkId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderJobLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPSalesOrderJobLinkDto()
			};
		}
		return result;
	}
}
