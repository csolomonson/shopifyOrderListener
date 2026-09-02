using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPToolMovementModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ToolMovements with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ToolMovements to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllToolMovements(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ToolMovement information based on the specified ToolMovement Unique Id.
	/// </summary>
	/// <param name="toolMovementId">The Unique Id of the ToolMovement.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetToolMovement(Guid toolMovementId);

	/// <summary>
	/// Validates the PUT request for creating or updating ToolMovement information based on the specified ToolMovement.
	/// </summary>
	/// <param name="toolMovement">The ToolMovement details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutToolMovement(ERPToolMovementDto toolMovement);

	/// <summary>
	/// Processes the request to retrieve all ToolMovements with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ToolMovements to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ToolMovements DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPToolMovementDto>>> Process_GetAllToolMovements(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ToolMovement.
	/// </summary>
	/// <param name="toolMovementId">The Unique Id of the ToolMovement to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ToolMovement DTO.</returns>
	Task<ERPResponseMessageDto<ERPToolMovementDto>> Process_GetToolMovement(Guid toolMovementId);

	/// <summary>
	/// Processes the creating or updating of a ToolMovement record.
	/// </summary>
	/// <param name="toolMovement">The ToolMovement data transfer object (DTO) containing the details of the ToolMovement to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ToolMovement details.</returns>
	Task<ERPResponseMessageDto<ERPToolMovementDto>> Process_PutToolMovement(ERPToolMovementDto toolMovement);

	/// <summary>
	/// Validates the request for deleting a ToolMovement record.
	/// </summary>
	/// <param name="toolMovementId">The Unique Id of the ToolMovement.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteToolMovement(Guid toolMovementId);

	/// <summary>
	/// Processes the request to delete a ToolMovement record.
	/// </summary>
	/// <param name="toolMovementId">The Unique Id of the ToolMovement.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPToolMovementDto>> Process_DeleteToolMovement(Guid toolMovementId);
}
