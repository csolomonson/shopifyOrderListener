using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartTransactionCostModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartTransactionCosts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartTransactionCosts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartTransactionCosts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartTransactionCost information based on the specified PartTransactionCost Unique Id.
	/// </summary>
	/// <param name="partTransactionCostId">The Unique Id of the PartTransactionCost.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartTransactionCost(Guid partTransactionCostId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartTransactionCost information based on the specified PartTransactionCost.
	/// </summary>
	/// <param name="partTransactionCost">The PartTransactionCost details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartTransactionCost(ERPPartTransactionCostDto partTransactionCost);

	/// <summary>
	/// Processes the request to retrieve all PartTransactionCosts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartTransactionCosts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartTransactionCosts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartTransactionCostDto>>> Process_GetAllPartTransactionCosts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartTransactionCost.
	/// </summary>
	/// <param name="partTransactionCostId">The Unique Id of the PartTransactionCost to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartTransactionCost DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartTransactionCostDto>> Process_GetPartTransactionCost(Guid partTransactionCostId);

	/// <summary>
	/// Processes the creating or updating of a PartTransactionCost record.
	/// </summary>
	/// <param name="partTransactionCost">The PartTransactionCost data transfer object (DTO) containing the details of the PartTransactionCost to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartTransactionCost details.</returns>
	Task<ERPResponseMessageDto<ERPPartTransactionCostDto>> Process_PutPartTransactionCost(ERPPartTransactionCostDto partTransactionCost);

	/// <summary>
	/// Validates the request for deleting a PartTransactionCost record.
	/// </summary>
	/// <param name="partTransactionCostId">The Unique Id of the PartTransactionCost.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartTransactionCost(Guid partTransactionCostId);

	/// <summary>
	/// Processes the request to delete a PartTransactionCost record.
	/// </summary>
	/// <param name="partTransactionCostId">The Unique Id of the PartTransactionCost.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartTransactionCostDto>> Process_DeletePartTransactionCost(Guid partTransactionCostId);
}
