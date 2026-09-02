using System.Collections.Generic;

namespace M1.Ax.Erp.JobSplit;

internal class GLJournalInfo
{
	public List<int> GllGLJournalLineID { get; set; } = new List<int>();

	public List<int> GllGLJournalID { get; set; } = new List<int>();

	public List<int> GllSourcePartTransaction { get; set; } = new List<int>();
}
