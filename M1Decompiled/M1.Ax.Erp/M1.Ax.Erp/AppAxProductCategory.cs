using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("ProductCategory")]
[ComVisible(true)]
public class AppAxProductCategory
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxProductCategory(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public void DeleteProductCategoryLine(object transaction, string productCategoryID, int lineID)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		new ProductCategory().DeleteProductCategoryLine(_Database, (SqlTransaction)transaction, productCategoryID, lineID);
	}
}
