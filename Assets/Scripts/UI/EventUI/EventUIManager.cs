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
        // ����UI��ֱ���¼�����
        gameObject.SetActive(false);
    }

    public void ShowPlotEvent(PlotEventInfo plotInfo)
    {
        // ����UIԪ��
        eventTitle.text = plotInfo.title;
        eventDescription.text = plotInfo.description;
        // eventImage.sprite = Resources.Load<Sprite>(plotInfo.picturePath); // �����ͼƬ·��

        // ��ʾUI
        gameObject.SetActive(true);

        // ���ز���Ҫ�İ�ť
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
        choiceButton3.gameObject.SetActive(false);
    }

    public void ShowRegularEvent(RegularEventInfo eventInfo)
    {
        // ����UIԪ��
        eventTitle.text = eventInfo.title;
        eventDescription.text = eventInfo.description;
        // eventImage.sprite = Resources.Load<Sprite>(eventInfo.picturePath);

        // ���ð�ť�ı��͵���¼�
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

        // ��ʾUI
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
        // ִ���¼�
        List<UnityAction> actions = EventManager.Instance.TranslateEffect(effect);
        foreach (var action in actions)
        {
            action.Invoke();
        }

        // ����UI
        gameObject.SetActive(false);
    }
}