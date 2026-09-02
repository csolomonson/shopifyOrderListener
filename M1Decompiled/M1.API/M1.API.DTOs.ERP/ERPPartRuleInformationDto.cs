using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartRuleInformationDto
{
	public string pcrCode { get; set; }

	public string pcrCreatedBy { get; set; }

	public DateTime? pcrCreatedDate { get; set; }

	public Guid pcrUniqueID { get; set; }

	public string pcrField { get; set; }

	public int pcrMethodAssemblyID { get; set; }

	public string pcrMethodID { get; set; }

	public int pcrMethodMaterialID { get; set; }

	public int pcrMethodOperationID { get; set; }

	public string pcrMethodRevisionID { get; set; }

	public byte pcrMethodType { get; set; }

	public short pcrProcessSequence { get; set; }

	public byte[] pcrRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
