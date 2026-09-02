using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPGLFiscalYearPeriodModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all GLFiscalYearPeriods with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYearPeriods to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllGLFiscalYearPeriods(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving GLFiscalYearPeriod information based on the specified GLFiscalYearPeriod Unique Id.
	/// </summary>
	/// <param name="gLFiscalYearPeriodId">The Unique Id of the GLFiscalYearPeriod.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetGLFiscalYearPeriod(Guid gLFiscalYearPeriodId);

	/// <summary>
	/// Validates the PUT request for creating or updating GLFiscalYearPeriod information based on the specified GLFiscalYearPeriod.
	/// </summary>
	/// <param name="gLFiscalYearPeriod">The GLFiscalYearPeriod details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutGLFiscalYearPeriod(ERPGLFiscalYearPeriodDto gLFiscalYearPeriod);

	/// <summary>
	/// Processes the request to retrieve all GLFiscalYearPeriods with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYearPeriods to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLFiscalYearPeriods DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPGLFiscalYearPeriodDto>>> Process_GetAllGLFiscalYearPeriods(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific GLFiscalYearPeriod.
	/// </summary>
	/// <param name="gLFiscalYearPeriodId">The Unique Id of the GLFiscalYearPeriod to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the GLFiscalYearPeriod DTO.</returns>
	Task<ERPResponseMessageDto<ERPGLFiscalYearPeriodDto>> Process_GetGLFiscalYearPeriod(Guid gLFiscalYearPeriodId);

	/// <summary>
	/// Processes the creating or updating of a GLFiscalYearPeriod record.
	/// </summary>
	/// <param name="gLFiscalYearPeriod">The GLFiscalYearPeriod data transfer object (DTO) containing the details of the GLFiscalYearPeriod to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the GLFiscalYearPeriod details.</returns>
	Task<ERPResponseMessageDto<ERPGLFiscalYearPeriodDto>> Process_PutGLFiscalYearPeriod(ERPGLFiscalYearPeriodDto gLFiscalYearPeriod);

	/// <summary>
	/// Validates the request for deleting a GLFiscalYearPeriod record.
	/// </summary>
	/// <param name="gLFiscalYearPeriodId">The Unique Id of the GLFiscalYearPeriod.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteGLFiscalYearPeriod(Guid gLFiscalYearPeriodId);

	/// <summary>
	/// Processes the request to delete a GLFiscalYearPeriod record.
	/// </summary>
	/// <param name="gLFiscalYearPeriodId">The Unique Id of the GLFiscalYearPeriod.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPGLFiscalYearPeriodDto>> Process_DeleteGLFiscalYearPeriod(Guid gLFiscalYearPeriodId);
}
