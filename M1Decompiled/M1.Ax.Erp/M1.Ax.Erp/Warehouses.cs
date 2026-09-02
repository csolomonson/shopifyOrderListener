using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class Warehouses
{
	public void InactivateWarehouseBins(M1BindingSource bindingSource)
	{
		if (bindingSource == null || !bindingSource.CurrentAsDataRow.Field<bool>("imwInactive"))
		{
			return;
		}
		foreach (DataRow row in bindingSource.PrimaryTable.GetChildBindingSource("WarehouseBins").GetDataTable().Rows)
		{
			if (!row.Field<bool>("inbInactive"))
			{
				row.SetField("inbInactive", value: true);
				row.SetField("inbInactiveDate", DateTime.Now);
				row.SetField("inbDefaultBin", value: false);
			}
		}
	}

	public DataTable GetPartsToConfirm(M1Database database, string warehouseID, string partBinID)
	{
		return database.GetDataTable(new SqlCommand("select p1.imbPartID,p1.imbPartRevisionID,'None' as imbWarehouseID, 'None' as imbPartBinID \r\n                from PartBins p1 inner join (select distinct imbPartID,imbPartRevisionID from PartBins where imbPartBinID=" + partBinID.ToSql() + " AND imbWarehouseID=" + warehouseID.ToSql() + " ) as p0 on  p1.imbPartID=p0.imbPartID and p1.imbPartRevisionID=p0.imbPartRevisionID\r\n                group by p1.imbPartID,p1.imbPartRevisionID \r\n                having sum(CONVERT(INT, imbDefaultBin))=0\r\n                union\r\n                select p1.imbPartID,p1.imbPartRevisionID,imbWarehouseID,imbPartBinID \r\n                from PartBins p1 inner join (select distinct imbPartID,imbPartRevisionID  from PartBins where imbPartBinID=" + partBinID.ToSql() + " AND imbWarehouseID=" + warehouseID.ToSql() + " ) as p0 on  p1.imbPartID=p0.imbPartID and p1.imbPartRevisionID=p0.imbPartRevisionID\r\n                where imbDefaultBin=1 and (imbPartBinID<>" + partBinID.ToSql() + " OR imbWarehouseID<>" + warehouseID.ToSql() + ")"));
	}
}
