using System;
using System.Threading.Tasks;

namespace M1.API.Repositories.Core;

public interface IEmployeeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks whether an employee with the specified ID exists.
	/// </summary>
	/// <param name="employeeId">The ID of the employee to check.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains <c>true</c> if the employee exists; otherwise, <c>false</c>.</returns>
	Task<bool> DoesEmployeeExistsAsync(string employeeId);
}
