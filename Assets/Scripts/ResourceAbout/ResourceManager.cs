using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public Dictionary<string, int> resources = new Dictionary<string, int>
    {
        {"Electricity", 0},
        {"Minerals", 0},
        {"Food", 0},
        {"Water", 0},
        {"Oil", 0}
    };

    void Awake()
    {
        Instance = this;
    }

    public void AddResource(string type, int amount)
    {
        if (resources.ContainsKey(type))
        {
            resources[type] += amount;
        }
    }

    public int GetResource(string type)
    {
        return resources.ContainsKey(type) ? resources[type] : 0;
    }
}
