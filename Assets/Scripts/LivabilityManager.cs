using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivabilityManager : MonoBehaviour
{
    public int eventLivability;
    public int livability;
    public HashSet<ILivability> livabilityBuildings;
    public Text livabilityText;
    public RectTransform pointer;
    public static LivabilityManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        Instance = this;
        eventLivability = 0;
        livability = 5;
        livabilityBuildings = new HashSet<ILivability>();
    }
    public void Recalculate()
    {
        livability = 5;
        foreach (var building in livabilityBuildings)
        {
            livability += building.Livability;
        }
        livability -= Mathf.FloorToInt(PopulationManager.Instance.currentPopulation / 15);
        livability = Mathf.Clamp(livability, -20, 20);
        BuildingManager.Instance.GloballyRecalculate();
        //最后记得更新给UI
        livabilityText.text = livability.ToString();
        pointer.eulerAngles = new Vector3(pointer.eulerAngles.x, pointer.eulerAngles.y, -6.5f * livability);
    }
}
