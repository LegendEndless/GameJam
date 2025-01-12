using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public CanvasGroup logoAndButtonCanvasGroup;
    public float delay = 3.0f; // 延迟时间
    public float fadeDuration = 1.0f; // 淡入持续时间

    void Start()
    {
        logoAndButtonCanvasGroup.alpha = 0; // 初始透明度为0
        StartCoroutine(ShowLogoAndButtons());
    }

    IEnumerator ShowLogoAndButtons()
    {
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            logoAndButtonCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        logoAndButtonCanvasGroup.alpha = 1; // 确保最终完全显示
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene"); // 替换为你的游戏场景名称
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}