using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("FixedAssets")]
[ComVisible(true)]
public class AppAxFixedAssets : IDisposable
{
	private IServiceProvider provider;

	public AppAxFixedAssets(IServiceProvider parentProvider)
	{
		provider = parentProvider;
	}

	public string CreateAssetAdjustmentInvoice(int assetAdjustmentID)
	{
		return new FixedAsset().CreateAssetAdjustmentInvoice((M1Database)provider.GetService(typeof(M1Database)), assetAdjustmentID);
	}

	public bool CreateAssetFromReceiptLine(DataRow lineRow, DataRow receiptRow, SqlTransaction trans, decimal quantity)
	{
		return new FixedAsset().CreateAssetFromReceiptLine((M1Database)provider.GetService(typeof(M1Database)), trans, lineRow, receiptRow, quantity);
	}

	public void SetAssetValues(M1BindingSource bindingSource)
	{
		new FixedAsset().SetAssetValues(bindingSource);
	}

	public void Dispose()
	{
		provider = null;
	}
}
