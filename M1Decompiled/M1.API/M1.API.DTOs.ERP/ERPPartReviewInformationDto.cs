using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartReviewInformationDto
{
	public string wgrComments { get; set; }

	public string wgrPartID { get; set; }

	public int wgrRating { get; set; }

	public string wgrReviewerEmailAddress { get; set; }

	public string wgrReviewerName { get; set; }

	public byte[] wgrRowVersion { get; set; }

	public int wgrPartReviewID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
