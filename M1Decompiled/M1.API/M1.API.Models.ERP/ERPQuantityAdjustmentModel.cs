using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPQuantityAdjustmentModel : ERPBaseModel, IERPQuantityAdjustmentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllQuantityAdjustments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPQuantityAdjustmentRepository iERPQuantityAdjustmentRepository = (base.ERPQuantityAdjustmentRepository = new ERPQuantityAdjustmentRepository(base.ApiClientContext));
		using (iERPQuantityAdjustmentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPQuantityAdjustmentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPQuantityAdjustmentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPQuantityAdjustmentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPQuantityAdjustmentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetQuantityAdjustment(Guid quantityAdjustmentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuantityAdjustmentRepository iERPQuantityAdjustmentRepository = (base.ERPQuantityAdjustmentRepository = new ERPQuantityAdjustmentRepository(base.ApiClientContext));
		using (iERPQuantityAdjustmentRepository)
		{
			if (!(await base.ERPQuantityAdjustmentRepository.DoesQuantityAdjustmentExist(quantityAdjustmentId)))
			{
				errorsList.Add($"QuantityAdjustment [{quantityAdjustmentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutQuantityAdjustment(ERPQuantityAdjustmentDto quantityAdjustment)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuantityAdjustmentRepository iERPQuantityAdjustmentRepository = (base.ERPQuantityAdjustmentRepository = new ERPQuantityAdjustmentRepository(base.ApiClientContext));
		using (iERPQuantityAdjustmentRepository)
		{
			if (!string.IsNullOrWhiteSpace(quantityAdjustment.inqPlantID) && !(await base.ERPQuantityAdjustmentRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { quantityAdjustment.inqPlantID })))
			{
				errorsList.Add("inqPlantID [" + quantityAdjustment.inqPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quantityAdjustment.inqPlantDepartmentID) && !(await base.ERPQuantityAdjustmentRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { quantityAdjustment.inqPlantID, quantityAdjustment.inqPlantDepartmentID })))
			{
				errorsList.Add("inqPlantDepartmentID [" + quantityAdjustment.inqPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quantityAdjustment.inqPartID) && !(await base.ERPQuantityAdjustmentRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { quantityAdjustment.inqPartID })))
			{
				errorsList.Add("inqPartID [" + quantityAdjustment.inqPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quantityAdjustment.inqPartRevisionID) && !(await base.ERPQuantityAdjustmentRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { quantityAdjustment.inqPartID, quantityAdjustment.inqPartRevisionID })))
			{
				errorsList.Add("inqPartRevisionID [" + quantityAdjustment.inqPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quantityAdjustment.inqPartWarehouseLocationID) && !(await base.ERPQuantityAdjustmentRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { quantityAdjustment.inqPartID, quantityAdjustment.inqPartRevisionID, quantityAdjustment.inqPartWarehouseLocationID })))
			{
				errorsList.Add("inqPartWarehouseLocationID [" + quantityAdjustment.inqPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quantityAdjustment.inqPartBinID) && !(await base.ERPQuantityAdjustmentRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { quantityAdjustment.inqPartID, quantityAdjustment.inqPartRevisionID, quantityAdjustment.inqPartWarehouseLocationID, quantityAdjustment.inqPartBinID })))
			{
				errorsList.Add("inqPartBinID [" + quantityAdjustment.inqPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quantityAdjustment.inqDestinationPartBinID) && !(await base.ERPQuantityAdjustmentRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { quantityAdjustment.inqPartID, quantityAdjustment.inqPartRevisionID, quantityAdjustment.inqDestinationWarehouseID, quantityAdjustment.inqDestinationPartBinID })))
			{
				errorsList.Add("inqDestinationPartBinID [" + quantityAdjustment.inqDestinationPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quantityAdjustment.inqDestinationWarehouseID) && !(await base.ERPQuantityAdjustmentRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { quantityAdjustment.inqPartID, quantityAdjustment.inqPartRevisionID, quantityAdjustment.inqDestinationWarehouseID })))
			{
				errorsList.Add("inqDestinationWarehouseID [" + quantityAdjustment.inqDestinationWarehouseID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPQuantityAdjustmentDto>>> Process_GetAllQuantityAdjustments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPQuantityAdjustmentDto> allQuantityAdjustmentsDto = new List<ERPQuantityAdjustmentDto>();
		ERPResponseMessageDto<IList<ERPQuantityAdjustmentDto>> result;
		try
		{
			IERPQuantityAdjustmentRepository iERPQuantityAdjustmentRepository = (base.ERPQuantityAdjustmentRepository = new ERPQuantityAdjustmentRepository(base.ApiClientContext));
			using (iERPQuantityAdjustmentRepository)
			{
				foreach (ERPQuantityAdjustmentInformationDto item2 in await base.ERPQuantityAdjustmentRepository.GetAllQuantityAdjustments(pageSize, pageNumber, filter, orderBy))
				{
					ERPQuantityAdjustmentDto item = new ERPQuantityAdjustmentDto
					{
						inqAdjustmentDate = item2.inqAdjustmentDate,
						inqAdjustmentDescription = item2.inqAdjustmentDescription,
						inqAdjustmentType = item2.inqAdjustmentType,
						inqBinQuantityReceipted = item2.inqBinQuantityReceipted,
						inqBinQuantityTransferred = item2.inqBinQuantityTransferred,
						inqChangeQuantity = item2.inqChangeQuantity,
						inqQuantityAdjustmentID = item2.inqQuantityAdjustmentID,
						inqCountedQuantity = item2.inqCountedQuantity,
						inqCreatedBy = item2.inqCreatedBy,
						inqCreatedDate = item2.inqCreatedDate,
						inqCurrentQuantity = item2.inqCurrentQuantity,
						inqDestinationPartBinID = item2.inqDestinationPartBinID,
						inqDestinationWarehouseID = item2.inqDestinationWarehouseID,
						inqUniqueID = item2.inqUniqueID,
						inqPosted = item2.inqPosted,
						inqNewQuantity = item2.inqNewQuantity,
						inqPartBinID = item2.inqPartBinID,
						inqPartID = item2.inqPartID,
						inqPartRevisionID = item2.inqPartRevisionID,
						inqPartShortDescription = item2.inqPartShortDescription,
						inqPartWarehouseLocationID = item2.inqPartWarehouseLocationID,
						inqPlantDepartmentID = item2.inqPlantDepartmentID,
						inqPlantID = item2.inqPlantID,
						inqPostedDate = item2.inqPostedDate,
						inqQuantitySince = item2.inqQuantitySince,
						inqRowVersion = item2.inqRowVersion,
						inqTransactionsSince = item2.inqTransactionsSince,
						inqUnitOfMeasure = item2.inqUnitOfMeasure,
						CustomFields = item2.CustomFields
					};
					allQuantityAdjustmentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all QuantityAdjustments]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPQuantityAdjustmentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allQuantityAdjustmentsDto,
				RecordCount = allQuantityAdjustmentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuantityAdjustmentDto>> Process_GetQuantityAdjustment(Guid quantityAdjustmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPQuantityAdjustmentDto quantityAdjustmentDto = null;
		ERPResponseMessageDto<ERPQuantityAdjustmentDto> result;
		try
		{
			IERPQuantityAdjustmentRepository iERPQuantityAdjustmentRepository = (base.ERPQuantityAdjustmentRepository = new ERPQuantityAdjustmentRepository(base.ApiClientContext));
			using (iERPQuantityAdjustmentRepository)
			{
				ERPQuantityAdjustmentInformationDto eRPQuantityAdjustmentInformationDto = await base.ERPQuantityAdjustmentRepository.GetQuantityAdjustment(quantityAdjustmentId);
				quantityAdjustmentDto = new ERPQuantityAdjustmentDto
				{
					inqAdjustmentDate = eRPQuantityAdjustmentInformationDto.inqAdjustmentDate,
					inqAdjustmentDescription = eRPQuantityAdjustmentInformationDto.inqAdjustmentDescription,
					inqAdjustmentType = eRPQuantityAdjustmentInformationDto.inqAdjustmentType,
					inqBinQuantityReceipted = eRPQuantityAdjustmentInformationDto.inqBinQuantityReceipted,
					inqBinQuantityTransferred = eRPQuantityAdjustmentInformationDto.inqBinQuantityTransferred,
					inqChangeQuantity = eRPQuantityAdjustmentInformationDto.inqChangeQuantity,
					inqQuantityAdjustmentID = eRPQuantityAdjustmentInformationDto.inqQuantityAdjustmentID,
					inqCountedQuantity = eRPQuantityAdjustmentInformationDto.inqCountedQuantity,
					inqCreatedBy = eRPQuantityAdjustmentInformationDto.inqCreatedBy,
					inqCreatedDate = eRPQuantityAdjustmentInformationDto.inqCreatedDate,
					inqCurrentQuantity = eRPQuantityAdjustmentInformationDto.inqCurrentQuantity,
					inqDestinationPartBinID = eRPQuantityAdjustmentInformationDto.inqDestinationPartBinID,
					inqDestinationWarehouseID = eRPQuantityAdjustmentInformationDto.inqDestinationWarehouseID,
					inqUniqueID = eRPQuantityAdjustmentInformationDto.inqUniqueID,
					inqPosted = eRPQuantityAdjustmentInformationDto.inqPosted,
					inqNewQuantity = eRPQuantityAdjustmentInformationDto.inqNewQuantity,
					inqPartBinID = eRPQuantityAdjustmentInformationDto.inqPartBinID,
					inqPartID = eRPQuantityAdjustmentInformationDto.inqPartID,
					inqPartRevisionID = eRPQuantityAdjustmentInformationDto.inqPartRevisionID,
					inqPartShortDescription = eRPQuantityAdjustmentInformationDto.inqPartShortDescription,
					inqPartWarehouseLocationID = eRPQuantityAdjustmentInformationDto.inqPartWarehouseLocationID,
					inqPlantDepartmentID = eRPQuantityAdjustmentInformationDto.inqPlantDepartmentID,
					inqPlantID = eRPQuantityAdjustmentInformationDto.inqPlantID,
					inqPostedDate = eRPQuantityAdjustmentInformationDto.inqPostedDate,
					inqQuantitySince = eRPQuantityAdjustmentInformationDto.inqQuantitySince,
					inqRowVersion = eRPQuantityAdjustmentInformationDto.inqRowVersion,
					inqTransactionsSince = eRPQuantityAdjustmentInformationDto.inqTransactionsSince,
					inqUnitOfMeasure = eRPQuantityAdjustmentInformationDto.inqUnitOfMeasure,
					CustomFields = eRPQuantityAdjustmentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the QuantityAdjustments []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuantityAdjustmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quantityAdjustmentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuantityAdjustmentDto>> Process_PutQuantityAdjustment(ERPQuantityAdjustmentDto quantityAdjustment)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPQuantityAdjustmentDto createdObject = null;
		ERPResponseMessageDto<ERPQuantityAdjustmentDto> result;
		try
		{
			IERPQuantityAdjustmentRepository iERPQuantityAdjustmentRepository = (base.ERPQuantityAdjustmentRepository = new ERPQuantityAdjustmentRepository(base.ApiClientContext));
			using (iERPQuantityAdjustmentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPQuantityAdjustmentRepository.SaveQuantityAdjustment(quantityAdjustment);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPQuantityAdjustmentInformationDto eRPQuantityAdjustmentInformationDto = await base.ERPQuantityAdjustmentRepository.GetQuantityAdjustment(quantityAdjustment.inqUniqueID);
					createdObject = new ERPQuantityAdjustmentDto
					{
						inqAdjustmentDate = eRPQuantityAdjustmentInformationDto.inqAdjustmentDate,
						inqAdjustmentDescription = eRPQuantityAdjustmentInformationDto.inqAdjustmentDescription,
						inqAdjustmentType = eRPQuantityAdjustmentInformationDto.inqAdjustmentType,
						inqBinQuantityReceipted = eRPQuantityAdjustmentInformationDto.inqBinQuantityReceipted,
						inqBinQuantityTransferred = eRPQuantityAdjustmentInformationDto.inqBinQuantityTransferred,
						inqChangeQuantity = eRPQuantityAdjustmentInformationDto.inqChangeQuantity,
						inqQuantityAdjustmentID = eRPQuantityAdjustmentInformationDto.inqQuantityAdjustmentID,
						inqCountedQuantity = eRPQuantityAdjustmentInformationDto.inqCountedQuantity,
						inqCreatedBy = eRPQuantityAdjustmentInformationDto.inqCreatedBy,
						inqCreatedDate = eRPQuantityAdjustmentInformationDto.inqCreatedDate,
						inqCurrentQuantity = eRPQuantityAdjustmentInformationDto.inqCurrentQuantity,
						inqDestinationPartBinID = eRPQuantityAdjustmentInformationDto.inqDestinationPartBinID,
						inqDestinationWarehouseID = eRPQuantityAdjustmentInformationDto.inqDestinationWarehouseID,
						inqUniqueID = eRPQuantityAdjustmentInformationDto.inqUniqueID,
						inqPosted = eRPQuantityAdjustmentInformationDto.inqPosted,
						inqNewQuantity = eRPQuantityAdjustmentInformationDto.inqNewQuantity,
						inqPartBinID = eRPQuantityAdjustmentInformationDto.inqPartBinID,
						inqPartID = eRPQuantityAdjustmentInformationDto.inqPartID,
						inqPartRevisionID = eRPQuantityAdjustmentInformationDto.inqPartRevisionID,
						inqPartShortDescription = eRPQuantityAdjustmentInformationDto.inqPartShortDescription,
						inqPartWarehouseLocationID = eRPQuantityAdjustmentInformationDto.inqPartWarehouseLocationID,
						inqPlantDepartmentID = eRPQuantityAdjustmentInformationDto.inqPlantDepartmentID,
						inqPlantID = eRPQuantityAdjustmentInformationDto.inqPlantID,
						inqPostedDate = eRPQuantityAdjustmentInformationDto.inqPostedDate,
						inqQuantitySince = eRPQuantityAdjustmentInformationDto.inqQuantitySince,
						inqRowVersion = eRPQuantityAdjustmentInformationDto.inqRowVersion,
						inqTransactionsSince = eRPQuantityAdjustmentInformationDto.inqTransactionsSince,
						inqUnitOfMeasure = eRPQuantityAdjustmentInformationDto.inqUnitOfMeasure,
						CustomFields = eRPQuantityAdjustmentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing QuantityAdjustment [{quantityAdjustment.inqUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuantityAdjustmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteQuantityAdjustment(Guid quantityAdjustmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuantityAdjustmentRepository iERPQuantityAdjustmentRepository = (base.ERPQuantityAdjustmentRepository = new ERPQuantityAdjustmentRepository(base.ApiClientContext));
		using (iERPQuantityAdjustmentRepository)
		{
			if (!(await base.ERPQuantityAdjustmentRepository.DoesQuantityAdjustmentExist(quantityAdjustmentId)))
			{
				base.ErrorsList.Add($"QuantityAdjustment [{quantityAdjustmentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPQuantityAdjustmentInformationDto eRPQuantityAdjustmentInformationDto = await base.ERPQuantityAdjustmentRepository.GetQuantityAdjustment(quantityAdjustmentId);
				string text = await base.ERPQuantityAdjustmentRepository.WhereUsed("QuantityAdjustments", new object[1] { eRPQuantityAdjustmentInformationDto.inqQuantityAdjustmentID }, new object[1] { "inqQuantityAdjustmentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("QuantityAdjustment cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPQuantityAdjustmentDto>> Process_DeleteQuantityAdjustment(Guid quantityAdjustmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPQuantityAdjustmentDto> result;
		try
		{
			IERPQuantityAdjustmentRepository iERPQuantityAdjustmentRepository = (base.ERPQuantityAdjustmentRepository = new ERPQuantityAdjustmentRepository(base.ApiClientContext));
			using (iERPQuantityAdjustmentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPQuantityAdjustmentRepository.DeleteRowFromTable("QuantityAdjustments", "inq", quantityAdjustmentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of QuantityAdjustment [{quantityAdjustmentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuantityAdjustmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPQuantityAdjustmentDto()
			};
		}
		return result;
	}
}
