using System;
using System.Drawing;

namespace M1.Core;

public sealed class ExplorerItemProps
{
	public bool ExplorerOnly;

	public int Sequence;

	public ExplorerType Type { get; set; }

	public Type ControlType { get; set; }

	public Type ViewerType { get; set; }

	public Image Image { get; set; }

	public string Title { get; set; }
}
