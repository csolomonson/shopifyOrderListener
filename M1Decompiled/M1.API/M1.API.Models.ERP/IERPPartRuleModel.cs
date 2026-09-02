using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartRuleModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartRules with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartRules to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartRules(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartRule information based on the specified PartRule Unique Id.
	/// </summary>
	/// <param name="partRuleId">The Unique Id of the PartRule.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartRule(Guid partRuleId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartRule information based on the specified PartRule.
	/// </summary>
	/// <param name="partRule">The PartRule details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartRule(ERPPartRuleDto partRule);

	/// <summary>
	/// Processes the request to retrieve all PartRules with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartRules to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartRules DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartRuleDto>>> Process_GetAllPartRules(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartRule.
	/// </summary>
	/// <param name="partRuleId">The Unique Id of the PartRule to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartRule DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartRuleDto>> Process_GetPartRule(Guid partRuleId);

	/// <summary>
	/// Processes the creating or updating of a PartRule record.
	/// </summary>
	/// <param name="partRule">The PartRule data transfer object (DTO) containing the details of the PartRule to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartRule details.</returns>
	Task<ERPResponseMessageDto<ERPPartRuleDto>> Process_PutPartRule(ERPPartRuleDto partRule);

	/// <summary>
	/// Validates the request for deleting a PartRule record.
	/// </summary>
	/// <param name="partRuleId">The Unique Id of the PartRule.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartRule(Guid partRuleId);

	/// <summary>
	/// Processes the request to delete a PartRule record.
	/// </summary>
	/// <param name="partRuleId">The Unique Id of the PartRule.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartRuleDto>> Process_DeletePartRule(Guid partRuleId);
}
