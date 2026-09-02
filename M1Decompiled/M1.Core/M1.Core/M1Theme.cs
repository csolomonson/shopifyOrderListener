using System;
using System.Drawing;

namespace M1.Core;

public class M1Theme
{
	public string Name = "LightGray";

	public Color LightColor;

	public Color MidColor;

	public Color DarkColor;

	public M1Theme()
		: this("LightGray")
	{
	}

	public M1Theme(string name)
	{
		LoadTheme(name);
	}

	public void LoadTheme(string name)
	{
		Name = name;
		if (name.Equals("Blue", StringComparison.CurrentCultureIgnoreCase))
		{
			LightColor = Color.FromArgb(191, 219, 255);
			MidColor = Color.FromArgb(164, 195, 238);
			DarkColor = Color.FromArgb(101, 147, 207);
		}
		else if (name.Equals("White", StringComparison.CurrentCultureIgnoreCase))
		{
			LightColor = Color.FromArgb(255, 255, 255);
			MidColor = Color.FromArgb(255, 255, 255);
			DarkColor = Color.FromArgb(173, 173, 173);
		}
		else if (name.Equals("DarkGray", StringComparison.CurrentCultureIgnoreCase))
		{
			LightColor = Color.FromArgb(222, 222, 222);
			MidColor = Color.FromArgb(247, 247, 247);
			DarkColor = Color.FromArgb(173, 173, 173);
		}
		else
		{
			LightColor = Color.FromArgb(247, 247, 247);
			MidColor = Color.FromArgb(247, 247, 247);
			DarkColor = Color.FromArgb(173, 173, 173);
		}
	}
}
