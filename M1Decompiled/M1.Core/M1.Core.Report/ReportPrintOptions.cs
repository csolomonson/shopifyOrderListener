using System.Runtime.InteropServices;

namespace M1.Core.Report;

[ComVisible(true)]
public class ReportPrintOptions
{
	public string PrinterName = string.Empty;

	public bool Collate;

	public int Copies = 1;

	public string Tray = string.Empty;

	public int Duplex;

	public int StartPage;

	public int EndPage;

	public bool SuppressRelatedDocumentsPrompt;
}
