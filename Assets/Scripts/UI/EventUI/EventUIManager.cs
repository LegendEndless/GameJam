using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EventUIManager : MonoBehaviour
{
    public static EventUIManager Instance { get; private set; }
    public Image background;
    public Text eventTitle;
    public Text eventDescription;
    public Text eventDescriptionLarger;
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
        background.sprite = Resources.Load<Sprite>("ui_event_2");
        if (RealTimeBuilder.Instance.isBuilding)
        {
            RealTimeBuilder.Instance.ExitBuildingMode();
        }
        Time.timeScale = 0;
        // 设置UI元素
        eventTitle.text = plotInfo.title;
        // eventDescription.text = plotInfo.description;
        // eventImage.sprite = Resources.Load<Sprite>(plotInfo.picturePath); // 如果有图片路径
        eventDescription.gameObject.SetActive(false);
        eventImage.gameObject.SetActive(false);
        eventDescriptionLarger.gameObject.SetActive(true);
        eventDescriptionLarger.text = plotInfo.description;

        // 显示UI
        gameObject.SetActive(true);

        // 隐藏不必要的按钮
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
        choiceButton3.gameObject.SetActive(true);
        choiceButton3.GetComponentInChildren<Text>().text = plotInfo.option;
        choiceButton3.onClick.AddListener(() =>
        {
            // 隐藏UI
            gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        });
        choiceButton3.interactable = true;
    }

    public void ShowRegularEvent(RegularEventInfo eventInfo)
    {
        background.sprite = Resources.Load<Sprite>("ui_event_1");
        if (RealTimeBuilder.Instance.isBuilding)
        {
            RealTimeBuilder.Instance.ExitBuildingMode();
        }
        Time.timeScale = 0;
        // 设置UI元素
        eventTitle.text = eventInfo.title;
        eventDescription.text = eventInfo.description;
        // eventImage.sprite = Resources.Load<Sprite>(eventInfo.picturePath);
        eventDescription.gameObject.SetActive(true);
        eventImage.gameObject.SetActive(true);
        eventDescriptionLarger.gameObject.SetActive(false);

        // 设置按钮文本和点击事件
        SetButton(choiceButton1, eventInfo.option1, eventInfo.effect1);
        SetButton(choiceButton2, eventInfo.option2, eventInfo.effect2);

        if (eventInfo.option3 != "" && eventInfo.effect3 != "")
        {
            SetButton(choiceButton3, eventInfo.option3, eventInfo.effect3);
        }
        else
        {
            choiceButton3.gameObject.SetActive(false);
        }
        if (!EventManager.Instance.CanChoose(eventInfo.effect1))
        {
            choiceButton1.interactable = false;
        }
        else
        {
            choiceButton1.interactable = true;
        }
        if (!EventManager.Instance.CanChoose(eventInfo.effect2))
        {
            choiceButton2.interactable = false;
        }
        else
        {
            choiceButton2.interactable = true;
        }
        if (!EventManager.Instance.CanChoose(eventInfo.effect3) || LivabilityManager.Instance.livability < eventInfo.unlock)
        {
            choiceButton3.interactable = false;
        }
        else
        {
            choiceButton3.interactable = true;
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

        Time.timeScale = 1.0f;
        // 隐藏UI
        gameObject.SetActive(false);
    }
}