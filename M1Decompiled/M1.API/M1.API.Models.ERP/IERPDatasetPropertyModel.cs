using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPDatasetPropertyModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all DatasetProperties with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DatasetProperties to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllDatasetProperties(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving DatasetProperty information based on the specified DatasetProperty Unique Id.
	/// </summary>
	/// <param name="datasetPropertyId">The Unique Id of the DatasetProperty.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetDatasetProperty(Guid datasetPropertyId);

	/// <summary>
	/// Processes the request to retrieve all DatasetProperties with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DatasetProperties to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of DatasetProperties DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPDatasetPropertyDto>>> Process_GetAllDatasetProperties(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific DatasetProperty.
	/// </summary>
	/// <param name="datasetPropertyId">The Unique Id of the DatasetProperty to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the DatasetProperty DTO.</returns>
	Task<ERPResponseMessageDto<ERPDatasetPropertyDto>> Process_GetDatasetProperty(Guid datasetPropertyId);
}
