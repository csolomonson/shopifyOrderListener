using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPNextIDModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all NextIDs with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of NextIDs to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllNextIDs(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving NextID information based on the specified NextID Unique Id.
	/// </summary>
	/// <param name="nextIDId">The Unique Id of the NextID.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetNextID(Guid nextIDId);

	/// <summary>
	/// Validates the PUT request for creating or updating NextID information based on the specified NextID.
	/// </summary>
	/// <param name="nextID">The NextID details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutNextID(ERPNextIDDto nextID);

	/// <summary>
	/// Processes the request to retrieve all NextIDs with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of NextIDs to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of NextIDs DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPNextIDDto>>> Process_GetAllNextIDs(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific NextID.
	/// </summary>
	/// <param name="nextIDId">The Unique Id of the NextID to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the NextID DTO.</returns>
	Task<ERPResponseMessageDto<ERPNextIDDto>> Process_GetNextID(Guid nextIDId);

	/// <summary>
	/// Processes the creating or updating of a NextID record.
	/// </summary>
	/// <param name="nextID">The NextID data transfer object (DTO) containing the details of the NextID to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the NextID details.</returns>
	Task<ERPResponseMessageDto<ERPNextIDDto>> Process_PutNextID(ERPNextIDDto nextID);

	/// <summary>
	/// Validates the request for deleting a NextID record.
	/// </summary>
	/// <param name="nextIDId">The Unique Id of the NextID.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteNextID(Guid nextIDId);

	/// <summary>
	/// Processes the request to delete a NextID record.
	/// </summary>
	/// <param name="nextIDId">The Unique Id of the NextID.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPNextIDDto>> Process_DeleteNextID(Guid nextIDId);
}
