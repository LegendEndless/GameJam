using UnityEngine;
using UnityEngine.UI;

public class SunTimeDisplay : MonoBehaviour
{
    public Text timeText;
    public Text dayTimeText;
    private float gameTime;

    void Update()
    {
        float timeLeft = SolarStormManager.Instance.TimeLeft;

        if (timeLeft < 0)
        {
            timeText.text = "00:00";
            return;
        }

        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // ��Ϸ��1�� = ��ʵ��10��
        gameTime += Time.deltaTime / 10.0f;

        // ����Ϸʱ��ת��Ϊ����
        int days = Mathf.FloorToInt(gameTime);

        // ����UI��ʾ
        dayTimeText.text = days.ToString();
    }
}
