using System.Collections.Generic;

public class PlotEventInfo
{
    public string title;
    public string description;
    public string option;
    public float occurrenceTime;
}
public class PlotEventCollection
{
    public List<PlotEventInfo> events;
}

public class RegularEventInfo
{
    public int id;
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