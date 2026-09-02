using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using CrystalDecisions.CrystalReports.Engine;

namespace M1.Core.Report;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDispatch)]
public class ReportProxy : IDisposable
{
	public Dictionary<string, string> Settings;

	private string _ReportComments = string.Empty;

	private string _ReportName = string.Empty;

	private string _ReportBaseName = string.Empty;

	private string _ReportFolder = string.Empty;

	public string ReportTitle = string.Empty;

	private string _HelpLink = string.Empty;

	public string OnPrintCommand = string.Empty;

	public string OnRunCommand = string.Empty;

	public ReportEmailOptions EmailOptions = new ReportEmailOptions();

	public ReportPrintOptions PrintOptions = new ReportPrintOptions();

	public bool PreviewShowTree;

	public int ZoomLevel = 100;

	public string[] SecurityModules = new string[0];

	private CrystalParameterCollection _Parameters = new CrystalParameterCollection();

	public List<ReportAddressDefinition> Addresses;

	public ReportAddressDefinition AddressDefinition;

	public List<string> Alerts;

	public Dictionary<string, ReportTableSource> TableSources = new Dictionary<string, ReportTableSource>();

	public bool IsVerified;

	public List<SqlExpressionUpdateInfo> SqlExpressionsWithParameters;

	public bool SelectionFormulaNeedsSetting;

	public string OriginalSelectionFormula = string.Empty;

	public int TotalRecordCount = -1;

	public ReportWhere FilterInfo;

	public object[][] AllDocumentKeys;

	public ReportAddress[] ContactGroups;

	public ReportDocument CrystalRefNew;

	public string ReportComments
	{
		get
		{
			return _ReportComments;
		}
		set
		{
			_ReportComments = value;
		}
	}

	public string ReportName => _ReportName;

	public string ReportBaseName => _ReportBaseName;

	public string ReportFolder => _ReportFolder;

	public string HelpLink
	{
		get
		{
			return _HelpLink;
		}
		set
		{
			_HelpLink = value;
		}
	}

	public CrystalParameterCollection Parameters => _Parameters;

	public ReportProxy(string fileName)
	{
		_ReportName = fileName;
		_ReportBaseName = Path.GetFileNameWithoutExtension(fileName);
		string text = Path.GetDirectoryName(_ReportName);
		if (text[text.Length - 1] == Path.DirectorySeparatorChar)
		{
			text = text.Substring(0, text.Length - 1);
		}
		int num = text.LastIndexOf(Path.DirectorySeparatorChar);
		if (num != -1)
		{
			_ReportFolder = text.Substring(num + 1);
		}
	}

	public ReportParameterData ReportFilterParameters(string name)
	{
		return Parameters[name].Data;
	}

	public void EmailContactFieldChanged(object sender, EventArgs e)
	{
		if (sender is ReportParameterData reportParameterData)
		{
			DropDownTextFilter valueListItem = reportParameterData.Fields[0].GetValueListItem(reportParameterData.FieldOptions[0].Values[0][0]);
			if (valueListItem != null)
			{
				EmailOptions.EmailContactField = valueListItem.Value.ToString();
				AddressDefinition = GetAddressDefinition(EmailOptions.EmailContactField);
			}
		}
	}

	public void GroupTypeChanged(object sender, EventArgs e)
	{
		if (!(sender is ReportParameterData reportParameterData))
		{
			return;
		}
		string text = reportParameterData.FieldOptions[0].Values[0][0].ToString();
		if (text.Equals("multiple", StringComparison.CurrentCultureIgnoreCase))
		{
			EmailOptions.MultipleRecordsPerContact = true;
			EmailOptions.MultipleAttachmentsPerEmail = false;
			return;
		}
		EmailOptions.MultipleRecordsPerContact = false;
		if (text.Equals("oneandone", StringComparison.CurrentCultureIgnoreCase))
		{
			EmailOptions.MultipleAttachmentsPerEmail = false;
		}
		else
		{
			EmailOptions.MultipleAttachmentsPerEmail = true;
		}
	}

	public ReportAddressDefinition GetAddressDefinition(string contactField)
	{
		foreach (ReportAddressDefinition address in Addresses)
		{
			if (address.LastContactField.Equals(contactField, StringComparison.CurrentCultureIgnoreCase))
			{
				return address;
			}
		}
		return null;
	}

	public void Dispose()
	{
		if (SqlExpressionsWithParameters != null)
		{
			foreach (SqlExpressionUpdateInfo sqlExpressionsWithParameter in SqlExpressionsWithParameters)
			{
				sqlExpressionsWithParameter.Dispose();
			}
			SqlExpressionsWithParameters.Clear();
			SqlExpressionsWithParameters = null;
		}
		if (CrystalRefNew != null)
		{
			CrystalRefNew.Close();
			CrystalRefNew.Dispose();
			CrystalRefNew = null;
			GC.Collect();
		}
	}
}
