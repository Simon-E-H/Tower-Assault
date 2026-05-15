using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [Header("Path A")]
    public Button[] pathA_Buttons;
    public TextMeshProUGUI[] pathA_Names;
    public TextMeshProUGUI[] pathA_Descriptions;
    public TextMeshProUGUI[] pathA_CostTexts;

    [Header("Path B")]
    public Button[] pathB_Buttons;
    public TextMeshProUGUI[] pathB_Names;
    public TextMeshProUGUI[] pathB_Descriptions;
    public TextMeshProUGUI[] pathB_CostTexts;

    private Turret currentTurret;

    [Header("Close Button")]
    public Button closeButton;

    public void ShowUpgrades(Turret turret)
    {
        currentTurret = turret;
        if (turret == null)
        {
            gameObject.SetActive(false);
            return;
        }
        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() => BuildManager.Instance.CloseUpgradeMenu());
        }
        UpdatePathUI(turret.data.pathA, pathA_Buttons, pathA_Names, pathA_Descriptions, pathA_CostTexts, 0);
        UpdatePathUI(turret.data.pathB, pathB_Buttons, pathB_Names, pathB_Descriptions, pathB_CostTexts, 1);
    }

    private void UpdatePathUI(UpgradePath path, Button[] buttons, TextMeshProUGUI[] names,
                             TextMeshProUGUI[] descs, TextMeshProUGUI[] costTexts, int pathIndex)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i >= path.upgrades.Length)
            {
                buttons[i].gameObject.SetActive(false);
                continue;
            }

            UpgradeStep step = path.upgrades[i];

            names[i].text = step.upgradeName;
            descs[i].text = step.description;

            if (costTexts[i] != null)
            {
                costTexts[i].text = step.cost + "$";
            }

            bool isNextLevel = (i == currentTurret.UpgradeLevel);
            bool correctPath = (currentTurret.UpgradeLevel == 0) || (currentTurret.CurrentPath == path);

            buttons[i].interactable = isNextLevel && correctPath &&
                                     (PlayerStats.Instance.money >= step.cost);

            int upgradeIndex = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => UpgradeClicked(path, upgradeIndex));
        }
    }

    public void UpgradeClicked(UpgradePath path, int level)
    {
        if (currentTurret == null) return;

        UpgradeStep step = path.upgrades[level];

        if (PlayerStats.Instance.money >= step.cost)
        {
            PlayerStats.Instance.SpendMoney(step.cost);
            UIManager.Instance.UpdateMoneyUI();
            if (currentTurret.Upgrade(path))
            {
                ShowUpgrades(currentTurret);
            }
        }
    }
}