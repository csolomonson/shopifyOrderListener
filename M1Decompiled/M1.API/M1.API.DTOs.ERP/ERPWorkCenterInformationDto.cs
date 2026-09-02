using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPWorkCenterInformationDto
{
	public byte xawCalendarColor { get; set; }

	public string xawCalendarLocation { get; set; }

	public string xawWorkCenterID { get; set; }

	public string xawCreatedBy { get; set; }

	public DateTime? xawCreatedDate { get; set; }

	public decimal xawDayStartTimeFri { get; set; }

	public decimal xawDayStartTimeMon { get; set; }

	public decimal xawDayStartTimeSat { get; set; }

	public decimal xawDayStartTimeSun { get; set; }

	public decimal xawDayStartTimeThu { get; set; }

	public decimal xawDayStartTimeTue { get; set; }

	public decimal xawDayStartTimeWed { get; set; }

	public string xawDescription { get; set; }

	public Guid xawUniqueID { get; set; }

	public decimal xawFiniteTolerance { get; set; }

	public decimal xawHoursFri { get; set; }

	public decimal xawHoursMon { get; set; }

	public decimal xawHoursSat { get; set; }

	public decimal xawHoursSun { get; set; }

	public decimal xawHoursThu { get; set; }

	public decimal xawHoursTue { get; set; }

	public decimal xawHoursWed { get; set; }

	public DateTime? xawInactiveDate { get; set; }

	public bool xawInactive { get; set; }

	public bool xawEnableCalendar { get; set; }

	public bool xawExcludeFromShopLoad { get; set; }

	public bool xawExportToCalendar { get; set; }

	public bool xawInfiniteCapacity { get; set; }

	public bool xawOutsideProcessing { get; set; }

	public bool xawSetMachineToLaborHours { get; set; }

	public bool xawSplitMachineHours { get; set; }

	public decimal xawMoveTime { get; set; }

	public short xawNumberOfMachines { get; set; }

	public byte xawOverheadCalculationType { get; set; }

	public decimal xawOverheadRate { get; set; }

	public short xawPeoplePerMachineProd { get; set; }

	public short xawPeoplePerMachineSetup { get; set; }

	public string xawPlantID { get; set; }

	public string xawProcessID { get; set; }

	public string xawProductionDepartmentID { get; set; }

	public decimal xawProductionStandard { get; set; }

	public decimal xawQueueTime { get; set; }

	public decimal xawQuotingRate { get; set; }

	public byte[] xawRowVersion { get; set; }

	public decimal xawSetupHours { get; set; }

	public string xawStandardFactor { get; set; }

	public decimal xawStartHour { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
