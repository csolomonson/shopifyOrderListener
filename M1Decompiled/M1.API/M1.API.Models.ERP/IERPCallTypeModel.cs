using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPCallTypeModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all CallTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CallTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllCallTypes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving CallType information based on the specified CallType Unique Id.
	/// </summary>
	/// <param name="callTypeId">The Unique Id of the CallType.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetCallType(Guid callTypeId);

	/// <summary>
	/// Processes the request to retrieve all CallTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CallTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CallTypes DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPCallTypeDto>>> Process_GetAllCallTypes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific CallType.
	/// </summary>
	/// <param name="callTypeId">The Unique Id of the CallType to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the CallType DTO.</returns>
	Task<ERPResponseMessageDto<ERPCallTypeDto>> Process_GetCallType(Guid callTypeId);
}
