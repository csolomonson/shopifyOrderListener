using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace M1.Core.Report;

[ComVisible(true)]
public class ReportAddress
{
	public ReportAddressDefinition TableInfo;

	public object[] AddressKeys;

	public List<object[]> DocumentKeys;

	public string CrystalFilter;

	public string SqlFilter;

	public string Email;

	public string Fax;

	public string OrganizationName;

	public string ContactName;

	public string Subject;

	public string Body;

	public string AttachmentName;
}
