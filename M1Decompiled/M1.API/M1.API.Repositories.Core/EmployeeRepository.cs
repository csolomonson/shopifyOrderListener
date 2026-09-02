using System;
using System.Threading.Tasks;
using M1.API.Utilities;
using M1.Core;

namespace M1.API.Repositories.Core;

public class EmployeeRepository : APIBaseRepository, IEmployeeRepository, IAPIBaseRepository, IDisposable
{
	public EmployeeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public EmployeeRepository(M1Database database)
	{
		base.M1database = database;
	}

	public Task<bool> DoesEmployeeExistsAsync(string employeeId)
	{
		InitializeParameterLists();
		base.filterList.Add("lmeEmployeeID|C", employeeId);
		base.filterList.Add("lmeQuoterEmployee", 1);
		base.selectList.Add("lmeEmployeeID");
		return Task.FromResult(GetAsObject("Employees", base.filterList, base.selectList, null, null) != null);
	}

	public new void Dispose()
	{
		base.Dispose(disposing: true);
	}
}
