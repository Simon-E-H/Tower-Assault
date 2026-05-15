using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    [Header("Menus")]
    public GameObject buildMenuPanel;

    [Header("Available Towers")]
    public TowerData[] availableTowers;

    [Header("Prefabs")]
    public GameObject mainCanvas;
    public GameObject turretHealthBarPrefab;
    [Header("Upgrade Menu")]
    public GameObject upgradeMenuPanel;

    private Node selectedNode;

    private Turret selectedTurret;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenBuildMenu(Node node)
    {
        CloseUpgradeMenu();
        selectedNode = node;
        buildMenuPanel.SetActive(true);
    }

    public void CloseBuildMenu()
    {
        buildMenuPanel.SetActive(false);
        selectedNode = null;
    }

    public void SelectTowerToBuild(int towerIndex)
    {
        if (selectedNode == null || towerIndex < 0 || towerIndex >= availableTowers.Length)
            return;

        TowerData data = availableTowers[towerIndex];

        if (PlayerStats.Instance.money < data.cost)
        {
            return;
        }

        PlayerStats.Instance.SpendMoney(data.cost);

        Vector3 spawnPos = selectedNode.transform.position;
        spawnPos.y += 1.0f;

        GameObject newTower = Instantiate(data.towerPrefab, spawnPos, Quaternion.identity);

        newTower.transform.rotation = Quaternion.Euler(90,0,0);

        Turret turretScript = newTower.GetComponent<Turret>();
        if (turretScript != null)
        {
            turretScript.Initialize(data, selectedNode);
        }

        CloseBuildMenu();

        UIManager.Instance.UpdateMoneyUI();
    }

    public void FindUIReferences()
    {
        GameObject canvasGO = GameObject.Find("Canvas");
        if (canvasGO != null)
        {
            mainCanvas = canvasGO;


            buildMenuPanel = canvasGO.transform.Find("BuildMenu")?.gameObject;
            upgradeMenuPanel = canvasGO.transform.Find("UpgradeMenuPanel")?.gameObject;
        }
        else
        {
            Debug.LogError("no cavas");
        }
    }

    public void OpenUpgradeMenu(Turret turret, Node node)
    {
        CloseBuildMenu();
        selectedTurret = turret;
        selectedNode = node;

        if (upgradeMenuPanel == null)
        {
            Debug.LogError("no upgrade assigned to buildmanger");
            return;
        }

        upgradeMenuPanel.SetActive(true);
        buildMenuPanel.SetActive(false);

        UpgradeMenu menu = upgradeMenuPanel.GetComponent<UpgradeMenu>();
        if (menu != null)
        {
            menu.ShowUpgrades(turret);
        }
        else
        {
            Debug.LogError("upgrademenu hasnt got script");
        }
    }

    public void CloseUpgradeMenu()
    {
        if (upgradeMenuPanel != null)
        {
            upgradeMenuPanel.SetActive(false);
        }

        selectedTurret = null;
        selectedNode = null;
    }

    public void OnTurretDestroyed(Turret destroyedTurret)
    {
        if (selectedTurret = destroyedTurret)
        {
            CloseUpgradeMenu();
        }
    }
}