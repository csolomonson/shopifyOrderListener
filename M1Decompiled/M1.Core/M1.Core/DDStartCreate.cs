using System;
using System.Windows.Forms;

namespace M1.Core;

public class DDStartCreate
{
	public delegate void AddItemDelegate(string text);

	public delegate void RedrawListDelegate();

	public delegate void SelectItemDelegate(string item);

	public AppContext Context;

	public string location = string.Empty;

	public string dbName = string.Empty;

	public string productCode = string.Empty;

	public int size;

	public Form FormRef;

	public AddItemDelegate AddItemFunc;

	public RedrawListDelegate RedrawListFunc;

	public SelectItemDelegate SelectItemFunc;

	public Action<DDStartCreate> OnSuccess;

	public Action<Exception> OnFailure;

	public void Start()
	{
		try
		{
			new DmoDD(Context).CreateDataDictionaryDB(this, location, dbName, size, productCode);
			FormRef.Invoke(new Action<DDStartCreate>(OnSuccess.Invoke), this);
		}
		catch (Exception ex)
		{
			FormRef.Invoke(new Action<Exception>(OnFailure.Invoke), ex);
		}
	}
}
