using System;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("JobMaterial")]
[ComVisible(true)]
public class AppAxJobMaterial : IDisposable
{
	private IServiceProvider _provider;

	private M1Database _database;

	public AppAxJobMaterial(IServiceProvider parentProvider)
	{
		_provider = parentProvider;
		_database = parentProvider.GetService(typeof(M1Database)) as M1Database;
	}

	public double CalculateAllocatedQuantityExpression(double estimatedQuantity, double quantityReceived, bool pullAllFromStock, bool receivedComplete)
	{
		if (pullAllFromStock)
		{
			if (quantityReceived <= 0.0 || receivedComplete)
			{
				return estimatedQuantity;
			}
			return estimatedQuantity - (estimatedQuantity - quantityReceived);
		}
		return 0.0;
	}

	public void Dispose()
	{
		_database = null;
		_provider = null;
	}
}
