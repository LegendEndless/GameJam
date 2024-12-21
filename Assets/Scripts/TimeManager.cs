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
            //������һ���ֱ俪ʼ��ʱ��
        }
        if(ApocalypseProgress()==1)
        {
            //��ʧ��Դ
        }
    }
    //Ԥ�����ȣ�0��ʾδ��ʼԤ����1��ʾ�����ֱ���
    public float ApocalypseProgress()
    {
        return Mathf.Clamp((time - apocalypseStart + warningDuration) / warningDuration,0,1);
    }
}
