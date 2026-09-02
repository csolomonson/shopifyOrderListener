using System;

namespace M1.Core;

public interface IRunReport
{
	void RunReport(IServiceProvider provider, string reportName);
}
