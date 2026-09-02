using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPProductionCalendarDayInformationDto
{
	public byte jmyDayOfWeek { get; set; }

	public decimal jmyDayStartTime { get; set; }

	public decimal jmyHours { get; set; }

	public bool jmyHoliday { get; set; }

	public string jmyPlantID { get; set; }

	public byte jmyProductionCalendarDay { get; set; }

	public byte jmyProductionCalendarMonth { get; set; }

	public short jmyProductionCalendarYearID { get; set; }

	public byte[] jmyRowVersion { get; set; }

	public string jmyWorkCenterID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
