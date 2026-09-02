using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("Organizations")]
[ComVisible(true)]
public class AppAxOrganizations
{
	private IServiceProvider provider;

	public AppAxOrganizations(IServiceProvider parentProvider)
	{
		provider = parentProvider;
	}

	public void CustomerCreditCheck(string customerID, string locationID, byte creditMsgType, byte holdMsgType, decimal orderOffsetAmount, decimal shipmentOffsetAmount, decimal invoiceOffsetAmount, ValidationInfo validationInfo, bool isShipment = false)
	{
		new Organizations().CustomerCreditCheck((M1Database)provider.GetService(typeof(M1Database)), customerID, locationID, creditMsgType, holdMsgType, orderOffsetAmount, shipmentOffsetAmount, invoiceOffsetAmount, validationInfo, isShipment);
	}

	public string GetAttachmentPath(string orgID, string locID)
	{
		M1Database obj = (M1Database)provider.GetService(typeof(M1Database));
		SqlCommand sqlCommand = obj.NewSqlCommand("SELECT 'HDAttachmentPath' = CASE WHEN CAST(cmlHDAttachmentFilePath AS NVARCHAR(200)) = '' THEN (CASE WHEN CAST(xapHDAttachmentFilePath AS NVARCHAR(200))= '' THEN '' ELSE CAST(xapHDAttachmentFilePath AS NVARCHAR(200)) END) ELSE CAST(cmlHDAttachmentFilePath AS NVARCHAR(200)) END FROM OrganizationLocations, ProductionProperties WHERE cmlOrganizationID = @OrgID AND cmlLocationID = @LocationID");
		sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = orgID;
		sqlCommand.Parameters.Add(new SqlParameter("@LocationID", SqlDbType.NVarChar)).Value = locID;
		object obj2 = obj.ExecuteScalar(sqlCommand);
		if (obj2 != null)
		{
			string text = obj2.ToString().Trim();
			if (text.Length != 0 && !Directory.Exists(text) && !text.EndsWith("\\"))
			{
				text += "\\";
			}
			return text;
		}
		return string.Empty;
	}

	public void AddressValidation(M1BindingSource bindingSource)
	{
		new Organizations().AddressValidation(bindingSource);
	}

	public string GetCaptionAddressValidation()
	{
		return new Organizations().GetCaptionAddressValidation((M1Database)provider.GetService(typeof(M1Database)));
	}
}
