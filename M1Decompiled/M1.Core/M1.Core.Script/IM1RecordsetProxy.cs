using System.Data;
using System.Runtime.InteropServices;

namespace M1.Core.Script;

[ComVisible(true)]
public interface IM1RecordsetProxy
{
	M1AdoRecordsetFieldsProxy FieldsCollection
	{
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
	}

	bool EOF { get; }

	bool BOF { get; }

	int RecordCount { get; }

	DataRow CurrentDataRow { get; }

	int Bookmark { get; set; }

	string Sort { get; set; }

	string Filter { get; set; }

	object Value { get; }

	void Open(string query, object connection, int cursorType = 3, int lockType = 1, int options = 0, object transaction = null);

	M1AdoFieldProxy Fields(string name);

	void Close();

	void UpdateBatch(string primaryTable = "");

	void AddNew();

	void MoveFirst();

	void MoveNext();

	void MovePrevious();

	void MoveLast();

	string FormatDateForSelect(object date);

	M1AdoRecordsetProxy Select(string filterExpression, string sort = "");

	void Requery();

	void Find(string findCriteria, int skipRows = 0, int direction = 1, int startPosition = 0);

	M1AdoRecordsetProxy Rows(int rowNumber);

	object[,] GetRows(int Rows = -1, int StartRecord = 0, object Fields = null);
}
