using System;

namespace M1.Core;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class DatabaseOptionAttribute : Attribute
{
	private readonly string caption;

	private readonly string parentNode;

	private readonly string id;

	private readonly string helpLink;

	private readonly string tablesUsed;

	private readonly string inRoleCheck;

	public string ParentNode => parentNode;

	public string Caption => caption;

	public string ID => id;

	public string HelpLink => helpLink;

	public string TablesUsed => tablesUsed;

	public string InRoleCheck => inRoleCheck;

	public DatabaseOptionAttribute(string parentNode, string caption, string id, string helpLink, string tablesUsed)
	{
		this.parentNode = parentNode;
		this.caption = caption;
		this.id = id;
		this.helpLink = helpLink;
		this.tablesUsed = tablesUsed;
		inRoleCheck = string.Empty;
	}

	public DatabaseOptionAttribute(string parentNode, string caption, string id, string helpLink, string tablesUsed, string inRoleCheck)
	{
		this.parentNode = parentNode;
		this.caption = caption;
		this.id = id;
		this.helpLink = helpLink;
		this.tablesUsed = tablesUsed;
		this.inRoleCheck = inRoleCheck;
	}
}
