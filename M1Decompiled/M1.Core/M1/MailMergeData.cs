using System.Data;

namespace M1;

public class MailMergeData
{
	public DataTable SourceData;

	public string MissingFields;

	public DataRow[] ContactsWithValidEmailAddresses;

	public DataRow[] ContactsWithEmptyEmailAddresses;
}
