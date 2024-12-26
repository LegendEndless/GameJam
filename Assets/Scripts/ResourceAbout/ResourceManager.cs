using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public Dictionary<string, float> resources = new Dictionary<string, float>
    {
        {"electric", 0},
        {"mine", 0},
        {"food", 0},
        {"water", 0},
        {"oil", 0},
        {"chip", 0},
        {"ti", 0},
        {"carbon", 0},
        {"nuclear_part", 0},
        {"life_part", 0},
        {"shell_part", 0},
        {"chip_part", 0},
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
