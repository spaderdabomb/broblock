using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Key", menuName = "BroBlock/Key")]
public class Key : ScriptableObject
{
    [SerializeField] string baseName;
    [SerializeField] string displayName;
    [SerializeField] public KeyType keyType;
    [SerializeField] public GameObject keyPrefab;
    [SerializeField] public GameObject gatePrefab;
    [field: SerializeField] public Color color { get; private set; }
}

public enum KeyType
{
    Green,
    Blue,
    Red,
    Yellow,
    Purple
}
