using System;
using System.Runtime.InteropServices;
using M1.Core.Script;
using M1.Script.Interfaces;

namespace M1.Ax.Erp;

[AxScript("ProductConfigurator")]
[ComVisible(true)]
public class AppAxProductConfigurator
{
	private IServiceProvider provider;

	public AppAxProductConfigurator(IServiceProvider parentProvider)
	{
		provider = parentProvider;
	}

	public IProductConfigurator CreateProductConfigurator()
	{
		return new ProductConfigurator(provider);
	}
}
