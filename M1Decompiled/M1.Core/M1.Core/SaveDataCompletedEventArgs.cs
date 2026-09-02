using System;
using System.Collections.Generic;

namespace M1.Core;

public class SaveDataCompletedEventArgs : EventArgs
{
	public bool UpdateAddedRowsOnly = true;

	public bool UpdateChangedRowsOnly = true;

	public List<TableChangedEventArgs> TableChanges = new List<TableChangedEventArgs>();
}
