using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPLandedCostModel : ERPBaseModel, IERPLandedCostModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllLandedCosts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPLandedCostRepository iERPLandedCostRepository = (base.ERPLandedCostRepository = new ERPLandedCostRepository(base.ApiClientContext));
		using (iERPLandedCostRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPLandedCostRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPLandedCostRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPLandedCostRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPLandedCostRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetLandedCost(Guid landedCostId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLandedCostRepository iERPLandedCostRepository = (base.ERPLandedCostRepository = new ERPLandedCostRepository(base.ApiClientContext));
		using (iERPLandedCostRepository)
		{
			if (!(await base.ERPLandedCostRepository.DoesLandedCostExist(landedCostId)))
			{
				errorsList.Add($"LandedCost [{landedCostId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutLandedCost(ERPLandedCostDto landedCost)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLandedCostRepository iERPLandedCostRepository = (base.ERPLandedCostRepository = new ERPLandedCostRepository(base.ApiClientContext));
		using (iERPLandedCostRepository)
		{
			if (!string.IsNullOrWhiteSpace(landedCost.rmcShipOrganizationID) && !(await base.ERPLandedCostRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { landedCost.rmcShipOrganizationID })))
			{
				errorsList.Add("rmcShipOrganizationID [" + landedCost.rmcShipOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(landedCost.rmcShipLocationID) && !(await base.ERPLandedCostRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { landedCost.rmcShipOrganizationID, landedCost.rmcShipLocationID })))
			{
				errorsList.Add("rmcShipLocationID [" + landedCost.rmcShipLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(landedCost.rmcShipContactID) && !(await base.ERPLandedCostRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { landedCost.rmcShipOrganizationID, landedCost.rmcShipLocationID, landedCost.rmcShipContactID })))
			{
				errorsList.Add("rmcShipContactID [" + landedCost.rmcShipContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(landedCost.rmcConsigneeOrganizationID) && !(await base.ERPLandedCostRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { landedCost.rmcConsigneeOrganizationID })))
			{
				errorsList.Add("rmcConsigneeOrganizationID [" + landedCost.rmcConsigneeOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(landedCost.rmcConsigneeLocationID) && !(await base.ERPLandedCostRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { landedCost.rmcConsigneeOrganizationID, landedCost.rmcConsigneeLocationID })))
			{
				errorsList.Add("rmcConsigneeLocationID [" + landedCost.rmcConsigneeLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(landedCost.rmcConsigneeContactID) && !(await base.ERPLandedCostRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { landedCost.rmcConsigneeOrganizationID, landedCost.rmcConsigneeLocationID, landedCost.rmcConsigneeContactID })))
			{
				errorsList.Add("rmcConsigneeContactID [" + landedCost.rmcConsigneeContactID + "] not found.");
			}
			if (landedCost.rmcGlFiscalYearID > 0 && !(await base.ERPLandedCostRepository.DoesRecordExistInTableUsingKeys("GLFiscalYears", new object[1] { "GLZGLFISCALYEARID" }, new object[1] { landedCost.rmcGlFiscalYearID })))
			{
				errorsList.Add($"rmcGlFiscalYearID [{landedCost.rmcGlFiscalYearID}] not found.");
			}
			if (landedCost.rmcGlFiscalYearPeriodID > 0 && !(await base.ERPLandedCostRepository.DoesRecordExistInTableUsingKeys("GLFiscalYearPeriods", new object[2] { "GLFGLFISCALYEARID", "GLFGLFISCALYEARPERIODID" }, new object[2] { landedCost.rmcGlFiscalYearID, landedCost.rmcGlFiscalYearPeriodID })))
			{
				errorsList.Add($"rmcGlFiscalYearPeriodID [{landedCost.rmcGlFiscalYearPeriodID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(landedCost.rmcReverseLandedCostID) && !(await base.ERPLandedCostRepository.DoesRecordExistInTableUsingKeys("LandedCosts", new object[1] { "RMCLANDEDCOSTID" }, new object[1] { landedCost.rmcReverseLandedCostID })))
			{
				errorsList.Add("rmcReverseLandedCostID [" + landedCost.rmcReverseLandedCostID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(landedCost.rmcPlantDepartmentID) && !(await base.ERPLandedCostRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { landedCost.rmcPlantID, landedCost.rmcPlantDepartmentID })))
			{
				errorsList.Add("rmcPlantDepartmentID [" + landedCost.rmcPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(landedCost.rmcPlantID) && !(await base.ERPLandedCostRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { landedCost.rmcPlantID })))
			{
				errorsList.Add("rmcPlantID [" + landedCost.rmcPlantID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPLandedCostDto>>> Process_GetAllLandedCosts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPLandedCostDto> allLandedCostsDto = new List<ERPLandedCostDto>();
		ERPResponseMessageDto<IList<ERPLandedCostDto>> result;
		try
		{
			IERPLandedCostRepository iERPLandedCostRepository = (base.ERPLandedCostRepository = new ERPLandedCostRepository(base.ApiClientContext));
			using (iERPLandedCostRepository)
			{
				foreach (ERPLandedCostInformationDto item2 in await base.ERPLandedCostRepository.GetAllLandedCosts(pageSize, pageNumber, filter, orderBy))
				{
					ERPLandedCostDto item = new ERPLandedCostDto
					{
						rmcCarrierName = item2.rmcCarrierName,
						rmcClosedDate = item2.rmcClosedDate,
						rmcLandedCostID = item2.rmcLandedCostID,
						rmcConsigneeContactID = item2.rmcConsigneeContactID,
						rmcConsigneeLocationID = item2.rmcConsigneeLocationID,
						rmcConsigneeOrganizationID = item2.rmcConsigneeOrganizationID,
						rmcCreatedBy = item2.rmcCreatedBy,
						rmcCreatedDate = item2.rmcCreatedDate,
						rmcDischargePoint = item2.rmcDischargePoint,
						rmcUniqueID = item2.rmcUniqueID,
						rmcGlFiscalYearID = item2.rmcGlFiscalYearID,
						rmcGlFiscalYearPeriodID = item2.rmcGlFiscalYearPeriodID,
						rmcChargesComplete = item2.rmcChargesComplete,
						rmcChargesJournalsCreated = item2.rmcChargesJournalsCreated,
						rmcClosed = item2.rmcClosed,
						rmcPoInTransitComplete = item2.rmcPoInTransitComplete,
						rmcPoInTransitJournalsCreated = item2.rmcPoInTransitJournalsCreated,
						rmcPostedToGl = item2.rmcPostedToGl,
						rmcReversalEntry = item2.rmcReversalEntry,
						rmcReversed = item2.rmcReversed,
						rmcLandedCostChargesTotal = item2.rmcLandedCostChargesTotal,
						rmcLandedCostDate = item2.rmcLandedCostDate,
						rmcLandedCostPurchasesTotal = item2.rmcLandedCostPurchasesTotal,
						rmcLandedCostReceiptsTotal = item2.rmcLandedCostReceiptsTotal,
						rmcLandedCostTotal = item2.rmcLandedCostTotal,
						rmcLoadingPoint = item2.rmcLoadingPoint,
						rmcLongDescriptionRtf = item2.rmcLongDescriptionRtf,
						rmcLongDescriptionText = item2.rmcLongDescriptionText,
						rmcPlantDepartmentID = item2.rmcPlantDepartmentID,
						rmcPlantID = item2.rmcPlantID,
						rmcPostedDate = item2.rmcPostedDate,
						rmcReverseLandedCostID = item2.rmcReverseLandedCostID,
						rmcRowVersion = item2.rmcRowVersion,
						rmcShipContactID = item2.rmcShipContactID,
						rmcShipLocationID = item2.rmcShipLocationID,
						rmcShipOrganizationID = item2.rmcShipOrganizationID,
						rmcTrackingNumber = item2.rmcTrackingNumber,
						CustomFields = item2.CustomFields
					};
					allLandedCostsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all LandedCosts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPLandedCostDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allLandedCostsDto,
				RecordCount = allLandedCostsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLandedCostDto>> Process_GetLandedCost(Guid landedCostId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPLandedCostDto landedCostDto = null;
		ERPResponseMessageDto<ERPLandedCostDto> result;
		try
		{
			IERPLandedCostRepository iERPLandedCostRepository = (base.ERPLandedCostRepository = new ERPLandedCostRepository(base.ApiClientContext));
			using (iERPLandedCostRepository)
			{
				ERPLandedCostInformationDto eRPLandedCostInformationDto = await base.ERPLandedCostRepository.GetLandedCost(landedCostId);
				landedCostDto = new ERPLandedCostDto
				{
					rmcCarrierName = eRPLandedCostInformationDto.rmcCarrierName,
					rmcClosedDate = eRPLandedCostInformationDto.rmcClosedDate,
					rmcLandedCostID = eRPLandedCostInformationDto.rmcLandedCostID,
					rmcConsigneeContactID = eRPLandedCostInformationDto.rmcConsigneeContactID,
					rmcConsigneeLocationID = eRPLandedCostInformationDto.rmcConsigneeLocationID,
					rmcConsigneeOrganizationID = eRPLandedCostInformationDto.rmcConsigneeOrganizationID,
					rmcCreatedBy = eRPLandedCostInformationDto.rmcCreatedBy,
					rmcCreatedDate = eRPLandedCostInformationDto.rmcCreatedDate,
					rmcDischargePoint = eRPLandedCostInformationDto.rmcDischargePoint,
					rmcUniqueID = eRPLandedCostInformationDto.rmcUniqueID,
					rmcGlFiscalYearID = eRPLandedCostInformationDto.rmcGlFiscalYearID,
					rmcGlFiscalYearPeriodID = eRPLandedCostInformationDto.rmcGlFiscalYearPeriodID,
					rmcChargesComplete = eRPLandedCostInformationDto.rmcChargesComplete,
					rmcChargesJournalsCreated = eRPLandedCostInformationDto.rmcChargesJournalsCreated,
					rmcClosed = eRPLandedCostInformationDto.rmcClosed,
					rmcPoInTransitComplete = eRPLandedCostInformationDto.rmcPoInTransitComplete,
					rmcPoInTransitJournalsCreated = eRPLandedCostInformationDto.rmcPoInTransitJournalsCreated,
					rmcPostedToGl = eRPLandedCostInformationDto.rmcPostedToGl,
					rmcReversalEntry = eRPLandedCostInformationDto.rmcReversalEntry,
					rmcReversed = eRPLandedCostInformationDto.rmcReversed,
					rmcLandedCostChargesTotal = eRPLandedCostInformationDto.rmcLandedCostChargesTotal,
					rmcLandedCostDate = eRPLandedCostInformationDto.rmcLandedCostDate,
					rmcLandedCostPurchasesTotal = eRPLandedCostInformationDto.rmcLandedCostPurchasesTotal,
					rmcLandedCostReceiptsTotal = eRPLandedCostInformationDto.rmcLandedCostReceiptsTotal,
					rmcLandedCostTotal = eRPLandedCostInformationDto.rmcLandedCostTotal,
					rmcLoadingPoint = eRPLandedCostInformationDto.rmcLoadingPoint,
					rmcLongDescriptionRtf = eRPLandedCostInformationDto.rmcLongDescriptionRtf,
					rmcLongDescriptionText = eRPLandedCostInformationDto.rmcLongDescriptionText,
					rmcPlantDepartmentID = eRPLandedCostInformationDto.rmcPlantDepartmentID,
					rmcPlantID = eRPLandedCostInformationDto.rmcPlantID,
					rmcPostedDate = eRPLandedCostInformationDto.rmcPostedDate,
					rmcReverseLandedCostID = eRPLandedCostInformationDto.rmcReverseLandedCostID,
					rmcRowVersion = eRPLandedCostInformationDto.rmcRowVersion,
					rmcShipContactID = eRPLandedCostInformationDto.rmcShipContactID,
					rmcShipLocationID = eRPLandedCostInformationDto.rmcShipLocationID,
					rmcShipOrganizationID = eRPLandedCostInformationDto.rmcShipOrganizationID,
					rmcTrackingNumber = eRPLandedCostInformationDto.rmcTrackingNumber,
					CustomFields = eRPLandedCostInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the LandedCosts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLandedCostDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = landedCostDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLandedCostDto>> Process_PutLandedCost(ERPLandedCostDto landedCost)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPLandedCostDto createdObject = null;
		ERPResponseMessageDto<ERPLandedCostDto> result;
		try
		{
			IERPLandedCostRepository iERPLandedCostRepository = (base.ERPLandedCostRepository = new ERPLandedCostRepository(base.ApiClientContext));
			using (iERPLandedCostRepository)
			{
				APIValidationInfoDto postResult = await base.ERPLandedCostRepository.SaveLandedCost(landedCost);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPLandedCostInformationDto eRPLandedCostInformationDto = await base.ERPLandedCostRepository.GetLandedCost(landedCost.rmcUniqueID);
					createdObject = new ERPLandedCostDto
					{
						rmcCarrierName = eRPLandedCostInformationDto.rmcCarrierName,
						rmcClosedDate = eRPLandedCostInformationDto.rmcClosedDate,
						rmcLandedCostID = eRPLandedCostInformationDto.rmcLandedCostID,
						rmcConsigneeContactID = eRPLandedCostInformationDto.rmcConsigneeContactID,
						rmcConsigneeLocationID = eRPLandedCostInformationDto.rmcConsigneeLocationID,
						rmcConsigneeOrganizationID = eRPLandedCostInformationDto.rmcConsigneeOrganizationID,
						rmcCreatedBy = eRPLandedCostInformationDto.rmcCreatedBy,
						rmcCreatedDate = eRPLandedCostInformationDto.rmcCreatedDate,
						rmcDischargePoint = eRPLandedCostInformationDto.rmcDischargePoint,
						rmcUniqueID = eRPLandedCostInformationDto.rmcUniqueID,
						rmcGlFiscalYearID = eRPLandedCostInformationDto.rmcGlFiscalYearID,
						rmcGlFiscalYearPeriodID = eRPLandedCostInformationDto.rmcGlFiscalYearPeriodID,
						rmcChargesComplete = eRPLandedCostInformationDto.rmcChargesComplete,
						rmcChargesJournalsCreated = eRPLandedCostInformationDto.rmcChargesJournalsCreated,
						rmcClosed = eRPLandedCostInformationDto.rmcClosed,
						rmcPoInTransitComplete = eRPLandedCostInformationDto.rmcPoInTransitComplete,
						rmcPoInTransitJournalsCreated = eRPLandedCostInformationDto.rmcPoInTransitJournalsCreated,
						rmcPostedToGl = eRPLandedCostInformationDto.rmcPostedToGl,
						rmcReversalEntry = eRPLandedCostInformationDto.rmcReversalEntry,
						rmcReversed = eRPLandedCostInformationDto.rmcReversed,
						rmcLandedCostChargesTotal = eRPLandedCostInformationDto.rmcLandedCostChargesTotal,
						rmcLandedCostDate = eRPLandedCostInformationDto.rmcLandedCostDate,
						rmcLandedCostPurchasesTotal = eRPLandedCostInformationDto.rmcLandedCostPurchasesTotal,
						rmcLandedCostReceiptsTotal = eRPLandedCostInformationDto.rmcLandedCostReceiptsTotal,
						rmcLandedCostTotal = eRPLandedCostInformationDto.rmcLandedCostTotal,
						rmcLoadingPoint = eRPLandedCostInformationDto.rmcLoadingPoint,
						rmcLongDescriptionRtf = eRPLandedCostInformationDto.rmcLongDescriptionRtf,
						rmcLongDescriptionText = eRPLandedCostInformationDto.rmcLongDescriptionText,
						rmcPlantDepartmentID = eRPLandedCostInformationDto.rmcPlantDepartmentID,
						rmcPlantID = eRPLandedCostInformationDto.rmcPlantID,
						rmcPostedDate = eRPLandedCostInformationDto.rmcPostedDate,
						rmcReverseLandedCostID = eRPLandedCostInformationDto.rmcReverseLandedCostID,
						rmcRowVersion = eRPLandedCostInformationDto.rmcRowVersion,
						rmcShipContactID = eRPLandedCostInformationDto.rmcShipContactID,
						rmcShipLocationID = eRPLandedCostInformationDto.rmcShipLocationID,
						rmcShipOrganizationID = eRPLandedCostInformationDto.rmcShipOrganizationID,
						rmcTrackingNumber = eRPLandedCostInformationDto.rmcTrackingNumber,
						CustomFields = eRPLandedCostInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing LandedCost [{landedCost.rmcUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLandedCostDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteLandedCost(Guid landedCostId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLandedCostRepository iERPLandedCostRepository = (base.ERPLandedCostRepository = new ERPLandedCostRepository(base.ApiClientContext));
		using (iERPLandedCostRepository)
		{
			if (!(await base.ERPLandedCostRepository.DoesLandedCostExist(landedCostId)))
			{
				base.ErrorsList.Add($"LandedCost [{landedCostId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPLandedCostInformationDto eRPLandedCostInformationDto = await base.ERPLandedCostRepository.GetLandedCost(landedCostId);
				string text = await base.ERPLandedCostRepository.WhereUsed("LandedCosts", new object[1] { eRPLandedCostInformationDto.rmcLandedCostID }, new object[1] { "rmcLandedCostID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("LandedCost cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPLandedCostDto>> Process_DeleteLandedCost(Guid landedCostId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPLandedCostDto> result;
		try
		{
			IERPLandedCostRepository iERPLandedCostRepository = (base.ERPLandedCostRepository = new ERPLandedCostRepository(base.ApiClientContext));
			using (iERPLandedCostRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPLandedCostRepository.DeleteRowFromTable("LandedCosts", "rmc", landedCostId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of LandedCost [{landedCostId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLandedCostDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPLandedCostDto()
			};
		}
		return result;
	}
}
