using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPServiceContractInformationDto
{
	public string kbsServiceContractID { get; set; }

	public decimal kbsContractAmount { get; set; }

	public short kbsContractLength { get; set; }

	public string kbsContractLengthType { get; set; }

	public string kbsCreatedBy { get; set; }

	public DateTime? kbsCreatedDate { get; set; }

	public string kbsDescription { get; set; }

	public DateTime? kbsEndDate { get; set; }

	public Guid kbsUniqueID { get; set; }

	public string kbsLongDescriptionRtf { get; set; }

	public string kbsLongDescriptionText { get; set; }

	public string kbsOrganizationID { get; set; }

	public string kbsPartID { get; set; }

	public string kbsPartRevisionID { get; set; }

	public string kbsPartShortDescription { get; set; }

	public string kbsProjectAreaID { get; set; }

	public string kbsProjectID { get; set; }

	public string kbsResellerOrganizationID { get; set; }

	public byte[] kbsRowVersion { get; set; }

	public string kbsSerialNumberID { get; set; }

	public string kbsServiceContractTypeID { get; set; }

	public DateTime? kbsStartDate { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
