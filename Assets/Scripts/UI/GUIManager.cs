using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoSingleton<GUIManager>
{
    public List<GameObject> panels; //面板资源

    public Stack<GameObject> panelStack = new Stack<GameObject>(); //通过栈来管理各个UI面板的显示顺序

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
            if (panelStack.Peek() != null)  //可以出栈
            {
                currentPanelObj.SetActive(false);
                panelStack.Pop();
            }
            panelStack.Push(panels[id]); //入栈
            currentPanelObj.SetActive(true);

        }
        else
        {
            panelStack.Push(panels[id]);
            currentPanelObj.SetActive(true);
        }
    }
    //返回前一页
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
