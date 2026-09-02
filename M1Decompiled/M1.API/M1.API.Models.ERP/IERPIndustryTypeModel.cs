using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPIndustryTypeModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all IndustryTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of IndustryTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllIndustryTypes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving IndustryType information based on the specified IndustryType Unique Id.
	/// </summary>
	/// <param name="industryTypeId">The Unique Id of the IndustryType.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetIndustryType(Guid industryTypeId);

	/// <summary>
	/// Validates the PUT request for creating or updating IndustryType information based on the specified IndustryType.
	/// </summary>
	/// <param name="industryType">The IndustryType details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutIndustryType(ERPIndustryTypeDto industryType);

	/// <summary>
	/// Processes the request to retrieve all IndustryTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of IndustryTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of IndustryTypes DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPIndustryTypeDto>>> Process_GetAllIndustryTypes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific IndustryType.
	/// </summary>
	/// <param name="industryTypeId">The Unique Id of the IndustryType to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the IndustryType DTO.</returns>
	Task<ERPResponseMessageDto<ERPIndustryTypeDto>> Process_GetIndustryType(Guid industryTypeId);

	/// <summary>
	/// Processes the creating or updating of a IndustryType record.
	/// </summary>
	/// <param name="industryType">The IndustryType data transfer object (DTO) containing the details of the IndustryType to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the IndustryType details.</returns>
	Task<ERPResponseMessageDto<ERPIndustryTypeDto>> Process_PutIndustryType(ERPIndustryTypeDto industryType);

	/// <summary>
	/// Validates the request for deleting a IndustryType record.
	/// </summary>
	/// <param name="industryTypeId">The Unique Id of the IndustryType.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteIndustryType(Guid industryTypeId);

	/// <summary>
	/// Processes the request to delete a IndustryType record.
	/// </summary>
	/// <param name="industryTypeId">The Unique Id of the IndustryType.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPIndustryTypeDto>> Process_DeleteIndustryType(Guid industryTypeId);
}
