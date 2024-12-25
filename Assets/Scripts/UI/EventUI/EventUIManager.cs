using UnityEngine;
using UnityEngine.UI;

public class EventUIManager : MonoBehaviour
{
    public Text eventTitle;
    public Text eventDescription;
    public Image eventImage;
    public Button choiceButton1;
    public Button choiceButton2;
    public Button choiceButton3;

    private RegularEventInfo currentEvent;

    void Start()
    {
        // 隐藏UI，直到事件触发
        gameObject.SetActive(false);
    }

    public void ShowEvent(RegularEventInfo eventInfo)
    {
        currentEvent = eventInfo;

        // 设置UI元素
        eventTitle.text = eventInfo.title;
        eventDescription.text = eventInfo.description;
        eventImage.sprite = Resources.Load<Sprite>(eventInfo.picturePath);

        // 设置按钮文本和点击事件
        SetButton(choiceButton1, eventInfo.option1, eventInfo.effect1);
        SetButton(choiceButton2, eventInfo.option2, eventInfo.effect2);

        // 检查宜居度条件
        if (LivabilityManager.Instance.livability >= eventInfo.livabilityForChoice3)
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
        // 执行事件效果
        EventManager.Instance.AddResource(effect);

        // 隐藏UI
        gameObject.SetActive(false);
    }
}