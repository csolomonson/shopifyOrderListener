using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace M1.Core;

[ComVisible(true)]
public interface IOpenWithParameters
{
	List<OpenWithDBInfo> Databases { get; set; }

	string TopLevelTable { get; set; }

	string RelatedTable { get; set; }

	string ViewMode { get; set; }

	string CurrentField { get; set; }

	int hWnd { get; set; }

	IntPtr Handle { get; set; }

	M1BindingSource BindingSource { get; set; }

	bool RefreshEnabled { get; set; }

	bool SaveData { get; set; }
}
