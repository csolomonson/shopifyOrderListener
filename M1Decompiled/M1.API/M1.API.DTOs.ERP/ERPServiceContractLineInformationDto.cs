using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPServiceContractLineInformationDto
{
	public short kbnContractLength { get; set; }

	public string kbnContractLengthType { get; set; }

	public string kbnCreatedBy { get; set; }

	public DateTime? kbnCreatedDate { get; set; }

	public DateTime? kbnEndDate { get; set; }

	public Guid kbnUniqueID { get; set; }

	public string kbnPartID { get; set; }

	public string kbnPartRevisionID { get; set; }

	public string kbnPartShortDescription { get; set; }

	public byte[] kbnRowVersion { get; set; }

	public short kbnServiceContractLineID { get; set; }

	public string kbnSerialNumberID { get; set; }

	public string kbnServiceContractID { get; set; }

	public DateTime? kbnStartDate { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
