using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp.SaveAs;

[SaveAsProcessing("PartRevisions")]
public class PartRevisionsSaveAs : ISaveAsProcessing
{
	public void BeforeUpdate(SaveAsProcessingParms parm)
	{
		if (parm.Table.Equals("PartRevisions", StringComparison.CurrentCultureIgnoreCase) && !parm.DataDictionary.ProductCode.IsModulePurchased("AB", parm.Database))
		{
			SqlCommand sqlCommand = parm.Database.NewSqlCommand("SELECT IsNull(COUNT(*),0) FROM PartRevisions WHERE imrPartID = @PartID AND imrPartRevisionID <> @RevisionID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = parm.NewKeyValues[0];
			sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = parm.NewKeyValues[1];
			if (Convert.ToInt32(parm.Database.ExecuteScalar(sqlCommand)) > 0)
			{
				throw new M1Exception("You may not have mulitple revisions for a part unless the Advanced Bill of Materials module has been purchased. Part " + parm.NewKeyValues[0].ToString() + " already has another revision.");
			}
		}
	}

	public void AfterUpdate(SaveAsProcessingParms parm)
	{
	}
}
