using System;
using System.Data;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1;

public static class MailMerge
{
	private static void ProcessMergeRecords(DataTable data, string outputType, string description, string templateFile, object attachments)
	{
		if (!outputType.StartsWith("Email", StringComparison.CurrentCultureIgnoreCase))
		{
			outputType.Equals("Printer", StringComparison.CurrentCultureIgnoreCase);
		}
	}

	private static string GetDocumentFields()
	{
		return string.Empty;
	}

	private static MailMergeData GetDataForMerge(IServiceProvider provider, object data, string fieldList, string outputType, string description, string orderBy)
	{
		MailMergeData mailMergeData = new MailMergeData();
		if (fieldList.IndexOf(',') == -1)
		{
			fieldList += ",cmcOrganizationID,cmcLocationID";
			if (fieldList.StartsWith(","))
			{
				fieldList = fieldList.Substring(1);
			}
		}
		if (fieldList.Length != 0)
		{
			fieldList = "," + fieldList + ",";
			if (fieldList.IndexOf(",cmcOrganizationID,", StringComparison.CurrentCultureIgnoreCase) == -1)
			{
				fieldList += "cmcOrganizationID,";
			}
			if (fieldList.IndexOf(",cmcLocationID,", StringComparison.CurrentCultureIgnoreCase) == -1)
			{
				fieldList += "cmcLocationID,";
			}
			if (fieldList.IndexOf(",cmcContactID,", StringComparison.CurrentCultureIgnoreCase) == -1)
			{
				fieldList += "cmcContactID,";
			}
			if (fieldList.IndexOf(",cmcName,", StringComparison.CurrentCultureIgnoreCase) == -1)
			{
				fieldList += "cmcName,";
			}
			if (outputType.StartsWith("Email", StringComparison.CurrentCultureIgnoreCase) && fieldList.IndexOf(",cmcEmailAddress,", StringComparison.CurrentCultureIgnoreCase) == -1)
			{
				fieldList += "cmcEmailAddress,";
			}
			fieldList = fieldList.Substring(1);
			fieldList = fieldList.Substring(0, fieldList.Length - 1);
			if (data.GetType() == typeof(DataTable))
			{
				StringBuilder stringBuilder = new StringBuilder();
				mailMergeData.SourceData = (DataTable)data;
				string[] array = fieldList.Split(',');
				foreach (string text in array)
				{
					if (!mailMergeData.SourceData.Columns.Contains(text))
					{
						stringBuilder.AppendLine(text);
					}
				}
				mailMergeData.MissingFields = stringBuilder.ToString();
			}
			else
			{
				string text2 = ((string)data).Trim();
				string text3 = string.Empty;
				if (text2.StartsWith("Where ", StringComparison.CurrentCultureIgnoreCase))
				{
					text2 = text2.Substring(6);
				}
				else if (text2.StartsWith("From ", StringComparison.CurrentCultureIgnoreCase))
				{
					text3 = text2.Substring(5);
					text2 = string.Empty;
				}
				string text4 = createFromClause(fieldList, text2, description);
				text2 = ((!string.IsNullOrWhiteSpace(text2)) ? (text2 + " And cmcInactive = 0") : "cmcInactive = 0");
				string queryString = "Select " + fieldList + " From " + text4 + text3 + (string.IsNullOrWhiteSpace(text2) ? string.Empty : (" Where " + text2)) + (string.IsNullOrWhiteSpace(orderBy) ? string.Empty : (" Order By " + orderBy));
				M1Database m1Database = provider.GetService(typeof(M1Database)) as M1Database;
				mailMergeData.SourceData = m1Database.GetDataTable(queryString);
			}
			if (mailMergeData.SourceData.Columns.Contains("cmcEmailAddress"))
			{
				mailMergeData.ContactsWithValidEmailAddresses = mailMergeData.SourceData.Select("cmcEmailAddress <> null And cmcEmailAddress <> ''");
				if (mailMergeData.ContactsWithValidEmailAddresses.Length == mailMergeData.SourceData.Rows.Count)
				{
					mailMergeData.ContactsWithEmptyEmailAddresses = new DataRow[0];
				}
				else
				{
					mailMergeData.ContactsWithValidEmailAddresses = mailMergeData.SourceData.Select("cmcEmailAddress = null Or cmcEmailAddress = ''");
				}
			}
		}
		return mailMergeData;
	}

