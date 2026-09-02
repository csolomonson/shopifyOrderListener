using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPurchasePlannerSessionModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PurchasePlannerSessions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchasePlannerSessions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPurchasePlannerSessions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PurchasePlannerSession information based on the specified PurchasePlannerSession Unique Id.
	/// </summary>
	/// <param name="purchasePlannerSessionId">The Unique Id of the PurchasePlannerSession.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPurchasePlannerSession(Guid purchasePlannerSessionId);

	/// <summary>
	/// Validates the PUT request for creating or updating PurchasePlannerSession information based on the specified PurchasePlannerSession.
	/// </summary>
	/// <param name="purchasePlannerSession">The PurchasePlannerSession details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPurchasePlannerSession(ERPPurchasePlannerSessionDto purchasePlannerSession);

	/// <summary>
	/// Processes the request to retrieve all PurchasePlannerSessions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchasePlannerSessions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchasePlannerSessions DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPurchasePlannerSessionDto>>> Process_GetAllPurchasePlannerSessions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PurchasePlannerSession.
	/// </summary>
	/// <param name="purchasePlannerSessionId">The Unique Id of the PurchasePlannerSession to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PurchasePlannerSession DTO.</returns>
	Task<ERPResponseMessageDto<ERPPurchasePlannerSessionDto>> Process_GetPurchasePlannerSession(Guid purchasePlannerSessionId);

	/// <summary>
	/// Processes the creating or updating of a PurchasePlannerSession record.
	/// </summary>
	/// <param name="purchasePlannerSession">The PurchasePlannerSession data transfer object (DTO) containing the details of the PurchasePlannerSession to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PurchasePlannerSession details.</returns>
	Task<ERPResponseMessageDto<ERPPurchasePlannerSessionDto>> Process_PutPurchasePlannerSession(ERPPurchasePlannerSessionDto purchasePlannerSession);

	/// <summary>
	/// Validates the request for deleting a PurchasePlannerSession record.
	/// </summary>
	/// <param name="purchasePlannerSessionId">The Unique Id of the PurchasePlannerSession.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePurchasePlannerSession(Guid purchasePlannerSessionId);

	/// <summary>
	/// Processes the request to delete a PurchasePlannerSession record.
	/// </summary>
	/// <param name="purchasePlannerSessionId">The Unique Id of the PurchasePlannerSession.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPurchasePlannerSessionDto>> Process_DeletePurchasePlannerSession(Guid purchasePlannerSessionId);
}
