using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.771", "", "")]
public class v92771
{
	public v92771(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1ADDFROMSCHEDULETREES') and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Delete from DDFormDetails where deControlName In ('grpUSBox14Info','chkPafUSBox14A','chkPafUSBox14B','chkPafUSBox14C','txtPafUSBox14Description') and deFormID = 'M1.Ax.Erp.Forms.Financial.Payroll.IncomeTaxTypeView' and deCustom = 1");
	}
}
