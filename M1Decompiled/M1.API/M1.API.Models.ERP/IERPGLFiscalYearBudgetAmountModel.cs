using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPGLFiscalYearBudgetAmountModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all GLFiscalYearBudgetAmounts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYearBudgetAmounts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllGLFiscalYearBudgetAmounts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving GLFiscalYearBudgetAmount information based on the specified GLFiscalYearBudgetAmount Unique Id.
	/// </summary>
	/// <param name="gLFiscalYearBudgetAmountId">The Unique Id of the GLFiscalYearBudgetAmount.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetGLFiscalYearBudgetAmount(Guid gLFiscalYearBudgetAmountId);

	/// <summary>
	/// Validates the PUT request for creating or updating GLFiscalYearBudgetAmount information based on the specified GLFiscalYearBudgetAmount.
	/// </summary>
	/// <param name="gLFiscalYearBudgetAmount">The GLFiscalYearBudgetAmount details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutGLFiscalYearBudgetAmount(ERPGLFiscalYearBudgetAmountDto gLFiscalYearBudgetAmount);

	/// <summary>
	/// Processes the request to retrieve all GLFiscalYearBudgetAmounts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYearBudgetAmounts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLFiscalYearBudgetAmounts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPGLFiscalYearBudgetAmountDto>>> Process_GetAllGLFiscalYearBudgetAmounts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific GLFiscalYearBudgetAmount.
	/// </summary>
	/// <param name="gLFiscalYearBudgetAmountId">The Unique Id of the GLFiscalYearBudgetAmount to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the GLFiscalYearBudgetAmount DTO.</returns>
	Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetAmountDto>> Process_GetGLFiscalYearBudgetAmount(Guid gLFiscalYearBudgetAmountId);

	/// <summary>
	/// Processes the creating or updating of a GLFiscalYearBudgetAmount record.
	/// </summary>
	/// <param name="gLFiscalYearBudgetAmount">The GLFiscalYearBudgetAmount data transfer object (DTO) containing the details of the GLFiscalYearBudgetAmount to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the GLFiscalYearBudgetAmount details.</returns>
	Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetAmountDto>> Process_PutGLFiscalYearBudgetAmount(ERPGLFiscalYearBudgetAmountDto gLFiscalYearBudgetAmount);

	/// <summary>
	/// Validates the request for deleting a GLFiscalYearBudgetAmount record.
	/// </summary>
	/// <param name="gLFiscalYearBudgetAmountId">The Unique Id of the GLFiscalYearBudgetAmount.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteGLFiscalYearBudgetAmount(Guid gLFiscalYearBudgetAmountId);

	/// <summary>
	/// Processes the request to delete a GLFiscalYearBudgetAmount record.
	/// </summary>
	/// <param name="gLFiscalYearBudgetAmountId">The Unique Id of the GLFiscalYearBudgetAmount.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetAmountDto>> Process_DeleteGLFiscalYearBudgetAmount(Guid gLFiscalYearBudgetAmountId);
}
