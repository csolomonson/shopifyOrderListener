using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPMRPDemandModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all MRPDemands with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MRPDemands to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllMRPDemands(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving MRPDemand information based on the specified MRPDemand Unique Id.
	/// </summary>
	/// <param name="mRPDemandId">The Unique Id of the MRPDemand.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetMRPDemand(Guid mRPDemandId);

	/// <summary>
	/// Validates the PUT request for creating or updating MRPDemand information based on the specified MRPDemand.
	/// </summary>
	/// <param name="mRPDemand">The MRPDemand details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutMRPDemand(ERPMRPDemandDto mRPDemand);

	/// <summary>
	/// Processes the request to retrieve all MRPDemands with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MRPDemands to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MRPDemands DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPMRPDemandDto>>> Process_GetAllMRPDemands(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific MRPDemand.
	/// </summary>
	/// <param name="mRPDemandId">The Unique Id of the MRPDemand to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the MRPDemand DTO.</returns>
	Task<ERPResponseMessageDto<ERPMRPDemandDto>> Process_GetMRPDemand(Guid mRPDemandId);

	/// <summary>
	/// Processes the creating or updating of a MRPDemand record.
	/// </summary>
	/// <param name="mRPDemand">The MRPDemand data transfer object (DTO) containing the details of the MRPDemand to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the MRPDemand details.</returns>
	Task<ERPResponseMessageDto<ERPMRPDemandDto>> Process_PutMRPDemand(ERPMRPDemandDto mRPDemand);

	/// <summary>
	/// Validates the request for deleting a MRPDemand record.
	/// </summary>
	/// <param name="mRPDemandId">The Unique Id of the MRPDemand.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteMRPDemand(Guid mRPDemandId);

	/// <summary>
	/// Processes the request to delete a MRPDemand record.
	/// </summary>
	/// <param name="mRPDemandId">The Unique Id of the MRPDemand.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPMRPDemandDto>> Process_DeleteMRPDemand(Guid mRPDemandId);
}
