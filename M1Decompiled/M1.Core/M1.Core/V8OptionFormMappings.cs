using System;
using System.Collections.Generic;

namespace M1.Core;

public class V8OptionFormMappings
{
	public Dictionary<string, string> GetList()
	{
		return new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
		{
			{ "M1.UI.Database.Options.CompanyDetails", "M1.Ax.Core.Forms.Database.CompanyDetails" },
			{ "M1.UI.Database.Options.GeneralSettings", "M1.Ax.Core.Forms.Database.GeneralSettings" },
			{ "M1.UI.Database.Options.RegionSettings", "M1.Ax.Core.Forms.Database.RegionSettings" },
			{ "M1.UI.Database.Options.ChangeRequestProperties", "M1.Ax.Erp.Forms.Database.Options.ChangeRequestProperties" },
			{ "M1.UI.Database.Options.CreditLimitProperties", "M1.Ax.Erp.Forms.Database.Options.CreditLimitProperties" },
			{ "M1.UI.Database.Options.FEDFedexAccountAddressInfo", "M1.Ax.Erp.Forms.Database.Options.FEDFedexAccountAddressInfo" },
			{ "M1.UI.Database.Options.FEDFedexDefaultPackageInfo", "M1.Ax.Erp.Forms.Database.Options.FEDFedexDefaultPackageInfo" },
			{ "M1.UI.Database.Options.FEDFedexDefaultRequestInfo", "M1.Ax.Erp.Forms.Database.Options.FEDFedexDefaultRequestInfo" },
			{ "M1.UI.Database.Options.FEDFedexDefaultSpecialServicesInfo", "M1.Ax.Erp.Forms.Database.Options.FEDFedexDefaultSpecialServicesInfo" },
			{ "M1.UI.Database.Options.FEDFedexDefaultVariableHandlingCharges", "M1.Ax.Erp.Forms.Database.Options.FEDFedexDefaultVariableHandlingCharges" },
			{ "M1.UI.Database.Options.FEDFedexGeneralProperties", "M1.Ax.Erp.Forms.Database.Options.FEDFedexGeneralProperties" },
			{ "M1.UI.Database.Options.FINAPProperties", "M1.Ax.Erp.Forms.Database.Options.FINAPProperties" },
			{ "M1.UI.Database.Options.FINARProperties", "M1.Ax.Erp.Forms.Database.Options.FINARProperties" },
			{ "M1.UI.Database.Options.FINCOGSProperties", "M1.Ax.Erp.Forms.Database.Options.FINCOGSProperties" },
			{ "M1.UI.Database.Options.FINGeneralProperties", "M1.Ax.Erp.Forms.Database.Options.FINGeneralProperties" },
			{ "M1.UI.Database.Options.FINGLProperties", "M1.Ax.Erp.Forms.Database.Options.FINGLProperties" },
			{ "M1.UI.Database.Options.FINPayPalProperties", "M1.Ax.Erp.Forms.Database.Options.FINPayPalProperties" },
			{ "M1.UI.Database.Options.FINPayrollProperties", "M1.Ax.Erp.Forms.Database.Options.FINPayrollProperties" },
			{ "M1.UI.Database.Options.POSProperties", "M1.Ax.Erp.Forms.Database.Options.POSProperties" },
			{ "M1.UI.Database.Options.PRDCallProperties", "M1.Ax.Erp.Forms.Database.Options.PRDCallProperties" },
			{ "M1.UI.Database.Options.PRDContactProperties", "M1.Ax.Erp.Forms.Database.Options.PRDContactProperties" },
			{ "M1.UI.Database.Options.PRDFieldServiceProperties", "M1.Ax.Erp.Forms.Database.Options.PRDFieldServiceProperties" },
			{ "M1.UI.Database.Options.PRDGeneralProperties", "M1.Ax.Erp.Forms.Database.Options.PRDGeneralProperties" },
			{ "M1.UI.Database.Options.PRDInventoryProperties", "M1.Ax.Erp.Forms.Database.Options.PRDInventoryProperties" },
			{ "M1.UI.Database.Options.PRDJobProperties", "M1.Ax.Erp.Forms.Database.Options.PRDJobProperties" },
			{ "M1.UI.Database.Options.PRDProjectProperties", "M1.Ax.Erp.Forms.Database.Options.PRDProjectProperties" },
			{ "M1.UI.Database.Options.PRDPurchasingProperties", "M1.Ax.Erp.Forms.Database.Options.PRDPurchasingProperties" },
			{ "M1.UI.Database.Options.PRDQualityProperties", "M1.Ax.Erp.Forms.Database.Options.PRDQualityProperties" },
			{ "M1.UI.Database.Options.PRDQuoteProperties", "M1.Ax.Erp.Forms.Database.Options.PRDQuoteProperties" },
			{ "M1.UI.Database.Options.PRDSalesOrderProperties", "M1.Ax.Erp.Forms.Database.Options.PRDSalesOrderProperties" },
			{ "M1.UI.Database.Options.PRDSerialNumberProperties", "M1.Ax.Erp.Forms.Database.Options.PRDSerialNumberProperties" },
			{ "M1.UI.Database.Options.PRDShippingProperties", "M1.Ax.Erp.Forms.Database.Options.PRDShippingProperties" },
			{ "M1.UI.Database.Options.PRDTimecardProperties", "M1.Ax.Erp.Forms.Database.Options.PRDTimecardProperties" },
			{ "M1.UI.Database.Options.SFEPasswords", "M1.Ax.Erp.Forms.Database.Options.SFEPasswords" },
			{ "M1.UI.Database.Options.ShopFloorEntryProperties", "M1.Ax.Erp.Forms.Database.Options.ShopFloorEntryProperties" },
			{ "M1.UI.Database.Options.WEBAttachmentFileUploadInfo", "M1.Ax.Erp.Forms.Database.Options.WEBAttachmentFileUploadInfo" },
			{ "M1.UI.Database.Options.WEBCallsInfo", "M1.Ax.Erp.Forms.Database.Options.WEBCallsInfo" },
			{ "M1.UI.Database.Options.WEBECommerceInfo", "M1.Ax.Erp.Forms.Database.Options.WEBECommerceInfo" },
			{ "M1.UI.Database.Options.WEBGeneral", "M1.Ax.Erp.Forms.Database.Options.WEBGeneral" },
			{ "M1.UI.Database.Options.WEBHTMLStrings", "M1.Ax.Erp.Forms.Database.Options.WEBHTMLStrings" },
			{ "M1.UI.Database.Options.WEBRMARequestInfo", "M1.Ax.Erp.Forms.Database.Options.WEBRMARequestInfo" }
		};
	}
}
