using System.Data;

namespace M1.Ax.Erp;

public class DMRClaim
{
	public void UpdateQuantitiesInGrid(DataRow row, string changedField)
	{
		bool flag = false;
		decimal num = default(decimal);
		if (!row.Table.Columns.Contains("ShippedComplete") || !row.Table.Columns.Contains("OpenQty") || !row.Table.Columns.Contains("QuantityShipped") || !row.Table.Columns.Contains("FieldSelected"))
		{
			return;
		}
		flag = ((!changedField.Equals("FieldSelected")) ? row.Field<bool>("ShippedComplete") : (row.Field<bool>("FieldSelected") ? true : false));
		num = row.Field<decimal>("OpenQty");
		if (flag)
		{
			if (row.Field<decimal>("QuantityShipped") == 0m)
			{
				row.SetField("QuantityShipped", num);
			}
		}
		else if (changedField.Equals("FieldSelected"))
		{
			row.SetField("QuantityShipped", 0m);
		}
		row.SetField("ShippedComplete", flag);
	}
}
