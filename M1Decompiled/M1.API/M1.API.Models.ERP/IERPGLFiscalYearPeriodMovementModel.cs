using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPGLFiscalYearPeriodMovementModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all GLFiscalYearPeriodMovements with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYearPeriodMovements to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllGLFiscalYearPeriodMovements(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving GLFiscalYearPeriodMovement information based on the specified GLFiscalYearPeriodMovement Unique Id.
	/// </summary>
	/// <param name="gLFiscalYearPeriodMovementId">The Unique Id of the GLFiscalYearPeriodMovement.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetGLFiscalYearPeriodMovement(Guid gLFiscalYearPeriodMovementId);

	/// <summary>
	/// Processes the request to retrieve all GLFiscalYearPeriodMovements with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYearPeriodMovements to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLFiscalYearPeriodMovements DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPGLFiscalYearPeriodMovementDto>>> Process_GetAllGLFiscalYearPeriodMovements(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific GLFiscalYearPeriodMovement.
	/// </summary>
	/// <param name="gLFiscalYearPeriodMovementId">The Unique Id of the GLFiscalYearPeriodMovement to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the GLFiscalYearPeriodMovement DTO.</returns>
	Task<ERPResponseMessageDto<ERPGLFiscalYearPeriodMovementDto>> Process_GetGLFiscalYearPeriodMovement(Guid gLFiscalYearPeriodMovementId);
}
