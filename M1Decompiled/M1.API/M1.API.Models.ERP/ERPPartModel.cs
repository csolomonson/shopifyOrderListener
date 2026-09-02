using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartModel : ERPBaseModel, IERPPartModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllParts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartRepository iERPPartRepository = (base.ERPPartRepository = new ERPPartRepository(base.ApiClientContext));
		using (iERPPartRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPart(Guid partId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartRepository iERPPartRepository = (base.ERPPartRepository = new ERPPartRepository(base.ApiClientContext));
		using (iERPPartRepository)
		{
			if (!(await base.ERPPartRepository.DoesPartExist(partId)))
			{
				errorsList.Add($"Part [{partId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPart(ERPPartDto part)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartRepository iERPPartRepository = (base.ERPPartRepository = new ERPPartRepository(base.ApiClientContext));
		using (iERPPartRepository)
		{
			if (!string.IsNullOrWhiteSpace(part.impPartGroupID) && !(await base.ERPPartRepository.DoesRecordExistInTableUsingKeys("PartGroups", new object[1] { "IMUPARTGROUPID" }, new object[1] { part.impPartGroupID })))
			{
				errorsList.Add("impPartGroupID [" + part.impPartGroupID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(part.impPartClassID) && !(await base.ERPPartRepository.DoesRecordExistInTableUsingKeys("PartClasses", new object[1] { "IMCPARTCLASSID" }, new object[1] { part.impPartClassID })))
			{
				errorsList.Add("impPartClassID [" + part.impPartClassID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(part.impCycleCodeID) && !(await base.ERPPartRepository.DoesRecordExistInTableUsingKeys("CycleCodes", new object[1] { "IMDCYCLECODEID" }, new object[1] { part.impCycleCodeID })))
			{
				errorsList.Add("impCycleCodeID [" + part.impCycleCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(part.impOEMOrganizationID) && !(await base.ERPPartRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { part.impOEMOrganizationID })))
			{
				errorsList.Add("impOEMOrganizationID [" + part.impOEMOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(part.impSecondTaxCodeID) && !(await base.ERPPartRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { part.impSecondTaxCodeID })))
			{
				errorsList.Add("impSecondTaxCodeID [" + part.impSecondTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(part.impTaxCodeID) && !(await base.ERPPartRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { part.impTaxCodeID })))
			{
				errorsList.Add("impTaxCodeID [" + part.impTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(part.impNonTaxReasonID) && !(await base.ERPPartRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { part.impNonTaxReasonID })))
			{
				errorsList.Add("impNonTaxReasonID [" + part.impNonTaxReasonID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartDto>>> Process_GetAllParts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartDto> allPartsDto = new List<ERPPartDto>();
		ERPResponseMessageDto<IList<ERPPartDto>> result;
		try
		{
			IERPPartRepository iERPPartRepository = (base.ERPPartRepository = new ERPPartRepository(base.ApiClientContext));
			using (iERPPartRepository)
			{
				foreach (ERPPartInformationDto item2 in await base.ERPPartRepository.GetAllParts(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartDto item = new ERPPartDto
					{
						impPartID = item2.impPartID,
						impContractLength = item2.impContractLength,
						impContractLengthType = item2.impContractLengthType,
						impCreatedBy = item2.impCreatedBy,
						impCreatedDate = item2.impCreatedDate,
						impCycleCodeID = item2.impCycleCodeID,
						impDeliveryType = item2.impDeliveryType,
						impUniqueID = item2.impUniqueID,
						impInactiveDate = item2.impInactiveDate,
						impInactive = item2.impInactive,
						impAlwaysNonTaxable = item2.impAlwaysNonTaxable,
						impBuyForInventory = item2.impBuyForInventory,
						impNonPhysicalShipment = item2.impNonPhysicalShipment,
						impNonStockedItem = item2.impNonStockedItem,
						impPhantomOrKitPart = item2.impPhantomOrKitPart,
						impTrackLotNumbers = item2.impTrackLotNumbers,
						impTrackSerialNumbers = item2.impTrackSerialNumbers,
						impLongDescriptionRtf = item2.impLongDescriptionRtf,
						impLongDescriptionText = item2.impLongDescriptionText,
						impNextSerialNumberIDFormula = item2.impNextSerialNumberIDFormula,
						impNonTaxReasonID = item2.impNonTaxReasonID,
						impOEMOrganizationID = item2.impOEMOrganizationID,
						impPartClassID = item2.impPartClassID,
						impPartGroupID = item2.impPartGroupID,
						impPartType = item2.impPartType,
						impReorderMethod = item2.impReorderMethod,
						impRowVersion = item2.impRowVersion,
						impSecondTaxCodeID = item2.impSecondTaxCodeID,
						impShortDescription = item2.impShortDescription,
						impTaxCodeID = item2.impTaxCodeID,
						CustomFields = item2.CustomFields
					};
					allPartsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Parts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartsDto,
				RecordCount = allPartsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartDto>> Process_GetPart(Guid partId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartDto partDto = null;
		ERPResponseMessageDto<ERPPartDto> result;
		try
		{
			IERPPartRepository iERPPartRepository = (base.ERPPartRepository = new ERPPartRepository(base.ApiClientContext));
			using (iERPPartRepository)
			{
				ERPPartInformationDto eRPPartInformationDto = await base.ERPPartRepository.GetPart(partId);
				partDto = new ERPPartDto
				{
					impPartID = eRPPartInformationDto.impPartID,
					impContractLength = eRPPartInformationDto.impContractLength,
					impContractLengthType = eRPPartInformationDto.impContractLengthType,
					impCreatedBy = eRPPartInformationDto.impCreatedBy,
					impCreatedDate = eRPPartInformationDto.impCreatedDate,
					impCycleCodeID = eRPPartInformationDto.impCycleCodeID,
					impDeliveryType = eRPPartInformationDto.impDeliveryType,
					impUniqueID = eRPPartInformationDto.impUniqueID,
					impInactiveDate = eRPPartInformationDto.impInactiveDate,
					impInactive = eRPPartInformationDto.impInactive,
					impAlwaysNonTaxable = eRPPartInformationDto.impAlwaysNonTaxable,
					impBuyForInventory = eRPPartInformationDto.impBuyForInventory,
					impNonPhysicalShipment = eRPPartInformationDto.impNonPhysicalShipment,
					impNonStockedItem = eRPPartInformationDto.impNonStockedItem,
					impPhantomOrKitPart = eRPPartInformationDto.impPhantomOrKitPart,
					impTrackLotNumbers = eRPPartInformationDto.impTrackLotNumbers,
					impTrackSerialNumbers = eRPPartInformationDto.impTrackSerialNumbers,
					impLongDescriptionRtf = eRPPartInformationDto.impLongDescriptionRtf,
					impLongDescriptionText = eRPPartInformationDto.impLongDescriptionText,
					impNextSerialNumberIDFormula = eRPPartInformationDto.impNextSerialNumberIDFormula,
					impNonTaxReasonID = eRPPartInformationDto.impNonTaxReasonID,
					impOEMOrganizationID = eRPPartInformationDto.impOEMOrganizationID,
					impPartClassID = eRPPartInformationDto.impPartClassID,
					impPartGroupID = eRPPartInformationDto.impPartGroupID,
					impPartType = eRPPartInformationDto.impPartType,
					impReorderMethod = eRPPartInformationDto.impReorderMethod,
					impRowVersion = eRPPartInformationDto.impRowVersion,
					impSecondTaxCodeID = eRPPartInformationDto.impSecondTaxCodeID,
					impShortDescription = eRPPartInformationDto.impShortDescription,
					impTaxCodeID = eRPPartInformationDto.impTaxCodeID,
					CustomFields = eRPPartInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Parts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartDto>> Process_PutPart(ERPPartDto part)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartDto createdObject = null;
		ERPResponseMessageDto<ERPPartDto> result;
		try
		{
			IERPPartRepository iERPPartRepository = (base.ERPPartRepository = new ERPPartRepository(base.ApiClientContext));
			using (iERPPartRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartRepository.SavePart(part);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartInformationDto eRPPartInformationDto = await base.ERPPartRepository.GetPart(part.impUniqueID);
					createdObject = new ERPPartDto
					{
						impPartID = eRPPartInformationDto.impPartID,
						impContractLength = eRPPartInformationDto.impContractLength,
						impContractLengthType = eRPPartInformationDto.impContractLengthType,
						impCreatedBy = eRPPartInformationDto.impCreatedBy,
						impCreatedDate = eRPPartInformationDto.impCreatedDate,
						impCycleCodeID = eRPPartInformationDto.impCycleCodeID,
						impDeliveryType = eRPPartInformationDto.impDeliveryType,
						impUniqueID = eRPPartInformationDto.impUniqueID,
						impInactiveDate = eRPPartInformationDto.impInactiveDate,
						impInactive = eRPPartInformationDto.impInactive,
						impAlwaysNonTaxable = eRPPartInformationDto.impAlwaysNonTaxable,
						impBuyForInventory = eRPPartInformationDto.impBuyForInventory,
						impNonPhysicalShipment = eRPPartInformationDto.impNonPhysicalShipment,
						impNonStockedItem = eRPPartInformationDto.impNonStockedItem,
						impPhantomOrKitPart = eRPPartInformationDto.impPhantomOrKitPart,
						impTrackLotNumbers = eRPPartInformationDto.impTrackLotNumbers,
						impTrackSerialNumbers = eRPPartInformationDto.impTrackSerialNumbers,
						impLongDescriptionRtf = eRPPartInformationDto.impLongDescriptionRtf,
						impLongDescriptionText = eRPPartInformationDto.impLongDescriptionText,
						impNextSerialNumberIDFormula = eRPPartInformationDto.impNextSerialNumberIDFormula,
						impNonTaxReasonID = eRPPartInformationDto.impNonTaxReasonID,
						impOEMOrganizationID = eRPPartInformationDto.impOEMOrganizationID,
						impPartClassID = eRPPartInformationDto.impPartClassID,
						impPartGroupID = eRPPartInformationDto.impPartGroupID,
						impPartType = eRPPartInformationDto.impPartType,
						impReorderMethod = eRPPartInformationDto.impReorderMethod,
						impRowVersion = eRPPartInformationDto.impRowVersion,
						impSecondTaxCodeID = eRPPartInformationDto.impSecondTaxCodeID,
						impShortDescription = eRPPartInformationDto.impShortDescription,
						impTaxCodeID = eRPPartInformationDto.impTaxCodeID,
						CustomFields = eRPPartInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Part [{part.impUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePart(Guid partId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartRepository iERPPartRepository = (base.ERPPartRepository = new ERPPartRepository(base.ApiClientContext));
		using (iERPPartRepository)
		{
			if (!(await base.ERPPartRepository.DoesPartExist(partId)))
			{
				base.ErrorsList.Add($"Part [{partId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartInformationDto eRPPartInformationDto = await base.ERPPartRepository.GetPart(partId);
				string text = await base.ERPPartRepository.WhereUsed("Parts", new object[1] { eRPPartInformationDto.impPartID }, new object[1] { "impPartID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Part cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartDto>> Process_DeletePart(Guid partId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartDto> result;
		try
		{
			IERPPartRepository iERPPartRepository = (base.ERPPartRepository = new ERPPartRepository(base.ApiClientContext));
			using (iERPPartRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartRepository.DeleteRowFromTable("Parts", "imp", partId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Part [{partId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartDto()
			};
		}
		return result;
	}
}
