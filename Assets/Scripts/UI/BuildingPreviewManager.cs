using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BuildingPreviewManager : MonoBehaviour
{
    [System.Serializable]
    public class ButtonMenuPair
    {
        public Button button;
        public GameObject menuUI;
    }

    public List<ButtonMenuPair> buttonMenuPairs = new List<ButtonMenuPair>();

    private void Start()
    {
        foreach (var pair in buttonMenuPairs)
        {
            pair.button.onClick.AddListener(() => ToggleMenu(pair));
            pair.menuUI.SetActive(false);
        }
    }

    private void ToggleMenu(ButtonMenuPair pair)
    {
        bool isActive = pair.menuUI.activeSelf;
        pair.menuUI.SetActive(!isActive);

        // 关闭其他菜单
        foreach (var otherPair in buttonMenuPairs)
        {
            if (otherPair != pair)
            {
                otherPair.menuUI.SetActive(false);
            }
        }
    }
}