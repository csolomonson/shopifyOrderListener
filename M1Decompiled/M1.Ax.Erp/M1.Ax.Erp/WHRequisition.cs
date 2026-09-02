using System.Data;

namespace M1.Ax.Erp;

public class WHRequisition
{
	public void UpdateQuantitiesInGrid(DataRow row, string changedField)
	{
		bool flag = false;
		decimal num = default(decimal);
		if (!row.Table.Columns.Contains("TransferredComplete") || !row.Table.Columns.Contains("OpenQty") || !row.Table.Columns.Contains("QtyShipped") || !row.Table.Columns.Contains("FieldSelected"))
		{
			return;
		}
		flag = ((!changedField.Equals("FieldSelected")) ? row.Field<bool>("TransferredComplete") : (row.Field<bool>("FieldSelected") ? true : false));
		num = row.Field<decimal>("OpenQty");
		if (flag)
		{
			if (row.Field<decimal>("QtyShipped") == 0m)
			{
				row.SetField("QtyShipped", num);
			}
		}
		else if (changedField.Equals("FieldSelected"))
		{
			row.SetField("QtyShipped", 0m);
		}
		row.SetField("TransferredComplete", flag);
	}
}
