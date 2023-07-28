using UnityEngine;
using Unity.Collections;

[CreateAssetMenu(fileName = "LevelData", menuName = "BroBlock/LevelData")]
public class LevelData : ScriptableObject
{
    public int numLevels;
    public int currentLevel;
    public Level[] levels;
}
