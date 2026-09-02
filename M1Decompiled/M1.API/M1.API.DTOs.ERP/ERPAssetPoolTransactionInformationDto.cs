using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAssetPoolTransactionInformationDto
{
	public decimal fawAmount { get; set; }

	public int fawAssetAdjustmentID { get; set; }

	public string fawAssetID { get; set; }

	public string fawCreatedBy { get; set; }

	public DateTime? fawCreatedDate { get; set; }

	public Guid fawUniqueID { get; set; }

	public int fawPoolTransactionID { get; set; }

	public short fawPoolYearID { get; set; }

	public byte[] fawRowVersion { get; set; }

	public DateTime? fawTransactionDate { get; set; }

	public string fawTransactionType { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
