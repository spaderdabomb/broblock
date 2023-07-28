using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedKey : MonoBehaviour
{
    [SerializeField] public Key keySO;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = keySO.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
