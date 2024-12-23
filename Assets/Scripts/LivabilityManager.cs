using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivabilityManager : MonoBehaviour
{
    public int livability;
    public HashSet<ILivability> livabilityBuildings;
    public static LivabilityManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        Instance = this;
    }
    public void Recalculate()
    {
        livability = 0;
        foreach(var building in livabilityBuildings)
        {
            livability += building.Livability;
        }
        livability -= Mathf.FloorToInt(PopulationManager.Instance.currentPopulation / 15);
        livability = Mathf.Clamp(livability, -20, 20);
        BuildingManager.Instance.GloballyRecalculate();
        //最后记得更新给UI
    }
}
