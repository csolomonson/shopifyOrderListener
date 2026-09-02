using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPProjectContactModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ProjectContacts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProjectContacts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllProjectContacts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ProjectContact information based on the specified ProjectContact Unique Id.
	/// </summary>
	/// <param name="projectContactId">The Unique Id of the ProjectContact.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetProjectContact(Guid projectContactId);

	/// <summary>
	/// Validates the PUT request for creating or updating ProjectContact information based on the specified ProjectContact.
	/// </summary>
	/// <param name="projectContact">The ProjectContact details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutProjectContact(ERPProjectContactDto projectContact);

	/// <summary>
	/// Processes the request to retrieve all ProjectContacts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProjectContacts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProjectContacts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPProjectContactDto>>> Process_GetAllProjectContacts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ProjectContact.
	/// </summary>
	/// <param name="projectContactId">The Unique Id of the ProjectContact to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ProjectContact DTO.</returns>
	Task<ERPResponseMessageDto<ERPProjectContactDto>> Process_GetProjectContact(Guid projectContactId);

	/// <summary>
	/// Processes the creating or updating of a ProjectContact record.
	/// </summary>
	/// <param name="projectContact">The ProjectContact data transfer object (DTO) containing the details of the ProjectContact to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ProjectContact details.</returns>
	Task<ERPResponseMessageDto<ERPProjectContactDto>> Process_PutProjectContact(ERPProjectContactDto projectContact);

	/// <summary>
	/// Validates the request for deleting a ProjectContact record.
	/// </summary>
	/// <param name="projectContactId">The Unique Id of the ProjectContact.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteProjectContact(Guid projectContactId);

	/// <summary>
	/// Processes the request to delete a ProjectContact record.
	/// </summary>
	/// <param name="projectContactId">The Unique Id of the ProjectContact.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPProjectContactDto>> Process_DeleteProjectContact(Guid projectContactId);
}
