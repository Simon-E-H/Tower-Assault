using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    

    void Start()
    {
        InvokeRepeating("EnemySpawn", 0f, 5f);
    }

    void EnemySpawn()
    {
        GameObject enemy = (GameObject)Instantiate(enemyPrefab, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
