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

        // 游戏中1天 = 现实中10秒
        gameTime += Time.deltaTime / 10.0f;

        // 将游戏时间转换为天数
        int days = Mathf.FloorToInt(gameTime);

        // 更新UI显示
        dayTimeText.text = days.ToString();
    }
}
