using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPCountyCodeModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all CountyCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CountyCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllCountyCodes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving CountyCode information based on the specified CountyCode Unique Id.
	/// </summary>
	/// <param name="countyCodeId">The Unique Id of the CountyCode.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetCountyCode(Guid countyCodeId);

	/// <summary>
	/// Validates the PUT request for creating or updating CountyCode information based on the specified CountyCode.
	/// </summary>
	/// <param name="countyCode">The CountyCode details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutCountyCode(ERPCountyCodeDto countyCode);

	/// <summary>
	/// Processes the request to retrieve all CountyCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CountyCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CountyCodes DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPCountyCodeDto>>> Process_GetAllCountyCodes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific CountyCode.
	/// </summary>
	/// <param name="countyCodeId">The Unique Id of the CountyCode to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the CountyCode DTO.</returns>
	Task<ERPResponseMessageDto<ERPCountyCodeDto>> Process_GetCountyCode(Guid countyCodeId);

	/// <summary>
	/// Processes the creating or updating of a CountyCode record.
	/// </summary>
	/// <param name="countyCode">The CountyCode data transfer object (DTO) containing the details of the CountyCode to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the CountyCode details.</returns>
	Task<ERPResponseMessageDto<ERPCountyCodeDto>> Process_PutCountyCode(ERPCountyCodeDto countyCode);

	/// <summary>
	/// Validates the request for deleting a CountyCode record.
	/// </summary>
	/// <param name="countyCodeId">The Unique Id of the CountyCode.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteCountyCode(Guid countyCodeId);

	/// <summary>
	/// Processes the request to delete a CountyCode record.
	/// </summary>
	/// <param name="countyCodeId">The Unique Id of the CountyCode.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPCountyCodeDto>> Process_DeleteCountyCode(Guid countyCodeId);
}
