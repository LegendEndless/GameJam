using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public static PopulationManager Instance
    {
        get; private set;
    }
    public float currentPopulation;
    public float maxPopulation;
    public float rate;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentPopulation = 60;
        maxPopulation = 100;
        rate = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PopulationIncrease()
    {
        currentPopulation += rate * currentPopulation * (1 - currentPopulation / maxPopulation);
    }

}
