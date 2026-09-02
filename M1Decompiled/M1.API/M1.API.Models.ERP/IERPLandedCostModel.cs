using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPLandedCostModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all LandedCosts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LandedCosts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllLandedCosts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving LandedCost information based on the specified LandedCost Unique Id.
	/// </summary>
	/// <param name="landedCostId">The Unique Id of the LandedCost.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetLandedCost(Guid landedCostId);

	/// <summary>
	/// Validates the PUT request for creating or updating LandedCost information based on the specified LandedCost.
	/// </summary>
	/// <param name="landedCost">The LandedCost details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutLandedCost(ERPLandedCostDto landedCost);

	/// <summary>
	/// Processes the request to retrieve all LandedCosts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LandedCosts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LandedCosts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPLandedCostDto>>> Process_GetAllLandedCosts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific LandedCost.
	/// </summary>
	/// <param name="landedCostId">The Unique Id of the LandedCost to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the LandedCost DTO.</returns>
	Task<ERPResponseMessageDto<ERPLandedCostDto>> Process_GetLandedCost(Guid landedCostId);

	/// <summary>
	/// Processes the creating or updating of a LandedCost record.
	/// </summary>
	/// <param name="landedCost">The LandedCost data transfer object (DTO) containing the details of the LandedCost to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the LandedCost details.</returns>
	Task<ERPResponseMessageDto<ERPLandedCostDto>> Process_PutLandedCost(ERPLandedCostDto landedCost);

	/// <summary>
	/// Validates the request for deleting a LandedCost record.
	/// </summary>
	/// <param name="landedCostId">The Unique Id of the LandedCost.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteLandedCost(Guid landedCostId);

	/// <summary>
	/// Processes the request to delete a LandedCost record.
	/// </summary>
	/// <param name="landedCostId">The Unique Id of the LandedCost.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPLandedCostDto>> Process_DeleteLandedCost(Guid landedCostId);
}
