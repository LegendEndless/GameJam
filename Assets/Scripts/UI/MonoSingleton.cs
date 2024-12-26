using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour   //����Լ������Ϊ�����ֻ࣬������ʵ����
    where T : MonoBehaviour    //T�̳�
{
    static T m_instance;

    public static T Instance { get => m_instance; }

    protected virtual void Awake()
    {
        m_instance = this as T;
    }
}
