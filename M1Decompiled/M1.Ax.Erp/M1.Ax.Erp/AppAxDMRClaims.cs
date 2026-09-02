using System;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("DMRClaims")]
[ComVisible(true)]
public class AppAxDMRClaims : IDisposable
{
	private IServiceProvider provider;

	public AppAxDMRClaims(IServiceProvider parentProvider)
	{
		provider = parentProvider;
	}

	public void UpdateQuantitiesInGrid(object sender, object e)
	{
		FieldDefinition.FieldValueChangedEventArgs e2 = (FieldDefinition.FieldValueChangedEventArgs)e;
		FieldDefinition fieldDefinition = (FieldDefinition)sender;
		new DMRClaim().UpdateQuantitiesInGrid(e2.Row, fieldDefinition.FieldName);
	}

	public void Dispose()
	{
		provider = null;
	}
}
