using System;
using System.Data;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("FedEx")]
[ComVisible(true)]
public class AppAxFedex
{
	private M1Database database;

	private bool? _FedexPopulated;

	public bool Populated => FedexPopulated;

	public bool FdxOff => !FedexPopulated;

	public bool FedexPopulated
	{
		get
		{
			if (!_FedexPopulated.HasValue)
			{
				DataRow dataRow = database.Props("SM");
				if (dataRow["xsmFdxHostAddress"] != DBNull.Value && dataRow.Field<string>("xsmFdxHostAddress").Trim().Length != 0 && dataRow["xsmFdxAccountNumber"] != DBNull.Value && dataRow.Field<string>("xsmFdxAccountNumber").Trim().Length != 0 && dataRow["xsmFdxHostService"] != DBNull.Value && dataRow.Field<string>("xsmFdxHostService").Trim().Length != 0)
				{
					_FedexPopulated = true;
				}
				else
				{
					_FedexPopulated = false;
				}
			}
			return _FedexPopulated.Value;
		}
	}

	public AppAxFedex(IServiceProvider parentProvider)
	{
		database = parentProvider.GetService(typeof(M1Database)) as M1Database;
	}
}
