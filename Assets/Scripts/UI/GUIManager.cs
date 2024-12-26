using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoSingleton<GUIManager>
{
    public List<GameObject> panels; //�����Դ

    public Stack<GameObject> panelStack = new Stack<GameObject>(); //ͨ��ջ���������UI������ʾ˳��

    public int currentNumber;

    public GameObject currentPanelObj
    {
        get
        {
            return panelStack.Peek();
        }
    }

    private void OnEnable()
    {
        OpenPanel(currentNumber);
    }

    public void OpenPanel(int id, bool isHidePrevious = false)
    {
        if (isHidePrevious)
        {
            if (panelStack.Peek() != null)  //���Գ�ջ
            {
                currentPanelObj.SetActive(false);
                panelStack.Pop();
            }
            panelStack.Push(panels[id]); //��ջ
            currentPanelObj.SetActive(true);

        }
        else
        {
            panelStack.Push(panels[id]);
            currentPanelObj.SetActive(true);
        }
    }
    //����ǰһҳ
    public void Back()
    {
        if (panelStack.Peek() != null)
        {
            currentPanelObj.SetActive(false);
            panelStack.Pop();
        }
        currentPanelObj.SetActive(true);

    }
}
