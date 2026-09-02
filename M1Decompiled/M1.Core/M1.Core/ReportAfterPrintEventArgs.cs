using System;
using System.Collections.Generic;
using M1.Core.Report;

namespace M1.Core;

public class ReportAfterPrintEventArgs : EventArgs
{
	public ReportProxy Report;

	public IServiceProvider Provider;

	public string WhereClause;

	public string OutputType;

	public List<string> Files;

	public ReportAfterPrintEventArgs(ReportProxy report, IServiceProvider provider, string whereClause, string outputType)
	{
		Report = report;
		Provider = provider;
		WhereClause = whereClause;
		OutputType = outputType;
		Files = new List<string>();
	}
}
