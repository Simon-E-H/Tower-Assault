using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Tower Defense/Tower Data")]
public class TowerData : ScriptableObject
{
    public string towerName = "New Tower";

    [Header("Prefab & Bullet")]
    public GameObject towerPrefab;
    public GameObject bulletPrefab;

    [Header("Upgrade System")]
    public UpgradePath pathA;
    public UpgradePath pathB;

    public int maxUpgrades = 3;

    [Header("Stats")]
    public int cost = 50;
    public float range = 10f;
    public float damage = 20f;
    public float fireRate = 1f;
    public float maxHealth = 100f;
    public float splashRadius = 0f;
    public float splashDamageMultiplier = 0.6f;

    [Header("Visuals")]
    public Sprite icon;
}