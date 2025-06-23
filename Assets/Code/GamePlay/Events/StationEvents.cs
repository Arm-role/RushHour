using System;
using System.Collections;
using UnityEngine;

public class StationEvents
{
    public WorkPogress OnWorkPogress { get; set; } = new();
    public WorkStarted OnWorkStarted { get; set; } = new();
    public WorkCompleted OnWorkCompleted { get; set; } = new();

}

public sealed class WorkPogress : EventBase<Station> { }
public sealed class WorkStarted : EventBase<Station> { }
public sealed class WorkCompleted : EventBase<Station> { }