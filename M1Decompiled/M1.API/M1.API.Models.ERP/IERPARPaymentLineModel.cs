using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPARPaymentLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ARPaymentLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARPaymentLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllARPaymentLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ARPaymentLine information based on the specified ARPaymentLine Unique Id.
	/// </summary>
	/// <param name="aRPaymentLineId">The Unique Id of the ARPaymentLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetARPaymentLine(Guid aRPaymentLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating ARPaymentLine information based on the specified ARPaymentLine.
	/// </summary>
	/// <param name="aRPaymentLine">The ARPaymentLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutARPaymentLine(ERPARPaymentLineDto aRPaymentLine);

	/// <summary>
	/// Processes the request to retrieve all ARPaymentLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARPaymentLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ARPaymentLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPARPaymentLineDto>>> Process_GetAllARPaymentLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ARPaymentLine.
	/// </summary>
	/// <param name="aRPaymentLineId">The Unique Id of the ARPaymentLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ARPaymentLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPARPaymentLineDto>> Process_GetARPaymentLine(Guid aRPaymentLineId);

	/// <summary>
	/// Processes the creating or updating of a ARPaymentLine record.
	/// </summary>
	/// <param name="aRPaymentLine">The ARPaymentLine data transfer object (DTO) containing the details of the ARPaymentLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ARPaymentLine details.</returns>
	Task<ERPResponseMessageDto<ERPARPaymentLineDto>> Process_PutARPaymentLine(ERPARPaymentLineDto aRPaymentLine);

	/// <summary>
	/// Validates the request for deleting a ARPaymentLine record.
	/// </summary>
	/// <param name="aRPaymentLineId">The Unique Id of the ARPaymentLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteARPaymentLine(Guid aRPaymentLineId);

	/// <summary>
	/// Processes the request to delete a ARPaymentLine record.
	/// </summary>
	/// <param name="aRPaymentLineId">The Unique Id of the ARPaymentLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPARPaymentLineDto>> Process_DeleteARPaymentLine(Guid aRPaymentLineId);
}
