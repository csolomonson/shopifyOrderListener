using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("NonConformances")]
[ComVisible(true)]
public class AppAxNonConformances : IDisposable
{
	private IServiceProvider provider;

	public AppAxNonConformances(IServiceProvider parentProvider)
	{
		provider = parentProvider;
	}

	public bool ConvertRMAClaimProblemsToNonConformances(SqlTransaction transaction)
	{
		return new NonConformance().ConvertRMAClaimProblemsToNonConformances((M1Database)provider.GetService(typeof(M1Database)), transaction);
	}

	public void Dispose()
	{
		provider = null;
	}
}
