using System;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp.AxScript;

[AxScript("JobMaterialComponent")]
[ComVisible(true)]
public class AppAxJobMaterialComponent : IDisposable
{
	private IServiceProvider _provider;

	private M1Database _database;

	public AppAxJobMaterialComponent(IServiceProvider parentProvider)
	{
		_provider = parentProvider;
		_database = parentProvider.GetService(typeof(M1Database)) as M1Database;
	}

	public double CalculateAllocatedQuantityExpression(double materialQuantity, double quantityReceived, bool pullAllFromStock, bool receivedComplete)
	{
		if (pullAllFromStock)
		{
			if (quantityReceived <= 0.0 || receivedComplete)
			{
				return materialQuantity;
			}
			return materialQuantity - (materialQuantity - quantityReceived);
		}
		return 0.0;
	}

	public void Dispose()
	{
		_database = null;
		_provider = null;
	}
}
