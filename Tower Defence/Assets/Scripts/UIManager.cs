using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Money UI")]
    public TextMeshProUGUI moneyText;

    [Header("Lives UI")]
    public TextMeshProUGUI livesText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateMoneyUI();
        UpdateLivesUI();
    }

    public void FindUIReferences()
    {
        GameObject moneyGO = GameObject.Find("MoneyText");
        if (moneyGO != null)
        {
            moneyText = moneyGO.GetComponent<TextMeshProUGUI>();
        }
        GameObject livesGO = GameObject.Find("LivesText");
        if (livesGO != null)
        {
            livesText = livesGO.GetComponent<TextMeshProUGUI>();
        }

        UpdateMoneyUI();
        UpdateLivesUI();
    }

    public void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = "$" + PlayerStats.Instance.money;
    }

    public void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Lives: " + PlayerStats.Instance.lives;
    }

    public void UpdateAllUI()
    {
        UpdateMoneyUI();
        UpdateLivesUI();
    }
}