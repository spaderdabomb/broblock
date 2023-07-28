using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "BroBlock/Level")]
public class Level : ScriptableObject
{
    public int numBigPlatforms;
    public int gateDensityBigPlatforms;
    public int numSmallPlatforms;
    public int enemyDensity;
    public int lavaSpeed;
}
