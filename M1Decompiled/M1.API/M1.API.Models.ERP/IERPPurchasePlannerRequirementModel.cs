using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPurchasePlannerRequirementModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PurchasePlannerRequirements with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchasePlannerRequirements to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPurchasePlannerRequirements(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PurchasePlannerRequirement information based on the specified PurchasePlannerRequirement Unique Id.
	/// </summary>
	/// <param name="purchasePlannerRequirementId">The Unique Id of the PurchasePlannerRequirement.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPurchasePlannerRequirement(Guid purchasePlannerRequirementId);

	/// <summary>
	/// Validates the PUT request for creating or updating PurchasePlannerRequirement information based on the specified PurchasePlannerRequirement.
	/// </summary>
	/// <param name="purchasePlannerRequirement">The PurchasePlannerRequirement details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPurchasePlannerRequirement(ERPPurchasePlannerRequirementDto purchasePlannerRequirement);

	/// <summary>
	/// Processes the request to retrieve all PurchasePlannerRequirements with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchasePlannerRequirements to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchasePlannerRequirements DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPurchasePlannerRequirementDto>>> Process_GetAllPurchasePlannerRequirements(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PurchasePlannerRequirement.
	/// </summary>
	/// <param name="purchasePlannerRequirementId">The Unique Id of the PurchasePlannerRequirement to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PurchasePlannerRequirement DTO.</returns>
	Task<ERPResponseMessageDto<ERPPurchasePlannerRequirementDto>> Process_GetPurchasePlannerRequirement(Guid purchasePlannerRequirementId);

	/// <summary>
	/// Processes the creating or updating of a PurchasePlannerRequirement record.
	/// </summary>
	/// <param name="purchasePlannerRequirement">The PurchasePlannerRequirement data transfer object (DTO) containing the details of the PurchasePlannerRequirement to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PurchasePlannerRequirement details.</returns>
	Task<ERPResponseMessageDto<ERPPurchasePlannerRequirementDto>> Process_PutPurchasePlannerRequirement(ERPPurchasePlannerRequirementDto purchasePlannerRequirement);

	/// <summary>
	/// Validates the request for deleting a PurchasePlannerRequirement record.
	/// </summary>
	/// <param name="purchasePlannerRequirementId">The Unique Id of the PurchasePlannerRequirement.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePurchasePlannerRequirement(Guid purchasePlannerRequirementId);

	/// <summary>
	/// Processes the request to delete a PurchasePlannerRequirement record.
	/// </summary>
	/// <param name="purchasePlannerRequirementId">The Unique Id of the PurchasePlannerRequirement.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPurchasePlannerRequirementDto>> Process_DeletePurchasePlannerRequirement(Guid purchasePlannerRequirementId);
}
