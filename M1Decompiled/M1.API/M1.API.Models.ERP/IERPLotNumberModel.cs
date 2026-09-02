using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPLotNumberModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all LotNumbers with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LotNumbers to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllLotNumbers(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving LotNumber information based on the specified LotNumber Unique Id.
	/// </summary>
	/// <param name="lotNumberId">The Unique Id of the LotNumber.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetLotNumber(Guid lotNumberId);

	/// <summary>
	/// Validates the PUT request for creating or updating LotNumber information based on the specified LotNumber.
	/// </summary>
	/// <param name="lotNumber">The LotNumber details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutLotNumber(ERPLotNumberDto lotNumber);

	/// <summary>
	/// Processes the request to retrieve all LotNumbers with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LotNumbers to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LotNumbers DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPLotNumberDto>>> Process_GetAllLotNumbers(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific LotNumber.
	/// </summary>
	/// <param name="lotNumberId">The Unique Id of the LotNumber to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the LotNumber DTO.</returns>
	Task<ERPResponseMessageDto<ERPLotNumberDto>> Process_GetLotNumber(Guid lotNumberId);

	/// <summary>
	/// Processes the creating or updating of a LotNumber record.
	/// </summary>
	/// <param name="lotNumber">The LotNumber data transfer object (DTO) containing the details of the LotNumber to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the LotNumber details.</returns>
	Task<ERPResponseMessageDto<ERPLotNumberDto>> Process_PutLotNumber(ERPLotNumberDto lotNumber);

	/// <summary>
	/// Validates the request for deleting a LotNumber record.
	/// </summary>
	/// <param name="lotNumberId">The Unique Id of the LotNumber.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteLotNumber(Guid lotNumberId);

	/// <summary>
	/// Processes the request to delete a LotNumber record.
	/// </summary>
	/// <param name="lotNumberId">The Unique Id of the LotNumber.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPLotNumberDto>> Process_DeleteLotNumber(Guid lotNumberId);
}
