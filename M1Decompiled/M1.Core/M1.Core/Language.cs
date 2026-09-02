using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using M1.Extensions;

namespace M1.Core;

public class Language
{
	private M1DataDictionary dataDictionary;

	private AppContext currentContext;

	public string LanguageTable = string.Empty;

	public string LanguageRegion = string.Empty;

	public Language(M1DataDictionary m1DataDictionary, AppContext context)
	{
		dataDictionary = m1DataDictionary;
		currentContext = context;
	}

	private string GetRegion(M1DataDictionary dataDictionary)
	{
		string result = dataDictionary.Region;
		using (DataTable dataTable = dataDictionary.GetDataTable("Select ddRegion From DDInfo"))
		{
			if (dataTable.Rows.Count != 0)
			{
				result = dataTable.Rows[0].Field<string>("ddRegion").Trim();
			}
		}
		return result;
	}

	public void CheckForLanguage()
	{
		string ddLanguageField = string.Empty;
		using (DataTable dataTable = dataDictionary.GetDataTable("Select ddLanguage From DDInfo"))
		{
			if (dataTable.Rows.Count != 0)
			{
				ddLanguageField = dataTable.Rows[0].Field<string>("ddLanguage").Trim();
			}
		}
		CheckForLanguage(ddLanguageField);
	}

	public void CheckForLanguage(string ddLanguageField)
	{
		LanguageTable = string.Empty;
		string text = ddLanguageField.Trim();
		if (text.Length != 0 && DoesLanguageTableExist(text))
		{
			LanguageTable = text.ToUpper();
		}
	}

	public bool DoesLanguageTableExist(string table)
	{
		return currentContext.DDServerManager.DoesTableExist(null, null, dataDictionary.ID, table, null);
	}

	private string getLanguageTable(M1Database m1Database)
	{
		if (m1Database != null)
		{
			LanguageRegion = m1Database.Region;
		}
		if (m1Database != null && m1Database.LanguageTable.Length != 0)
		{
			return m1Database.LanguageTable;
		}
		return LanguageTable;
	}

	public string GetdfCaptionJoin(M1Database m1Database)
	{
		return GetdfCaptionJoin(m1Database, string.Empty);
	}

	public string GetdfCaptionJoin(M1Database m1Database, string ddFieldsAlias)
	{
		if (ddFieldsAlias.Length != 0 && !ddFieldsAlias.EndsWith("."))
		{
			ddFieldsAlias += ".";
		}
		string languageTable = getLanguageTable(m1Database);
		if (languageTable.Length != 0)
		{
			return " Left Outer Join " + languageTable + " DDLangDfCaption With(NoLock) On DDLangDfCaption.dnID = " + ddFieldsAlias + "dfField And DDLangDfCaption.dnSource = 'DDFIELDS' And DDLangDfCaption.dnType = 'DFCAPTION' ";
		}
		return string.Empty;
	}

	public string GetdfCaptionField(M1Database m1Database)
	{
		return GetdfCaptionField(m1Database, string.Empty);
	}

	public string GetdfCaptionField(M1Database m1Database, string ddFieldsAlias)
	{
		return GetdfCaptionField(m1Database, ddFieldsAlias, removeAsClause: false);
	}

	public string GetdfCaptionField(M1Database m1Database, string ddFieldsAlias, bool removeAsClause)
	{
		if (ddFieldsAlias.Length != 0 && !ddFieldsAlias.EndsWith("."))
		{
			ddFieldsAlias += ".";
		}
		if (getLanguageTable(m1Database).Length != 0)
		{
			return "IsNull(DDLangDfCaption.dnText," + ddFieldsAlias + "dfCaption) " + (removeAsClause ? "" : " As dfCaption");
		}
		return ddFieldsAlias + "dfCaption";
	}

	public string GetdtCaptionField(M1Database m1Database)
	{
		return GetdtCaptionField(m1Database, removeAsClause: false);
	}

