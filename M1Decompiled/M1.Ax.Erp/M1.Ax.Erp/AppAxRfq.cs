using System;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("Rfq")]
[ComVisible(true)]
public class AppAxRfq
{
	private IServiceProvider provider;

	public AppAxRfq(IServiceProvider parentProvider)
	{
		provider = parentProvider;
	}

	public int AddSuppliersToRFQLine(object bindingSource)
	{
		return new Rfq().AddSuppliersToRfqLine((M1BindingSource)bindingSource);
	}

	public int TransferPrices(string cRFQ, int nLine, int nSequence, bool bExpireExisting, bool bUseForeignAmounts)
	{
		return new Rfq().TransferPrices(provider.GetService(typeof(M1Database)) as M1Database, cRFQ, nLine, nSequence, bExpireExisting, bUseForeignAmounts);
	}

	public void CalculateRfqPriceBreakFromQuantity(object sender, object e)
	{
		FieldDefinition.FieldValueChangedEventArgs e2 = (FieldDefinition.FieldValueChangedEventArgs)e;
		new Rfq().GetPriceForQuantity(e2.Database, e2.Row);
	}
}
