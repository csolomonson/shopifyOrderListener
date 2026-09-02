using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPLaserCalculatorLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all LaserCalculatorLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LaserCalculatorLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllLaserCalculatorLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving LaserCalculatorLine information based on the specified LaserCalculatorLine Unique Id.
	/// </summary>
	/// <param name="laserCalculatorLineId">The Unique Id of the LaserCalculatorLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetLaserCalculatorLine(Guid laserCalculatorLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating LaserCalculatorLine information based on the specified LaserCalculatorLine.
	/// </summary>
	/// <param name="laserCalculatorLine">The LaserCalculatorLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutLaserCalculatorLine(ERPLaserCalculatorLineDto laserCalculatorLine);

	/// <summary>
	/// Processes the request to retrieve all LaserCalculatorLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LaserCalculatorLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LaserCalculatorLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPLaserCalculatorLineDto>>> Process_GetAllLaserCalculatorLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific LaserCalculatorLine.
	/// </summary>
	/// <param name="laserCalculatorLineId">The Unique Id of the LaserCalculatorLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the LaserCalculatorLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPLaserCalculatorLineDto>> Process_GetLaserCalculatorLine(Guid laserCalculatorLineId);

	/// <summary>
	/// Processes the creating or updating of a LaserCalculatorLine record.
	/// </summary>
	/// <param name="laserCalculatorLine">The LaserCalculatorLine data transfer object (DTO) containing the details of the LaserCalculatorLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the LaserCalculatorLine details.</returns>
	Task<ERPResponseMessageDto<ERPLaserCalculatorLineDto>> Process_PutLaserCalculatorLine(ERPLaserCalculatorLineDto laserCalculatorLine);

	/// <summary>
	/// Validates the request for deleting a LaserCalculatorLine record.
	/// </summary>
	/// <param name="laserCalculatorLineId">The Unique Id of the LaserCalculatorLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteLaserCalculatorLine(Guid laserCalculatorLineId);

	/// <summary>
	/// Processes the request to delete a LaserCalculatorLine record.
	/// </summary>
	/// <param name="laserCalculatorLineId">The Unique Id of the LaserCalculatorLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPLaserCalculatorLineDto>> Process_DeleteLaserCalculatorLine(Guid laserCalculatorLineId);
}
