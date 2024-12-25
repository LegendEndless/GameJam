using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotEventInfo
{
    public string title;
    public string description;
    public string picturePath;
    public string effect;
    public float occurrenceTime;
    internal string[] choices;
    internal Sprite image;
}
public class PlotEventCollection
{
    public List<PlotEventInfo> events;
}

public class RegularEventInfo
{
    public string title;
    public string description;
    public string option1, option2, option3;
    public string effect1, effect2, effect3;
    public int unlock;
}
public class RegularEventCollection
{
    public List<RegularEventInfo> events;
}