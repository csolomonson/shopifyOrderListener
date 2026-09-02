using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPMRPSupplyModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all MRPSupply with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MRPSupply to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllMRPSupply(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving MRPSupply information based on the specified MRPSupply Unique Id.
	/// </summary>
	/// <param name="mRPSupplyId">The Unique Id of the MRPSupply.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetMRPSupply(Guid mRPSupplyId);

	/// <summary>
	/// Validates the PUT request for creating or updating MRPSupply information based on the specified MRPSupply.
	/// </summary>
	/// <param name="mRPSupply">The MRPSupply details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutMRPSupply(ERPMRPSupplyDto mRPSupply);

	/// <summary>
	/// Processes the request to retrieve all MRPSupply with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MRPSupply to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MRPSupply DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPMRPSupplyDto>>> Process_GetAllMRPSupply(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific MRPSupply.
	/// </summary>
	/// <param name="mRPSupplyId">The Unique Id of the MRPSupply to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the MRPSupply DTO.</returns>
	Task<ERPResponseMessageDto<ERPMRPSupplyDto>> Process_GetMRPSupply(Guid mRPSupplyId);

	/// <summary>
	/// Processes the creating or updating of a MRPSupply record.
	/// </summary>
	/// <param name="mRPSupply">The MRPSupply data transfer object (DTO) containing the details of the MRPSupply to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the MRPSupply details.</returns>
	Task<ERPResponseMessageDto<ERPMRPSupplyDto>> Process_PutMRPSupply(ERPMRPSupplyDto mRPSupply);

	/// <summary>
	/// Validates the request for deleting a MRPSupply record.
	/// </summary>
	/// <param name="mRPSupplyId">The Unique Id of the MRPSupply.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteMRPSupply(Guid mRPSupplyId);

	/// <summary>
	/// Processes the request to delete a MRPSupply record.
	/// </summary>
	/// <param name="mRPSupplyId">The Unique Id of the MRPSupply.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPMRPSupplyDto>> Process_DeleteMRPSupply(Guid mRPSupplyId);
}
