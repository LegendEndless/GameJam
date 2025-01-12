using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitPanel : MonoBehaviour
{
    public Button buttonConfirm;
    public Button buttonCancel;
    public static ExitPanel Instance {  get; private set; }
    private void Awake()
    {
        Instance = this;
        buttonConfirm.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        buttonCancel.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
        gameObject.SetActive(false);
    }
}
