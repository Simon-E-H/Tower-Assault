using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;

    [Header("Stats (from TowerData)")]
    public float range = 10f;
    public float damage = 20f;
    public float fireRate = 1f;

    [Header("Health")]
    public float maxHealth = 100f;
    private float currentHealth;
    public GameObject healthBarPrefab;
    private Slider healthBarSlider;
    private Transform healthBarTransform;

    private Transform target;
    private string enemyTag = "Enemy";

    private float fireCountdown = 0f;
    private GameObject bulletPrefab;

    public TowerData data;

    private Node myNode;

    private UpgradePath currentPath;
    private int upgradeLevel = 0;

    public int UpgradeLevel => upgradeLevel;
    public UpgradePath CurrentPath => currentPath;

    private bool ignoresTaunt;
    [Header("Regeneration")]
    private float regenAmount = 0f;
    private float regenTimer = 0f;

    public void Initialize(TowerData towerData, Node node)
    {
        data = towerData;
        range = towerData.range;
        damage = towerData.damage;
        fireRate = towerData.fireRate;
        bulletPrefab = towerData.bulletPrefab;
        maxHealth = towerData.maxHealth;

        currentHealth = maxHealth;

        gameObject.name = towerData.towerName;

        myNode = node;
        if (myNode != null)
        {
            myNode.Occupy(this);
        }

        CreateHealthBar();
    }

    public bool Upgrade(UpgradePath chosenPath)
    {
        if (upgradeLevel >= data.maxUpgrades) return false;

        if (upgradeLevel == 0)
        {
            currentPath = chosenPath;
        }
        else if (currentPath != chosenPath)
        {
            return false;
        }

        UpgradeStep step = chosenPath.upgrades[upgradeLevel];

        range *= step.rangeMultiplier;
        damage *= step.damageMultiplier;
        fireRate *= step.fireRateMultiplier;
        maxHealth *= step.healthMultiplier;
        currentHealth = maxHealth;
        transform.localScale *= 1.15f;

        if (step.ignoresTaunt)
            ignoresTaunt = true;

        if (step.hasRegeneration)
            regenAmount = step.regenAmount;

        upgradeLevel++;

        VisualUpgradeFeedback();

        return true;
    }

    private void VisualUpgradeFeedback()
    {
        transform.localScale *= 1.15f;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in renderers)
        {
            if (rend.material == null) continue;

            Color original = rend.material.color;

            if (currentPath == null)
            {
                rend.material.color = original;
                continue;
            }

            Color newColor;

            if (currentPath == data.pathA)
            {
                newColor = Color.Lerp(original, new Color(0.4f, 0.5f, 1.0f), 0.1f);
            }
            else
            {
                newColor = Color.Lerp(original, new Color(1.4f, 1.1f, 0.4f), 0.1f);
            }

            rend.material.color = newColor;
        }
    }

    void CreateHealthBar()
    {
        if (healthBarPrefab == null) return;

        GameObject barGO = Instantiate(healthBarPrefab);

        barGO.transform.SetParent(BuildManager.Instance.mainCanvas.transform, false);

        healthBarSlider = barGO.GetComponent<Slider>();
        healthBarTransform = barGO.transform;

        RectTransform rect = barGO.GetComponent<RectTransform>();
        rect.localScale = new Vector3(8f, 1f, 1f);
        rect.sizeDelta = new Vector2(140, 20);

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }
    }

    void LateUpdate()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.transform.LookAt(Camera.main.transform);
            healthBarSlider.transform.Rotate(0, 180, 0);
        }
    }

    void Update()
    {
        UpdateTarget();

        if (fireCountdown > 0f)
            fireCountdown -= Time.deltaTime;

        if (target != null && fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Euler(-90f, lookRot.eulerAngles.y, 0f);
        }

        UpdateRegeneration();
        UpdateHealthBarPosition();
        
    }
    void UpdateHealthBarPosition()
    {
        if (healthBarTransform == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 4.5f);
        healthBarTransform.position = screenPos;
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        Transform bestTarget = null;

        foreach (GameObject enemyGO in enemies)
        {
            Enemy enemy = enemyGO.GetComponent<Enemy>();
            if (enemy == null || enemy.data == null) continue;

            float distance = Vector3.Distance(transform.position, enemyGO.transform.position);

            if (enemy.data.hasTaunt)
            {
                if (distance <= range)
                {
                    target = enemyGO.transform;
                    return;
                }
            }

            if (distance < shortestDistance && distance <= range)
            {
                shortestDistance = distance;
                bestTarget = enemyGO.transform;
            }
        }

        target = bestTarget;
    }

    void Shoot()
    {
        if (firePoint == null || bulletPrefab == null) return;

        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
            bullet.damage = damage;

            if (data != null)
            {
                bullet.splashRadius = data.splashRadius;
                bullet.splashDamage = damage * data.splashDamageMultiplier;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (healthBarSlider != null)
            healthBarSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if(myNode != null)
        {
            myNode.FreeUp();
        }

        if (healthBarTransform != null)
        {
            Destroy(healthBarTransform.gameObject);
        }

        if (BuildManager.Instance != null)
        {
            BuildManager.Instance.OnTurretDestroyed(this);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void UpdateRegeneration()
    {
        if (regenAmount <= 0f)
        {
            return;
        }

        regenTimer += Time.deltaTime;
        if (regenTimer >= 1f)
        {
            currentHealth += regenAmount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            if (healthBarSlider != null)
            {
                healthBarSlider.value = currentHealth;
            }

            regenTimer = 0f;
        }
    }
}