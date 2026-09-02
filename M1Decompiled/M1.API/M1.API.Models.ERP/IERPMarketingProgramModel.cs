using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPMarketingProgramModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all MarketingPrograms with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MarketingPrograms to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllMarketingPrograms(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving MarketingProgram information based on the specified MarketingProgram Unique Id.
	/// </summary>
	/// <param name="marketingProgramId">The Unique Id of the MarketingProgram.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetMarketingProgram(Guid marketingProgramId);

	/// <summary>
	/// Validates the PUT request for creating or updating MarketingProgram information based on the specified MarketingProgram.
	/// </summary>
	/// <param name="marketingProgram">The MarketingProgram details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutMarketingProgram(ERPMarketingProgramDto marketingProgram);

	/// <summary>
	/// Processes the request to retrieve all MarketingPrograms with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MarketingPrograms to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MarketingPrograms DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPMarketingProgramDto>>> Process_GetAllMarketingPrograms(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific MarketingProgram.
	/// </summary>
	/// <param name="marketingProgramId">The Unique Id of the MarketingProgram to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the MarketingProgram DTO.</returns>
	Task<ERPResponseMessageDto<ERPMarketingProgramDto>> Process_GetMarketingProgram(Guid marketingProgramId);

	/// <summary>
	/// Processes the creating or updating of a MarketingProgram record.
	/// </summary>
	/// <param name="marketingProgram">The MarketingProgram data transfer object (DTO) containing the details of the MarketingProgram to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the MarketingProgram details.</returns>
	Task<ERPResponseMessageDto<ERPMarketingProgramDto>> Process_PutMarketingProgram(ERPMarketingProgramDto marketingProgram);

	/// <summary>
	/// Validates the request for deleting a MarketingProgram record.
	/// </summary>
	/// <param name="marketingProgramId">The Unique Id of the MarketingProgram.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteMarketingProgram(Guid marketingProgramId);

	/// <summary>
	/// Processes the request to delete a MarketingProgram record.
	/// </summary>
	/// <param name="marketingProgramId">The Unique Id of the MarketingProgram.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPMarketingProgramDto>> Process_DeleteMarketingProgram(Guid marketingProgramId);
}
