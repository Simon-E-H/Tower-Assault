using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Tower Defense/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName = "Minion";

    [Header("Stats")]
    public float health = 100f;
    public float speed = 10f;
    public int reward = 20;

    [Header("Attack")]
    public float attackDamage = 10f;
    public float attackRange = 8f;
    public float attackRate = 1f;
    public GameObject projectilePrefab;

    [Header("Special")]
    public bool hasTaunt = false;
    public float shockwaveDamage = 0f;
    public float shockwaveRange = 3f;
}