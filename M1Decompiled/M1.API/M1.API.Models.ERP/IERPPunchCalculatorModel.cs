using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPunchCalculatorModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PunchCalculators with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PunchCalculators to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPunchCalculators(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PunchCalculator information based on the specified PunchCalculator Unique Id.
	/// </summary>
	/// <param name="punchCalculatorId">The Unique Id of the PunchCalculator.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPunchCalculator(Guid punchCalculatorId);

	/// <summary>
	/// Validates the PUT request for creating or updating PunchCalculator information based on the specified PunchCalculator.
	/// </summary>
	/// <param name="punchCalculator">The PunchCalculator details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPunchCalculator(ERPPunchCalculatorDto punchCalculator);

	/// <summary>
	/// Processes the request to retrieve all PunchCalculators with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PunchCalculators to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PunchCalculators DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPunchCalculatorDto>>> Process_GetAllPunchCalculators(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PunchCalculator.
	/// </summary>
	/// <param name="punchCalculatorId">The Unique Id of the PunchCalculator to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PunchCalculator DTO.</returns>
	Task<ERPResponseMessageDto<ERPPunchCalculatorDto>> Process_GetPunchCalculator(Guid punchCalculatorId);

	/// <summary>
	/// Processes the creating or updating of a PunchCalculator record.
	/// </summary>
	/// <param name="punchCalculator">The PunchCalculator data transfer object (DTO) containing the details of the PunchCalculator to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PunchCalculator details.</returns>
	Task<ERPResponseMessageDto<ERPPunchCalculatorDto>> Process_PutPunchCalculator(ERPPunchCalculatorDto punchCalculator);

	/// <summary>
	/// Validates the request for deleting a PunchCalculator record.
	/// </summary>
	/// <param name="punchCalculatorId">The Unique Id of the PunchCalculator.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePunchCalculator(Guid punchCalculatorId);

	/// <summary>
	/// Processes the request to delete a PunchCalculator record.
	/// </summary>
	/// <param name="punchCalculatorId">The Unique Id of the PunchCalculator.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPunchCalculatorDto>> Process_DeletePunchCalculator(Guid punchCalculatorId);
}
