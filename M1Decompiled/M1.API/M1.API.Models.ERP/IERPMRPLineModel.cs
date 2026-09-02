using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPMRPLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all MRPLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MRPLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllMRPLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving MRPLine information based on the specified MRPLine Unique Id.
	/// </summary>
	/// <param name="mRPLineId">The Unique Id of the MRPLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetMRPLine(Guid mRPLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating MRPLine information based on the specified MRPLine.
	/// </summary>
	/// <param name="mRPLine">The MRPLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutMRPLine(ERPMRPLineDto mRPLine);

	/// <summary>
	/// Processes the request to retrieve all MRPLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MRPLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MRPLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPMRPLineDto>>> Process_GetAllMRPLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific MRPLine.
	/// </summary>
	/// <param name="mRPLineId">The Unique Id of the MRPLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the MRPLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPMRPLineDto>> Process_GetMRPLine(Guid mRPLineId);

	/// <summary>
	/// Processes the creating or updating of a MRPLine record.
	/// </summary>
	/// <param name="mRPLine">The MRPLine data transfer object (DTO) containing the details of the MRPLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the MRPLine details.</returns>
	Task<ERPResponseMessageDto<ERPMRPLineDto>> Process_PutMRPLine(ERPMRPLineDto mRPLine);

	/// <summary>
	/// Validates the request for deleting a MRPLine record.
	/// </summary>
	/// <param name="mRPLineId">The Unique Id of the MRPLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteMRPLine(Guid mRPLineId);

	/// <summary>
	/// Processes the request to delete a MRPLine record.
	/// </summary>
	/// <param name="mRPLineId">The Unique Id of the MRPLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPMRPLineDto>> Process_DeleteMRPLine(Guid mRPLineId);
}
