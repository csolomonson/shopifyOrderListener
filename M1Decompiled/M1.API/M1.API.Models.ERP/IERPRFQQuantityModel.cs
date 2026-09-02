using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPRFQQuantityModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all RFQQuantities with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RFQQuantities to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllRFQQuantities(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving RFQQuantity information based on the specified RFQQuantity Unique Id.
	/// </summary>
	/// <param name="rFQQuantityId">The Unique Id of the RFQQuantity.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetRFQQuantity(Guid rFQQuantityId);

	/// <summary>
	/// Validates the PUT request for creating or updating RFQQuantity information based on the specified RFQQuantity.
	/// </summary>
	/// <param name="rFQQuantity">The RFQQuantity details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutRFQQuantity(ERPRFQQuantityDto rFQQuantity);

	/// <summary>
	/// Processes the request to retrieve all RFQQuantities with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RFQQuantities to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RFQQuantities DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPRFQQuantityDto>>> Process_GetAllRFQQuantities(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific RFQQuantity.
	/// </summary>
	/// <param name="rFQQuantityId">The Unique Id of the RFQQuantity to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the RFQQuantity DTO.</returns>
	Task<ERPResponseMessageDto<ERPRFQQuantityDto>> Process_GetRFQQuantity(Guid rFQQuantityId);

	/// <summary>
	/// Processes the creating or updating of a RFQQuantity record.
	/// </summary>
	/// <param name="rFQQuantity">The RFQQuantity data transfer object (DTO) containing the details of the RFQQuantity to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the RFQQuantity details.</returns>
	Task<ERPResponseMessageDto<ERPRFQQuantityDto>> Process_PutRFQQuantity(ERPRFQQuantityDto rFQQuantity);

	/// <summary>
	/// Validates the request for deleting a RFQQuantity record.
	/// </summary>
	/// <param name="rFQQuantityId">The Unique Id of the RFQQuantity.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteRFQQuantity(Guid rFQQuantityId);

	/// <summary>
	/// Processes the request to delete a RFQQuantity record.
	/// </summary>
	/// <param name="rFQQuantityId">The Unique Id of the RFQQuantity.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPRFQQuantityDto>> Process_DeleteRFQQuantity(Guid rFQQuantityId);
}
