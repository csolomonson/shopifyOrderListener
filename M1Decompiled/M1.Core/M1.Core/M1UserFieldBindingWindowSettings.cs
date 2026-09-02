namespace M1.Core;

public class M1UserFieldBindingWindowSettings
{
	public bool FilterFieldsOnDrag = true;

	public FieldBindingLinkFieldTypeFilter CurrentFieldTypeFilter;

	public FieldBindingLinkTypeFilter CurrentLinkTypeFilter;

	public void LoadDefaults()
	{
		FilterFieldsOnDrag = true;
		CurrentFieldTypeFilter = FieldBindingLinkFieldTypeFilter.ShowAll;
		CurrentLinkTypeFilter = FieldBindingLinkTypeFilter.ShowAll;
	}
}