	public string GetdtCaptionField(M1Database m1Database, bool removeAsClause)
	{
		return GetdtCaptionField(m1Database, removeAsClause, string.Empty);
	}

	public string GetdtCaptionField(M1Database m1Database, bool removeAsClause, string alias)
	{
		if (alias.Length != 0)
		{
			alias += ".";
		}
		if (getLanguageTable(m1Database).Length != 0)
		{
			return "IsNull(DDLangDtCaption.dnText," + alias + "dtCaption) " + (removeAsClause ? "" : " As dtCaption");
		}
		return alias + "dtCaption";
	}

	public string GetdtCaptionJoin(M1Database m1Database)
	{
		return GetdtCaptionJoin(m1Database, string.Empty);
	}

	public string GetdtCaptionJoin(M1Database m1Database, string alias)
	{
		string languageTable = getLanguageTable(m1Database);
		if (languageTable.Length != 0)
		{
			if (alias.Length != 0)
			{
				alias += ".";
			}
			return " Left Outer Join " + languageTable + " DDLangDtCaption With(Nolock) On DDLangDtCaption.dnID = '" + alias + "dtTable' And DDLangDtCaption.dnSource = 'DDTABLES' And DDLangDtCaption.dnType = 'DTCAPTION' ";
		}
		return string.Empty;
	}

	public string GetdxTextField(M1Database m1Database)
	{
		if (getLanguageTable(m1Database).Length != 0)
		{
			return "IsNull(DDLangDxText.dnText,dxText) As dxText";
		}
		return "dxText";
	}

	public string GetdxTextField(M1Database m1Database, string alias)
	{
		if (getLanguageTable(m1Database).Length != 0)
		{
			return "IsNull(DDLangDxText.dnText," + ((alias.Length == 0) ? "" : (alias + ".")) + "dxText) As dxText";
		}
		return ((alias.Length == 0) ? "" : (alias + ".")) + "dxText";
	}

	public string GetdxTextJoin(M1Database m1Database)
	{
		string languageTable = getLanguageTable(m1Database);
		if (languageTable.Length != 0)
		{
			return " Left Outer Join " + languageTable + " DDLangDxText With(Nolock) On DDLangDxText.dnID = LTrim(CAST(dxUniqueID as varchar(50))) And DDLangDxText.dnSource = 'DDEXPLORER' And DDLangDxText.dnType = 'DXTEXT' ";
		}
		return string.Empty;
	}

	public string GetdxTextJoin(M1Database m1Database, string alias)
	{
		string languageTable = getLanguageTable(m1Database);
		if (languageTable.Length != 0)
		{
			return " Left Outer Join " + languageTable + " DDLangDxText With(Nolock) On DDLangDxText.dnID = LTrim(CAST(" + ((alias.Length == 0) ? "" : (alias + ".")) + "dxUniqueID as varchar(50))) And DDLangDxText.dnSource = 'DDEXPLORER' And DDLangDxText.dnType = 'DXTEXT' ";
		}
		return string.Empty;
	}

	public string GetdoNameField(M1Database m1Database)
	{
		if (getLanguageTable(m1Database).Length != 0)
		{
			return "IsNull(DDLangDoName.dnText,doName) As doName";
		}
		return "doName";
	}

	public string GetdoNameJoin(M1Database m1Database)
	{
		string languageTable = getLanguageTable(m1Database);
		if (languageTable.Length != 0)
		{
			return " Left Outer Join " + languageTable + " DDLangDoName With(Nolock) On DDLangDoName.dnID = doObjectID And DDLangDoName.dnSource = 'DDOBJECTS' And DDLangDoName.dnType = 'DONAME' ";
		}
		return string.Empty;
	}

	public string GetdoTitleField(M1Database m1Database)
	{
		if (getLanguageTable(m1Database).Length != 0)
		{
			return "IsNull(DDLangDoTitle.dnText,doTitle) As doTitle";
		}
		return "doTitle";
	}

	public string GetdoTitleJoin(M1Database m1Database)
	{
		return GetdoTitleJoin(m1Database, string.Empty);
	}

