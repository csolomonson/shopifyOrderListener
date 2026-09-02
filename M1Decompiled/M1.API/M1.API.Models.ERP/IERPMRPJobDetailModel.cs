using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPMRPJobDetailModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all MRPJobDetails with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MRPJobDetails to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllMRPJobDetails(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving MRPJobDetail information based on the specified MRPJobDetail Unique Id.
	/// </summary>
	/// <param name="mRPJobDetailId">The Unique Id of the MRPJobDetail.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetMRPJobDetail(Guid mRPJobDetailId);

	/// <summary>
	/// Validates the PUT request for creating or updating MRPJobDetail information based on the specified MRPJobDetail.
	/// </summary>
	/// <param name="mRPJobDetail">The MRPJobDetail details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutMRPJobDetail(ERPMRPJobDetailDto mRPJobDetail);

	/// <summary>
	/// Processes the request to retrieve all MRPJobDetails with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MRPJobDetails to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MRPJobDetails DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPMRPJobDetailDto>>> Process_GetAllMRPJobDetails(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific MRPJobDetail.
	/// </summary>
	/// <param name="mRPJobDetailId">The Unique Id of the MRPJobDetail to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the MRPJobDetail DTO.</returns>
	Task<ERPResponseMessageDto<ERPMRPJobDetailDto>> Process_GetMRPJobDetail(Guid mRPJobDetailId);

	/// <summary>
	/// Processes the creating or updating of a MRPJobDetail record.
	/// </summary>
	/// <param name="mRPJobDetail">The MRPJobDetail data transfer object (DTO) containing the details of the MRPJobDetail to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the MRPJobDetail details.</returns>
	Task<ERPResponseMessageDto<ERPMRPJobDetailDto>> Process_PutMRPJobDetail(ERPMRPJobDetailDto mRPJobDetail);

	/// <summary>
	/// Validates the request for deleting a MRPJobDetail record.
	/// </summary>
	/// <param name="mRPJobDetailId">The Unique Id of the MRPJobDetail.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteMRPJobDetail(Guid mRPJobDetailId);

	/// <summary>
	/// Processes the request to delete a MRPJobDetail record.
	/// </summary>
	/// <param name="mRPJobDetailId">The Unique Id of the MRPJobDetail.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPMRPJobDetailDto>> Process_DeleteMRPJobDetail(Guid mRPJobDetailId);
}
