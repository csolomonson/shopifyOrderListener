using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPGLFiscalYearModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all GLFiscalYears with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYears to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllGLFiscalYears(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving GLFiscalYear information based on the specified GLFiscalYear Unique Id.
	/// </summary>
	/// <param name="gLFiscalYearId">The Unique Id of the GLFiscalYear.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetGLFiscalYear(Guid gLFiscalYearId);

	/// <summary>
	/// Validates the PUT request for creating or updating GLFiscalYear information based on the specified GLFiscalYear.
	/// </summary>
	/// <param name="gLFiscalYear">The GLFiscalYear details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutGLFiscalYear(ERPGLFiscalYearDto gLFiscalYear);

	/// <summary>
	/// Processes the request to retrieve all GLFiscalYears with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYears to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLFiscalYears DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPGLFiscalYearDto>>> Process_GetAllGLFiscalYears(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific GLFiscalYear.
	/// </summary>
	/// <param name="gLFiscalYearId">The Unique Id of the GLFiscalYear to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the GLFiscalYear DTO.</returns>
	Task<ERPResponseMessageDto<ERPGLFiscalYearDto>> Process_GetGLFiscalYear(Guid gLFiscalYearId);

	/// <summary>
	/// Processes the creating or updating of a GLFiscalYear record.
	/// </summary>
	/// <param name="gLFiscalYear">The GLFiscalYear data transfer object (DTO) containing the details of the GLFiscalYear to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the GLFiscalYear details.</returns>
	Task<ERPResponseMessageDto<ERPGLFiscalYearDto>> Process_PutGLFiscalYear(ERPGLFiscalYearDto gLFiscalYear);

	/// <summary>
	/// Validates the request for deleting a GLFiscalYear record.
	/// </summary>
	/// <param name="gLFiscalYearId">The Unique Id of the GLFiscalYear.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteGLFiscalYear(Guid gLFiscalYearId);

	/// <summary>
	/// Processes the request to delete a GLFiscalYear record.
	/// </summary>
	/// <param name="gLFiscalYearId">The Unique Id of the GLFiscalYear.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPGLFiscalYearDto>> Process_DeleteGLFiscalYear(Guid gLFiscalYearId);
}