	public string GetdoTitleJoin(M1Database m1Database, string alias)
	{
		string languageTable = getLanguageTable(m1Database);
		if (languageTable.Length != 0)
		{
			return " Left Outer Join " + languageTable + " DDLangDoTitle With(Nolock) On DDLangDoTitle.dnID = " + ((alias.Length == 0) ? "" : (alias + ".")) + "doObjectID And DDLangDoTitle.dnSource = 'DDOBJECTS' And DDLangDoTitle.dnType = 'DOTITLE' ";
		}
		return string.Empty;
	}

	public string GetdmCaptionField(M1Database m1Database)
	{
		if (getLanguageTable(m1Database).Length != 0)
		{
			return "IsNull(DDLangDmCaption.dnText,dmCaption) As dmCaption";
		}
		return "dmCaption";
	}

	public string GetdmCaptionJoin(M1Database m1Database)
	{
		string languageTable = getLanguageTable(m1Database);
		if (languageTable.Length != 0)
		{
			return " Left Outer Join " + languageTable + " DDLangDmCaption With(Nolock) On DDLangDmCaption.dnID = dmFormID And DDLangDmCaption.dnSource = 'DDFORMS' And DDLangDmCaption.dnType = 'DMCAPTION' ";
		}
		return string.Empty;
	}

	public string GetdjDescJoin(M1Database m1Database)
	{
		string languageTable = getLanguageTable(m1Database);
		if (languageTable.Length != 0)
		{
			return " Left Outer Join " + languageTable + " DDLangDjDesc With(Nolock) On DDLangDjDesc.dnID = djGridID And DDLangDjDesc.dnSource = 'DDGRIDS' And DDLangDjDesc.dnType = 'DJDESC' ";
		}
		return string.Empty;
	}

	public string GetdjDescField(M1Database m1Database)
	{
		return GetdjDescField(m1Database, includeFieldAlias: true);
	}

	public string GetdjDescField(M1Database m1Database, bool includeFieldAlias)
	{
		if (getLanguageTable(m1Database).Length != 0)
		{
			if (includeFieldAlias)
			{
				return "IsNull(DDLangDjDesc.dnText,djDesc) As djDesc";
			}
			return "IsNull(DDLangDjDesc.dnText,djDesc) ";
		}
		return "djDesc";
	}

	public string GetdgSPTextField(M1Database m1Database)
	{
		if (getLanguageTable(m1Database).Length != 0)
		{
			return "IsNull(DDLangDgSPText.dnText,dgSPText) As dgSPText";
		}
		return "dgSPText";
	}

	public string GetdgSPTextJoin(M1Database m1Database)
	{
		string languageTable = getLanguageTable(m1Database);
		if (languageTable.Length != 0)
		{
			return " Left Outer Join " + languageTable + " DDLangDgSPText With(Nolock) On DDLangDgSPText.dnID = dgGridID And DDLangDgSPText.dnSource = 'DDGRIDDETAILS' And DDLangDgSPText.dnType = 'DGSPTEXT' ";
		}
		return string.Empty;
	}

	public string GetdfStatusField(M1Database m1Database)
	{
		if (getLanguageTable(m1Database).Length != 0)
		{
			return "IsNull(DDLangDfStatus.dnText,dfStatus) As dfStatus";
		}
		return "dfStatus";
	}

	public string GetdfStatusJoin(M1Database m1Database)
	{
		string languageTable = getLanguageTable(m1Database);
		if (languageTable.Length != 0)
		{
			return " Left Outer Join " + languageTable + " DDLangDfStatus With(Nolock) On DDLangDfStatus.dnID = dfField And DDLangDfStatus.dnSource = 'DDFIELDS' And DDLangDfStatus.dnType = 'DFSTATUS' ";
		}
		return string.Empty;
	}

	public string GetdwDescField(M1Database m1Database)
	{
		if (getLanguageTable(m1Database).Length != 0)
		{
			return "IsNull(DDLangDwDesc.dnText,dwDesc) As dwDesc";
		}
		return "dwDesc";
	}

