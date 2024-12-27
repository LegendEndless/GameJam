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
        // ����UI��ֱ���¼�����
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
        // ����UIԪ��
        eventTitle.text = plotInfo.title;
        // eventDescription.text = plotInfo.description;
        // eventImage.sprite = Resources.Load<Sprite>(plotInfo.picturePath); // �����ͼƬ·��
        eventDescription.gameObject.SetActive(false);
        eventImage.gameObject.SetActive(false);
        eventDescriptionLarger.gameObject.SetActive(true);
        eventDescriptionLarger.text = plotInfo.description;

        // ��ʾUI
        gameObject.SetActive(true);

        // ���ز���Ҫ�İ�ť
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
        choiceButton3.gameObject.SetActive(true);
        choiceButton3.GetComponentInChildren<Text>().text = plotInfo.option;
        choiceButton3.onClick.AddListener(() =>
        {
            // ����UI
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
        // ����UIԪ��
        eventTitle.text = eventInfo.title;
        eventDescription.text = eventInfo.description;
        // eventImage.sprite = Resources.Load<Sprite>(eventInfo.picturePath);
        eventDescription.gameObject.SetActive(true);
        eventImage.gameObject.SetActive(true);
        eventDescriptionLarger.gameObject.SetActive(false);

        // ���ð�ť�ı��͵���¼�
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

        Time.timeScale = 1.0f;
        // ����UI
        gameObject.SetActive(false);
    }
}