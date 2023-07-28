using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "BroBlock/Block", order = 1)]
public class Block : ScriptableObject
{
    public string type;
    public float health;
}