	public string GetdwDescJoin(M1Database m1Database)
	{
		string languageTable = getLanguageTable(m1Database);
		if (languageTable.Length != 0)
		{
			return " Left Outer Join " + languageTable + " DDLangDwDesc With(Nolock) On DDLangDwDesc.dnID = dwID And DDLangDwDesc.dnSource = 'DDOPENWITHS' And DDLangDwDesc.dnType = 'DWDESC' ";
		}
		return string.Empty;
	}

	public string GetLanguageText(M1Database m1Database, string languageID)
	{
		return GetLanguageText(m1Database, languageID, string.Empty, null, string.Empty);
	}

	public string GetLanguageText(M1Database m1Database, string languageID, string defaultText)
	{
		return GetLanguageText(m1Database, languageID, defaultText, null, string.Empty);
	}

	public string GetLanguageText(M1Database m1Database, string languageID, string defaultText, Array parms)
	{
		return GetLanguageText(m1Database, languageID, defaultText, parms, string.Empty);
	}

	public string GetLanguageText(M1Database m1Database, string languageID, string defaultText, Array parms, string type)
	{
		string languageTable = getLanguageTable(m1Database);
		string text = defaultText;
		if (languageTable.Length != 0)
		{
			languageID = languageID.Trim().ToUpper();
			if (languageID.Length > 0)
			{
				DataTable dataTable = dataDictionary.GetDataTable("Select Case When dnCustText = '' Then dnText Else dnCustText End As dnText From " + languageTable + " Where dnSource = 'MISCELLANEOUS' And dnID = " + languageID.ToSql());
				if (dataTable.Rows.Count > 0)
				{
					text = dataTable.Rows[0].Field<string>("dnText").Trim();
				}
			}
		}
		if (parms != null)
		{
			int num = 0;
			string empty = string.Empty;
			foreach (object parm in parms)
			{
				empty = ((parm == null) ? string.Empty : parm.ToString());
				num = text.IndexOf('%');
				if (num != -1)
				{
					text = text.Substring(0, num) + empty + text.Substring(num + 1);
				}
			}
		}
		return text;
	}

	public string GetFormCaption(M1Database m1Database, string formID, string defaultText)
	{
		return GetFormCaption(m1Database, formID, defaultText, null);
	}

	public string GetFormCaption(M1Database m1Database, string formID, string defaultText, Array parms)
	{
		string languageTable = getLanguageTable(m1Database);
		string text = defaultText;
		if (languageTable.Length != 0)
		{
			formID = formID.Trim().ToUpper();
			if (formID.Length > 0)
			{
				DataTable dataTable = dataDictionary.GetDataTable("Select IsNull(Case When IsNull(dnCustText,'') = '' Then dnText Else dnCustText End,dmCaption) As dmCaption From DDForms Left outer Join " + languageTable + " On dmFormID = dnID And dnSource = 'DDFORMS' And dnType = 'DMCAPTION' Where dmFormID = " + formID.ToSql());
				if (dataTable.Rows.Count > 0)
				{
					text = dataTable.Rows[0].Field<string>("dmCaption").Trim();
				}
			}
		}
		if (parms != null)
		{
			string empty = string.Empty;
			int num = 0;
			foreach (object parm in parms)
			{
				empty = ((parm == null) ? string.Empty : parm.ToString());
				num = text.IndexOf('%');
				if (num != -1)
				{
					text = text.Substring(0, num) + empty + text.Substring(num + 1);
				}
			}
		}
		return text;
	}

