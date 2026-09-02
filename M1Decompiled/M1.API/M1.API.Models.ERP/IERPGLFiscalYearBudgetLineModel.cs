using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPGLFiscalYearBudgetLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all GLFiscalYearBudgetLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYearBudgetLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllGLFiscalYearBudgetLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving GLFiscalYearBudgetLine information based on the specified GLFiscalYearBudgetLine Unique Id.
	/// </summary>
	/// <param name="gLFiscalYearBudgetLineId">The Unique Id of the GLFiscalYearBudgetLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetGLFiscalYearBudgetLine(Guid gLFiscalYearBudgetLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating GLFiscalYearBudgetLine information based on the specified GLFiscalYearBudgetLine.
	/// </summary>
	/// <param name="gLFiscalYearBudgetLine">The GLFiscalYearBudgetLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutGLFiscalYearBudgetLine(ERPGLFiscalYearBudgetLineDto gLFiscalYearBudgetLine);

	/// <summary>
	/// Processes the request to retrieve all GLFiscalYearBudgetLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYearBudgetLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLFiscalYearBudgetLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPGLFiscalYearBudgetLineDto>>> Process_GetAllGLFiscalYearBudgetLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific GLFiscalYearBudgetLine.
	/// </summary>
	/// <param name="gLFiscalYearBudgetLineId">The Unique Id of the GLFiscalYearBudgetLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the GLFiscalYearBudgetLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetLineDto>> Process_GetGLFiscalYearBudgetLine(Guid gLFiscalYearBudgetLineId);

	/// <summary>
	/// Processes the creating or updating of a GLFiscalYearBudgetLine record.
	/// </summary>
	/// <param name="gLFiscalYearBudgetLine">The GLFiscalYearBudgetLine data transfer object (DTO) containing the details of the GLFiscalYearBudgetLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the GLFiscalYearBudgetLine details.</returns>
	Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetLineDto>> Process_PutGLFiscalYearBudgetLine(ERPGLFiscalYearBudgetLineDto gLFiscalYearBudgetLine);

	/// <summary>
	/// Validates the request for deleting a GLFiscalYearBudgetLine record.
	/// </summary>
	/// <param name="gLFiscalYearBudgetLineId">The Unique Id of the GLFiscalYearBudgetLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteGLFiscalYearBudgetLine(Guid gLFiscalYearBudgetLineId);

	/// <summary>
	/// Processes the request to delete a GLFiscalYearBudgetLine record.
	/// </summary>
	/// <param name="gLFiscalYearBudgetLineId">The Unique Id of the GLFiscalYearBudgetLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetLineDto>> Process_DeleteGLFiscalYearBudgetLine(Guid gLFiscalYearBudgetLineId);
}
