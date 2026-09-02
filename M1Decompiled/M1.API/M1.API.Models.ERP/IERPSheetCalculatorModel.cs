using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPSheetCalculatorModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all SheetCalculators with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SheetCalculators to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllSheetCalculators(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving SheetCalculator information based on the specified SheetCalculator Unique Id.
	/// </summary>
	/// <param name="sheetCalculatorId">The Unique Id of the SheetCalculator.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetSheetCalculator(Guid sheetCalculatorId);

	/// <summary>
	/// Validates the PUT request for creating or updating SheetCalculator information based on the specified SheetCalculator.
	/// </summary>
	/// <param name="sheetCalculator">The SheetCalculator details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutSheetCalculator(ERPSheetCalculatorDto sheetCalculator);

	/// <summary>
	/// Processes the request to retrieve all SheetCalculators with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SheetCalculators to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SheetCalculators DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPSheetCalculatorDto>>> Process_GetAllSheetCalculators(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific SheetCalculator.
	/// </summary>
	/// <param name="sheetCalculatorId">The Unique Id of the SheetCalculator to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the SheetCalculator DTO.</returns>
	Task<ERPResponseMessageDto<ERPSheetCalculatorDto>> Process_GetSheetCalculator(Guid sheetCalculatorId);

	/// <summary>
	/// Processes the creating or updating of a SheetCalculator record.
	/// </summary>
	/// <param name="sheetCalculator">The SheetCalculator data transfer object (DTO) containing the details of the SheetCalculator to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the SheetCalculator details.</returns>
	Task<ERPResponseMessageDto<ERPSheetCalculatorDto>> Process_PutSheetCalculator(ERPSheetCalculatorDto sheetCalculator);

	/// <summary>
	/// Validates the request for deleting a SheetCalculator record.
	/// </summary>
	/// <param name="sheetCalculatorId">The Unique Id of the SheetCalculator.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteSheetCalculator(Guid sheetCalculatorId);

	/// <summary>
	/// Processes the request to delete a SheetCalculator record.
	/// </summary>
	/// <param name="sheetCalculatorId">The Unique Id of the SheetCalculator.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPSheetCalculatorDto>> Process_DeleteSheetCalculator(Guid sheetCalculatorId);
}
