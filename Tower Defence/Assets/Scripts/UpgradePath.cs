using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Path", menuName = "Tower Defense/Upgrade Path")]
public class UpgradePath : ScriptableObject
{
    public string pathName = "Default";

    [Header("Upgrades")]
    public UpgradeStep[] upgrades;
}

[System.Serializable]
public class UpgradeStep
{
    public string upgradeName = "Larger Range";
    public string description = "+30% Range";

    [Header("Stats")]
    public float rangeMultiplier = 1f;
    public float damageMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public float healthMultiplier = 1f;

    [Header("Special")]
    public bool ignoresTaunt = false;
    public bool hasRegeneration = false;
    public float regenAmount = 0f;

    [Header("Cost")]
    public int cost = 100;
}