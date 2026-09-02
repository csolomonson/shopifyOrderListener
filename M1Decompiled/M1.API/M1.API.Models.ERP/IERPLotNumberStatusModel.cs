using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPLotNumberStatusModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all LotNumberStatuses with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LotNumberStatuses to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllLotNumberStatuses(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving LotNumberStatus information based on the specified LotNumberStatus Unique Id.
	/// </summary>
	/// <param name="lotNumberStatusId">The Unique Id of the LotNumberStatus.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetLotNumberStatus(Guid lotNumberStatusId);

	/// <summary>
	/// Validates the PUT request for creating or updating LotNumberStatus information based on the specified LotNumberStatus.
	/// </summary>
	/// <param name="lotNumberStatus">The LotNumberStatus details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutLotNumberStatus(ERPLotNumberStatusDto lotNumberStatus);

	/// <summary>
	/// Processes the request to retrieve all LotNumberStatuses with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LotNumberStatuses to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LotNumberStatuses DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPLotNumberStatusDto>>> Process_GetAllLotNumberStatuses(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific LotNumberStatus.
	/// </summary>
	/// <param name="lotNumberStatusId">The Unique Id of the LotNumberStatus to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the LotNumberStatus DTO.</returns>
	Task<ERPResponseMessageDto<ERPLotNumberStatusDto>> Process_GetLotNumberStatus(Guid lotNumberStatusId);

	/// <summary>
	/// Processes the creating or updating of a LotNumberStatus record.
	/// </summary>
	/// <param name="lotNumberStatus">The LotNumberStatus data transfer object (DTO) containing the details of the LotNumberStatus to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the LotNumberStatus details.</returns>
	Task<ERPResponseMessageDto<ERPLotNumberStatusDto>> Process_PutLotNumberStatus(ERPLotNumberStatusDto lotNumberStatus);

	/// <summary>
	/// Validates the request for deleting a LotNumberStatus record.
	/// </summary>
	/// <param name="lotNumberStatusId">The Unique Id of the LotNumberStatus.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteLotNumberStatus(Guid lotNumberStatusId);

	/// <summary>
	/// Processes the request to delete a LotNumberStatus record.
	/// </summary>
	/// <param name="lotNumberStatusId">The Unique Id of the LotNumberStatus.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPLotNumberStatusDto>> Process_DeleteLotNumberStatus(Guid lotNumberStatusId);
}
