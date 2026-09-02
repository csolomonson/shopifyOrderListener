using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.DD.DBDefaults;

[DBCreateDefault("Create default Attachment Types")]
public class AttachmentTypes
{
	public AttachmentTypes(DBCreateDefaultParms parm)
	{
		foreach (KeyValuePair<string, string> item in new Dictionary<string, string>
		{
			{ "PHONE", "Phone" },
			{ "FAX", "Fax" },
			{ "WORD", "Word" },
			{ "SPSH", "Spreadsheet" },
			{ "TEXT", "Text Document" },
			{ "DRAW", "Draw" },
			{ "IMG", "Image" },
			{ "MEMO", "Memo" }
		})
		{
			SqlDataAdapter adapter;
			DataTable dataTable = parm.ServerManager.GetDataTable(null, parm.User, parm.DatabaseName, 0, "Select * From attachmentTypes", fillSchema: true, out adapter);
			DataRow dataRow = dataTable.NewRow();
			dataRow.BeginEdit();
			dataRow.BlankRow();
			dataRow.SetField("cmtAttachmentTypeID", item.Key.ToString());
			dataRow.SetField("cmtDescription", item.Value.ToString());
			dataRow.SetField("cmtCreatedDate", DateTime.Now);
			dataRow.SetField("cmtCreatedBy", parm.User.ID);
			dataRow.EndEdit();
			dataTable.Rows.Add(dataRow);
			parm.ServerManager.UpdateData(null, parm.User, parm.DatabaseName, new DataRow[1] { dataRow }, adapter);
		}
	}
}
