using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPTimecardInformationDto
{
	public DateTime? lmpActualEndTime { get; set; }

	public DateTime? lmpActualStartTime { get; set; }

	public string lmpCreatedBy { get; set; }

	public DateTime? lmpCreatedDate { get; set; }

	public string lmpEmployeeID { get; set; }

	public Guid lmpUniqueID { get; set; }

	public string lmpExchangeID { get; set; }

	public bool lmpActive { get; set; }

	public bool lmpAutoClockedOut { get; set; }

	public bool lmpCreatedFromPayrollSession { get; set; }

	public bool lmpPostedToWip { get; set; }

	public bool lmpTransferredToPayroll { get; set; }

	public DateTime? lmpLastEndTime { get; set; }

	public string lmpLeaveAccrualID { get; set; }

	public decimal lmpMachineHours { get; set; }

	public string lmpNoteRtf { get; set; }

	public string lmpNoteText { get; set; }

	public decimal lmpOtherHours { get; set; }

	public string lmpOtherPayrollRateID { get; set; }

	public decimal lmpOTPeriod1Hours { get; set; }

	public string lmpOTPeriod1PayrollRateID { get; set; }

	public decimal lmpOTPeriod2Hours { get; set; }

	public string lmpOTPeriod2PayrollRateID { get; set; }

	public decimal lmpOTPeriod3Hours { get; set; }

	public string lmpOTPeriod3PayrollRateID { get; set; }

	public decimal lmpOTPeriod4Hours { get; set; }

	public string lmpOTPeriod4PayrollRateID { get; set; }

	public DateTime? lmpPaidDate { get; set; }

	public decimal lmpPayrollHours { get; set; }

	public string lmpPlantDepartmentID { get; set; }

	public string lmpPlantID { get; set; }

	public DateTime? lmpPostedDate { get; set; }

	public string lmpProjectID { get; set; }

	public DateTime? lmpRoundedEndTime { get; set; }

	public DateTime? lmpRoundedStartTime { get; set; }

	public byte[] lmpRowVersion { get; set; }

	public int lmpTimecardID { get; set; }

	public byte lmpShiftBreakID { get; set; }

	public short lmpShiftID { get; set; }

	public byte lmpSource { get; set; }

	public decimal lmpStandardHours { get; set; }

	public string lmpStandardPayrollRateID { get; set; }

	public DateTime? lmpTimecardDate { get; set; }

	public decimal lmpTotalPayrollHours { get; set; }

	public DateTime? lmpTransferredDate { get; set; }

	public short? lmpUtcOffset { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
