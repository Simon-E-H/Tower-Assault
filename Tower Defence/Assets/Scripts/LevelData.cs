using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Tower Defense/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName = "Level 1";
    public string sceneName = "Level1";

    public WaveData[] waves;

    public int startingMoneyBonus = 150;
    public int levelCompleteBonus = 400;
}