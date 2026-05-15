using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        GameObject managers = GameObject.Find("Managers");
        if (managers != null )
        {
            Destroy(managers);
        }

        WaveSpawner.currentLevelIndex = 0;
        SceneManager.LoadScene("Level1");
    }
}