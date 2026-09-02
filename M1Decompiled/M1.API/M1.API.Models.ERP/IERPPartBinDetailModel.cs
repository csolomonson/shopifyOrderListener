using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartBinDetailModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartBinDetails with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartBinDetails to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartBinDetails(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartBinDetail information based on the specified PartBinDetail Unique Id.
	/// </summary>
	/// <param name="partBinDetailId">The Unique Id of the PartBinDetail.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartBinDetail(Guid partBinDetailId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartBinDetail information based on the specified PartBinDetail.
	/// </summary>
	/// <param name="partBinDetail">The PartBinDetail details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartBinDetail(ERPPartBinDetailDto partBinDetail);

	/// <summary>
	/// Processes the request to retrieve all PartBinDetails with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartBinDetails to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartBinDetails DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartBinDetailDto>>> Process_GetAllPartBinDetails(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartBinDetail.
	/// </summary>
	/// <param name="partBinDetailId">The Unique Id of the PartBinDetail to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartBinDetail DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartBinDetailDto>> Process_GetPartBinDetail(Guid partBinDetailId);

	/// <summary>
	/// Processes the creating or updating of a PartBinDetail record.
	/// </summary>
	/// <param name="partBinDetail">The PartBinDetail data transfer object (DTO) containing the details of the PartBinDetail to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartBinDetail details.</returns>
	Task<ERPResponseMessageDto<ERPPartBinDetailDto>> Process_PutPartBinDetail(ERPPartBinDetailDto partBinDetail);

	/// <summary>
	/// Validates the request for deleting a PartBinDetail record.
	/// </summary>
	/// <param name="partBinDetailId">The Unique Id of the PartBinDetail.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartBinDetail(Guid partBinDetailId);

	/// <summary>
	/// Processes the request to delete a PartBinDetail record.
	/// </summary>
	/// <param name="partBinDetailId">The Unique Id of the PartBinDetail.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartBinDetailDto>> Process_DeletePartBinDetail(Guid partBinDetailId);
}
