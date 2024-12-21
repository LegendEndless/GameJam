using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time> apocalypseStart + apocalypseDuration)
        {
            //设置下一次灾变开始的时间
        }
        if(ApocalypseProgress()==1)
        {
            //损失资源
        }
    }
    //预警进度，0表示未开始预警，1表示正在灾变中
    public float ApocalypseProgress()
    {
        return Mathf.Clamp((time - apocalypseStart + warningDuration) / warningDuration,0,1);
    }
}
