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
        // ����UI��ֱ���¼�����
        gameObject.SetActive(false);
    }

    public void ShowEvent(RegularEventInfo eventInfo)
    {
        currentEvent = eventInfo;

        // ����UIԪ��
        eventTitle.text = eventInfo.title;
        eventDescription.text = eventInfo.description;
        eventImage.sprite = Resources.Load<Sprite>(eventInfo.picturePath);

        // ���ð�ť�ı��͵���¼�
        SetButton(choiceButton1, eventInfo.option1, eventInfo.effect1);
        SetButton(choiceButton2, eventInfo.option2, eventInfo.effect2);

        // ����˾Ӷ�����
        if (LivabilityManager.Instance.livability >= eventInfo.livabilityForChoice3)
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
        // ִ���¼�Ч��
        EventManager.Instance.AddResource(effect);

        // ����UI
        gameObject.SetActive(false);
    }
}