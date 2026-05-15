using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildMenu : MonoBehaviour
{
    [Header("Build Buttons")]
    public Button[] buildButtons;

    [Header("Optional Cost Texts")]
    public TextMeshProUGUI[] costTexts;

    [Header("Close Button")]
    public Button closeButton;

    private void OnEnable()
    {
        AssignBuildButtons();
    }

    public void AssignBuildButtons()
    {
        if (BuildManager.Instance == null)
        {
            return;
        }

        for (int i = 0; i < buildButtons.Length; i++)
        {
            if (buildButtons[i] == null) continue;

            int index = i;

            buildButtons[i].onClick.RemoveAllListeners();
            buildButtons[i].onClick.AddListener(() =>
            {
                BuildManager.Instance.SelectTowerToBuild(index);
            });

            if (costTexts != null && i < costTexts.Length && i < BuildManager.Instance.availableTowers.Length)
            {
                TowerData data = BuildManager.Instance.availableTowers[i];
                if (costTexts[i] != null && data != null)
                    costTexts[i].text = data.cost + "$";
            }
            if (closeButton != null)
            {
                closeButton.onClick.RemoveAllListeners();
                closeButton.onClick.AddListener(() => BuildManager.Instance.CloseBuildMenu());
            }
        }
    }
}