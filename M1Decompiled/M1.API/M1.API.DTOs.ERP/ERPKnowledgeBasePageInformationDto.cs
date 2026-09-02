using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPKnowledgeBasePageInformationDto
{
	public decimal kbbAccessedCount { get; set; }

	public string kbbClosedByEmployeeID { get; set; }

	public DateTime? kbbClosedDate { get; set; }

	public string kbbKnowledgeBasePageID { get; set; }

	public string kbbCreatedBy { get; set; }

	public DateTime? kbbCreatedDate { get; set; }

	public string kbbDescription { get; set; }

	public Guid kbbUniqueID { get; set; }

	public string kbbOpenedByEmployeeID { get; set; }

	public DateTime? kbbOpenedDate { get; set; }

	public string kbbPartID { get; set; }

	public string kbbPartRevisionID { get; set; }

	public string kbbProblemDescriptionRtf { get; set; }

	public string kbbProblemDescriptionText { get; set; }

	public string kbbResolutionDescriptionRtf { get; set; }

	public string kbbResolutionDescriptionText { get; set; }

	public string kbbResolvedPartID { get; set; }

	public string kbbResolvedPartRevisionID { get; set; }

	public byte[] kbbRowVersion { get; set; }

	public string kbbStatus { get; set; }

	public string kbbWorkAroundDescriptionRtf { get; set; }

	public string kbbWorkAroundDescriptionText { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
