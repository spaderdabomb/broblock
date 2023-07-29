using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedBlock : MonoBehaviour
{
    [SerializeField] Block blockSO;
    [field: SerializeField] public Block BlockInstance { get; private set; }

    SpriteRenderer blockSpriteRenderer;

    private void Awake()
    {
        blockSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        BlockInstance = ScriptableObject.Instantiate(blockSO);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DamageBlock(float damageValue)
    {
        BlockInstance.health -= damageValue;

        float healthRatio = BlockInstance.health / blockSO.health;
        Color spriteColor = blockSpriteRenderer.color;
        spriteColor.a = healthRatio;
        blockSpriteRenderer.color = spriteColor;

        if (BlockInstance.health <= 0)
        {
            DestroyBlock();
        }
    }

    public void DestroyBlock()
    {
        Destroy(gameObject);
    }
}
