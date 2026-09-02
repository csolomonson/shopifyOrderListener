using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPLaserCalculatorModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all LaserCalculators with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LaserCalculators to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllLaserCalculators(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving LaserCalculator information based on the specified LaserCalculator Unique Id.
	/// </summary>
	/// <param name="laserCalculatorId">The Unique Id of the LaserCalculator.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetLaserCalculator(Guid laserCalculatorId);

	/// <summary>
	/// Validates the PUT request for creating or updating LaserCalculator information based on the specified LaserCalculator.
	/// </summary>
	/// <param name="laserCalculator">The LaserCalculator details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutLaserCalculator(ERPLaserCalculatorDto laserCalculator);

	/// <summary>
	/// Processes the request to retrieve all LaserCalculators with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LaserCalculators to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LaserCalculators DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPLaserCalculatorDto>>> Process_GetAllLaserCalculators(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific LaserCalculator.
	/// </summary>
	/// <param name="laserCalculatorId">The Unique Id of the LaserCalculator to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the LaserCalculator DTO.</returns>
	Task<ERPResponseMessageDto<ERPLaserCalculatorDto>> Process_GetLaserCalculator(Guid laserCalculatorId);

	/// <summary>
	/// Processes the creating or updating of a LaserCalculator record.
	/// </summary>
	/// <param name="laserCalculator">The LaserCalculator data transfer object (DTO) containing the details of the LaserCalculator to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the LaserCalculator details.</returns>
	Task<ERPResponseMessageDto<ERPLaserCalculatorDto>> Process_PutLaserCalculator(ERPLaserCalculatorDto laserCalculator);

	/// <summary>
	/// Validates the request for deleting a LaserCalculator record.
	/// </summary>
	/// <param name="laserCalculatorId">The Unique Id of the LaserCalculator.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteLaserCalculator(Guid laserCalculatorId);

	/// <summary>
	/// Processes the request to delete a LaserCalculator record.
	/// </summary>
	/// <param name="laserCalculatorId">The Unique Id of the LaserCalculator.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPLaserCalculatorDto>> Process_DeleteLaserCalculator(Guid laserCalculatorId);
}
