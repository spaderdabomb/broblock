using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vegetation", menuName = "BroBlock/Vegetation")]
public class Vegetation : ScriptableObject
{
    public Sprite sprite;
    public int maxSpawns = 2;
    public int minSpawns = 1;
    public float minSpacing = 2f;
    public float maxSpacing = 20f;
    public bool canFlipX = true;
    public int sortOrder = 0;
    public float offsetY = 0f;
    public VegetationType vegetationType = VegetationType.None;
}

public enum VegetationType
{
    None,
    Grass,
    Rock,
    Mushroom,
    Tree,
    Bush,
    Vine
}
