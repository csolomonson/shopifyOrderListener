using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPEmployeeInformationDto
{
	public string lmeCallTypeID { get; set; }

	public string lmeCessationType { get; set; }

	public string lmeEmployeeID { get; set; }

	public decimal lmeCommissionRate { get; set; }

	public string lmeContactTitleID { get; set; }

	public string lmeCountyCodeID { get; set; }

	public string lmeCreatedBy { get; set; }

	public DateTime? lmeCreatedDate { get; set; }

	public short lmeDefaultShiftID { get; set; }

	public string lmeDefaultWorkCenterID { get; set; }

	public string lmeDirectExpenseID { get; set; }

	public byte lmeEarningType { get; set; }

	public string lmeEmployeeName { get; set; }

	public Guid lmeUniqueID { get; set; }

	public DateTime? lmeHireDate { get; set; }

	public string lmeHomeProductionDepartmentID { get; set; }

	public string lmeIndirectExpenseID { get; set; }

	public bool lmeBuyerEmployee { get; set; }

	public bool lmeEngineerEmployee { get; set; }

	public bool lmeInspectorEmployee { get; set; }

	public bool lmeLockShift { get; set; }

	public bool lmePayrollEmployee { get; set; }

	public bool lmePlannerEmployee { get; set; }

	public bool lmeProjectManagerEmployee { get; set; }

	public bool lmeQuoterEmployee { get; set; }

	public bool lmeSalesEmployee { get; set; }

	public bool lmeShopEmployee { get; set; }

	public bool lmeSortSfebyWorkcenter { get; set; }

	public bool lmeSupportEmployee { get; set; }

	public string lmeLanguage { get; set; }

	public string lmePassword { get; set; }

	public string lmePlantDepartmentID { get; set; }

	public string lmePlantID { get; set; }

	public decimal lmePoApprovalAmount { get; set; }

	public string lmePreviousEmployeeID { get; set; }

	public byte[] lmeRowVersion { get; set; }

	public decimal lmeSOApprovalAmount { get; set; }

	public DateTime? lmeTerminationDate { get; set; }

	public string lmeTerminationReasonID { get; set; }

	public byte lmeUseEmail { get; set; }

	public byte lmeUseEmailPayslips { get; set; }

	public string lmeUserID { get; set; }

	public string lmeWorkEmailAddress { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
