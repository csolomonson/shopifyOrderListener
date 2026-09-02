using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPGLFiscalYearBudgetHeaderModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all GLFiscalYearBudgetHeaders with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYearBudgetHeaders to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllGLFiscalYearBudgetHeaders(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving GLFiscalYearBudgetHeader information based on the specified GLFiscalYearBudgetHeader Unique Id.
	/// </summary>
	/// <param name="gLFiscalYearBudgetHeaderId">The Unique Id of the GLFiscalYearBudgetHeader.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetGLFiscalYearBudgetHeader(Guid gLFiscalYearBudgetHeaderId);

	/// <summary>
	/// Validates the PUT request for creating or updating GLFiscalYearBudgetHeader information based on the specified GLFiscalYearBudgetHeader.
	/// </summary>
	/// <param name="gLFiscalYearBudgetHeader">The GLFiscalYearBudgetHeader details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutGLFiscalYearBudgetHeader(ERPGLFiscalYearBudgetHeaderDto gLFiscalYearBudgetHeader);

	/// <summary>
	/// Processes the request to retrieve all GLFiscalYearBudgetHeaders with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYearBudgetHeaders to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLFiscalYearBudgetHeaders DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPGLFiscalYearBudgetHeaderDto>>> Process_GetAllGLFiscalYearBudgetHeaders(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific GLFiscalYearBudgetHeader.
	/// </summary>
	/// <param name="gLFiscalYearBudgetHeaderId">The Unique Id of the GLFiscalYearBudgetHeader to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the GLFiscalYearBudgetHeader DTO.</returns>
	Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetHeaderDto>> Process_GetGLFiscalYearBudgetHeader(Guid gLFiscalYearBudgetHeaderId);

	/// <summary>
	/// Processes the creating or updating of a GLFiscalYearBudgetHeader record.
	/// </summary>
	/// <param name="gLFiscalYearBudgetHeader">The GLFiscalYearBudgetHeader data transfer object (DTO) containing the details of the GLFiscalYearBudgetHeader to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the GLFiscalYearBudgetHeader details.</returns>
	Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetHeaderDto>> Process_PutGLFiscalYearBudgetHeader(ERPGLFiscalYearBudgetHeaderDto gLFiscalYearBudgetHeader);

	/// <summary>
	/// Validates the request for deleting a GLFiscalYearBudgetHeader record.
	/// </summary>
	/// <param name="gLFiscalYearBudgetHeaderId">The Unique Id of the GLFiscalYearBudgetHeader.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteGLFiscalYearBudgetHeader(Guid gLFiscalYearBudgetHeaderId);

	/// <summary>
	/// Processes the request to delete a GLFiscalYearBudgetHeader record.
	/// </summary>
	/// <param name="gLFiscalYearBudgetHeaderId">The Unique Id of the GLFiscalYearBudgetHeader.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetHeaderDto>> Process_DeleteGLFiscalYearBudgetHeader(Guid gLFiscalYearBudgetHeaderId);
}
