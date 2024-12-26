using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour   //泛型约束，类为抽象类，只让子类实例化
    where T : MonoBehaviour    //T继承
{
    static T m_instance;

    public static T Instance { get => m_instance; }

    protected virtual void Awake()
    {
        m_instance = this as T;
    }
}
