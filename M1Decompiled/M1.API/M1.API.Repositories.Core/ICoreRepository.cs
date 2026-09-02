using System;
using System.Threading.Tasks;
using M1.API.DTOs.Custom;

namespace M1.API.Repositories.Core;

public interface ICoreRepository : IAPIBaseRepository, IDisposable
{
	Task<CTMProcessDto> GetAllProcesses();

	Task<CTMWorkCenterDto> GetAllWorkCenters();

	Task<CTMWarehousesDto> GetAllWarehouses();

	Task<CTMWarehouseBinsDto> GetAllWarehouseBins();

	/// <summary>
	/// Checks if a warehouse exists based on its ID.
	/// </summary>
	/// <param name="partId">The ID of the part containing the warehouse.</param>
	/// <param name="partRevisionId">The ID of the part revision containing the warehouse.</param>
	/// <param name="warehouseId">The ID of the warehouse to check.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. 
	/// The task result contains a boolean value indicating whether the warehouse exists.
	/// </returns>
	Task<bool> DoesWarehouseExistAsync(string partId, string partRevisionId, string warehouseId);

	/// <summary>
	/// Checks if a bin exists within a specified warehouse based on its ID.
	/// </summary>
	/// <param name="partId">The ID of the part containing the bin.</param>
	/// <param name="partRevisionId">The ID of the part revision containing the bin.</param>
	/// <param name="warehouseId">The ID of the warehouse containing the bin.</param>
	/// <param name="binId">The ID of the bin to check.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. 
	/// The task result contains a boolean value indicating whether the bin exists within the specified warehouse.
	/// </returns>
	Task<bool> DoesBinExistAsync(string partId, string partRevisionId, string warehouseId, string binId);
}
