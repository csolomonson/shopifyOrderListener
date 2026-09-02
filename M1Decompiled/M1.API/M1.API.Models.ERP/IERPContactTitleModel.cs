using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPContactTitleModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ContactTitles with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ContactTitles to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllContactTitles(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ContactTitle information based on the specified ContactTitle Unique Id.
	/// </summary>
	/// <param name="contactTitleId">The Unique Id of the ContactTitle.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetContactTitle(Guid contactTitleId);

	/// <summary>
	/// Validates the PUT request for creating or updating ContactTitle information based on the specified ContactTitle.
	/// </summary>
	/// <param name="contactTitle">The ContactTitle details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutContactTitle(ERPContactTitleDto contactTitle);

	/// <summary>
	/// Processes the request to retrieve all ContactTitles with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ContactTitles to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ContactTitles DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPContactTitleDto>>> Process_GetAllContactTitles(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ContactTitle.
	/// </summary>
	/// <param name="contactTitleId">The Unique Id of the ContactTitle to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ContactTitle DTO.</returns>
	Task<ERPResponseMessageDto<ERPContactTitleDto>> Process_GetContactTitle(Guid contactTitleId);

	/// <summary>
	/// Processes the creating or updating of a ContactTitle record.
	/// </summary>
	/// <param name="contactTitle">The ContactTitle data transfer object (DTO) containing the details of the ContactTitle to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ContactTitle details.</returns>
	Task<ERPResponseMessageDto<ERPContactTitleDto>> Process_PutContactTitle(ERPContactTitleDto contactTitle);

	/// <summary>
	/// Validates the request for deleting a ContactTitle record.
	/// </summary>
	/// <param name="contactTitleId">The Unique Id of the ContactTitle.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteContactTitle(Guid contactTitleId);

	/// <summary>
	/// Processes the request to delete a ContactTitle record.
	/// </summary>
	/// <param name="contactTitleId">The Unique Id of the ContactTitle.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPContactTitleDto>> Process_DeleteContactTitle(Guid contactTitleId);
}
