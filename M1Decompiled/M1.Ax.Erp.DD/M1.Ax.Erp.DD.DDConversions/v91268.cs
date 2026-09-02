using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.268", "", "")]
public class v91268
{
	public v91268(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Delete From DDFormDetails where (deControlName In ('cmbImoOverlap', 'cmbQmoOverlap', 'chkXawIgnoreCalendarQueue', 'chkXawIgnoreCalendarMove', 'txtSun', 'txtMon', 'txtTue', 'txtWed', 'txtThu', 'txtFri', 'txtSat', 'txtHrs', 'txtXawHoursSun', 'txtXawHoursMon', 'txtXawHoursTue', 'txtXawHoursWed', 'txtXawHoursThu', 'txtXawHoursFri','txtXawHoursSat', 'txtStart', 'txtXawDayStartTimeSun', 'txtXawDayStartTimeMon', 'txtXawDayStartTimeTue', 'txtXawDayStartTimeWed', 'txtXawDayStartTimeThu', 'txtXawDayStartTimeFri', 'txtXawDayStartTimeSat', 'chkXawIgnoreCalendarQueue','CMBJMOOVERLAP', 'txtJmoOverlapJobOperationID') and deCustom = 0) Or (deControlName = 'm1MaskedTextEditor1' and deFormID = 'M1.Ax.Erp.Forms.Production.Job.WorkCenterView' and deCustom = 0) Or (deControlName = 'objSerialNumbersRework' and deCustom = 0)");
	}
}
