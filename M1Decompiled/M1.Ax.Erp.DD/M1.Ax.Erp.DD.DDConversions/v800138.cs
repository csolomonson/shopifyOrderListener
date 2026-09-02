using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.00.138", "", "")]
public class v800138
{
	public v800138(DDConversionParms parms)
	{
		parms.DmoDD.ConvertCustomControls(parms.DatabaseName, "VIEWDATASETPROPERTIES", "M1.Ax.Erp.Forms.Database.Options.GeneralSettings", "m1BindingSourceDatasetProperties", 16, 195, convertDataBindings: true, null, moveCustomControlsToBottom: false, parms.ConvertCustomFormCode);
		parms.DmoDD.ConvertCustomControls(parms.DatabaseName, "VIEWFINANCIALPROPERTIES", "M1.Ax.Erp.Forms.Database.Options.FINGeneralProperties", "m1BindingSourceFinancialProperties", 16, 346, convertDataBindings: true, null, moveCustomControlsToBottom: false, parms.ConvertCustomFormCode);
		parms.DmoDD.ConvertCustomControls(parms.DatabaseName, "VIEWPRODUCTIONPROPERTIES", "M1.Ax.Erp.Forms.Database.Options.PRDGeneralProperties", "m1BindingSourceProductionProperties", 16, 131, convertDataBindings: true, null, moveCustomControlsToBottom: false, parms.ConvertCustomFormCode);
		parms.DmoDD.ConvertCustomControls(parms.DatabaseName, "VIEWSHIPPINGPROPERTIES", "M1.Ax.Erp.Forms.Database.Options.FEDFedexGeneralProperties", "m1BindingSourceShippingProperties", 16, 250, convertDataBindings: true, null, moveCustomControlsToBottom: false, parms.ConvertCustomFormCode);
		parms.DmoDD.ConvertCustomControls(parms.DatabaseName, "VIEWSHOPFLOORENTRYPROPERTIES", "M1.Ax.Erp.Forms.Database.Options.ShopFloorEntryProperties", "m1BindingSourceProductionProperties", 16, 390, convertDataBindings: true, null, moveCustomControlsToBottom: false, parms.ConvertCustomFormCode);
		parms.DmoDD.ConvertCustomControls(parms.DatabaseName, "VIEWWEBGEARPROPERTIES", "M1.Ax.Erp.Forms.Database.Options.WEBGeneral", "m1BindingSourceWebGearProperties", 16, 193, convertDataBindings: true, null, moveCustomControlsToBottom: false, parms.ConvertCustomFormCode);
	}
}