	public string GetFieldCaption(M1Database m1Database, string sField)
	{
		string result = string.Empty;
		string text = getLanguageTable(m1Database).Trim();
		sField = sField.Trim();
		if (!string.IsNullOrEmpty(sField))
		{
			if (text.Length != 0)
			{
				DataTable dataTable = dataDictionary.GetDataTable(dataDictionary.NewSqlCommand("Select IsNull(Case When IsNull(dnCustText,'') = '' Then dnText Else dnCustText End,dfCaption) As dfCaption From DDFields Left outer Join " + text + " On dfField = dnID And dnSource = 'DDFIELDS' And dnType = 'DFCAPTION' Where dfField = " + sField.ToSql()));
				if (dataTable.Rows.Count > 0)
				{
					result = dataTable.Rows[0]["dfCaption"].ToString().Trim();
				}
				dataTable.Clear();
			}
			else
			{
				DataTable dataTable2 = dataDictionary.GetDataTable(dataDictionary.NewSqlCommand("Select dfCaption From DDFields Where dfField = " + sField.ToSql()));
				if (dataTable2.Rows.Count > 0)
				{
					result = dataTable2.Rows[0]["dfCaption"].ToString().Trim();
				}
				dataTable2.Clear();
			}
		}
		return result;
	}

	public void DropLanguageTable(string table)
	{
		table = table.Trim();
		if (table.Length != 0)
		{
			if (table.Equals(LanguageTable, StringComparison.CurrentCultureIgnoreCase))
			{
				LanguageTable = string.Empty;
			}
			if (DoesLanguageTableExist(table))
			{
				new Dmo(currentContext, currentContext.DDServerManager).DropTable(null, null, dataDictionary.ID, table);
			}
		}
	}

	public void ReloadLanguageTable(string table)
	{
		new DmoDD(currentContext).ReloadTable(dataDictionary.ID, table, recreateTable: false, null, null);
	}

	public void CreateLanguageTable(string table)
	{
		table = table.Trim();
		if (DoesLanguageTableExist(table))
		{
			new Dmo(currentContext, currentContext.DDServerManager).DropTable(null, null, dataDictionary.ID, table);
		}
		string queryString = "CREATE TABLE dbo." + table + " (dnID nvarchar(55) NOT NULL DEFAULT(''), dnType nvarchar(15) NOT NULL DEFAULT(''), dnSource nvarchar(15) NOT NULL DEFAULT(''), dnLength smallint NOT NULL DEFAULT 0, dnText nvarchar(500) NOT NULL DEFAULT(''), dnCustText nvarchar(500) NOT NULL DEFAULT(''))";
		dataDictionary.ExecuteCommand(queryString);
		queryString = "CREATE INDEX dnSource ON " + table + " (dnSource)\rCREATE INDEX dnID ON " + table + " (dnID)\rCREATE INDEX dnType ON " + table + " (dnType)\rCREATE UNIQUE INDEX MainKey ON " + table + " (dnSource,dnType,dnID)";
		dataDictionary.ExecuteCommand(queryString);
	}

	public string GetLocalString(string textToCheck)
	{
		if (LanguageRegion == "")
		{
			LanguageRegion = dataDictionary.Region;
		}
		if (!string.IsNullOrWhiteSpace(textToCheck))
		{
			if (LanguageRegion == "US")
			{
				textToCheck = textToCheck.Replace("Labour", "Labor").Replace("labour", "labor", caseInsensitive: true);
				textToCheck = textToCheck.Replace("Centre", "Center").Replace("centre", "center", caseInsensitive: true);
				textToCheck = replaceWord(textToCheck, "Cheque", "Check", ignoreCase: false, matchWholeWord: true);
				textToCheck = replaceWord(textToCheck, "cheque", "check", ignoreCase: true, matchWholeWord: true);
				textToCheck = textToCheck.Replace("Colour", "Color").Replace("colour", "color", caseInsensitive: true);
				textToCheck = textToCheck.Replace("Organisation", "Organization").Replace("organisation", "organization", caseInsensitive: true);
				textToCheck = textToCheck.Replace("Customise", "Customize").Replace("customise", "customize", caseInsensitive: true);
			}
			else
			{
				textToCheck = textToCheck.Replace("Labor", "Labour").Replace("labor", "labour", caseInsensitive: true);
				textToCheck = textToCheck.Replace("Center", "Centre").Replace("center", "centre", caseInsensitive: true);
				textToCheck = replaceWord(textToCheck, "Check", "Cheque", ignoreCase: false, matchWholeWord: true);
				textToCheck = replaceWord(textToCheck, "check", "cheque", ignoreCase: true, matchWholeWord: true);
				textToCheck = textToCheck.Replace("Color", "Colour").Replace("color", "colour", caseInsensitive: true);
				textToCheck = textToCheck.Replace("Organization", "Organisation").Replace("organization", "organisation", caseInsensitive: true);
				textToCheck = textToCheck.Replace("Customize", "Customise").Replace("customize", "customise", caseInsensitive: true);
			}
		}
		else
		{
			textToCheck = string.Empty;
		}
		return textToCheck;
	}

