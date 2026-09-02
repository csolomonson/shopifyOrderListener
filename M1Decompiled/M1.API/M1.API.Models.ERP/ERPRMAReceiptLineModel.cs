using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPRMAReceiptLineModel : ERPBaseModel, IERPRMAReceiptLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllRMAReceiptLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPRMAReceiptLineRepository iERPRMAReceiptLineRepository = (base.ERPRMAReceiptLineRepository = new ERPRMAReceiptLineRepository(base.ApiClientContext));
		using (iERPRMAReceiptLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPRMAReceiptLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPRMAReceiptLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPRMAReceiptLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPRMAReceiptLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetRMAReceiptLine(Guid rMAReceiptLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAReceiptLineRepository iERPRMAReceiptLineRepository = (base.ERPRMAReceiptLineRepository = new ERPRMAReceiptLineRepository(base.ApiClientContext));
		using (iERPRMAReceiptLineRepository)
		{
			if (!(await base.ERPRMAReceiptLineRepository.DoesRMAReceiptLineExist(rMAReceiptLineId)))
			{
				errorsList.Add($"RMAReceiptLine [{rMAReceiptLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutRMAReceiptLine(ERPRMAReceiptLineDto rMAReceiptLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAReceiptLineRepository iERPRMAReceiptLineRepository = (base.ERPRMAReceiptLineRepository = new ERPRMAReceiptLineRepository(base.ApiClientContext));
		using (iERPRMAReceiptLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(rMAReceiptLine.rrlRmaReceiptID) && !(await base.ERPRMAReceiptLineRepository.DoesRecordExistInTableUsingKeys("RMAReceipts", new object[1] { "RRPRMARECEIPTID" }, new object[1] { rMAReceiptLine.rrlRmaReceiptID })))
			{
				errorsList.Add("rrlRmaReceiptID [" + rMAReceiptLine.rrlRmaReceiptID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceiptLine.rrlRmaClaimID) && !(await base.ERPRMAReceiptLineRepository.DoesRecordExistInTableUsingKeys("RMAClaims", new object[1] { "RAPRMACLAIMID" }, new object[1] { rMAReceiptLine.rrlRmaClaimID })))
			{
				errorsList.Add("rrlRmaClaimID [" + rMAReceiptLine.rrlRmaClaimID + "] not found.");
			}
			if (rMAReceiptLine.rrlRmaClaimLineID > 0 && !(await base.ERPRMAReceiptLineRepository.DoesRecordExistInTableUsingKeys("RMAClaimLines", new object[2] { "RALRMACLAIMID", "RALRMACLAIMLINEID" }, new object[2] { rMAReceiptLine.rrlRmaClaimID, rMAReceiptLine.rrlRmaClaimLineID })))
			{
				errorsList.Add($"rrlRmaClaimLineID [{rMAReceiptLine.rrlRmaClaimLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceiptLine.rrlPartID) && !(await base.ERPRMAReceiptLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { rMAReceiptLine.rrlPartID })))
			{
				errorsList.Add("rrlPartID [" + rMAReceiptLine.rrlPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceiptLine.rrlPartRevisionID) && !(await base.ERPRMAReceiptLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { rMAReceiptLine.rrlPartID, rMAReceiptLine.rrlPartRevisionID })))
			{
				errorsList.Add("rrlPartRevisionID [" + rMAReceiptLine.rrlPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceiptLine.rrlPartWarehouseLocationID) && !(await base.ERPRMAReceiptLineRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { rMAReceiptLine.rrlPartID, rMAReceiptLine.rrlPartRevisionID, rMAReceiptLine.rrlPartWarehouseLocationID })))
			{
				errorsList.Add("rrlPartWarehouseLocationID [" + rMAReceiptLine.rrlPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceiptLine.rrlPartBinID) && !(await base.ERPRMAReceiptLineRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { rMAReceiptLine.rrlPartID, rMAReceiptLine.rrlPartRevisionID, rMAReceiptLine.rrlPartWarehouseLocationID, rMAReceiptLine.rrlPartBinID })))
			{
				errorsList.Add("rrlPartBinID [" + rMAReceiptLine.rrlPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceiptLine.rrlProjectID) && !(await base.ERPRMAReceiptLineRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { rMAReceiptLine.rrlProjectID })))
			{
				errorsList.Add("rrlProjectID [" + rMAReceiptLine.rrlProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceiptLine.rrlProjectAreaID) && !(await base.ERPRMAReceiptLineRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { rMAReceiptLine.rrlProjectID, rMAReceiptLine.rrlProjectAreaID })))
			{
				errorsList.Add("rrlProjectAreaID [" + rMAReceiptLine.rrlProjectAreaID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceiptLine.rrlReverseRmaReceiptID) && !(await base.ERPRMAReceiptLineRepository.DoesRecordExistInTableUsingKeys("RMAReceipts", new object[1] { "RRPRMARECEIPTID" }, new object[1] { rMAReceiptLine.rrlReverseRmaReceiptID })))
			{
				errorsList.Add("rrlReverseRmaReceiptID [" + rMAReceiptLine.rrlReverseRmaReceiptID + "] not found.");
			}
			if (rMAReceiptLine.rrlReverseRmaReceiptLineID > 0 && !(await base.ERPRMAReceiptLineRepository.DoesRecordExistInTableUsingKeys("RMAReceiptLines", new object[2] { "RRLRMARECEIPTID", "RRLRMARECEIPTLINEID" }, new object[2] { rMAReceiptLine.rrlReverseRmaReceiptID, rMAReceiptLine.rrlReverseRmaReceiptLineID })))
			{
				errorsList.Add($"rrlReverseRmaReceiptLineID [{rMAReceiptLine.rrlReverseRmaReceiptLineID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPRMAReceiptLineDto>>> Process_GetAllRMAReceiptLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPRMAReceiptLineDto> allRMAReceiptLinesDto = new List<ERPRMAReceiptLineDto>();
		ERPResponseMessageDto<IList<ERPRMAReceiptLineDto>> result;
		try
		{
			IERPRMAReceiptLineRepository iERPRMAReceiptLineRepository = (base.ERPRMAReceiptLineRepository = new ERPRMAReceiptLineRepository(base.ApiClientContext));
			using (iERPRMAReceiptLineRepository)
			{
				foreach (ERPRMAReceiptLineInformationDto item2 in await base.ERPRMAReceiptLineRepository.GetAllRMAReceiptLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPRMAReceiptLineDto item = new ERPRMAReceiptLineDto
					{
						rrlConversionFactor = item2.rrlConversionFactor,
						rrlCreatedBy = item2.rrlCreatedBy,
						rrlCreatedDate = item2.rrlCreatedDate,
						rrlDescription = item2.rrlDescription,
						rrlUniqueID = item2.rrlUniqueID,
						rrlExtendedCost = item2.rrlExtendedCost,
						rrlExtendedCostForeign = item2.rrlExtendedCostForeign,
						rrlHeatLot = item2.rrlHeatLot,
						rrlInventoryQuantityReceived = item2.rrlInventoryQuantityReceived,
						rrlInventoryUnitOfMeasure = item2.rrlInventoryUnitOfMeasure,
						rrlClosed = item2.rrlClosed,
						rrlInInspection = item2.rrlInInspection,
						rrlInspectionComplete = item2.rrlInspectionComplete,
						rrlInvoicedComplete = item2.rrlInvoicedComplete,
						rrlKitPart = item2.rrlKitPart,
						rrlPosted = item2.rrlPosted,
						rrlReceivedComplete = item2.rrlReceivedComplete,
						rrlRequiresInspection = item2.rrlRequiresInspection,
						rrlReversed = item2.rrlReversed,
						rrlOrgPartID = item2.rrlOrgPartID,
						rrlOrgPartShortDescription = item2.rrlOrgPartShortDescription,
						rrlPartBinID = item2.rrlPartBinID,
						rrlPartID = item2.rrlPartID,
						rrlPartLongDescriptionRtf = item2.rrlPartLongDescriptionRtf,
						rrlPartLongDescriptionText = item2.rrlPartLongDescriptionText,
						rrlPartRevisionID = item2.rrlPartRevisionID,
						rrlPartWarehouseLocationID = item2.rrlPartWarehouseLocationID,
						rrlProjectAreaID = item2.rrlProjectAreaID,
						rrlProjectID = item2.rrlProjectID,
						rrlQuantityToInspect = item2.rrlQuantityToInspect,
						rrlReference = item2.rrlReference,
						rrlReverseRmaReceiptID = item2.rrlReverseRmaReceiptID,
						rrlReverseRmaReceiptLineID = item2.rrlReverseRmaReceiptLineID,
						rrlRmaClaimID = item2.rrlRmaClaimID,
						rrlRmaClaimLineID = item2.rrlRmaClaimLineID,
						rrlRmaClaimQuantity = item2.rrlRmaClaimQuantity,
						rrlRmaOpenQuantity = item2.rrlRmaOpenQuantity,
						rrlRmaReceiptID = item2.rrlRmaReceiptID,
						rrlRowVersion = item2.rrlRowVersion,
						rrlSalesQuantityReceived = item2.rrlSalesQuantityReceived,
						rrlSalesUnitOfMeasure = item2.rrlSalesUnitOfMeasure,
						rrlRmaReceiptLineID = item2.rrlRmaReceiptLineID,
						rrlTotalComponentCosts = item2.rrlTotalComponentCosts,
						rrlUnitCost = item2.rrlUnitCost,
						rrlUnitCostForeign = item2.rrlUnitCostForeign,
						CustomFields = item2.CustomFields
					};
					allRMAReceiptLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all RMAReceiptLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPRMAReceiptLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allRMAReceiptLinesDto,
				RecordCount = allRMAReceiptLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRMAReceiptLineDto>> Process_GetRMAReceiptLine(Guid rMAReceiptLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPRMAReceiptLineDto rMAReceiptLineDto = null;
		ERPResponseMessageDto<ERPRMAReceiptLineDto> result;
		try
		{
			IERPRMAReceiptLineRepository iERPRMAReceiptLineRepository = (base.ERPRMAReceiptLineRepository = new ERPRMAReceiptLineRepository(base.ApiClientContext));
			using (iERPRMAReceiptLineRepository)
			{
				ERPRMAReceiptLineInformationDto eRPRMAReceiptLineInformationDto = await base.ERPRMAReceiptLineRepository.GetRMAReceiptLine(rMAReceiptLineId);
				rMAReceiptLineDto = new ERPRMAReceiptLineDto
				{
					rrlConversionFactor = eRPRMAReceiptLineInformationDto.rrlConversionFactor,
					rrlCreatedBy = eRPRMAReceiptLineInformationDto.rrlCreatedBy,
					rrlCreatedDate = eRPRMAReceiptLineInformationDto.rrlCreatedDate,
					rrlDescription = eRPRMAReceiptLineInformationDto.rrlDescription,
					rrlUniqueID = eRPRMAReceiptLineInformationDto.rrlUniqueID,
					rrlExtendedCost = eRPRMAReceiptLineInformationDto.rrlExtendedCost,
					rrlExtendedCostForeign = eRPRMAReceiptLineInformationDto.rrlExtendedCostForeign,
					rrlHeatLot = eRPRMAReceiptLineInformationDto.rrlHeatLot,
					rrlInventoryQuantityReceived = eRPRMAReceiptLineInformationDto.rrlInventoryQuantityReceived,
					rrlInventoryUnitOfMeasure = eRPRMAReceiptLineInformationDto.rrlInventoryUnitOfMeasure,
					rrlClosed = eRPRMAReceiptLineInformationDto.rrlClosed,
					rrlInInspection = eRPRMAReceiptLineInformationDto.rrlInInspection,
					rrlInspectionComplete = eRPRMAReceiptLineInformationDto.rrlInspectionComplete,
					rrlInvoicedComplete = eRPRMAReceiptLineInformationDto.rrlInvoicedComplete,
					rrlKitPart = eRPRMAReceiptLineInformationDto.rrlKitPart,
					rrlPosted = eRPRMAReceiptLineInformationDto.rrlPosted,
					rrlReceivedComplete = eRPRMAReceiptLineInformationDto.rrlReceivedComplete,
					rrlRequiresInspection = eRPRMAReceiptLineInformationDto.rrlRequiresInspection,
					rrlReversed = eRPRMAReceiptLineInformationDto.rrlReversed,
					rrlOrgPartID = eRPRMAReceiptLineInformationDto.rrlOrgPartID,
					rrlOrgPartShortDescription = eRPRMAReceiptLineInformationDto.rrlOrgPartShortDescription,
					rrlPartBinID = eRPRMAReceiptLineInformationDto.rrlPartBinID,
					rrlPartID = eRPRMAReceiptLineInformationDto.rrlPartID,
					rrlPartLongDescriptionRtf = eRPRMAReceiptLineInformationDto.rrlPartLongDescriptionRtf,
					rrlPartLongDescriptionText = eRPRMAReceiptLineInformationDto.rrlPartLongDescriptionText,
					rrlPartRevisionID = eRPRMAReceiptLineInformationDto.rrlPartRevisionID,
					rrlPartWarehouseLocationID = eRPRMAReceiptLineInformationDto.rrlPartWarehouseLocationID,
					rrlProjectAreaID = eRPRMAReceiptLineInformationDto.rrlProjectAreaID,
					rrlProjectID = eRPRMAReceiptLineInformationDto.rrlProjectID,
					rrlQuantityToInspect = eRPRMAReceiptLineInformationDto.rrlQuantityToInspect,
					rrlReference = eRPRMAReceiptLineInformationDto.rrlReference,
					rrlReverseRmaReceiptID = eRPRMAReceiptLineInformationDto.rrlReverseRmaReceiptID,
					rrlReverseRmaReceiptLineID = eRPRMAReceiptLineInformationDto.rrlReverseRmaReceiptLineID,
					rrlRmaClaimID = eRPRMAReceiptLineInformationDto.rrlRmaClaimID,
					rrlRmaClaimLineID = eRPRMAReceiptLineInformationDto.rrlRmaClaimLineID,
					rrlRmaClaimQuantity = eRPRMAReceiptLineInformationDto.rrlRmaClaimQuantity,
					rrlRmaOpenQuantity = eRPRMAReceiptLineInformationDto.rrlRmaOpenQuantity,
					rrlRmaReceiptID = eRPRMAReceiptLineInformationDto.rrlRmaReceiptID,
					rrlRowVersion = eRPRMAReceiptLineInformationDto.rrlRowVersion,
					rrlSalesQuantityReceived = eRPRMAReceiptLineInformationDto.rrlSalesQuantityReceived,
					rrlSalesUnitOfMeasure = eRPRMAReceiptLineInformationDto.rrlSalesUnitOfMeasure,
					rrlRmaReceiptLineID = eRPRMAReceiptLineInformationDto.rrlRmaReceiptLineID,
					rrlTotalComponentCosts = eRPRMAReceiptLineInformationDto.rrlTotalComponentCosts,
					rrlUnitCost = eRPRMAReceiptLineInformationDto.rrlUnitCost,
					rrlUnitCostForeign = eRPRMAReceiptLineInformationDto.rrlUnitCostForeign,
					CustomFields = eRPRMAReceiptLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the RMAReceiptLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAReceiptLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = rMAReceiptLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRMAReceiptLineDto>> Process_PutRMAReceiptLine(ERPRMAReceiptLineDto rMAReceiptLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPRMAReceiptLineDto createdObject = null;
		ERPResponseMessageDto<ERPRMAReceiptLineDto> result;
		try
		{
			IERPRMAReceiptLineRepository iERPRMAReceiptLineRepository = (base.ERPRMAReceiptLineRepository = new ERPRMAReceiptLineRepository(base.ApiClientContext));
			using (iERPRMAReceiptLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPRMAReceiptLineRepository.SaveRMAReceiptLine(rMAReceiptLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPRMAReceiptLineInformationDto eRPRMAReceiptLineInformationDto = await base.ERPRMAReceiptLineRepository.GetRMAReceiptLine(rMAReceiptLine.rrlUniqueID);
					createdObject = new ERPRMAReceiptLineDto
					{
						rrlConversionFactor = eRPRMAReceiptLineInformationDto.rrlConversionFactor,
						rrlCreatedBy = eRPRMAReceiptLineInformationDto.rrlCreatedBy,
						rrlCreatedDate = eRPRMAReceiptLineInformationDto.rrlCreatedDate,
						rrlDescription = eRPRMAReceiptLineInformationDto.rrlDescription,
						rrlUniqueID = eRPRMAReceiptLineInformationDto.rrlUniqueID,
						rrlExtendedCost = eRPRMAReceiptLineInformationDto.rrlExtendedCost,
						rrlExtendedCostForeign = eRPRMAReceiptLineInformationDto.rrlExtendedCostForeign,
						rrlHeatLot = eRPRMAReceiptLineInformationDto.rrlHeatLot,
						rrlInventoryQuantityReceived = eRPRMAReceiptLineInformationDto.rrlInventoryQuantityReceived,
						rrlInventoryUnitOfMeasure = eRPRMAReceiptLineInformationDto.rrlInventoryUnitOfMeasure,
						rrlClosed = eRPRMAReceiptLineInformationDto.rrlClosed,
						rrlInInspection = eRPRMAReceiptLineInformationDto.rrlInInspection,
						rrlInspectionComplete = eRPRMAReceiptLineInformationDto.rrlInspectionComplete,
						rrlInvoicedComplete = eRPRMAReceiptLineInformationDto.rrlInvoicedComplete,
						rrlKitPart = eRPRMAReceiptLineInformationDto.rrlKitPart,
						rrlPosted = eRPRMAReceiptLineInformationDto.rrlPosted,
						rrlReceivedComplete = eRPRMAReceiptLineInformationDto.rrlReceivedComplete,
						rrlRequiresInspection = eRPRMAReceiptLineInformationDto.rrlRequiresInspection,
						rrlReversed = eRPRMAReceiptLineInformationDto.rrlReversed,
						rrlOrgPartID = eRPRMAReceiptLineInformationDto.rrlOrgPartID,
						rrlOrgPartShortDescription = eRPRMAReceiptLineInformationDto.rrlOrgPartShortDescription,
						rrlPartBinID = eRPRMAReceiptLineInformationDto.rrlPartBinID,
						rrlPartID = eRPRMAReceiptLineInformationDto.rrlPartID,
						rrlPartLongDescriptionRtf = eRPRMAReceiptLineInformationDto.rrlPartLongDescriptionRtf,
						rrlPartLongDescriptionText = eRPRMAReceiptLineInformationDto.rrlPartLongDescriptionText,
						rrlPartRevisionID = eRPRMAReceiptLineInformationDto.rrlPartRevisionID,
						rrlPartWarehouseLocationID = eRPRMAReceiptLineInformationDto.rrlPartWarehouseLocationID,
						rrlProjectAreaID = eRPRMAReceiptLineInformationDto.rrlProjectAreaID,
						rrlProjectID = eRPRMAReceiptLineInformationDto.rrlProjectID,
						rrlQuantityToInspect = eRPRMAReceiptLineInformationDto.rrlQuantityToInspect,
						rrlReference = eRPRMAReceiptLineInformationDto.rrlReference,
						rrlReverseRmaReceiptID = eRPRMAReceiptLineInformationDto.rrlReverseRmaReceiptID,
						rrlReverseRmaReceiptLineID = eRPRMAReceiptLineInformationDto.rrlReverseRmaReceiptLineID,
						rrlRmaClaimID = eRPRMAReceiptLineInformationDto.rrlRmaClaimID,
						rrlRmaClaimLineID = eRPRMAReceiptLineInformationDto.rrlRmaClaimLineID,
						rrlRmaClaimQuantity = eRPRMAReceiptLineInformationDto.rrlRmaClaimQuantity,
						rrlRmaOpenQuantity = eRPRMAReceiptLineInformationDto.rrlRmaOpenQuantity,
						rrlRmaReceiptID = eRPRMAReceiptLineInformationDto.rrlRmaReceiptID,
						rrlRowVersion = eRPRMAReceiptLineInformationDto.rrlRowVersion,
						rrlSalesQuantityReceived = eRPRMAReceiptLineInformationDto.rrlSalesQuantityReceived,
						rrlSalesUnitOfMeasure = eRPRMAReceiptLineInformationDto.rrlSalesUnitOfMeasure,
						rrlRmaReceiptLineID = eRPRMAReceiptLineInformationDto.rrlRmaReceiptLineID,
						rrlTotalComponentCosts = eRPRMAReceiptLineInformationDto.rrlTotalComponentCosts,
						rrlUnitCost = eRPRMAReceiptLineInformationDto.rrlUnitCost,
						rrlUnitCostForeign = eRPRMAReceiptLineInformationDto.rrlUnitCostForeign,
						CustomFields = eRPRMAReceiptLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing RMAReceiptLine [{rMAReceiptLine.rrlUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAReceiptLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteRMAReceiptLine(Guid rMAReceiptLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAReceiptLineRepository iERPRMAReceiptLineRepository = (base.ERPRMAReceiptLineRepository = new ERPRMAReceiptLineRepository(base.ApiClientContext));
		using (iERPRMAReceiptLineRepository)
		{
			if (!(await base.ERPRMAReceiptLineRepository.DoesRMAReceiptLineExist(rMAReceiptLineId)))
			{
				base.ErrorsList.Add($"RMAReceiptLine [{rMAReceiptLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPRMAReceiptLineInformationDto eRPRMAReceiptLineInformationDto = await base.ERPRMAReceiptLineRepository.GetRMAReceiptLine(rMAReceiptLineId);
				string text = await base.ERPRMAReceiptLineRepository.WhereUsed("RMAReceiptLines", new object[2] { eRPRMAReceiptLineInformationDto.rrlRmaReceiptID, eRPRMAReceiptLineInformationDto.rrlRmaReceiptLineID }, new object[2] { "rrlRmaReceiptID", "rrlRmaReceiptLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("RMAReceiptLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPRMAReceiptLineDto>> Process_DeleteRMAReceiptLine(Guid rMAReceiptLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPRMAReceiptLineDto> result;
		try
		{
			IERPRMAReceiptLineRepository iERPRMAReceiptLineRepository = (base.ERPRMAReceiptLineRepository = new ERPRMAReceiptLineRepository(base.ApiClientContext));
			using (iERPRMAReceiptLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPRMAReceiptLineRepository.DeleteRowFromTable("RMAReceiptLines", "rrl", rMAReceiptLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of RMAReceiptLine [{rMAReceiptLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAReceiptLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPRMAReceiptLineDto()
			};
		}
		return result;
	}
}
