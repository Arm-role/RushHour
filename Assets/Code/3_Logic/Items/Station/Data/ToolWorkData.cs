using System;

[Serializable]
public class ToolWorkData : StationDataComponent
{
    public bool IsWorking = false;
    public bool IsCancel = false;

    public float ActionCount = 0;
    public float RequiredActions = 0;
    public Item SourceItem = null;
    public Item ResultItem = null;

    public void Reset()
    {
        IsWorking = false;
        IsCancel = false;

        ActionCount = 0;
        RequiredActions = 0;
        SourceItem = null;
        ResultItem = null;
    }

    public override void DebugListeners()
    {
    }
}