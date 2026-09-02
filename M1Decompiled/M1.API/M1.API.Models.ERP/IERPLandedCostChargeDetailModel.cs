using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPLandedCostChargeDetailModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all LandedCostChargeDetails with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LandedCostChargeDetails to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllLandedCostChargeDetails(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving LandedCostChargeDetail information based on the specified LandedCostChargeDetail Unique Id.
	/// </summary>
	/// <param name="landedCostChargeDetailId">The Unique Id of the LandedCostChargeDetail.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetLandedCostChargeDetail(Guid landedCostChargeDetailId);

	/// <summary>
	/// Validates the PUT request for creating or updating LandedCostChargeDetail information based on the specified LandedCostChargeDetail.
	/// </summary>
	/// <param name="landedCostChargeDetail">The LandedCostChargeDetail details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutLandedCostChargeDetail(ERPLandedCostChargeDetailDto landedCostChargeDetail);

	/// <summary>
	/// Processes the request to retrieve all LandedCostChargeDetails with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LandedCostChargeDetails to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LandedCostChargeDetails DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPLandedCostChargeDetailDto>>> Process_GetAllLandedCostChargeDetails(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific LandedCostChargeDetail.
	/// </summary>
	/// <param name="landedCostChargeDetailId">The Unique Id of the LandedCostChargeDetail to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the LandedCostChargeDetail DTO.</returns>
	Task<ERPResponseMessageDto<ERPLandedCostChargeDetailDto>> Process_GetLandedCostChargeDetail(Guid landedCostChargeDetailId);

	/// <summary>
	/// Processes the creating or updating of a LandedCostChargeDetail record.
	/// </summary>
	/// <param name="landedCostChargeDetail">The LandedCostChargeDetail data transfer object (DTO) containing the details of the LandedCostChargeDetail to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the LandedCostChargeDetail details.</returns>
	Task<ERPResponseMessageDto<ERPLandedCostChargeDetailDto>> Process_PutLandedCostChargeDetail(ERPLandedCostChargeDetailDto landedCostChargeDetail);

	/// <summary>
	/// Validates the request for deleting a LandedCostChargeDetail record.
	/// </summary>
	/// <param name="landedCostChargeDetailId">The Unique Id of the LandedCostChargeDetail.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteLandedCostChargeDetail(Guid landedCostChargeDetailId);

	/// <summary>
	/// Processes the request to delete a LandedCostChargeDetail record.
	/// </summary>
	/// <param name="landedCostChargeDetailId">The Unique Id of the LandedCostChargeDetail.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPLandedCostChargeDetailDto>> Process_DeleteLandedCostChargeDetail(Guid landedCostChargeDetailId);
}
