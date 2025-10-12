using System;
using UnityEngine;

[Serializable]
public struct ObjectWorkEntry
{
    [Header("RawItem")]
    public string RawName;
    public int RawId;

    [Header("ToolItem")]
    public string ToolName;
    public int ToolId;

    [Header("CookItem")]
    public EToolType ToolType;
    public string CookName;
    public int CookId;
}