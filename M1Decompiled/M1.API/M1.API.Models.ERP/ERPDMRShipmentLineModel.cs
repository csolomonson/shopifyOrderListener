using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPDMRShipmentLineModel : ERPBaseModel, IERPDMRShipmentLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllDMRShipmentLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPDMRShipmentLineRepository iERPDMRShipmentLineRepository = (base.ERPDMRShipmentLineRepository = new ERPDMRShipmentLineRepository(base.ApiClientContext));
		using (iERPDMRShipmentLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPDMRShipmentLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPDMRShipmentLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPDMRShipmentLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPDMRShipmentLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetDMRShipmentLine(Guid dMRShipmentLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRShipmentLineRepository iERPDMRShipmentLineRepository = (base.ERPDMRShipmentLineRepository = new ERPDMRShipmentLineRepository(base.ApiClientContext));
		using (iERPDMRShipmentLineRepository)
		{
			if (!(await base.ERPDMRShipmentLineRepository.DoesDMRShipmentLineExist(dMRShipmentLineId)))
			{
				errorsList.Add($"DMRShipmentLine [{dMRShipmentLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutDMRShipmentLine(ERPDMRShipmentLineDto dMRShipmentLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRShipmentLineRepository iERPDMRShipmentLineRepository = (base.ERPDMRShipmentLineRepository = new ERPDMRShipmentLineRepository(base.ApiClientContext));
		using (iERPDMRShipmentLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(dMRShipmentLine.dslDmrShipmentID) && !(await base.ERPDMRShipmentLineRepository.DoesRecordExistInTableUsingKeys("DMRShipments", new object[1] { "DSPDMRSHIPMENTID" }, new object[1] { dMRShipmentLine.dslDmrShipmentID })))
			{
				errorsList.Add("dslDmrShipmentID [" + dMRShipmentLine.dslDmrShipmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentLine.dslPartID) && !(await base.ERPDMRShipmentLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { dMRShipmentLine.dslPartID })))
			{
				errorsList.Add("dslPartID [" + dMRShipmentLine.dslPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentLine.dslPartRevisionID) && !(await base.ERPDMRShipmentLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { dMRShipmentLine.dslPartID, dMRShipmentLine.dslPartRevisionID })))
			{
				errorsList.Add("dslPartRevisionID [" + dMRShipmentLine.dslPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentLine.dslPartWarehouseLocationID) && !(await base.ERPDMRShipmentLineRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { dMRShipmentLine.dslPartID, dMRShipmentLine.dslPartRevisionID, dMRShipmentLine.dslPartWarehouseLocationID })))
			{
				errorsList.Add("dslPartWarehouseLocationID [" + dMRShipmentLine.dslPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentLine.dslPartBinID) && !(await base.ERPDMRShipmentLineRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { dMRShipmentLine.dslPartID, dMRShipmentLine.dslPartRevisionID, dMRShipmentLine.dslPartWarehouseLocationID, dMRShipmentLine.dslPartBinID })))
			{
				errorsList.Add("dslPartBinID [" + dMRShipmentLine.dslPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentLine.dslProjectID) && !(await base.ERPDMRShipmentLineRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { dMRShipmentLine.dslProjectID })))
			{
				errorsList.Add("dslProjectID [" + dMRShipmentLine.dslProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentLine.dslProjectAreaID) && !(await base.ERPDMRShipmentLineRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { dMRShipmentLine.dslProjectID, dMRShipmentLine.dslProjectAreaID })))
			{
				errorsList.Add("dslProjectAreaID [" + dMRShipmentLine.dslProjectAreaID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentLine.dslDmrClaimID) && !(await base.ERPDMRShipmentLineRepository.DoesRecordExistInTableUsingKeys("DMRClaims", new object[1] { "DMPDMRCLAIMID" }, new object[1] { dMRShipmentLine.dslDmrClaimID })))
			{
				errorsList.Add("dslDmrClaimID [" + dMRShipmentLine.dslDmrClaimID + "] not found.");
			}
			if (dMRShipmentLine.dslDmrClaimLineID > 0 && !(await base.ERPDMRShipmentLineRepository.DoesRecordExistInTableUsingKeys("DMRClaimLines", new object[2] { "DMLDMRCLAIMID", "DMLDMRCLAIMLINEID" }, new object[2] { dMRShipmentLine.dslDmrClaimID, dMRShipmentLine.dslDmrClaimLineID })))
			{
				errorsList.Add($"dslDmrClaimLineID [{dMRShipmentLine.dslDmrClaimLineID}] not found.");
			}
			if (dMRShipmentLine.dslJobAssemblyID > 0 && !(await base.ERPDMRShipmentLineRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { dMRShipmentLine.dslJobID, dMRShipmentLine.dslJobAssemblyID })))
			{
				errorsList.Add($"dslJobAssemblyID [{dMRShipmentLine.dslJobAssemblyID}] not found.");
			}
			if (dMRShipmentLine.dslJobMaterialID > 0 && !(await base.ERPDMRShipmentLineRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { dMRShipmentLine.dslJobID, dMRShipmentLine.dslJobAssemblyID, dMRShipmentLine.dslJobMaterialID })))
			{
				errorsList.Add($"dslJobMaterialID [{dMRShipmentLine.dslJobMaterialID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentLine.dslJobID) && !(await base.ERPDMRShipmentLineRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { dMRShipmentLine.dslJobID })))
			{
				errorsList.Add("dslJobID [" + dMRShipmentLine.dslJobID + "] not found.");
			}
			if (dMRShipmentLine.dslJobOperationID > 0 && !(await base.ERPDMRShipmentLineRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { dMRShipmentLine.dslJobID, dMRShipmentLine.dslJobAssemblyID, dMRShipmentLine.dslJobOperationID })))
			{
				errorsList.Add($"dslJobOperationID [{dMRShipmentLine.dslJobOperationID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentLine.dslReverseDmrShipmentID) && !(await base.ERPDMRShipmentLineRepository.DoesRecordExistInTableUsingKeys("DMRShipments", new object[1] { "DSPDMRSHIPMENTID" }, new object[1] { dMRShipmentLine.dslReverseDmrShipmentID })))
			{
				errorsList.Add("dslReverseDmrShipmentID [" + dMRShipmentLine.dslReverseDmrShipmentID + "] not found.");
			}
			if (dMRShipmentLine.dslReverseDmrShipmentLineID > 0 && !(await base.ERPDMRShipmentLineRepository.DoesRecordExistInTableUsingKeys("DMRShipmentLines", new object[2] { "DSLDMRSHIPMENTID", "DSLDMRSHIPMENTLINEID" }, new object[2] { dMRShipmentLine.dslReverseDmrShipmentID, dMRShipmentLine.dslReverseDmrShipmentLineID })))
			{
				errorsList.Add($"dslReverseDmrShipmentLineID [{dMRShipmentLine.dslReverseDmrShipmentLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentLine.dslInspectionID) && !(await base.ERPDMRShipmentLineRepository.DoesRecordExistInTableUsingKeys("Inspections", new object[1] { "QAPINSPECTIONID" }, new object[1] { dMRShipmentLine.dslInspectionID })))
			{
				errorsList.Add("dslInspectionID [" + dMRShipmentLine.dslInspectionID + "] not found.");
			}
			if (dMRShipmentLine.dslInspectionLineID > 0 && !(await base.ERPDMRShipmentLineRepository.DoesRecordExistInTableUsingKeys("InspectionLines", new object[2] { "QALINSPECTIONID", "QALINSPECTIONLINEID" }, new object[2] { dMRShipmentLine.dslInspectionID, dMRShipmentLine.dslInspectionLineID })))
			{
				errorsList.Add($"dslInspectionLineID [{dMRShipmentLine.dslInspectionLineID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPDMRShipmentLineDto>>> Process_GetAllDMRShipmentLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPDMRShipmentLineDto> allDMRShipmentLinesDto = new List<ERPDMRShipmentLineDto>();
		ERPResponseMessageDto<IList<ERPDMRShipmentLineDto>> result;
		try
		{
			IERPDMRShipmentLineRepository iERPDMRShipmentLineRepository = (base.ERPDMRShipmentLineRepository = new ERPDMRShipmentLineRepository(base.ApiClientContext));
			using (iERPDMRShipmentLineRepository)
			{
				foreach (ERPDMRShipmentLineInformationDto item2 in await base.ERPDMRShipmentLineRepository.GetAllDMRShipmentLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPDMRShipmentLineDto item = new ERPDMRShipmentLineDto
					{
						dslConversionFactor = item2.dslConversionFactor,
						dslCreatedBy = item2.dslCreatedBy,
						dslCreatedDate = item2.dslCreatedDate,
						dslDescription = item2.dslDescription,
						dslDmrClaimID = item2.dslDmrClaimID,
						dslDmrClaimLineID = item2.dslDmrClaimLineID,
						dslDmrClaimQuantity = item2.dslDmrClaimQuantity,
						dslDmrOpenQuantity = item2.dslDmrOpenQuantity,
						dslDmrShipmentID = item2.dslDmrShipmentID,
						dslUniqueID = item2.dslUniqueID,
						dslInspectionID = item2.dslInspectionID,
						dslInspectionLineID = item2.dslInspectionLineID,
						dslInventoryQuantityShipped = item2.dslInventoryQuantityShipped,
						dslInventoryUnitOfMeasure = item2.dslInventoryUnitOfMeasure,
						dslClosed = item2.dslClosed,
						dslInvoicedComplete = item2.dslInvoicedComplete,
						dslKitPart = item2.dslKitPart,
						dslPosted = item2.dslPosted,
						dslReversed = item2.dslReversed,
						dslShippedComplete = item2.dslShippedComplete,
						dslJobAssemblyID = item2.dslJobAssemblyID,
						dslJobID = item2.dslJobID,
						dslJobMaterialID = item2.dslJobMaterialID,
						dslJobMatQuantityShipped = item2.dslJobMatQuantityShipped,
						dslJobOperationID = item2.dslJobOperationID,
						dslJobOprQuantityShipped = item2.dslJobOprQuantityShipped,
						dslPartBinID = item2.dslPartBinID,
						dslPartID = item2.dslPartID,
						dslPartLongDescriptionRtf = item2.dslPartLongDescriptionRtf,
						dslPartLongDescriptionText = item2.dslPartLongDescriptionText,
						dslPartRevisionID = item2.dslPartRevisionID,
						dslPartWarehouseLocationID = item2.dslPartWarehouseLocationID,
						dslProjectAreaID = item2.dslProjectAreaID,
						dslProjectID = item2.dslProjectID,
						dslQuantityShipped = item2.dslQuantityShipped,
						dslReturnQuantityShipped = item2.dslReturnQuantityShipped,
						dslReverseDmrShipmentID = item2.dslReverseDmrShipmentID,
						dslReverseDmrShipmentLineID = item2.dslReverseDmrShipmentLineID,
						dslRowVersion = item2.dslRowVersion,
						dslDmrShipmentLineID = item2.dslDmrShipmentLineID,
						dslUnitOfMeasure = item2.dslUnitOfMeasure,
						dslUnitPrice = item2.dslUnitPrice,
						dslUnitPriceForeign = item2.dslUnitPriceForeign,
						CustomFields = item2.CustomFields
					};
					allDMRShipmentLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all DMRShipmentLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPDMRShipmentLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allDMRShipmentLinesDto,
				RecordCount = allDMRShipmentLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPDMRShipmentLineDto>> Process_GetDMRShipmentLine(Guid dMRShipmentLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPDMRShipmentLineDto dMRShipmentLineDto = null;
		ERPResponseMessageDto<ERPDMRShipmentLineDto> result;
		try
		{
			IERPDMRShipmentLineRepository iERPDMRShipmentLineRepository = (base.ERPDMRShipmentLineRepository = new ERPDMRShipmentLineRepository(base.ApiClientContext));
			using (iERPDMRShipmentLineRepository)
			{
				ERPDMRShipmentLineInformationDto eRPDMRShipmentLineInformationDto = await base.ERPDMRShipmentLineRepository.GetDMRShipmentLine(dMRShipmentLineId);
				dMRShipmentLineDto = new ERPDMRShipmentLineDto
				{
					dslConversionFactor = eRPDMRShipmentLineInformationDto.dslConversionFactor,
					dslCreatedBy = eRPDMRShipmentLineInformationDto.dslCreatedBy,
					dslCreatedDate = eRPDMRShipmentLineInformationDto.dslCreatedDate,
					dslDescription = eRPDMRShipmentLineInformationDto.dslDescription,
					dslDmrClaimID = eRPDMRShipmentLineInformationDto.dslDmrClaimID,
					dslDmrClaimLineID = eRPDMRShipmentLineInformationDto.dslDmrClaimLineID,
					dslDmrClaimQuantity = eRPDMRShipmentLineInformationDto.dslDmrClaimQuantity,
					dslDmrOpenQuantity = eRPDMRShipmentLineInformationDto.dslDmrOpenQuantity,
					dslDmrShipmentID = eRPDMRShipmentLineInformationDto.dslDmrShipmentID,
					dslUniqueID = eRPDMRShipmentLineInformationDto.dslUniqueID,
					dslInspectionID = eRPDMRShipmentLineInformationDto.dslInspectionID,
					dslInspectionLineID = eRPDMRShipmentLineInformationDto.dslInspectionLineID,
					dslInventoryQuantityShipped = eRPDMRShipmentLineInformationDto.dslInventoryQuantityShipped,
					dslInventoryUnitOfMeasure = eRPDMRShipmentLineInformationDto.dslInventoryUnitOfMeasure,
					dslClosed = eRPDMRShipmentLineInformationDto.dslClosed,
					dslInvoicedComplete = eRPDMRShipmentLineInformationDto.dslInvoicedComplete,
					dslKitPart = eRPDMRShipmentLineInformationDto.dslKitPart,
					dslPosted = eRPDMRShipmentLineInformationDto.dslPosted,
					dslReversed = eRPDMRShipmentLineInformationDto.dslReversed,
					dslShippedComplete = eRPDMRShipmentLineInformationDto.dslShippedComplete,
					dslJobAssemblyID = eRPDMRShipmentLineInformationDto.dslJobAssemblyID,
					dslJobID = eRPDMRShipmentLineInformationDto.dslJobID,
					dslJobMaterialID = eRPDMRShipmentLineInformationDto.dslJobMaterialID,
					dslJobMatQuantityShipped = eRPDMRShipmentLineInformationDto.dslJobMatQuantityShipped,
					dslJobOperationID = eRPDMRShipmentLineInformationDto.dslJobOperationID,
					dslJobOprQuantityShipped = eRPDMRShipmentLineInformationDto.dslJobOprQuantityShipped,
					dslPartBinID = eRPDMRShipmentLineInformationDto.dslPartBinID,
					dslPartID = eRPDMRShipmentLineInformationDto.dslPartID,
					dslPartLongDescriptionRtf = eRPDMRShipmentLineInformationDto.dslPartLongDescriptionRtf,
					dslPartLongDescriptionText = eRPDMRShipmentLineInformationDto.dslPartLongDescriptionText,
					dslPartRevisionID = eRPDMRShipmentLineInformationDto.dslPartRevisionID,
					dslPartWarehouseLocationID = eRPDMRShipmentLineInformationDto.dslPartWarehouseLocationID,
					dslProjectAreaID = eRPDMRShipmentLineInformationDto.dslProjectAreaID,
					dslProjectID = eRPDMRShipmentLineInformationDto.dslProjectID,
					dslQuantityShipped = eRPDMRShipmentLineInformationDto.dslQuantityShipped,
					dslReturnQuantityShipped = eRPDMRShipmentLineInformationDto.dslReturnQuantityShipped,
					dslReverseDmrShipmentID = eRPDMRShipmentLineInformationDto.dslReverseDmrShipmentID,
					dslReverseDmrShipmentLineID = eRPDMRShipmentLineInformationDto.dslReverseDmrShipmentLineID,
					dslRowVersion = eRPDMRShipmentLineInformationDto.dslRowVersion,
					dslDmrShipmentLineID = eRPDMRShipmentLineInformationDto.dslDmrShipmentLineID,
					dslUnitOfMeasure = eRPDMRShipmentLineInformationDto.dslUnitOfMeasure,
					dslUnitPrice = eRPDMRShipmentLineInformationDto.dslUnitPrice,
					dslUnitPriceForeign = eRPDMRShipmentLineInformationDto.dslUnitPriceForeign,
					CustomFields = eRPDMRShipmentLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the DMRShipmentLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRShipmentLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = dMRShipmentLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPDMRShipmentLineDto>> Process_PutDMRShipmentLine(ERPDMRShipmentLineDto dMRShipmentLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPDMRShipmentLineDto createdObject = null;
		ERPResponseMessageDto<ERPDMRShipmentLineDto> result;
		try
		{
			IERPDMRShipmentLineRepository iERPDMRShipmentLineRepository = (base.ERPDMRShipmentLineRepository = new ERPDMRShipmentLineRepository(base.ApiClientContext));
			using (iERPDMRShipmentLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPDMRShipmentLineRepository.SaveDMRShipmentLine(dMRShipmentLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPDMRShipmentLineInformationDto eRPDMRShipmentLineInformationDto = await base.ERPDMRShipmentLineRepository.GetDMRShipmentLine(dMRShipmentLine.dslUniqueID);
					createdObject = new ERPDMRShipmentLineDto
					{
						dslConversionFactor = eRPDMRShipmentLineInformationDto.dslConversionFactor,
						dslCreatedBy = eRPDMRShipmentLineInformationDto.dslCreatedBy,
						dslCreatedDate = eRPDMRShipmentLineInformationDto.dslCreatedDate,
						dslDescription = eRPDMRShipmentLineInformationDto.dslDescription,
						dslDmrClaimID = eRPDMRShipmentLineInformationDto.dslDmrClaimID,
						dslDmrClaimLineID = eRPDMRShipmentLineInformationDto.dslDmrClaimLineID,
						dslDmrClaimQuantity = eRPDMRShipmentLineInformationDto.dslDmrClaimQuantity,
						dslDmrOpenQuantity = eRPDMRShipmentLineInformationDto.dslDmrOpenQuantity,
						dslDmrShipmentID = eRPDMRShipmentLineInformationDto.dslDmrShipmentID,
						dslUniqueID = eRPDMRShipmentLineInformationDto.dslUniqueID,
						dslInspectionID = eRPDMRShipmentLineInformationDto.dslInspectionID,
						dslInspectionLineID = eRPDMRShipmentLineInformationDto.dslInspectionLineID,
						dslInventoryQuantityShipped = eRPDMRShipmentLineInformationDto.dslInventoryQuantityShipped,
						dslInventoryUnitOfMeasure = eRPDMRShipmentLineInformationDto.dslInventoryUnitOfMeasure,
						dslClosed = eRPDMRShipmentLineInformationDto.dslClosed,
						dslInvoicedComplete = eRPDMRShipmentLineInformationDto.dslInvoicedComplete,
						dslKitPart = eRPDMRShipmentLineInformationDto.dslKitPart,
						dslPosted = eRPDMRShipmentLineInformationDto.dslPosted,
						dslReversed = eRPDMRShipmentLineInformationDto.dslReversed,
						dslShippedComplete = eRPDMRShipmentLineInformationDto.dslShippedComplete,
						dslJobAssemblyID = eRPDMRShipmentLineInformationDto.dslJobAssemblyID,
						dslJobID = eRPDMRShipmentLineInformationDto.dslJobID,
						dslJobMaterialID = eRPDMRShipmentLineInformationDto.dslJobMaterialID,
						dslJobMatQuantityShipped = eRPDMRShipmentLineInformationDto.dslJobMatQuantityShipped,
						dslJobOperationID = eRPDMRShipmentLineInformationDto.dslJobOperationID,
						dslJobOprQuantityShipped = eRPDMRShipmentLineInformationDto.dslJobOprQuantityShipped,
						dslPartBinID = eRPDMRShipmentLineInformationDto.dslPartBinID,
						dslPartID = eRPDMRShipmentLineInformationDto.dslPartID,
						dslPartLongDescriptionRtf = eRPDMRShipmentLineInformationDto.dslPartLongDescriptionRtf,
						dslPartLongDescriptionText = eRPDMRShipmentLineInformationDto.dslPartLongDescriptionText,
						dslPartRevisionID = eRPDMRShipmentLineInformationDto.dslPartRevisionID,
						dslPartWarehouseLocationID = eRPDMRShipmentLineInformationDto.dslPartWarehouseLocationID,
						dslProjectAreaID = eRPDMRShipmentLineInformationDto.dslProjectAreaID,
						dslProjectID = eRPDMRShipmentLineInformationDto.dslProjectID,
						dslQuantityShipped = eRPDMRShipmentLineInformationDto.dslQuantityShipped,
						dslReturnQuantityShipped = eRPDMRShipmentLineInformationDto.dslReturnQuantityShipped,
						dslReverseDmrShipmentID = eRPDMRShipmentLineInformationDto.dslReverseDmrShipmentID,
						dslReverseDmrShipmentLineID = eRPDMRShipmentLineInformationDto.dslReverseDmrShipmentLineID,
						dslRowVersion = eRPDMRShipmentLineInformationDto.dslRowVersion,
						dslDmrShipmentLineID = eRPDMRShipmentLineInformationDto.dslDmrShipmentLineID,
						dslUnitOfMeasure = eRPDMRShipmentLineInformationDto.dslUnitOfMeasure,
						dslUnitPrice = eRPDMRShipmentLineInformationDto.dslUnitPrice,
						dslUnitPriceForeign = eRPDMRShipmentLineInformationDto.dslUnitPriceForeign,
						CustomFields = eRPDMRShipmentLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing DMRShipmentLine [{dMRShipmentLine.dslUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRShipmentLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteDMRShipmentLine(Guid dMRShipmentLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRShipmentLineRepository iERPDMRShipmentLineRepository = (base.ERPDMRShipmentLineRepository = new ERPDMRShipmentLineRepository(base.ApiClientContext));
		using (iERPDMRShipmentLineRepository)
		{
			if (!(await base.ERPDMRShipmentLineRepository.DoesDMRShipmentLineExist(dMRShipmentLineId)))
			{
				base.ErrorsList.Add($"DMRShipmentLine [{dMRShipmentLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPDMRShipmentLineInformationDto eRPDMRShipmentLineInformationDto = await base.ERPDMRShipmentLineRepository.GetDMRShipmentLine(dMRShipmentLineId);
				string text = await base.ERPDMRShipmentLineRepository.WhereUsed("DMRShipmentLines", new object[2] { eRPDMRShipmentLineInformationDto.dslDmrShipmentID, eRPDMRShipmentLineInformationDto.dslDmrShipmentLineID }, new object[2] { "dslDmrShipmentID", "dslDmrShipmentLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("DMRShipmentLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPDMRShipmentLineDto>> Process_DeleteDMRShipmentLine(Guid dMRShipmentLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPDMRShipmentLineDto> result;
		try
		{
			IERPDMRShipmentLineRepository iERPDMRShipmentLineRepository = (base.ERPDMRShipmentLineRepository = new ERPDMRShipmentLineRepository(base.ApiClientContext));
			using (iERPDMRShipmentLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPDMRShipmentLineRepository.DeleteRowFromTable("DMRShipmentLines", "dsl", dMRShipmentLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of DMRShipmentLine [{dMRShipmentLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRShipmentLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPDMRShipmentLineDto()
			};
		}
		return result;
	}
}
