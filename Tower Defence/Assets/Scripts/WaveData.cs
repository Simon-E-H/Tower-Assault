using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Tower Defense/Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("Wave Settings")]
    public string waveName = "Wave 1";

    [Header("Enemies in this wave")]
    public EnemySpawnInfo[] enemies;

    [Header("Timing")]
    public float timeBetweenEnemies = 0.8f;
    public float timeBeforeNextWave = 8f;
}

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public int count = 10;
}