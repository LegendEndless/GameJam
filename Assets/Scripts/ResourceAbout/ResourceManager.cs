using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public Dictionary<string, float> resources = new Dictionary<string, float>
    {
        {"Electricity", 0},
        {"Minerals", 0},
        {"Food", 0},
        {"Water", 0},
        {"Oil", 0},
        {"Chips", 0},
        {"Alloy", 0},
        {"Fibre", 0},
        {"People", 0},
        {"PeopleAvailable", 0},
    };

    void Awake()
    {
        Instance = this;
    }

    public void AddResource(string type, float amount)
    {
        if (resources.ContainsKey(type))
        {
            resources[type] += amount;
        }
    }

    public float GetResourceCount(string type)
    {
        return resources.ContainsKey(type) ? resources[type] : 0;
    }
}
