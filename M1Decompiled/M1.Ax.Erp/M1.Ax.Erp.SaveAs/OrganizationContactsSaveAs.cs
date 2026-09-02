using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp.SaveAs;

[SaveAsProcessing("OrganizationContacts")]
public class OrganizationContactsSaveAs : ISaveAsProcessing
{
	public void BeforeUpdate(SaveAsProcessingParms parm)
	{
		if (!parm.ParentIdExists && parm.ParentTable.Equals("OrganizationContacts", StringComparison.CurrentCultureIgnoreCase) && string.IsNullOrWhiteSpace(parm.NewKeyValues[2].ToString()))
		{
			SqlCommand sqlCommand = parm.Database.NewSqlCommand("SELECT 1 As Dummy FROM OrganizationLocations WHERE cmlOrganizationID = @OrgID AND cmlLocationID = @LocID");
			sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = parm.NewKeyValues[0].ToString();
			sqlCommand.Parameters.Add(new SqlParameter("@LocID", SqlDbType.NVarChar)).Value = parm.NewKeyValues[1].ToString();
			if (parm.Database.ExecuteScalar(sqlCommand) != null)
			{
				parm.ParentIdExists = true;
			}
		}
	}

	public void AfterUpdate(SaveAsProcessingParms parm)
	{
	}
}
