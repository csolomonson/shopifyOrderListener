using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartBinModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartBins with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartBins to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartBins(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartBin information based on the specified PartBin Unique Id.
	/// </summary>
	/// <param name="partBinId">The Unique Id of the PartBin.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartBin(Guid partBinId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartBin information based on the specified PartBin.
	/// </summary>
	/// <param name="partBin">The PartBin details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartBin(ERPPartBinDto partBin);

	/// <summary>
	/// Processes the request to retrieve all PartBins with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartBins to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartBins DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartBinDto>>> Process_GetAllPartBins(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartBin.
	/// </summary>
	/// <param name="partBinId">The Unique Id of the PartBin to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartBin DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartBinDto>> Process_GetPartBin(Guid partBinId);

	/// <summary>
	/// Processes the creating or updating of a PartBin record.
	/// </summary>
	/// <param name="partBin">The PartBin data transfer object (DTO) containing the details of the PartBin to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartBin details.</returns>
	Task<ERPResponseMessageDto<ERPPartBinDto>> Process_PutPartBin(ERPPartBinDto partBin);

	/// <summary>
	/// Validates the request for deleting a PartBin record.
	/// </summary>
	/// <param name="partBinId">The Unique Id of the PartBin.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartBin(Guid partBinId);

	/// <summary>
	/// Processes the request to delete a PartBin record.
	/// </summary>
	/// <param name="partBinId">The Unique Id of the PartBin.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartBinDto>> Process_DeletePartBin(Guid partBinId);
}
