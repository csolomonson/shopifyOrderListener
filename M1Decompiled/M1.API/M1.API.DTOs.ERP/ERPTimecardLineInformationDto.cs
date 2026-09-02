using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPTimecardLineInformationDto
{
	public DateTime? lmlActualEndTime { get; set; }

	public DateTime? lmlActualStartTime { get; set; }

	public byte lmlCompletionType { get; set; }

	public string lmlCreatedBy { get; set; }

	public DateTime? lmlCreatedDate { get; set; }

	public string lmlEmployeeID { get; set; }

	public Guid lmlUniqueID { get; set; }

	public string lmlExpenseID { get; set; }

	public decimal lmlGoodQuantity { get; set; }

	public string lmlIndirectLaborID { get; set; }

	public bool lmlActive { get; set; }

	public bool lmlCreatedFromPayrollSession { get; set; }

	public bool lmlLaborHoursCalculated { get; set; }

	public bool lmlMachineHoursCalculated { get; set; }

	public bool lmlPostedToWip { get; set; }

	public bool lmlSuspended { get; set; }

	public bool lmlTransferredToPayroll { get; set; }

	public int lmlJobAssemblyID { get; set; }

	public string lmlJobID { get; set; }

	public int lmlJobOperationID { get; set; }

	public decimal lmlLaborCost { get; set; }

	public string lmlLaborDescriptionRtf { get; set; }

	public string lmlLaborDescriptionText { get; set; }

	public decimal lmlLaborHours { get; set; }

	public decimal lmlMachineHours { get; set; }

	public decimal lmlOverheadCost { get; set; }

	public string lmlProcessID { get; set; }

	public string lmlProjectAreaID { get; set; }

	public string lmlProjectID { get; set; }

	public decimal lmlReworkQuantity { get; set; }

	public string lmlReworkReasonID { get; set; }

	public DateTime? lmlRoundedEndTime { get; set; }

	public DateTime? lmlRoundedStartTime { get; set; }

	public byte[] lmlRowVersion { get; set; }

	public decimal lmlScrapQuantity { get; set; }

	public string lmlScrapReasonID { get; set; }

	public short lmlTimecardLineID { get; set; }

	public short lmlSetupPercentCompleted { get; set; }

	public short lmlShiftID { get; set; }

	public byte lmlSource { get; set; }

	public int lmlTimecardID { get; set; }

	public byte lmlTimecardType { get; set; }

	public string lmlWorkCenterID { get; set; }

	public byte lmlWorkType { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
