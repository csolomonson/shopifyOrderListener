using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPRFQLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all RFQLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RFQLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllRFQLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving RFQLine information based on the specified RFQLine Unique Id.
	/// </summary>
	/// <param name="rFQLineId">The Unique Id of the RFQLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetRFQLine(Guid rFQLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating RFQLine information based on the specified RFQLine.
	/// </summary>
	/// <param name="rFQLine">The RFQLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutRFQLine(ERPRFQLineDto rFQLine);

	/// <summary>
	/// Processes the request to retrieve all RFQLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RFQLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RFQLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPRFQLineDto>>> Process_GetAllRFQLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific RFQLine.
	/// </summary>
	/// <param name="rFQLineId">The Unique Id of the RFQLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the RFQLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPRFQLineDto>> Process_GetRFQLine(Guid rFQLineId);

	/// <summary>
	/// Processes the creating or updating of a RFQLine record.
	/// </summary>
	/// <param name="rFQLine">The RFQLine data transfer object (DTO) containing the details of the RFQLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the RFQLine details.</returns>
	Task<ERPResponseMessageDto<ERPRFQLineDto>> Process_PutRFQLine(ERPRFQLineDto rFQLine);

	/// <summary>
	/// Validates the request for deleting a RFQLine record.
	/// </summary>
	/// <param name="rFQLineId">The Unique Id of the RFQLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteRFQLine(Guid rFQLineId);

	/// <summary>
	/// Processes the request to delete a RFQLine record.
	/// </summary>
	/// <param name="rFQLineId">The Unique Id of the RFQLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPRFQLineDto>> Process_DeleteRFQLine(Guid rFQLineId);
}
