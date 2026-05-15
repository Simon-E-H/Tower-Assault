using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int money = 200;
    public int lives = 20;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public void SpendMoney(int amount)
    {
        money -= amount;
    }

    public void LoseLife()
    {
        lives--;
        UIManager.Instance.UpdateLivesUI();
        
        if (lives <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoseScreen");
    }
}