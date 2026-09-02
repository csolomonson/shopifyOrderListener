using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPFreightPackageLinkModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all FreightPackageLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FreightPackageLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllFreightPackageLinks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving FreightPackageLink information based on the specified FreightPackageLink Unique Id.
	/// </summary>
	/// <param name="freightPackageLinkId">The Unique Id of the FreightPackageLink.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetFreightPackageLink(Guid freightPackageLinkId);

	/// <summary>
	/// Validates the PUT request for creating or updating FreightPackageLink information based on the specified FreightPackageLink.
	/// </summary>
	/// <param name="freightPackageLink">The FreightPackageLink details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutFreightPackageLink(ERPFreightPackageLinkDto freightPackageLink);

	/// <summary>
	/// Processes the request to retrieve all FreightPackageLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FreightPackageLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of FreightPackageLinks DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPFreightPackageLinkDto>>> Process_GetAllFreightPackageLinks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific FreightPackageLink.
	/// </summary>
	/// <param name="freightPackageLinkId">The Unique Id of the FreightPackageLink to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the FreightPackageLink DTO.</returns>
	Task<ERPResponseMessageDto<ERPFreightPackageLinkDto>> Process_GetFreightPackageLink(Guid freightPackageLinkId);

	/// <summary>
	/// Processes the creating or updating of a FreightPackageLink record.
	/// </summary>
	/// <param name="freightPackageLink">The FreightPackageLink data transfer object (DTO) containing the details of the FreightPackageLink to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the FreightPackageLink details.</returns>
	Task<ERPResponseMessageDto<ERPFreightPackageLinkDto>> Process_PutFreightPackageLink(ERPFreightPackageLinkDto freightPackageLink);

	/// <summary>
	/// Validates the request for deleting a FreightPackageLink record.
	/// </summary>
	/// <param name="freightPackageLinkId">The Unique Id of the FreightPackageLink.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteFreightPackageLink(Guid freightPackageLinkId);

	/// <summary>
	/// Processes the request to delete a FreightPackageLink record.
	/// </summary>
	/// <param name="freightPackageLinkId">The Unique Id of the FreightPackageLink.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPFreightPackageLinkDto>> Process_DeleteFreightPackageLink(Guid freightPackageLinkId);
}
