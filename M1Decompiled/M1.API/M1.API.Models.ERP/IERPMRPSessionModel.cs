using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPMRPSessionModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all MRPSessions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MRPSessions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllMRPSessions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving MRPSession information based on the specified MRPSession Unique Id.
	/// </summary>
	/// <param name="mRPSessionId">The Unique Id of the MRPSession.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetMRPSession(Guid mRPSessionId);

	/// <summary>
	/// Validates the PUT request for creating or updating MRPSession information based on the specified MRPSession.
	/// </summary>
	/// <param name="mRPSession">The MRPSession details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutMRPSession(ERPMRPSessionDto mRPSession);

	/// <summary>
	/// Processes the request to retrieve all MRPSessions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MRPSessions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MRPSessions DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPMRPSessionDto>>> Process_GetAllMRPSessions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific MRPSession.
	/// </summary>
	/// <param name="mRPSessionId">The Unique Id of the MRPSession to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the MRPSession DTO.</returns>
	Task<ERPResponseMessageDto<ERPMRPSessionDto>> Process_GetMRPSession(Guid mRPSessionId);

	/// <summary>
	/// Processes the creating or updating of a MRPSession record.
	/// </summary>
	/// <param name="mRPSession">The MRPSession data transfer object (DTO) containing the details of the MRPSession to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the MRPSession details.</returns>
	Task<ERPResponseMessageDto<ERPMRPSessionDto>> Process_PutMRPSession(ERPMRPSessionDto mRPSession);

	/// <summary>
	/// Validates the request for deleting a MRPSession record.
	/// </summary>
	/// <param name="mRPSessionId">The Unique Id of the MRPSession.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteMRPSession(Guid mRPSessionId);

	/// <summary>
	/// Processes the request to delete a MRPSession record.
	/// </summary>
	/// <param name="mRPSessionId">The Unique Id of the MRPSession.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPMRPSessionDto>> Process_DeleteMRPSession(Guid mRPSessionId);
}
