using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPNonConformanceInformationDto
{
	public decimal qarActualHours { get; set; }

	public string qarNonConformanceID { get; set; }

	public string qarCorrectiveActionCategoryID { get; set; }

	public string qarCorrectiveActionCodeID { get; set; }

	public DateTime? qarCorrectiveActionDate { get; set; }

	public string qarCorrectiveActionRTF { get; set; }

	public string qarCorrectiveActionText { get; set; }

	public byte qarCorrectiveActionType { get; set; }

	public string qarCreatedBy { get; set; }

	public DateTime? qarCreatedDate { get; set; }

	public Guid qarUniqueID { get; set; }

	public decimal qarHoursAllowed { get; set; }

	public decimal qarHoursRequested { get; set; }

	public string qarInspectionID { get; set; }

	public short qarInspectionLineID { get; set; }

	public bool qarCorrectiveActionComplete { get; set; }

	public int qarJobAssemblyID { get; set; }

	public string qarJobID { get; set; }

	public int qarJobMaterialID { get; set; }

	public int qarJobOperationID { get; set; }

	public string qarNonConformanceCategoryID { get; set; }

	public string qarNonConformanceCauseID { get; set; }

	public string qarNonConformanceCodeID { get; set; }

	public string qarNonConformanceRTF { get; set; }

	public string qarNonConformanceText { get; set; }

	public string qarPartBinID { get; set; }

	public string qarPartID { get; set; }

	public string qarPartRevisionID { get; set; }

	public string qarPartShortDescription { get; set; }

	public string qarPartWareHouseLocationID { get; set; }

	public decimal qarQuantity { get; set; }

	public string qarRepairedByOrganizationID { get; set; }

	public string qarReportedByEmployeeID { get; set; }

	public string qarRmaClaimID { get; set; }

	public short qarRmaClaimLineID { get; set; }

	public byte[] qarRowVersion { get; set; }

	public decimal qarSubcontractAmount { get; set; }

	public decimal qarSubcontractAmountForeign { get; set; }

	public string qarUnitOfMeasure { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
