using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public float gridSpacing = 1f;

    [SerializeField] Player player;

    public float GridSize { get; private set; }
    private int interactLayer;
    public LayerMask blockLayerMask;

    private void Awake()
    {
        Instance = this;
    }

    public bool IsBlockPlaceable(Vector2 position)
    {
        bool blockIsPlaceable = true;
        Collider2D col = Physics2D.OverlapBox(position, new Vector2(gridSpacing / 4f, gridSpacing / 4f), 0f, blockLayerMask);

/*        if (col != null)
        {
            print(col.gameObject);

            blockIsPlaceable = false;
        }*/


        if (player.boxCheckCol.bounds.Contains(position))
        {
            print("hit player");

            blockIsPlaceable = false;
        }

        return blockIsPlaceable;
    }
}
