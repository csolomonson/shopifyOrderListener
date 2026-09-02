using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPFreightPackageModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all FreightPackages with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FreightPackages to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllFreightPackages(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving FreightPackage information based on the specified FreightPackage Unique Id.
	/// </summary>
	/// <param name="freightPackageId">The Unique Id of the FreightPackage.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetFreightPackage(Guid freightPackageId);

	/// <summary>
	/// Validates the PUT request for creating or updating FreightPackage information based on the specified FreightPackage.
	/// </summary>
	/// <param name="freightPackage">The FreightPackage details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutFreightPackage(ERPFreightPackageDto freightPackage);

	/// <summary>
	/// Processes the request to retrieve all FreightPackages with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FreightPackages to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of FreightPackages DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPFreightPackageDto>>> Process_GetAllFreightPackages(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific FreightPackage.
	/// </summary>
	/// <param name="freightPackageId">The Unique Id of the FreightPackage to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the FreightPackage DTO.</returns>
	Task<ERPResponseMessageDto<ERPFreightPackageDto>> Process_GetFreightPackage(Guid freightPackageId);

	/// <summary>
	/// Processes the creating or updating of a FreightPackage record.
	/// </summary>
	/// <param name="freightPackage">The FreightPackage data transfer object (DTO) containing the details of the FreightPackage to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the FreightPackage details.</returns>
	Task<ERPResponseMessageDto<ERPFreightPackageDto>> Process_PutFreightPackage(ERPFreightPackageDto freightPackage);

	/// <summary>
	/// Validates the request for deleting a FreightPackage record.
	/// </summary>
	/// <param name="freightPackageId">The Unique Id of the FreightPackage.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteFreightPackage(Guid freightPackageId);

	/// <summary>
	/// Processes the request to delete a FreightPackage record.
	/// </summary>
	/// <param name="freightPackageId">The Unique Id of the FreightPackage.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPFreightPackageDto>> Process_DeleteFreightPackage(Guid freightPackageId);
}
