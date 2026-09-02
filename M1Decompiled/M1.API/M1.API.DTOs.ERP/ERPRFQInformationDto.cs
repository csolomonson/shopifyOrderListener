using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPRFQInformationDto
{
	public string rqpBuyerEmployeeID { get; set; }

	public DateTime? rqpClosedDate { get; set; }

	public string rqpRfqID { get; set; }

	public string rqpCreatedBy { get; set; }

	public DateTime? rqpCreatedDate { get; set; }

	public DateTime? rqpDueDate { get; set; }

	public Guid rqpUniqueID { get; set; }

	public bool rqpClosed { get; set; }

	public bool rqpReadyToPrint { get; set; }

	public string rqpPlantDepartmentID { get; set; }

	public string rqpPlantID { get; set; }

	public DateTime? rqpRfqDate { get; set; }

	public byte[] rqpRowVersion { get; set; }

	public string rqpStandardMessageID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
