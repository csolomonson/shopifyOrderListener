using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAPPaymentLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all APPaymentLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APPaymentLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAPPaymentLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving APPaymentLine information based on the specified APPaymentLine Unique Id.
	/// </summary>
	/// <param name="aPPaymentLineId">The Unique Id of the APPaymentLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAPPaymentLine(Guid aPPaymentLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating APPaymentLine information based on the specified APPaymentLine.
	/// </summary>
	/// <param name="aPPaymentLine">The APPaymentLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutAPPaymentLine(ERPAPPaymentLineDto aPPaymentLine);

	/// <summary>
	/// Processes the request to retrieve all APPaymentLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APPaymentLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of APPaymentLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAPPaymentLineDto>>> Process_GetAllAPPaymentLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific APPaymentLine.
	/// </summary>
	/// <param name="aPPaymentLineId">The Unique Id of the APPaymentLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the APPaymentLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPAPPaymentLineDto>> Process_GetAPPaymentLine(Guid aPPaymentLineId);

	/// <summary>
	/// Processes the creating or updating of a APPaymentLine record.
	/// </summary>
	/// <param name="aPPaymentLine">The APPaymentLine data transfer object (DTO) containing the details of the APPaymentLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the APPaymentLine details.</returns>
	Task<ERPResponseMessageDto<ERPAPPaymentLineDto>> Process_PutAPPaymentLine(ERPAPPaymentLineDto aPPaymentLine);

	/// <summary>
	/// Validates the request for deleting a APPaymentLine record.
	/// </summary>
	/// <param name="aPPaymentLineId">The Unique Id of the APPaymentLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteAPPaymentLine(Guid aPPaymentLineId);

	/// <summary>
	/// Processes the request to delete a APPaymentLine record.
	/// </summary>
	/// <param name="aPPaymentLineId">The Unique Id of the APPaymentLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPAPPaymentLineDto>> Process_DeleteAPPaymentLine(Guid aPPaymentLineId);
}
