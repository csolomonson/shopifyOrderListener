using System;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("Calls")]
[ComVisible(true)]
public class AppAxCalls : IDisposable
{
	private IServiceProvider provider;

	public AppAxCalls(IServiceProvider parentProvider)
	{
		provider = parentProvider;
	}

	public string CreateFieldServiceInvoice(string callID)
	{
		return new Call().CreateFieldServiceInvoice((M1Database)provider.GetService(typeof(M1Database)), callID, null, 0, 0);
	}

	public string CreateFieldServiceJob(string callID)
	{
		Call call = new Call();
		try
		{
			call.CreateFieldServiceJob((M1Database)provider.GetService(typeof(M1Database)), callID);
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
		return string.Empty;
	}

	public void Dispose()
	{
		provider = null;
	}
}
