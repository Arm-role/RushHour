using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class ItemWorkService
{
    private AssetProvider<Item> _assetProvider;
    private WorkLibrary _workLibrary;

    public ItemWorkService(AssetProvider<Item> assetProvider, WorkLibrary workLibrary)
    {
        _assetProvider = assetProvider;
        _workLibrary = workLibrary;
    }
    public bool TryGetToolName(string cookItemName, out Task<Item> item)
    {
        string toolName = _workLibrary.GetToolName(cookItemName);
        if (toolName != string.Empty)
        {
            item = _assetProvider.Get(toolName);
            return true;
        }

        item = null;
        return false;
    }
}
