using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPLandedCostChargeModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all LandedCostCharges with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LandedCostCharges to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllLandedCostCharges(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving LandedCostCharge information based on the specified LandedCostCharge Unique Id.
	/// </summary>
	/// <param name="landedCostChargeId">The Unique Id of the LandedCostCharge.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetLandedCostCharge(Guid landedCostChargeId);

	/// <summary>
	/// Validates the PUT request for creating or updating LandedCostCharge information based on the specified LandedCostCharge.
	/// </summary>
	/// <param name="landedCostCharge">The LandedCostCharge details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutLandedCostCharge(ERPLandedCostChargeDto landedCostCharge);

	/// <summary>
	/// Processes the request to retrieve all LandedCostCharges with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LandedCostCharges to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LandedCostCharges DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPLandedCostChargeDto>>> Process_GetAllLandedCostCharges(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific LandedCostCharge.
	/// </summary>
	/// <param name="landedCostChargeId">The Unique Id of the LandedCostCharge to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the LandedCostCharge DTO.</returns>
	Task<ERPResponseMessageDto<ERPLandedCostChargeDto>> Process_GetLandedCostCharge(Guid landedCostChargeId);

	/// <summary>
	/// Processes the creating or updating of a LandedCostCharge record.
	/// </summary>
	/// <param name="landedCostCharge">The LandedCostCharge data transfer object (DTO) containing the details of the LandedCostCharge to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the LandedCostCharge details.</returns>
	Task<ERPResponseMessageDto<ERPLandedCostChargeDto>> Process_PutLandedCostCharge(ERPLandedCostChargeDto landedCostCharge);

	/// <summary>
	/// Validates the request for deleting a LandedCostCharge record.
	/// </summary>
	/// <param name="landedCostChargeId">The Unique Id of the LandedCostCharge.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteLandedCostCharge(Guid landedCostChargeId);

	/// <summary>
	/// Processes the request to delete a LandedCostCharge record.
	/// </summary>
	/// <param name="landedCostChargeId">The Unique Id of the LandedCostCharge.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPLandedCostChargeDto>> Process_DeleteLandedCostCharge(Guid landedCostChargeId);
}
