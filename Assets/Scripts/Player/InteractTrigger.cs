using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<SpawnedKey>() != null)
        {
            player.OnCollideWithKey(collision.GetComponent<SpawnedKey>());
        }
    }
}
