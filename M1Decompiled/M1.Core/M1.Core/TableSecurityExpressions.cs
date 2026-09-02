namespace M1.Core;

public class TableSecurityExpressions
{
	public string View;

	public string Edit;

	public string Add;

	public string Delete;

	public string ChangeID;

	public TableSecurityExpressions(string view, string edit, string add, string delete, string changeID)
	{
		View = view;
		Edit = edit;
		Add = add;
		Delete = delete;
		ChangeID = changeID;
	}
}
