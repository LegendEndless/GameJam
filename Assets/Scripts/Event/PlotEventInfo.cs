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
    public string picturePath;
    public string choice1, choice2, choice3;
    public string effect1, effect2, effect3;
    public int livabilityForChoice3;
}
public class RegularEventCollection
{
    public List<RegularEventInfo> events;
}