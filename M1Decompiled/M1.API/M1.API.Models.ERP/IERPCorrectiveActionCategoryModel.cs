using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPCorrectiveActionCategoryModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all CorrectiveActionCategories with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CorrectiveActionCategories to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllCorrectiveActionCategories(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving CorrectiveActionCategory information based on the specified CorrectiveActionCategory Unique Id.
	/// </summary>
	/// <param name="correctiveActionCategoryId">The Unique Id of the CorrectiveActionCategory.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetCorrectiveActionCategory(Guid correctiveActionCategoryId);

	/// <summary>
	/// Validates the PUT request for creating or updating CorrectiveActionCategory information based on the specified CorrectiveActionCategory.
	/// </summary>
	/// <param name="correctiveActionCategory">The CorrectiveActionCategory details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutCorrectiveActionCategory(ERPCorrectiveActionCategoryDto correctiveActionCategory);

	/// <summary>
	/// Processes the request to retrieve all CorrectiveActionCategories with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CorrectiveActionCategories to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CorrectiveActionCategories DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPCorrectiveActionCategoryDto>>> Process_GetAllCorrectiveActionCategories(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific CorrectiveActionCategory.
	/// </summary>
	/// <param name="correctiveActionCategoryId">The Unique Id of the CorrectiveActionCategory to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the CorrectiveActionCategory DTO.</returns>
	Task<ERPResponseMessageDto<ERPCorrectiveActionCategoryDto>> Process_GetCorrectiveActionCategory(Guid correctiveActionCategoryId);

	/// <summary>
	/// Processes the creating or updating of a CorrectiveActionCategory record.
	/// </summary>
	/// <param name="correctiveActionCategory">The CorrectiveActionCategory data transfer object (DTO) containing the details of the CorrectiveActionCategory to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the CorrectiveActionCategory details.</returns>
	Task<ERPResponseMessageDto<ERPCorrectiveActionCategoryDto>> Process_PutCorrectiveActionCategory(ERPCorrectiveActionCategoryDto correctiveActionCategory);

	/// <summary>
	/// Validates the request for deleting a CorrectiveActionCategory record.
	/// </summary>
	/// <param name="correctiveActionCategoryId">The Unique Id of the CorrectiveActionCategory.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteCorrectiveActionCategory(Guid correctiveActionCategoryId);

	/// <summary>
	/// Processes the request to delete a CorrectiveActionCategory record.
	/// </summary>
	/// <param name="correctiveActionCategoryId">The Unique Id of the CorrectiveActionCategory.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPCorrectiveActionCategoryDto>> Process_DeleteCorrectiveActionCategory(Guid correctiveActionCategoryId);
}
