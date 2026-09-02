using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPDatasetPropertyRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a DatasetProperty with the specified Unique Id exists.
	/// </summary>
	/// <param name="datasetPropertyId">The Unique Id of the DatasetProperty to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the DatasetProperty exists or not.</returns>
	Task<bool> DoesDatasetPropertyExist(Guid datasetPropertyId);

	/// <summary>
	/// Retrieves all DatasetProperties with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DatasetProperties to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of DatasetProperties DTOs.</returns>
	Task<ICollection<ERPDatasetPropertyInformationDto>> GetAllDatasetProperties(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific DatasetProperty.
	/// </summary>
	/// <param name="datasetPropertyId">The Unique Id of the DatasetProperty to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the DatasetProperty DTO.</returns>
	Task<ERPDatasetPropertyInformationDto> GetDatasetProperty(Guid datasetPropertyId);
}
