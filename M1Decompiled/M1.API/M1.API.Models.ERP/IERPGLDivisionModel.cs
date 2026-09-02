using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPGLDivisionModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all GLDivisions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLDivisions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllGLDivisions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving GLDivision information based on the specified GLDivision Unique Id.
	/// </summary>
	/// <param name="gLDivisionId">The Unique Id of the GLDivision.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetGLDivision(Guid gLDivisionId);

	/// <summary>
	/// Validates the PUT request for creating or updating GLDivision information based on the specified GLDivision.
	/// </summary>
	/// <param name="gLDivision">The GLDivision details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutGLDivision(ERPGLDivisionDto gLDivision);

	/// <summary>
	/// Processes the request to retrieve all GLDivisions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLDivisions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLDivisions DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPGLDivisionDto>>> Process_GetAllGLDivisions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific GLDivision.
	/// </summary>
	/// <param name="gLDivisionId">The Unique Id of the GLDivision to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the GLDivision DTO.</returns>
	Task<ERPResponseMessageDto<ERPGLDivisionDto>> Process_GetGLDivision(Guid gLDivisionId);

	/// <summary>
	/// Processes the creating or updating of a GLDivision record.
	/// </summary>
	/// <param name="gLDivision">The GLDivision data transfer object (DTO) containing the details of the GLDivision to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the GLDivision details.</returns>
	Task<ERPResponseMessageDto<ERPGLDivisionDto>> Process_PutGLDivision(ERPGLDivisionDto gLDivision);

	/// <summary>
	/// Validates the request for deleting a GLDivision record.
	/// </summary>
	/// <param name="gLDivisionId">The Unique Id of the GLDivision.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteGLDivision(Guid gLDivisionId);

	/// <summary>
	/// Processes the request to delete a GLDivision record.
	/// </summary>
	/// <param name="gLDivisionId">The Unique Id of the GLDivision.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPGLDivisionDto>> Process_DeleteGLDivision(Guid gLDivisionId);
}
