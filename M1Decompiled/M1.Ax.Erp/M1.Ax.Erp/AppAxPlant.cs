using System.Data;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("Plant")]
[ComVisible(true)]
public class AppAxPlant
{
	public string GetWhereUsedList(DataRow row, M1BindingSource bindingSource)
	{
		return new Plant().GetWhereUsedList(row, bindingSource);
	}
}
