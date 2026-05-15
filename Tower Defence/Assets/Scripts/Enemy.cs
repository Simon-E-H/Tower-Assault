using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Data")]
    public EnemyData data;

    [Header("Visual Effects")]
    public Material shockwaveMat;

    private GameObject currentShockwave;
    private Transform targetWaypoint;
    private int waypointIndex = 0;

    private float currentHealth;
    private float attackCountdown = 0f;

    void Start()
    {
        currentHealth = data.health;
        targetWaypoint = Waypoints.waypoints[0];
    }

    void Update()
    {
        if (targetWaypoint != null)
        {
            Vector3 direction = targetWaypoint.position - transform.position;
            transform.Translate(direction.normalized * data.speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, targetWaypoint.position) <= 0.2f)
            {
                GetNextWaypoint();
            }
        }

        attackCountdown -= Time.deltaTime;
        if (attackCountdown <= 0f)
        {
            AttackNearestTurret();
            attackCountdown = 1f / data.attackRate;
        }
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.waypoints.Length - 1)
        {
            PlayerStats.Instance.LoseLife();
            Die();
        }
        else
        {
            waypointIndex++;
            targetWaypoint = Waypoints.waypoints[waypointIndex];
        }
    }

    void AttackNearestTurret()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
        float shortestDistance = Mathf.Infinity;
        Turret nearestTurret = null;

        foreach (GameObject t in turrets)
        {
            float distance = Vector3.Distance(transform.position, t.transform.position);
            if (distance < shortestDistance && distance <= data.attackRange)
            {
                shortestDistance = distance;
                nearestTurret = t.GetComponent<Turret>();
            }
        }

        if (nearestTurret != null)
        {
            if (data.projectilePrefab != null)
            {
                GameObject proj = Instantiate(data.projectilePrefab,
                                            transform.position + Vector3.up * 1.8f,
                                            Quaternion.identity);

                EnemyProjectile enemyProj = proj.GetComponent<EnemyProjectile>();
                if (enemyProj != null)
                {
                    enemyProj.damage = data.attackDamage;
                    enemyProj.Seek(nearestTurret.transform);
                }
            }
            else
            {
                nearestTurret.TakeDamage(data.attackDamage);
                currentShockwave = CreateShockwaveVisual(transform.position);
            }
        }
    }

    GameObject CreateShockwaveVisual(Vector3 position)
    {
        GameObject shockwave = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        shockwave.transform.position = position + Vector3.up * 0.5f;
        shockwave.transform.localScale = Vector3.one * 0.3f;

        Renderer rend = shockwave.GetComponent<Renderer>();
        
        if (shockwaveMat != null)
        {
            rend.material = shockwaveMat;
        }
        else
        {
            Debug.LogWarning("no mat");
        }

        Destroy(shockwave.GetComponent<Collider>());

        StartCoroutine(ExpandShockwave(shockwave, rend));

        return shockwave;
    }

    IEnumerator ExpandShockwave(GameObject shockwave, Renderer rend)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 startScale = shockwave.transform.localScale;
        Vector3 endScale = Vector3.one * data.shockwaveRange * 2f;

        Color startColor = rend.material.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            shockwave.transform.localScale = Vector3.Lerp(startScale, endScale, t);

            Color color = rend.material.color;
            color.a = Mathf.Lerp(color.a, 0f, t);
            rend.material.color = color;

            yield return null;
        }

        Destroy(shockwave);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (currentShockwave != null)
        {
            Destroy(currentShockwave);
        }

        PlayerStats.Instance.AddMoney(data.reward);
        UIManager.Instance.UpdateMoneyUI();
        WaveSpawner.Instance.OnEnemyDeath();
        Destroy(gameObject);
    }
}