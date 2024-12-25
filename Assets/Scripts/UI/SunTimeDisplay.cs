using UnityEngine;
using UnityEngine.UI;

public class SunTimeDisplay : MonoBehaviour
{
    public Text timeText; // 关联到UI Text组件

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
    }
}
