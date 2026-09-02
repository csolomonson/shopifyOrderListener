using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPRFQModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all RFQs with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RFQs to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllRFQs(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving RFQ information based on the specified RFQ Unique Id.
	/// </summary>
	/// <param name="rFQId">The Unique Id of the RFQ.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetRFQ(Guid rFQId);

	/// <summary>
	/// Validates the PUT request for creating or updating RFQ information based on the specified RFQ.
	/// </summary>
	/// <param name="rFQ">The RFQ details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutRFQ(ERPRFQDto rFQ);

	/// <summary>
	/// Processes the request to retrieve all RFQs with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RFQs to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RFQs DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPRFQDto>>> Process_GetAllRFQs(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific RFQ.
	/// </summary>
	/// <param name="rFQId">The Unique Id of the RFQ to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the RFQ DTO.</returns>
	Task<ERPResponseMessageDto<ERPRFQDto>> Process_GetRFQ(Guid rFQId);

	/// <summary>
	/// Processes the creating or updating of a RFQ record.
	/// </summary>
	/// <param name="rFQ">The RFQ data transfer object (DTO) containing the details of the RFQ to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the RFQ details.</returns>
	Task<ERPResponseMessageDto<ERPRFQDto>> Process_PutRFQ(ERPRFQDto rFQ);

	/// <summary>
	/// Validates the request for deleting a RFQ record.
	/// </summary>
	/// <param name="rFQId">The Unique Id of the RFQ.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteRFQ(Guid rFQId);

	/// <summary>
	/// Processes the request to delete a RFQ record.
	/// </summary>
	/// <param name="rFQId">The Unique Id of the RFQ.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPRFQDto>> Process_DeleteRFQ(Guid rFQId);
}
