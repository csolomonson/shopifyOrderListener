namespace M1.Core;

public interface IExplorerViewer
{
	void ExplorerViewerLoad(ExplorerViewerParameters parms);

	void ExplorerViewerUnload();

	void ExplorerViewerRefresh();

	void ExplorerViewerCustomize();
}
