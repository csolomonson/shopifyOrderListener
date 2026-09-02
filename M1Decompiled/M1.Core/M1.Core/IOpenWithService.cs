namespace M1.Core;

public interface IOpenWithService
{
	void RunOpenWith(OpenWithDefinition openWith, OpenWithParameters parms);

	void RunOpenWith(OpenWithToolTypeEnum toolType, OpenWithParameters parms);
}