	private static string createFromClause(string fieldList, string where, string subject)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder2.Append(" Organizations inner join OrganizationLocations on cmoOrganizationID=cmlOrganizationID inner join OrganizationContacts on cmlOrganizationID = cmcOrganizationID And cmlLocationID = cmcLocationID ");
		if (!string.IsNullOrWhiteSpace(fieldList))
		{
			string[] array = fieldList.Split(',');
			foreach (string text in array)
			{
				string text2 = "";
				if (text.StartsWith("xad", StringComparison.CurrentCultureIgnoreCase) || text.StartsWith("uxad", StringComparison.CurrentCultureIgnoreCase))
				{
					flag = true;
				}
				else if (text.StartsWith("u", StringComparison.CurrentCultureIgnoreCase))
				{
					if (text.Equals("uSubject", StringComparison.CurrentCultureIgnoreCase))
					{
						text2 = M1Util.ConvertToSql(subject) + " As ";
					}
					else if (text.Equals("uUserName", StringComparison.CurrentCultureIgnoreCase))
					{
						text2 = "lmeEmployeeName As ";
						flag2 = true;
					}
					else if (text.Equals("uUserTitle", StringComparison.CurrentCultureIgnoreCase))
					{
						text2 = "EmployeeTitles.cmeDescription As ";
						flag2 = true;
					}
					else if (text.Equals("uUserEmailAddress", StringComparison.CurrentCultureIgnoreCase))
					{
						text2 = "lmeWorkEmailAddress As ";
						flag2 = true;
					}
					else if (text.Equals("uContactFirstName", StringComparison.CurrentCultureIgnoreCase))
					{
						text2 = "Left(Replace(Replace(Replace(Replace(Replace(Replace(cmcName,'Ms ',''),'Ms. ',''),'Mrs ',''),'Mrs. ',''),'Mr ',''),'Mr. ',''), Case When CharIndex(' ',Replace(Replace(Replace(Replace(Replace(Replace(cmcName,'Ms ',''),'Ms. ',''),'Mrs ',''),'Mrs. ',''),'Mr ',''),'Mr. ','')) = 0 THEN 50 ELSE CharIndex(' ',Replace(Replace(Replace(Replace(Replace(Replace(cmcName,'Ms ',''),'Ms. ',''),'Mrs ',''),'Mrs. ',''),'Mr ',''),'Mr. ','')) END) As ";
					}
					else if (text.Equals("uContactLastName", StringComparison.CurrentCultureIgnoreCase))
					{
						text2 = "LTrim(Right(Replace(Replace(Replace(Replace(Replace(Replace(cmcName,'Ms ',''),'Ms. ',''),'Mrs ',''),'Mrs. ',''),'Mr ',''),'Mr. ',''),CharIndex(' ',Reverse(Replace(Replace(Replace(Replace(Replace(Replace(cmcName,'Ms ',''),'Ms. ',''),'Mrs ',''),'Mrs. ',''),'Mr ',''),'Mr. ',''))))) As ";
					}
					else if (text.Equals("uContactTitle", StringComparison.CurrentCultureIgnoreCase))
					{
						text2 = "ContactTitles.cmeDescription As ";
						flag3 = true;
					}
				}
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append(text2 + text);
			}
			fieldList = stringBuilder.ToString();
			if (flag3)
			{
				stringBuilder2.Append(" Left Outer Join ContactTitles ContactTitles On cmcContactTitleID = ContactTitles.cmeContactTitleID ");
			}
			if (flag2)
			{
				stringBuilder2.Insert(0, " Employees Left Outer Join ContactTitles EmployeeTitles On lmeContactTitleID = EmployeeTitles.cmeContactTitleID , ");
			}
			if (flag)
			{
				stringBuilder2.Insert(0, " DatasetProperties, ");
			}
		}
		return stringBuilder2.ToString();
	}
}
