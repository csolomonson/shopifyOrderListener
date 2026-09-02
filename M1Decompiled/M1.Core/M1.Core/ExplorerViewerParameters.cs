using System;

namespace M1.Core;

public sealed class ExplorerViewerParameters
{
	public string TitleText { get; set; }

	public string TitleToolTipText { get; set; }

	public IServiceProvider Provider { get; set; }

	public ExplorerItem ExplorerItem { get; set; }
}
