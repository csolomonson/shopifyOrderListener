using System;

namespace M1.Core;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class UserOptionAttribute : Attribute
{
	private readonly string caption;

	private readonly string id;

	private readonly string helpLink;

	private readonly string inRoleCheck;

	private readonly string image;

	public string Caption => caption;

	public string ID => id;

	public string HelpLink => helpLink;

	public string Image => image;

	public string InRoleCheck => inRoleCheck;

	public UserOptionAttribute(string caption, string id, string helpLink, string image)
	{
		this.caption = caption;
		this.id = id;
		this.helpLink = helpLink;
		this.image = image;
		inRoleCheck = string.Empty;
	}

	public UserOptionAttribute(string caption, string id, string helpLink, string image, string inRoleCheck)
	{
		this.caption = caption;
		this.id = id;
		this.helpLink = helpLink;
		this.image = image;
		this.inRoleCheck = inRoleCheck;
	}
}
