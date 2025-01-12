using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public CanvasGroup logoAndButtonCanvasGroup;
    public float delay = 3.0f; // �ӳ�ʱ��
    public float fadeDuration = 1.0f; // �������ʱ��

    void Start()
    {
        logoAndButtonCanvasGroup.alpha = 0; // ��ʼ͸����Ϊ0
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
        logoAndButtonCanvasGroup.alpha = 1; // ȷ��������ȫ��ʾ
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene"); // �滻Ϊ�����Ϸ��������
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}