using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance;

    [Header("Levels")]
    public LevelData[] levels;

    [Header("References")]
    public Transform spawnPoint;
    public TextMeshProUGUI waveText;

    public static int currentLevelIndex = 0;

    private int enemiesAlive = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadCurrentLevel()
    {
        if (currentLevelIndex >= levels.Length)
        {
            WinGame();
            return;
        }

        LevelData level = levels[currentLevelIndex];
        
        if (!string.IsNullOrEmpty(level.sceneName))
        {
            SceneManager.LoadScene(level.sceneName);
        }
        else
        {
            Debug.LogError("Eeror");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedLevelStart());
    }

    private IEnumerator DelayedLevelStart()
    {
        yield return new WaitForSeconds(0.2f);

        if (currentLevelIndex >= levels.Length)
        {
            WinGame();
            yield break;
        }

        LevelData level = levels[currentLevelIndex];
        if(BuildManager.Instance != null)
        {
            BuildManager.Instance.FindUIReferences();
            BuildManager.Instance.buildMenuPanel.GetComponent<BuildMenu>().AssignBuildButtons();
        }
        if(UIManager.Instance != null)
        {
            UIManager.Instance.FindUIReferences();
        }
        FindReferences();
        enemiesAlive = 0;
        StartCoroutine(SpawnLevel(level));
    }

    private IEnumerator SpawnLevel(LevelData level)
    {
        for (int waveIndex = 0; waveIndex < level.waves.Length; waveIndex++)
        {
            WaveData wave = level.waves[waveIndex];

            foreach (EnemySpawnInfo info in wave.enemies)
            {
                for (int i = 0; i < info.count; i++)
                {
                    SpawnEnemy(info.enemyPrefab);
                    yield return new WaitForSeconds(wave.timeBetweenEnemies);
                }
            }

            yield return new WaitUntil(() => enemiesAlive <= 0);
            yield return new WaitForSeconds(wave.timeBeforeNextWave);
        }

        CompleteLevel();
    }

    void SpawnEnemy(GameObject prefab)
    {
        if (prefab == null) return;
        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        enemiesAlive++;
    }

    public void FindReferences()
    {
        GameObject startGO = GameObject.Find("Start");
        if (startGO != null)
        {
            spawnPoint = startGO.transform;
        }
        else
        {
            Debug.LogError("no spawn");
        }

        GameObject waveTextGO = GameObject.Find("WaveText");
        if (waveTextGO != null)
            waveText = waveTextGO.GetComponent<TextMeshProUGUI>();
    }

    public void OnEnemyDeath()
    {
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
    }

    public void CompleteLevel()
    {
        LevelData completed = levels[currentLevelIndex];
        PlayerStats.Instance.AddMoney(completed.levelCompleteBonus);
        UIManager.Instance.UpdateMoneyUI();
        currentLevelIndex++;
        LoadCurrentLevel();
    }

    private void WinGame()
    {
        SceneManager.LoadScene("EndScreen");
    }
}