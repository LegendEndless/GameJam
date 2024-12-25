using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EventUIManager : MonoBehaviour
{
    public static EventUIManager Instance { get; private set; }

    public Text eventTitle;
    public Text eventDescription;
    public Image eventImage;
    public Button choiceButton1;
    public Button choiceButton2;
    public Button choiceButton3;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // 隐藏UI，直到事件触发
        gameObject.SetActive(false);
    }

    public void ShowPlotEvent(PlotEventInfo plotInfo)
    {
        // 设置UI元素
        eventTitle.text = plotInfo.title;
        eventDescription.text = plotInfo.description;
        // eventImage.sprite = Resources.Load<Sprite>(plotInfo.picturePath); // 如果有图片路径

        // 显示UI
        gameObject.SetActive(true);

        // 隐藏不必要的按钮
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
        choiceButton3.gameObject.SetActive(false);
    }

    public void ShowRegularEvent(RegularEventInfo eventInfo)
    {
        // 设置UI元素
        eventTitle.text = eventInfo.title;
        eventDescription.text = eventInfo.description;
        // eventImage.sprite = Resources.Load<Sprite>(eventInfo.picturePath);

        // 设置按钮文本和点击事件
        SetButton(choiceButton1, eventInfo.option1, eventInfo.effect1);
        SetButton(choiceButton2, eventInfo.option2, eventInfo.effect2);

        if (eventInfo.option3 != null && eventInfo.effect3 != null)
        {
            SetButton(choiceButton3, eventInfo.option3, eventInfo.effect3);
        }
        else
        {
            choiceButton3.gameObject.SetActive(false);
        }

        // 显示UI
        gameObject.SetActive(true);
    }

    private void SetButton(Button button, string choiceText, string effect)
    {
        button.GetComponentInChildren<Text>().text = choiceText;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnChoiceSelected(effect));
        button.gameObject.SetActive(true);
    }

    private void OnChoiceSelected(string effect)
    {
        // 执行事件
        List<UnityAction> actions = EventManager.Instance.TranslateEffect(effect);
        foreach (var action in actions)
        {
            action.Invoke();
        }

        // 隐藏UI
        gameObject.SetActive(false);
    }
}