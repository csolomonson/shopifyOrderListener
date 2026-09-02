using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPContactMethodModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ContactMethods with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ContactMethods to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllContactMethods(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ContactMethod information based on the specified ContactMethod Unique Id.
	/// </summary>
	/// <param name="contactMethodId">The Unique Id of the ContactMethod.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetContactMethod(Guid contactMethodId);

	/// <summary>
	/// Validates the PUT request for creating or updating ContactMethod information based on the specified ContactMethod.
	/// </summary>
	/// <param name="contactMethod">The ContactMethod details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutContactMethod(ERPContactMethodDto contactMethod);

	/// <summary>
	/// Processes the request to retrieve all ContactMethods with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ContactMethods to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ContactMethods DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPContactMethodDto>>> Process_GetAllContactMethods(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ContactMethod.
	/// </summary>
	/// <param name="contactMethodId">The Unique Id of the ContactMethod to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ContactMethod DTO.</returns>
	Task<ERPResponseMessageDto<ERPContactMethodDto>> Process_GetContactMethod(Guid contactMethodId);

	/// <summary>
	/// Processes the creating or updating of a ContactMethod record.
	/// </summary>
	/// <param name="contactMethod">The ContactMethod data transfer object (DTO) containing the details of the ContactMethod to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ContactMethod details.</returns>
	Task<ERPResponseMessageDto<ERPContactMethodDto>> Process_PutContactMethod(ERPContactMethodDto contactMethod);

	/// <summary>
	/// Validates the request for deleting a ContactMethod record.
	/// </summary>
	/// <param name="contactMethodId">The Unique Id of the ContactMethod.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteContactMethod(Guid contactMethodId);

	/// <summary>
	/// Processes the request to delete a ContactMethod record.
	/// </summary>
	/// <param name="contactMethodId">The Unique Id of the ContactMethod.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPContactMethodDto>> Process_DeleteContactMethod(Guid contactMethodId);
}
