using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    static TimeManager instance;
    public static TimeManager Instance => instance;
    public float time;
    public float apocalypseStart = 100;
    public float apocalypseDuration = 30;
    public float warningDuration = 30;
    public int Days
    {
        get
        {
            return (int)(time / 10);
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time> apocalypseStart + apocalypseDuration)
        {
            //设置下一次灾变开始的时间
        }
        
    }
    
}