	private string replaceWord(string oldText, string sourceText, string destText, bool ignoreCase, bool matchWholeWord)
	{
		int length = sourceText.Length;
		string text = destText.Substring(0, 1);
		string text2 = destText.Substring(1);
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		int num = 0;
		for (num = oldText.IndexOf(sourceText, StringComparison.CurrentCultureIgnoreCase); num != -1; num = oldText.IndexOf(sourceText, num + sourceText.Length, StringComparison.CurrentCultureIgnoreCase))
		{
			empty = oldText.Substring(0, num);
			string text3 = oldText.Substring(num);
			empty4 = text3.Substring(0, length);
			empty2 = text3.Substring(length);
			if ((matchWholeWord && (empty2.Length == 0 || empty2.StartsWith(" "))) || !matchWholeWord)
			{
				if (!ignoreCase)
				{
					empty3 = ((!(text3.Substring(0, 1) == empty4.Substring(0, 1).ToUpper())) ? text.ToLower() : text.ToUpper());
					empty3 = ((!(empty4.Substring(1, 1) == empty4.Substring(1, 1).ToUpper())) ? (empty3 + text2.ToLower()) : (empty3 + text2.ToUpper()));
				}
				else
				{
					empty3 = destText;
				}
				oldText = empty + empty3 + empty2;
			}
		}
		return oldText;
	}

	public Hashtable GetLanguagesInFolder()
	{
		string empty = string.Empty;
		Hashtable hashtable = new Hashtable();
		string empty2 = string.Empty;
		string[] files = Directory.GetFiles(currentContext.Server.Location + "DataDict\\", "DDLang*.xml");
		foreach (string text in files)
		{
			empty = Path.GetFileNameWithoutExtension(text);
			empty2 = text.Replace(".xml", "");
			DataSet dataSet = new DataSet();
			dataSet.ReadXml(text, XmlReadMode.Auto);
			if (dataSet.Tables.Contains(empty))
			{
				DataRow[] array = dataSet.Tables[empty].Select("dnID = 'MISCLANGUAGEDESCRIPTION'");
				if (array.Length != 0)
				{
					empty2 = array[0].Field<string>("dnText").Trim();
				}
				hashtable.Add(empty, empty2);
			}
		}
		return hashtable;
	}

	public Dictionary<string, string> GetLanguages()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
		string empty = string.Empty;
		string empty2 = string.Empty;
		DataTable dataTable = null;
		DataTable dataTable2 = dataDictionary.GetDataTable("exec sp_tables @table_name = 'DDLANG%', @table_type = \"'TABLE'\"");
		if (dataTable2.Rows.Count != 0)
		{
			foreach (DataRow row in dataTable2.Rows)
			{
				empty = row.Field<string>("Table_Name").Trim();
				empty2 = string.Empty;
				dataTable = dataDictionary.GetDataTable("Select Case When dnCustText = '' Then dnText Else dnCustText End As dnText From " + empty + " Where dnID = 'MISCLANGUAGEDESCRIPTION'");
				if (dataTable.Rows.Count != 0)
				{
					empty2 = dataTable.Rows[0].Field<string>("dnText").Trim();
				}
				dictionary.Add(empty, empty2);
			}
		}
		return dictionary;
	}
}
