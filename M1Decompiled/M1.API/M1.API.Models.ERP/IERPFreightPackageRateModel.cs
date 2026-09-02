using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPFreightPackageRateModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all FreightPackageRates with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FreightPackageRates to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllFreightPackageRates(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving FreightPackageRate information based on the specified FreightPackageRate Unique Id.
	/// </summary>
	/// <param name="freightPackageRateId">The Unique Id of the FreightPackageRate.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetFreightPackageRate(Guid freightPackageRateId);

	/// <summary>
	/// Validates the PUT request for creating or updating FreightPackageRate information based on the specified FreightPackageRate.
	/// </summary>
	/// <param name="freightPackageRate">The FreightPackageRate details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutFreightPackageRate(ERPFreightPackageRateDto freightPackageRate);

	/// <summary>
	/// Processes the request to retrieve all FreightPackageRates with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FreightPackageRates to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of FreightPackageRates DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPFreightPackageRateDto>>> Process_GetAllFreightPackageRates(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific FreightPackageRate.
	/// </summary>
	/// <param name="freightPackageRateId">The Unique Id of the FreightPackageRate to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the FreightPackageRate DTO.</returns>
	Task<ERPResponseMessageDto<ERPFreightPackageRateDto>> Process_GetFreightPackageRate(Guid freightPackageRateId);

	/// <summary>
	/// Processes the creating or updating of a FreightPackageRate record.
	/// </summary>
	/// <param name="freightPackageRate">The FreightPackageRate data transfer object (DTO) containing the details of the FreightPackageRate to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the FreightPackageRate details.</returns>
	Task<ERPResponseMessageDto<ERPFreightPackageRateDto>> Process_PutFreightPackageRate(ERPFreightPackageRateDto freightPackageRate);

	/// <summary>
	/// Validates the request for deleting a FreightPackageRate record.
	/// </summary>
	/// <param name="freightPackageRateId">The Unique Id of the FreightPackageRate.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteFreightPackageRate(Guid freightPackageRateId);

	/// <summary>
	/// Processes the request to delete a FreightPackageRate record.
	/// </summary>
	/// <param name="freightPackageRateId">The Unique Id of the FreightPackageRate.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPFreightPackageRateDto>> Process_DeleteFreightPackageRate(Guid freightPackageRateId);
}
